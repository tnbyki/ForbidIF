import cv2
import mediapipe as mp
from mediapipe.tasks.python import vision
import socket
import json
import time
import math
from collections import deque

try:
    import msvcrt
except Exception:
    msvcrt = None

sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)

HEADTRACK_ADDR = ("127.0.0.1", 5005)
BRIDGE_ADDR = ("127.0.0.1", 9999)

FACE_STATE_WINDOW_SEC = 5.0
FACE_STATE_SEND_INTERVAL_SEC = 1.0
NOFACE_IMMEDIATE_SEC = 1.2
WAVE_WINDOW_SEC = 1.2
WAVE_MIN_X_SPAN = 0.16
WAVE_MIN_DIRECTION_CHANGES = 2
WAVE_COOLDOWN_SEC = 2.0
SHOW_PREVIEW = True

CAMERA_WIDTH = 640
CAMERA_HEIGHT = 480
CAMERA_FPS = 30
PROCESS_INTERVAL_SEC = 1.0 / 30.0
HEADTRACK_SEND_INTERVAL_SEC = 1.0 / 30.0
HAND_PROCESS_INTERVAL_SEC = 1.0 / 15.0
LEFT_HAND_Z_LOG_INTERVAL_SEC = 0.5

last_send_log_time = 0.0
last_logged_y = 0.0

cap = cv2.VideoCapture(0)
cap.set(cv2.CAP_PROP_FRAME_WIDTH, CAMERA_WIDTH)
cap.set(cv2.CAP_PROP_FRAME_HEIGHT, CAMERA_HEIGHT)
cap.set(cv2.CAP_PROP_FPS, CAMERA_FPS)
cap.set(cv2.CAP_PROP_BUFFERSIZE, 1)

BaseOptions = mp.tasks.BaseOptions
FaceLandmarker = vision.FaceLandmarker
FaceLandmarkerOptions = vision.FaceLandmarkerOptions
HandLandmarker = vision.HandLandmarker
HandLandmarkerOptions = vision.HandLandmarkerOptions
VisionRunningMode = vision.RunningMode

options = FaceLandmarkerOptions(
    base_options=BaseOptions(model_asset_path='face_landmarker.task'),
    running_mode=VisionRunningMode.VIDEO,
    num_faces=1
)
landmarker = FaceLandmarker.create_from_options(options)

try:
    hand_options = HandLandmarkerOptions(
        base_options=BaseOptions(model_asset_path='hand_landmarker.task'),
        running_mode=VisionRunningMode.VIDEO,
        num_hands=2
    )
    hand_landmarker = HandLandmarker.create_from_options(hand_options)
except Exception as e:
    hand_landmarker = None
    print("HandLandmarker disabled:", e)

smooth_x = smooth_y = smooth_z = 0.0
last_timestamp = 0

face_state_samples = deque()
distance_samples = deque()
last_face_state_send_time = 0.0
noface_started_at = None
last_sent_state = None
hand_motion_samples = deque()
last_wave_event_time = 0.0
last_process_time = 0.0
last_headtrack_send_time = 0.0
last_hand_process_time = 0.0
last_left_hand_z_log_time = 0.0


def get_face_state(nose, face_width):
    if face_width <= 0.03:
        return "NoFace"
    centered_x = abs(nose.x - 0.5) <= 0.22
    centered_y = abs(nose.y - 0.5) <= 0.28
    return "Looking" if centered_x and centered_y else "NoLooking"


def add_face_sample(state, distance, now):
    face_state_samples.append((now, state))
    distance_samples.append((now, distance))
    while face_state_samples and now - face_state_samples[0][0] > FACE_STATE_WINDOW_SEC:
        face_state_samples.popleft()
    while distance_samples and now - distance_samples[0][0] > 1.0:
        distance_samples.popleft()


def build_face_state_payload(now, force_state=None):
    if not face_state_samples:
        state = force_state or "NoFace"
        confidence = 0.0
        looking_ratio = 0.0
        noface_ratio = 1.0
    else:
        counts = {"Looking": 0, "NoLooking": 0, "NoFace": 0}
        for _, sample_state in face_state_samples:
            counts[sample_state] = counts.get(sample_state, 0) + 1
        total = max(1, sum(counts.values()))
        state = force_state or max(counts, key=counts.get)
        confidence = counts.get(state, 0) / total
        looking_ratio = counts.get("Looking", 0) / total
        noface_ratio = counts.get("NoFace", 0) / total

    distance = sum(v for _, v in distance_samples) / len(distance_samples) if distance_samples else 0.0
    return {
        "type": "face_state",
        "state": state,
        "confidence": round(confidence, 3),
        "distance": round(distance, 4),
        "looking_ratio_5s": round(looking_ratio, 3),
        "noface_ratio_5s": round(noface_ratio, 3),
        "timestamp": round(now, 3),
    }


def send_face_state(now, force_state=None, force=False):
    global last_face_state_send_time, last_sent_state
    payload = build_face_state_payload(now, force_state)
    state = payload["state"]

    if not force and now - last_face_state_send_time < FACE_STATE_SEND_INTERVAL_SEC:
        return

    msg = "FACE_STATE|" + json.dumps(payload, ensure_ascii=False)
    sock.sendto(msg.encode("utf-8"), BRIDGE_ADDR)

    last_face_state_send_time = now
    last_sent_state = state


def send_face_event(event, confidence, now):
    payload = {"type": "face_event", "event": event, "confidence": round(confidence, 3), "timestamp": round(now, 3)}
    msg = "FACE_EVENT|" + json.dumps(payload, ensure_ascii=False)
    sock.sendto(msg.encode("utf-8"), BRIDGE_ADDR)


def count_direction_changes(values):
    if len(values) < 4: return 0
    changes = 0
    last_direction = 0
    for prev, current in zip(values, values[1:]):
        diff = current - prev
        if abs(diff) < 0.015: continue
        direction = 1 if diff > 0 else -1
        if last_direction != 0 and direction != last_direction:
            changes += 1
        last_direction = direction
    return changes


def detect_wave(hand_result, now):
    global last_wave_event_time
    if hand_result is None or not hand_result.hand_landmarks:
        hand_motion_samples.clear()
        return False

    wrists = [hand[0] for hand in hand_result.hand_landmarks]
    wrist = max(wrists, key=lambda item: abs(item.x - 0.5))
    wrist_x = wrist.x
    hand_motion_samples.append((now, wrist_x))

    while hand_motion_samples and now - hand_motion_samples[0][0] > WAVE_WINDOW_SEC:
        hand_motion_samples.popleft()

    if now - last_wave_event_time < WAVE_COOLDOWN_SEC:
        return False

    xs = [x for _, x in hand_motion_samples]
    if len(xs) < 6: return False

    x_span = max(xs) - min(xs)
    direction_changes = count_direction_changes(xs)

    if x_span >= WAVE_MIN_X_SPAN and direction_changes >= WAVE_MIN_DIRECTION_CHANGES:
        confidence = min(1.0, (x_span / WAVE_MIN_X_SPAN) * 0.55 + (direction_changes / 4.0) * 0.45)
        send_face_event("Wave", confidence, now)
        last_wave_event_time = now
        hand_motion_samples.clear()
        return True
    return False


def read_control_key(show_preview):
    key = 255
    if show_preview:
        key = cv2.waitKey(1) & 0xFF
    else:
        cv2.waitKey(1)

    if msvcrt is not None:
        while msvcrt.kbhit():
            ch = msvcrt.getwch()
            if ch in ("\x00", "\xe0"):
                if msvcrt.kbhit(): msvcrt.getwch()
                continue
            if ch: key = ord(ch[0])
    return key


def average_landmark(hand_landmarks, indexes):
    count = float(len(indexes))
    x = sum(hand_landmarks[i].x for i in indexes) / count
    y = sum(hand_landmarks[i].y for i in indexes) / count
    z = sum(hand_landmarks[i].z for i in indexes) / count
    return x, y, z


def calculate_wrist_angle(hand_landmarks):
    wrist = hand_landmarks[0]
    index_tip = hand_landmarks[8]
    middle_tip = hand_landmarks[12]
    index_mcp = hand_landmarks[5]
    pinky_mcp = hand_landmarks[17]
    palm_x, palm_y, palm_z = average_landmark(hand_landmarks, [5, 9, 13, 17])

    # 手首から指の付け根群へ向かう「手のひら中心軸」を主に使う。
    # 少しだけ中指先方向を混ぜると、棒の向きとして自然に見えやすい。
    dx = (palm_x - wrist.x) * 0.75 + (middle_tip.x - wrist.x) * 0.25
    dx = (palm_x - wrist.x) * 0.75 + (middle_tip.x - wrist.x) * 0.25
    dy = (palm_y - wrist.y) * 0.75 + (middle_tip.y - wrist.y) * 0.25
    dz = (palm_z - wrist.z) * 0.75 + (middle_tip.z - wrist.z) * 0.25

    if abs(dx) < 0.0001 and abs(dy) < 0.0001:
        dx = index_tip.x - wrist.x
        dy = index_tip.y - wrist.y
        dz = index_tip.z - wrist.z

    angle = math.degrees(math.atan2(-dy, dx))
    return angle, dx, dy, dz, palm_x, palm_y, index_mcp, pinky_mcp


def distance2d(a, b):
    dx = a.x - b.x
    dy = a.y - b.y
    return math.sqrt(dx * dx + dy * dy)


def classify_hand_state(hand_landmarks):
    wrist = hand_landmarks[0]
    index_mcp = hand_landmarks[5]
    middle_mcp = hand_landmarks[9]
    pinky_mcp = hand_landmarks[17]
    palm_x, palm_y, _ = average_landmark(hand_landmarks, [0, 5, 9, 13, 17])

    class Point:
        pass

    palm = Point()
    palm.x = palm_x
    palm.y = palm_y

    palm_scale = max(
        distance2d(wrist, middle_mcp),
        distance2d(index_mcp, pinky_mcp),
        0.001
    )

    tip_indexes = [4, 8, 12, 16, 20]
    avg_tip_distance = sum(
        distance2d(hand_landmarks[i], palm)
        for i in tip_indexes
    ) / float(len(tip_indexes))

    open_ratio = avg_tip_distance / palm_scale

    if open_ratio <= 0.58:
        return "HAND_CLOSED"
    if open_ratio <= 0.95:
        return "HAND_HALF"
    return "HAND_OPEN"


print("H: show/hide preview, ESC: exit")

while True:
    ret, frame = cap.read()
    if not ret: break

    now = time.monotonic()
    if now - last_process_time < PROCESS_INTERVAL_SEC:
        key = read_control_key(SHOW_PREVIEW)
        if key == 27: break
        if key == ord("h") or key == ord("H"):
            SHOW_PREVIEW = not SHOW_PREVIEW
            if not SHOW_PREVIEW:
                try: cv2.destroyWindow("Head Tracker")
                except: pass
                print("Preview hidden. Press H again.")
            else:
                print("Preview shown.")
        time.sleep(0.001)
        continue
    last_process_time = now

    timestamp_ms = int(now * 1000)
    if timestamp_ms <= last_timestamp:
        timestamp_ms = last_timestamp + 1
    last_timestamp = timestamp_ms

    rgb = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
    mp_image = mp.Image(image_format=mp.ImageFormat.SRGB, data=rgb)

    result = landmarker.detect_for_video(mp_image, timestamp_ms)

    if hand_landmarker is not None and now - last_hand_process_time >= HAND_PROCESS_INTERVAL_SEC:
        hand_result = hand_landmarker.detect_for_video(mp_image, timestamp_ms)
        last_hand_process_time = now
    else:
        hand_result = None

    wave_detected = detect_wave(hand_result, now)

    if result.face_landmarks:
        landmarks = result.face_landmarks[0]
        nose = landmarks[1]
        left = landmarks[234]
        right = landmarks[454]

        face_width = abs(right.x - left.x)

        head_x = (nose.x - 0.5) * 2.0
        head_y = (nose.y - 0.5) * 2.0
        head_z = (face_width - 0.25) * 3.0   # 近くても暴れにくい調整

        smooth_x = smooth_x * 0.65 + head_x * 0.35
        smooth_y = smooth_y * 0.60 + head_y * 0.40
        smooth_z = smooth_z * 0.72 + head_z * 0.28

        msg = f"{smooth_x:.4f},{smooth_y:.4f},{smooth_z:.4f}"
        if now - last_headtrack_send_time >= HEADTRACK_SEND_INTERVAL_SEC:
            sock.sendto(msg.encode(), HEADTRACK_ADDR)
            last_headtrack_send_time = now

        state = get_face_state(nose, face_width)
        add_face_sample(state, face_width, now)
        noface_started_at = None
        send_face_state(now)

        text = f"X:{smooth_x:.3f} Y:{smooth_y:.3f} Z:{smooth_z:.3f}"

        h, w, _ = frame.shape
        cv2.circle(frame, (int(nose.x*w), int(nose.y*h)), 5, (0,255,0), -1)

    else:
        add_face_sample("NoFace", 0.0, now)
        if noface_started_at is None:
            noface_started_at = now
        if now - noface_started_at >= NOFACE_IMMEDIATE_SEC:
            send_face_state(now, force_state="NoFace", force=True)
        else:
            send_face_state(now)
        text = "NO FACE"

# ====================== 手のトラッキング ======================
    if hand_result is not None and hand_result.hand_landmarks:
        
        # 左手のZ最小・最大を保持
        if 'left_z_min' not in globals():
            global left_z_min, left_z_max
            left_z_min = 999.0
            left_z_max = -999.0

        for hand_idx, (hand_landmarks, handedness) in enumerate(zip(hand_result.hand_landmarks, hand_result.handedness)):
            
            hand_label = handedness[0].category_name
            side = "LEFT" if hand_label == "Left" else "RIGHT"

            wrist = hand_landmarks[0]
            middle = hand_landmarks[9]
            
            center_x = (wrist.x + middle.x) * 0.5
            center_y = (wrist.y + middle.y) * 0.5
            center_z = (wrist.z * 0.8) + (middle.z * 0.2)
            angle, dir_x, dir_y, dir_z, palm_x, palm_y, index_mcp, pinky_mcp = calculate_wrist_angle(hand_landmarks)
            hand_state = classify_hand_state(hand_landmarks)

            # 送信
            hand_msg = f"{side},{center_x:.4f},{center_y:.4f},{center_z:.4f},{angle:.2f},{dir_x:.5f},{dir_y:.5f},{dir_z:.5f},{hand_state}"
            sock.sendto(hand_msg.encode(), ("127.0.0.1", 5006))

            # プレビュー
            h, w, _ = frame.shape
            color = (0, 255, 255) if side == "LEFT" else (255, 0, 255)
            cv2.circle(frame, (int(center_x * w), int(center_y * h)), 10, color, -1)
            cv2.line(
                frame,
                (int(wrist.x * w), int(wrist.y * h)),
                (int(palm_x * w), int(palm_y * h)),
                color,
                3
            )
            cv2.line(
                frame,
                (int(index_mcp.x * w), int(index_mcp.y * h)),
                (int(pinky_mcp.x * w), int(pinky_mcp.y * h)),
                color,
                2
            )

            # ====================== 左手のZ振れ幅ログ（更新時のみ） ======================
            cv2.putText(
                frame,
                hand_state.replace("HAND_", ""),
                (int(center_x * w) + 12, int(center_y * h)),
                cv2.FONT_HERSHEY_SIMPLEX,
                0.55,
                color,
                2
            )

            if side == "LEFT":
                updated = False
                if center_z < left_z_min:
                    left_z_min = center_z
                    updated = True
                if center_z > left_z_max:
                    left_z_max = center_z
                    updated = True
                
                if updated and now - last_left_hand_z_log_time >= LEFT_HAND_Z_LOG_INTERVAL_SEC:
                    range_val = left_z_max - left_z_min
                    print(f"[LEFT HAND Z UPDATE] rawZ={center_z:.5f} | Min={left_z_min:.5f} | Max={left_z_max:.5f} | Range={range_val:.5f}")
                    last_left_hand_z_log_time = now
    # =========================================================================
    
    if wave_detected:
        cv2.putText(frame, "WAVE", (30, 65), cv2.FONT_HERSHEY_SIMPLEX, 0.8, (0,255,255), 2)

    cv2.putText(frame, text, (30, 30), cv2.FONT_HERSHEY_SIMPLEX, 0.8, (0,255,0), 2)

    if SHOW_PREVIEW:
        cv2.imshow("Head Tracker", frame)

    key = read_control_key(SHOW_PREVIEW)

    if key == 27: break
    if key == ord("h") or key == ord("H"):
        SHOW_PREVIEW = not SHOW_PREVIEW
        if not SHOW_PREVIEW:
            try: cv2.destroyWindow("Head Tracker")
            except: pass
            print("Preview hidden. Press H again.")
        else:
            print("Preview shown.")

cap.release()
if hand_landmarker is not None:
    hand_landmarker.close()
cv2.destroyAllWindows()

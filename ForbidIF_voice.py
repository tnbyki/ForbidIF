# =========================================================
# ForbidIF VOICE Server (VOICEVOX Bridge)
# New Format:
# 🔊VOICE|speaker=ラム|id=2|speed=1.13|volume=1.08|text=セリフ
# =========================================================

import json
import time
import threading
import queue
import uuid
from pathlib import Path

import requests
import pygame
from flask import Flask, request, jsonify

BASE_DIR = Path(__file__).parent
TEMP_DIR = BASE_DIR / "Temp"
TEMP_DIR.mkdir(exist_ok=True)

VOICEVOX_URL = "http://127.0.0.1:50021"

DEFAULT_VOICE_ID = 1
DEFAULT_SPEED = 1.0

# ===============================
# VOICEVOX: (話者名, スタイル名) -> style_id
# ===============================

def get_style_id(speaker_name: str, style_name: str = "ノーマル") -> int:
    speakers = requests.get(f"{VOICEVOX_URL}/speakers", timeout=10).json()
    for sp in speakers:
        if sp.get("name") == speaker_name:
            for st in sp.get("styles", []):
                if st.get("name") == style_name:
                    return int(st.get("id"))
    raise ValueError(f"style_id not found: {speaker_name} / {style_name}")


VOICE_STYLE_SPEC = {
0: ("青山龍星", "ノーマル"),
1: ("白上虎太郎", "ふつう"),
2: ("四国めたん", "あまあま"),
3: ("波音リツ", "ノーマル"),
4: ("春日部つむぎ", "ノーマル"),
5: ("冥鳴ひまり", "ノーマル"),
6: ("九州そら", "ノーマル"),
7: ("春日部つむぎ", "ノーマル"),
}

VOICE_MAP = {}

def build_voice_map():
    global VOICE_MAP
    VOICE_MAP = {}

    for vid, (name, style) in VOICE_STYLE_SPEC.items():
        try:
            VOICE_MAP[vid] = get_style_id(name, style)
        except Exception as e:
            print("VOICE取得失敗:", vid, name, style, e)


# ===============================
# 新VOICEフォーマット解析
# ===============================

def parse_voice_lines(text_block: str):

    results = []

    for line in text_block.splitlines():

        line = line.strip()

        if not line.startswith("🔊VOICE|"):
            continue

        parts = line.split("|")[1:]
        data = {}

        for p in parts:

            if "=" in p:
                k, v = p.split("=", 1)
                data[k] = v

        name = data.get("speaker", "")
        vid = int(data.get("id", DEFAULT_VOICE_ID))
        spd = float(data.get("speed", DEFAULT_SPEED))
        vol = float(data.get("volume", 1.0))
        txt = data.get("text", "")

        if txt:
            results.append((name, txt, vid, spd, vol))

    return results


# ===============================
# VOICEVOX 合成
# ===============================

def synth_wav_bytes(text, speaker, speed, volume):

    q = requests.post(
        f"{VOICEVOX_URL}/audio_query",
        params={"text": text, "speaker": speaker},
        timeout=30,
    ).json()

    q["speedScale"] = speed
    q["volumeScale"] = volume

    wav = requests.post(
        f"{VOICEVOX_URL}/synthesis",
        params={"speaker": speaker},
        data=json.dumps(q),
        timeout=60,
    )

    wav.raise_for_status()

    return wav.content


play_q = queue.Queue()

def player_worker():

    while True:

        item = play_q.get()

        if item is None:
            break

        name, text, role_vid, speed, volume = item

        speaker = VOICE_MAP.get(role_vid, DEFAULT_VOICE_ID)

        print("PLAY:", name, role_vid, text)

        try:

            wav_bytes = synth_wav_bytes(text, speaker, speed, volume)

            fname = TEMP_DIR / f"temp_{uuid.uuid4().hex}.wav"

            with open(fname, "wb") as f:
                f.write(wav_bytes)

            pygame.mixer.music.load(str(fname))
            pygame.mixer.music.play()

            while pygame.mixer.music.get_busy():
                time.sleep(0.02)

        except Exception as e:
            print("再生エラー:", e)

        finally:
            play_q.task_done()


# ===============================
# Flask
# ===============================

app = Flask(__name__)

@app.route("/voice_input", methods=["POST"])
def voice_input():

    payload = request.get_json(silent=True)

    if payload is None:
        payload = request.get_data(as_text=True)

    text_block = ""

    if isinstance(payload, dict):

        if isinstance(payload.get("lines"), list):
            text_block = "\n".join(payload["lines"])

        elif "text" in payload:
            text_block = payload["text"]

    elif isinstance(payload, str):
        text_block = payload

    items = parse_voice_lines(text_block)

    for name, txt, vid, spd, vol in items:
        play_q.put((name, txt, vid, spd, vol))

    return jsonify({"queued": len(items)})


if __name__ == "__main__":

    print("VOICE SERVER START")

    build_voice_map()

    pygame.mixer.init()

    t = threading.Thread(target=player_worker, daemon=True)
    t.start()

    app.run(host="127.0.0.1", port=5000)

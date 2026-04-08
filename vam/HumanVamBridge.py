# ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
# FILE:HumanVamBridge.py
# VAM BRIDGE (AI → TAM → PY → VAM)
# Real-time VOICE / POSE Controller
# TRACK / TM Timeline Engine
# Author: VAMT
# ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
import os
import socket
import threading
import time
import tkinter as tk
from tkinter import scrolledtext, filedialog
from http.server import BaseHTTPRequestHandler, HTTPServer
import tkinter.font as tkfont

# --- 設定 ---
VAM_IP = "127.0.0.1"
VAM_PORT_TX = 10001
PY_PORT_RX = 9999
TAM_PORT_RX = 8080
POSE_DIR = "./poses"

# 同じTRACKのPOSEをまとめる待ち時間
POSE_BUFFER_SEC = 0.35

# ===== フォント設定（カラー絵文字対応）=====
EMOJI_FONT = "Segoe UI Emoji"


class TamHandler(BaseHTTPRequestHandler):
    def do_POST(self):
        content_length = int(self.headers.get("Content-Length", 0))
        post_data = self.rfile.read(content_length).decode("utf-8")

        if post_data and hasattr(self.server, "gui_app"):
            self.server.gui_app.auto_relay_from_tam(post_data)

        self.send_response(200)
        self.send_header("Access-Control-Allow-Origin", "*")
        self.send_header("Access-Control-Allow-Methods", "POST, OPTIONS")
        self.send_header("Access-Control-Allow-Headers", "Content-Type")
        self.end_headers()
        self.wfile.write(b"OK")

    def do_OPTIONS(self):
        self.send_response(200)
        self.send_header("Access-Control-Allow-Origin", "*")
        self.send_header("Access-Control-Allow-Methods", "POST, OPTIONS")
        self.send_header("Access-Control-Allow-Headers", "Content-Type")
        self.end_headers()

    def log_message(self, format, *args):
        return


class VamBridgeGUI:
    def __init__(self, root):
        self.root = root
        self.root.title("HumanVamBridge (VOICE / POSE)")
        self.root.geometry("760x660")

        try:
            default_font = tkfont.nametofont("TkDefaultFont")
            default_font.config(family=EMOJI_FONT, size=10)

            text_font = tkfont.nametofont("TkTextFont")
            text_font.config(family=EMOJI_FONT, size=10)
        except Exception as e:
            print("Font setting error:", e)

        # ===== 再生制御 =====
        self.track_threads = {}
        self.track_stop_flags = {}
        self.track_run_ids = {}
        self._next_run_id = 1
        self.track_lock = threading.Lock()

        # ===== UI表示用 TRACK ブロック =====
        self.pose_blocks = {}
        self.pose_block_order = []

        # ===== POSEバッファ =====
        self.pose_pending = {}
        self.pose_pending_lock = threading.Lock()

        # ===== ログ更新順序制御 =====
        self._log_seq = 0

        # ===== Stop Loops 点滅制御 =====
        self.stop_loops_blinking = False
        self.stop_loops_blink_state = False
        self.stop_loops_default_bg = "#ffe8b3"
        self.stop_loops_active_bg = "#ff6b6b"

        # ===== 受信ログ =====
        tk.Label(root, text="Incoming Log (TAM / VaM):").pack(pady=5)
        self.rx_text = scrolledtext.ScrolledText(root, height=6, width=90)
        self.rx_text.pack(padx=10, pady=5)
        self.rx_text.config(state=tk.DISABLED, font=(EMOJI_FONT, 10))

        # ===== VOICE =====
        tk.Label(root, text="🔊 VOICE").pack(pady=2)
        self.tx_voice = scrolledtext.ScrolledText(root, height=5, width=90)
        self.tx_voice.pack(padx=10, pady=2)
        self.tx_voice.config(font=(EMOJI_FONT, 10))

        # ===== POSE =====
        tk.Label(root, text="💽 POSE").pack(pady=2)
        self.tx_pose = scrolledtext.ScrolledText(root, height=10, width=90)
        self.tx_pose.pack(padx=10, pady=2)
        self.tx_pose.config(font=(EMOJI_FONT, 10))

        # ===== 小ボタン群 =====
        btn_frame = tk.Frame(root)
        btn_frame.pack(pady=10)

        self.clear_pose_btn = tk.Button(
            btn_frame,
            text="Clear POSE",
            width=14,
            bg="#ffd6d6",
            command=self.clear_pose_box,
        )
        self.clear_pose_btn.pack(side=tk.LEFT, padx=4)

        self.clear_voice_btn = tk.Button(
            btn_frame,
            text="Clear VOICE",
            width=14,
            bg="#d6ecff",
            command=self.clear_voice_box,
        )
        self.clear_voice_btn.pack(side=tk.LEFT, padx=4)

        self.clear_all_btn = tk.Button(
            btn_frame,
            text="Clear All",
            width=14,
            bg="#eeeeee",
            command=self.clear_all_boxes,
        )
        self.clear_all_btn.pack(side=tk.LEFT, padx=4)

        self.stop_loops_btn = tk.Button(
            btn_frame,
            text="Stop Loops",
            width=14,
            bg=self.stop_loops_default_bg,
            command=self.stop_all_tracks,
        )
        self.stop_loops_btn.pack(side=tk.LEFT, padx=4)

        self.load_pose_btn = tk.Button(
            btn_frame,
            text="Load POSE File",
            width=16,
            bg="#d6ffd6",
            command=self.load_pose_from_dialog,
        )
        self.load_pose_btn.pack(side=tk.LEFT, padx=4)

        # ===== 下部Sendボタン =====
        self.send_btn = tk.Button(
            root,
            text="Send To VaM",
            font=(EMOJI_FONT, 14, "bold"),
            height=2,
            bg="#cce5ff",
            command=self.send_to_vam,
        )
        self.send_btn.pack(fill=tk.X, padx=10, pady=10)

        # ===== UDP =====
        self.sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
        self.sock.bind(("0.0.0.0", PY_PORT_RX))

        self.running = True

        threading.Thread(target=self.receive_loop_vam, daemon=True).start()
        threading.Thread(target=self.run_tam_server, daemon=True).start()

        self.root.protocol("WM_DELETE_WINDOW", self.on_close)

    # ============================================================
    # 基本ユーティリティ
    # ============================================================
    def log(self, msg: str):
        print(msg)
        self._log_seq += 1
        seq = self._log_seq
        self.root.after(0, self.update_rx_text, msg, seq)

    def send_udp_line(self, line: str):
        try:
            self.sock.sendto(line.encode("utf-8"), (VAM_IP, VAM_PORT_TX))
        except Exception as e:
            self.log(f"[PY][Send Error] {e}")

    def safe_float(self, s, default=0.5):
        try:
            return float(s)
        except Exception:
            return default

    def ensure_pose_dir(self):
        try:
            os.makedirs(POSE_DIR, exist_ok=True)
        except Exception as e:
            self.log(f"[POSE DIR ERROR] {e}")

    def load_pose_file(self, filename: str):
        try:
            self.ensure_pose_dir()
            safe_name = os.path.basename(filename.strip())
            path = os.path.join(POSE_DIR, safe_name)

            with open(path, "r", encoding="utf-8") as f:
                content = f.read()

            self.log(f"[POSE FILE LOAD] {safe_name}")
            return content
        except Exception as e:
            self.log(f"[POSE FILE LOAD ERROR] {filename} / {e}")
            return None

    def load_pose_from_dialog(self):
        self.ensure_pose_dir()

        path = filedialog.askopenfilename(
            title="Select POSE File",
            initialdir=os.path.abspath(POSE_DIR),
            filetypes=[
                ("POSE files", "*.pose"),
                ("Text files", "*.txt"),
                ("All files", "*.*"),
            ],
        )

        if not path:
            return

        try:
            with open(path, "r", encoding="utf-8-sig") as f:
                content = f.read().strip()

            if not content:
                self.log("[POSE LOAD] empty file")
                return

            self.set_pose_box_text(content)
            self.log(f"[POSE LOAD] {os.path.basename(path)}")

        except Exception as e:
            self.log(f"[POSE LOAD ERROR] {e}")

    def extract_pose_filename(self, line: str):
        parts = line.split("|")
        for part in parts:
            part = part.strip()
            if part.startswith("FILE,"):
                return part[len("FILE,"):].strip()
        return None

    # ============================================================
    # UI表示用 POSEブロック管理
    # ============================================================
    def get_pose_track_name(self, line: str):
        item = self.parse_pose_line(line)
        if item is None:
            return "__NO_TRACK__"
        track = (item.get("track") or "").strip()
        return track if track else "__NO_TRACK__"

    def update_pose_blocks(self, pose_lines):
        grouped = {}
        order = []

        for line in pose_lines:
            track = self.get_pose_track_name(line)
            if track not in grouped:
                grouped[track] = []
                order.append(track)
            grouped[track].append(line)

        self.pose_blocks = grouped
        self.pose_block_order = order
        self.refresh_pose_box()

    def refresh_pose_box(self):
        blocks = []

        for track in self.pose_block_order:
            lines = self.pose_blocks.get(track)
            if not lines:
                continue

            if track == "__NO_TRACK__":
                block = "\n".join(lines)
            else:
                header = f"[{track}]"
                body = "\n".join(lines)
                block = header + "\n" + body

            blocks.append(block)

        self.set_pose_box_text("\n\n".join(blocks))

    # ============================================================
    # Stop Loops 点滅表示
    # ============================================================
    def set_stop_loops_blink(self, active: bool):
        self.root.after(0, self._set_stop_loops_blink_ui, active)

    def _set_stop_loops_blink_ui(self, active: bool):
        if active:
            if not self.stop_loops_blinking:
                self.stop_loops_blinking = True
                self.stop_loops_blink_state = False
                self._blink_stop_loops_btn()
        else:
            self.stop_loops_blinking = False
            self.stop_loops_blink_state = False
            try:
                self.stop_loops_btn.config(
                    bg=self.stop_loops_default_bg,
                    activebackground=self.stop_loops_default_bg
                )
            except Exception:
                pass

    def _blink_stop_loops_btn(self):
        if not self.stop_loops_blinking:
            try:
                self.stop_loops_btn.config(
                    bg=self.stop_loops_default_bg,
                    activebackground=self.stop_loops_default_bg
                )
            except Exception:
                pass
            return

        self.stop_loops_blink_state = not self.stop_loops_blink_state
        color = self.stop_loops_active_bg if self.stop_loops_blink_state else self.stop_loops_default_bg

        try:
            self.stop_loops_btn.config(bg=color, activebackground=color)
        except Exception:
            return

        self.root.after(350, self._blink_stop_loops_btn)

    def refresh_stop_loops_blink(self):
        with self.track_lock:
            any_loop_running = any(not flag for flag in self.track_stop_flags.values())
        self.set_stop_loops_blink(any_loop_running)

    # ============================================================
    # POSEバッファ
    # ============================================================
    def buffer_pose_lines(self, pose_lines):
        grouped = {}

        for line in pose_lines:
            track = self.get_pose_track_name(line)
            grouped.setdefault(track, []).append(line)

        for track, lines in grouped.items():
            self.add_pose_pending(track, lines)

    def add_pose_pending(self, track, lines):
        with self.pose_pending_lock:
            entry = self.pose_pending.get(track)
            if entry is None:
                entry = {
                    "lines": [],
                    "timer": None,
                }
                self.pose_pending[track] = entry

            existing = set(entry["lines"])
            for line in lines:
                if line not in existing:
                    entry["lines"].append(line)
                    existing.add(line)

            has_loop = False
            for line in entry["lines"]:
                item = self.parse_pose_line(line)
                if item is not None and item.get("loop", False):
                    has_loop = True
                    break

            old_timer = entry.get("timer")
            if old_timer is not None:
                try:
                    old_timer.cancel()
                except Exception:
                    pass
                entry["timer"] = None

            if has_loop:
                timer = threading.Timer(0.01, self.flush_pose_pending_track, args=(track,))
            else:
                timer = threading.Timer(POSE_BUFFER_SEC, self.flush_pose_pending_track, args=(track,))

            entry["timer"] = timer
            timer.daemon = True
            timer.start()

    def flush_pose_pending_track(self, track):
        with self.pose_pending_lock:
            entry = self.pose_pending.get(track)
            if not entry:
                return

            lines = list(entry.get("lines", []))
            timer = entry.get("timer")
            if timer is not None:
                try:
                    timer.cancel()
                except Exception:
                    pass

            self.pose_pending.pop(track, None)

        if not lines:
            return

        self.log(f"[POSE][FLUSH] {track} / lines={len(lines)}")

        self.root.after(0, self.update_pose_blocks, list(lines))

        self.stop_all_tracks()
        self.handle_pose_lines(lines)

    def flush_all_pose_pending(self):
        with self.pose_pending_lock:
            tracks = list(self.pose_pending.keys())

        for track in tracks:
            self.flush_pose_pending_track(track)

    # ============================================================
    # TAMサーバ
    # ============================================================
    def run_tam_server(self):
        server = HTTPServer(("0.0.0.0", TAM_PORT_RX), TamHandler)
        server.gui_app = self
        server.serve_forever()

    # ============================================================
    # TAM受信
    # ============================================================
    def auto_relay_from_tam(self, msg: str, update_ui: bool = True, buffer_pose: bool = True):
        self.log(f"[TAM] {msg}")

        lines = [line.strip() for line in msg.splitlines() if line.strip()]
        if not lines:
            return

        voice_lines = []
        pose_lines = []
        other_lines = []

        for line in lines:
            if line.startswith("💽POSE|FILE,"):
                filename = self.extract_pose_filename(line)
                if filename:
                    content = self.load_pose_file(filename)
                    if content:
                        self.log(f"[POSE FILE EXPAND] {filename}")
                        self.auto_relay_from_tam(content, update_ui=update_ui, buffer_pose=buffer_pose)
                continue

            if line.startswith("🔊VOICE|"):
                voice_lines.append(line)
            elif line.startswith("💽POSE|"):
                pose_lines.append(line)
            else:
                other_lines.append(line)

        if update_ui and voice_lines:
            self.root.after(0, self.set_voice_box_text, "\n".join(voice_lines))

        for line in voice_lines:
            self.send_udp_line(line)

        if pose_lines:
            if buffer_pose:
                self.buffer_pose_lines(pose_lines)
            else:
                if update_ui:
                    self.root.after(0, self.update_pose_blocks, list(pose_lines))
                self.stop_all_tracks()
                self.handle_pose_lines(pose_lines)

        for line in other_lines:
            self.send_udp_line(line)

    # ============================================================
    # VOICE / POSE 表示
    # ============================================================
    def set_voice_box_text(self, text: str):
        self.tx_voice.delete(1.0, tk.END)
        self.tx_voice.insert(tk.END, text)

    def set_pose_box_text(self, text: str):
        self.tx_pose.delete(1.0, tk.END)
        self.tx_pose.insert(tk.END, text)

    # ============================================================
    # POSE TM / TRACK 処理
    # ============================================================
    def parse_pose_line(self, line: str):
        parts = line.split("|")
        if len(parts) < 3:
            return None

        if parts[0] != "💽POSE":
            return None

        if len(parts) >= 2 and (parts[1] or "").strip().startswith("FILE,"):
            return None

        raw_track = (parts[1] or "").strip()
        tm_raw = (parts[2] or "").strip()

        track_name = raw_track.strip()

        if not tm_raw.startswith("TM,"):
            return None

        hold_sec = 0.5
        loop_flag = False

        tm_parts = [p.strip() for p in tm_raw.split(",")]
        if len(tm_parts) >= 2:
            hold_sec = self.safe_float(tm_parts[1], 0.5)
        if len(tm_parts) >= 3 and tm_parts[2].upper() == "L":
            loop_flag = True

        return {
            "track": track_name,
            "hold": hold_sec,
            "loop": loop_flag,
            "raw": line,
        }

    def handle_pose_lines(self, pose_lines):
        parsed = []
        for line in pose_lines:
            item = self.parse_pose_line(line)
            if item is None:
                self.send_udp_line(line)
            else:
                parsed.append(item)

        if not parsed:
            return

        grouped = {}
        for item in parsed:
            grouped.setdefault(item["track"], []).append(item)

        for track_name, frames in grouped.items():
            self.start_or_replace_track(track_name, frames)

    def start_or_replace_track(self, track_name, frames):
        if not track_name:
            for frame in frames:
                self.send_udp_line(frame["raw"])
            return

        with self.track_lock:
            run_id = self._next_run_id
            self._next_run_id += 1

            self.track_run_ids[track_name] = run_id
            self.track_stop_flags[track_name] = False

            th = threading.Thread(
                target=self.run_track_loop,
                args=(track_name, frames, run_id),
                daemon=True,
            )
            self.track_threads[track_name] = th
            th.start()

    def is_track_stopped(self, track_name, run_id):
        with self.track_lock:
            if self.track_stop_flags.get(track_name, True):
                return True
            current_run_id = self.track_run_ids.get(track_name)
            return current_run_id != run_id

    def run_track_loop(self, track_name, frames, run_id):
        has_loop = any(frame["loop"] for frame in frames)

        self.log(f"[POSE][START] {track_name} / frames={len(frames)} / loop={has_loop} / run_id={run_id}")

        if has_loop:
            self.set_stop_loops_blink(True)

        try:
            while True:
                for idx, frame in enumerate(frames):
                    if self.is_track_stopped(track_name, run_id):
                        self.log(f"[POSE][STOP] {track_name} / run_id={run_id}")
                        self.refresh_stop_loops_blink()
                        return

                    self.send_udp_line(frame["raw"])
                    self.log(
                        f"[POSE][SEND] {track_name} "
                        f"frame={idx + 1}/{len(frames)} "
                        f"hold={frame['hold']} "
                        f"loop_end={frame['loop']} "
                        f"run_id={run_id}"
                    )

                    sleep_left = max(0.01, frame["hold"])
                    step = 0.02
                    while sleep_left > 0:
                        if self.is_track_stopped(track_name, run_id):
                            self.log(f"[POSE][STOP] {track_name} / run_id={run_id}")
                            self.refresh_stop_loops_blink()
                            return
                        time.sleep(min(step, sleep_left))
                        sleep_left -= step

                if not has_loop:
                    self.log(f"[POSE][END] {track_name} / run_id={run_id}")
                    with self.track_lock:
                        if self.track_run_ids.get(track_name) == run_id:
                            self.track_stop_flags[track_name] = True
                    self.refresh_stop_loops_blink()
                    return

        except Exception as e:
            self.log(f"[POSE][ERROR] {track_name} / run_id={run_id} / {e}")
            with self.track_lock:
                if self.track_run_ids.get(track_name) == run_id:
                    self.track_stop_flags[track_name] = True
            self.refresh_stop_loops_blink()

    def stop_all_tracks(self):
        with self.track_lock:
            for k in list(self.track_stop_flags.keys()):
                self.track_stop_flags[k] = True
        self.set_stop_loops_blink(False)
        self.log("[POSE] all tracks stop requested")

    # ============================================================
    # VaM受信
    # ============================================================
    def receive_loop_vam(self):
        while self.running:
            try:
                data, _ = self.sock.recvfrom(8192)
                msg = data.decode("utf-8")
                self._log_seq += 1
                seq = self._log_seq
                self.root.after(0, self.update_rx_text, f"[VaM] {msg}", seq)
            except Exception:
                break

    # ============================================================
    # ログ更新
    # ============================================================
    def update_rx_text(self, msg: str, seq: int):
        if seq != self._log_seq:
            return

        self.rx_text.config(state=tk.NORMAL)
        self.rx_text.delete(1.0, tk.END)
        self.rx_text.insert(tk.END, msg)
        self.rx_text.config(state=tk.DISABLED)

    # ============================================================
    # 手動送信
    # ============================================================
    def send_to_vam(self):
        voice = self.tx_voice.get(1.0, tk.END).strip()
        pose = self.tx_pose.get(1.0, tk.END).strip()

        if voice:
            self.send_udp_line(voice)

        if pose:
            lines = [line.strip() for line in pose.splitlines() if line.strip()]
            expanded_lines = []

            for line in lines:
                if line.endswith(".pose") and not line.startswith("💽POSE|"):
                    content = self.load_pose_file(line)
                    if content:
                        self.log(f"[POSE FILE EXPAND] {line}")
                        expanded_lines.append(content.strip())
                    continue

                if line.startswith("[") and line.endswith("]"):
                    continue

                expanded_lines.append(line)

            merged = "\n".join(expanded_lines)

            did_expand_pose_file = any(
                line.endswith(".pose") and not line.startswith("💽POSE|")
                for line in lines
            )

            if expanded_lines and did_expand_pose_file:
                self.set_pose_box_text(merged)

            self.auto_relay_from_tam(merged, update_ui=False, buffer_pose=False)

    # ============================================================
    # クリアボタン
    # ============================================================
    def clear_pose_box(self):
        self.tx_pose.delete(1.0, tk.END)
        self.pose_blocks = {}
        self.pose_block_order = []
        with self.pose_pending_lock:
            for track, entry in list(self.pose_pending.items()):
                timer = entry.get("timer")
                if timer is not None:
                    try:
                        timer.cancel()
                    except Exception:
                        pass
            self.pose_pending = {}
        self.log("[UI] POSE box cleared")

    def clear_voice_box(self):
        self.tx_voice.delete(1.0, tk.END)
        self.log("[UI] VOICE box cleared")

    def clear_all_boxes(self):
        self.tx_voice.delete(1.0, tk.END)
        self.tx_pose.delete(1.0, tk.END)
        self.pose_blocks = {}
        self.pose_block_order = []
        with self.pose_pending_lock:
            for track, entry in list(self.pose_pending.items()):
                timer = entry.get("timer")
                if timer is not None:
                    try:
                        timer.cancel()
                    except Exception:
                        pass
            self.pose_pending = {}
        self.log("[UI] all boxes cleared")

    # ============================================================
    # 終了
    # ============================================================
    def on_close(self):
        self.running = False
        self.stop_all_tracks()
        self.flush_all_pose_pending()
        try:
            self.sock.close()
        except Exception:
            pass
        self.root.destroy()


if __name__ == "__main__":
    root = tk.Tk()
    app = VamBridgeGUI(root)
    root.mainloop()
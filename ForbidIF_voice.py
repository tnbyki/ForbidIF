# =========================================================
# ForbidIF VOICE Server (VOICEVOX Bridge)
# - Receives [V]...[/V] lines from Tampermonkey (Gemini)
# - Queues and plays them via VOICEVOX + pygame
# =========================================================
import re
import json
import time
import threading
import queue
import uuid
from pathlib import Path

import requests
import pygame
from flask import Flask, request, jsonify

# ===== Temp directory setup =====
BASE_DIR = Path(__file__).parent
TEMP_DIR = BASE_DIR / "Temp"
TEMP_DIR.mkdir(exist_ok=True)

VOICEVOX_URL = "http://127.0.0.1:50021"

DEFAULT_VOICE_ID = 1
DEFAULT_SPEED = 1.0

# ===============================
# VOICEVOX: (話者名, スタイル名) -> style_id を取得
# ===============================
def get_style_id(speaker_name: str, style_name: str = "ノーマル") -> int:
    speakers = requests.get(f"{VOICEVOX_URL}/speakers", timeout=10).json()
    for sp in speakers:
        if sp.get("name") == speaker_name:
            for st in sp.get("styles", []):
                if st.get("name") == style_name:
                    return int(st.get("id"))
    raise ValueError(f"style_id not found: {speaker_name} / {style_name}")

# ===============================
# あなたの「役割VOICE_ID」 -> VOICEVOX style_id
# ===============================
VOICE_STYLE_SPEC = {
0: ("青山龍星", "ノーマル"),   # ナレーション（予約）
1: ("白上虎太郎", "ふつう"),   # 元気
2: ("四国めたん", "あまあま"), # かわいい
3: ("波音リツ", "ノーマル"),   # おとな
4: ("春日部つむぎ", "ノーマル"),# クール
5: ("冥鳴ひまり", "ノーマル"), # セクシー
6: ("九州そら", "ノーマル"),   # まったり
7: ("春日部つむぎ", "ノーマル"),# せっかち
}

VOICE_MAP: dict[int, int] = {}

def build_voice_map() -> None:
    """起動時に (役割VOICE_ID -> VOICEVOX style_id) を確定させる"""
    global VOICE_MAP
    VOICE_MAP = {}
    for vid, (name, style) in VOICE_STYLE_SPEC.items():
        try:
            VOICE_MAP[vid] = get_style_id(name, style)
        except Exception as e:
            print("VOICE取得失敗:", vid, name, style, e)

# ===============================
# [V] タグ解析
# 例: [V]佐藤 栞(3:0.90)……ギリギリね。[/V]
#     [V]名前(VOICE_ID:speed:volume)セリフ[/V] もOK
# ===============================
VOICE_RE = re.compile(
    r"\[V\]\s*([^\(\[\]\r\n]+?)\s*\((\d+):([0-9]+(?:\.[0-9]+)?)(?::([0-9]+(?:\.[0-9]+)?))?\)\s*([^\r\n]*?)\s*\[/V\]"
)

def parse_voice_lines(text_block: str):
    """text_block から (name,text,voice_id,speed,volume) を抜き出す"""
    results = []
    for m in VOICE_RE.finditer(text_block or ""):
        name = (m.group(1) or "").strip()
        vid = int(m.group(2))
        spd = float(m.group(3))
        vol = float(m.group(4)) if m.group(4) else 1.0
        txt = (m.group(5) or "").strip()
        if txt:
            results.append((name, txt, vid, spd, vol))
    return results

# ===============================
# VOICEVOX 合成
# ===============================
def synth_wav_bytes(text: str, speaker: int, speed: float, volume: float = 1.0) -> bytes:
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

# ===============================
# 再生キュー（順番待ちで必ず連続再生）
# ===============================
play_q: "queue.Queue[tuple[str, str, int, float, float]]" = queue.Queue()

def player_worker():
    """キューを順番に再生。wavは毎回ユニーク名で保存（Windowsロック対策）"""
    while True:
        item = play_q.get()
        if item is None:
            break

        name, text, role_vid, speed, volume = item
        speaker = VOICE_MAP.get(role_vid, VOICE_MAP.get(DEFAULT_VOICE_ID, DEFAULT_VOICE_ID))
        print("SPEAKER:", speaker, "ROLE_ID:", role_vid, "SPEC:", VOICE_STYLE_SPEC.get(role_vid))

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

@app.get("/health")
def health():
    return jsonify({"status": "ok", "voice_map": VOICE_MAP})

def _extract_text_block(payload) -> str:
    """
    受け取り形式の揺れ吸収:
    - {"line": "..."}        (旧)
    - {"text": "..."}        (別名)
    - {"lines": ["..."]}     (推奨)
    - "..." (text/plain)
    """
    if payload is None:
        return ""
    if isinstance(payload, str):
        return payload
    if isinstance(payload, dict):
        if isinstance(payload.get("lines"), list):
            return "\n".join([str(x) for x in payload.get("lines")])
        if "line" in payload:
            return str(payload.get("line") or "")
        if "text" in payload:
            return str(payload.get("text") or "")
    return ""

def _queue_from_text_block(text_block: str) -> int:
    items = parse_voice_lines(text_block)
    for name, txt, vid, spd, vol in items:
        print(f"[RECV] {name} VOICE_ID={vid} speed={spd} vol={vol} : {txt}")
        play_q.put((name, txt, vid, spd, vol))
    return len(items)

@app.route("/voice_input", methods=["POST"])
def voice_input():
    payload = request.get_json(silent=True)
    if payload is None:
        payload = request.get_data(as_text=True)

    print("[HTTP] /voice_input payload =", payload)

    text_block = _extract_text_block(payload)
    queued = _queue_from_text_block(text_block)
    return jsonify({"status": "ok", "queued": queued})

# 互換: 古いTAMが叩く可能性があるパスも受ける
@app.route("/tam", methods=["POST"])
def tam_alias():
    payload = request.get_json(silent=True)
    if payload is None:
        payload = request.get_data(as_text=True)

    print("[HTTP] /tam payload =", payload)

    text_block = _extract_text_block(payload)
    queued = _queue_from_text_block(text_block)
    return jsonify({"status": "ok", "queued": queued})

if __name__ == "__main__":
    print("🎤 VOICEサーバー起動中...")

    print("=== VOICEVOX speakers ===")
    print(requests.get("http://127.0.0.1:50021/speakers").json())
    print("=========================")

    build_voice_map()
    print("VOICE_MAP:", VOICE_MAP)

    pygame.mixer.init()

    t = threading.Thread(target=player_worker, daemon=True)
    t.start()

    app.run(host="127.0.0.1", port=5000)

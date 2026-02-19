# =========================================================
# ForbidIF VOICE Server (VOICEVOX Bridge)
# Queue playback / role → style mapping
# =========================================================
import re
import json
import time
import threading
import queue
import uuid
import os
import requests
import pygame
from flask import Flask, request, jsonify

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
    0: ("青山龍星", "ノーマル"),      # ナレーション（予約）
    1: ("白上虎太郎", "ノーマル"),    # 元気
    2: ("四国めたん", "あまあま"),    # かわいい
    3: ("波音リツ", "ノーマル"),    # おとな
    4: ("春日部つむぎ", "ノーマル"),  # クール
    5: ("冥鳴ひまり", "ノーマル"),    # セクシー
    6: ("九州そら", "ノーマル"),      # まったり
    7: ("春日部つむぎ", "ノーマル"),  # せっかち（つむぎはノーマルのみの環境が多い）
}

VOICE_MAP: dict[int, int] = {}

def build_voice_map() -> None:
    """
    起動時に (役割VOICE_ID -> VOICEVOX style_id) を確定させる
    """
    global VOICE_MAP
    VOICE_MAP = {}
    for vid, (name, style) in VOICE_STYLE_SPEC.items():
        try:
            VOICE_MAP[vid] = get_style_id(name, style)
        except Exception as e:
            print("VOICE取得失敗:", vid, name, style, e)

# ===============================
# [VOICE] タグ解析
# 例: 🔴Dolf(4:1.05)[VOICE]来たぞ！[/VOICE]
# ===============================
VOICE_RE = re.compile(
    r"\((\d+):([0-9]+(?:\.[0-9]+)?)\)\[VOICE\]([\s\S]+?)\[/VOICE\]"
)

def parse_voice_lines(text_block: str):
    """
    テキストから (text, voice_id, speed) を全部抜き出す
    """
    results = []
    for m in VOICE_RE.finditer(text_block):
        vid = int(m.group(1))
        spd = float(m.group(2))
        txt = m.group(3).strip()
        if txt:
            results.append((txt, vid, spd))
    return results

# ===============================
# VOICEVOX 合成
# ===============================
def synth_wav_bytes(text: str, speaker: int, speed: float) -> bytes:
    q = requests.post(
        f"{VOICEVOX_URL}/audio_query",
        params={"text": text, "speaker": speaker},
        timeout=30,
    ).json()
    q["speedScale"] = speed

    wav = requests.post(
        f"{VOICEVOX_URL}/synthesis",
        params={"speaker": speaker},
        data=json.dumps(q),
        timeout=60,
    )
    return wav.content

# ===============================
# 再生キュー（順番待ちで必ず連続再生）
# ===============================
play_q: "queue.Queue[tuple[str, int, float]]" = queue.Queue()

def safe_remove(path: str) -> None:
    try:
        os.remove(path)
    except:
        pass

def player_worker():
    """
    キューを順番に再生。wavは毎回ユニーク名で保存（Windowsロック対策）
    """
    while True:
        item = play_q.get()
        if item is None:
            break

        text, role_vid, speed = item

        speaker = VOICE_MAP.get(role_vid, VOICE_MAP.get(DEFAULT_VOICE_ID, DEFAULT_VOICE_ID))

        try:
            wav_bytes = synth_wav_bytes(text, speaker, speed)

            # ★重要：毎回ファイル名を変える（temp.wav固定だとPermission deniedになりやすい）
            fname = f"temp_{uuid.uuid4().hex}.wav"
            with open(fname, "wb") as f:
                f.write(wav_bytes)

            pygame.mixer.music.load(fname)
            pygame.mixer.music.play()

            while pygame.mixer.music.get_busy():
                time.sleep(0.02)

            safe_remove(fname)

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
    data = request.json or {}
    text_block = data.get("line", "")

    items = parse_voice_lines(text_block)
    for txt, vid, spd in items:
        print(f"[キュー] VOICE_ID={vid} speed={spd} : {txt}")
        play_q.put((txt, vid, spd))

    return jsonify({"status": "ok", "queued": len(items)})

# ===============================
# 起動
# ===============================
if __name__ == "__main__":
    print("🎤 VOICEサーバー起動中...")

    build_voice_map()
    print("VOICE_MAP:", VOICE_MAP)

    # mixerは1回だけ初期化（連続再生安定）
    pygame.mixer.init()

    # 再生スレッド開始
    t = threading.Thread(target=player_worker, daemon=True)
    t.start()

    app.run(port=5000)

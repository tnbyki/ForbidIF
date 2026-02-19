# ForbidIF VOICE Environment Setup  
ForbidIF VOICE 環境構築ガイド

---

## 🔊 Overview / 概要

This module connects ChatGPT output to real-time emotional voice playback.  
ChatGPTの出力をリアルタイム音声として再生するためのモジュールです。

Text logs are not read aloud —  
they are **rendered as a live phenomenon in real time**.

これは読み上げではありません。  
構造化ログを「現在進行している現象」として再生します。

---

## 🧩 System Architecture / システム構成

ChatGPT  
↓  
Tampermonkey (VOICE line detection)  
↓  
Python VOICE Server  
↓  
VOICEVOX (audio synthesis)

---

## 📦 Requirements / 必要環境

### Common
- VOICEVOX
- Python 3.10+
- Browser (Chrome recommended)
- Tampermonkey

---

## ① Install VOICEVOX / VOICEVOXのインストール

Download:  
https://voicevox.hiroshiba.jp/

Start VOICEVOX and keep it running.

VOICEVOXを起動したままにしてください。

---

## ② Install Python / Pythonのインストール

Download:  
https://www.python.org/

```md
確認:

```bash
python --version
③ Install Python Libraries / ライブラリ導入
pip install requests
例：

```md
## ❗ Troubleshooting / トラブルシュート

No sound → Check:
- VOICEVOX is running
- Python server is running
- Browser autoplay is allowed

④ Start VOICE Server / VOICEサーバー起動
python ForbidIF_voice.py
When running:

VOICE server started
と表示されればOK。

⑤ Install Tampermonkey / Tampermonkey導入
Chrome Extension:

https://www.tampermonkey.net/

⑥ Install User Script / ユーザースクリプト登録
Tampermonkey → Create new script

Paste ForbidIF_voice.js

Set target:

@match https://chat.openai.com/*
@match https://chatgpt.com/*
Save

▶ Execution Flow / 実行手順
Start VOICEVOX

Run Python server

Open ChatGPT

Generate dialogue with VOICE tags

VOICE付き発話が出ると自動再生されます。

🧪 VOICE Format / VOICE形式
🔴名前(VOICE_ID:speed)[VOICE]セリフ[/VOICE]
Example:

🔴ラム(2:1.05)[VOICE]こんにちはだっちゃ[/VOICE]
🎭 VOICE_ID Reference / VOICE_ID対応表
ID	Style
0	Narration
1	Energetic
2	Cute
3	Calm / Adult
4	Cool
5	Sexy
6	Relaxed
7	Fast
⚠ Notes / 注意
VOICEVOX must be running

Python server must be running

Browser auto-play must be allowed

🚀 Concept / コンセプト
VOICE is not TTS.
It is a real-time synchronization interface between narrative time and experience time.

VOICEはTTSではない。
物語時間と体験時間を同期する現実側インターフェースである。


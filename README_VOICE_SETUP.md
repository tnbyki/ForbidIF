# ✅ ForbidIF VOICE Setup


→ 概要・特徴は README_VOICE.md を参照
---
# ⚡ 最短1分クイックスタート

1) VOICEVOX 起動  
2) python ForbidIF_voice.py  
3) Tampermonkey を ON  
4) AI CHATで、ForbidIF_CORE.txt ※起動
5) VOICE_ON ※音声モードをONに変更
6) TAPEもしくは、手動入力でシナリオを開始
---

# 🖼 Tampermonkey 設定手順

1. Chrome に Tampermonkey をインストール  
2. ダッシュボードを開く  
3. 「＋ 新規スクリプト」  
4. ForbidIF_voice.js を貼り付けて保存  
5. Enabled を確認  

> 💡 画面右下に **[TAM: ON]** が表示されていれば動作中  
> ボタンを押すと VOICEVOX に送るパラメータが表示されます  
> `Ctrl + Shift + R` で有効化される場合があります

# 🖥 Python 起動コマンド

## Windows
python ForbidIF_voice.py
または
py ForbidIF_voice.py
または
エクスプローラーからダブルクリック

## Mac
python3 ForbidIF_voice.py

---

# 🔧 ポート番号を変更した場合
ForbidIF_voice.py
app.run(port=5000)
⬆ ここを変更

Tampermonkey 側：
WAN_ENDPOINT = "http://127.0.0.1:5000/voice_input"
⬆ 同じ番号にする

---

# 📦 Downloads
🌟スペシャルサンクス

VOICEVOX  
https://voicevox.hiroshiba.jp/

Python  
https://www.python.org/downloads/

Tampermonkey  
https://chromewebstore.google.com/

---

# ⚠️ Chrome の設定
chrome://extensions/
Tampermonkey  
→ サイトアクセス許可

自動再生ブロック  
→ 許可

以上です。

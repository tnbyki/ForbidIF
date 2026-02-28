# ✅ ForbidIF VOICE Setup

→ 概要・特徴は README_VOICE.md を参照

---

# ⚡ 最短1分クイックスタート

① VOICEVOX 起動  
② python ForbidIF_voice.py  
③ Tampermonkey を ON  
④ @CMD VOICE_ON  

→ 発話の直下に [V] が出れば成功

---

# 🖼 Tampermonkey 設定手順

1. Chrome に Tampermonkey をインストール  
2. ダッシュボードを開く  
3. 「＋ 新規スクリプト」  
4. ForbidIF_voice.js を貼り付けて保存  
5. Enabled を確認  

対象ページで F12 → Console

[TAM] injected

が出ればOK

---

# 🖥 Python 起動コマンド

## Windows

python ForbidIF_voice.py

または

py ForbidIF_voice.py

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

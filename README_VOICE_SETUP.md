# ✅ ForbidIF VOICE Setup

このドキュメントは「動かす」ためだけの手順です。

---

## 0) 必要なもの

- VOICEVOX
- Python 3.x
- Tampermonkey
- ForbidIF_voice.js
- ForbidIF_voice.py

---

## 1) VOICEVOX を起動

VOICEVOX Engine を起動し、以下が開く状態にします：

http://127.0.0.1:50021

---

## 2) Python VOICE Server を起動

python ForbidIF_voice.py

---

## 3) Tampermonkey を有効化

- Tampermonkey をインストール
- ForbidIF_voice.js を追加
- 対象サイトで ON

---

## 4) VOICE を ON にする

@CMD VOICE_ON

---

## 5) 動作確認

発話の直下に `[V]` 行が出たら成功です。

```text
🔴名前：セリフ
[V]名前(VOICE_ID:speed:volume)セリフ[/V]
```

---

## 6) VOICE OFF

@CMD VOICE_OFF

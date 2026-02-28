# 🎤 ForbidIF_CORE VOICE

> 同じセリフでも、状況が変われば、声が変わる。

ForbidIF_CORE VOICE は、AIの発話をリアルタイムで感情付き音声として再生するシステムです。  
ただの読み上げではなく、物語の状態・関係性・緊張度から声の速度・音量・キャラクター性を自動生成します。

---

## ✨ Demo

```text
🔴ラム：ぴぽ
[V]ラム(2:1.05:1.03)ぴぽ[/V]
```

---

## 🧠 Why it’s cool

### 🎭 声が「キャラとして固定」される
初登場時に VOICE_ID を決定し、セッション中は変更しません。
>💡ForbidIF_COREより、対象人物の性別、性格より設定します。

### 💓 感情で「演技」が変わる
同じセリフでも状況が違えば speed / volume が変化します。
>💡ForbidIF_COREより、対象人物の状況により設定します。

### 🔉 漢字のよみかた
漢字は一度AIが解釈するため、自然な感じで漢字を読みます。

### 🧩 表示と音声が分離（安全・安定）
表示テキストは改変せず、音声取得は `[V]` 行だけで行います。


---

## 🏗 Architecture

```text
AI（ForbidIF CORE）
   ↓
Tampermonkey（ForbidIF_voice.js）
   ↓
Python VOICE Server（ForbidIF_voice.py）
   ↓
VOICEVOX（音声合成）
```

---

## 🚀 Quick Start

セットアップは `README_VOICE_SETUP.md` を参照してください。

---

## 🎛 Commands

```text
@CMD VOICE_ON
@CMD VOICE_OFF
```







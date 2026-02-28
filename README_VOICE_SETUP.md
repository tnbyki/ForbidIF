ForbidIF VOICE Environment SetupForbidIF VOICE 環境構築ガイド Overview / 概要This module connects ChatGPT output to real-time emotional voice playback, enhancing emotional capture in VOICE interactions.ChatGPTの出力をリアルタイム音声として再生するためのモジュールです。VOICEでより感情を捉え、VOICEVOXが感情的に話す点を強化しています。Text logs are not read aloud —they are rendered as a live phenomenon in real time .これは読み上げではありません。構造化ログを「現在進行している現象」として再生します。よりなめらかな日本語AIによるフリガナ生成をサポートし、発話の自然さを向上させます。 System Architecture / システム構成ChatGPT↓Tampermonkey (VOICE line detection)↓Python VOICE Server (ForbidIF_voice.py)↓VOICEVOX (audio synthesis with emotional intonation) Requirements / 必要環境CommonVOICEVOX (for emotional voice synthesis)
Python 3.10+
Browser (Chrome recommended)
Tampermonkey

① Install VOICEVOX / VOICEVOXのインストールDownload:
https://voicevox.hiroshiba.jp/Start VOICEVOX and keep it running. This enables emotional speech variations based on VOICE parameters.VOICEVOXを起動したままにしてください。感情的な話し方を可能にします。② Install Python / PythonのインストールDownload:
https://www.python.org/確認:bash

python --version

③ Install Python Libraries / ライブラリ導入bash

pip install requests

例：md

## ❗ Troubleshooting / トラブルシュート

No sound → Check:
- VOICEVOX is running
- Python server is running
- Browser autoplay is allowed
- Emotional parameters are correctly parsed (check console for errors)

---

## ④ Start VOICE Server / VOICEサーバー起動 (TAM PY導入実行)

TAM PY（Tampermonkey Python連携）の導入実行として、以下の手順でForbidIF_voice.pyをセットアップします。

1. リポジトリからForbidIF_voice.pyをダウンロード（https://github.com/tnbyki/ForbidIF/blob/main/ForbidIF_voice.py）。

2. コマンドラインでスクリプトのあるディレクトリに移動。

3. 実行：

```bash
python ForbidIF_voice.py

When running:VOICE server started
と表示されればOK。感情パラメータ（VOICE_ID:speed:volume）を処理し、VOICEVOXに送信します。⑤ Install Tampermonkey / Tampermonkey導入Chrome Extension:https://www.tampermonkey.net/⑥ Install User Script / ユーザースクリプト登録Tampermonkey → Create new scriptPaste ForbidIF_voice.js（https://github.com/tnbyki/ForbidIF/blob/main/ForbidIF_voice.js）Set target:@match
 https://chat.openai.com/*
@match
 https://chatgpt.com/*Save Execution Flow / 実行手順Start VOICEVOX
Run Python server (ForbidIF_voice.py)
Open ChatGPT
Generate dialogue with VOICE tags

VOICE付き発話が出ると自動再生されます。感情を捉えたVOICEVOXの話し方が適用され、なめらかなフリガナ生成で日本語発音を最適化。 VOICE Format / VOICE形式名前(VOICE_ID:speed)[VOICE]セリフ[/VOICE]Updated to capture emotions more deeply: （CORE v3.1準拠、HEART_VOICEで感情パラメータ自動生成）Example:ラム(2:1.05)[VOICE]こんにちはだっちゃ[/VOICE]フリガナ例（AI生成）：こんにちはだっちゃ（こん・にち・わ・だっ・ちゃ） – よりなめらかな発音を実現。 VOICE_ID Reference / VOICE_ID対応表 (Emotional Mapping)ID	Style	Emotional Focus
0	Narration	Neutral, descriptive
1	Energetic	High excitement, joy
2	Cute	Playful, affectionate
3	Calm / Adult	Composed, thoughtful
4	Cool	Confident, detached
5	Sexy	Seductive, intimate
6	Relaxed	Casual, easygoing
7	Fast	Urgent, tenseVOICE_IDは感情を捉え、VOICEVOXの感情的話し方を強化。speed/volumeで微調整。 Notes / 注意VOICEVOX must be running for emotional synthesis.Python server (ForbidIF_voice.py) must be running to handle TAM PY integration.Browser auto-play must be allowed.フリガナはAIにより自動生成され、なめらかな日本語発音をサポート（オプションで有効化）。


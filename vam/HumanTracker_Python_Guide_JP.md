# HumanTracker.py 説明書

## 1. 概要

`HumanTracker.py` は、Webカメラから顔と手を読み取り、VaM側へトラッキング情報を送るPythonプログラムです。

主な役割:

- 顔の向き、視線、表情用の情報を取得する
- 手の位置、手の開閉、手首角度を取得する
- VaM側プラグインへUDPで情報を送信する
- カメラ画面を表示し、必要に応じて非表示にする

VaM側で実際にAtomを動かす処理は、主に `HumanGrabTracker.cs` などのC#プラグインが担当します。

## 2. Python 3.11 の準備

このツールは Python 3.11 での実行を前提にしています。

起動時は `py -3.11` を使用してください。

```bat
py -3.11 HumanTracker.py
```

Python 3.11 が入っていない場合は、以下からダウンロードしてインストールします。

Python 3.11.9:

```text
https://www.python.org/downloads/release/python-3119/
```

Python公式ダウンロードページ:

```text
https://www.python.org/downloads/
```

インストール後、以下で確認できます。

```bat
py -3.11 --version
```

`Python 3.11.x` と表示されればOKです。

## 3. MediaPipe taskファイルの準備

顔と手のトラッキングには、MediaPipeのtaskファイルが必要です。

必要なファイル:

```text
hand_landmarker.task
face_landmarker.task
```

`HumanTracker.py` と同じフォルダに置くのがおすすめです。

例:

```text
Custom/Scripts/VAMT/hand_landmarker.task
Custom/Scripts/VAMT/face_landmarker.task
```

ダウンロード先:

```text
https://storage.googleapis.com/mediapipe-models/hand_landmarker/hand_landmarker/float16/1/hand_landmarker.task
```

```text
https://storage.googleapis.com/mediapipe-models/face_landmarker/face_landmarker/float16/latest/face_landmarker.task
```

ファイル名は必ず以下のままにしてください。

```text
hand_landmarker.task
face_landmarker.task
```

## 4. 起動方法

起動用bat、または以下のコマンドで起動します。

```bat
py -3.11 HumanTracker.py
```

Webカメラ画面が表示され、顔と手の認識が開始されます。

## 5. 画面の表示/非表示

Python側のカメラ画面は、`H` キーで表示/非表示を切り替えできます。

```text
H : カメラ画面の表示/非表示
```

VaM操作中に画面が邪魔な場合は、`H` で隠して使用します。

## 6. 実行時の流れ

1. `HumanTracker.py` を `py -3.11` で起動します。
2. Webカメラで顔と手が認識されていることを確認します。
3. 必要なら `H` キーでPython画面を非表示にします。
4. VaMを起動します。
5. VaM側で `HumanGrabTracker.cs` などのプラグインをONにします。
6. 手を動かして、VaM側のHand Atomが追従するか確認します。

## 7. 調整のポイント

### 7.1 手の開閉が合わない

手の開閉判定は、Python側で `HAND_OPEN` / `HAND_HALF` / `HAND_CLOSED` として送信されます。

VaM側では、現在 `HAND_HALF` は `HAND_CLOSED` 扱いにしています。

手を開いているのに閉じ判定になる場合や、閉じているのに開き判定になる場合は、Python側のしきい値を調整します。

### 7.2 起動が遅い

MediaPipeのモデル読み込みがあるため、起動時に少し時間がかかります。

`hand_landmarker.task` と `face_landmarker.task` が存在しない、またはパスが違う場合も起動が遅くなったり失敗したりします。

## 8. トラブル時

### 8.1 Pythonが起動しない

確認すること:

- Python 3.11で起動しているか
- `py -3.11 --version` が通るか
- 必要なPythonライブラリが入っているか
- `hand_landmarker.task` / `face_landmarker.task` があるか

### 8.2 Webカメラが映らない

確認すること:

- 他のアプリがWebカメラを使用していないか
- Windows側でカメラ使用許可がONか
- カメラ番号が合っているか

### 8.3 VaM側が反応しない

確認すること:

- `HumanTracker.py` が起動しているか
- VaM側プラグインがONか
- UDPポートがVaM側と一致しているか
- Windowsファイアウォールで止まっていないか

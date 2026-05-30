# HumanGrabTracker.cs 説明書

## 1. 概要

`HumanGrabTracker.cs` は、VaM側で手のトラッキング、Body Pull、Cloth Grabを行うC#プラグインです。

主な役割:

- Python側から受け取った手の位置でHand Atomを動かす
- 手を閉じた時に、体の部位を掴んで引っ張る
- 服用の `ClothGrabSphere` を使って服を引っ張る
- 掴み解除時にIKコントロールを補正する

Python側のWebカメラ処理は `HumanTracker.py` が担当します。

## 2. 配置

VaMのスクリプトフォルダへ、以下のファイルを配置します。

```text
Custom/Scripts/VAMT/HumanGrabTracker.cs
```

対象のPerson AtomにPluginとして追加して使用します。

## 3. 基本操作

### 3.1 プラグイン追加

VaMで対象のPerson Atomを選択し、Pluginとして以下を追加します。

```text
HumanGrabTracker.cs
```

### 3.2 Hand Atomの選択

設定画面で、左右の手に使用するAtomを選択します。

```text
Left Hand Atom
Right Hand Atom
```

注意:

一覧に表示されるのは、Atom名に `hand` が含まれているものだけです。

使用したいAtom名には `Hand` または `hand` を含めてください。

例:

```text
LeftHandEmpty
RightHandEmpty
MyLeftHand
MyRightHand
```

一覧に出ない場合は、Atom名を確認してから以下を押します。

```text
Refresh Hand Atom List
```

### 3.3 Hand TrackingをONにする

設定画面で以下をONにします。

```text
Enable Hand Tracking
```

ONにすると、Pythonから受け取った手の位置に合わせて、VaM内のHand Atomが動きます。

OFFにすると、Hand Atomのカメラ追従を外します。

## 4. Body Pull

手を閉じた状態で、対象部位に近づけるとBody Pullが発動します。

対象:

```text
Nipple
Hand
Knee
Foot
Head
Hip
```

Pull系チェックは基本ONのまま使う想定です。

```text
Enable Body Pull
Pull Nipple
Pull Hand
Pull Knee
Pull Foot
Pull Head
Pull Hip
```

Nippleは他の部位より強めに引っ張るようにしています。

## 5. Cloth Grab

服を引っ張るための機能です。

手が `HAND_OPEN` から `HAND_CLOSED` に変わった瞬間に、服用の `ClothGrabSphere` が手の前に出ます。

使用Atom:

```text
ClothGrab_L
ClothGrab_R
```

これらは自動生成されます。

主な設定:

```text
Enable Cloth Grab
Cloth Grab Forward Offset
```

`Cloth Grab Forward Offset` は、服用ColliderをHand Atomのどれくらい前に出すかを調整します。

服を離す時は、手を開いた状態を少し維持します。

服側のCloth Physics / Collisionが有効になっている必要があります。

## 6. 実行の流れ

1. Python側で `HumanTracker.py` を起動します。
2. VaMを起動します。
3. Person Atomに `HumanGrabTracker.cs` を追加します。
4. `Left Hand Atom` / `Right Hand Atom` を選択します。
5. `Enable Hand Tracking` をONにします。
6. 手を動かして、VaM内のHand Atomが追従するか確認します。
7. 手を閉じて、Body PullやCloth Grabを試します。

## 7. 主な設定

### 7.1 手の左右位置

```text
Hand X Scale
```

Webカメラ上の左右移動幅と、VaM内の左右移動幅を合わせます。

### 7.2 手の奥行き

```text
Hand Depth Scale
Hand Depth Dead Zone
Hand Depth Limit
Left Depth Bias
Right Depth Bias
Invert Hand Depth
```

手の前後位置が合わない場合に調整します。

### 7.3 掴み判定

```text
Body Pull Distance
Grab Forward Offset
```

`Body Pull Distance` は掴める範囲です。

小さいほど誤爆しにくく、大きいほど掴みやすくなります。

`Grab Forward Offset` は、掴み判定に使う位置をHand Atomより前へずらす量です。

### 7.4 服の掴み位置

```text
Cloth Grab Forward Offset
```

服用Colliderの前後位置を調整します。

### 7.5 手首のX回転

```text
Enable Hand X Rotation
Hand X Neutral Angle
Hand X Rotation Scale
Hand X Rotation Offset
```

Python側から受け取った手首角度を、Hand AtomのX回転へ反映します。

自然な向きにならない場合は、`Hand X Neutral Angle` と `Hand X Rotation Offset` を調整します。

## 8. IK補正

掴み解除時に、IKコントロールが表示上の体から離れることがあります。

このプラグインでは、掴み解除時に対象部位のIKを表示上の体へ寄せる補正を行います。

手動で補正したい場合は、以下を使用します。

```text
Reset Pull Targets
```

対象:

```text
Hand
Knee
Foot
Head
Hip
```

## 9. トラブル時

### 9.1 手が動かない

確認すること:

- `HumanTracker.py` が起動しているか
- `Enable Hand Tracking` がONか
- `Left Hand Atom` / `Right Hand Atom` が選択されているか
- Hand Atom名に `hand` が含まれているか

### 9.2 Hand Atomが一覧に出ない

確認すること:

- Atom名に `hand` が含まれているか
- `Refresh Hand Atom List` を押したか

### 9.3 掴めない

確認すること:

- 手が `HAND_CLOSED` として認識されているか
- `Body Pull Distance` が小さすぎないか
- Pull系チェックがONか
- Hand Atomと対象部位が画面上で近いか

### 9.4 服が引っ張れない

確認すること:

- `Enable Cloth Grab` がONか
- `ClothGrab_L/R` が生成されているか
- 服側のCloth Physics / Collisionが有効か
- `Cloth Grab Forward Offset` が小さすぎないか

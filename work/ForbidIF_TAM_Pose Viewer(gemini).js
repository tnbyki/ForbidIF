// ==UserScript==
// @name         Gemini Pose Viewer Minimal
// @namespace    http://tampermonkey.net/
// @version      0.2
// @match        https://gemini.google.com/*
// @match        https://gemini.google.*/*
// @grant        none
// @run-at       document-idle
// ==/UserScript==

(function (){
 'use strict';

 const PANEL_ID = 'gemini-pose-min-panel';
 const CANVAS_ID = 'gemini-pose-min-canvas';
let rootState = {
  scenePosition: { x: 0, y: 0, z: 0 },
  sceneVelocity: { x: 0, y: 0, z: 0 },

  bodyForward: { x: 0, y: 0, z: 1 },
  bodyUp: { x: 0, y: 1, z: 0 },
  bodyRight: { x: 1, y: 0, z: 0 },

  headForward: { x: 0, y: 0, z: 1 },
  headUp: { x: 0, y: 1, z: 0 },
  headRight: { x: 1, y: 0, z: 0 },

  rightFootContact: false,
  leftFootContact: false,
  isGrounded: true,
  isJumping: false,

  groundedFrames: 0,
  airborneFrames: 0,

  // ここを修正！ 初期値0だと初回Vyが無限大になる
  _prevRHeelY: -1.35,   // ← Right Heelの初期Y（poseのデフォルト値）
  _prevLHeelY: -1.35    // ← Left Heelの初期Y
};

let isDragging = false;
let isPanning = false;
let isPanelDragging = false;
let lastMouseX = 0;
let lastMouseY = 0;
let stripPoseJsonOnSend = true;
let globalPointerHandlersInstalled = false;
let sendInterceptorInstalled = false;
let poseExtra = null;
let hoveredPointId = null;

const ACTION_BUTTONS = [
 { key: 'push',   icon: '👆', help: '押す' },
 { key: 'lick',   icon: '👅', help: '舐める' },
 { key: 'Massage',    icon: '🤲', help: '揉む' },
 { key: 'kiss',   icon: '💋', help: 'キス' },
 { key: 'touch',  icon: '🫳', help: '触る' },
 { key: 'pull',   icon: '🤏', help: '引き寄せる' },
 { key: 'insert', icon: '♂♀', help: '入れる' },
 { key: 'hold',   icon: '✊', help: '持つ' },
 { key: 'press',  icon: '✋', help: '押す' },
 { key: 'lift',  icon: '💪', help: '持ち上げる' },
 { key: 'open',   icon: '🦵', help: '広げる' },
 { key: 'see',    icon: '👀', help: '見る' },
 { key: 'tune',   icon: '🔎', help: '調べる' },
{ key: 'strength', icon: '🟢', help: '強さ切替' }
];
const ACTION_LABELS = {
 push: '押す',
 lick: '舐める',
 Massage: '揉む',
 kiss: 'キス',
 touch: '触る',
 pull: '引き寄せる',
 insert: '入れる',
 hold: '持つ',
 press: '押す',
 lift: '持ち上げる',
 open: '開く',
 see: '見る',
 tune: '調整',
 extra: '予備'
};

let activeActionKeys = new Set();
let strengthMode = 0;

const STRENGTH_PRESETS = [
 { icon: '🟢', label: '' },
 { icon: '🟡', label: 'つよく ' },
 { icon: '🔴', label: '激しく ' },
 { icon: '🔵', label: 'やさしく ' }
];

const INTERNAL_CAMERA = {
 yaw: 0,
 pitch: 0,
 roll: 0,
 scale: 1,
 tx: 0,
 ty: 0
};

let pose = {
 frame: 1,
 root: 'ID10',
 points: {
  ID00: { name: 'ROOT', x: 0.00, y: 0.00, z: 0.00 },
  ID01: { name: 'Lumbar', x: 0.00, y: 0.45, z: 0.00 },
  ID02: { name: 'Thoracic', x: 0.00, y: 0.95, z: 0.00 },
  ID03: { name: 'Cervical', x: 0.00, y: 1.25, z: 0.00 },
  ID04: { name: 'Skull', x: 0.00, y: 1.55, z: 0.02 },
  ID05: { name: 'Right Shoulder', x: 0.28, y: 0.95, z: 0.00 },
  ID06: { name: 'Left Shoulder', x: -0.28, y: 0.95, z: 0.00 },
  ID07: { name: 'Right Elbow', x: 0.58, y: 0.95, z: 0.00 },
  ID08: { name: 'Left Elbow', x: -0.58, y: 0.95, z: 0.00 },
  ID09: { name: 'Right Wrist', x: 0.88, y: 0.95, z: 0.00 },
  ID10: { name: 'Pelvis', x: 0.00, y: 0.00, z: 0.00 },
  ID11: { name: 'Left Wrist', x: -0.88, y: 0.95, z: 0.00 },
  ID12: { name: 'Right Hip', x: 0.16, y: -0.05, z: 0.00 },
  ID13: { name: 'Left Hip', x: -0.16, y: -0.05, z: 0.00 },
  ID14: { name: 'Right Knee', x: 0.16, y: -0.72, z: 0.00 },
  ID15: { name: 'Left Knee', x: -0.16, y: -0.72, z: 0.00 },
  ID16: { name: 'Right Heel', x: 0.16, y: -1.35, z: 0.00 },
  ID17: { name: 'Left Heel', x: -0.16, y: -1.35, z: 0.00 },
  ID18: { name: 'Mouth', x: 0.00, y: 1.38, z: 0.10 },
  ID19: { name: 'Right Nipple', x: 0.15, y: 0.78, z: 0.12 },
  ID20: { name: 'Left Nipple', x: -0.15, y: 0.78, z: 0.12 },
  ID21: { name: 'Genital', x: 0.00, y: -0.18, z: 0.10 },
  ID22: { name: 'Anus', x: 0.00, y: -0.12, z: -0.10 },
  ID23: { name: 'Right Outer', x: 0.12, y: 0.72, z: 0.16 },
  ID24: { name: 'Left Outer', x: -0.12, y: 0.72, z: 0.16 },
  ID25: { name: 'Right Lower', x: 0.10, y: 0.66, z: 0.14 },
  ID26: { name: 'Left Lower', x: -0.10, y: 0.66, z: 0.14 },
  ID27: { name: 'Bust Center', x: 0.00, y: 0.76, z: 0.14 }
 }
};
let actionHighlights = [];

 let lastPoseBlock = '';
 let autoSyncObserverStarted = false;
 let autoSyncTimer = null;
 let removePoseTimer = null;

let forceFrontView = false;
let poseSceneMeta = null;
let fixedFitScale = null;
let lastPoseMetaBlock = '';
let lastPoseExtraBlock = '';
let lastProjectedPoints = null;
let selectedPointId = null;
let mouseDownButton = -1;
let dragStartX = 0;
let dragStartY = 0;
let didDragSinceMouseDown = false;
let pendingSelectPointId = null;
let pendingSelectShift = false;
const POINT_LABELS = {
 ID01: 'HEAD',
 ID02: 'CHEST',
 ID03: 'NECK',
 ID04: 'HEAD_TOP',

 ID05: 'SHOULDR',
 ID06: 'SHOULDL',

 ID07: 'ARMR',
 ID08: 'ARML',

 ID09: 'HANDR',
 ID11: 'HANDL',

 ID10: 'PELVIS',

 ID12: 'HIPR',
 ID13: 'HIPL',

 ID14: 'KNEER',
 ID15: 'KNEEL',

 ID16: 'FOOTR',
 ID17: 'FOOTL',

 ID18: 'MOUTH',

 ID19: 'BSTR',
 ID20: 'BSTL',
 ID27: 'BUST',

 ID21: 'GENITAL',
 ID22: 'ANUS'
};
function getAppearanceColors(extra) {
  const appearance = extra?.appearance || {};

  const hairColor =
    appearance.hairColor ?? extra?.hairColor ?? "#2b2b2b";

  const skinColor =
    appearance.skinColor ?? extra?.skinColor ?? "#d9a07a";

  return { hairColor, skinColor };
}
function toggleActionButton(key){
 if(key === 'strength'){
  strengthMode = (strengthMode + 1) % STRENGTH_PRESETS.length;
  updateActionButtonsUI();
  setStatus(`status: strength ${STRENGTH_PRESETS[strengthMode].icon}`);
  return;
 }

 if(activeActionKeys.has(key)){
  activeActionKeys.delete(key);
 }else{
  activeActionKeys.add(key);
 }

 updateActionButtonsUI();
}
function updateActionButtonsUI(){
 const root = document.getElementById('pose-min-action-row');
 if(!root) return;

 const buttons = root.querySelectorAll('button[data-action-key]');

 for(const btn of buttons){
  const key = btn.getAttribute('data-action-key');

  const isActive = (key === 'strength')
   ? strengthMode !== 0
   : activeActionKeys.has(key);

  if(key === 'strength'){
   btn.textContent = STRENGTH_PRESETS[strengthMode].icon;
  }

  if(isActive){
   btn.style.background = '#00e0ff';
   btn.style.color = '#000';
   btn.style.border = '1px solid #00e0ff';
   btn.style.transform = 'translateY(1px) scale(0.96)';
   btn.style.boxShadow = 'inset 0 2px 6px rgba(0,0,0,0.5)';
  }else{
   btn.style.background = '#111';
   btn.style.color = '#fff';
   btn.style.border = '1px solid #444';
   btn.style.transform = 'none';
   btn.style.boxShadow = 'none';
  }
 }
}

 function sanitizePoseJsonText(text){
  if(!text) return '';
  return text
   .replace(/[\u200B-\u200D\uFEFF]/g, '')
   .replace(/\r/g, '')
   .replace(/[“”]/g, '"')
   .replace(/[‘’]/g, "'")
   .replace(/,\s*([}\]])/g, '$1')
   .trim();
 }

 function extractLatestPoseJsonBlock(text){
  if(!text) return null;

  const matches = [...text.matchAll(/<POSE_JSON_START>([\s\S]*?)<POSE_JSON_END>/g)];
  if(!matches.length) return null;

  let block = matches[matches.length - 1][1].trim();

  const jsonStart = block.indexOf('{');
  const jsonEnd = block.lastIndexOf('}');

  if(jsonStart === -1 || jsonEnd === -1 || jsonEnd < jsonStart) return null;

  block = block.slice(jsonStart, jsonEnd + 1);
  return sanitizePoseJsonText(block);
 }

 function extractLatestPoseSceneMetaBlock(text){
  if(!text) return null;

  const matches = [...text.matchAll(/<POSE_SCENE_META_START>([\s\S]*?)<POSE_SCENE_META_END>/g)];
  if(!matches.length) return null;

  let block = matches[matches.length - 1][1].trim();

  const jsonStart = block.indexOf('{');
  const jsonEnd = block.lastIndexOf('}');

  if(jsonStart === -1 || jsonEnd === -1 || jsonEnd < jsonStart) return null;

  block = block.slice(jsonStart, jsonEnd + 1);
  return sanitizePoseJsonText(block);
 }
  function hasPoseSceneMetaBlock(text){
 return text.includes('<POSE_SCENE_META_START>') &&
     text.includes('<POSE_SCENE_META_END>');
}
function extractLatestPoseExtraBlock(text){
 if(!text) return null;

 const matches = [...text.matchAll(/<POSE_EXTRA>([\s\S]*?)<\/POSE_EXTRA>/g)];
 if(!matches.length) return null;

 let block = matches[matches.length - 1][1].trim();

 const jsonStart = block.indexOf('{');
 const jsonEnd = block.lastIndexOf('}');

 if(jsonStart === -1 || jsonEnd === -1) return null;

 block = block.slice(jsonStart, jsonEnd + 1);
 return sanitizePoseJsonText(block);
}
 function updateSyncButtonByMeta(){
 const btn = document.getElementById('pose-min-sync-btn');
 if(!btn) return;

 const text = document.body?.textContent || '';

 if(hasPoseSceneMetaBlock(text)){
  btn.style.background = '#8b1a1a';
  btn.style.border = '1px solid #ff6b6b';
 }else{
  btn.style.background = '#111';
  btn.style.border = '1px solid #444';
 }
}
  setInterval(updateSyncButtonByMeta, 200);

function ensureCameraDefaults(){
 if(typeof INTERNAL_CAMERA.yaw !== 'number') INTERNAL_CAMERA.yaw = 0;
 if(typeof INTERNAL_CAMERA.pitch !== 'number') INTERNAL_CAMERA.pitch = 0;
 if(typeof INTERNAL_CAMERA.roll !== 'number') INTERNAL_CAMERA.roll = 0;
 if(typeof INTERNAL_CAMERA.scale !== 'number') INTERNAL_CAMERA.scale = 1;
 if(typeof INTERNAL_CAMERA.tx !== 'number') INTERNAL_CAMERA.tx = 0;
 if(typeof INTERNAL_CAMERA.ty !== 'number') INTERNAL_CAMERA.ty = 0;
}

function applyForcedViewIfNeeded(){
const hasScene =
 !!poseSceneMeta?.scene &&
 Object.keys(poseSceneMeta.scene).length > 0;

if(forceFrontView && hasScene){
 INTERNAL_CAMERA.yaw = 0;
 INTERNAL_CAMERA.pitch = 0;
}
}

function resetCamera(withDraw = true){
 INTERNAL_CAMERA.yaw = 0;
 INTERNAL_CAMERA.pitch = 0;
 INTERNAL_CAMERA.roll = 0;
 INTERNAL_CAMERA.scale = 1;
 INTERNAL_CAMERA.tx = 0;
 INTERNAL_CAMERA.ty = 0;

 if(withDraw){
  drawPose();
 }
}

function makeButton(label, onClick, id = ''){
 const btn = document.createElement('button');
 if(id) btn.id = id;
 btn.textContent = label;
 btn.style.flex = '1';
 btn.style.height = '28px';
 btn.style.border = '1px solid #444';
 btn.style.borderRadius = '6px';
 btn.style.background = '#111';
 btn.style.color = '#fff';
 btn.style.cursor = 'pointer';
 btn.style.fontSize = '11px';
 btn.addEventListener('click', onClick);
 return btn;
}

function setStatus(text){
 const el = document.getElementById('pose-min-status');
 if(el) el.textContent = text;
}

 function setForceFrontUI(v, withStatus = false){
  forceFrontView = !!v;

  const cb = document.getElementById('pose-min-force-front');
  if(cb) cb.checked = forceFrontView;

  if(withStatus){
   setStatus(`status: force front ${forceFrontView ? 'on' : 'off'}`);
  }
 }
function setSyncButtonLoading(isLoading){
 const btn = document.getElementById('pose-min-sync-btn');
 if(!btn) return;

 if(isLoading){
btn.style.background = '#1a2f66';
btn.style.border = '1px solid #2e4a99';
  btn.style.color = '#ffffff';
  btn.textContent = 'SYNC';
 }else{
  btn.style.background = '#111';
  btn.style.border = '1px solid #444';
  btn.style.color = '#ffffff';
  btn.textContent = 'SYNC';
 }
}
function rotatePoint(p, cam){
 let x = p.x;
 let y = p.y;
 let z = p.z;

 const yaw = cam.yaw || 0;
 const pitch = cam.pitch || 0;
 const roll = cam.roll || 0;

 const cy = Math.cos(yaw);
 const sy = Math.sin(yaw);
 const cp = Math.cos(pitch);
 const sp = Math.sin(pitch);
 const cr = Math.cos(roll);
 const sr = Math.sin(roll);

 const x1 = x * cy + z * sy;
 const z1 = -x * sy + z * cy;

 const y1 = y * cp - z1 * sp;
 const z2 = y * sp + z1 * cp;

 const x2 = x1 * cr - y1 * sr;
 const y2 = x1 * sr + y1 * cr;

 return { x: x2, y: y2, z: z2 };
}
function getViewCamera(cam){
 return {
  yaw: cam?.yaw || 0,
  pitch: cam?.pitch || 0,
  roll: cam?.roll || 0
 };
}

function getScenePositionOffset(){
 const sp =
  poseExtra?.scene?.position ||
  poseSceneMeta?.scene?.position;

 return {
  x: Number(sp?.x) || 0,
  y: Number(sp?.y) || 0,
  z: Number(sp?.z) || 0
 };
}

function getGroundOffset(){
 const go = poseExtra?.placement?.groundOffset;
 return {
  x: Number(go?.x) || 0,
  y: Number(go?.y) || 0,
  z: Number(go?.z) || 0
 };
}

function applyScenePositionToPoint(p){
 if(!p) return p;

 const ofs = getScenePositionOffset();
 const go = getGroundOffset();

 return {
  ...p,
  x: p.x + ofs.x - go.x,
  y: p.y + ofs.y - go.y,
  z: p.z + ofs.z - go.z
 };
}
function getPosePointsWithSceneOffset(points){
 const out = {};
 for(const id of Object.keys(points || {})){
  out[id] = applyScenePositionToPoint(points[id]);
 }
 return out;
}
function projectPoint(p){
 const r = rotatePoint(p, getViewCamera(INTERNAL_CAMERA));
 return {
  x: r.x,
  y: r.y
 };
}
 function computeLayout(points, width, height){
  ensureCameraDefaults();

  const projected = {};
  const ids = Object.keys(points || {});
  if(!ids.length) return projected;

  let minX = Infinity, maxX = -Infinity, minY = Infinity, maxY = -Infinity;

  for(const id of ids){
   const p = projectPoint(points[id]);
   projected[id] = p;
   minX = Math.min(minX, p.x);
   maxX = Math.max(maxX, p.x);
   minY = Math.min(minY, p.y);
   maxY = Math.max(maxY, p.y);
  }

  const spanX = Math.max(0.01, maxX - minX);
  const spanY = Math.max(0.01, maxY - minY);
  const padding = 22;

const autoFitScale = Math.min(
 (width - padding * 2) / spanX,
 (height - padding * 2) / spanY
);

if(fixedFitScale == null){
 fixedFitScale = autoFitScale;
}

const scale = fixedFitScale * (INTERNAL_CAMERA.scale || 1);

const centerX = (minX + maxX) / 2;
const centerY = (minY + maxY) / 2;
const tx = INTERNAL_CAMERA.tx || 0;
const ty = INTERNAL_CAMERA.ty || 0;

  for(const id of ids){
   const p = projected[id];
   projected[id] = {
    x: width / 2 + tx + (p.x - centerX) * scale,
    y: height / 2 + ty - (p.y - centerY) * scale
   };
  }

  return projected;
 }

 function getLayoutMetrics(points, width, height){
  ensureCameraDefaults();

  const ids = Object.keys(points || {});
  if(!ids.length){
   return { fitScale: 1, centerX: 0, centerY: 0 };
  }

  let minX = Infinity, maxX = -Infinity, minY = Infinity, maxY = -Infinity;

  for(const id of ids){
   const p = projectPoint(points[id]);
   minX = Math.min(minX, p.x);
   maxX = Math.max(maxX, p.x);
   minY = Math.min(minY, p.y);
   maxY = Math.max(maxY, p.y);
  }

  const spanX = Math.max(0.01, maxX - minX);
  const spanY = Math.max(0.01, maxY - minY);
  const padding = 22;

const autoFitScale = Math.min(
 (width - padding * 2) / spanX,
 (height - padding * 2) / spanY
);

if(fixedFitScale == null){
 fixedFitScale = autoFitScale;
}

return {
 fitScale: fixedFitScale,
 centerX: (minX + maxX) / 2,
 centerY: (minY + maxY) / 2
};

}
function getFacingClassFromCamera(cam){
 let a = cam?.yaw || 0;

 while(a > Math.PI) a -= Math.PI * 2;
 while(a < -Math.PI) a += Math.PI * 2;

 const deg = Math.abs(a) * 180 / Math.PI;

if(deg < 22) return 1;
if(deg < 55) return 2;
if(deg < 105) return 3;
if(deg < 145) return 4;
return 5;
}
function getHeadFacingFromPose(rawPoints, cam){
 const skull = rawPoints?.ID04;
 const neck = rawPoints?.ID03;
 const mouth = rawPoints?.ID18;

 if(!skull || !neck || !mouth){
const viewCam = getViewCamera(cam);

return {
 facing: getFacingClassFromCamera(viewCam),
 lr: Math.sin(viewCam.yaw || 0) >= 0 ? 1 : -1,
 frontness: 0.5,
 sideness: 0
};
 }

 const viewCam = getViewCamera(cam);

 const rs = rotatePoint(skull, viewCam);
 const rn = rotatePoint(neck, viewCam);
 const rm = rotatePoint(mouth, viewCam);

 const cx = (rs.x + rn.x) * 0.5;
 const cz = (rs.z + rn.z) * 0.5;

 const dz = rm.z - cz;

 const dx = rm.x - cx;

 const frontness = clamp((dz + 0.12) / 0.24, 0, 1);
 const backness = 1 - frontness;

 const sideness = clamp(Math.abs(dx) / 0.10, 0, 1);

 let facing = 3;

 if(frontness > 0.72 && sideness < 0.38){
  facing = 1;
 } else if(frontness > 0.52){
  facing = 2;
 } else if(backness > 0.72 && sideness < 0.38){
  facing = 5;
 } else if(backness > 0.52){
  facing = 4;
 }else{
  facing = 3;
 }

 return {
  facing,
  lr: dx >= 0 ? 1 : -1,
  frontness,
  sideness
 };
}
function vecSub(a, b){
 return {
  x: (a?.x || 0) - (b?.x || 0),
  y: (a?.y || 0) - (b?.y || 0),
  z: (a?.z || 0) - (b?.z || 0)
 };
}

function vecDot(a, b){
 return (a.x * b.x) + (a.y * b.y) + (a.z * b.z);
}

function vecCross(a, b){
 return {
  x: a.y * b.z - a.z * b.y,
  y: a.z * b.x - a.x * b.z,
  z: a.x * b.y - a.y * b.x
 };
}

function vecLen(v){
 return Math.hypot(v.x, v.y, v.z);
}

function vecNormalize(v){
 const len = vecLen(v) || 1;
 return {
  x: v.x / len,
  y: v.y / len,
  z: v.z / len
 };
}

function buildRootBasis(points){
 const pelvis = points?.ID10;
 const chest  = points?.ID02;
 const rHip   = points?.ID12;
 const lHip   = points?.ID13;
 const genital = points?.ID21;
 const anus = points?.ID22;

 if(!pelvis || !chest || !rHip || !lHip) return null;

 const rightRaw = vecSub(rHip, lHip);
 const upRaw = vecSub(chest, pelvis);

 let right = vecNormalize(rightRaw);
 let up = vecNormalize(upRaw);

 let forward = vecNormalize(vecCross(up, right));

 if(genital && anus){
  const frontHint = vecNormalize(vecSub(genital, anus));
  if(vecDot(forward, frontHint) < 0){
   forward = {
    x: -forward.x,
    y: -forward.y,
    z: -forward.z
   };
  }
 }

 right = vecNormalize(vecCross(forward, up));
 up = vecNormalize(vecCross(right, forward));

 return {
  origin: pelvis,
  right,
  up,
  forward
 };
}
function getRootFacingFromBasis(points, cam){
 const basis = buildRootBasis(points);
 if(!basis){
  return {
   facing: getFacingClassFromCamera(cam),
   score: 0,
   basis: null
  };
 }

 const viewCam = getViewCamera(cam);
 const f = rotatePoint(basis.forward, viewCam);
 const forwardZ = f.z;
 const sideX = Math.abs(f.x);

 let facing = 3;

 if(forwardZ > 0.55 && sideX < 0.45){
  facing = 1;
 } else if(forwardZ > 0.18){
  facing = 2;
 } else if(forwardZ < -0.55 && sideX < 0.45){
  facing = 5;
 } else if(forwardZ < -0.18){
  facing = 4;
 }else{
  facing = 3;
 }

 return {
  facing,
  score: forwardZ,
  basis
 };
}
function getFacingFromForwardVec(forward, cam){
 if(
  !forward ||
  !Number.isFinite(forward.x) ||
  !Number.isFinite(forward.y) ||
  !Number.isFinite(forward.z)
 ){
  return {
   facing: getFacingClassFromCamera(getViewCamera(cam)),
   score: 0
  };
 }

 const nf = vecNormalize(forward);
 const viewCam = getViewCamera(cam);
 const f = rotatePoint(nf, viewCam);

 const forwardZ = f.z;
 const sideX = Math.abs(f.x);

 let facing = 3;

 if(forwardZ > 0.55 && sideX < 0.45){
  facing = 1;
 } else if(forwardZ > 0.18){
  facing = 2;
 } else if(forwardZ < -0.55 && sideX < 0.45){
  facing = 5;
 } else if(forwardZ < -0.18){
  facing = 4;
 }else{
  facing = 3;
 }

 return {
  facing,
  score: forwardZ
 };
}

function getBodyFacingFromMetaOrPose(points, meta, cam){
 const metaForward = meta?.body?.forward;

 if(
  metaForward &&
  Number.isFinite(metaForward.x) &&
  Number.isFinite(metaForward.y) &&
  Number.isFinite(metaForward.z)
 ){
  const info = getFacingFromForwardVec(metaForward, cam);
  return {
   ...info,
   source: 'meta'
  };
 }

 const fallback = getRootFacingFromBasis(points, cam);
 return {
  ...fallback,
  source: 'pose'
 };
}
function getMetaBasis(metaPart, fallbackForward, fallbackUp){
 const f = metaPart?.forward;
 const u = metaPart?.up;

 const hasMetaForward =
  f &&
  Number.isFinite(f.x) &&
  Number.isFinite(f.y) &&
  Number.isFinite(f.z);

 const hasMetaUp =
  u &&
  Number.isFinite(u.x) &&
  Number.isFinite(u.y) &&
  Number.isFinite(u.z);

 let forward = hasMetaForward
  ? vecNormalize(f)
  : vecNormalize(fallbackForward || { x: 0, y: 0, z: 1 });

 let up = hasMetaUp
  ? vecNormalize(u)
  : vecNormalize(fallbackUp || { x: 0, y: 1, z: 0 });

 if(Math.abs(vecDot(forward, up)) > 0.98){
  up = Math.abs(forward.y) < 0.9
   ? { x: 0, y: 1, z: 0 }
   : { x: 0, y: 0, z: 1 };
 }

 let right = vecNormalize(vecCross(forward, up));

 if(vecLen(right) < 0.0001){
  right = { x: 1, y: 0, z: 0 };
 }

 up = vecNormalize(vecCross(right, forward));

 return { forward, up, right };
}

function drawPose(){
 const panel = getOrCreatePanel();
 const canvas = panel.querySelector('#' + CANVAS_ID);
 if(!canvas) return;

 const body = poseExtra?.body || {};
 const waistWidth = body.waistWidth ?? 1.0;

 const ctx = canvas.getContext('2d');
 if(!ctx) return;

 const clothes = poseExtra?.clothes || {};

// ===== 基本色（appearance対応） =====
const skinColor =
  poseExtra?.appearance?.skinColor ?? poseExtra?.skinColor ?? '#e6e6e6';

const hairColor =
  poseExtra?.appearance?.hairColor ?? poseExtra?.hairColor ?? '#8b5a2b';

// ===== ヘルパ =====
function hasColor(c){
  return !!c && c !== 'none';
}
function pickColor(...colors){
  for(const c of colors){
    if(hasColor(c)) return c;
  }
  return null;
}

// ===== 元色 =====
const outerTopColor = hasColor(clothes.outerTop) ? clothes.outerTop : null;
const innerTopColor = hasColor(clothes.innerTop) ? clothes.innerTop : null;
const braColor = hasColor(clothes.bra) ? clothes.bra : null;
const outerBottomColor = hasColor(clothes.outerBottom) ? clothes.outerBottom : null;
const pantiesColor = hasColor(clothes.panties) ? clothes.panties : null;

// ===== 顔 =====
const faceColorBase = shadeColor(skinColor, 6);

// ===== 上半身 =====
// ルール
// 1) outerTopあり + innerTopあり
//    肩=outerTop / 腕=outerTop / 胴体=innerTop / 胸(左右)=innerTop / 胸中央=innerTop
// 2) outerTopなし + innerTopあり
//    肩=肌 / 腕=肌 / 胴体=innerTop / 胸(左右)=innerTop / 胸中央=肌
// 3) outerTopなし + innerTopなし + braあり
//    肩=肌 / 腕=肌 / 胴体=肌 / 胸(左右)=bra / 胸中央=肌

const hasOuterTop = hasColor(outerTopColor);
const hasInnerTop = hasColor(innerTopColor);
const hasBra = hasColor(braColor);

// 肩
const shoulderColor =
  (hasOuterTop && hasInnerTop) ? outerTopColor : skinColor;

// 腕
const armColor =
  (hasOuterTop && hasInnerTop) ? outerTopColor : skinColor;

// 胴体
const torsoBaseColor =
  hasInnerTop ? innerTopColor : skinColor;

const torsoColor = shadeColor(torsoBaseColor, -3);

// 胸（左右）
const breastColor =
  hasInnerTop ? innerTopColor :
  hasBra ? braColor :
  skinColor;

// 胸中央
const breastCenterColor =
  (hasOuterTop && hasInnerTop) ? outerTopColor : skinColor;

// 手
const handColor = skinColor;

// ===== 下半身 =====
// 胴下部分 = outerBottom → 肌
// 腰ボール = panties → 白
// 腿 = 肌
// 足 = 肌

const lowerBodyColor = pickColor(
  outerBottomColor,
  skinColor
) || skinColor;

const legColor = skinColor;
const footColor = skinColor;
const waistCenterColor = pickColor(
  pantiesColor,
  '#ffffff'
) || '#ffffff';

// その他
const breastCColor = breastCenterColor;
const pelvisLineColor = 'rgba(255,179,199,0.18)';
 ctx.clearRect(0, 0, canvas.width, canvas.height);
 ctx.fillStyle = '#0b0b0b';
const bgColor = poseExtra?.bgColor || '#0b0b0b';

ctx.fillStyle = bgColor;
ctx.fillRect(0, 0, canvas.width, canvas.height);
const scenePoints = getPosePointsWithSceneOffset(pose.points || {});
drawWorldFloor(ctx, scenePoints, canvas.width, canvas.height);
try {
  drawExtraBoxes(ctx, scenePoints, canvas.width, canvas.height);
} catch (e) {
  console.error('[drawExtraBoxes error]', e);
}
const P = computeLayout(scenePoints, canvas.width, canvas.height);
 const head = P.ID04;
 const neck = P.ID03;
 const chest = P.ID02;
 const pelvis = P.ID10;
const hipR = P.ID12;
const hipL = P.ID13;

 const rShoulder = P.ID05;
 const lShoulder = P.ID06;
 const rElbow = P.ID07;
 const lElbow = P.ID08;
 const rWrist = P.ID09;
 const lWrist = P.ID11;

 const rHip = P.ID12;
 const lHip = P.ID13;
 const rKnee = P.ID14;
 const lKnee = P.ID15;
 const rHeel = P.ID16;
 const lHeel = P.ID17;

 const genital = P.ID21;
 const anus = P.ID22;

 if(!head || !neck || !chest || !pelvis || !rHip || !lHip){
  setStatus('status: draw skipped (missing key points)');
  return;
 }

 const { fitScale } = getLayoutMetrics(scenePoints, canvas.width, canvas.height);
 const currentScale = fitScale * (INTERNAL_CAMERA.scale || 1);

 const rawShoulderWidth = dist3D(scenePoints.ID05, scenePoints.ID06);
 const shoulderWidth = rawShoulderWidth * currentScale;

 const pitch = INTERNAL_CAMERA.pitch || 0;

const poseBasis = getMetaBasis(
 poseSceneMeta?.body,
 { x: 0, y: 0, z: 1 },
 { x: 0, y: 1, z: 0 }
);

 const bodyBasis = getMetaBasis(
  poseSceneMeta?.body,
  poseBasis.forward,
  poseBasis.up
 );

 const bodyFacingInfo = getBodyFacingFromMetaOrPose(scenePoints, poseSceneMeta, INTERNAL_CAMERA);
 const headFacingInfo = getHeadFacingFromMetaOrPose(scenePoints, poseSceneMeta, INTERNAL_CAMERA);

 const bodyFacing = bodyFacingInfo.facing;
 const headFacing = headFacingInfo.facing;

 const lookUpHeadShrink = lerp(1.0, 0.78, smoothstep01(((-pitch) - 0.20) / 0.60));
 const lowAngleStrengthForHead = smoothstep01(((-pitch) - 0.18) / 0.80);
 const depthHeadScale = lerp(1.0, 0.90, lowAngleStrengthForHead);

 const headRadius = Math.max(
  15,
  Math.min(72, (rawShoulderWidth * currentScale * 0.36 || 22) * lookUpHeadShrink * depthHeadScale)
 );

 const armWidth = Math.max(9, Math.min(28, shoulderWidth * 0.26 || 12));
 const legWidth = Math.max(8, Math.min(80, shoulderWidth * 0.24 || 11));
 const thighWidth = legWidth * 1.25;

 const view = getViewMetrics(scenePoints, INTERNAL_CAMERA, currentScale, bodyBasis);
 const sideStrength = view.sideStrength;

 const limbSideStrength = smoothstep01((sideStrength - 0.45) / 0.45);
 const thighWidthDraw = thighWidth * lerp(1.0, 1.75, limbSideStrength);
 const legWidthDraw = legWidth * lerp(1.0, 1.55, limbSideStrength);
 const armWidthDraw = armWidth * lerp(1.0, 1.45, limbSideStrength);

 const pitchForward = smoothstep01((pitch - 0.18) / 0.75);
 const shoulderLift = Math.max(6, headRadius * 0.22);
 const shoulderSlope = Math.max(2, shoulderWidth * 0.10);
 const shoulderInward = shoulderWidth * 0.05 * pitchForward;
 const shoulderDropForward = headRadius * 0.04 * pitchForward;

 const rShoulderDraw = rShoulder ? {
  x: rShoulder.x - shoulderInward,
  y: rShoulder.y - shoulderLift + shoulderSlope + shoulderDropForward
 } : null;

 const lShoulderDraw = lShoulder ? {
  x: lShoulder.x + shoulderInward,
  y: lShoulder.y - shoulderLift + shoulderSlope + shoulderDropForward
 } : null;

 const shoulderMid = midpoint(rShoulderDraw, lShoulderDraw);
 const shoulderNeckMid = mixPoint(shoulderMid, neck, 0.33);
  const neckTop2D = neck;

 const neckBottom2D = (rShoulderDraw && lShoulderDraw)
  ? {
    x: (rShoulderDraw.x + lShoulderDraw.x) * 0.5,
    y: (rShoulderDraw.y + lShoulderDraw.y) * 0.5
   }
  : null;

 const neckWidth = Math.max(16, headRadius * 0.32);

function drawHeadBlock(){
 const skullTop = scenePoints['ID04'];
 const neck = scenePoints['ID03'];
 if(!skullTop || !neck) return;

const hairColor =
  poseExtra?.appearance?.hairColor ?? poseExtra?.hairColor ?? '#8b5a2b';

 const metaHead = poseSceneMeta?.head;
 let f = metaHead?.forward || { x: 0, y: 0, z: 1 };

 const normalize = (v) => {
  const l = Math.hypot(v.x, v.y, v.z) || 1;
  return { x: v.x / l, y: v.y / l, z: v.z / l };
 };

 f = normalize(f);

 const headCenter = {
  x: (skullTop.x + neck.x) * 0.5,
  y: (skullTop.y + neck.y) * 0.5,
  z: (skullTop.z + neck.z) * 0.5
 };

 let neckDir = {
  x: neck.x - headCenter.x,
  y: neck.y - headCenter.y,
  z: neck.z - headCenter.z
 };
 neckDir = normalize(neckDir);

 const mouthForwardOffset = 0.10;
 const mouthNeckOffset = 0.093;

 const mouthPoint = {
  x: headCenter.x + f.x * mouthForwardOffset + neckDir.x * mouthNeckOffset,
  y: headCenter.y + f.y * mouthForwardOffset + neckDir.y * mouthNeckOffset,
  z: headCenter.z + f.z * mouthForwardOffset + neckDir.z * mouthNeckOffset
 };

 const projected = computeLayout(
  {
   ...scenePoints,
   __HEAD_CENTER__: headCenter,
   __MOUTH_POINT__: mouthPoint
  },
  canvas.width,
  canvas.height
 );

 const head2D = projected.__HEAD_CENTER__;
 const mouth2D = projected.__MOUTH_POINT__;
 if(!head2D || !mouth2D) return;

 const nippleL = P.ID20;
 const nippleR = P.ID19;
 if(!nippleL || !nippleR) return;

 const cam = getViewCamera(INTERNAL_CAMERA);
 const headR = rotatePoint(headCenter, cam);
 const mouthR = rotatePoint(mouthPoint, cam);

 const headRad = headRadius * 1.15;
 const faceRad = headRad * 0.75;

 const mouthFront = mouthR.z > headR.z;

 const earOffsetX = faceRad * 1.0;
 const earOffsetBack = faceRad * 1.00;
 const earOffsetY = faceRad * -0.22;

 const earL = {
  x: mouth2D.x - earOffsetX,
  y: mouth2D.y - earOffsetBack - earOffsetY
 };

 const earR = {
  x: mouth2D.x + earOffsetX,
  y: mouth2D.y - earOffsetBack - earOffsetY
 };

 const hairEndL = {
  x: earL.x,
  y: earL.y + headRad * 0.9
};

const hairEndR = {
  x: earR.x,
  y: earR.y + headRad * 0.9
};
const faceColor = faceColorBase;

if(mouthFront){
 drawCircle(ctx, head2D, headRad, hairColor);
 drawCircle(ctx, mouth2D, faceRad, faceColor);
}else{
 drawCircle(ctx, mouth2D, faceRad, faceColor);
 drawCircle(ctx, head2D, headRad, hairColor);
}

drawCapsule(ctx, earL, hairEndL, 6, hairColor);
drawCapsule(ctx, earR, hairEndR, 6, hairColor);

}
function drawTorsoBlock(){

  // ===== 胴の形を一度パスとして作る =====
  ctx.save();
  ctx.beginPath();
drawBodySurface(ctx, P, scenePoints, bodyBasis, waistWidth, null);
  ctx.clip();


const splitCenter = {
  x: lerp(P.ID02.x, P.ID10.x, 0.7),
  y: lerp(P.ID02.y, P.ID10.y, 0.7)
};

let up2D = null;

if(bodyBasis?.up){
  const upView = rotatePoint(bodyBasis.up, getViewCamera(INTERNAL_CAMERA));
  const upLen = Math.hypot(upView.x, upView.y);

  if(upLen > 0.0001){
    up2D = {
      x: upView.x / upLen,
      y: -upView.y / upLen
    };
  }
}

// up が画面上で潰れる角度用の保険
if(!up2D){
  const chestToPelvisX = P.ID02.x - P.ID10.x;
  const chestToPelvisY = P.ID02.y - P.ID10.y;
  const fallbackLen = Math.hypot(chestToPelvisX, chestToPelvisY) || 1;

  up2D = {
    x: chestToPelvisX / fallbackLen,
    y: chestToPelvisY / fallbackLen
  };
}

const sideX = -up2D.y;
const sideY = up2D.x;

// 胸側
fillHalfPlane(
  ctx,
  splitCenter,
  sideX,
  sideY,
  up2D.x,
  up2D.y,
  torsoColor
);

// 骨盤側
fillHalfPlane(
  ctx,
  splitCenter,
  sideX,
  sideY,
  -up2D.x,
  -up2D.y,
  lowerBodyColor
);
  ctx.restore();


  // 肩
  drawShoulderPeak(
    ctx,
    lShoulderDraw,
    shoulderNeckMid,
    rShoulderDraw,
    armWidth * 0.72,
    shoulderColor
  );
}
    function fillHalfPlane(ctx, center, sideX, sideY, dirX, dirY, color){
  const FAR = 4000;

  const p1 = {
    x: center.x + sideX * FAR,
    y: center.y + sideY * FAR
  };
  const p2 = {
    x: center.x - sideX * FAR,
    y: center.y - sideY * FAR
  };
  const p3 = {
    x: p2.x + dirX * FAR,
    y: p2.y + dirY * FAR
  };
  const p4 = {
    x: p1.x + dirX * FAR,
    y: p1.y + dirY * FAR
  };

  ctx.beginPath();
  ctx.moveTo(p1.x, p1.y);
  ctx.lineTo(p2.x, p2.y);
  ctx.lineTo(p3.x, p3.y);
  ctx.lineTo(p4.x, p4.y);
  ctx.closePath();

  ctx.fillStyle = color;
  ctx.fill();
}

function drawBreastBlock(){
  const refs = {
    chest,
    pelvis,
    rShoulder: rShoulderDraw,
    lShoulder: lShoulderDraw,
    rHip,
    lHip
  };

  drawBreasts(
    ctx,
    P,
    scenePoints,
    breastColor,
    refs,
    INTERNAL_CAMERA,
    currentScale,
    view,
    bodyBasis,
    bodyFacing
  );

  drawBreastBridge(ctx, P, breastCColor, view);
}
function drawWaistCenterBall(ctx, pelvis, rHip, lHip, legWidth, waistWidth = 1.0, color = '#ffffff', alpha = 1){
  if(!pelvis || !rHip || !lHip) return;

  const hipMid = midpoint(rHip, lHip);
  if(!hipMid) return;

  const center = {
    x: lerp(hipMid.x, pelvis.x, 0.22),
    y: lerp(hipMid.y, pelvis.y, 0.35) + legWidth * 0.15
  };

const rx = legWidth * 0.9 * waistWidth;
  const ry = rx * 0.65;

  ctx.save();
  ctx.globalAlpha = alpha;
  drawEllipse(ctx, center, rx, ry, color);
  ctx.restore();
}
function drawLowPantiesPanel(
  ctx,
  pelvis,
  rHip,
  lHip,
  genital,
  anus,
  legWidth,
  color = '#ff99cc',
  bodyFacing = 3
){
  if(!pelvis || !rHip || !lHip) return;

  const hipMid = midpoint(rHip, lHip);
  if(!hipMid) return;

  const hipSpan = Math.abs(rHip.x - lHip.x);
  const halfW = Math.max(legWidth * 0.95, hipSpan * 0.42);

  const frontView = bodyFacing <= 2;
  const backView = bodyFacing >= 4;

  const anchor = frontView
    ? (genital || hipMid)
    : backView
      ? (anus || hipMid)
      : hipMid;

  const topY = lerp(hipMid.y, anchor.y, 0.16);
  const bottomY = lerp(hipMid.y, anchor.y, 0.78);

  const centerDrop = frontView
    ? legWidth * 0.34
    : backView
      ? legWidth * 0.20
      : legWidth * 0.26;

  const sideDrop = frontView
    ? legWidth * 0.10
    : backView
      ? legWidth * 0.04
      : legWidth * 0.07;

  const p1 = { x: hipMid.x - halfW,        y: topY };
  const p2 = { x: hipMid.x + halfW,        y: topY };
  const p3 = { x: hipMid.x + halfW * 0.78, y: bottomY - sideDrop };
  const p4 = { x: hipMid.x,                y: bottomY + centerDrop };
  const p5 = { x: hipMid.x - halfW * 0.78, y: bottomY - sideDrop };

  ctx.save();
  ctx.fillStyle = color;
  ctx.beginPath();
  ctx.moveTo(p1.x, p1.y);
  ctx.lineTo(p2.x, p2.y);
  ctx.lineTo(p3.x, p3.y);
  ctx.lineTo(p4.x, p4.y);
  ctx.lineTo(p5.x, p5.y);
  ctx.closePath();
  ctx.fill();
  ctx.restore();
}
function drawLegBlock(){
  const hipRadius = thighWidth * lerp(0.90, 0.94, sideStrength);

  const rKneeDraw = spreadSidePoint(rKnee, 1, legWidth * 1.20, view);
  const lKneeDraw = spreadSidePoint(lKnee, -1, legWidth * 1.20, view);
  const rHeelDraw = spreadSidePoint(rHeel, 1, legWidth * 1.60, view);
  const lHeelDraw = spreadSidePoint(lHeel, -1, legWidth * 1.60, view);

  const lowerLegColor = shadeColor(footColor, -4);
  const hipColor = skinColor;
  const thighColor = legColor;

  const thighWidthBoost = 1.3;
  const kneeTaper = 0.88;
  const calfBoost = 1.05;

  const waistBallAlpha = bodyFacing >= 4 ? 0.92 : 0.42;

drawWaistCenterBall(
  ctx,
  pelvis,
  rHip,
  lHip,
  legWidth,
  waistWidth,
  waistCenterColor,
  waistBallAlpha
);

  drawCircle(ctx, { x: rHip.x, y: rHip.y }, hipRadius * 0.95, hipColor);
  drawCircle(ctx, { x: lHip.x, y: lHip.y }, hipRadius * 0.95, hipColor);

  if(rKneeDraw && rHeelDraw){
    drawCapsule(
      ctx,
      rHip,
      rKneeDraw,
      thighWidthDraw * thighWidthBoost,
      thighColor
    );

    drawCapsule(
      ctx,
      rKneeDraw,
      rHeelDraw,
      legWidthDraw * kneeTaper * calfBoost,
      lowerLegColor
    );

    drawCircle(ctx, rHeelDraw, legWidthDraw * 0.62, footColor);
  }

  if(lKneeDraw && lHeelDraw){
    drawCapsule(
      ctx,
      lHip,
      lKneeDraw,
      thighWidthDraw * thighWidthBoost,
      thighColor
    );

    drawCapsule(
      ctx,
      lKneeDraw,
      lHeelDraw,
      legWidthDraw * kneeTaper * calfBoost,
      lowerLegColor
    );

    drawCircle(ctx, lHeelDraw, legWidthDraw * 0.62, footColor);
  }
}
function drawArmBlock(){
  const upperArmColor = armColor;
  const lowerArmColor = armColor;

  if(rShoulderDraw && rElbow){
    drawCapsule(ctx, rShoulderDraw, rElbow, armWidthDraw, upperArmColor);
  }
  if(rElbow && rWrist){
    drawCapsule(ctx, rElbow, rWrist, armWidthDraw * 0.92, lowerArmColor);
    drawCircle(ctx, rWrist, armWidthDraw * 0.62, handColor);
  }

  if(lShoulderDraw && lElbow){
    drawCapsule(ctx, lShoulderDraw, lElbow, armWidthDraw, upperArmColor);
  }
  if(lElbow && lWrist){
    drawCapsule(ctx, lElbow, lWrist, armWidthDraw * 0.92, lowerArmColor);
    drawCircle(ctx, lWrist, armWidthDraw * 0.62, handColor);
  }
}
function drawPantiesOverlay(){
  if (outerBottomColor && outerBottomColor !== 'none') return;
  if (!pantiesColor || pantiesColor === 'none') return;

  drawLowPantiesPanel(
    ctx,
    pelvis,
    rHip,
    lHip,
    genital,
    anus,
    legWidth,
    pantiesColor,
    bodyFacing
  );
}
 const drawParts = [];

 const headDepth =
  (() => {
   const ids = ['ID18', 'ID04', 'ID03'];
   let sum = 0;
   let count = 0;
   for(const id of ids){
    const p = scenePoints[id];
    if(!p) continue;
    const r = rotatePoint(p, getViewCamera(INTERNAL_CAMERA));
    sum += r.z;
    count++;
   }
   return count ? (sum / count) : 0;
  })();

const torsoDepth =
  (() => {
   const ids = ['ID02', 'ID01', 'ID10'];
   let sum = 0;
   let count = 0;
   for(const id of ids){
    const p = scenePoints[id];
    if(!p) continue;
    const r = rotatePoint(p, getViewCamera(INTERNAL_CAMERA));
    sum += r.z;
    count++;
   }
   return count ? (sum / count) : 0;
  })();
const breastDepth =
 (() => {
  const ids = ['ID19', 'ID20', 'ID27'];
  let maxZ = -Infinity;
  let sum = 0;
  let count = 0;

  for(const id of ids){
   const p = scenePoints[id];
   if(!p) continue;
   const r = rotatePoint(p, getViewCamera(INTERNAL_CAMERA));
   maxZ = Math.max(maxZ, r.z);
   sum += r.z;
   count++;
  }

  if(count === 0) return torsoDepth;


return maxZ;
 })();
 const armDepth =
  (() => {
   const ids = ['ID05', 'ID06', 'ID07', 'ID08', 'ID09', 'ID11'];
   let sum = 0;
   let count = 0;
   for(const id of ids){
    const p = scenePoints[id];
    if(!p) continue;
    const r = rotatePoint(p, getViewCamera(INTERNAL_CAMERA));
    sum += r.z;
    count++;
   }
   return count ? (sum / count) : 0;
  })();

 const legDepth =
  (() => {
   const ids = ['ID10', 'ID12', 'ID13', 'ID14', 'ID15', 'ID16', 'ID17', 'ID21', 'ID22'];
   let sum = 0;
   let count = 0;
   for(const id of ids){
    const p = scenePoints[id];
    if(!p) continue;
    const r = rotatePoint(p, getViewCamera(INTERNAL_CAMERA));
    sum += r.z;
    count++;
   }
   return count ? (sum / count) : 0;
  })();

const bodyForwardScore = bodyFacingInfo?.score ?? 0;

let breastDepthFinal = breastDepth;

// 前向きなら胸を少し前へ
if(bodyForwardScore > 0.15){
  const t = clamp((bodyForwardScore - 0.15) / 0.55, 0, 1);
  const bias = 0.02 * t;
  breastDepthFinal = Math.max(breastDepthFinal, torsoDepth + bias);
}

// 後ろ向きなら胸を少し後ろへ
if(bodyForwardScore < -0.10){
  const t = clamp(((-bodyForwardScore) - 0.10) / 0.55, 0, 1);
  const bias = 0.06 * t;
  breastDepthFinal = Math.min(breastDepthFinal, torsoDepth - bias);
}

drawParts.push({
 name: 'legs',
 depth: legDepth,
 draw: () => drawLegBlock()
});

drawParts.push({
  name: 'torso',
  depth: torsoDepth,
  draw: () => drawTorsoBlock()
});

drawParts.push({
  name: 'panties',
  depth: torsoDepth + 1,
  draw: () => drawPantiesOverlay()
});

drawParts.push({
 name: 'breasts',
 depth: breastDepthFinal,
 draw: () => drawBreastBlock()
});

drawParts.push({
  name: 'arms',
  depth: armDepth,
  draw: () => drawArmBlock()
});
drawParts.push({
 name: 'head',
 depth: headDepth,
 draw: () => {
const hairColor =
  poseExtra?.appearance?.hairColor ?? poseExtra?.hairColor ?? '#8b5a2b';

  drawEllipse(
   ctx,
   {
    x: head.x - headRadius * 0.70,
    y: head.y + headRadius * 1.20
   },
   headRadius * 0.55,
   headRadius * 1.6,
   hairColor
  );

  if(neckTop2D && neckBottom2D){
   drawNeckSimple(ctx, neckTop2D, neckBottom2D, neckWidth, handColor);
  }
  drawHeadBlock();
 }
});

 drawParts.sort((a, b) => a.depth - b.depth);

 for(const part of drawParts){
  part.draw();
 }

drawPointDots(ctx, P, scenePoints);
  drawActionHighlights(ctx, P);
drawPointLabels(ctx, P, scenePoints);
updateSelectedPointInfo(scenePoints);

setStatus(`status: ready | meta ${poseSceneMeta ? 'on' : 'off'} | body=${bodyFacing} head=${headFacing}`);
}

function drawBodySurface(ctx, P, rawPoints, bodyBasis, waistWidth = 1.0, fillColor = '#888'){
 if(!P || !rawPoints) return;

 const shoulderR = P.ID05;
 const shoulderL = P.ID06;
 const spine     = P.ID01;
 const pelvis    = P.ID10;
 const hipR      = P.ID12;
 const hipL      = P.ID13;

 if(!shoulderR || !shoulderL || !spine || !pelvis || !hipR || !hipL) return;

 const rawHipR = rawPoints.ID12;
 const rawHipL = rawPoints.ID13;

 const pelvisWidth3D = (rawHipR && rawHipL)
  ? dist3D(rawHipR, rawHipL)
  : 0.32;

 const panel = document.getElementById(PANEL_ID);
 const canvas = panel?.querySelector('#' + CANVAS_ID);
 const cw = canvas?.width || 500;
 const ch = canvas?.height || 500;

 const { fitScale } = getLayoutMetrics(rawPoints, cw, ch);
 const currentScale = fitScale * (INTERNAL_CAMERA.scale || 1);

const pelvisExpand = 1.0;
 const minWidthPx = 10;

 const width = Math.max(
  minWidthPx,
  pelvisWidth3D * pelvisExpand * currentScale
 );

const rawSpine = rawPoints.ID01;

const right = bodyBasis?.right || { x: 1, y: 0, z: 0 };

const halfWidth3D = 0.18;

const candA = {
 x: rawSpine.x + right.x * halfWidth3D,
 y: rawSpine.y + right.y * halfWidth3D,
 z: rawSpine.z + right.z * halfWidth3D
};

const candB = {
 x: rawSpine.x - right.x * halfWidth3D,
 y: rawSpine.y - right.y * halfWidth3D,
 z: rawSpine.z - right.z * halfWidth3D
};

const distAtoL = dist3D(candA, rawHipL);
const distAtoR = dist3D(candA, rawHipR);

const spineL3 = distAtoL <= distAtoR ? candA : candB;
const spineR3 = distAtoL <= distAtoR ? candB : candA;

const layoutMetrics = getLayoutMetrics(rawPoints, cw, ch);

let spineL = projectPointToScreen(spineL3, cw, ch, layoutMetrics);
let spineR = projectPointToScreen(spineR3, cw, ch, layoutMetrics);

// ===== 真横で胴の厚みが潰れるときの2D補正 =====
const torsoMinScreenWidth = 14;

const spineMid = {
 x: (spineL.x + spineR.x) * 0.5,
 y: (spineL.y + spineR.y) * 0.5
};

const torsoAxis = {
 x: pelvis.x - spineMid.x,
 y: pelvis.y - spineMid.y
};

const torsoAxisLen = Math.hypot(torsoAxis.x, torsoAxis.y) || 1;

// 胴の縦方向に直角な2D方向
let torsoSide2D = {
 x: -torsoAxis.y / torsoAxisLen,
 y:  torsoAxis.x / torsoAxisLen
};

// もし縦軸が潰れていたら保険で画面横方向
if(!Number.isFinite(torsoSide2D.x) || !Number.isFinite(torsoSide2D.y)){
 torsoSide2D = { x: 1, y: 0 };
}

const currentTorsoWidth = Math.hypot(
 spineR.x - spineL.x,
 spineR.y - spineL.y
);

if(currentTorsoWidth < torsoMinScreenWidth){
 const halfFix = torsoMinScreenWidth * 0.5;

 spineL = {
  x: spineMid.x - torsoSide2D.x * halfFix,
  y: spineMid.y - torsoSide2D.y * halfFix
 };

 spineR = {
  x: spineMid.x + torsoSide2D.x * halfFix,
  y: spineMid.y + torsoSide2D.y * halfFix
 };
}

const bodyHipWidthScale = 1.8 * waistWidth;
const bodyBottomExtend = 20;

const bodyHipR = {
 x: pelvis.x + (hipR.x - pelvis.x) * bodyHipWidthScale,
 y: hipR.y + bodyBottomExtend
};

const bodyHipL = {
 x: pelvis.x + (hipL.x - pelvis.x) * bodyHipWidthScale,
 y: hipL.y + bodyBottomExtend
};

 const ctrlShoulderL = {
  x: shoulderL.x + (spineL.x - shoulderL.x) * 0.5,
  y: shoulderL.y + (spineL.y - shoulderL.y) * 0.5
 };
 const hipCurveOut = 1.35;

 const ctrlHipL = {
  x: spineL.x + (bodyHipL.x - spineL.x) * hipCurveOut,
  y: spineL.y + (bodyHipL.y - spineL.y) * 0.5
 };

 const ctrlHipR = {
  x: spineR.x + (bodyHipR.x - spineR.x) * hipCurveOut,
  y: spineR.y + (bodyHipR.y - spineR.y) * 0.5
 };

 const ctrlShoulderR = {
  x: shoulderR.x + (spineR.x - shoulderR.x) * 0.5,
  y: shoulderR.y + (spineR.y - shoulderR.y) * 0.5
 };

 ctx.beginPath();

 ctx.moveTo(shoulderR.x, shoulderR.y);
 ctx.lineTo(shoulderL.x, shoulderL.y);

 ctx.quadraticCurveTo(
  ctrlShoulderL.x, ctrlShoulderL.y,
  spineL.x, spineL.y
 );

 ctx.quadraticCurveTo(
  ctrlHipL.x, ctrlHipL.y,
  bodyHipL.x, bodyHipL.y
 );

 ctx.lineTo(bodyHipR.x, bodyHipR.y);

 ctx.quadraticCurveTo(
  ctrlHipR.x, ctrlHipR.y,
  spineR.x, spineR.y
 );

 ctx.quadraticCurveTo(
  ctrlShoulderR.x, ctrlShoulderR.y,
  shoulderR.x, shoulderR.y
 );

 ctx.closePath();

if(fillColor){
  ctx.fillStyle = fillColor;
  ctx.fill();
}

ctx.fillStyle = 'red';

ctx.fillStyle = '#fff';
ctx.font = '10px monospace';

}


function projectPointToScreen(p, width, height, layoutMetrics){
 const r = projectPoint(p);

 const scale = layoutMetrics.fitScale * (INTERNAL_CAMERA.scale || 1);
 const tx = INTERNAL_CAMERA.tx || 0;
 const ty = INTERNAL_CAMERA.ty || 0;

 return {
  x: width / 2 + tx + (r.x - layoutMetrics.centerX) * scale,
  y: height / 2 + ty - (r.y - layoutMetrics.centerY) * scale
 };
}
function getGroundScreenTransform(posePoints, canvasW, canvasH){
  const { fitScale } = getLayoutMetrics(
    posePoints || {},
    canvasW,
    canvasH
  );

  const scale = fitScale * (INTERNAL_CAMERA.scale || 1);
  const tx = INTERNAL_CAMERA.tx || 0;
  const ty = INTERNAL_CAMERA.ty || 0;

  // worldの床基準点（scene.position）
  const scenePos = getScenePositionOffset();
  const groundOrigin = {
    x: scenePos.x,
    y: scenePos.y,
    z: scenePos.z
  };

  const r = projectPoint(groundOrigin);

  // 床が画面のやや下に来るように固定
  const screenGroundY = canvasH * 0.78 + ty;

  return {
    scale,
    originX: canvasW / 2 + tx - r.x * scale,
    originY: screenGroundY + r.y * scale
  };
}
async function syncPose(){
 setSyncButtonLoading(true);
 await new Promise(r => setTimeout(r, 300));

 const text = document.body?.textContent || '';
 const jsonText = extractLatestPoseJsonBlock(text);
 const metaText = extractLatestPoseSceneMetaBlock(text);
 const extraText = extractLatestPoseExtraBlock(text);

 let parsed = null;
 let parsedMeta = null;

if(extraText){
 try {
  poseExtra = JSON.parse(extraText);
 } catch (e){
  console.warn('[POSE_EXTRA parse error]', e);
 }
}

 if(!jsonText && !metaText){
  setSyncButtonLoading(false);
  return;
 }

 if(jsonText){
  try {
   parsed = JSON.parse(jsonText);
  } catch (e){
   setStatus('status: json parse error');
   setSyncButtonLoading(false);
   return;
  }
 }

 if(metaText){
  try {
   parsedMeta = JSON.parse(metaText);
  } catch (e){
   setStatus('status: meta parse error');
   setSyncButtonLoading(false);
   return;
  }
 }

 try {
  ensureCameraDefaults();

  if(parsedMeta){
   poseSceneMeta = parsedMeta;
  }

  if(parsed){
   if(forceFrontView){
    if(typeof parsed.camera?.scale === 'number') INTERNAL_CAMERA.scale = parsed.camera.scale;
    if(typeof parsed.camera?.tx === 'number') INTERNAL_CAMERA.tx = parsed.camera.tx;
    if(typeof parsed.camera?.ty === 'number') INTERNAL_CAMERA.ty = parsed.camera.ty;
   }

   const rawPose = {
    frame: parsed.frame ?? pose.frame,
    root: parsed.root ?? pose.root,
    points: { ...pose.points, ...(parsed.points || {}) }
   };

   const result = updateRoot(rawPose, rootState, 1 / 30);
   const normalized = result.out;
   rootState = result.nextState;

   pose.frame = normalized.frame ?? pose.frame;
   pose.root = normalized.root ?? pose.root;
   pose.points = normalized.points || pose.points;

   poseSceneMeta = {
    scene: parsedMeta?.scene || normalized.scene || {},
    body: parsedMeta?.body || normalized.body || {},
    head: parsedMeta?.head || normalized.head || {}
   };
  }

  const hasScene =
   !!poseSceneMeta?.scene &&
   Object.keys(poseSceneMeta.scene).length > 0;

  if(forceFrontView && hasScene){
   INTERNAL_CAMERA.yaw = 0;
   INTERNAL_CAMERA.pitch = 0;
   fixedFitScale = null;
  }

  drawPose();

  const p = pose.points?.ID14;
  setStatus(
   `status: sync ok knee=${p?.x?.toFixed?.(2)},${p?.y?.toFixed?.(2)},${p?.z?.toFixed?.(2)} grounded=${poseSceneMeta?.scene?.isGrounded ? '1' : '0'} jump=${poseSceneMeta?.scene?.isJumping ? '1' : '0'}`
  );
  setSyncButtonLoading(false);
 } catch (e){
  console.error('[POSE syncPose error]', e);
  setStatus('status: sync exception');
  setSyncButtonLoading(false);
 }
}

function checkAndAutoSyncPose(){
 const text = document.body?.textContent || '';
 const block = extractLatestPoseJsonBlock(text);
 const metaBlock = extractLatestPoseSceneMetaBlock(text);
 const extraBlock = extractLatestPoseExtraBlock(text);

 if(
  block === lastPoseBlock &&
  metaBlock === lastPoseMetaBlock &&
  extraBlock === lastPoseExtraBlock
 ){
  return;
 }

 if(isDragging || isPanning || isPanelDragging) return;

 lastPoseBlock = block || '';
 lastPoseMetaBlock = metaBlock || '';
 lastPoseExtraBlock = extraBlock || '';

 syncPose();
}
function startAutoSyncObserver(){
 if(autoSyncObserverStarted) return;
 if(!document.body) return;

 autoSyncObserverStarted = true;

 const observer = new MutationObserver(() => {
  clearTimeout(autoSyncTimer);
  autoSyncTimer = setTimeout(() => {
   checkAndAutoSyncPose();
  }, 250);

  clearTimeout(removePoseTimer);
  removePoseTimer = setTimeout(() => {
   if(!stripPoseJsonOnSend) return;

   const text = document.body?.textContent || '';
if(
 !text.includes('<POSE_JSON_START>') &&
 !text.includes('<POSE_SCENE_META_START>') &&
 !text.includes('<POSE_EXTRA>')
) return;

   removePoseJsonBlocksFromDom(document.body);
   setStatus('status: pose json removed from page');
  }, 700);
 });

const target = document.querySelector('main') || document.body;

observer.observe(target, {
 childList: true,
 subtree: true
});

 setTimeout(checkAndAutoSyncPose, 400);
}

function installSendInterceptor(){
 if(sendInterceptorInstalled) return;
 sendInterceptorInstalled = true;

 document.addEventListener('click', (e) => {
  const target = e.target;
  if(!(target instanceof Element)) return;

  const sendBtn = target.closest('button');
  if(!sendBtn) return;

  const label =
   (sendBtn.getAttribute('aria-label') || '') + ' ' +
   (sendBtn.textContent || '');

  if(/send|送信/i.test(label)){
   stripPoseJsonFromComposer();
  }
 }, true);

document.addEventListener('keydown', (e) => {
 if(e.key !== 'Enter') return;
 if(e.shiftKey || e.ctrlKey || e.altKey || e.metaKey) return;

 const composer = findGeminiComposer();
 if(!composer) return;

 stripPoseJsonFromComposer();
}, true);
}

 function getOrCreatePanel(){
  let panel = document.getElementById(PANEL_ID);
  if(panel) return panel;

  panel = document.createElement('div');
  panel.id = PANEL_ID;
  panel.style.position = 'fixed';
  panel.style.left = '8px';
  panel.style.top = '80px';
 panel.style.width = '280px';
  panel.style.background = 'rgba(20,20,20,0.96)';
  panel.style.color = '#ddd';
  panel.style.border = '1px solid #444';
  panel.style.borderRadius = '8px';
  panel.style.padding = '6px';
  panel.style.zIndex = '2147483647';
  panel.style.fontSize = '11px';
  panel.style.fontFamily = 'sans-serif';

const headerRow = document.createElement('div');
headerRow.style.display = 'flex';
headerRow.style.justifyContent = 'space-between';
headerRow.style.alignItems = 'center';
headerRow.style.marginBottom = '6px';
headerRow.style.cursor = 'move';
headerRow.style.userSelect = 'none';

  const title = document.createElement('div');
  title.textContent = 'POSE VIEWER';
  title.style.fontWeight = '700';
  title.style.color = '#fff';

  const toggleBtn = document.createElement('button');
  toggleBtn.textContent = 'CLOSE';
  toggleBtn.style.height = '24px';
  toggleBtn.style.border = '1px solid #444';
  toggleBtn.style.borderRadius = '6px';
  toggleBtn.style.background = '#111';
  toggleBtn.style.color = '#fff';
  toggleBtn.style.cursor = 'pointer';
  toggleBtn.style.fontSize = '11px';

  let isOpen = true;

  toggleBtn.addEventListener('click', () => {
   const body = document.getElementById('pose-min-body');
   if(!body) return;

   isOpen = !isOpen;
   body.style.display = isOpen ? 'block' : 'none';
   toggleBtn.textContent = isOpen ? 'CLOSE' : 'OPEN';
  });
headerRow.addEventListener('mousedown', (e) => {
 if(e.target instanceof HTMLElement && e.target.tagName === 'BUTTON') return;

 isPanelDragging = true;
 lastMouseX = e.clientX;
 lastMouseY = e.clientY;
 e.preventDefault();
});
  headerRow.appendChild(title);
  headerRow.appendChild(toggleBtn);
  panel.appendChild(headerRow);

  const bodyWrap = document.createElement('div');
  bodyWrap.id = 'pose-min-body';
  panel.appendChild(bodyWrap);

  const canvas = document.createElement('canvas');
  canvas.id = CANVAS_ID;
canvas.width = 500;
canvas.height = 500;
  canvas.style.background = '#0b0b0b';
  canvas.style.border = '1px solid #333';
  canvas.style.borderRadius = '6px';
  canvas.style.display = 'block';

canvas.addEventListener('mousedown', (e) => {
 e.preventDefault();

 lastMouseX = e.clientX;
 lastMouseY = e.clientY;
 dragStartX = e.clientX;
 dragStartY = e.clientY;
 didDragSinceMouseDown = false;
 mouseDownButton = e.button;

 const rect = canvas.getBoundingClientRect();
 const mx = e.clientX - rect.left;
 const my = e.clientY - rect.top;

 pendingSelectPointId = null;
 pendingSelectShift = !!e.shiftKey;

 if(e.button === 0 && !e.shiftKey && typeof findNearestPointId === 'function'){
  pendingSelectPointId = findNearestPointId(mx, my, lastProjectedPoints, 12);
  isDragging = true;
  isPanning = false;
  return;
 }

 if(e.button === 1 || (e.button === 0 && e.shiftKey)){
  isPanning = true;
  isDragging = false;
  return;
 }

 if(e.button === 0){
  isDragging = true;
  isPanning = false;
 }
});
  canvas.addEventListener('auxclick', (e) => {
   if(e.button === 1) e.preventDefault();
  });
 canvas.addEventListener('wheel', (e) => {
 e.preventDefault();
 e.stopPropagation();

 ensureCameraDefaults();

 const rect = canvas.getBoundingClientRect();
 const mx = e.clientX - rect.left;
 const my = e.clientY - rect.top;

 const wheelScenePoints = getPosePointsWithSceneOffset(pose.points || {});
 const { fitScale } = getLayoutMetrics(
  wheelScenePoints,
  canvas.width,
  canvas.height
 );

 const oldUserScale = INTERNAL_CAMERA.scale || 1;
 const oldScale = fitScale * oldUserScale;

 const zoomFactor = e.deltaY < 0 ? 1.12 : 1 / 1.12;
 let newUserScale = oldUserScale * zoomFactor;

 if(newUserScale < 0.2) newUserScale = 0.2;
 if(newUserScale > 5) newUserScale = 5;

 const newScale = fitScale * newUserScale;

 const tx = INTERNAL_CAMERA.tx || 0;
 const ty = INTERNAL_CAMERA.ty || 0;

 const localX = (mx - canvas.width / 2 - tx) / oldScale;
 const localY = -(my - canvas.height / 2 - ty) / oldScale;

 INTERNAL_CAMERA.scale = newUserScale;
 INTERNAL_CAMERA.tx = mx - canvas.width / 2 - localX * newScale;
 INTERNAL_CAMERA.ty = my - canvas.height / 2 + localY * newScale;

 drawPose();
}, { passive: false });
const actionRow = document.createElement('div');
actionRow.id = 'pose-min-action-row';
actionRow.style.display = 'flex';
actionRow.style.flexWrap = 'wrap';
actionRow.style.gap = '4px';
actionRow.style.marginTop = '6px';

for(const item of ACTION_BUTTONS){
 const btn = document.createElement('button');
 btn.type = 'button';
 btn.textContent = item.icon;
 btn.setAttribute('data-action-key', item.key);
btn.title = item.help || item.key;

 btn.style.width = '36px';
 btn.style.height = '28px';
 btn.style.borderRadius = '8px';
 btn.style.border = '1px solid #444';
 btn.style.background = '#111';
 btn.style.color = '#fff';
 btn.style.cursor = 'pointer';
 btn.style.fontSize = '16px';
 btn.style.lineHeight = '1';
 btn.style.padding = '0';
 btn.style.display = 'flex';
 btn.style.alignItems = 'center';
 btn.style.justifyContent = 'center';
 btn.style.transition = 'transform 0.06s ease, background 0.12s ease, border 0.12s ease, box-shadow 0.12s ease';

 btn.addEventListener('click', (e) => {
  e.preventDefault();
  e.stopPropagation();
  toggleActionButton(item.key);
 });

 btn.addEventListener('mouseenter', () => {
  if(!activeActionKeys.has(item.key)){
   btn.style.background = '#181818';
  }
 });

 btn.addEventListener('mouseleave', () => {
  if(!activeActionKeys.has(item.key)){
   btn.style.background = '#111';
  }
 });

 actionRow.appendChild(btn);
}
bodyWrap.appendChild(canvas);

bodyWrap.appendChild(actionRow);

const selectedInfo = document.createElement('div');
selectedInfo.id = 'pose-min-selected-info';
selectedInfo.style.marginTop = '6px';
selectedInfo.style.padding = '6px 8px';
selectedInfo.style.border = '1px solid #333';
selectedInfo.style.borderRadius = '6px';
selectedInfo.style.background = 'rgba(255,255,255,0.03)';
selectedInfo.style.color = '#bbb';
selectedInfo.style.lineHeight = '1.35';
selectedInfo.style.minHeight = '34px';
selectedInfo.textContent = 'selected: none';
bodyWrap.appendChild(selectedInfo);

const meta = document.createElement('div');
meta.id = 'pose-min-meta';
meta.style.marginTop = '6px';
meta.style.lineHeight = '1.3';
bodyWrap.appendChild(meta);

const row = document.createElement('div');
row.style.display = 'flex';
row.style.gap = '4px';
row.style.marginTop = '6px';

const viewOptionRow = document.createElement('label');
viewOptionRow.style.display = 'flex';
viewOptionRow.style.alignItems = 'center';
viewOptionRow.style.gap = '6px';
viewOptionRow.style.marginTop = '6px';
viewOptionRow.style.cursor = 'pointer';
viewOptionRow.style.userSelect = 'none';

const forceFrontCheckbox = document.createElement('input');
forceFrontCheckbox.id = 'pose-min-force-front';
forceFrontCheckbox.type = 'checkbox';
forceFrontCheckbox.checked = forceFrontView;

forceFrontCheckbox.addEventListener('change', () => {
 setForceFrontUI(forceFrontCheckbox.checked);

 if(forceFrontView){
  applyForcedViewIfNeeded();
 }

 drawPose();
 setStatus(`status: force front ${forceFrontView ? 'on' : 'off'}`);
});

const forceFrontText = document.createElement('span');
forceFrontText.textContent = 'FORCE VIEW';

viewOptionRow.appendChild(forceFrontCheckbox);
viewOptionRow.appendChild(forceFrontText);

const resetBtn = makeButton('RESET', () => resetCamera());
const syncBtn = makeButton('SYNC', () => syncPose(), 'pose-min-sync-btn');

const stripBtn = makeButton('', () => {
 stripPoseJsonOnSend = !stripPoseJsonOnSend;
 updateStripButtonLabel(stripBtn);
 setStatus(`status: strip ${stripPoseJsonOnSend ? 'on' : 'off'}`);
});

updateStripButtonLabel(stripBtn);

row.appendChild(resetBtn);
row.appendChild(syncBtn);
row.appendChild(stripBtn);
const camRow = document.createElement('div');
camRow.style.display = 'grid';
camRow.style.gridTemplateColumns = '1fr 1fr 1fr';
camRow.style.gap = '4px';
camRow.style.marginTop = '6px';

const leftBtn = makeButton('LEFT', () => {
 setForceFrontUI(false, true);
 resetCamera(false);
 INTERNAL_CAMERA.yaw = -Math.PI / 2;
 drawPose();
});

const topBtn = makeButton('TOP', () => {
 setForceFrontUI(false, true);
 resetCamera(false);
 INTERNAL_CAMERA.pitch = Math.PI * 0.495;
 drawPose();
});

const rightBtn = makeButton('RIGHT', () => {
 setForceFrontUI(false, true);
 resetCamera(false);
 INTERNAL_CAMERA.yaw = Math.PI / 2;
 drawPose();
});

const frontBtn = makeButton('FRONT', () => {
 setForceFrontUI(true, true);
 resetCamera(false);
 applyForcedViewIfNeeded();
 drawPose();
});

const backBtn = makeButton('BACK', () => {
 setForceFrontUI(false, true);
 resetCamera(false);
 INTERNAL_CAMERA.yaw = Math.PI;
 drawPose();
});

const bottomBtn = makeButton('BOTTOM', () => {
 setForceFrontUI(false, true);
 resetCamera(false);
 INTERNAL_CAMERA.pitch = -Math.PI * 0.495;
 drawPose();
});

camRow.appendChild(leftBtn);
camRow.appendChild(topBtn);
camRow.appendChild(rightBtn);
camRow.appendChild(frontBtn);
camRow.appendChild(backBtn);
camRow.appendChild(bottomBtn);
updateActionButtonsUI();

bodyWrap.appendChild(canvas);
bodyWrap.appendChild(actionRow);
bodyWrap.appendChild(camRow);
bodyWrap.appendChild(selectedInfo);
bodyWrap.appendChild(meta);
bodyWrap.appendChild(viewOptionRow);
bodyWrap.appendChild(row);

const status = document.createElement('div');
status.id = 'pose-min-status';
status.style.marginTop = '6px';
status.style.color = '#999';
status.textContent = 'status: ready';
bodyWrap.appendChild(status);

  document.body.appendChild(panel);
  return panel;
 }

function boot(){
 getOrCreatePanel();
 ensureCameraDefaults();
 drawPose();
 startAutoSyncObserver();
 installGlobalPointerHandlers();
 installSendInterceptor();
 setStatus('status: boot ok');
}

boot();
window.addEventListener('load', boot, { once: true });
function updateStripButtonLabel(btn){
 if(!btn) return;
 btn.textContent = stripPoseJsonOnSend ? 'STRIP: ON' : 'STRIP: OFF';
}
function installGlobalPointerHandlers(){
 if(globalPointerHandlersInstalled) return;
 globalPointerHandlersInstalled = true;

 document.addEventListener('mousemove', (e) => {
  const dx = e.clientX - lastMouseX;
  const dy = e.clientY - lastMouseY;

  const totalMove = Math.hypot(e.clientX - dragStartX, e.clientY - dragStartY);
  if(totalMove > 4){
   didDragSinceMouseDown = true;
  }

  lastMouseX = e.clientX;
  lastMouseY = e.clientY;

  if(!isDragging && !isPanning && !isPanelDragging && lastProjectedPoints){
   const panel = document.getElementById(PANEL_ID);
   const canvas = panel?.querySelector('#' + CANVAS_ID);

   if(canvas){
    const rect = canvas.getBoundingClientRect();
    const mx = e.clientX - rect.left;
    const my = e.clientY - rect.top;

    if(mx < 0 || my < 0 || mx > rect.width || my > rect.height){
     hoveredPointId = null;
    }else{
     hoveredPointId = findNearestPointId(mx, my, lastProjectedPoints, 10);
    }

    if(!selectedPointId){
     updateSelectedPointInfo(getPosePointsWithSceneOffset(pose.points || {}));
    }
   }
  }

  if(isPanelDragging){
   const panel = document.getElementById(PANEL_ID);
   if(!panel) return;

   const left = parseInt(panel.style.left || '0', 10);
   const top = parseInt(panel.style.top || '0', 10);
   panel.style.left = `${left + dx}px`;
   panel.style.top = `${top + dy}px`;
   return;
  }

  if(isDragging){
   if(forceFrontView){
    setForceFrontUI(false, true);
   }

   INTERNAL_CAMERA.yaw += dx * 0.01;
   INTERNAL_CAMERA.pitch = clampPitch(INTERNAL_CAMERA.pitch + dy * 0.01);
   drawPose();
   return;
  }

  if(isPanning){
   INTERNAL_CAMERA.tx += dx;
   INTERNAL_CAMERA.ty += dy;
   drawPose();
   return;
  }
 });

 document.addEventListener('mouseup', () => {
  if(
   mouseDownButton === 0 &&
   !pendingSelectShift &&
   !didDragSinceMouseDown
  ){
   if(pendingSelectPointId && pendingSelectPointId === selectedPointId){
    selectedPointId = null;
   }else{
    selectedPointId = pendingSelectPointId || null;
   }

   if(selectedPointId){
    setStatus(`status: selected ${selectedPointId} ${pose.points?.[selectedPointId]?.name || ''}`);

    actionHighlights.push({
     id: selectedPointId,
     time: Date.now()
    });

    drawPose();
    setTimeout(() => drawPose(), 100);
    setTimeout(() => drawPose(), 300);
    setTimeout(() => drawPose(), 600);
    setTimeout(() => drawPose(), 1000);
    setTimeout(() => drawPose(), 1600);
    setTimeout(() => drawPose(), 2200);
    setTimeout(() => drawPose(), 3000);

    sendSelectedAction(selectedPointId);
   }else{
    setStatus('status: selected none');
    drawPose();
   }
  }

  isDragging = false;
  isPanning = false;
  isPanelDragging = false;

  mouseDownButton = -1;
  pendingSelectPointId = null;
  pendingSelectShift = false;
  didDragSinceMouseDown = false;
 });
}
function findGeminiComposer(){
 const candidates = [
  document.querySelector('div[contenteditable="true"][role="textbox"]'),
  document.querySelector('rich-textarea div[contenteditable="true"]'),
  document.querySelector('div[contenteditable="true"]'),
  document.querySelector('[data-placeholder*="Reply to"]'),
  document.querySelector('[data-placeholder*="メッセージを入力"]'),
 ];

 for(const el of candidates){
  if(el && el.isContentEditable){
   return el;
  }
 }

 const rich = document.querySelector('rich-textarea');
 if(rich && rich.shadowRoot){
  const shadowEl = rich.shadowRoot.querySelector('div[contenteditable="true"]');
  if(shadowEl){
   return shadowEl;
  }
 }

 return null;
}
function buildActionText(pointId){
 const part = POINT_LABELS[pointId] || pointId;
 const verbs = [...activeActionKeys]
  .map(key => ACTION_LABELS[key])
  .filter(Boolean);

 const strength = STRENGTH_PRESETS[strengthMode]?.label || '';

 if(!verbs.length) return `${strength}${part}`.trim();
 return `${strength}${verbs.join(' ')} ${part}`.trim();
}

function findSendButton(){
 const buttons = [...document.querySelectorAll('button')];
 return buttons.find(btn => {
  const label =
   (btn.getAttribute('aria-label') || '') + ' ' +
   (btn.textContent || '');
  return /send|送信/i.test(label);
 }) || null;
}

function sendTextToGemini(text){
 const composer = findGeminiComposer();
 if(!composer){
  setStatus('status: composer not found');
  return false;
 }

 composer.focus();

 const plain = (composer.innerText || composer.textContent || '').trim();
 const nextText = plain ? `${plain}\n${text}` : text;

 composer.innerText = nextText;
 composer.dispatchEvent(new InputEvent('input', { bubbles: true }));

setTimeout(() => {
 let tries = 0;

 const timer = setInterval(() => {
  tries++;

  const sendBtn = findSendButton();
if(sendBtn && !sendBtn.disabled){
 clearInterval(timer);
 sendBtn.click();

 selectedPointId = null;
 drawPose();

 setStatus(`status: sent ${text}`);
 return;
}

  if(tries >= 10){
   clearInterval(timer);
   setStatus('status: send retry timeout');
  }
 }, 200);
}, 200);

 return true;
}

function sendSelectedAction(pointId){
 if(!pointId) return false;
 const text = buildActionText(pointId);
 return sendTextToGemini(text);
}
function stripPoseJsonFromComposer(){
 if(!stripPoseJsonOnSend) return false;

 const composer = findGeminiComposer();
 if(!composer){
  setStatus('status: strip failed (no composer)');
  return false;
 }

 const before = (composer.innerText || composer.textContent || '').trim();
 const after = removePoseJsonBlocks(before);

if(
 before === after ||
 (
  !before.includes('<POSE_JSON_START>') &&
  !before.includes('<POSE_SCENE_META_START>') &&
  !before.includes('<POSE_EXTRA>')
 )
){
 return false;
}

 try {
  composer.focus();

  const range = document.createRange();
  range.selectNodeContents(composer);
  const sel = window.getSelection();
  sel.removeAllRanges();
  sel.addRange(range);

  navigator.clipboard.writeText(after).then(() => {
   document.execCommand('paste');
  }).catch(() => {
   composer.innerText = after;
  });

  setTimeout(() => {
   composer.dispatchEvent(new InputEvent('input', { bubbles: true, cancelable: true }));
   composer.dispatchEvent(new InputEvent('beforeinput', { bubbles: true }));
   composer.dispatchEvent(new Event('change', { bubbles: true }));
   composer.dispatchEvent(new KeyboardEvent('input', { bubbles: true }));
  }, 30);

  setStatus('status: pose json stripped (enhanced)');
  return true;

 } catch (err){
  console.error('[POSE STRIP CRITICAL ERROR]', err);
  composer.innerText = after;
  setStatus('status: stripped (emergency fallback)');
  return true;
 }
}

function removePoseJsonBlocks(text){
 if(!text) return text;
 return text
  .replace(/<POSE_JSON_START>[\s\S]*?<POSE_JSON_END>\s*/g, '')
  .replace(/<POSE_SCENE_META_START>[\s\S]*?<POSE_SCENE_META_END>\s*/g, '')
  .replace(/<POSE_EXTRA>[\s\S]*?<\/POSE_EXTRA>\s*/g, '')
  .trim();
}
function removePoseJsonBlocksFromDom(root){
 if(!root) return;

 const walker = document.createTreeWalker(root, NodeFilter.SHOW_TEXT);
 const textNodes = [];
 let node;

 while((node = walker.nextNode())){
  textNodes.push(node);
 }

 for(const textNode of textNodes){
  const original = textNode.nodeValue;
const replaced = original
 .replace(/<POSE_JSON_START>[\s\S]*?<POSE_JSON_END>\s*/g, '')
 .replace(/<POSE_SCENE_META_START>[\s\S]*?<POSE_SCENE_META_END>\s*/g, '')
 .replace(/<POSE_EXTRA>[\s\S]*?<\/POSE_EXTRA>\s*/g, '');

  if(replaced !== original){
   textNode.nodeValue = replaced;
  }
 }
}
  function midpoint(a, b){
 if(!a || !b) return null;
 return {
  x: (a.x + b.x) / 2,
  y: (a.y + b.y) / 2
 };
}

function dist2D(a, b){
 if(!a || !b) return 0;
 return Math.hypot(b.x - a.x, b.y - a.y);
}
function dist3D(a, b){
 if(!a || !b) return 0;
 return Math.hypot(
  b.x - a.x,
  b.y - a.y,
  b.z - a.z
 );
}
function drawRoundLimb(ctx, a, b, width, color){
 if(!a || !b) return;
 ctx.strokeStyle = color;
 ctx.lineWidth = width;
 ctx.lineCap = 'round';
 ctx.lineJoin = 'round';
 ctx.beginPath();
 ctx.moveTo(a.x, a.y);
 ctx.lineTo(b.x, b.y);
 ctx.stroke();
}

function drawCircle(ctx, p, r, color){
 if(!p) return;
 ctx.fillStyle = color;
 ctx.beginPath();
 ctx.arc(p.x, p.y, r, 0, Math.PI * 2);
 ctx.fill();
}

function drawCapsule(ctx, a, b, width, color){
 if(!a || !b) return;

 const dx = b.x - a.x;
 const dy = b.y - a.y;
 const len = Math.hypot(dx, dy);

 if(len < 0.001){
  drawCircle(ctx, a, width / 2, color);
  return;
 }

 const nx = -dy / len;
 const ny = dx / len;
 const r = width / 2;

 ctx.fillStyle = color;
 ctx.beginPath();

 ctx.moveTo(a.x + nx * r, a.y + ny * r);
 ctx.lineTo(b.x + nx * r, b.y + ny * r);
 ctx.arc(b.x, b.y, r, Math.atan2(ny, nx), Math.atan2(-ny, -nx), false);
 ctx.lineTo(a.x - nx * r, a.y - ny * r);
 ctx.arc(a.x, a.y, r, Math.atan2(-ny, -nx), Math.atan2(ny, nx), false);

 ctx.closePath();
 ctx.fill();
}

function drawNeckSimple(ctx, topPoint, bottomPoint, neckWidth, color){
 if(!topPoint || !bottomPoint) return;
 drawCapsule(ctx, topPoint, bottomPoint, neckWidth, color);
}
function drawSoftTorso(ctx, chest, pelvis, shoulderR, shoulderL, hipR, hipL, color){
 if(!shoulderR || !shoulderL || !hipR || !hipL) return;

 const chestCenter = midpoint(shoulderR, shoulderL) || chest;
 const hipCenter = midpoint(hipR, hipL) || pelvis;
 if(!chestCenter || !hipCenter) return;

 const shoulderInset = 0.10;
 const hipInset = 0.04;
 const curveY = Math.max(10, dist2D(shoulderR, shoulderL) * 0.20);

 const topR = {
  x: shoulderR.x + (chestCenter.x - shoulderR.x) * shoulderInset,
  y: shoulderR.y + (chestCenter.y - shoulderR.y) * shoulderInset
 };
 const topL = {
  x: shoulderL.x + (chestCenter.x - shoulderL.x) * shoulderInset,
  y: shoulderL.y + (chestCenter.y - shoulderL.y) * shoulderInset
 };

const hipOut = 0.78;

const botR = {
 x: hipR.x + (hipCenter.x - hipR.x) * hipInset + (hipR.x - hipCenter.x) * hipOut,
 y: hipR.y + (hipCenter.y - hipR.y) * hipInset
};
const botL = {
 x: hipL.x + (hipCenter.x - hipL.x) * hipInset + (hipL.x - hipCenter.x) * hipOut,
 y: hipL.y + (hipCenter.y - hipL.y) * hipInset
};

 const waistY = lerp(chestCenter.y, hipCenter.y, 0.80);

 const topHalfWidth = Math.abs(topR.x - topL.x) * 0.5;
 const botHalfWidth = Math.abs(botR.x - botL.x) * 0.5;

 const waistHalfWidth = Math.min(topHalfWidth, botHalfWidth) * 0.15;

 const waistR = {
  x: chestCenter.x + waistHalfWidth,
  y: waistY
 };
 const waistL = {
  x: chestCenter.x - waistHalfWidth,
  y: waistY
 };

 const hipBlend = 0.84;

 const hipCurveR = {
  x: lerp(waistR.x, botR.x, hipBlend) + (botR.x - waistR.x) * 0.30,
  y: lerp(waistR.y, botR.y, hipBlend)
 };

 const hipCurveL = {
  x: lerp(waistL.x, botL.x, hipBlend) + (botL.x - waistL.x) * 0.30,
  y: lerp(waistL.y, botL.y, hipBlend)
 };

 const rightUpperCtrl = {
  x: lerp(topR.x, waistR.x, 0.46) - topHalfWidth * 0.10,
  y: lerp(topR.y, waistR.y, 0.68)
 };
 const leftUpperCtrl = {
  x: lerp(topL.x, waistL.x, 0.46) + topHalfWidth * 0.10,
  y: lerp(topL.y, waistL.y, 0.68)
 };

 ctx.fillStyle = color;
 ctx.beginPath();
 ctx.moveTo(topL.x, topL.y);

 ctx.quadraticCurveTo(
  chestCenter.x,
  chestCenter.y - curveY,
  topR.x,
  topR.y
 );

 ctx.bezierCurveTo(
  rightUpperCtrl.x, rightUpperCtrl.y,
  hipCurveR.x, hipCurveR.y,
  botR.x, botR.y
 );

 ctx.quadraticCurveTo(
  hipCenter.x,
  hipCenter.y + curveY * 1.10,
  botL.x,
  botL.y
 );

 ctx.bezierCurveTo(
  hipCurveL.x, hipCurveL.y,
  leftUpperCtrl.x, leftUpperCtrl.y,
  topL.x, topL.y
 );

 ctx.closePath();
 ctx.fill();
}

function clamp(v, min, max){
 return Math.max(min, Math.min(max, v));
}

function lerp(a, b, t){
 return a + (b - a) * t;
}

function mixPoint(a, b, t){
 if(!a && !b) return null;
 if(!a) return { x: b.x, y: b.y };
 if(!b) return { x: a.x, y: a.y };
 return {
  x: lerp(a.x, b.x, t),
  y: lerp(a.y, b.y, t)
 };
}
function spreadSidePoint(p, sideTag, amount, view){
 if(!p) return null;

 const sideStrength = view?.sideStrength || 0;
const t = smoothstep01((sideStrength - 0.60) / 0.35);

 return {
  x: p.x + sideTag * amount * t,
  y: p.y
 };
}
function getViewMetrics(rawPoints, cam, currentScale = 1, bodyBasis = null){
 const pR = rawPoints?.ID05;
 const pL = rawPoints?.ID06;
 const chest = rawPoints?.ID02;
 const pelvis = rawPoints?.ID10;

 if(!pR || !pL || !chest || !pelvis){
  return {
   sideStrength: 0,
   shoulderScreenWidth: 0,
   torsoScreenHeight: 0
  };
 }

 const viewCam = getViewCamera(cam);

 const rr = rotatePoint(pR, viewCam);
 const rl = rotatePoint(pL, viewCam);
 const rc = rotatePoint(chest, viewCam);
 const rp = rotatePoint(pelvis, viewCam);

 const shoulderScreenWidth = Math.abs(rr.x - rl.x) * currentScale;
 const torsoScreenHeight = Math.abs(rc.y - rp.y) * currentScale;

 const rawShoulderWidth3D = dist3D(pR, pL);
 const denom = Math.max(0.0001, rawShoulderWidth3D);
 const sideStrength = clamp(1 - Math.abs(rr.x - rl.x) / denom, 0, 1);

 return {
  sideStrength,
  shoulderScreenWidth,
  torsoScreenHeight
 };
}

function smoothstep01(t){
 t = clamp(t, 0, 1);
 return t * t * (3 - 2 * t);
}

function getFrontTorsoShape(P, refs){
 const chest = refs.chest || P.ID02;
 const pelvis = refs.pelvis || P.ID10;
 const neck = P.ID03;
 const bodyFacing = refs.bodyFacing || 1;

 const shoulderR = refs.rShoulder;
 const shoulderL = refs.lShoulder;
 const hipR = refs.rHip;
 const hipL = refs.lHip;

 if(!shoulderR || !shoulderL || !hipR || !hipL || !chest || !pelvis || !neck) return null;

 const chestCenter = midpoint(shoulderR, shoulderL) || chest;
 const hipCenter = midpoint(hipR, hipL) || pelvis;

 const shoulderInset = 0.10;
 const hipInset = 0.08;
 const curveY = Math.max(6, dist2D(shoulderR, shoulderL) * 0.18);

 const topR = {
  x: shoulderR.x + (chestCenter.x - shoulderR.x) * shoulderInset,
  y: shoulderR.y + (chestCenter.y - shoulderR.y) * shoulderInset
 };

 const topL = {
  x: shoulderL.x + (chestCenter.x - shoulderL.x) * shoulderInset,
  y: shoulderL.y + (chestCenter.y - shoulderL.y) * shoulderInset
 };

 // 下側を太めにする
 const hipOut = 0.56;

 const botR = {
  x: hipR.x + (hipCenter.x - hipR.x) * hipInset + (hipR.x - hipCenter.x) * hipOut,
  y: hipR.y + (hipCenter.y - hipR.y) * hipInset + 4
 };

 const botL = {
  x: hipL.x + (hipCenter.x - hipL.x) * hipInset + (hipL.x - hipCenter.x) * hipOut,
  y: hipL.y + (hipCenter.y - hipL.y) * hipInset + 4
 };

 // 腰を少し下げつつ、細すぎないようにする
 const waistY = lerp(chestCenter.y, hipCenter.y, 0.56);

 const topHalfWidth = Math.abs(topR.x - topL.x) * 0.5;
 const botHalfWidth = Math.abs(botR.x - botL.x) * 0.5;

 const waistHalfWidth = lerp(topHalfWidth, botHalfWidth, 0.42) * 0.72;

 const waistR = {
  x: chestCenter.x + waistHalfWidth,
  y: waistY
 };

 const waistL = {
  x: chestCenter.x - waistHalfWidth,
  y: waistY
 };

 const neckWidth = Math.max(10, dist2D(topR, topL) * 0.22);

 const neckFrontR = {
  x: chestCenter.x + neckWidth * 0.35,
  y: neck.y + curveY * 0.25
 };

 const neckBackL = {
  x: chestCenter.x - neckWidth * 0.35,
  y: neck.y + curveY * 0.25
 };

 const isBackView = bodyFacing >= 4;

 if(isBackView){
  return {
   neckBack: neckFrontR,
   upperBack: topR,
   lowerBack: waistR,
   backBottom: botR,
   frontBottom: botL,
   belly: waistL,
   chestFront: topL,
   neckFront: neckBackL
  };
 }

 return {
  neckBack: neckBackL,
  upperBack: topL,
  lowerBack: waistL,
  backBottom: botL,
  frontBottom: botR,
  belly: waistR,
  chestFront: topR,
  neckFront: neckFrontR
 };
}
function getSideTorsoShape(P, rawPoints, cam, refs, view){
 const chest = refs.chest || P.ID02;
 const pelvis = refs.pelvis || P.ID10;
 const neck = P.ID03;

 const genital = P.ID21 || pelvis;
 const anus = P.ID22 || pelvis;
 const breastCenter = P.ID27 || chest;
 const breastLowerR = P.ID25 || breastCenter;
 const breastLowerL = P.ID26 || breastCenter;
 const mouth = P.ID18 || neck;

 if(!chest || !neck || !pelvis) return null;

 const bodyView = getBodyViewInfo(cam);
 const side = bodyView.sideSign;

 const shoulderSpan = dist2D(refs.rShoulder, refs.lShoulder) || 40;
 const hipSpan = dist2D(refs.rHip, refs.lHip) || 24;
 const torsoHeight = Math.max(40, dist2D(chest, pelvis));

 const chestWidthHalf = clamp(shoulderSpan * 0.22, 10, 24);
 const waistWidthHalf = clamp(hipSpan * 0.16, 8, 18);

const bodyDepthBase = clamp(torsoHeight * 0.18, 11, 30);

const bustHint = clamp(
 Math.max(
  dist2D(breastCenter, chest),
  dist2D(breastLowerR, chest),
  dist2D(breastLowerL, chest)
 ) * 0.68,
 0,
 13
);

const chestFrontDepth = clamp(bodyDepthBase * 1.08 + bustHint, 12, 38);
const upperBackDepth  = clamp(bodyDepthBase * 0.40, 4, 15);
const bellyDepth      = clamp(bodyDepthBase * 1.10, 12, 34);
const buttDepth       = clamp(bodyDepthBase * 1.16, 11, 36);
const lowerBackDepth  = clamp(bodyDepthBase * 0.62, 8, 22);

const sideBoost = lerp(
 0.50,
 0.82,
 clamp(view?.sideStrength || 0, 0, 1)
);
const chestFront = {
 x: chest.x + side * (chestWidthHalf * 0.10 + chestFrontDepth * sideBoost * 0.72),
 y: chest.y - torsoHeight * 0.01
};

const upperBack = {
 x: chest.x - side * (chestWidthHalf * 0.13 + upperBackDepth * sideBoost * 1.05),
 y: chest.y - torsoHeight * 0.01
};

 const belly = {
  x: lerp(chest.x, pelvis.x, 0.45) + side * (bellyDepth * sideBoost),
y: lerp(chest.y, pelvis.y, 0.56)
 };

 const groinFront = {
  x: genital.x + side * (waistWidthHalf * 0.24 + bellyDepth * 0.18),
  y: genital.y + torsoHeight * 0.03
 };

 const lowerBack = {
  x: lerp(neck.x, pelvis.x, 0.70) - side * (waistWidthHalf * 0.12 + lowerBackDepth * sideBoost),
  y: lerp(neck.y, pelvis.y, 0.72)
 };

 const butt = {
  x: anus.x - side * (waistWidthHalf * 0.14 + buttDepth * sideBoost),
  y: anus.y + torsoHeight * 0.02
 };

 const neckFrontBase = mixPoint(mouth, neck, 0.65) || neck;
 const neckY = chest.y + torsoHeight * 0.04;

 const neckFront = {
  x: lerp(neckFrontBase.x, chestFront.x, 0.55),
  y: neckY
 };

 const neckBack = {
  x: lerp(neckFrontBase.x, upperBack.x, 0.55),
  y: neckY
 };

const frontBottom = mixPoint(groinFront, pelvis, 0.18);
const backBottom = mixPoint(butt, pelvis, 0.18);

 return {
  neckBack,
  upperBack,
  lowerBack,
  backBottom,
  frontBottom,
  belly,
  chestFront,
  neckFront
 };
}

function drawTorsoShape(ctx, shape, color){
 if(!shape) return;

const shoulderTopMid = {
 x: lerp(shape.upperBack.x, shape.chestFront.x, 0.48),
 y: Math.min(shape.upperBack.y, shape.chestFront.y) - 1.0
};

 ctx.fillStyle = color;
 ctx.beginPath();

 ctx.moveTo(shape.upperBack.x, shape.upperBack.y);

  ctx.quadraticCurveTo(
    lerp(shape.upperBack.x, shape.lowerBack.x, 0.18) - 8,
    lerp(shape.upperBack.y, shape.lowerBack.y, 0.50),
    shape.lowerBack.x,
    shape.lowerBack.y
  );

 ctx.quadraticCurveTo(
  shape.backBottom.x, shape.backBottom.y,
  shape.frontBottom.x, shape.frontBottom.y
 );

 ctx.quadraticCurveTo(
  shape.belly.x, shape.belly.y,
  shape.chestFront.x, shape.chestFront.y
 );

 ctx.quadraticCurveTo(
  shoulderTopMid.x, shoulderTopMid.y,
  shape.upperBack.x, shape.upperBack.y
 );

 ctx.closePath();
 ctx.fill();
}
function drawBlendTorso(ctx, P, rawPoints, color, cam, refs, t, view){
 const front = getFrontTorsoShape(P, refs);
 const side = getSideTorsoShape(P, rawPoints, cam, refs, view);

 if(!front && !side) return;
 if(!front){
  drawTorsoShape(ctx, side, color);
  return;
 }
 if(!side){
  drawTorsoShape(ctx, front, color);
  return;
 }

 const tb = smoothstep01(t);

 const axisTop = mixPoint(
  midpoint(front.upperBack, front.chestFront),
  midpoint(side.upperBack, side.chestFront),
  tb
 );

 const axisMid = mixPoint(
  midpoint(front.lowerBack, front.belly),
  midpoint(side.lowerBack, side.belly),
  tb
 );

 const axisBottom = mixPoint(
  midpoint(front.backBottom, front.frontBottom),
  midpoint(side.backBottom, side.frontBottom),
  tb
 );

 const frontTopHalf = dist2D(front.upperBack, front.chestFront) * 0.5;
 const frontMidHalf = dist2D(front.lowerBack, front.belly) * 0.5;
 const frontBotHalf = dist2D(front.backBottom, front.frontBottom) * 0.5;

 const sideTopHalf = dist2D(side.upperBack, side.chestFront) * 0.5;
 const sideMidHalf = dist2D(side.lowerBack, side.belly) * 0.5;
 const sideBotHalf = dist2D(side.backBottom, side.frontBottom) * 0.5;

 const topHalf = lerp(frontTopHalf, sideTopHalf, tb * 0.85);
 const midHalf = lerp(frontMidHalf, sideMidHalf, tb * 0.85);
 const botHalf = lerp(frontBotHalf, sideBotHalf, tb * 0.85);

 const shape = {
  neckBack: {
   x: axisTop.x - topHalf,
   y: axisTop.y
  },
  upperBack: {
   x: axisTop.x - topHalf,
   y: axisTop.y
  },
  lowerBack: {
   x: axisMid.x - midHalf,
   y: axisMid.y
  },
  backBottom: {
   x: axisBottom.x - botHalf,
   y: axisBottom.y
  },
  frontBottom: {
   x: axisBottom.x + botHalf,
   y: axisBottom.y
  },
  belly: {
   x: axisMid.x + midHalf,
   y: axisMid.y
  },
  chestFront: {
   x: axisTop.x + topHalf,
   y: axisTop.y
  },
  neckFront: {
   x: axisTop.x + topHalf,
   y: axisTop.y
  }
 };

 drawTorsoShape(ctx, shape, color);
}
function drawSideTorso(ctx, P, rawPoints, color, cam, refs, view){
 const shape = getSideTorsoShape(P, rawPoints, cam, refs, view);
 drawTorsoShape(ctx, shape, color);
}

function drawBreasts(ctx, projected, rawPoints, color, refs, cam, currentScale, view, bodyBasis, bodyFacing){
 if(!projected || !rawPoints) return;

 const nippleR = projected.ID19;
 const nippleL = projected.ID20;

 const rawNippleR = rawPoints.ID19;
 const rawNippleL = rawPoints.ID20;
 const rawOuterR = rawPoints.ID23;
 const rawOuterL = rawPoints.ID24;
 const rawLowerR = rawPoints.ID25;
 const rawLowerL = rawPoints.ID26;
 const rawChest   = rawPoints.ID02;
 const rawRShoulder = rawPoints.ID05;
 const rawLShoulder = rawPoints.ID06;

 if(!nippleR || !nippleL || !rawChest || !rawRShoulder || !rawLShoulder) return;

 const forwardDepth = Math.max(
  rawNippleR ? dist3D(rawChest, rawNippleR) : 0,
  rawNippleL ? dist3D(rawChest, rawNippleL) : 0
 );

 const lateralWidth = (
  rawOuterR && rawOuterL
   ? dist3D(rawOuterR, rawOuterL)
   : 0
 );

 const lowerDrop = Math.max(
  rawLowerR ? dist3D(rawChest, rawLowerR) : 0,
  rawLowerL ? dist3D(rawChest, rawLowerL) : 0
 );

 const breastSize3D =
  forwardDepth * 0.5 +
  lateralWidth * 0.3 +
  lowerDrop   * 0.2;

 const radius = clamp(
  breastSize3D * currentScale * 0.45,
  8,
  42
 );

 const drop = radius * 0.2;

 const centerR = {
  x: nippleR.x,
  y: nippleR.y + drop
 };

 const centerL = {
  x: nippleL.x,
  y: nippleL.y + drop
 };

 const sideStrength = view?.sideStrength || 0;

 const rx = radius * (1 - sideStrength * 0.3);
 const ry = radius;

drawBreastSplit(ctx, centerR, rx, ry, color);
drawBreastSplit(ctx, centerL, rx, ry, color);
}
function drawBreastSplit(ctx, center, rx, ry, color){
 if(!center) return;

 // 下側はそのまま丸みを残す
 const lowerCenter = {
  x: center.x,
  y: center.y + ry * 0.10
 };

 const lowerRx = rx;
 const lowerRy = ry * 0.72;

 // 上側は少し細く・少し浅く
 const upperCenter = {
  x: center.x,
  y: center.y - ry * 0.22
 };

 const upperRx = rx * 0.82;
 const upperRy = ry * 0.42;

 // 下半分
 ctx.save();
 ctx.beginPath();
 ctx.rect(
  center.x - rx - 4,
  center.y,
  (rx + 4) * 2,
  ry + 8
 );
 ctx.clip();
 drawEllipse(ctx, lowerCenter, lowerRx, lowerRy, color);
 ctx.restore();

 // 上半分
 ctx.save();
 ctx.beginPath();
 ctx.rect(
  center.x - rx - 4,
  center.y - ry - 8,
  (rx + 4) * 2,
  ry + 8
 );
 ctx.clip();
 drawEllipse(ctx, upperCenter, upperRx, upperRy, color);
 ctx.restore();
}
function drawEllipse(ctx, p, rx, ry, color){
 if(!p) return;
 ctx.save();
 ctx.beginPath();
 ctx.translate(p.x, p.y);
 ctx.scale(rx, ry);
 ctx.arc(0, 0, 1, 0, Math.PI * 2);
 ctx.fillStyle = color;
 ctx.fill();
 ctx.restore();
}
function normalizeVec3(v){
  const x = Number(v?.x) || 0;
  const y = Number(v?.y) || 0;
  const z = Number(v?.z) || 0;
  const len = Math.hypot(x, y, z) || 1;
  return { x: x / len, y: y / len, z: z / len };
}

function crossVec3(a, b){
  return {
    x: a.y * b.z - a.z * b.y,
    y: a.z * b.x - a.x * b.z,
    z: a.x * b.y - a.y * b.x
  };
}

function addVec3(a, b){
  return {
    x: (a?.x || 0) + (b?.x || 0),
    y: (a?.y || 0) + (b?.y || 0),
    z: (a?.z || 0) + (b?.z || 0)
  };
}

function scaleVec3(v, s){
  return {
    x: (v?.x || 0) * s,
    y: (v?.y || 0) * s,
    z: (v?.z || 0) * s
  };
}

function drawWorldFloor(ctx, posePoints, canvasW, canvasH){
  const scene = poseExtra?.scene || {};

  const origin = {
    x: Number(scene.position?.x) || 0,
    y: Number(scene.position?.y) || 0,
    z: Number(scene.position?.z) || 0
  };

  const up = normalizeVec3(scene.up || { x: 0, y: 1, z: 0 });
  let forward = normalizeVec3(scene.forward || { x: 0, y: 0, z: 1 });

  let right = crossVec3(forward, up);
  const rightLen = Math.hypot(right.x, right.y, right.z);
  if(rightLen < 1e-6){
    forward = { x: 0, y: 0, z: 1 };
    right = crossVec3(forward, up);
  }
  right = normalizeVec3(right);
  forward = normalizeVec3(crossVec3(up, right));

  // 人体と同じ中心・ズーム
  const { fitScale, centerX, centerY } = getLayoutMetrics(
    posePoints || {},
    canvasW,
    canvasH
  );

  const scale = fitScale * (INTERNAL_CAMERA.scale || 1);
  const tx = INTERNAL_CAMERA.tx || 0;
  const ty = INTERNAL_CAMERA.ty || 0;

  function toScreen(p){
    const r = projectPoint(p);
    return {
      x: canvasW / 2 + tx + (r.x - centerX) * scale,
      y: canvasH / 2 + ty - (r.y - centerY) * scale
    };
  }

  // 床サイズ
  const halfW = 8.0;
  const halfD = 8.0;

  const p1 = addVec3(addVec3(origin, scaleVec3(right, -halfW)), scaleVec3(forward, -halfD));
  const p2 = addVec3(addVec3(origin, scaleVec3(right,  halfW)), scaleVec3(forward, -halfD));
  const p3 = addVec3(addVec3(origin, scaleVec3(right,  halfW)), scaleVec3(forward,  halfD));
  const p4 = addVec3(addVec3(origin, scaleVec3(right, -halfW)), scaleVec3(forward,  halfD));

  const s1 = toScreen(p1);
  const s2 = toScreen(p2);
  const s3 = toScreen(p3);
  const s4 = toScreen(p4);

  ctx.save();
  ctx.beginPath();
  ctx.moveTo(s1.x, s1.y);
  ctx.lineTo(s2.x, s2.y);
  ctx.lineTo(s3.x, s3.y);
  ctx.lineTo(s4.x, s4.y);
  ctx.closePath();

  ctx.fillStyle = poseExtra?.groundColor || '#6fa84f';
  ctx.fill();

  ctx.strokeStyle = 'rgba(0,0,0,0.18)';
  ctx.lineWidth = 1;
  ctx.stroke();

  ctx.restore();
}
function drawExtraBoxes(ctx, posePoints, canvasW, canvasH){
  const boxes = poseExtra?.boxes;
  if(!Array.isArray(boxes) || !boxes.length) return;

  const { fitScale, centerX, centerY } = getLayoutMetrics(
    posePoints || {},
    canvasW,
    canvasH
  );

  const scale = fitScale * (INTERNAL_CAMERA.scale || 1);
  const tx = INTERNAL_CAMERA.tx || 0;
  const ty = INTERNAL_CAMERA.ty || 0;

  function toScreen(p){
    const r = projectPoint(p);
    return {
      x: canvasW / 2 + tx + (r.x - centerX) * scale,
      y: canvasH / 2 + ty - (r.y - centerY) * scale
    };
  }

  for(const box of boxes){
    const x = Number(box?.x) || 0;
    const y = (Number(box?.y) || 0) - getGroundOffset().y;
    const z = -(Number(box?.z) || 0);

    const w = Math.max(0.01, Number(box?.w) || 1);
    const h = Math.max(0.01, Number(box?.h) || 1);
    const d = Math.max(0.01, Number(box?.d) || 1);

    const color = box?.color || '#888888';

    const xL = x - w * 0.5;
    const xR = x + w * 0.5;
    const yB = y;
    const yT = y + h;
    const zF = z;
    const zBk = z - d;

    const pts = [
      toScreen({ x: xL, y: yT, z: zF }),
      toScreen({ x: xR, y: yT, z: zF }),
      toScreen({ x: xR, y: yB, z: zF }),
      toScreen({ x: xL, y: yB, z: zF })
    ];

    ctx.beginPath();
    ctx.moveTo(pts[0].x, pts[0].y);
    ctx.lineTo(pts[1].x, pts[1].y);
    ctx.lineTo(pts[2].x, pts[2].y);
    ctx.lineTo(pts[3].x, pts[3].y);
    ctx.closePath();

    ctx.fillStyle = color;
    ctx.fill();

    ctx.strokeStyle = 'rgba(0,0,0,0.3)';
    ctx.lineWidth = 1;
    ctx.stroke();
  }
}
function getExtraBoxesDepth(){
 const boxes = poseExtra?.boxes;
 if(!Array.isArray(boxes) || !boxes.length) return -9999;

 let sum = 0;
 let count = 0;

 for(const box of boxes){
  const x = Number(box?.x) || 0;
  const y = Number(box?.y) || 0;
  const z = Number(box?.z) || 0;
  const h = Math.max(0.01, Number(box?.h) || 1);
  const d = Math.max(0.01, Number(box?.d) || 1);

const center = {
  x,
  y: y + h * 0.5,
  z: z - d * 0.5
};

  const r = rotatePoint(center, getViewCamera(INTERNAL_CAMERA));
  sum += r.z;
  count++;
 }

 return count ? (sum / count) : -9999;
}

function drawBreastBridge(ctx, projected, color, view){
 const chest = projected?.ID02;
 const nippleR = projected?.ID19;
 const nippleL = projected?.ID20;
 const bustCenter = projected?.ID27;

const pitch = Math.abs(INTERNAL_CAMERA.pitch || 0);
 if(pitch > 1.0) return;

 if(!chest || !nippleR || !nippleL || !bustCenter) return;

 const sideStrength = view?.sideStrength || 0;

 const bridgeCenter = {
  x: bustCenter.x,
  y: lerp(chest.y, bustCenter.y, 0.72)
 };

 const halfSpan = Math.abs(nippleR.x - nippleL.x) * 0.5;
const rx = Math.max(8, halfSpan * lerp(0.42, 0.24, sideStrength));
const ry = Math.max(6, rx * lerp(0.72, 0.52, sideStrength));
 drawEllipse(ctx, bridgeCenter, rx, ry, color);
}



function drawShoulderPeak(ctx, leftShoulder, neckMid, rightShoulder, width, color){
 if(!leftShoulder || !neckMid || !rightShoulder) return;

 ctx.strokeStyle = color;
 ctx.lineWidth = width;
 ctx.lineCap = 'round';
 ctx.lineJoin = 'round';
 ctx.beginPath();
 ctx.moveTo(leftShoulder.x, leftShoulder.y);
 ctx.lineTo(neckMid.x, neckMid.y);
 ctx.lineTo(rightShoulder.x, rightShoulder.y);
 ctx.stroke();
}

function drawWaistBridge(ctx, pelvis, rHip, lHip, genital, anus, legWidth){
 if(!pelvis || !rHip || !lHip) return;

 const hipMid = midpoint(rHip, lHip);
 if(!hipMid) return;

 const innerR = {
  x: lerp(rHip.x, hipMid.x, 0.34),
  y: lerp(rHip.y, hipMid.y, 0.18)
 };

 const innerL = {
  x: lerp(lHip.x, hipMid.x, 0.34),
  y: lerp(lHip.y, hipMid.y, 0.18)
 };

 const frontCenterBase = genital || pelvis;
 const backCenterBase = anus || pelvis;

 const frontCenter = {
  x: lerp(pelvis.x, frontCenterBase.x, 0.42),
  y: lerp(pelvis.y, frontCenterBase.y, 0.42)
 };

 const backCenter = {
  x: lerp(pelvis.x, backCenterBase.x, 0.42),
  y: lerp(pelvis.y, backCenterBase.y, 0.42)
 };

 const topLeft = {
  x: lerp(innerL.x, pelvis.x, 0.18),
  y: lerp(innerL.y, pelvis.y, 0.55)
 };

 const topRight = {
  x: lerp(innerR.x, pelvis.x, 0.18),
  y: lerp(innerR.y, pelvis.y, 0.55)
 };

 const frontBulge = Math.max(6, legWidth * 0.22);
 const backBulge = Math.max(5, legWidth * 0.18);

 const frontCtrl = {
  x: frontCenter.x,
  y: frontCenter.y + frontBulge
 };

 const backCtrl = {
  x: backCenter.x,
  y: backCenter.y - backBulge
 };

 ctx.fillStyle = '#888';
 ctx.beginPath();

 ctx.moveTo(topLeft.x, topLeft.y);

 ctx.quadraticCurveTo(
  backCtrl.x,
  backCtrl.y,
  topRight.x,
  topRight.y
 );

 ctx.quadraticCurveTo(
  frontCtrl.x,
  frontCtrl.y,
  innerR.x,
  innerR.y
 );

 ctx.lineTo(innerL.x, innerL.y);

 ctx.quadraticCurveTo(
  frontCtrl.x,
  frontCtrl.y,
  topLeft.x,
  topLeft.y
 );

 ctx.closePath();
 ctx.fill();
}

function clampPitch(v){
 const LIMIT = Math.PI * 0.495;
 return clamp(v, -LIMIT, LIMIT);
}

function findNearestPointId(screenX, screenY, projected, maxDist = 12){
 if(!projected) return null;

 let bestId = null;
 let bestDist = Infinity;

 for(const id of Object.keys(projected)){
  if(id.startsWith('__')) continue;

  const p = projected[id];
  if(!p) continue;

  const d = Math.hypot(screenX - p.x, screenY - p.y);
  if(d < bestDist){
   bestDist = d;
   bestId = id;
  }
 }

 return bestDist <= maxDist ? bestId : null;
}
function drawPointDots(ctx, projected, rawPoints){
 const ids = Object.keys(projected || {});
 if(!ids.length) return;

 const forward = getBodyForwardForDepth();

 ids.sort((a, b) => {
  const pa = rawPoints?.[a];
  const pb = rawPoints?.[b];
  if(!pa || !pb) return 0;

  const da = getDepthByForward(pa, forward);
  const db = getDepthByForward(pb, forward);

  return da - db;
 });

 for(const id of ids){
  const pt = projected[id];
  if(!pt) continue;

  let r = 4.6;
  let color = 'rgba(140,220,255,0.95)';

  if(
   id === 'ID18' ||
   id === 'ID19' ||
   id === 'ID20' ||
   id === 'ID21'
  ){
   r = 5.0;
   color = 'rgba(255,170,185,0.95)';
  }

  if(id === 'ID22'){
   r = 4.8;
   color = 'rgba(255,235,150,0.95)';
  }

if(id === selectedPointId){
 r = 10;
 color = 'rgba(0,224,255,0.98)';
}

ctx.beginPath();
ctx.arc(pt.x, pt.y, r, 0, Math.PI * 2);
ctx.fillStyle = color;
ctx.fill();

if(id === hoveredPointId && id !== selectedPointId){
 ctx.beginPath();
 ctx.arc(pt.x, pt.y, r + 4, 0, Math.PI * 2);
 ctx.strokeStyle = 'rgba(255,255,255,0.85)';
 ctx.lineWidth = 2;
 ctx.stroke();
}
 }
}

function updateRoot(rawPose, prevState, dt){
  const pts = rawPose.points || {};

  // 位置は完全に固定（置くだけ）
  const scenePosition = { x: 0, y: 0, z: 0 };
  const sceneVelocity = { x: 0, y: 0, z: 0 };

  // 向き計算だけ残す（ポーズの向きは保つ）
  const pelvis = pts.ID10;
  const chest  = pts.ID02;
  const rHip   = pts.ID12;
  const lHip   = pts.ID13;

  if (!pelvis || !chest || !rHip || !lHip) {
    return buildFallbackOutput(rawPose, prevState);
  }

  let bodyRight = normalize(sub(rHip, lHip));
  let bodyUp    = normalize(sub(chest, pelvis));
  let bodyForward = normalize(cross(bodyUp, bodyRight));

  const genital = pts.ID21;
  const anus = pts.ID22;
  if (genital && anus) {
    const frontHint = normalize(sub(genital, anus));
    if (dot(bodyForward, frontHint) < 0) {
      bodyForward = { x: -bodyForward.x, y: -bodyForward.y, z: -bodyForward.z };
    }
  }

  bodyRight   = normalize(cross(bodyForward, bodyUp));
  bodyUp      = normalize(cross(bodyRight, bodyForward));

  const headForward = prevState.headForward || { x: 0, y: 0, z: 1 };
  const headUp      = prevState.headUp      || { x: 0, y: 1, z: 0 };
  const headRight   = prevState.headRight   || { x: 1, y: 0, z: 0 };

  const out = {
    frame: (rawPose.frame ?? 0),
    root: "ID10",
    scene: {
      position: scenePosition,
      velocity: sceneVelocity,
      groundY: 0,
      isGrounded: false,
      isJumping: false
    },
    body: {
      root: "ID10",
      forward: roundVec(bodyForward),
      up: roundVec(bodyUp),
      right: roundVec(bodyRight)
    },
    head: {
      root: "ID03",
      forward: roundVec(headForward),
      up: roundVec(headUp),
      right: roundVec(headRight)
    },
    points: pts
  };

  const nextState = {
    ...prevState,
    scenePosition,
    sceneVelocity,
    bodyForward,
    bodyUp,
    bodyRight,
    headForward,
    headUp,
    headRight
  };

  return { out, nextState };
}
function sub(a, b){
 return {
  x: a.x - b.x,
  y: a.y - b.y,
  z: a.z - b.z
 };
}

function dot(a, b){
 return a.x * b.x + a.y * b.y + a.z * b.z;
}

function cross(a, b){
 return {
  x: a.y * b.z - a.z * b.y,
  y: a.z * b.x - a.x * b.z,
  z: a.x * b.y - a.y * b.x
 };
}

function length(v){
 return Math.hypot(v.x, v.y, v.z);
}

function normalize(v){
 const len = length(v);
 if(!isFinite(len) || len < 1e-8){
  return { x: 0, y: 0, z: 0 };
 }
 return {
  x: v.x / len,
  y: v.y / len,
  z: v.z / len
 };
}
function clampHeadYawPitch(bodyForward, bodyUp, headForward){
 const bodyRight = normalize(cross(bodyForward, bodyUp));

 const f =
  dot(headForward, bodyForward);
 const r =
  dot(headForward, bodyRight);
 const u =
  dot(headForward, bodyUp);

 const yawLimit = Math.sin(Math.PI * 80 / 180);
 const pitchUpLimit = Math.sin(Math.PI * 45 / 180);
 const pitchDownLimit = Math.sin(Math.PI * 45 / 180);

 const rr = clamp(r, -yawLimit, yawLimit);
 const uu = clamp(u, -pitchDownLimit, pitchUpLimit);

 let out = {
  x: bodyForward.x * f + bodyRight.x * rr + bodyUp.x * uu,
  y: bodyForward.y * f + bodyRight.y * rr + bodyUp.y * uu,
  z: bodyForward.z * f + bodyRight.z * rr + bodyUp.z * uu
 };

 return normalize(out);
}

function lerpVec(a, b, t){
 return {
  x: lerp(a.x, b.x, t),
  y: lerp(a.y, b.y, t),
  z: lerp(a.z, b.z, t)
 };
}

function isFiniteVec(v){
 return Number.isFinite(v.x) && Number.isFinite(v.y) && Number.isFinite(v.z);
}

function roundVec(v){
 return {
  x: +v.x.toFixed(4),
  y: +v.y.toFixed(4),
  z: +v.z.toFixed(4)
 };
}
function getHeadFacingFromMetaOrPose(points, meta, cam){
 const metaForward = meta?.head?.forward;

 if(
  metaForward &&
  Number.isFinite(metaForward.x) &&
  Number.isFinite(metaForward.y) &&
  Number.isFinite(metaForward.z)
 ){
  const nf = vecNormalize(metaForward);
  const info = getFacingFromForwardVec(nf, cam);

  const viewCam = getViewCamera(cam);
  const rf = rotatePoint(nf, viewCam);

  return {
   facing: info.facing,
   lr: rf.x >= 0 ? 1 : -1,
   frontness: clamp((rf.z + 1) * 0.5, 0, 1),
   sideness: clamp(Math.abs(rf.x), 0, 1),
   source: 'meta'
  };
 }

 const fallback = getHeadFacingFromPose(points, cam);
 return {
  ...fallback,
  source: 'pose'
 };
}
function getDepthByForward(p, forward){
 if(!p || !forward) return 0;
 return p.x * forward.x + p.y * forward.y + p.z * forward.z;
}
function getBodyViewInfo(cam){
 const forwardWorld = getBodyForwardForDepth();
 const viewCam = getViewCamera(cam);
 const forwardView = rotatePoint(forwardWorld, viewCam);

 return {
  forwardWorld,
  forwardView,
  sideSign: forwardView.x >= 0 ? 1 : -1,
  frontBackSign: forwardView.z >= 0 ? 1 : -1,
  sideStrength: clamp(Math.abs(forwardView.x), 0, 1),
  frontness: clamp(Math.abs(forwardView.z), 0, 1)
 };
}
function getBodyForwardForDepth(){
 const f = poseSceneMeta?.body?.forward;
 if(
  f &&
  Number.isFinite(f.x) &&
  Number.isFinite(f.y) &&
  Number.isFinite(f.z)
 ){
  return vecNormalize(f);
 }

 const basis = buildRootBasis(getPosePointsWithSceneOffset(pose.points || {}));
 return basis?.forward || { x: 0, y: 0, z: 1 };
}
function drawPointLabels(ctx, projected, rawPoints){
 if(!projected) return;

 ctx.save();
 ctx.font = '10px monospace';
 ctx.fillStyle = '#00ff88';
 ctx.textAlign = 'left';
 ctx.textBaseline = 'middle';

 for(const id in projected){
  if(id.startsWith('__')) continue;

  const p = projected[id];
  if(!p) continue;

  ctx.fillStyle = '#ffffff';
  ctx.fillRect(p.x - 1, p.y - 1, 2, 2);

  ctx.fillStyle = '#00ff88';
const name = rawPoints?.[id]?.name || '';
 }

 ctx.restore();
}
  function formatPointCoord(v){
 if(!Number.isFinite(v)) return '-';
 return v.toFixed(3);
}

function updateSelectedPointInfo(rawPoints){
 const el = document.getElementById('pose-min-selected-info');
 if(!el) return;

 while(el.firstChild){
  el.removeChild(el.firstChild);
 }

 const activeId = selectedPointId || hoveredPointId;

 if(!activeId || !rawPoints?.[activeId]){
  const span = document.createElement('span');
  span.style.color = '#777';
  span.textContent = 'selected: none';
  el.appendChild(span);
  return;
 }

 const p = rawPoints[activeId];
 const name = p.name || '';

 const line1 = document.createElement('div');
 line1.style.color = selectedPointId ? '#fff3a0' : '#8fdcff';

 const b = document.createElement('b');
 b.textContent = activeId;
 line1.appendChild(b);

 const stateText = document.createTextNode(selectedPointId ? ' selected ' : ' hover ');
 line1.appendChild(stateText);

 const nameText = document.createTextNode(name);
 line1.appendChild(nameText);

 const line2 = document.createElement('div');
 line2.style.color = '#ddd';
 line2.textContent =
  `x: ${formatPointCoord(p.x)}　y: ${formatPointCoord(p.y)}　z: ${formatPointCoord(p.z)}`;

 el.appendChild(line1);
 el.appendChild(line2);
}

function drawSelectedPointOverlay(ctx, projected, rawPoints){
 if(!selectedPointId) return;

 const p2 = projected?.[selectedPointId];
 const p3 = rawPoints?.[selectedPointId];
 if(!p2 || !p3) return;

 const label = `${selectedPointId} ${p3.name || ''}`;
 const coord = `x:${formatPointCoord(p3.x)} y:${formatPointCoord(p3.y)} z:${formatPointCoord(p3.z)}`;

 ctx.save();

 ctx.beginPath();
ctx.arc(p2.x, p2.y, 16, 0, Math.PI * 2);
 ctx.strokeStyle = 'rgba(255,230,80,0.95)';
 ctx.lineWidth = 2;
 ctx.stroke();

 ctx.beginPath();
 ctx.arc(p2.x, p2.y, 16, 0, Math.PI * 2);
 ctx.strokeStyle = 'rgba(255,230,80,0.35)';
 ctx.lineWidth = 1;
 ctx.stroke();

 const padX = 8;
 const padY = 6;
 const boxX = p2.x + 14;
 const boxY = p2.y - 34;

 ctx.font = '11px monospace';
 const w = Math.max(
  ctx.measureText(label).width,
  ctx.measureText(coord).width
 ) + padX * 2;

 const h = 34;

 ctx.fillStyle = 'rgba(10,10,10,0.88)';
 ctx.strokeStyle = 'rgba(255,230,80,0.75)';
 ctx.lineWidth = 1;
 roundRectPath(ctx, boxX, boxY, w, h, 6);
 ctx.fill();
 ctx.stroke();

 ctx.fillStyle = '#fff3a0';
 ctx.textBaseline = 'top';
 ctx.fillText(label, boxX + padX, boxY + 5);

 ctx.fillStyle = '#ffffff';
 ctx.fillText(coord, boxX + padX, boxY + 18);

 ctx.restore();
}
function drawActionHighlights(ctx, projected){
 const now = Date.now();

 actionHighlights = actionHighlights.filter(h => now - h.time < 3000);

 for(const h of actionHighlights){
  const p = projected[h.id];
  if(!p) continue;

  const age = now - h.time;
  const t = 1 - age / 3000;

  ctx.beginPath();
  ctx.arc(p.x, p.y, 20, 0, Math.PI * 2);
  ctx.fillStyle = `rgba(255,120,180,${0.35 * t})`;
  ctx.fill();
 }
}

function roundRectPath(ctx, x, y, w, h, r){
 ctx.beginPath();
 ctx.moveTo(x + r, y);
 ctx.lineTo(x + w - r, y);
 ctx.quadraticCurveTo(x + w, y, x + w, y + r);
 ctx.lineTo(x + w, y + h - r);
 ctx.quadraticCurveTo(x + w, y + h, x + w - r, y + h);
 ctx.lineTo(x + r, y + h);
 ctx.quadraticCurveTo(x, y + h, x, y + h - r);
 ctx.lineTo(x, y + r);
 ctx.quadraticCurveTo(x, y, x + r, y);
 ctx.closePath();
}

function shadeColor(color, percent){
 const num = parseInt(color.slice(1), 16);
 const amt = Math.round(2.55 * percent);
 const R = (num >> 16) + amt;
 const G = (num >> 8 & 0x00FF) + amt;
 const B = (num & 0x0000FF) + amt;

 return "#" + (
  0x1000000 +
  (R < 255 ? (R < 0 ? 0 : R) : 255) * 0x10000 +
  (G < 255 ? (G < 0 ? 0 : G) : 255) * 0x100 +
  (B < 255 ? (B < 0 ? 0 : B) : 255)
 ).toString(16).slice(1);
}

function darkenColor(color, amount){
 return shadeColor(color, -amount);
}

function buildFallbackOutput(rawPose, prevState){
 return {
  out: {
   frame: rawPose?.frame ?? 0,
   root: rawPose?.root || 'ID10',
   scene: {
    position: roundVec(prevState.scenePosition || { x: 0, y: 0, z: 0 }),
    velocity: roundVec(prevState.sceneVelocity || { x: 0, y: 0, z: 0 }),
    groundY: 0,
    isGrounded: !!prevState.isGrounded,
    isJumping: !!prevState.isJumping
   },
   body: {
    root: 'ID10',
    forward: roundVec(prevState.bodyForward || { x: 0, y: 0, z: 1 }),
    up: roundVec(prevState.bodyUp || { x: 0, y: 1, z: 0 }),
    right: roundVec(prevState.bodyRight || { x: 1, y: 0, z: 0 })
   },
   head: {
    root: 'ID03',
    forward: roundVec(prevState.headForward || { x: 0, y: 0, z: 1 }),
    up: roundVec(prevState.headUp || { x: 0, y: 1, z: 0 }),
    right: roundVec(prevState.headRight || { x: 1, y: 0, z: 0 })
   },
   points: rawPose?.points || {}
  },
  nextState: prevState
 };
}
})();

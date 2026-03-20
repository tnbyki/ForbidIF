// ==UserScript==
// @name         Gemini Pose Viewer Minimal
// @namespace    http://tampermonkey.net/
// @version      0.2
// @match        https://gemini.google.com/*
// @match        https://gemini.google.*/*
// @grant        none
// @run-at       document-idle
// ==/UserScript==

(function () {
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

  _prevRHeelY: 0,
  _prevLHeelY: 0
};

let isDragging = false;
let isPanning = false;
let isPanelDragging = false;
let lastMouseX = 0;
let lastMouseY = 0;
let stripPoseJsonOnSend = true;   // true: 送信時に <POSE_JSON_START>～<POSE_JSON_END> を削除
let globalPointerHandlersInstalled = false;
let sendInterceptorInstalled = false;

  const BONES = [
    ['ID04', 'ID03'],
    ['ID03', 'ID02'],
    ['ID02', 'ID01'],
    ['ID01', 'ID10'],
    ['ID02', 'ID05'],
    ['ID05', 'ID07'],
    ['ID07', 'ID09'],
    ['ID02', 'ID06'],
    ['ID06', 'ID08'],
    ['ID08', 'ID11'],
    ['ID10', 'ID12'],
    ['ID12', 'ID14'],
    ['ID14', 'ID16'],
    ['ID10', 'ID13'],
    ['ID13', 'ID15'],
    ['ID15', 'ID17']
  ];

const INTERNAL_CAMERA = {
  yaw: 0,
  pitch: 0,
  roll: 0,
  scale: 1,
  tx: 0,
  ty: 0
};

const WORLD_FLOOR_Y = -1.55; // 世界の床の高さ

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

  let lastPoseBlock = '';
  let autoSyncObserverStarted = false;
  let autoSyncTimer = null;
  let removePoseTimer = null;

let forceFrontView = false;
let fixedFitScale = null;
let poseSceneMeta = null;
let lastPoseMetaBlock = '';

  function sanitizePoseJsonText(text) {
    if (!text) return '';
    return text
      .replace(/[\u200B-\u200D\uFEFF]/g, '')
      .replace(/\r/g, '')
      .replace(/[“”]/g, '"')
      .replace(/[‘’]/g, "'")
      .replace(/,\s*([}\]])/g, '$1')
      .trim();
  }

  function extractLatestPoseJsonBlock(text) {
    if (!text) return null;

    const matches = [...text.matchAll(/<POSE_JSON_START>([\s\S]*?)<POSE_JSON_END>/g)];
    if (!matches.length) return null;

    let block = matches[matches.length - 1][1].trim();

    const jsonStart = block.indexOf('{');
    const jsonEnd = block.lastIndexOf('}');

    if (jsonStart === -1 || jsonEnd === -1 || jsonEnd < jsonStart) return null;

    block = block.slice(jsonStart, jsonEnd + 1);
    return sanitizePoseJsonText(block);
  }

  function extractLatestPoseSceneMetaBlock(text) {
    if (!text) return null;

    const matches = [...text.matchAll(/<POSE_SCENE_META_START>([\s\S]*?)<POSE_SCENE_META_END>/g)];
    if (!matches.length) return null;

    let block = matches[matches.length - 1][1].trim();

    const jsonStart = block.indexOf('{');
    const jsonEnd = block.lastIndexOf('}');

    if (jsonStart === -1 || jsonEnd === -1 || jsonEnd < jsonStart) return null;

    block = block.slice(jsonStart, jsonEnd + 1);
    return sanitizePoseJsonText(block);
  }
function getPoseSourceText() {
  const bodyText = document.body?.textContent || '';
  const composer = findGeminiComposer();
  const composerText = composer
    ? ((composer.innerText || composer.textContent || ''))
    : '';

  return `${composerText}\n${bodyText}`;
}
function ensureCameraDefaults() {
  if (typeof INTERNAL_CAMERA.yaw !== 'number') INTERNAL_CAMERA.yaw = 0;
  if (typeof INTERNAL_CAMERA.pitch !== 'number') INTERNAL_CAMERA.pitch = 0;
  if (typeof INTERNAL_CAMERA.roll !== 'number') INTERNAL_CAMERA.roll = 0;
  if (typeof INTERNAL_CAMERA.scale !== 'number') INTERNAL_CAMERA.scale = 1;
  if (typeof INTERNAL_CAMERA.tx !== 'number') INTERNAL_CAMERA.tx = 0;
  if (typeof INTERNAL_CAMERA.ty !== 'number') INTERNAL_CAMERA.ty = 0;
}
    function degToRad(v) {
  return v * Math.PI / 180;
}

function normalizeIncomingCamera(camera) {
  const next = { ...(camera || {}) };

  if (typeof next.yaw === 'number' && Math.abs(next.yaw) > Math.PI * 1.5) {
    next.yaw = degToRad(next.yaw);
  }
  if (typeof next.pitch === 'number' && Math.abs(next.pitch) > Math.PI * 1.5) {
    next.pitch = degToRad(next.pitch);
  }
  if (typeof next.roll === 'number' && Math.abs(next.roll) > Math.PI * 1.5) {
    next.roll = degToRad(next.roll);
  }

  // yawを -π〜π に正規化
  if (typeof next.yaw === 'number') {
    while (next.yaw > Math.PI) next.yaw -= Math.PI * 2;
    while (next.yaw < -Math.PI) next.yaw += Math.PI * 2;
  }

  return next;
}

function applyForcedViewIfNeeded() {
  ensureCameraDefaults();

  if (!forceFrontView) return;

  INTERNAL_CAMERA.yaw = 0;
  INTERNAL_CAMERA.pitch = 0;
  INTERNAL_CAMERA.roll = 0;
}

function resetCamera(withDraw = true) {
  INTERNAL_CAMERA.yaw = 0;
  INTERNAL_CAMERA.pitch = 0;
  INTERNAL_CAMERA.roll = 0;
  INTERNAL_CAMERA.scale = 1;
  INTERNAL_CAMERA.tx = 0;
  INTERNAL_CAMERA.ty = 0;

  if (withDraw) {
    drawPose();
  }
}

  function makeButton(label, onClick) {
    const btn = document.createElement('button');
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

function setStatus(text) {
  const el = document.getElementById('pose-min-status');
  if (el) el.textContent = text;
}

  function setForceFrontUI(v, withStatus = false) {
    forceFrontView = !!v;

    const cb = document.getElementById('pose-min-force-front');
    if (cb) cb.checked = forceFrontView;

    if (withStatus) {
      setStatus(`status: force front ${forceFrontView ? 'on' : 'off'}`);
    }
  }

function rotatePoint(p, cam) {
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

  // yaw: Y軸回転
  const x1 = x * cy + z * sy;
  const z1 = -x * sy + z * cy;

  // pitch: X軸回転
  const y1 = y * cp - z1 * sp;
  const z2 = y * sp + z1 * cp;

  // roll: Z軸回転
  const x2 = x1 * cr - y1 * sr;
  const y2 = x1 * sr + y1 * cr;

  return { x: x2, y: y2, z: z2 };
}
function getViewCamera(cam) {
  return {
    yaw: cam?.yaw || 0,       // ← - を外す
    pitch: cam?.pitch || 0,   // ← - を外す
    roll: cam?.roll || 0      // ← - を外す
  };
}

function getScenePositionOffset() {
  const sp = poseSceneMeta?.scene?.position;
  return {
    x: Number(sp?.x) || 0,
    y: Number(sp?.y) || 0,
    z: Number(sp?.z) || 0
  };
}

function applyScenePositionToPoint(p) {
  if (!p) return p;
  const ofs = getScenePositionOffset();
  return {
    ...p,
    x: p.x + ofs.x,
    y: p.y + ofs.y,
    z: p.z + ofs.z
  };
}

function getPosePointsWithSceneOffset(points) {
  const out = {};
  for (const id of Object.keys(points || {})) {
    out[id] = applyScenePositionToPoint(points[id]);
  }
  return out;
}

function projectPoint(p) {
  const r = rotatePoint(p, getViewCamera(INTERNAL_CAMERA));
  return {
    x: r.x,
    y: r.y
  };
}

  function computeLayout(points, width, height) {
    ensureCameraDefaults();

    const projected = {};
    const ids = Object.keys(points || {});
    if (!ids.length) return projected;

    let minX = Infinity, maxX = -Infinity, minY = Infinity, maxY = -Infinity;

    for (const id of ids) {
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

if (fixedFitScale == null) {
  fixedFitScale = autoFitScale;
}

const scale = fixedFitScale * (INTERNAL_CAMERA.scale || 1);

const centerX = (minX + maxX) / 2;
const centerY = (minY + maxY) / 2;
const tx = INTERNAL_CAMERA.tx || 0;
const ty = INTERNAL_CAMERA.ty || 0;

    for (const id of ids) {
      const p = projected[id];
      projected[id] = {
        x: width / 2 + tx + (p.x - centerX) * scale,
        y: height / 2 + ty - (p.y - centerY) * scale
      };
    }

    return projected;
  }

  function getLayoutMetrics(points, width, height) {
    ensureCameraDefaults();

    const ids = Object.keys(points || {});
    if (!ids.length) {
      return { fitScale: 1, centerX: 0, centerY: 0 };
    }

    let minX = Infinity, maxX = -Infinity, minY = Infinity, maxY = -Infinity;

    for (const id of ids) {
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

if (fixedFitScale == null) {
  fixedFitScale = autoFitScale;
}

return {
  fitScale: fixedFitScale,
  centerX: (minX + maxX) / 2,
  centerY: (minY + maxY) / 2
};

}
function getFacingClassFromCamera(cam) {
  let a = cam?.yaw || 0;

  while (a > Math.PI) a -= Math.PI * 2;
  while (a < -Math.PI) a += Math.PI * 2;

  const deg = Math.abs(a) * 180 / Math.PI;

if (deg < 22) return 1;   // FRONT
if (deg < 55) return 2;   // FRONT-SIDE
if (deg < 105) return 3;  // SIDE
if (deg < 145) return 4;  // BACK-SIDE
return 5;                 // BACK              // BACK
}
function getHeadFacingFromPose(rawPoints, cam) {
  const skull = rawPoints?.ID04;
  const neck = rawPoints?.ID03;
  const mouth = rawPoints?.ID18;

  if (!skull || !neck || !mouth) {
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

  // 顔中心（首～頭の中間）
  const cx = (rs.x + rn.x) * 0.5;
  const cz = (rs.z + rn.z) * 0.5;

  // mouth が頭中心より
  // zで手前にあれば正面寄り、奥なら背面寄り
  const dz = rm.z - cz;

  // xずれで左右向きの気配を見る
  const dx = rm.x - cx;

  // 正面↔背面の強さ
  const frontness = clamp((dz + 0.12) / 0.24, 0, 1); // 0=背面, 1=正面
  const backness = 1 - frontness;

  // 横向きの強さ
  const sideness = clamp(Math.abs(dx) / 0.10, 0, 1);

  let facing = 3; // SIDE 基本

  if (frontness > 0.72 && sideness < 0.38) {
    facing = 1; // FRONT
  } else if (frontness > 0.52) {
    facing = 2; // FRONT-SIDE
  } else if (backness > 0.72 && sideness < 0.38) {
    facing = 5; // BACK
  } else if (backness > 0.52) {
    facing = 4; // BACK-SIDE
  } else {
    facing = 3; // SIDE
  }

  return {
    facing,
    lr: dx >= 0 ? 1 : -1,
    frontness,
    sideness
  };
}
function vecSub(a, b) {
  return {
    x: (a?.x || 0) - (b?.x || 0),
    y: (a?.y || 0) - (b?.y || 0),
    z: (a?.z || 0) - (b?.z || 0)
  };
}

function vecDot(a, b) {
  return (a.x * b.x) + (a.y * b.y) + (a.z * b.z);
}

function vecCross(a, b) {
  return {
    x: a.y * b.z - a.z * b.y,
    y: a.z * b.x - a.x * b.z,
    z: a.x * b.y - a.y * b.x
  };
}

function vecLen(v) {
  return Math.hypot(v.x, v.y, v.z);
}

function vecNormalize(v) {
  const len = vecLen(v) || 1;
  return {
    x: v.x / len,
    y: v.y / len,
    z: v.z / len
  };
}

function buildRootBasis(points) {
  const pelvis = points?.ID10;
  const chest  = points?.ID02;
  const rHip   = points?.ID12;
  const lHip   = points?.ID13;
  const genital = points?.ID21;
  const anus = points?.ID22;

  if (!pelvis || !chest || !rHip || !lHip) return null;

  const rightRaw = vecSub(rHip, lHip);
  const upRaw = vecSub(chest, pelvis);

  let right = vecNormalize(rightRaw);
  let up = vecNormalize(upRaw);

  // 仮forward
  let forward = vecNormalize(vecCross(up, right));

  // 前後の符号を genital / anus で決める
  if (genital && anus) {
    const frontHint = vecNormalize(vecSub(genital, anus));
    if (vecDot(forward, frontHint) < 0) {
      forward = {
        x: -forward.x,
        y: -forward.y,
        z: -forward.z
      };
    }
  }

  // 直交補正
  right = vecNormalize(vecCross(forward, up));
  up = vecNormalize(vecCross(right, forward));

  return {
    origin: pelvis,
    right,
    up,
    forward
  };
}
function getRootFacingFromBasis(points, cam) {
  const basis = buildRootBasis(points);
  if (!basis) {
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

  if (forwardZ > 0.55 && sideX < 0.45) {
    facing = 1; // FRONT
  } else if (forwardZ > 0.18) {
    facing = 2; // FRONT-SIDE
  } else if (forwardZ < -0.55 && sideX < 0.45) {
    facing = 5; // BACK
  } else if (forwardZ < -0.18) {
    facing = 4; // BACK-SIDE
  } else {
    facing = 3; // SIDE
  }

  return {
    facing,
    score: forwardZ,
    basis
  };
}
function getFacingFromForwardVec(forward, cam) {
  if (
    !forward ||
    !Number.isFinite(forward.x) ||
    !Number.isFinite(forward.y) ||
    !Number.isFinite(forward.z)
  ) {
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

  if (forwardZ > 0.55 && sideX < 0.45) {
    facing = 1; // FRONT
  } else if (forwardZ > 0.18) {
    facing = 2; // FRONT-SIDE
  } else if (forwardZ < -0.55 && sideX < 0.45) {
    facing = 5; // BACK
  } else if (forwardZ < -0.18) {
    facing = 4; // BACK-SIDE
  } else {
    facing = 3; // SIDE
  }

  return {
    facing,
    score: forwardZ
  };
}

function getBodyFacingFromMetaOrPose(points, meta, cam) {
  const metaForward = meta?.body?.forward;

  // meta があるなら viewer はそれを絶対優先
  if (
    metaForward &&
    Number.isFinite(metaForward.x) &&
    Number.isFinite(metaForward.y) &&
    Number.isFinite(metaForward.z)
  ) {
    const info = getFacingFromForwardVec(metaForward, cam);
    return {
      ...info,
      source: 'meta'
    };
  }

  // meta が無い時だけ pose から推定
  const fallback = getRootFacingFromBasis(points, cam);
  return {
    ...fallback,
    source: 'pose'
  };
}
function getMetaBasis(metaPart, fallbackForward, fallbackUp) {
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

  // forward と up がほぼ平行なら up を補正
  if (Math.abs(vecDot(forward, up)) > 0.98) {
    up = Math.abs(forward.y) < 0.9
      ? { x: 0, y: 1, z: 0 }
      : { x: 0, y: 0, z: 1 };
  }

  let right = vecNormalize(vecCross(forward, up));

  if (vecLen(right) < 0.0001) {
    right = { x: 1, y: 0, z: 0 };
  }

  up = vecNormalize(vecCross(right, forward));

  return { forward, up, right };
}

let headDraw;  // ← ここでグローバル宣言（これが大事！）

function drawPose() {
  const panel = getOrCreatePanel();
  const canvas = panel.querySelector('#' + CANVAS_ID);
  if (!canvas) return;

  const ctx = canvas.getContext('2d');
  if (!ctx) return;

  const torsoColor = '#cfcfcf';
  const bodyColor = '#e6e6e6';
  const pelvisLineColor = 'rgba(255,179,199,0.18)';
  const headColor = '#f6d6cc';
  const handColor = '#ffd6de';
  const footColor = '#f2b8a8';

  ctx.clearRect(0, 0, canvas.width, canvas.height);
  ctx.fillStyle = '#0b0b0b';
  ctx.fillRect(0, 0, canvas.width, canvas.height);

  const scenePoints = getPosePointsWithSceneOffset(pose.points || {});
  const P = computeLayout(scenePoints, canvas.width, canvas.height);

  const head = P.ID04;
  const neck = P.ID03;
  const chest = P.ID02;
  const pelvis = P.ID10;
const hipR = P.ID12; // 右股関節
const hipL = P.ID13; // 左股関節

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

  if (!head || !neck || !chest || !pelvis || !rHip || !lHip) {
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

  const headBasis = getMetaBasis(
    poseSceneMeta?.head,
    bodyBasis.forward,
    bodyBasis.up
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

function drawHeadBlock() {
  const skullTop = scenePoints['ID04'];
  const neck = scenePoints['ID03'];
  if (!skullTop || !neck) return;

  const metaHead = poseSceneMeta?.head;
  let f = metaHead?.forward || { x: 0, y: 0, z: 1 };

  const normalize = (v) => {
    const l = Math.hypot(v.x, v.y, v.z) || 1;
    return { x: v.x / l, y: v.y / l, z: v.z / l };
  };

  f = normalize(f);

  // 頭の中心 = 頭頂(ID04) と 首(ID03) の中点
  const headCenter = {
    x: (skullTop.x + neck.x) * 0.5,
    y: (skullTop.y + neck.y) * 0.5,
    z: (skullTop.z + neck.z) * 0.5
  };

  // 首方向ベクトル = 頭中心 → 首
  let neckDir = {
    x: neck.x - headCenter.x,
    y: neck.y - headCenter.y,
    z: neck.z - headCenter.z
  };
  neckDir = normalize(neckDir);

  // 口点 = 頭中心 + 前方向に少し + 首方向に少し
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
  if (!head2D || !mouth2D) return;

  const cam = getViewCamera(INTERNAL_CAMERA);
  const headR = rotatePoint(headCenter, cam);
  const mouthR = rotatePoint(mouthPoint, cam);

const headRad = headRadius * 1.15;
const faceRad = headRad * 0.75;

  const mouthFront = mouthR.z > headR.z;

  if (mouthFront) {
    drawCircle(ctx, head2D, headRad, '#8b5a2b');
    drawCircle(ctx, mouth2D, faceRad, '#f4c7a1');
  } else {
    drawCircle(ctx, mouth2D, faceRad, '#f4c7a1');
    drawCircle(ctx, head2D, headRad, '#8b5a2b');
  }
}

function drawTorsoBlock() {
  if (sideStrength > 0.72) {
    drawBodySurfaceThick(ctx, P, scenePoints, bodyBasis);
  } else {
    drawBodySurface(ctx, P, scenePoints, bodyBasis);
  }

  drawShoulderPeak(ctx, lShoulderDraw, shoulderNeckMid, rShoulderDraw, armWidth * 0.72, bodyColor);
}
function drawBreastBlock() {
  const breastRefs = {
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
    bodyColor,
    breastRefs,
    INTERNAL_CAMERA,
    currentScale,
    view,
    bodyBasis,
    bodyFacing
  );

  drawBreastBridge(ctx, P, bodyColor, view);
}

const breastDepth =
  (() => {
    const ids = ['ID19', 'ID20', 'ID23', 'ID24', 'ID25', 'ID26', 'ID27'];
    let sum = 0;
    let count = 0;
    for (const id of ids) {
      const p = scenePoints[id];
      if (!p) continue;
      const r = rotatePoint(p, getViewCamera(INTERNAL_CAMERA));
      sum += r.z;
      count++;
    }
    return count ? (sum / count) : torsoDepth;
  })();

  function drawLegBlock() {
    const hipRadius = thighWidth * lerp(0.90, 0.94, sideStrength);
    const rKneeDraw = spreadSidePoint(rKnee, 1, legWidth * 1.20, view);
    const lKneeDraw = spreadSidePoint(lKnee, -1, legWidth * 1.20, view);
    const rHeelDraw = spreadSidePoint(rHeel, 1, legWidth * 1.60, view);
    const lHeelDraw = spreadSidePoint(lHeel, -1, legWidth * 1.60, view);

    drawCircle(ctx, { x: rHip.x, y: rHip.y }, hipRadius * 0.95, footColor);
    drawCircle(ctx, { x: lHip.x, y: lHip.y }, hipRadius * 0.95, footColor);

    const pantsColor = 'rgba(140, 190, 240, 0.5)';
    drawCrotchFill(ctx, pelvis, rHip, lHip, legWidth, pantsColor);

    if (rKneeDraw && rHeelDraw) {
      drawCapsule(ctx, rHip, rKneeDraw, thighWidthDraw, footColor);
      drawCapsule(ctx, rKneeDraw, rHeelDraw, legWidthDraw * 0.95, footColor);
      drawCircle(ctx, rHeelDraw, legWidthDraw * 0.62, footColor);
    }

    if (lKneeDraw && lHeelDraw) {
      drawCapsule(ctx, lHip, lKneeDraw, thighWidthDraw, footColor);
      drawCapsule(ctx, lKneeDraw, lHeelDraw, legWidthDraw * 0.95, footColor);
      drawCircle(ctx, lHeelDraw, legWidthDraw * 0.62, footColor);
    }

    if (bodyFacing === 1 || bodyFacing === 2) {
      drawPelvisDiamond(ctx, rHip, genital, lHip, anus, pelvisLineColor);
    } else if (bodyFacing === 4 || bodyFacing === 5) {
      drawCenterLine(ctx, rHip, lHip, genital, anus, 'rgba(140,220,255,0.95)');
    }
  }

  function drawArmBlock() {
    if (rShoulderDraw && rElbow) {
      drawCapsule(ctx, rShoulderDraw, rElbow, armWidthDraw, handColor);
    }
    if (rElbow && rWrist) {
      drawCapsule(ctx, rElbow, rWrist, armWidthDraw * 0.92, handColor);
      drawCircle(ctx, rWrist, armWidthDraw * 0.62, '#eab899');
    }

    if (lShoulderDraw && lElbow) {
      drawCapsule(ctx, lShoulderDraw, lElbow, armWidthDraw, handColor);
    }
    if (lElbow && lWrist) {
      drawCapsule(ctx, lElbow, lWrist, armWidthDraw * 0.92, handColor);
      drawCircle(ctx, lWrist, armWidthDraw * 0.62, '#eab899');
    }
  }

  const drawParts = [];

  // 各パーツの depth を「回転後のZ」で決める
  const headDepth =
    (() => {
      const ids = ['ID18', 'ID04', 'ID03'];
      let sum = 0;
      let count = 0;
      for (const id of ids) {
        const p = scenePoints[id];
        if (!p) continue;
        const r = rotatePoint(p, getViewCamera(INTERNAL_CAMERA));
        sum += r.z;
        count++;
      }
      return count ? (sum / count) : 0;
    })();

  const torsoDepth =
    (() => {
      const ids = ['ID02', 'ID01', 'ID10', 'ID19', 'ID20', 'ID27'];
      let sum = 0;
      let count = 0;
      for (const id of ids) {
        const p = scenePoints[id];
        if (!p) continue;
        const r = rotatePoint(p, getViewCamera(INTERNAL_CAMERA));
        sum += r.z;
        count++;
      }
      return count ? (sum / count) : 0;
    })();

  const armDepth =
    (() => {
      const ids = ['ID05', 'ID06', 'ID07', 'ID08', 'ID09', 'ID11'];
      let sum = 0;
      let count = 0;
      for (const id of ids) {
        const p = scenePoints[id];
        if (!p) continue;
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
      for (const id of ids) {
        const p = scenePoints[id];
        if (!p) continue;
        const r = rotatePoint(p, getViewCamera(INTERNAL_CAMERA));
        sum += r.z;
        count++;
      }
      return count ? (sum / count) : 0;
    })();
  drawFloorPad(ctx, scenePoints, canvas.width, canvas.height);
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
  name: 'breasts',
  depth: breastDepth,
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
      if (neckTop2D && neckBottom2D) {
        drawNeckSimple(ctx, neckTop2D, neckBottom2D, neckWidth, handColor);
      }
      drawHeadBlock();
    }
  });

  // 奥→手前の順で描く
  drawParts.sort((a, b) => a.depth - b.depth);

  // デバッグ用
  //console.log(
  //  '[PART Z ORDER]',
  //  drawParts.map(v => `${v.name}: ${v.depth.toFixed(3)}`).join(' | ')
  //);

  for (const part of drawParts) {
    part.draw();
  }


drawPointDots(ctx, P, scenePoints);
drawPointLabels(ctx, P, scenePoints);

  setStatus(`status: ready | meta ${poseSceneMeta ? 'on' : 'off'} | body=${bodyFacing} head=${headFacing}`);
}

function drawBodySurface(ctx, P, rawPoints, bodyBasis) {
  if (!P || !rawPoints) return;

  const shoulderR = P.ID05;
  const shoulderL = P.ID06;
  const spine     = P.ID01;
  const pelvis    = P.ID10;
  const hipR      = P.ID12;
  const hipL      = P.ID13;

  if (!shoulderR || !shoulderL || !spine || !pelvis || !hipR || !hipL) return;

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

  //⭐腰の幅
const pelvisExpand = 1.0;
  const minWidthPx = 10;

  const width = Math.max(
    minWidthPx,
    pelvisWidth3D * pelvisExpand * currentScale
  );


// === ID01を3Dでコピーして左右にずらす（最小版） ===
const rawSpine = rawPoints.ID01;

const right = bodyBasis?.right || { x: 1, y: 0, z: 0 };

const halfWidth3D = 0.18;

// 候補を2つ作る
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

// どちらが左股関節側か / 右股関節側かで決める
const distAtoL = dist3D(candA, rawHipL);
const distAtoR = dist3D(candA, rawHipR);

// candA が左股関節に近ければ A=Left, B=Right
const spineL3 = distAtoL <= distAtoR ? candA : candB;
const spineR3 = distAtoL <= distAtoR ? candB : candA;

// 既存の投影系に乗せるため、一時的にcomputeLayoutを使う
const layoutMetrics = getLayoutMetrics(rawPoints, cw, ch);

const spineL = projectPointToScreen(spineL3, cw, ch, layoutMetrics);
const spineR = projectPointToScreen(spineR3, cw, ch, layoutMetrics);


//⭐下辺の広がり
const bodyHipWidthScale = 1.8;

  const bodyHipR = {
    x: pelvis.x + (hipR.x - pelvis.x) * bodyHipWidthScale,
    y: hipR.y
  };

  const bodyHipL = {
    x: pelvis.x + (hipL.x - pelvis.x) * bodyHipWidthScale,
    y: hipL.y
  };

  const ctrlShoulderL = {
    x: shoulderL.x + (spineL.x - shoulderL.x) * 0.5,
    y: shoulderL.y + (spineL.y - shoulderL.y) * 0.5
  };
//⭐カーブ
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

  ctx.fillStyle = '#888';
  ctx.fill();


    // === DEBUG: spineL / spineR ===
ctx.fillStyle = 'red';

ctx.beginPath();
ctx.arc(spineL.x, spineL.y, 4, 0, Math.PI * 2);
ctx.fill();

ctx.beginPath();
ctx.arc(spineR.x, spineR.y, 4, 0, Math.PI * 2);
ctx.fill();

// ラベル（任意）
ctx.fillStyle = '#fff';
ctx.font = '10px monospace';
ctx.fillText('L', spineL.x + 4, spineL.y - 4);
ctx.fillText('R', spineR.x + 4, spineR.y - 4);




}
function drawBodySurfaceThick(ctx, P, rawPoints, bodyBasis) {
  if (!P || !rawPoints) return;

  const shoulderR = P.ID05;
  const shoulderL = P.ID06;
  const pelvis    = P.ID10;
  const hipR      = P.ID12;
  const hipL      = P.ID13;

  if (!shoulderR || !shoulderL || !pelvis || !hipR || !hipL) return;

  const panel = document.getElementById(PANEL_ID);
  const canvas = panel?.querySelector('#' + CANVAS_ID);
  const cw = canvas?.width || 500;
  const ch = canvas?.height || 500;

  const rawSpine = rawPoints.ID01;
  const rawHipR = rawPoints.ID12;
  const rawHipL = rawPoints.ID13;
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

  const bodyHipWidthScale = 1.5;
  let bodyHipR = {
    x: pelvis.x + (hipR.x - pelvis.x) * bodyHipWidthScale,
    y: hipR.y
  };

  let bodyHipL = {
    x: pelvis.x + (hipL.x - pelvis.x) * bodyHipWidthScale,
    y: hipL.y
  };

  // 横向きのときだけ、最小厚みを持たせる
  const bodyView = getBodyViewInfo(INTERNAL_CAMERA);
  const thickT = smoothstep01((bodyView.sideStrength - 0.55) / 0.30);

  const spineMid = midpoint(spineL, spineR);
  const hipMid = midpoint(bodyHipL, bodyHipR);

  const spineHalfMin = lerp(0, 16, thickT);
  const hipHalfMin   = lerp(0, 20, thickT);

  if (spineMid) {
    spineL = {
      x: spineMid.x - spineHalfMin,
      y: spineL.y
    };
    spineR = {
      x: spineMid.x + spineHalfMin,
      y: spineR.y
    };
  }

  if (hipMid) {
    bodyHipL = {
      x: hipMid.x - hipHalfMin,
      y: bodyHipL.y
    };
    bodyHipR = {
      x: hipMid.x + hipHalfMin,
      y: bodyHipR.y
    };
  }

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
  ctx.fillStyle = '#888';
  ctx.fill();
}

    function drawBodyCapsule(ctx, P, rawPoints) {
  const chest = P.ID02;
  const pelvis = P.ID10;
  const hipR = P.ID12;
  const hipL = P.ID13;
  const shoulderR = P.ID05;
  const shoulderL = P.ID06;

  if (!chest || !pelvis || !hipR || !hipL || !shoulderR || !shoulderL) return;

  const chestCenter = {
    x: (shoulderR.x + shoulderL.x) * 0.5,
    y: (shoulderR.y + shoulderL.y) * 0.5
  };

  const pelvisCenter = {
    x: (hipR.x + hipL.x) * 0.5,
    y: (hipR.y + hipL.y) * 0.5
  };

  const shoulderWidth = dist2D(shoulderR, shoulderL);
  const hipWidth = dist2D(hipR, hipL);

  const bodyWidth = Math.max(18, Math.min(90, Math.max(shoulderWidth * 0.55, hipWidth * 1.35)));

  drawCapsule(ctx, chestCenter, pelvisCenter, bodyWidth, '#888');
}




function projectPointToScreen(p, width, height, layoutMetrics) {
  const r = projectPoint(p);

  const scale = layoutMetrics.fitScale * (INTERNAL_CAMERA.scale || 1);
  const tx = INTERNAL_CAMERA.tx || 0;
  const ty = INTERNAL_CAMERA.ty || 0;

  return {
    x: width / 2 + tx + (r.x - layoutMetrics.centerX) * scale,
    y: height / 2 + ty - (r.y - layoutMetrics.centerY) * scale
  };
}
function drawShoulderBridge(ctx, leftShoulder, rightShoulder, width, color) {
  if (!leftShoulder || !rightShoulder) return;

  ctx.strokeStyle = color;
  ctx.lineWidth = width;
  ctx.lineCap = 'round';
  ctx.lineJoin = 'round';
  ctx.beginPath();
  ctx.moveTo(leftShoulder.x, leftShoulder.y);
  ctx.lineTo(rightShoulder.x, rightShoulder.y);
  ctx.stroke();
}
function syncPose() {
  const text = document.body?.textContent || '';
  const jsonText = extractLatestPoseJsonBlock(text);
  const metaText = extractLatestPoseSceneMetaBlock(text);

  if (!jsonText && !metaText) {
    setStatus('status: syncPose no block');
    return;
  }

  let parsed = null;
  let parsedMeta = null;

  if (jsonText) {
    try {
      parsed = JSON.parse(jsonText);
    } catch (e) {
      setStatus('status: json parse error');
      return;
    }
  }

  if (metaText) {
    try {
      parsedMeta = JSON.parse(metaText);
    } catch (e) {
      setStatus('status: meta parse error');
      return;
    }
  }

  try {
    ensureCameraDefaults();

    if (parsedMeta) {
      poseSceneMeta = parsedMeta;
    }

    if (parsed) {
      if (typeof parsed.camera?.scale === 'number') INTERNAL_CAMERA.scale = parsed.camera.scale;
      if (typeof parsed.camera?.tx === 'number') INTERNAL_CAMERA.tx = parsed.camera.tx;
      if (typeof parsed.camera?.ty === 'number') INTERNAL_CAMERA.ty = parsed.camera.ty;

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

    if (forceFrontView) {
      INTERNAL_CAMERA.yaw = 0;
      INTERNAL_CAMERA.pitch = 0;
    }

    fixedFitScale = null;
    drawPose();

    const p = pose.points?.ID14;
    setStatus(
      `status: sync ok knee=${p?.x?.toFixed?.(2)},${p?.y?.toFixed?.(2)},${p?.z?.toFixed?.(2)} grounded=${poseSceneMeta?.scene?.isGrounded ? '1' : '0'} jump=${poseSceneMeta?.scene?.isJumping ? '1' : '0'}`
    );
  } catch (e) {
    console.error('[POSE syncPose error]', e);
    setStatus('status: sync exception');
  }
}

function checkAndAutoSyncPose() {
  const text = document.body?.textContent || '';
  const block = extractLatestPoseJsonBlock(text);
  const metaBlock = extractLatestPoseSceneMetaBlock(text);

  // 前回と同じなら同期しない
  if (block === lastPoseBlock && metaBlock === lastPoseMetaBlock) {
    return;
  }

  // 手動操作中は同期しない
  if (isDragging || isPanning || isPanelDragging) return;

  lastPoseBlock = block || '';
  lastPoseMetaBlock = metaBlock || '';

//  console.log('[POSE AUTO SYNC]');
  syncPose();
}
function startAutoSyncObserver() {
  if (autoSyncObserverStarted) return;
  if (!document.body) return;

  autoSyncObserverStarted = true;

  const observer = new MutationObserver(() => {
    clearTimeout(autoSyncTimer);
    autoSyncTimer = setTimeout(() => {
      checkAndAutoSyncPose();
    }, 250);

    clearTimeout(removePoseTimer);
    removePoseTimer = setTimeout(() => {
      if (!stripPoseJsonOnSend) return;

      const text = document.body?.textContent || '';
if (
  !text.includes('<POSE_JSON_START>') &&
  !text.includes('<POSE_SCENE_META_START>')
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
function installGlobalPointerHandlers() {
  if (globalPointerHandlersInstalled) return;
  globalPointerHandlersInstalled = true;

  document.addEventListener('mousemove', (e) => {
    const dx = e.clientX - lastMouseX;
    const dy = e.clientY - lastMouseY;

    lastMouseX = e.clientX;
    lastMouseY = e.clientY;

    if (isPanelDragging) {
      const panel = document.getElementById(PANEL_ID);
      if (!panel) return;

      const left = parseInt(panel.style.left || '0', 10);
      const top = parseInt(panel.style.top || '0', 10);
      panel.style.left = `${left + dx}px`;
      panel.style.top = `${top + dy}px`;
      return;
    }

    if (isDragging) {
      if (forceFrontView) {
        setForceFrontUI(false, true);
      }

INTERNAL_CAMERA.yaw += dx * 0.01;
INTERNAL_CAMERA.pitch = clampPitch(INTERNAL_CAMERA.pitch + dy * 0.01);
drawPose();
    }

    if (isPanning) {
INTERNAL_CAMERA.tx += dx;
INTERNAL_CAMERA.ty += dy;
drawPose();
    }
  });

  document.addEventListener('mouseup', () => {
    isDragging = false;
    isPanning = false;
    isPanelDragging = false;
  });
}

function installSendInterceptor() {
  if (sendInterceptorInstalled) return;
  sendInterceptorInstalled = true;

  document.addEventListener('click', (e) => {
    const target = e.target;
    if (!(target instanceof Element)) return;

    const sendBtn = target.closest('button');
    if (!sendBtn) return;

    const label =
      (sendBtn.getAttribute('aria-label') || '') + ' ' +
      (sendBtn.textContent || '');

    if (/send|送信/i.test(label)) {
      stripPoseJsonFromComposer();
    }
  }, true);

document.addEventListener('keydown', (e) => {
  if (e.key !== 'Enter') return;
  if (e.shiftKey || e.ctrlKey || e.altKey || e.metaKey) return;

  const composer = findGeminiComposer();
  if (!composer) return;

  stripPoseJsonFromComposer();
}, true);
}

  function getOrCreatePanel() {
    let panel = document.getElementById(PANEL_ID);
    if (panel) return panel;

    panel = document.createElement('div');
    panel.id = PANEL_ID;
    panel.style.position = 'fixed';
    panel.style.left = '8px';
    panel.style.top = '80px';
    panel.style.width = '190px';
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
      if (!body) return;

      isOpen = !isOpen;
      body.style.display = isOpen ? 'block' : 'none';
      toggleBtn.textContent = isOpen ? 'CLOSE' : 'OPEN';
    });
headerRow.addEventListener('mousedown', (e) => {
  // CLOSEボタン押下時はパネル移動しない
  if (e.target instanceof HTMLElement && e.target.tagName === 'BUTTON') return;

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

  if (e.button === 1 || (e.button === 0 && e.shiftKey)) {
    isPanning = true;
  } else if (e.button === 0) {
    isDragging = true;
  }
});
canvas.addEventListener('contextmenu', (e) => {
  e.preventDefault();
});
    canvas.addEventListener('auxclick', (e) => {
      if (e.button === 1) e.preventDefault();
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

  if (newUserScale < 0.2) newUserScale = 0.2;
  if (newUserScale > 5) newUserScale = 5;

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

    bodyWrap.appendChild(canvas);

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

  if (forceFrontView) {
    applyForcedViewIfNeeded();
  }

  drawPose();
  setStatus(`status: force front ${forceFrontView ? 'on' : 'off'}`);
});

const forceFrontText = document.createElement('span');
forceFrontText.textContent = 'FORCE FRONT';

viewOptionRow.appendChild(forceFrontCheckbox);
viewOptionRow.appendChild(forceFrontText);


const resetBtn = makeButton('RESET', () => resetCamera());
const syncBtn = makeButton('SYNC', () => syncPose());


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
    camRow.style.display = 'flex';
    camRow.style.flexWrap = 'wrap';
    camRow.style.gap = '4px';
    camRow.style.marginTop = '6px';

camRow.appendChild(makeButton('FRONT', () => {
  setForceFrontUI(true, true);
  resetCamera(false);
  applyForcedViewIfNeeded();
  drawPose();
}));

camRow.appendChild(makeButton('BACK', () => {
  setForceFrontUI(false, true);
  resetCamera(false);
  INTERNAL_CAMERA.yaw = Math.PI;
  drawPose();
}));

camRow.appendChild(makeButton('LEFT', () => {
  setForceFrontUI(false, true);
  resetCamera(false);
  INTERNAL_CAMERA.yaw = -Math.PI / 2;
  drawPose();
}));

camRow.appendChild(makeButton('RIGHT', () => {
  setForceFrontUI(false, true);
  resetCamera(false);
  INTERNAL_CAMERA.yaw = Math.PI / 2;
  drawPose();
}));

camRow.appendChild(makeButton('TOP', () => {
  setForceFrontUI(false, true);
  resetCamera(false);
  INTERNAL_CAMERA.pitch = Math.PI * 0.495;
  drawPose();
}));

camRow.appendChild(makeButton('BOTTOM', () => {
  setForceFrontUI(false, true);
  resetCamera(false);
  INTERNAL_CAMERA.pitch = -Math.PI * 0.495;
  drawPose();
}));
bodyWrap.appendChild(camRow);
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

function boot() {
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
function updateStripButtonLabel(btn) {
  if (!btn) return;
  btn.textContent = stripPoseJsonOnSend ? 'STRIP: ON' : 'STRIP: OFF';
}
function findGeminiComposer() {
  // Geminiの最新構造に対応した強化版
  const candidates = [
    document.querySelector('div[contenteditable="true"][role="textbox"]'),
    document.querySelector('rich-textarea div[contenteditable="true"]'),
    document.querySelector('div[contenteditable="true"]'),
    document.querySelector('[data-placeholder*="Reply to"]'),
    document.querySelector('[data-placeholder*="メッセージを入力"]'), // 日本語の場合
  ];

  for (const el of candidates) {
    if (el && el.isContentEditable) {
      console.log('[POSE] Composer found:', el);
      return el;
    }
  }

  // Shadow DOM内も少し探す
  const rich = document.querySelector('rich-textarea');
  if (rich && rich.shadowRoot) {
    const shadowEl = rich.shadowRoot.querySelector('div[contenteditable="true"]');
    if (shadowEl) {
      console.log('[POSE] Composer found in Shadow DOM');
      return shadowEl;
    }
  }

  return null;
}

function stripPoseJsonFromComposer() {
  if (!stripPoseJsonOnSend) return false;

  const composer = findGeminiComposer();
  if (!composer) {
    console.warn('[POSE] Composer not found - strip skipped');
    setStatus('status: strip failed (no composer)');
    return false;
  }

  const before = (composer.innerText || composer.textContent || '').trim();
  const after = removePoseJsonBlocks(before);

if (
  before === after ||
  (
    !before.includes('<POSE_JSON_START>') &&
    !before.includes('<POSE_SCENE_META_START>')
  )
) {
  return false;
}

  console.log('[POSE] Stripping... before:', before.length, '→ after:', after.length);

  try {
    composer.focus();

    // 方法1: 全選択して削除 → クリーンなテキストを貼り付け（現在最も安定）
    const range = document.createRange();
    range.selectNodeContents(composer);
    const sel = window.getSelection();
    sel.removeAllRanges();
    sel.addRange(range);

    // クリップボード経由で安全に貼り付け
    navigator.clipboard.writeText(after).then(() => {
      document.execCommand('paste');
      console.log('[POSE] Stripped via clipboard paste');
    }).catch(() => {
      // フォールバック
      composer.innerText = after;
      console.log('[POSE] Stripped via innerText fallback');
    });

    // イベントを多めに送る
    setTimeout(() => {
      composer.dispatchEvent(new InputEvent('input', { bubbles: true, cancelable: true }));
      composer.dispatchEvent(new InputEvent('beforeinput', { bubbles: true }));
      composer.dispatchEvent(new Event('change', { bubbles: true }));
      composer.dispatchEvent(new KeyboardEvent('input', { bubbles: true }));
    }, 30);

    setStatus('status: pose json stripped (enhanced)');
    return true;

  } catch (err) {
    console.error('[POSE STRIP CRITICAL ERROR]', err);
    composer.innerText = after;  // 最後の手段
    setStatus('status: stripped (emergency fallback)');
    return true;
  }
}


function removePoseJsonBlocks(text) {
  if (!text) return text;
  return text
    .replace(/<POSE_JSON_START>[\s\S]*?<POSE_JSON_END>\s*/g, '')
    .replace(/<POSE_SCENE_META_START>[\s\S]*?<POSE_SCENE_META_END>\s*/g, '')
    .trim();
}
function removePoseJsonBlocksFromDom(root) {
  if (!root) return;

  const walker = document.createTreeWalker(root, NodeFilter.SHOW_TEXT);
  const textNodes = [];
  let node;

  while ((node = walker.nextNode())) {
    textNodes.push(node);
  }

  for (const textNode of textNodes) {
    const original = textNode.nodeValue;
const replaced = original
  .replace(/<POSE_JSON_START>[\s\S]*?<POSE_JSON_END>\s*/g, '')
  .replace(/<POSE_SCENE_META_START>[\s\S]*?<POSE_SCENE_META_END>\s*/g, '');

    if (replaced !== original) {
      textNode.nodeValue = replaced;
    }
  }
}
    function midpoint(a, b) {
  if (!a || !b) return null;
  return {
    x: (a.x + b.x) / 2,
    y: (a.y + b.y) / 2
  };
}

function dist2D(a, b) {
  if (!a || !b) return 0;
  return Math.hypot(b.x - a.x, b.y - a.y);
}
function dist3D(a, b) {
  if (!a || !b) return 0;
  return Math.hypot(
    b.x - a.x,
    b.y - a.y,
    b.z - a.z
  );
}
function drawRoundLimb(ctx, a, b, width, color) {
  if (!a || !b) return;
  ctx.strokeStyle = color;
  ctx.lineWidth = width;
  ctx.lineCap = 'round';
  ctx.lineJoin = 'round';
  ctx.beginPath();
  ctx.moveTo(a.x, a.y);
  ctx.lineTo(b.x, b.y);
  ctx.stroke();
}

function drawCircle(ctx, p, r, color) {
  if (!p) return;
  ctx.fillStyle = color;
  ctx.beginPath();
  ctx.arc(p.x, p.y, r, 0, Math.PI * 2);
  ctx.fill();
}

function drawCapsule(ctx, a, b, width, color) {
  if (!a || !b) return;

  const dx = b.x - a.x;
  const dy = b.y - a.y;
  const len = Math.hypot(dx, dy);

  if (len < 0.001) {
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

function drawTorso(ctx, chest, pelvis, shoulderR, shoulderL, hipR, hipL, color) {
  if (!chest || !pelvis) return;

  const shoulderWidth = dist2D(shoulderR, shoulderL);
  const hipWidth = dist2D(hipR, hipL);

  const torsoWidth = Math.max(16, Math.min(40, Math.max(shoulderWidth * 0.55, hipWidth * 1.2)));
  drawCapsule(ctx, chest, pelvis, torsoWidth, color);
}
function drawNeckSimple(ctx, topPoint, bottomPoint, neckWidth, color) {
  if (!topPoint || !bottomPoint) return;
  drawCapsule(ctx, topPoint, bottomPoint, neckWidth, color);
}
function drawSoftTorso(ctx, chest, pelvis, shoulderR, shoulderL, hipR, hipL, color) {
  if (!shoulderR || !shoulderL || !hipR || !hipL) return;

  const chestCenter = midpoint(shoulderR, shoulderL) || chest;
  const hipCenter = midpoint(hipR, hipL) || pelvis;
  if (!chestCenter || !hipCenter) return;

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

  // 下の広がりは今ぐらいで維持
  const hipOut = 1.10;

  const botR = {
    x: hipR.x + (hipCenter.x - hipR.x) * hipInset + (hipR.x - hipCenter.x) * hipOut,
    y: hipR.y + (hipCenter.y - hipR.y) * hipInset
  };
  const botL = {
    x: hipL.x + (hipCenter.x - hipL.x) * hipInset + (hipL.x - hipCenter.x) * hipOut,
    y: hipL.y + (hipCenter.y - hipL.y) * hipInset
  };

  // くびれ位置を少し上へ
   // くびれ位置：少しだけ上へ戻す
  const waistY = lerp(chestCenter.y, hipCenter.y, 0.80);

  const topHalfWidth = Math.abs(topR.x - topL.x) * 0.5;
  const botHalfWidth = Math.abs(botR.x - botL.x) * 0.5;

  // くびれは今より少しだけ強める
  const waistHalfWidth = Math.min(topHalfWidth, botHalfWidth) * 0.15;

  const waistR = {
    x: chestCenter.x + waistHalfWidth,
    y: waistY
  };
  const waistL = {
    x: chestCenter.x - waistHalfWidth,
    y: waistY
  };

  // 腰のふくらみ開始をもっと下へ
  const hipBlend = 0.84;

  const hipCurveR = {
    x: lerp(waistR.x, botR.x, hipBlend) + (botR.x - waistR.x) * 0.30,
    y: lerp(waistR.y, botR.y, hipBlend)
  };

  const hipCurveL = {
    x: lerp(waistL.x, botL.x, hipBlend) + (botL.x - waistL.x) * 0.30,
    y: lerp(waistL.y, botL.y, hipBlend)
  };

  // 胸→くびれ：下の方で絞る
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

  // 上辺
  ctx.quadraticCurveTo(
    chestCenter.x,
    chestCenter.y - curveY,
    topR.x,
    topR.y
  );

  // 右上 → 右下
  ctx.bezierCurveTo(
    rightUpperCtrl.x, rightUpperCtrl.y,
    hipCurveR.x, hipCurveR.y,
    botR.x, botR.y
  );

  // 下辺
  ctx.quadraticCurveTo(
    hipCenter.x,
    hipCenter.y + curveY * 1.10,
    botL.x,
    botL.y
  );

  // 左下 → 左上
  ctx.bezierCurveTo(
    hipCurveL.x, hipCurveL.y,
    leftUpperCtrl.x, leftUpperCtrl.y,
    topL.x, topL.y
  );

  ctx.closePath();
  ctx.fill();
}
function drawSimpleTorso(ctx, chest, pelvis, shoulderR, shoulderL, hipR, hipL, color, view) {
  if (!shoulderR || !shoulderL || !hipR || !hipL) return;

  const chestCenter = midpoint(shoulderR, shoulderL) || chest;
  const hipCenter = midpoint(hipR, hipL) || pelvis;
  if (!chestCenter || !hipCenter) return;

  const sideStrength = clamp(view?.sideStrength || 0, 0, 1);

  // 横向きほど胴の横幅を細くする
  const sideNarrow = lerp(1.0, 0.38, sideStrength);

  const topHalfBase = Math.abs(shoulderR.x - shoulderL.x) * 0.40;
  const midHalfBase = Math.abs(shoulderR.x - shoulderL.x) * 0.18;
  const botHalfBase = Math.abs(hipR.x - hipL.x) * 1.05;

  const topHalf = Math.max(8, topHalfBase * sideNarrow);
  const midHalf = Math.max(6, midHalfBase * sideNarrow);
  const botHalf = Math.max(10, botHalfBase * sideNarrow);

  const topY = lerp(chestCenter.y, hipCenter.y, 0.01);
  const midY = lerp(chestCenter.y, hipCenter.y, 0.48);
  const botY = lerp(chestCenter.y, hipCenter.y, 0.94);

  const topLeft =  { x: chestCenter.x - topHalf, y: topY };
  const topRight = { x: chestCenter.x + topHalf, y: topY };

  const waistLeft =  { x: chestCenter.x - midHalf, y: midY };
  const waistRight = { x: chestCenter.x + midHalf, y: midY };

  const botLeft =  { x: hipCenter.x - botHalf, y: botY };
  const botRight = { x: hipCenter.x + botHalf, y: botY };

  const curveY = Math.max(8, dist2D(topLeft, topRight) * 0.12);

  ctx.fillStyle = color;
  ctx.beginPath();

  ctx.moveTo(topLeft.x, topLeft.y);

  // 上辺
  ctx.quadraticCurveTo(
    chestCenter.x,
    topY - curveY,
    topRight.x,
    topRight.y
  );

  // 右側
  ctx.bezierCurveTo(
    topRight.x, lerp(topY, midY, 0.45),
    waistRight.x, lerp(midY, botY, 0.35),
    botRight.x, botRight.y
  );

  // 下辺
  ctx.quadraticCurveTo(
    hipCenter.x,
    botY + curveY * 0.7,
    botLeft.x,
    botLeft.y
  );

  // 左側
  ctx.bezierCurveTo(
    waistLeft.x, lerp(midY, botY, 0.35),
    topLeft.x, lerp(topY, midY, 0.45),
    topLeft.x, topLeft.y
  );

  ctx.closePath();
  ctx.fill();
}

function clamp(v, min, max) {
  return Math.max(min, Math.min(max, v));
}

function lerp(a, b, t) {
  return a + (b - a) * t;
}

function mixPoint(a, b, t) {
  if (!a && !b) return null;
  if (!a) return { x: b.x, y: b.y };
  if (!b) return { x: a.x, y: a.y };
  return {
    x: lerp(a.x, b.x, t),
    y: lerp(a.y, b.y, t)
  };
}
function spreadSidePoint(p, sideTag, amount, view) {
  if (!p) return null;

  const sideStrength = view?.sideStrength || 0;
const t = smoothstep01((sideStrength - 0.60) / 0.35);

  return {
    x: p.x + sideTag * amount * t,
    y: p.y
  };
}
function getViewMetrics(rawPoints, cam, currentScale = 1, bodyBasis = null) {
  const pR = rawPoints?.ID05;
  const pL = rawPoints?.ID06;
  const chest = rawPoints?.ID02;
  const pelvis = rawPoints?.ID10;

  if (!pR || !pL || !chest || !pelvis) {
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


function smoothstep01(t) {
  t = clamp(t, 0, 1);
  return t * t * (3 - 2 * t);
}

function drawSmartTorso(ctx, projected, rawPoints, color, cam, refs, view) {
  const baseSideStrength = view?.sideStrength || 0;

  const pitch = Math.abs(cam?.pitch || 0);
  const topViewFactor = clamp((pitch - 0.9) / 0.6, 0, 1);

  const sideStrength = Math.max(
    baseSideStrength,
    topViewFactor * 0.25
  );

const blendStart = 0.10;
const blendEnd = 0.92;

  if (sideStrength <= blendStart) {
    drawSoftTorso(
      ctx,
      refs.chest,
      refs.pelvis,
      refs.rShoulder,
      refs.lShoulder,
      refs.rHip,
      refs.lHip,
      color
    );
    return;
  }

if (sideStrength >= blendEnd) {
  drawSideTorso(ctx, projected, rawPoints, color, cam, refs, view);
  return;
}

const t = smoothstep01((sideStrength - blendStart) / (blendEnd - blendStart));
drawBlendTorso(ctx, projected, rawPoints, color, cam, refs, t, view);
}
function getFrontTorsoShape(P, refs) {
  const chest = refs.chest || P.ID02;
  const pelvis = refs.pelvis || P.ID10;
  const neck = P.ID03;

  const shoulderR = refs.rShoulder;
  const shoulderL = refs.lShoulder;
  const hipR = refs.rHip;
  const hipL = refs.lHip;

  if (!shoulderR || !shoulderL || !hipR || !hipL || !chest || !pelvis || !neck) return null;

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

const hipOut = 0.38; // ←追加

const botR = {
  x: hipR.x + (hipCenter.x - hipR.x) * hipInset + (hipR.x - hipCenter.x) * hipOut,
  y: hipR.y + (hipCenter.y - hipR.y) * hipInset
};
const botL = {
  x: hipL.x + (hipCenter.x - hipL.x) * hipInset + (hipL.x - hipCenter.x) * hipOut,
  y: hipL.y + (hipCenter.y - hipL.y) * hipInset
};

  const waistY = lerp(chestCenter.y, hipCenter.y, 0.44);

  const topHalfWidth = Math.abs(topR.x - topL.x) * 0.5;
  const botHalfWidth = Math.abs(botR.x - botL.x) * 0.5;
  const waistHalfWidth = Math.min(topHalfWidth, botHalfWidth) * 0.58;




  const waistR = {
    x: chestCenter.x + waistHalfWidth,
    y: waistY
  };

  const waistL = {
    x: chestCenter.x - waistHalfWidth,
    y: waistY
  };
const hipBlend = 0.78; // ← 追加（0.5〜0.7で調整）

const hipCurveR = {
  x: lerp(waistR.x, botR.x, hipBlend) + (botR.x - waistR.x) * 0.25,
  y: lerp(waistR.y, botR.y, hipBlend)
};

const hipCurveL = {
  x: lerp(waistL.x, botL.x, hipBlend) + (botL.x - waistL.x) * 0.25,
  y: lerp(waistL.y, botL.y, hipBlend)
};

  const neckWidth = Math.max(10, dist2D(topR, topL) * 0.22);

  const neckFront = {
    x: chestCenter.x + neckWidth * 0.35,
    y: neck.y + curveY * 0.25
  };

  const neckBack = {
    x: chestCenter.x - neckWidth * 0.35,
    y: neck.y + curveY * 0.25
  };

  return {
    neckBack,
    upperBack: topL,
    lowerBack: waistL,
    backBottom: botL,
    frontBottom: botR,
    belly: waistR,
    chestFront: topR,
    neckFront
  };
}

function getSideTorsoShape(P, rawPoints, cam, refs, view) {
  const chest = refs.chest || P.ID02;
  const pelvis = refs.pelvis || P.ID10;
  const neck = P.ID03;

  const genital = P.ID21 || pelvis;
  const anus = P.ID22 || pelvis;
  const breastCenter = P.ID27 || chest;
  const breastLowerR = P.ID25 || breastCenter;
  const breastLowerL = P.ID26 || breastCenter;
  const mouth = P.ID18 || neck;

  if (!chest || !neck || !pelvis) return null;

  const bodyView = getBodyViewInfo(cam);
  const side = bodyView.sideSign;

  const shoulderSpan = dist2D(refs.rShoulder, refs.lShoulder) || 40;
  const hipSpan = dist2D(refs.rHip, refs.lHip) || 24;
  const torsoHeight = Math.max(40, dist2D(chest, pelvis));

  // =========================
  // width 系
  // =========================
  const chestWidthHalf = clamp(shoulderSpan * 0.22, 10, 24);
  const waistWidthHalf = clamp(hipSpan * 0.16, 8, 18);

  // =========================
  // depth 系
  // 幅ではなく torsoHeight ベースで厚みの基準を作る
  // =========================
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

function drawTorsoShape(ctx, shape, color) {
  if (!shape) return;

const shoulderTopMid = {
  x: lerp(shape.upperBack.x, shape.chestFront.x, 0.48),
  y: Math.min(shape.upperBack.y, shape.chestFront.y) - 1.0
};

  ctx.fillStyle = color;
  ctx.beginPath();

  // 背中上端から開始
  ctx.moveTo(shape.upperBack.x, shape.upperBack.y);

  // 背中側
  ctx.quadraticCurveTo(
    shape.upperBack.x, shape.upperBack.y,
    shape.lowerBack.x, shape.lowerBack.y
  );

  // 下側
  ctx.quadraticCurveTo(
    shape.backBottom.x, shape.backBottom.y,
    shape.frontBottom.x, shape.frontBottom.y
  );

  // 前側
  ctx.quadraticCurveTo(
    shape.belly.x, shape.belly.y,
    shape.chestFront.x, shape.chestFront.y
  );

  // 上辺：首で閉じず、肩ライン寄りの補間点で戻す
  ctx.quadraticCurveTo(
    shoulderTopMid.x, shoulderTopMid.y,
    shape.upperBack.x, shape.upperBack.y
  );

  ctx.closePath();
  ctx.fill();
}
function drawBlendTorso(ctx, P, rawPoints, color, cam, refs, t, view) {
  const front = getFrontTorsoShape(P, refs);
  const side = getSideTorsoShape(P, rawPoints, cam, refs, view);

  if (!front && !side) return;
  if (!front) {
    drawTorsoShape(ctx, side, color);
    return;
  }
  if (!side) {
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
function drawSideTorso(ctx, P, rawPoints, color, cam, refs, view) {
  const shape = getSideTorsoShape(P, rawPoints, cam, refs, view);
  drawTorsoShape(ctx, shape, color);
}


function drawBreasts(ctx, projected, rawPoints, color, refs, cam, currentScale, view, bodyBasis, bodyFacing) {
  if (!projected || !rawPoints) return;

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

  if (!nippleR || !nippleL || !rawChest || !rawRShoulder || !rawLShoulder) return;

  // --- サイズ決定（シンプル合成） ---
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

  // --- 中心位置（乳首ベースで単純） ---
  const drop = radius * 0.2;

  const centerR = {
    x: nippleR.x,
    y: nippleR.y + drop
  };

  const centerL = {
    x: nippleL.x,
    y: nippleL.y + drop
  };

  // --- 形状（最小） ---
  const sideStrength = view?.sideStrength || 0;

  const rx = radius * (1 - sideStrength * 0.3);
  const ry = radius;

  // --- 描画（Z順は外で管理される前提） ---
  drawEllipse(ctx, centerR, rx, ry, color);
  drawEllipse(ctx, centerL, rx, ry, color);
}
function drawEllipse(ctx, p, rx, ry, color) {
  if (!p) return;
  ctx.save();
  ctx.beginPath();
  ctx.translate(p.x, p.y);
  ctx.scale(rx, ry);
  ctx.arc(0, 0, 1, 0, Math.PI * 2);
  ctx.fillStyle = color;
  ctx.fill();
  ctx.restore();
}
function drawFloorPad(ctx, posePoints, canvasW, canvasH) {
  const pelvisRaw = posePoints?.ID10;
  if (!pelvisRaw) return;

  const size = 1.0; // 座布団くらい
  const half = size / 2;

  // pose内の最下Yを探す
  let minY = Infinity;
  for (const id of Object.keys(posePoints || {})) {
    const p = posePoints[id];
    if (!p) continue;
    if (typeof p.y === 'number' && p.y < minY) {
      minY = p.y;
    }
  }
  if (!isFinite(minY)) return;

  // 少しだけ下にずらす
  const floorY = minY - 0.10;

  const corners3D = [
    { x: pelvisRaw.x - half, y: floorY, z: pelvisRaw.z - half },
    { x: pelvisRaw.x + half, y: floorY, z: pelvisRaw.z - half },
    { x: pelvisRaw.x + half, y: floorY, z: pelvisRaw.z + half },
    { x: pelvisRaw.x - half, y: floorY, z: pelvisRaw.z + half }
  ];

  const projected = corners3D.map(p => projectPoint(p));

  const { fitScale, centerX, centerY } = getLayoutMetrics(
    posePoints || {},
    canvasW,
    canvasH
  );

  const scale = fitScale * (INTERNAL_CAMERA.scale || 1);
  const tx = INTERNAL_CAMERA.tx || 0;
  const ty = INTERNAL_CAMERA.ty || 0;

  const screenPts = projected.map(p => ({
    x: canvasW / 2 + tx + (p.x - centerX) * scale,
    y: canvasH / 2 + ty - (p.y - centerY) * scale
  }));

  ctx.save();
  ctx.fillStyle = 'rgba(80, 255, 140, 0.18)';
  ctx.strokeStyle = 'rgba(120, 255, 170, 0.45)';
  ctx.lineWidth = 1;

  ctx.beginPath();
  ctx.moveTo(screenPts[0].x, screenPts[0].y);
  ctx.lineTo(screenPts[1].x, screenPts[1].y);
  ctx.lineTo(screenPts[2].x, screenPts[2].y);
  ctx.lineTo(screenPts[3].x, screenPts[3].y);
  ctx.closePath();
  ctx.fill();
  ctx.stroke();

  ctx.restore();
}
function drawSideBreastByDepth(ctx, projected, rawPoints, color, cam, refs, currentScale, view) {
  const chest = projected.ID02;
  const nippleR = projected.ID19;
  const nippleL = projected.ID20;

  if (!chest) return;

const shape = getSideTorsoShape(projected, rawPoints, cam, refs, view);
  if (!shape) return;

  const front = shape.chestFront;
  const back = shape.upperBack;
  if (!front || !back) return;

  const depth = dist2D(front, back);
  if (depth <= 0.01) return;

const bodyView = getBodyViewInfo(cam);
const side = bodyView.sideSign;

  // 背中～前胸の距離の中で、胸の本体はやや前寄り
  const rx = clamp(depth * 0.34, 10, 26);
  const ry = rx * 0.90;

  const center = {
    x: lerp(back.x, front.x, 0.62),
    y: chest.y + ry * 0.95
  };

  drawEllipse(ctx, center, rx, ry, color);
}
function drawBreastBridge(ctx, projected, color, view) {
  const chest = projected?.ID02;
  const nippleR = projected?.ID19;
  const nippleL = projected?.ID20;
  const bustCenter = projected?.ID27;

const pitch = Math.abs(INTERNAL_CAMERA.pitch || 0);
  if (pitch > 1.0) return;


  if (!chest || !nippleR || !nippleL || !bustCenter) return;

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
function drawCrotchFill(ctx, pelvis, hipR, hipL, upperLegWidth, color) {
  if (!pelvis || !hipR || !hipL) return;

  const midHip = midpoint(hipR, hipL);
  if (!midHip) return;

  // 上辺を広げる
  const topY = pelvis.y - upperLegWidth * 0.34;
  const topHalfWidth = upperLegWidth * 0.92;

  const topLeft = {
    x: midHip.x - topHalfWidth,
    y: topY
  };

  const topRight = {
    x: midHip.x + topHalfWidth,
    y: topY
  };

  // 左右の下り始め
  const innerRightTop = {
    x: hipR.x - upperLegWidth * 0.46,
    y: hipR.y + upperLegWidth * 0.02
  };

  const innerLeftTop = {
    x: hipL.x + upperLegWidth * 0.46,
    y: hipL.y + upperLegWidth * 0.02
  };

  // 下は閉じる
  const crotchBottom = {
    x: midHip.x,
    y: Math.max(innerRightTop.y, innerLeftTop.y) + upperLegWidth * 0.18
  };

  // 上辺を少し丸める
  const topCtrl = {
    x: midHip.x,
    y: topY - upperLegWidth * 0.10
  };

  ctx.fillStyle = color;
  ctx.beginPath();

  ctx.moveTo(topLeft.x, topLeft.y);

  // 上辺
  ctx.quadraticCurveTo(
    topCtrl.x,
    topCtrl.y,
    topRight.x,
    topRight.y
  );

  // 右側
  ctx.quadraticCurveTo(
    innerRightTop.x,
    innerRightTop.y,
    crotchBottom.x,
    crotchBottom.y
  );

  // 左側
  ctx.quadraticCurveTo(
    innerLeftTop.x,
    innerLeftTop.y,
    topLeft.x,
    topLeft.y
  );

  ctx.closePath();
  ctx.fill();
}
function drawPelvisDiamond(ctx, pHipR, genital, pHipL, anus, color) {
  if (!pHipR || !genital || !pHipL || !anus) return;

  const hipMid = midpoint(pHipR, pHipL);
  if (!hipMid) return;

  const top = {
    x: hipMid.x,
    y: lerp(hipMid.y, anus.y, 0.42)
  };

  const bottom = {
    x: genital.x,
    y: genital.y + 1
  };

  const right = {
    x: lerp(top.x, pHipR.x, 0.78) - 1.5,
    y: lerp(top.y, pHipR.y, 0.72)
  };

  const left = {
    x: lerp(top.x, pHipL.x, 0.78) + 1.5,
    y: lerp(top.y, pHipL.y, 0.72)
  };

  const rightCtrlTop = {
    x: lerp(top.x, right.x, 0.55),
    y: lerp(top.y, right.y, 0.25)
  };

  const rightCtrlBottom = {
    x: lerp(bottom.x, right.x, 0.62),
    y: lerp(bottom.y, right.y, 0.48)
  };

  const leftCtrlBottom = {
    x: lerp(bottom.x, left.x, 0.62),
    y: lerp(bottom.y, left.y, 0.48)
  };

  const leftCtrlTop = {
    x: lerp(top.x, left.x, 0.55),
    y: lerp(top.y, left.y, 0.25)
  };

  ctx.fillStyle = color;
  ctx.beginPath();

  ctx.moveTo(top.x, top.y);

  ctx.bezierCurveTo(
    rightCtrlTop.x, rightCtrlTop.y,
    rightCtrlBottom.x, rightCtrlBottom.y,
    bottom.x, bottom.y
  );

  ctx.bezierCurveTo(
    leftCtrlBottom.x, leftCtrlBottom.y,
    leftCtrlTop.x, leftCtrlTop.y,
    top.x, top.y
  );

  ctx.closePath();
  ctx.fill();
}
function drawBobHair(ctx, head, r, facing, facingLR = 1) {
  if (!head) return;

  const hairColor = '#8b5a2b';
  ctx.save();
  ctx.fillStyle = hairColor;


// =========================
// 1 FRONT（完成版）
// =========================
if (facing === 1) {



// =========================
// 前髪（内側＋1.2倍＋上へ）
// =========================

// 左
ctx.save();
ctx.translate(head.x - r * 0.48, head.y - r * 0.80); // ← 上げる（重要）
ctx.rotate(0.55);
ctx.beginPath();
ctx.ellipse(0, 0, r * 0.43, r * 0.65, 0, 0, Math.PI * 2); // ← 1.2倍
ctx.fill();
ctx.restore();

// 右
ctx.save();
ctx.translate(head.x + r * 0.48, head.y - r * 0.80); // ← 上げる
ctx.rotate(-0.55);
ctx.beginPath();
ctx.ellipse(0, 0, r * 0.43, r * 0.65, 0, 0, Math.PI * 2); // ← 1.2倍
ctx.fill();
ctx.restore();

  // =========================
  // 左サイド髪
  // =========================
  ctx.beginPath();
  ctx.roundRect(
    head.x - r * 1.02,
    head.y - r * 0.58,
    r * 0.30,
    r * 1.02,
    r * 0.12
  );
  ctx.fill();

  // =========================
  // 右サイド髪
  // =========================
  ctx.beginPath();
  ctx.roundRect(
    head.x + r * 0.72,
    head.y - r * 0.58,
    r * 0.30,
    r * 1.02,
    r * 0.12
  );
  ctx.fill();
}

 // =========================
// 2 FRONT-SIDE（楕円版）
// =========================
// =========================
// 2 FRONT-SIDE（楕円版・透明なし）
// =========================
else if (facing === 2) {
  const side = facingLR >= 0 ? 1 : -1;

  // 奥側
  ctx.save();
  ctx.translate(
    head.x - side * r * 0.42,
    head.y - r * 0.78
  );
  ctx.rotate(0.55 * side);
  ctx.beginPath();
  ctx.ellipse(0, 0, r * 0.40, r * 0.60, 0, 0, Math.PI * 2);
  ctx.fill();
  ctx.restore();

  // 手前側
  ctx.save();
  ctx.translate(
    head.x + side * r * 0.44,
    head.y - r * 0.76
  );
  ctx.rotate(-0.48 * side);
  ctx.beginPath();
  ctx.ellipse(0, 0, r * 0.43, r * 0.64, 0, 0, Math.PI * 2);
  ctx.fill();
  ctx.restore();

  // サイド髪
  ctx.beginPath();
  ctx.roundRect(
    head.x + side * r * 0.72 - (side > 0 ? 0 : r * 0.30),
    head.y - r * 0.58,
    r * 0.30,
    r * 1.02,
    r * 0.12
  );
  ctx.fill();
}
  // =========================
  // 3 SIDE
  // =========================
  // =========================
  // 3 SIDE
  // =========================
  else if (facing === 3) {
    const side = facingLR >= 0 ? 1 : -1;

    // 後頭部の大きい塊
    ctx.beginPath();
    ctx.ellipse(
      head.x + side * r * 0.22,
      head.y + r * 0.02,
      r * 0.98,
      r * 1.08,
      0,
      0,
      Math.PI * 2
    );
    ctx.fill();

    // 顔側を少し開けた上面
    ctx.beginPath();
    ctx.moveTo(head.x - side * r * 0.42, head.y - r * 0.78);
    ctx.quadraticCurveTo(
      head.x + side * r * 0.08,
      head.y - r * 1.12,
      head.x + side * r * 0.88,
      head.y - r * 0.66
    );
    ctx.lineTo(head.x + side * r * 0.92, head.y + r * 0.30);
    ctx.quadraticCurveTo(
      head.x + side * r * 0.20,
      head.y + r * 0.52,
      head.x - side * r * 0.28,
      head.y + r * 0.12
    );
    ctx.closePath();
    ctx.fill();

    // 襟足：向いている側の反対寄りに少し出す
    ctx.beginPath();
    ctx.roundRect(
      head.x + side * r * 0.38 - (side > 0 ? 0 : r * 0.30),
      head.y + r * 0.10,
      r * 0.34,
      r * 0.58,
      r * 0.10
    );
    ctx.fill();
  }
  // =========================
  // 4 BACK SIDE
  // =========================
  else if (facing === 4) {
    const side = facingLR >= 0 ? 1 : -1;

    // 丸寄りの後頭部
ctx.beginPath();
    ctx.ellipse(
      head.x - side * r * 0.06,
      head.y + r * 0.02,
      r * 1.03,
      r * 1.12,
      0,
      0,
      Math.PI * 2
    );
    ctx.fill();

    // 下中央を少しだけえぐって、おかっぱ感
    ctx.save();
    ctx.globalCompositeOperation = 'destination-out';
    ctx.beginPath();
    ctx.moveTo(head.x - r * 0.14, head.y + r * 0.44);
    ctx.quadraticCurveTo(head.x, head.y + r * 0.76, head.x + r * 0.14, head.y + r * 0.44);
    ctx.closePath();
    ctx.fill();
    ctx.restore();

    // 手前側を少しだけ下げて、斜め後ろ感
    ctx.beginPath();
    ctx.roundRect(
      head.x + side * r * 0.62 - (side > 0 ? 0 : r * 0.24),
      head.y - r * 0.34,
      r * 0.22,
      r * 0.82,
      r * 0.10
    );
    ctx.fill();
  }
  // =========================
  // 5 BACK
  // =========================
  else {
    // 真後ろ：四角ではなく丸い後頭部ベース
    ctx.beginPath();
    ctx.ellipse(
      head.x,
      head.y + r * 0.02,
      r * 1.16,
      r * 1.22,
      0,
      0,
      Math.PI * 2
    );
    ctx.fill();

    // 下中央を少しえぐる
    ctx.save();
    ctx.globalCompositeOperation = 'destination-out';
    ctx.beginPath();
    ctx.moveTo(head.x - r * 0.16, head.y + r * 0.46);
    ctx.quadraticCurveTo(head.x, head.y + r * 0.82, head.x + r * 0.16, head.y + r * 0.46);
    ctx.closePath();
    ctx.fill();
    ctx.restore();
  }

  ctx.restore();
}

function drawShoulderPeak(ctx, leftShoulder, neckMid, rightShoulder, width, color) {
  if (!leftShoulder || !neckMid || !rightShoulder) return;

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
function validateLegChain(points) {
  const rHip = points?.ID12;
  const rKnee = points?.ID14;
  const rHeel = points?.ID16;
  const lHip = points?.ID13;
  const lKnee = points?.ID15;
  const lHeel = points?.ID17;

  const warnings = [];

  if (rHip && rKnee && rHeel) {
    if (rKnee.y > rHip.y + 0.05) warnings.push('Right knee is above right hip');
    if (rHeel.y > rKnee.y + 0.05) warnings.push('Right heel is above right knee');
  }

  if (lHip && lKnee && lHeel) {
    if (lKnee.y > lHip.y + 0.05) warnings.push('Left knee is above left hip');
    if (lHeel.y > lKnee.y + 0.05) warnings.push('Left heel is above left knee');
  }

  return warnings;
}
function drawRotatedEllipse(ctx, center, rx, ry, angle, color) {
  if (!center) return;

  ctx.save();
  ctx.translate(center.x, center.y);
  ctx.rotate(angle);

  ctx.beginPath();
  ctx.scale(rx, ry);
  ctx.arc(0, 0, 1, 0, Math.PI * 2);
  ctx.fillStyle = color;
  ctx.fill();

  ctx.restore();
}
function drawCenterLine(ctx, rHip, lHip, genital, anus, color) {
  if (!genital || !anus) return;

  const centerX = (genital.x + anus.x) * 0.5;
  const centerY = (genital.y + anus.y) * 0.5;

  // 横幅
  const baseWidth = Math.max(12, Math.abs(anus.y - genital.y) * 1.0);

  // 下にふくらむ量
  const bulge = Math.max(10, baseWidth * 1.3);

  const topRight = {
    x: genital.x + baseWidth * 0.42,
    y: genital.y
  };

  const topLeft = {
    x: genital.x - baseWidth * 0.42,
    y: genital.y
  };

  const bottom = {
    x: anus.x,
    y: anus.y
  };

  const rightCtrl = {
    x: centerX + baseWidth * 0.42,
    y: Math.max(genital.y, anus.y) + bulge
  };

  const leftCtrl = {
    x: centerX - baseWidth * 0.42,
    y: Math.max(genital.y, anus.y) + bulge
  };

  ctx.fillStyle = color;
  ctx.beginPath();

  ctx.moveTo(topLeft.x, topLeft.y);
  ctx.lineTo(topRight.x, topRight.y);

  ctx.quadraticCurveTo(
    rightCtrl.x,
    rightCtrl.y,
    bottom.x,
    bottom.y
  );

  ctx.quadraticCurveTo(
    leftCtrl.x,
    leftCtrl.y,
    topLeft.x,
    topLeft.y
  );

  ctx.closePath();
  ctx.fill();
}
function clampPitch(v) {
  const LIMIT = Math.PI * 0.495; // 約89.1度
  return clamp(v, -LIMIT, LIMIT);
}
function getAverageForwardDepth(rawPoints, ids, forward) {
  let sum = 0;
  let count = 0;

  for (const id of ids) {
    const p = rawPoints?.[id];
    if (!p) continue;
    sum += getDepthByForward(p, forward);
    count++;
  }

  return count ? (sum / count) : 0;
}

function getDrawOrderFromDepth(rawPoints) {
  const forward = getBodyForwardForDepth();

  const headFront  = rawPoints?.ID18 || rawPoints?.ID04 || rawPoints?.ID03;
  const headCore   = rawPoints?.ID04 || rawPoints?.ID03;
  const neckCore   = rawPoints?.ID03 || rawPoints?.ID02;
  const chestCore  = rawPoints?.ID02 || rawPoints?.ID01;
  const pelvisCore = rawPoints?.ID10 || rawPoints?.ID01;

  const headDepth =
    (getDepthByForward(headFront, forward) * 0.55) +
    (getDepthByForward(headCore, forward)  * 0.30) +
    (getDepthByForward(neckCore, forward)  * 0.15);

  const torsoDepth =
    (getDepthByForward(chestCore, forward)  * 0.62) +
    (getDepthByForward(pelvisCore, forward) * 0.38);

  const armsDepth = getAverageForwardDepth(
    rawPoints,
    ['ID05','ID06','ID07','ID08','ID09','ID11'],
    forward
  );

  const legsDepth = getAverageForwardDepth(
    rawPoints,
    ['ID10','ID12','ID13','ID14','ID15','ID16','ID17'],
    forward
  );

  const items = [
    { name: 'legs',  z: legsDepth },
    { name: 'torso', z: torsoDepth },
    { name: 'arms',  z: armsDepth },
    { name: 'head',  z: headDepth }
  ];

  const tieBias = {
    legs: 0.00,
    torso: 0.01,
    arms: 0.02,
    head: 0.03
  };

  items.sort((a, b) => (a.z + tieBias[a.name]) - (b.z + tieBias[b.name]));

  return {
    order: items.map(v => v.name),
    drawHeadLast: false
  };
}
function drawButtMass(ctx, pelvis, rHip, lHip, genital, anus, view) {
  if (!pelvis || !anus || !rHip || !lHip) return;

  const sideStrength = clamp(view?.sideStrength || 0, 0, 1);
const bodyView = getBodyViewInfo(INTERNAL_CAMERA);
const frontBackSign = bodyView.frontBackSign;

  const rawRHip = pose?.points?.ID12;
  const rawLHip = pose?.points?.ID13;
  const hipSpan3D = dist3D(rawRHip, rawLHip);
const hipSpan = Math.max(14, hipSpan3D * (fixedFitScale || 1) * (INTERNAL_CAMERA.scale || 1));

  // 尻の中心は pelvis→anus の中間寄り
  const buttBase = mixPoint(pelvis, anus, 0.58);
  if (!buttBase) return;

  // 左右配置は hip ライン基準に固定する
  const hipAxisRaw = {
    x: rHip.x - lHip.x,
    y: rHip.y - lHip.y
  };
  const hipAxisLen = Math.hypot(hipAxisRaw.x, hipAxisRaw.y) || 1;

  const sideAxis = {
    x: hipAxisRaw.x / hipAxisLen,
    y: hipAxisRaw.y / hipAxisLen
  };

  // サイズ
  const radius = clamp(hipSpan * 0.44, 10, 120);
  const gap = radius * 0.56;

  // 横向きほど少しだけ後ろへ逃がす
  const backDir = (frontBackSign === 0 ? 1 : -frontBackSign);
  const depthShift = radius * 0.34 * sideStrength;

  const centerR = {
    x: buttBase.x + sideAxis.x * gap + backDir * depthShift,
    y: buttBase.y + sideAxis.y * gap + radius * 0.12
  };

  const centerL = {
    x: buttBase.x - sideAxis.x * gap + backDir * depthShift,
    y: buttBase.y - sideAxis.y * gap + radius * 0.12
  };

  // 横向きでは横幅だけ少し縮める
  const rx = radius * lerp(0.96, 0.68, sideStrength);
  const ry = radius * lerp(1.08, 0.96, sideStrength);

  const buttColor = '#ff9f8a';

  drawEllipse(ctx, centerR, rx, ry, buttColor);
  drawEllipse(ctx, centerL, rx, ry, buttColor);

  // 割れ目
  const cleftTop = mixPoint(pelvis, anus, 0.26);
  const cleftBottom = mixPoint(pelvis, anus, 0.96);

  if (cleftTop && cleftBottom) {
    ctx.strokeStyle = 'rgba(255,235,170,0.95)';
    ctx.lineWidth = Math.max(1.2, Math.min(10, radius * 0.10));
    ctx.lineCap = 'round';
    ctx.beginPath();
    ctx.moveTo(cleftTop.x + backDir * depthShift * 0.35, cleftTop.y);
    ctx.quadraticCurveTo(
      buttBase.x + backDir * depthShift * 0.58,
      buttBase.y + radius * 0.18,
      cleftBottom.x + backDir * depthShift * 0.20,
      cleftBottom.y
    );
    ctx.stroke();
  }
}

function drawPointDots(ctx, projected, rawPoints) {
  const ids = Object.keys(projected || {});
  if (!ids.length) return;

  const forward = getBodyForwardForDepth();

  ids.sort((a, b) => {
    const pa = rawPoints?.[a];
    const pb = rawPoints?.[b];
    if (!pa || !pb) return 0;

    const da = getDepthByForward(pa, forward);
    const db = getDepthByForward(pb, forward);

    return da - db;
  });

//  console.log(
//    '[POINT Z ORDER]',
//    ids.map(id => {
//      const p = rawPoints?.[id];
//      const z = p ? rotatePoint(p, viewCam).z : null;
//      return [id, z];
//    })
//  );

  for (const id of ids) {
    const pt = projected[id];
    if (!pt) continue;

    let r = 4.6;
    let color = 'rgba(140,220,255,0.95)'; // それ以外：水色

    // 薄い赤：秘部、乳首、口
    if (
      id === 'ID18' || // Mouth
      id === 'ID19' || // Right Nipple
      id === 'ID20' || // Left Nipple
      id === 'ID21'    // Genital
    ) {
      r = 5.0;
      color = 'rgba(255,170,185,0.95)';
    }

    // 薄い黄色：尻穴
    if (id === 'ID22') {
      r = 4.8;
      color = 'rgba(255,235,150,0.95)';
    }

    ctx.beginPath();
    ctx.arc(pt.x, pt.y, r, 0, Math.PI * 2);
    ctx.fillStyle = color;
    ctx.fill();
  }
}

function updateRoot(rawPose, prevState, dt) {
  const pts = rawPose.points || {};

  // --------------------------------------------------
  // 1) 必須点
  // --------------------------------------------------
  const pelvis = pts.ID10;
  const chest  = pts.ID02;
  const neck   = pts.ID03;
  const rHip   = pts.ID12;
  const lHip   = pts.ID13;
  const rHeel  = pts.ID16;
  const lHeel  = pts.ID17;
  const skull  = pts.ID04;
  const mouth  = pts.ID18;

  // 必須点不足時は前回維持
  if (!pelvis || !chest || !rHip || !lHip) {
    return buildFallbackOutput(rawPose, prevState);
  }

  // --------------------------------------------------
  // 2) body basis
  // --------------------------------------------------
let bodyRight = normalize(sub(rHip, lHip));
let bodyUp    = normalize(sub(chest, pelvis));
let bodyForward = normalize(cross(bodyUp, bodyRight));

const genital = pts.ID21;
const anus = pts.ID22;

if (genital && anus) {
  const frontHint = normalize(sub(genital, anus));
  if (dot(bodyForward, frontHint) < 0) {
    bodyForward = {
      x: -bodyForward.x,
      y: -bodyForward.y,
      z: -bodyForward.z
    };
  }
}

  if (!isFiniteVec(bodyForward) || length(bodyForward) < 0.0001) {
    bodyForward = prevState.bodyForward;
  }

  // 直交補正
  bodyRight   = normalize(cross(bodyForward, bodyUp));
  bodyUp      = normalize(cross(bodyRight, bodyForward));
  bodyForward = normalize(bodyForward);

  // smoothing
  bodyForward = normalize(lerpVec(prevState.bodyForward, bodyForward, 0.25));
  bodyUp      = normalize(lerpVec(prevState.bodyUp,      bodyUp,      0.25));
  bodyRight   = normalize(cross(bodyForward, bodyUp));
  bodyUp      = normalize(cross(bodyRight, bodyForward));

  // --------------------------------------------------
  // 3) head basis
  // --------------------------------------------------
  let headForward = bodyForward;
  let headUp = bodyUp;
  let headRight = bodyRight;

  if (neck && skull && mouth) {
    let rawHeadUp = normalize(sub(skull, neck));
    let faceDir   = normalize(sub(mouth, neck));

    // headRight を faceDir と up から作る
    let rawHeadRight = normalize(cross(faceDir, rawHeadUp));
    let rawHeadForward = normalize(cross(rawHeadUp, rawHeadRight));

    if (isFiniteVec(rawHeadForward) && length(rawHeadForward) > 0.0001) {
      headForward = rawHeadForward;
      headUp = rawHeadUp;
      headRight = normalize(cross(headForward, headUp));
      headUp = normalize(cross(headRight, headForward));
    }
  }

  // bodyからの乖離を制限
  headForward = clampHeadYawPitch(bodyForward, bodyUp, headForward);

  // smoothing
  headForward = normalize(lerpVec(prevState.headForward, headForward, 0.35));
  headUp      = normalize(lerpVec(prevState.headUp, headUp, 0.35));
  headRight   = normalize(cross(headForward, headUp));
  headUp      = normalize(cross(headRight, headForward));

// --------------------------------------------------
// 4) scene.position を先に仮更新
// --------------------------------------------------
let scenePosition = { ...prevState.scenePosition };
let sceneVelocity = { ...prevState.sceneVelocity };

// 水平移動は root側ロジック次第
scenePosition.x += sceneVelocity.x * dt;
scenePosition.z += sceneVelocity.z * dt;

// 重力でいったん仮更新
const groundY = 0.0;
const gravity = -9.8;
sceneVelocity.y += gravity * dt;
scenePosition.y += sceneVelocity.y * dt;


// --------------------------------------------------
// 5) 足接地判定（world座標）
// --------------------------------------------------
const footEps = 0.04;
const velEps  = 0.12;
const footOffEps = 0.07;
const velOffEps  = 0.20;

const prevRHeelY = prevState._prevRHeelY ?? rHeel?.y ?? groundY;
const prevLHeelY = prevState._prevLHeelY ?? lHeel?.y ?? groundY;

const rHeelVy = rHeel ? (rHeel.y - prevRHeelY) / Math.max(dt, 1e-6) : 999;
const lHeelVy = lHeel ? (lHeel.y - prevLHeelY) / Math.max(dt, 1e-6) : 999;

const rWorldY = (rHeel?.y ?? Infinity) + scenePosition.y;
const lWorldY = (lHeel?.y ?? Infinity) + scenePosition.y;

const rightFootContact =
  prevState.rightFootContact
    ? (
        !!rHeel &&
        Math.abs(rWorldY - groundY) <= footOffEps &&
        Math.abs(rHeelVy) <= velOffEps
      )
    : (
        !!rHeel &&
        Math.abs(rWorldY - groundY) <= footEps &&
        Math.abs(rHeelVy) <= velEps
      );

const leftFootContact =
  prevState.leftFootContact
    ? (
        !!lHeel &&
        Math.abs(lWorldY - groundY) <= footOffEps &&
        Math.abs(lHeelVy) <= velOffEps
      )
    : (
        !!lHeel &&
        Math.abs(lWorldY - groundY) <= footEps &&
        Math.abs(lHeelVy) <= velEps
      );

const isGrounded = rightFootContact || leftFootContact;
// --------------------------------------------------
// 5.5) jump / airborne state
// --------------------------------------------------
let groundedFrames = prevState.groundedFrames || 0;
let airborneFrames = prevState.airborneFrames || 0;

if (isGrounded) {
  groundedFrames += 1;
  airborneFrames = 0;
} else {
  groundedFrames = 0;
  airborneFrames += 1;
}

let isJumping = prevState.isJumping;

if (!isGrounded && airborneFrames >= 2) {
  isJumping = true;
}
if (isGrounded && groundedFrames >= 2) {
  isJumping = false;
}

// --------------------------------------------------
// 6) groundedなら床へ吸着補正
// --------------------------------------------------
if (isGrounded) {
  sceneVelocity.y = 0;

  const footY = Math.min(
    rHeel?.y ?? Infinity,
    lHeel?.y ?? Infinity
  );

  if (isFinite(footY)) {
    scenePosition.y = groundY - footY;
  }
}

  // --------------------------------------------------
  // 7) JSON組み立て
  // --------------------------------------------------
  const out = {
    frame: (rawPose.frame ?? 0),

    root: "ID10",

    scene: {
      position: roundVec(scenePosition),
      velocity: roundVec(sceneVelocity),
      groundY,
      isGrounded,
      isJumping
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
    headRight,

    rightFootContact,
    leftFootContact,
    isGrounded,
    isJumping,

    groundedFrames,
    airborneFrames,

    _prevRHeelY: rHeel?.y ?? prevRHeelY,
    _prevLHeelY: lHeel?.y ?? prevLHeelY
  };

  return { out, nextState };
}
function sub(a, b) {
  return {
    x: a.x - b.x,
    y: a.y - b.y,
    z: a.z - b.z
  };
}

function dot(a, b) {
  return a.x * b.x + a.y * b.y + a.z * b.z;
}

function cross(a, b) {
  return {
    x: a.y * b.z - a.z * b.y,
    y: a.z * b.x - a.x * b.z,
    z: a.x * b.y - a.y * b.x
  };
}

function length(v) {
  return Math.hypot(v.x, v.y, v.z);
}

function normalize(v) {
  const len = length(v);
  if (!isFinite(len) || len < 1e-8) {
    return { x: 0, y: 0, z: 0 };
  }
  return {
    x: v.x / len,
    y: v.y / len,
    z: v.z / len
  };
}
function clampHeadYawPitch(bodyForward, bodyUp, headForward) {
  // body基準の右
  const bodyRight = normalize(cross(bodyForward, bodyUp));

  // body平面へ分解
  const f =
    dot(headForward, bodyForward);
  const r =
    dot(headForward, bodyRight);
  const u =
    dot(headForward, bodyUp);

  // yaw/pitch相当を制限
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
function lerp(a, b, t) {
  return a + (b - a) * t;
}

function lerpVec(a, b, t) {
  return {
    x: lerp(a.x, b.x, t),
    y: lerp(a.y, b.y, t),
    z: lerp(a.z, b.z, t)
  };
}

function isFiniteVec(v) {
  return Number.isFinite(v.x) && Number.isFinite(v.y) && Number.isFinite(v.z);
}

function roundVec(v) {
  return {
    x: +v.x.toFixed(4),
    y: +v.y.toFixed(4),
    z: +v.z.toFixed(4)
  };
}
function getHeadFacingFromMetaOrPose(points, meta, cam) {
  const metaForward = meta?.head?.forward;

  if (
    metaForward &&
    Number.isFinite(metaForward.x) &&
    Number.isFinite(metaForward.y) &&
    Number.isFinite(metaForward.z)
  ) {
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
function getDepthByForward(p, forward) {
  if (!p || !forward) return 0;
  return p.x * forward.x + p.y * forward.y + p.z * forward.z;
}
function getBodyViewInfo(cam) {
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
function getBodyForwardForDepth() {
  const f = poseSceneMeta?.body?.forward;
  if (
    f &&
    Number.isFinite(f.x) &&
    Number.isFinite(f.y) &&
    Number.isFinite(f.z)
  ) {
    return vecNormalize(f);
  }

  const basis = buildRootBasis(getPosePointsWithSceneOffset(pose.points || {}));
  return basis?.forward || { x: 0, y: 0, z: 1 };
}
function drawPointLabels(ctx, projected, rawPoints) {
  if (!projected) return;

  ctx.save();
  ctx.font = '10px monospace';
  ctx.fillStyle = '#00ff88';
  ctx.textAlign = 'left';
  ctx.textBaseline = 'middle';

  for (const id in projected) {
    if (id.startsWith('__')) continue;

    const p = projected[id];
    if (!p) continue;

    // 点
    ctx.fillStyle = '#ffffff';
    ctx.fillRect(p.x - 1, p.y - 1, 2, 2);

    // ラベル
    ctx.fillStyle = '#00ff88';
const name = rawPoints?.[id]?.name || '';
ctx.fillText(id + ' ' + name, p.x + 3, p.y);
  }

  ctx.restore();
}


function buildFallbackOutput(rawPose, prevState) {
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

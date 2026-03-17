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

  let pose = {
    frame: 1,
    root: 'ID10',
    camera: { yaw: 0, pitch: 0, roll: 0, scale: 1, tx: 0, ty: 0 },
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

  function ensureCameraDefaults() {
    if (!pose.camera) pose.camera = {};
    if (typeof pose.camera.yaw !== 'number') pose.camera.yaw = 0;
    if (typeof pose.camera.pitch !== 'number') pose.camera.pitch = 0;
    if (typeof pose.camera.roll !== 'number') pose.camera.roll = 0;
    if (typeof pose.camera.scale !== 'number') pose.camera.scale = 1;
    if (typeof pose.camera.tx !== 'number') pose.camera.tx = 0;
    if (typeof pose.camera.ty !== 'number') pose.camera.ty = 0;
  }
    function degToRad(v) {
  return v * Math.PI / 180;
}

function normalizeIncomingCamera(camera) {
  const next = { ...(camera || {}) };

  // 角度が明らかに大きい場合は度数法とみなす
  if (typeof next.yaw === 'number' && Math.abs(next.yaw) > Math.PI * 2) {
    next.yaw = degToRad(next.yaw);
  }
  if (typeof next.pitch === 'number' && Math.abs(next.pitch) > Math.PI * 2) {
    next.pitch = degToRad(next.pitch);
  }
  if (typeof next.roll === 'number' && Math.abs(next.roll) > Math.PI * 2) {
    next.roll = degToRad(next.roll);
  }

  return next;
}


function applyForcedViewIfNeeded() {
  ensureCameraDefaults();

  if (!forceFrontView) return;

  pose.camera.yaw = 0;
  pose.camera.pitch = 0;
  pose.camera.roll = 0;
}

 function resetCamera(withDraw = true) {
    pose.camera.yaw = 0;
    pose.camera.pitch = 0;
    pose.camera.roll = 0;
    pose.camera.scale = 1;
    pose.camera.tx = 0;
    pose.camera.ty = 0;

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
  function projectPoint(p) {
    const r = rotatePoint(p, pose.camera || {});
    return { x: r.x, y: r.y };
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

    const fitScale = Math.min(
      (width - padding * 2) / spanX,
      (height - padding * 2) / spanY
    );

    const scale = fitScale * (pose.camera.scale || 1);
    const centerX = (minX + maxX) / 2;
    const centerY = (minY + maxY) / 2;
    const tx = pose.camera.tx || 0;
    const ty = pose.camera.ty || 0;

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

    const fitScale = Math.min(
      (width - padding * 2) / spanX,
      (height - padding * 2) / spanY
    );

    return {
      fitScale,
      centerX: (minX + maxX) / 2,
      centerY: (minY + maxY) / 2
    };
  }
function getFacingClassFromCamera(cam) {
  let a = cam?.yaw || 0;

  while (a > Math.PI) a -= Math.PI * 2;
  while (a < -Math.PI) a += Math.PI * 2;

  const deg = Math.abs(a) * 180 / Math.PI;

  if (deg < 30) return 1;    // FRONT
  if (deg < 70) return 2;    // FRONT-SIDE
  if (deg < 110) return 3;   // SIDE
  if (deg < 150) return 4;   // BACK-SIDE
  return 5;                  // BACK
}
function drawPose() {

  const panel = getOrCreatePanel();
  const canvas = panel.querySelector('#' + CANVAS_ID);
  const ctx = canvas.getContext('2d');

const torsoColor = '#cfcfcf';
  const bodyColor = '#e6e6e6';
  const limbColor = '#d9d9d9';
const shadowColor = 'rgba(255,255,255,0.14)';
const pelvisLineColor = 'rgba(255,179,199,0.18)';// ← 薄い紫
const headColor = '#f6d6cc'; // ← 手より少し白い肌色

const handColor = '#ffd6de';   // 薄いピンク
const footColor = '#f2b8a8';   // 薄い紫

  ctx.clearRect(0, 0, canvas.width, canvas.height);
  ctx.fillStyle = '#0b0b0b';
  ctx.fillRect(0, 0, canvas.width, canvas.height);

  const projected = computeLayout(pose.points || {}, canvas.width, canvas.height);
  const P = projected;

  const head = P.ID04;
  const neck = P.ID03;
  const chest = P.ID02;
  const waist = P.ID01;
  const pelvis = P.ID10;

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

const hipCenter = midpoint(rHip, lHip) || pelvis;

const { fitScale } = getLayoutMetrics(
  pose.points || {},
  canvas.width,
  canvas.height
);
const currentScale = fitScale * (pose.camera.scale || 1);

const shoulderWidth = dist3D(pose.points.ID05, pose.points.ID06) * currentScale;
const rawShoulderWidth = dist3D(pose.points.ID05, pose.points.ID06);

const headRadius = Math.max(18, Math.min(80, rawShoulderWidth * currentScale * 0.36 || 22));
const armWidth = Math.max(9, Math.min(28, shoulderWidth * 0.26 || 12));
const legWidth = Math.max(8, Math.min(30, shoulderWidth * 0.24 || 11));

const thighWidth = legWidth * 1.25;

const pitch = pose.camera?.pitch || 0;

const view = getViewMetrics(pose.points, pose.camera || {}, currentScale);
const sideStrength = view.sideStrength;
const frontBackSign = view.frontBackSign;
// 脚（極太FIX）
const limbSideStrength = smoothstep01((sideStrength - 0.45) / 0.45);

const thighWidthDraw = thighWidth * lerp(1.0, 1.75, limbSideStrength);
const legWidthDraw = legWidth * lerp(1.0, 1.55, limbSideStrength);
const armWidthDraw = armWidth * lerp(1.0, 1.45, limbSideStrength);

const pitchForward = smoothstep01((pitch - 0.18) / 0.75);

const shoulderLift = Math.max(6, headRadius * 0.22);
const shoulderSlope = Math.max(2, shoulderWidth * 0.10);

// 前かがみ時は肩を少し内側・下側へ
const shoulderInward = shoulderWidth * 0.05 * pitchForward;
const shoulderDropForward = headRadius * 0.04 * pitchForward;

const rShoulderDraw = rShoulder
  ? {
      x: rShoulder.x - shoulderInward,
      y: rShoulder.y - shoulderLift + shoulderSlope + shoulderDropForward
    }
  : null;

const lShoulderDraw = lShoulder
  ? {
      x: lShoulder.x + shoulderInward,
      y: lShoulder.y - shoulderLift + shoulderSlope + shoulderDropForward
    }
  : null;

const shoulderMid = midpoint(rShoulderDraw, lShoulderDraw);
const shoulderNeckMid = mixPoint(shoulderMid, neck, 0.33);

const shoulderCenter = midpoint(rShoulderDraw, lShoulderDraw) || chest || neck;
const facing = getFacingClassFromCamera(pose.camera || {});

function drawHeadBlock() {
  const HEAD_ANCHOR_MODE = 'TOP';

  const headCenterOffsetY =
    (HEAD_ANCHOR_MODE === 'TOP')
      ? headRadius
      : headRadius * 0.10;

  // 前かがみ時は頭を少し下げる
const headForwardDrop = headRadius * 0.20 * pitchForward;

  const frontness = 1 - sideStrength;
  const headBackLift = headRadius * 0.10 * frontness;
  const headBackShiftX = (view?.frontBackSign || 1) * headRadius * 0.06 * sideStrength;

  const headDraw = head
    ? {
        x: head.x - headBackShiftX,
        y: head.y + headCenterOffsetY + headForwardDrop - headBackLift
      }
    : null;

  const yaw = pose.camera?.yaw || 0;
  const facingLR = yaw >= 0 ? -1 : 1;

  const hairScale =
    facing === 1 ? 1.12 :
    facing === 2 ? 1.12 :
    facing === 3 ? 1.10 :
    facing === 4 ? 1.14 :
    1.16;

  if (facing <= 3) {
    drawCircle(ctx, headDraw, headRadius, headColor);
    drawBobHair(ctx, headDraw, headRadius * hairScale, facing, facingLR);
  } else {
    drawCircle(ctx, headDraw, headRadius, bodyColor);
    drawBobHair(ctx, headDraw, headRadius * hairScale, facing, facingLR);
  }

  // 前かがみ時は首元を少し詰める
  const neckForwardTuck = headRadius * 0.10 * pitchForward;
  const shoulderCenterAdjusted = shoulderCenter
    ? { x: shoulderCenter.x, y: shoulderCenter.y - neckForwardTuck }
    : shoulderCenter;

  if (headDraw && shoulderCenterAdjusted) {
    drawNeck(ctx, headDraw, shoulderCenterAdjusted, headRadius, bodyColor);
  }
}
function drawTorsoBlock() {
  drawSmartTorso(ctx, P, pose.points, torsoColor, pose.camera || {}, {
    chest, pelvis,
    rShoulder: rShoulderDraw,
    lShoulder: lShoulderDraw,
    rHip, lHip
  }, view);

  drawBreasts(
    ctx,
    P,
    pose.points,
    bodyColor,
    breastRefs,
    pose.camera || {},
    currentScale,
    view
  );
  drawBreastBridge(ctx, P, bodyColor, view);

  drawShoulderPeak(
    ctx,
    lShoulderDraw,
    shoulderNeckMid,
    rShoulderDraw,
    armWidth * 0.72,
    bodyColor
  );
}

function drawLegBlock() {
  const hipInset = legWidth * 0.14;
  const hipRadius = thighWidth * lerp(1.15, 1.35, sideStrength);

  const hipBackShiftBase = hipRadius * 0.35;
  const hipBackShift = hipBackShiftBase * sideStrength * frontBackSign;

  drawCircle(
    ctx,
    { x: rHip.x - hipInset - hipBackShift, y: rHip.y },
    hipRadius,
    bodyColor
  );

  drawCircle(
    ctx,
    { x: lHip.x + hipInset - hipBackShift, y: lHip.y },
    hipRadius,
    bodyColor
  );

drawCrotchFill(ctx, pelvis, rHip, lHip, legWidth, bodyColor);
drawPelvisDiamond(ctx, rHip, genital, lHip, anus, pelvisLineColor);

drawCapsule(ctx, rHip, rKneeDraw, thighWidthDraw, footColor);
drawCapsule(ctx, rKneeDraw, rHeelDraw, legWidthDraw * 0.95, footColor);
drawCapsule(ctx, lHip, lKneeDraw, thighWidthDraw, footColor);
drawCapsule(ctx, lKneeDraw, lHeelDraw, legWidthDraw * 0.95, footColor);

  drawCircle(ctx, rHeelDraw, legWidthDraw * 0.58, footColor);
  drawCircle(ctx, lHeelDraw, legWidthDraw * 0.58, footColor);
}


    const breastRefs = {
  chest, pelvis,
  rShoulder: rShoulderDraw,
  lShoulder: lShoulderDraw,
  rHip, lHip
};
//console.log('drawPose reached');
//console.log({ chest, pelvis, rShoulderDraw, lShoulderDraw, rHip, lHip });

const rElbowDraw = spreadSidePoint(rElbow,  1, armWidth * 1.35, view);
const lElbowDraw = spreadSidePoint(lElbow, -1, armWidth * 1.35, view);
const rWristDraw = spreadSidePoint(rWrist,  1, armWidth * 1.80, view);
const lWristDraw = spreadSidePoint(lWrist, -1, armWidth * 1.80, view);

const rKneeDraw = spreadSidePoint(rKnee,  1, legWidth * 1.20, view);
const lKneeDraw = spreadSidePoint(lKnee, -1, legWidth * 1.20, view);
const rHeelDraw = spreadSidePoint(rHeel,  1, legWidth * 1.60, view);
const lHeelDraw = spreadSidePoint(lHeel, -1, legWidth * 1.60, view);






    // 影を少しだけ
ctx.save();
ctx.translate(4, 4);

drawCapsule(ctx, rHip, rKneeDraw, legWidthDraw * 1.12, shadowColor);
drawCapsule(ctx, rKneeDraw, rHeelDraw, legWidthDraw * 1.05, shadowColor);
drawCapsule(ctx, lHip, lKneeDraw, legWidthDraw * 1.12, shadowColor);
drawCapsule(ctx, lKneeDraw, lHeelDraw, legWidthDraw * 1.05, shadowColor);

drawRoundLimb(ctx, rShoulderDraw, rElbowDraw, armWidthDraw * 1.12, shadowColor);
drawRoundLimb(ctx, lShoulderDraw, lElbowDraw, armWidthDraw * 1.12, shadowColor);

drawCircle(ctx, rShoulderDraw, armWidth * 0.62, shadowColor);
drawCircle(ctx, lShoulderDraw, armWidth * 0.62, shadowColor);

// 胴
drawSmartTorso(ctx, P, pose.points, shadowColor, pose.camera || {}, {
  chest, pelvis,
  rShoulder: rShoulderDraw,
  lShoulder: lShoulderDraw,
  rHip, lHip
}, view);

// 胸
drawBreasts(
  ctx,
  P,
  pose.points,
  shadowColor,
  breastRefs,
  pose.camera || {},
  currentScale,
  view
);
drawBreastBridge(ctx, P, shadowColor, view);

const shadowHeadDraw = head ? { x: head.x, y: head.y + headRadius } : null;
drawCircle(ctx, shadowHeadDraw, headRadius * 1.08, shadowColor);

ctx.restore();

function avgRotatedZ(ids) {
  const vals = ids
    .map(id => pose.points?.[id])
    .filter(Boolean)
    .map(p => rotatePoint(p, pose.camera || {}).z);

  if (!vals.length) return 0;
  return vals.reduce((a, b) => a + b, 0) / vals.length;
}

function armDepthMetric(ids) {
  const vals = ids
    .map(id => pose.points?.[id])
    .filter(Boolean)
    .map(p => rotatePoint(p, pose.camera || {}).z);

  if (!vals.length) return { avg: 0, max: 0 };

  return {
    avg: vals.reduce((a, b) => a + b, 0) / vals.length,
    max: Math.max(...vals)
  };
}

function drawRightArmBlock() {
  drawRoundLimb(ctx, rShoulderDraw, rElbowDraw, armWidthDraw, handColor);
  drawRoundLimb(ctx, rElbowDraw, rWristDraw, armWidthDraw * 0.92, handColor);
  drawCircle(ctx, rShoulderDraw, armWidthDraw * 0.52, bodyColor);
  drawCircle(ctx, rWristDraw, armWidthDraw * 0.58, handColor);
}

function drawLeftArmBlock() {
  drawRoundLimb(ctx, lShoulderDraw, lElbowDraw, armWidthDraw, handColor);
  drawRoundLimb(ctx, lElbowDraw, lWristDraw, armWidthDraw * 0.92, handColor);
  drawCircle(ctx, lShoulderDraw, armWidthDraw * 0.52, bodyColor);
  drawCircle(ctx, lWristDraw, armWidthDraw * 0.58, handColor);
}

function drawBodyWithDepth() {
  const torsoZ = avgRotatedZ(['ID02', 'ID03', 'ID10', 'ID27']);
  const headZ = avgRotatedZ(['ID03', 'ID04', 'ID18']);

  const rightArm = armDepthMetric(['ID05', 'ID07', 'ID09']);
  const leftArm  = armDepthMetric(['ID06', 'ID08', 'ID11']);

  const backParts = [];
  const midParts = [];
  const frontParts = [];

function pushArm(depth, drawFn) {
  const armFrontThreshold = torsoZ + 0.04; // 少し下げる

  if (depth.avg < torsoZ - 0.015) {
    backParts.push({ z: depth.avg, draw: drawFn });
  } else if (depth.avg <= armFrontThreshold) {
    midParts.push({ z: depth.avg, draw: drawFn });
  } else {
    frontParts.push({ z: depth.avg, draw: drawFn });
  }
}

  pushArm(rightArm, drawRightArmBlock);
  pushArm(leftArm, drawLeftArmBlock);

  backParts.sort((a, b) => a.z - b.z);
  midParts.sort((a, b) => a.z - b.z);
  frontParts.sort((a, b) => a.z - b.z);

const headFrontBias = (1 - sideStrength) * 0.035;
const headBehindTorso = (headZ <= torsoZ + headFrontBias);

  if (headBehindTorso) {
    drawHeadBlock();
  }

  for (const p of backParts) p.draw();

  drawTorsoBlock();
  if (!headBehindTorso) {
    drawHeadBlock();
  }

  for (const p of midParts) p.draw();
  for (const p of frontParts) p.draw();
}

if (pitch < -0.55) {
  drawBodyWithDepth();
  drawLegBlock();
} else if (pitch > 0.55) {
  drawLegBlock();
  drawBodyWithDepth();
} else {
  drawLegBlock();
  drawBodyWithDepth();
}



    const SURFACE_IDS = new Set([
    'ID18', 'ID19', 'ID20', 'ID21', 'ID22',
    'ID23', 'ID24', 'ID25', 'ID26', 'ID27'
  ]);

  const pointEntries = Object.entries(pose.points || {})
    .map(([id, src]) => {
      const screen = projected[id];
      const rotated = rotatePoint(src, pose.camera || {});
      return { id, screen, z: rotated.z };
    })
    .filter(item => item.screen)
    .sort((a, b) => a.z - b.z);

  const surfacePoints = pointEntries.filter(item => SURFACE_IDS.has(item.id));

  // 補助点だけ残す
  for (const item of surfacePoints) {
  if (facing === 5 && item.id === 'ID18') continue;
      const { id, screen } = item;

      let dotX = screen.x;
let dotY = screen.y;

if (id === 'ID19' || id === 'ID20') {
  dotY += 14; // ← 赤い点だけ下へ
}

ctx.beginPath();

    if (id === 'ID18' || id === 'ID19' || id === 'ID20' || id === 'ID21') {
      ctx.fillStyle = '#ff4d4f';
    } else if (id === 'ID22') {
      ctx.fillStyle = '#ffd84d';
    } else if (id === 'ID23' || id === 'ID24' || id === 'ID25' || id === 'ID26' || id === 'ID27') {
      ctx.fillStyle = '#9aa0a6';
    } else {
      ctx.fillStyle = '#4da3ff';
    }

    ctx.beginPath();
ctx.arc(dotX, dotY, 3.0, 0, Math.PI * 2);
    ctx.fill();
  }

  ctx.fillStyle = '#aaa';
ctx.font = '10px sans-serif';

const debugPitch = Math.abs(pose?.camera?.pitch || 0);
const showLabels = debugPitch < 0.95;

if (showLabels) {
  for (const [id, label] of Object.entries({
    ID04: 'HEAD ANCHOR',
    ID02: 'CHEST',
    ID10: 'PELVIS',
    ID09: 'R HAND',
    ID11: 'L HAND',
    ID16: 'R FOOT',
    ID17: 'L FOOT',
    ID18: 'MOUTH',
    ID19: 'R NIPPLE',
    ID20: 'L NIPPLE',
    ID21: 'GENITAL',
    ID22: 'ANUS',
    ID23: 'R OUTER',
    ID24: 'L OUTER',
    ID25: 'R LOWER',
    ID26: 'L LOWER',
    ID27: 'B CENTER'
  })) {
    const p = projected[id];
    if (!p) continue;

    let lx = p.x + 6;
    let ly = p.y - 6;

    if (id === 'ID19' || id === 'ID20') ly -= 6;
    if (id === 'ID23' || id === 'ID24') lx += 8;
    if (id === 'ID25' || id === 'ID26') ly += 6;
    if (id === 'ID27') lx -= 8;

    ctx.fillText(label, lx, ly);
  }
}

  const meta = document.getElementById('pose-min-meta');
  if (meta) {
const facing = getFacingClassFromCamera(pose.camera || {});
const facingLabel =
  facing === 1 ? '1 FRONT' :
  facing === 2 ? '2 FRONT-SIDE' :
  facing === 3 ? '3 SIDE' :
  facing === 4 ? '4 BACK-SIDE' :
  '5 BACK';
meta.textContent =
  `frame: ${pose.frame ?? '-'}\n` +
  `root: ${pose.root ?? '-'}\n` +
  `points: ${Object.keys(pose.points || {}).length}\n` +
`view: ${facingLabel}`;
    meta.style.whiteSpace = 'pre-line';
  }
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

    if (!jsonText) {
      setStatus('status: pose block not found');
      return;
    }

    console.log('[POSE JSON RAW]', jsonText);

    let parsed;
    try {
      parsed = JSON.parse(jsonText);
      console.log('[POSE PARSED OK]', parsed);
    } catch (err) {
      console.error('[POSE PARSE ERROR]', err);
      setStatus('status: parse error');
      return;
    }

    if (!parsed || typeof parsed !== 'object') {
      setStatus('status: invalid json object');
      return;
    }

    let nextPoints = null;
    const nextRoot = parsed.root || pose.root;

    if (parsed.points && typeof parsed.points === 'object') {
      nextPoints = parsed.points;
    } else if (
      parsed.ID00 ||
      parsed.ID01 ||
      parsed.ID10 ||
      parsed.ID18 ||
      parsed.ID22
    ) {
      nextPoints = parsed;
    } else {
      setStatus('status: invalid pose shape');
      console.log('[POSE INVALID SHAPE]', parsed);
      return;
    }

    try {
      ensureCameraDefaults();

const normalizedParsedCamera = normalizeIncomingCamera(parsed.camera || {});

// 手動視点を優先し、外部からは scale / tx / ty だけ受ける
const mergedCamera = {
  ...(pose.camera || {}),
  scale: typeof normalizedParsedCamera.scale === 'number'
    ? normalizedParsedCamera.scale
    : (pose.camera?.scale ?? 1),
  tx: typeof normalizedParsedCamera.tx === 'number'
    ? normalizedParsedCamera.tx
    : (pose.camera?.tx ?? 0),
  ty: typeof normalizedParsedCamera.ty === 'number'
    ? normalizedParsedCamera.ty
    : (pose.camera?.ty ?? 0)
};

if (forceFrontView) {
  mergedCamera.yaw = 0;
  mergedCamera.pitch = 0;
  mergedCamera.roll = 0;
}

pose = {
  ...pose,
  ...parsed,
  root: nextRoot,
  camera: mergedCamera,
  points: {
    ...pose.points,
    ...nextPoints
  }
};

ensureCameraDefaults();
//applyForcedViewIfNeeded();

const legWarnings = validateLegChain(pose.points);
if (legWarnings.length) {
  console.warn('[POSE LEG WARNING]', legWarnings);
}

drawPose();

const count = Object.keys(pose.points || {}).length;
console.log('[POSE DRAW OK]', count);

if (legWarnings.length) {
  setStatus(`status: pose updated (${count} points) / leg warning`);
} else {
  setStatus(`status: pose updated (${count} points)`);
}
    } catch (err) {
      console.error('[POSE DRAW ERROR]', err);
      setStatus('status: draw error');
    }
  }

  function checkAndAutoSyncPose() {
    const text = document.body?.textContent || '';
    const block = extractLatestPoseJsonBlock(text);

    if (!block) return;
    if (block === lastPoseBlock) return;

    lastPoseBlock = block;
    console.log('[POSE AUTO SYNC]');
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
      if (!text.includes('<POSE_JSON_START>')) return;

      removePoseJsonBlocksFromDom(document.body);
      setStatus('status: pose json removed from page');
    }, 700);
  });

  observer.observe(document.body, {
    childList: true,
    subtree: true,
    characterData: true
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

      pose.camera.yaw += dx * 0.01;
      pose.camera.pitch += dy * 0.01;
      drawPose();
    }

    if (isPanning) {
      pose.camera.tx += dx;
      pose.camera.ty += dy;
      drawPose();
    }
  });

  document.addEventListener('mouseup', () => {
    isDragging = false;
    isPanning = false;
    isPanelDragging = false;
  });
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

      const { fitScale } = getLayoutMetrics(
        pose.points || {},
        canvas.width,
        canvas.height
      );

      const oldUserScale = pose.camera.scale || 1;
      const oldScale = fitScale * oldUserScale;

      const zoomFactor = e.deltaY < 0 ? 1.12 : 1 / 1.12;
      let newUserScale = oldUserScale * zoomFactor;

      if (newUserScale < 0.2) newUserScale = 0.2;
      if (newUserScale > 5) newUserScale = 5;

      const newScale = fitScale * newUserScale;

      const tx = pose.camera.tx || 0;
      const ty = pose.camera.ty || 0;

      const localX = (mx - canvas.width / 2 - tx) / oldScale;
      const localY = -(my - canvas.height / 2 - ty) / oldScale;

      pose.camera.scale = newUserScale;
      pose.camera.tx = mx - canvas.width / 2 - localX * newScale;
      pose.camera.ty = my - canvas.height / 2 + localY * newScale;

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
  pose.camera.yaw = Math.PI;
  drawPose();
}));

camRow.appendChild(makeButton('LEFT', () => {
  setForceFrontUI(false, true);
  resetCamera(false);
  pose.camera.yaw = -Math.PI / 2;
  drawPose();
}));

camRow.appendChild(makeButton('RIGHT', () => {
  setForceFrontUI(false, true);
  resetCamera(false);
  pose.camera.yaw = Math.PI / 2;
  drawPose();
}));

camRow.appendChild(makeButton('TOP', () => {
  setForceFrontUI(false, true);
  resetCamera(false);
  pose.camera.pitch = -Math.PI / 2;
  drawPose();
}));

camRow.appendChild(makeButton('BOTTOM', () => {
  setForceFrontUI(false, true);
  resetCamera(false);
  pose.camera.pitch = Math.PI / 2;
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
  setStatus('status: boot ok');
}

  boot();
  window.addEventListener('load', boot);
  setTimeout(boot, 500);
  setTimeout(boot, 1500);
  setTimeout(boot, 3000);
function updateStripButtonLabel(btn) {
  if (!btn) return;
  btn.textContent = stripPoseJsonOnSend ? 'STRIP: ON' : 'STRIP: OFF';
}
    function findGeminiComposer() {
  return (
    document.querySelector('[contenteditable="true"][role="textbox"]') ||
    document.querySelector('div[contenteditable="true"]') ||
    document.querySelector('rich-textarea div[contenteditable="true"]')
  );
}

function stripPoseJsonFromComposer() {
  if (!stripPoseJsonOnSend) return false;

  const composer = findGeminiComposer();
  if (!composer) return false;

  const before = composer.innerText || composer.textContent || '';
  const after = removePoseJsonBlocks(before);

  if (before === after) return false;

  // ここを変更
  composer.innerText = after;

  composer.dispatchEvent(new InputEvent('input', { bubbles: true }));
  composer.dispatchEvent(new Event('change', { bubbles: true }));

  setStatus('status: pose json stripped before send');
  return true;
}


function removePoseJsonBlocks(text) {
  if (!text) return text;
  return text.replace(
    /<POSE_JSON_START>[\s\S]*?<POSE_JSON_END>\s*/g,
    ''
  ).trim();
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
    const replaced = original.replace(
      /<POSE_JSON_START>[\s\S]*?<POSE_JSON_END>\s*/g,
      ''
    );

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
function drawNeck(ctx, headAnchor, shoulderCenter, headRadius, color) {
  if (!headAnchor || !shoulderCenter) return;

  const neckWidth = Math.max(12, headRadius * 0.32);

  const neckTop = {
    x: lerp(headAnchor.x, shoulderCenter.x, 0.12),
    y: headAnchor.y + headRadius * 0.92
  };

  const neckBottom = {
    x: shoulderCenter.x,
    y: shoulderCenter.y + neckWidth * 0.04
  };

  drawCapsule(ctx, neckTop, neckBottom, neckWidth, color);
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
function getViewMetrics(rawPoints, cam, currentScale = 1) {
  const pR = rawPoints?.ID05;
  const pL = rawPoints?.ID06;
  const chest = rawPoints?.ID02;
  const pelvis = rawPoints?.ID10;

  if (!pR || !pL || !chest || !pelvis) {
    return {
      sideStrength: 0,
      frontBackSign: 1,
      shoulderScreenWidth: 0,
      torsoScreenHeight: 0
    };
  }

  const rr = rotatePoint(pR, cam || {});
  const rl = rotatePoint(pL, cam || {});
  const rc = rotatePoint(chest, cam || {});
  const rp = rotatePoint(pelvis, cam || {});

  const shoulderScreenWidth = Math.abs(rr.x - rl.x) * currentScale;
  const torsoScreenHeight = Math.abs(rc.y - rp.y) * currentScale;

  const rawShoulderWidth3D = dist3D(pR, pL);

  const denom = Math.max(0.0001, rawShoulderWidth3D);
  const sideStrength = clamp(1 - Math.abs(rr.x - rl.x) / denom, 0, 1);

const yaw = cam?.yaw || 0;
const frontBackSign = Math.cos(yaw) >= 0 ? 1 : -1;

  return {
    sideStrength,
    frontBackSign,
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

  const blendStart = 0.18;
  const blendEnd = 0.82;

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

  const side = (Math.sin(cam?.yaw || 0) >= 0) ? 1 : -1;

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

const tTop = Math.min(1, t * 1.8);

const shape = {
  neckBack: mixPoint(front.neckBack, side.neckBack, tTop),
  upperBack: mixPoint(front.upperBack, side.upperBack, tTop),
  lowerBack: mixPoint(front.lowerBack, side.lowerBack, t),
  backBottom: mixPoint(front.backBottom, side.backBottom, t),
  frontBottom: mixPoint(front.frontBottom, side.frontBottom, t),
  belly: mixPoint(front.belly, side.belly, t),
  chestFront: mixPoint(front.chestFront, side.chestFront, tTop),
  neckFront: mixPoint(front.neckFront, side.neckFront, tTop)
};

  drawTorsoShape(ctx, shape, color);
}
function drawSideTorso(ctx, P, rawPoints, color, cam, refs, view) {
  const shape = getSideTorsoShape(P, rawPoints, cam, refs, view);
  drawTorsoShape(ctx, shape, color);
}
function drawBreasts(ctx, projected, rawPoints, color, refs, cam, currentScale, view) {
  if (!projected || !rawPoints) return;


const chest = projected.ID02;
const nippleR = projected.ID19;
const nippleL = projected.ID20;

const rawChest = rawPoints.ID02;
const rawNippleR = rawPoints.ID19;
const rawNippleL = rawPoints.ID20;
const rawOuterR = rawPoints.ID23;
const rawOuterL = rawPoints.ID24;
const rawLowerR = rawPoints.ID25;
const rawLowerL = rawPoints.ID26;
const rawRShoulder = rawPoints.ID05;
const rawLShoulder = rawPoints.ID06;

if (!chest || !nippleR || !nippleL || !rawChest || !rawRShoulder || !rawLShoulder) return;

const shoulderWidth3D = dist3D(rawRShoulder, rawLShoulder);

const rawSpineUpper = rawPoints.ID02; // Thoracic
const rawSpineLower = rawPoints.ID01; // Lumbar

// 前方向の出っ張り
const forwardDepth = Math.max(
  rawNippleR ? dist3D(rawSpineUpper, rawNippleR) : 0,
  rawNippleL ? dist3D(rawSpineUpper, rawNippleL) : 0
);

// 横方向の広がり
const lateralWidth = (
  rawOuterR && rawOuterL
    ? dist3D(rawOuterR, rawOuterL)
    : 0
);

// 下方向のボリューム
const lowerDrop = Math.max(
  rawLowerR ? dist3D(rawSpineUpper, rawLowerR) : 0,
  rawLowerL ? dist3D(rawSpineUpper, rawLowerL) : 0
);

// 胸の中心～乳首の基本距離も少しだけ混ぜる
const nippleDepth = Math.max(
  rawNippleR ? dist3D(rawChest, rawNippleR) : 0,
  rawNippleL ? dist3D(rawChest, rawNippleL) : 0
);

// リアル寄り合成
const breastSize3D =
  forwardDepth * 0.42 +
  lateralWidth * 0.28 +
  lowerDrop * 0.30 +
  nippleDepth * 0.18;

// 最終半径
const radius = clamp(
  breastSize3D * currentScale * 0.42,
  8,
  42
);
    const nippleDrop = radius * 0.18;

    const nR = {
  x: nippleR.x,
  y: nippleR.y + nippleDrop
};

const nL = {
  x: nippleL.x,
  y: nippleL.y + nippleDrop
};



const sideStrength = view?.sideStrength || 0;
const frontness = 1 - sideStrength;
const frontBackSign = view?.frontBackSign || 1;

// pitch は abs にしない
const pitchValue = cam?.pitch || 0;

// 見上げ（あおり）と見下ろしを分ける
const lookUpStrength = smoothstep01((-pitchValue - 0.35) / 0.75);
const lookDownStrength = smoothstep01(( pitchValue - 0.35) / 0.75);

// 俯瞰のときだけ潰す
const topFade = 1 - smoothstep01((lookDownStrength - 0.15) / 0.85);

// 乳首TOP基準
const forward = radius * lerp(0.45, 0.30, lookUpStrength);

// ベースの左右広がり
const spreadXBase = clamp(
  shoulderWidth3D * currentScale * 0.03,
  0,
  radius * 0.22
) * frontness;

// あおりでは左右に開きすぎない
const spreadX = spreadXBase * lerp(1.0, 0.22, lookUpStrength);

// 真横寄りでは胸本体を少し後ろへ逃がす
const backShift = radius * 0.22 * sideStrength * frontBackSign;

const inward = radius * 0.18;
const lookUpInward = radius * 0.16 * lookUpStrength;// あおり時に中央寄せ
const lift = radius * lerp(0.32, 0.18, lookUpStrength);

// あおり時は少し下側ボリュームを残す
const sag = radius * lerp(
  lerp(0.18, 0.35, frontness),
  0.28,
  lookUpStrength
);

// 胸の中心
const centerR = {
  x: nR.x + spreadX - backShift - inward - lookUpInward,
  y: nR.y + radius - forward - lift + sag
};

const centerL = {
  x: nL.x - spreadX - backShift + inward + lookUpInward,
  y: nL.y + radius - forward - lift + sag
};

// あおりでは少しだけ大きく見せる
const lookUpScale = lerp(1.0, 1.18, lookUpStrength);

const baseRx = radius * lerp(1.0, 0.72, sideStrength) * lookUpScale;
const baseRy = radius * lerp(1.0, 0.92, sideStrength) * lookUpScale;

// 潰しすぎ防止
const rx = baseRx * lerp(0.72, 1.0, topFade);
const ry = baseRy * lerp(0.58, 1.0, topFade);

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

  const side = (Math.sin(cam?.yaw || 0) >= 0) ? 1 : -1;

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

const pitch = Math.abs(pose?.camera?.pitch || 0);
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

  const innerRightTop = {
    x: hipR.x - upperLegWidth * 0.22,
    y: hipR.y + upperLegWidth * 0.36
  };

  const innerLeftTop = {
    x: hipL.x + upperLegWidth * 0.22,
    y: hipL.y + upperLegWidth * 0.36
  };

  const crotchTop = {
    x: pelvis.x,
    y: pelvis.y + upperLegWidth * 0.04
  };

  const split = upperLegWidth * 0.22;

  const crotchBottomR = {
    x: midHip.x + split,
    y: Math.max(innerRightTop.y, innerLeftTop.y) + upperLegWidth * 0.12
  };

  const crotchBottomL = {
    x: midHip.x - split,
    y: Math.max(innerRightTop.y, innerLeftTop.y) + upperLegWidth * 0.12
  };

  ctx.fillStyle = color;
  ctx.beginPath();
  ctx.moveTo(crotchTop.x, crotchTop.y);

  ctx.quadraticCurveTo(
    innerRightTop.x,
    innerRightTop.y,
    crotchBottomR.x,
    crotchBottomR.y
  );

  ctx.quadraticCurveTo(
    midHip.x,
    crotchBottomR.y + upperLegWidth * 0.06,
    crotchBottomL.x,
    crotchBottomL.y
  );

  ctx.quadraticCurveTo(
    innerLeftTop.x,
    innerLeftTop.y,
    crotchTop.x,
    crotchTop.y
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
    head.x + side * r * 0.52,
    head.y - r * 0.80
  );
  ctx.rotate(-0.55 * side);
  ctx.beginPath();
  ctx.ellipse(0, 0, r * 0.48, r * 0.70, 0, 0, Math.PI * 2);
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
      head.x - side * r * 0.10,
      head.y + r * 0.02,
      r * 1.10,
      r * 1.18,
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

})();

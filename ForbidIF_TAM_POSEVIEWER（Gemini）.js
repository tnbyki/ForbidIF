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
      ID17: { name: 'Left Heel', x: -0.16, y: -1.35, z: 0.00 }
    }
  };

  let lastPoseBlock = '';
  let autoSyncObserverStarted = false;
  let autoSyncTimer = null;
  let removePoseTimer = null;

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

  function resetCamera() {
    pose.camera.yaw = 0;
    pose.camera.pitch = 0;
    pose.camera.roll = 0;
    pose.camera.scale = 1;
    pose.camera.tx = 0;
    pose.camera.ty = 0;
    drawPose();
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

function drawPose() {
  const panel = getOrCreatePanel();
  const canvas = panel.querySelector('#' + CANVAS_ID);
  const ctx = canvas.getContext('2d');

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

  const bodyColor = '#e6e6e6';
  const limbColor = '#d9d9d9';
  const shadowColor = 'rgba(0,0,0,0.22)';

  const hipCenter = midpoint(rHip, lHip) || pelvis;

const shoulderWidth = dist2D(rShoulder, lShoulder);

const { fitScale } = getLayoutMetrics(
  pose.points || {},
  canvas.width,
  canvas.height
);
const currentScale = fitScale * (pose.camera.scale || 1);

const rawShoulderWidth = dist3D(pose.points.ID05, pose.points.ID06);

const headRadius = Math.max(18, Math.min(80, rawShoulderWidth * currentScale * 0.36 || 22));
const armWidth = Math.max(9, Math.min(28, shoulderWidth * 0.26 || 12));
const legWidth = Math.max(8, Math.min(30, shoulderWidth * 0.24 || 11));


const thighWidth = legWidth * 1.25;

const shoulderLift = Math.max(6, headRadius * 0.22);
const rShoulderDraw = rShoulder ? { x: rShoulder.x, y: rShoulder.y - shoulderLift } : null;
const lShoulderDraw = lShoulder ? { x: lShoulder.x, y: lShoulder.y - shoulderLift } : null;

const shoulderCenter = midpoint(rShoulderDraw, lShoulderDraw) || chest || neck;
  // 影を少しだけ
  ctx.save();
  ctx.translate(1.5, 1.5);

  drawRoundLimb(ctx, rHip, rKnee, legWidth, shadowColor);
  drawRoundLimb(ctx, rKnee, rHeel, legWidth * 0.95, shadowColor);
  drawRoundLimb(ctx, lHip, lKnee, legWidth, shadowColor);
  drawRoundLimb(ctx, lKnee, lHeel, legWidth * 0.95, shadowColor);

drawRoundLimb(ctx, rShoulderDraw, rElbow, armWidth, shadowColor);
drawRoundLimb(ctx, lShoulderDraw, lElbow, armWidth, shadowColor);

drawCircle(ctx, rShoulderDraw, armWidth * 0.52, shadowColor);
drawCircle(ctx, lShoulderDraw, armWidth * 0.52, shadowColor);

 //胴
drawSmartTorso(ctx, P, pose.points, shadowColor, pose.camera || {}, {
  chest, pelvis,
  rShoulder: rShoulderDraw,
  lShoulder: lShoulderDraw,
  rHip, lHip
});
const shadowHeadDraw = head ? { x: head.x, y: head.y + headRadius } : null;
drawCircle(ctx, shadowHeadDraw, headRadius, shadowColor);

  ctx.restore();

    // 股関節を丸く
drawCircle(ctx, rHip, legWidth * 0.45, bodyColor);
drawCircle(ctx, lHip, legWidth * 0.45, bodyColor);

  // 脚
drawRoundLimb(ctx, rHip, rKnee, thighWidth, limbColor);
drawRoundLimb(ctx, rKnee, rHeel, legWidth * 0.95, limbColor);
drawRoundLimb(ctx, lHip, lKnee, thighWidth, limbColor);
drawRoundLimb(ctx, lKnee, lHeel, legWidth * 0.95, limbColor);

  // 股を埋める
  drawCrotchFill(ctx, pelvis, rHip, lHip, legWidth, bodyColor);

  // 腕
drawRoundLimb(ctx, rShoulderDraw, rElbow, armWidth, limbColor);
drawRoundLimb(ctx, rElbow, rWrist, armWidth * 0.92, limbColor);
drawRoundLimb(ctx, lShoulderDraw, lElbow, armWidth, limbColor);
drawRoundLimb(ctx, lElbow, lWrist, armWidth * 0.92, limbColor);

drawCircle(ctx, rShoulderDraw, armWidth * 0.52, bodyColor);
drawCircle(ctx, lShoulderDraw, armWidth * 0.52, bodyColor);

drawSmartTorso(ctx, P, pose.points, bodyColor, pose.camera || {}, {
  chest, pelvis,
  rShoulder: rShoulderDraw,
  lShoulder: lShoulderDraw,
  rHip, lHip
});

  // 頭
const HEAD_ANCHOR_MODE = 'TOP';   // ID04 は頭頂寄りアンカー
const headCenterOffsetY = (HEAD_ANCHOR_MODE === 'TOP') ? headRadius : headRadius * 0.10;
const headDraw = head ? { x: head.x, y: head.y + headCenterOffsetY } : null;
drawCircle(ctx, headDraw, headRadius, bodyColor);

      // 骨格点を青で表示
  const jointIds = [
    'ID03', 'ID04',
    'ID05', 'ID06',
    'ID07', 'ID08',
    'ID09', 'ID11',
    'ID10',
    'ID12', 'ID13',
    'ID14', 'ID15',
    'ID16', 'ID17'
  ];

  for (const id of jointIds) {
    const p = projected[id];
    if (!p) continue;

    if (id === 'ID10') {
      ctx.fillStyle = '#f2c94c'; // pelvis は黄色のままでもOK
      ctx.beginPath();
      ctx.arc(p.x, p.y, 3.5, 0, Math.PI * 2);
      ctx.fill();
      continue;
    }

    ctx.fillStyle = '#66ccff';
    ctx.beginPath();
    ctx.arc(p.x, p.y, 3.0, 0, Math.PI * 2);
    ctx.fill();
  }




  // 首を少しだけ足す
if (head && shoulderCenter) {
  drawNeck(ctx, head, shoulderCenter, headRadius, bodyColor);
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
    const { id, screen } = item;

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
    ctx.arc(screen.x, screen.y, 3.0, 0, Math.PI * 2);
    ctx.fill();
  }

  ctx.fillStyle = '#aaa';
  ctx.font = '10px sans-serif';

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

  const meta = document.getElementById('pose-min-meta');
  if (meta) {
    meta.textContent =
      `frame: ${pose.frame ?? '-'}\n` +
      `root: ${pose.root ?? '-'}\n` +
      `points: ${Object.keys(pose.points || {}).length}`;
    meta.style.whiteSpace = 'pre-line';
  }
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

      const mergedCamera = {
        yaw: 0,
        pitch: 0,
        roll: 0,
        scale: 1,
        tx: 0,
        ty: 0,
        ...(pose.camera || {}),
        ...(parsed.camera || {})
      };

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
      drawPose();

      const count = Object.keys(pose.points || {}).length;
      console.log('[POSE DRAW OK]', count);
      setStatus(`status: pose updated (${count} points)`);
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

    camRow.appendChild(makeButton('FRONT', () => resetCamera()));
    camRow.appendChild(makeButton('BACK', () => {
      resetCamera();
      pose.camera.yaw = Math.PI;
      drawPose();
    }));
    camRow.appendChild(makeButton('LEFT', () => {
      resetCamera();
      pose.camera.yaw = -Math.PI / 2;
      drawPose();
    }));
    camRow.appendChild(makeButton('RIGHT', () => {
      resetCamera();
      pose.camera.yaw = Math.PI / 2;
      drawPose();
    }));
    camRow.appendChild(makeButton('TOP', () => {
      resetCamera();
      pose.camera.pitch = -Math.PI / 2;
      drawPose();
    }));
    camRow.appendChild(makeButton('BOTTOM', () => {
      resetCamera();
      pose.camera.pitch = Math.PI / 2;
      drawPose();
    }));

    bodyWrap.appendChild(camRow);
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

  const neckWidth = Math.max(16, headRadius * 0.42);

  // ID04 は「頭頂寄りアンカー」
  const neckTop = {
    x: headAnchor.x,
    y: headAnchor.y + headRadius * 1.55
  };

  const neckBottom = {
    x: shoulderCenter.x,
    y: shoulderCenter.y + neckWidth * 0.08
  };

  drawCapsule(ctx, neckTop, neckBottom, neckWidth, color);
}
function drawSoftTorso(ctx, chest, pelvis, shoulderR, shoulderL, hipR, hipL, color) {
  if (!shoulderR || !shoulderL || !hipR || !hipL) return;

  const chestCenter = midpoint(shoulderR, shoulderL) || chest;
  const hipCenter = midpoint(hipR, hipL) || pelvis;
  if (!chestCenter || !hipCenter) return;

const shoulderInset = 0.08;
const hipInset = 0.02;
const curveY = Math.max(4, dist2D(shoulderR, shoulderL) * 0.10);

  const topR = {
    x: shoulderR.x + (chestCenter.x - shoulderR.x) * shoulderInset,
    y: shoulderR.y + (chestCenter.y - shoulderR.y) * shoulderInset
  };
  const topL = {
    x: shoulderL.x + (chestCenter.x - shoulderL.x) * shoulderInset,
    y: shoulderL.y + (chestCenter.y - shoulderL.y) * shoulderInset
  };

  const botR = {
    x: hipR.x + (hipCenter.x - hipR.x) * hipInset,
    y: hipR.y + (hipCenter.y - hipR.y) * hipInset
  };
  const botL = {
    x: hipL.x + (hipCenter.x - hipL.x) * hipInset,
    y: hipL.y + (hipCenter.y - hipL.y) * hipInset
  };

  ctx.fillStyle = color;
  ctx.beginPath();
  ctx.moveTo(topL.x, topL.y);
ctx.quadraticCurveTo(chestCenter.x, chestCenter.y - curveY, topR.x, topR.y);
  ctx.lineTo(botR.x, botR.y);
ctx.quadraticCurveTo(hipCenter.x, hipCenter.y + curveY * 1.2, botL.x, botL.y);
  ctx.closePath();
  ctx.fill();
}
function drawCrotchFill(ctx, pelvis, hipR, hipL, upperLegWidth, color) {
  if (!pelvis || !hipR || !hipL) return;

  const midHip = midpoint(hipR, hipL);
  if (!midHip) return;

  const drop = upperLegWidth * 0.55;

  const pTop = {
    x: pelvis.x,
    y: pelvis.y + upperLegWidth * 0.10
  };

  const pRight = {
    x: hipR.x - upperLegWidth * 0.18,
    y: hipR.y + drop
  };

  const pLeft = {
    x: hipL.x + upperLegWidth * 0.18,
    y: hipL.y + drop
  };

  ctx.fillStyle = color;
  ctx.beginPath();
  ctx.moveTo(pTop.x, pTop.y);
  ctx.lineTo(pRight.x, pRight.y);
  ctx.lineTo(pLeft.x, pLeft.y);
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

function getSideViewStrength(cam) {
  const yaw = cam?.yaw || 0;
  return Math.abs(Math.sin(yaw));
}
function smoothstep01(t) {
  t = clamp(t, 0, 1);
  return t * t * (3 - 2 * t);
}

function drawSmartTorso(ctx, projected, rawPoints, color, cam, refs) {
  const sideStrength = getSideViewStrength(cam);

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
    drawSideTorso(ctx, projected, rawPoints, color, cam, refs);
    return;
  }

  const t = smoothstep01((sideStrength - blendStart) / (blendEnd - blendStart));
  drawBlendTorso(ctx, projected, rawPoints, color, cam, refs, t);
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

  const shoulderInset = 0.08;
  const hipInset = 0.02;
  const curveY = Math.max(4, dist2D(shoulderR, shoulderL) * 0.10);

  const topR = {
    x: shoulderR.x + (chestCenter.x - shoulderR.x) * shoulderInset,
    y: shoulderR.y + (chestCenter.y - shoulderR.y) * shoulderInset
  };

  const topL = {
    x: shoulderL.x + (chestCenter.x - shoulderL.x) * shoulderInset,
    y: shoulderL.y + (chestCenter.y - shoulderL.y) * shoulderInset
  };

  const botR = {
    x: hipR.x + (hipCenter.x - hipR.x) * hipInset,
    y: hipR.y + (hipCenter.y - hipR.y) * hipInset
  };

  const botL = {
    x: hipL.x + (hipCenter.x - hipL.x) * hipInset,
    y: hipL.y + (hipCenter.y - hipL.y) * hipInset
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
    lowerBack: botL,
    backBottom: botL,
    frontBottom: botR,
    belly: {
      x: (botR.x + topR.x) * 0.5,
      y: (botR.y + topR.y) * 0.5
    },
    chestFront: topR,
    neckFront
  };
}

function getSideTorsoShape(P, rawPoints, cam, refs) {
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

  const torsoHalf = clamp(shoulderSpan * 0.22, 10, 24);
  const waistHalf = clamp(hipSpan * 0.16, 8, 18);

  const bustDepth = clamp(
    Math.max(
      dist2D(breastCenter, chest),
      dist2D(breastLowerR, chest),
      dist2D(breastLowerL, chest)
    ) * 0.9,
    6,
    24
  );

const bellyDepth = clamp(torsoHalf * 0.66 + bustDepth * 0.22, 7, 22);
const buttDepth = clamp(waistHalf * 1.10 + torsoHalf * 0.48, 10, 28);

  const sideBoost = lerp(0.55, 1.0, clamp(getSideViewStrength(cam), 0, 1));

const chestFront = {
  x: chest.x + side * (torsoHalf * 0.32 + bustDepth * sideBoost * 0.75),
  y: chest.y - torsoHeight * 0.01
};

const upperBack = {
  x: chest.x - side * (torsoHalf * 0.30),
  y: chest.y - torsoHeight * 0.01
};

  const belly = {
    x: lerp(chest.x, pelvis.x, 0.45) + side * (bellyDepth * sideBoost),
    y: lerp(chest.y, pelvis.y, 0.48)
  };

  const groinFront = {
    x: genital.x + side * (waistHalf * 0.45),
    y: genital.y + torsoHeight * 0.03
  };

  const lowerBack = {
    x: lerp(neck.x, pelvis.x, 0.70) - side * (waistHalf * 0.40),
    y: lerp(neck.y, pelvis.y, 0.72)
  };

  const butt = {
    x: anus.x - side * (buttDepth * sideBoost),
    y: anus.y + torsoHeight * 0.02
  };

const neckFrontBase = mixPoint(mouth, neck, 0.65) || neck;
const neckY = chest.y + torsoHeight * 0.04;

const neckFront = {
  x: chestFront.x,
  y: neckY
};

const neckBack = {
  x: upperBack.x,
  y: neckY
};

  const frontBottom = mixPoint(groinFront, pelvis, 0.35);
  const backBottom = mixPoint(butt, pelvis, 0.35);

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
    y: lerp(shape.upperBack.y, shape.chestFront.y, 0.42) + 0.0
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

function drawBlendTorso(ctx, P, rawPoints, color, cam, refs, t) {
  const front = getFrontTorsoShape(P, refs);
  const side = getSideTorsoShape(P, rawPoints, cam, refs);

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
    function drawSideTorso(ctx, P, rawPoints, color, cam, refs) {
  const shape = getSideTorsoShape(P, rawPoints, cam, refs);
  drawTorsoShape(ctx, shape, color);
}
})();

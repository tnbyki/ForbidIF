// ==UserScript==
// @name         Gemini Pose Viewer Minimal
// @namespace    http://tampermonkey.net/
// @version      0.1
// @match        https://gemini.google.com/*
// @grant        none
// @run-at       document-idle
// ==/UserScript==

(function () {
  'use strict';

  const PANEL_ID = 'gemini-pose-min-panel';
  const CANVAS_ID = 'gemini-pose-min-canvas';

    let isDragging = false;
    let isPanning = false;
    let lastMouseX = 0;
    let lastMouseY = 0;



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


headerRow.appendChild(title);
headerRow.appendChild(toggleBtn);
panel.appendChild(headerRow);

const bodyWrap = document.createElement('div');
bodyWrap.id = 'pose-min-body';
panel.appendChild(bodyWrap);



    const canvas = document.createElement('canvas');
    canvas.id = CANVAS_ID;
    canvas.width = 176;
    canvas.height = 260;
    canvas.style.background = '#0b0b0b';
    canvas.style.border = '1px solid #333';
    canvas.style.borderRadius = '6px';
    canvas.style.display = 'block';
canvas.addEventListener('mousedown', (e) => {
  lastMouseX = e.clientX;
  lastMouseY = e.clientY;

  if (e.button === 1) {
    isPanning = true;   // ホイール押し込み
  } else {
    isDragging = true;  // 左ドラッグ
  }
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

document.addEventListener('mousemove', (e) => {

  const dx = e.clientX - lastMouseX;
  const dy = e.clientY - lastMouseY;

  lastMouseX = e.clientX;
  lastMouseY = e.clientY;

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
});
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

row.appendChild(makeButton('RESET', () => resetCamera()));
row.appendChild(makeButton('SYNC', () => syncPose()));

const camRow = document.createElement('div');
camRow.style.display = 'flex';
camRow.style.flexWrap = 'wrap';
camRow.style.gap = '4px';
camRow.style.marginTop = '6px';

camRow.appendChild(makeButton('FRONT', () => {
  pose.camera.yaw = 0;
  pose.camera.pitch = 0;
  drawPose();
}));

camRow.appendChild(makeButton('BACK', () => {
  pose.camera.yaw = Math.PI;
  drawPose();
}));

camRow.appendChild(makeButton('LEFT', () => {
  pose.camera.yaw = -Math.PI / 2;
  drawPose();
}));

camRow.appendChild(makeButton('RIGHT', () => {
  pose.camera.yaw = Math.PI / 2;
  drawPose();
}));

camRow.appendChild(makeButton('TOP', () => {
  pose.camera.pitch = -Math.PI / 2;
  drawPose();
}));

camRow.appendChild(makeButton('BOTTOM', () => {
  pose.camera.pitch = Math.PI / 2;
  drawPose();
}));

camRow.appendChild(makeButton('↑', () => {
  pose.camera.pitch -= 0.2;
  drawPose();
}));

camRow.appendChild(makeButton('↓', () => {
  pose.camera.pitch += 0.2;
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

function setStatus(text) {
  const el = document.getElementById('pose-min-status');
  if (el) el.textContent = text;
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

function rotatePoint(p, cam) {
  let x = p.x;
  let y = p.y;
  let z = p.z;

  const yaw = cam.yaw || 0;
  const pitch = cam.pitch || 0;

  const cy = Math.cos(yaw);
  const sy = Math.sin(yaw);

  const cp = Math.cos(pitch);
  const sp = Math.sin(pitch);

  // yaw (Y回転)
  let x1 = x * cy - z * sy;
  let z1 = x * sy + z * cy;

  // pitch (X回転)
  let y1 = y * cp - z1 * sp;
  let z2 = y * sp + z1 * cp;

  return { x: x1, y: y1, z: z2 };
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

  const userScale = pose.camera.scale || 1;
  const scale = fitScale * userScale;

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
    return {
      fitScale: 1,
      centerX: 0,
      centerY: 0
    };
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

    ctx.strokeStyle = '#d9d9d9';
    ctx.lineWidth = 2;
    for (const [a, b] of BONES) {
      const p1 = projected[a];
      const p2 = projected[b];
      if (!p1 || !p2) continue;
      ctx.beginPath();
      ctx.moveTo(p1.x, p1.y);
      ctx.lineTo(p2.x, p2.y);
      ctx.stroke();
    }

for (const [id, p] of Object.entries(projected)) {
  if (id === 'ID10') {
    ctx.fillStyle = '#f2c94c'; // pelvis
  } else if (['ID18', 'ID19', 'ID20', 'ID21', 'ID22'].includes(id)) {
    ctx.fillStyle = '#ff88aa'; // added markers
  } else {
    ctx.fillStyle = '#66ccff'; // normal joints
  }

  ctx.beginPath();
  ctx.arc(p.x, p.y, 3.5, 0, Math.PI * 2);
  ctx.fill();
}
ctx.fillStyle = '#aaa';
ctx.font = '10px sans-serif';

for (const [id, label] of Object.entries({
  ID04: 'HEAD',
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
  ID22: 'ANUS'
})) {
  const p = projected[id];
  if (!p) continue;
  ctx.fillText(label, p.x + 5, p.y - 4);
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
  const text = document.body?.innerText || '';
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
  let nextRoot = parsed.root || pose.root;

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

    const keepCamera = {
      yaw: pose.camera.yaw,
      pitch: pose.camera.pitch,
      roll: pose.camera.roll,
      scale: pose.camera.scale,
      tx: pose.camera.tx,
      ty: pose.camera.ty
    };

    pose = {
      ...pose,
      ...parsed,
      root: nextRoot,
      camera: keepCamera,
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


function boot() {
  getOrCreatePanel();
  ensureCameraDefaults();
  drawPose();
  startAutoSyncObserver();
  setStatus('status: boot ok');
}

let lastPoseBlock = '';
let autoSyncObserverStarted = false;
let autoSyncTimer = null;

function checkAndAutoSyncPose() {
  const text = document.body?.innerText || '';
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
  });

  observer.observe(document.body, {
    childList: true,
    subtree: true,
    characterData: true
  });

  setTimeout(checkAndAutoSyncPose, 400);
}

boot();
window.addEventListener('load', boot);
setTimeout(boot, 500);
setTimeout(boot, 1500);
setTimeout(boot, 3000);

})();
function sanitizePoseJsonText(text) {
  if (!text) return '';

  return text
    .replace(/[\u200B-\u200D\uFEFF]/g, '')   // zero width
    .replace(/\r/g, '')
    .replace(/[“”]/g, '"')
    .replace(/[‘’]/g, "'")
    .replace(/,\s*([}\]])/g, '$1')           // 末尾カンマ除去
    .trim();
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


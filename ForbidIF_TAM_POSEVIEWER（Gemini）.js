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

const SURFACE_IDS = new Set([
  'ID18', 'ID19', 'ID20', 'ID21', 'ID22',
  'ID23', 'ID24', 'ID25', 'ID26', 'ID27'
]);

const pointEntries = Object.entries(pose.points || {})
  .map(([id, src]) => {
    const screen = projected[id];
    const rotated = rotatePoint(src, pose.camera || {});
    return {
      id,
      screen,
      z: rotated.z
    };
  })
  .filter(item => item.screen)
  .sort((a, b) => a.z - b.z); // 奥→手前

const skeletonPoints = pointEntries.filter(item => !SURFACE_IDS.has(item.id));
const surfacePoints  = pointEntries.filter(item =>  SURFACE_IDS.has(item.id));

// 1) 骨格点
for (const item of skeletonPoints) {
  const { id, screen } = item;

  if (id === 'ID10') {
    ctx.fillStyle = '#f2c94c'; // pelvis
  } else {
    ctx.fillStyle = '#66ccff'; // normal joints
  }

  ctx.beginPath();
  ctx.arc(screen.x, screen.y, 3.5, 0, Math.PI * 2);
  ctx.fill();
}

// 2) 表面補助点（最後に描く）
for (const item of surfacePoints) {
  const { id, screen } = item;

  // 赤：口・乳首左右・秘部
  if (
    id === 'ID18' || // mouth
    id === 'ID19' || // R nipple
    id === 'ID20' || // L nipple
    id === 'ID21'    // genital
  ) {
    ctx.fillStyle = '#ff4d4f';
  }

  // 黄：肛門
  else if (id === 'ID22') {
    ctx.fillStyle = '#ffd84d';
  }

  // グレー：胸補助点
  else if (
    id === 'ID23' ||
    id === 'ID24' ||
    id === 'ID25' ||
    id === 'ID26' ||
    id === 'ID27'
  ) {
    ctx.fillStyle = '#9aa0a6';
  }

  else {
    ctx.fillStyle = '#4da3ff';
  }

  ctx.beginPath();
  ctx.arc(screen.x, screen.y, 3.2, 0, Math.PI * 2);
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

// 胸周辺ラベルを少しずらす
if (id === 'ID19' || id === 'ID20') ly -= 6;     // 乳首
if (id === 'ID23' || id === 'ID24') lx += 8;     // 外胸
if (id === 'ID25' || id === 'ID26') ly += 6;     // 下胸
if (id === 'ID27') lx -= 8;                      // 胸中央

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
    canvas.width = 176;
    canvas.height = 260;
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
})();

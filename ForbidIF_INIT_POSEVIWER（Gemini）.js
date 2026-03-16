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
    camera: { yaw: 0, pitch: 0, roll: 0, scale: 1 },
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

    const title = document.createElement('div');
    title.textContent = 'POSE VIEWER';
    title.style.fontWeight = '700';
    title.style.marginBottom = '6px';
    title.style.color = '#fff';
    panel.appendChild(title);

    const canvas = document.createElement('canvas');
    canvas.id = CANVAS_ID;
    canvas.width = 176;
    canvas.height = 260;
    canvas.style.background = '#0b0b0b';
    canvas.style.border = '1px solid #333';
    canvas.style.borderRadius = '6px';
    canvas.style.display = 'block';
    panel.appendChild(canvas);

    const meta = document.createElement('div');
    meta.id = 'pose-min-meta';
    meta.style.marginTop = '6px';
    meta.style.lineHeight = '1.3';
    panel.appendChild(meta);

    const row = document.createElement('div');
    row.style.display = 'flex';
    row.style.gap = '4px';
    row.style.marginTop = '6px';

    row.appendChild(makeButton('REDRAW', () => drawPose()));
    row.appendChild(makeButton('SYNC', () => syncPose()));

    panel.appendChild(row);

    const status = document.createElement('div');
    status.id = 'pose-min-status';
    status.style.marginTop = '6px';
    status.style.color = '#999';
    status.textContent = 'status: ready';
    panel.appendChild(status);

    document.body.appendChild(panel);
    return panel;
  }

  function setStatus(text) {
    const el = document.getElementById('pose-min-status');
    if (el) el.textContent = text;
  }

  function projectPoint(p) {
    return { x: Number(p.x || 0), y: Number(p.y || 0) };
  }

  function computeLayout(points, width, height) {
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

    const scale = Math.min(
      (width - padding * 2) / spanX,
      (height - padding * 2) / spanY
    );

    const centerX = (minX + maxX) / 2;
    const centerY = (minY + maxY) / 2;

    for (const id of ids) {
      const p = projected[id];
      projected[id] = {
        x: width / 2 + (p.x - centerX) * scale,
        y: height / 2 - (p.y - centerY) * scale
      };
    }

    return projected;
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
      ctx.fillStyle = id === 'ID10' ? '#f2c94c' : '#66ccff';
      ctx.beginPath();
      ctx.arc(p.x, p.y, 3.5, 0, Math.PI * 2);
      ctx.fill();
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

  if (!parsed.points || !parsed.root) {
    setStatus('status: invalid pose shape');
    return;
  }

  try {
    pose = parsed;
    drawPose();
    console.log('[POSE DRAW OK]');
    setStatus('status: pose updated');
  } catch (err) {
    console.error('[POSE DRAW ERROR]', err);
    setStatus('status: draw error');
  }
}


  function boot() {
    getOrCreatePanel();
    drawPose();
    setStatus('status: boot ok');
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

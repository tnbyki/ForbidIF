// ==UserScript==
// @name         ForbidIF Voice Bridge (Stable + Toggle)
// @namespace    https://tnbyki.example/forbidif
// @version      3.1.0
// @description  Scoped detect [V]...[/V], send to WAN, stable remove, toggle UI.
// @match        https://chatgpt.com/*
// @match        https://chat.openai.com/*
// @match        https://gemini.google.com/*
// @match        https://grok.com/*
// @match        https://x.com/i/grok*
// @run-at       document-idle
// @grant        GM_xmlhttpRequest
// @connect      127.0.0.1
// @connect      localhost
// ==/UserScript==

(() => {
  "use strict";

  console.log("[TAM] injected", location.href);

  const WAN_ENDPOINT = "http://127.0.0.1:5000/voice_input";
  const VTAG_REGEX = /\[V\][\s\S]*?\[\/V\]/g;

  // Toggle: remove [V] blocks from UI or not
  let REMOVE_ENABLED = true;

  // Keep already-sent [V] blocks so we don't re-send history
  const seen = new Set();

  // ----------------------
  // Scope root (best effort):
  // - ChatGPT: main article (latest)
  // - Otherwise fallback to body
  // ----------------------
  function getScopeRoot() {
    try {
      const articles = document.querySelectorAll("main article");
      if (articles && articles.length) return articles[articles.length - 1];
    } catch (_) {}
    return document.body;
  }

  function extractV(text) {
    const m = text.match(VTAG_REGEX);
    return m ? m.map(v => v.trim()).filter(Boolean) : [];
  }

  function send(lines) {
    if (!lines || !lines.length) return;

    GM_xmlhttpRequest({
      method: "POST",
      url: WAN_ENDPOINT,
      headers: { "Content-Type": "application/json" },
      data: JSON.stringify({ lines }),
      onload: (res) => console.log("[TAM] WAN", res.status),
      onerror: (e) => console.warn("[TAM] WAN error", e),
    });
  }

  // ----------------------
  // Robust remove: delete first [V] ... first [/V] within the scope,
  // repeat until no more complete pairs or passes max.
  // This avoids "whole page" scanning and reduces cross-matching.
  // ----------------------
  function removeVBlocksRobust(root) {
    if (!root) return;

    const MAX_PASSES = 50;
    let removedAny = false;

    for (let pass = 0; pass < MAX_PASSES; pass++) {
      const walker = document.createTreeWalker(root, NodeFilter.SHOW_TEXT);

      const nodes = [];
      let n;
      while ((n = walker.nextNode())) nodes.push(n);

      // Find start "[V]"
      let startNode = null, startOffset = -1;
      for (let i = 0; i < nodes.length; i++) {
        const idx = nodes[i].nodeValue.indexOf("[V]");
        if (idx !== -1) {
          startNode = nodes[i];
          startOffset = idx;
          break;
        }
      }
      if (!startNode) break;

      // Find end "[/V]" after start
      const startIndex = nodes.indexOf(startNode);
      let endNode = null, endOffsetAfter = -1;

      for (let i = startIndex; i < nodes.length; i++) {
        const idx = nodes[i].nodeValue.indexOf("[/V]");
        if (idx !== -1) {
          endNode = nodes[i];
          endOffsetAfter = idx + "[/V]".length;
          break;
        }
      }

      // If end not found (streaming), stop and wait
      if (!endNode) break;

      const range = document.createRange();
      range.setStart(startNode, startOffset);
      range.setEnd(endNode, endOffsetAfter);
      range.deleteContents();

      removedAny = true;
    }

    if (removedAny) console.log("[TAM] removed V blocks");
  }

  // ----------------------
  // Scan: detect complete [V]...[/V] within the latest scope and send only new ones
  // ----------------------
  function scanAndSend() {
    const root = getScopeRoot();
    if (!root) return;

    const text = root.innerText || "";
    const vlines = extractV(text);

    const newLines = vlines.filter(v => !seen.has(v));
    newLines.forEach(v => seen.add(v));

    if (newLines.length) {
      console.log("[TAM] send", newLines.length);
      send(newLines);
    }
  }

  // Cache existing [V] at init so we don't resend history after reload
  function initSeen() {
    const root = getScopeRoot();
    if (!root) return;
    extractV(root.innerText || "").forEach(v => seen.add(v));
  }

  // ----------------------
  // UI toggle button (works for SPA by retrying until body exists)
  // ----------------------
  function createToggle() {
    const addButton = () => {
      if (!document.body) return;
      if (document.getElementById("tam-toggle")) return;

      const btn = document.createElement("div");
      btn.id = "tam-toggle";
      btn.textContent = "TAM: ON";

      Object.assign(btn.style, {
        position: "fixed",
        right: "16px",
        bottom: "16px",
        zIndex: 2147483647, // top-most
        background: "#111",
        color: "#0f0",
        padding: "6px 10px",
        fontSize: "12px",
        borderRadius: "10px",
        cursor: "pointer",
        fontFamily: "monospace",
        opacity: "0.85",
        userSelect: "none",
      });

      btn.addEventListener("click", () => {
        REMOVE_ENABLED = !REMOVE_ENABLED;
        btn.textContent = REMOVE_ENABLED ? "TAM: ON" : "TAM: SEND ONLY";
        btn.style.color = REMOVE_ENABLED ? "#0f0" : "#ff0";
      });

  // --- tooltip (hover) ---
const tip = document.createElement("div");
tip.textContent = "Hard reload: Ctrl + Shift + R";
Object.assign(tip.style, {
  position: "fixed",
  right: "16px",
  bottom: "48px",            // ボタンの少し上
  zIndex: 2147483647,
  background: "#111",
  color: "#fff",
  padding: "6px 10px",
  fontSize: "12px",
  borderRadius: "10px",
  fontFamily: "monospace",
  opacity: "0",
  pointerEvents: "none",
  transform: "translateY(4px)",
  transition: "opacity 120ms ease, transform 120ms ease",
});

document.body.appendChild(tip);

btn.addEventListener("mouseenter", () => {
  tip.style.opacity = "0.9";
  tip.style.transform = "translateY(0)";
});

btn.addEventListener("mouseleave", () => {
  tip.style.opacity = "0";
  tip.style.transform = "translateY(4px)";
});

      document.body.appendChild(btn);
      console.log("[TAM] toggle ready");
    };

    let tries = 0;
    const timer = setInterval(() => {
      tries++;
      addButton();
      if (document.getElementById("tam-toggle")) {
        clearInterval(timer);
        return;
      }
      if (tries > 60) clearInterval(timer); // ~12s
    }, 200);
  }

  // ----------------------
  // Boot
  // ----------------------
  initSeen();
  createToggle();

  // ----------------------
  // Observer strategy (stable):
  // - scan/send lightly (200ms)
  // - remove only after UI settles (700ms)
  // This reduces flicker and heavy DOM edits during streaming.
  // ----------------------
  let scanTimer = null;
  let removeTimer = null;

  const obsTarget = document.body || document.documentElement;

  new MutationObserver(() => {
    // detect & send
    if (scanTimer) clearTimeout(scanTimer);
    scanTimer = setTimeout(() => {
      scanTimer = null;
      scanAndSend();
    }, 200);

    // remove after settled (only if enabled)
    if (removeTimer) clearTimeout(removeTimer);
    removeTimer = setTimeout(() => {
      removeTimer = null;
      if (!REMOVE_ENABLED) return;

      const root = getScopeRoot();
      if (!root) return;

      // only remove if a complete pair exists in scope
      const t = root.innerText || "";
      if (VTAG_REGEX.test(t)) {
        removeVBlocksRobust(root);
      }
    }, 700);
  }).observe(obsTarget, { childList: true, subtree: true, characterData: true });

  console.log("[TAM] bridge ready");
})();

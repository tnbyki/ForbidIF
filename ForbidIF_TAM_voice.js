// ==UserScript==
// @name         ForbidIF Voice Bridge v4
// @match        https://chatgpt.com/*
// @match        https://chat.openai.com/*
// @match        https://gemini.google.com/*
// @match        https://grok.com/*
// @match        https://x.com/i/grok*
// @run-at       document-idle
// @grant        GM_xmlhttpRequest
// @connect      127.0.0.1
// @connect      localhost
// @noframes
// ==/UserScript==
(() => {
  "use strict";

  const guardId = "forbidif-voice-bridge-loaded";
  if (document.documentElement.hasAttribute(guardId)) {
    console.log("[TAM] duplicate load blocked");
    return;
  }
  console.log('[Voice] SCRIPT START');
  document.documentElement.setAttribute(guardId, "1");

  const WAN_ENDPOINT = "http://127.0.0.1:5000/voice_input";
  const VOICE_REGEX = /^🔊VOICE\|.*$/gm;

  let REMOVE_ENABLED = true;
  const seen = new Set();
const SEEN_KEY = "forbidif_voice_seen_v1";

  loadSeen();
　initSeenFromPage();

function loadSeen() {
  try {
    const arr = JSON.parse(sessionStorage.getItem(SEEN_KEY) || "[]");
    arr.forEach(v => seen.add(v));
  } catch (_) {}
}

function saveSeen() {
  try {
    sessionStorage.setItem(SEEN_KEY, JSON.stringify([...seen].slice(-200)));
  } catch (_) {}
}
function initSeenFromPage() {
  const root = getScopeRoot ? getScopeRoot() : document.body;
  if (!root) return;

  const text = root.innerText || "";
  const lines = extractVoice(text);

  lines.forEach(v => seen.add(v));
  saveSeen();
}
  function extractVoice(text) {
    const m = text.match(VOICE_REGEX);
    if (!m) return [];

    return m
      .map(v => v.trim())
      .filter(Boolean)
      .filter(v => {
        if (v.includes("speaker=名前")) return false;
        if (v.includes("id=VOICE_ID")) return false;
        if (v.includes("speed=speed")) return false;
        if (v.includes("volume=volume")) return false;
        if (v.includes("text=よみ")) return false;
        return true;
      });
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

  function getScopeRoot() {
    const host = location.hostname;

    if (host.includes("gemini.google.com")) {
      return document.body;
    }

    const articles = document.querySelectorAll("main article");
    if (articles && articles.length) {
      return articles[articles.length - 1];
    }

    return document.body;
  }

  function scan() {
    const root = getScopeRoot();
    if (!root) return;

    const text = root.innerText || "";
    const lines = extractVoice(text);

  const newLines = lines.filter(v => !seen.has(v));

  if (newLines.length) {
    setTimeout(() => {
      const root2 = getScopeRoot();
      if (!root2) return;

      const text2 = root2.innerText || "";
      const lines2 = extractVoice(text2);

      const confirmed = newLines.filter(v => lines2.includes(v) && !seen.has(v));
      confirmed.forEach(v => seen.add(v));

      if (confirmed.length) {
        saveSeen();
        send(confirmed);
      }
    }, 400);
  }
  }

  function removeVoiceLines(root) {
    if (!root) return;

    const walker = document.createTreeWalker(root, NodeFilter.SHOW_TEXT);
    const textNodes = [];
    let n;

    while ((n = walker.nextNode())) {
      textNodes.push(n);
    }

    for (const node of textNodes) {
      const original = node.nodeValue;
      const replaced = original.replace(/^🔊VOICE\|.*(?:\n|$)/gm, "");
      if (replaced !== original) {
        node.nodeValue = replaced;
      }
    }
  }

function createToggle() {
  const ensureButton = () => {
    if (!document.body) return false;

    let btn = document.getElementById("tam-toggle");
    if (!btn) {
      btn = document.createElement("button");
      btn.id = "tam-toggle";
      btn.textContent = REMOVE_ENABLED ? "TAM: ON" : "TAM: SEND ONLY";

      Object.assign(btn.style, {
        position: "fixed",
        right: "16px",
        bottom: "16px",
        zIndex: "2147483647",
        background: "#111",
        color: REMOVE_ENABLED ? "#0f0" : "#ff0",
        border: "1px solid #666",
        padding: "8px 12px",
        fontSize: "12px",
        borderRadius: "8px",
        cursor: "pointer",
        fontFamily: "monospace",
        opacity: "0.95",
        pointerEvents: "auto",
      });

      btn.addEventListener("click", () => {
        REMOVE_ENABLED = !REMOVE_ENABLED;
        btn.textContent = REMOVE_ENABLED ? "TAM: ON" : "TAM: SEND ONLY";
        btn.style.color = REMOVE_ENABLED ? "#0f0" : "#ff0";
      });

      document.body.appendChild(btn);
      console.log("[TAM] toggle ready");
    }

    return true;
  };

  let tries = 0;
  const timer = setInterval(() => {
    tries++;
    const ok = ensureButton();
    if (ok && document.getElementById("tam-toggle")) {
      if (tries > 5) clearInterval(timer);
    }
    if (tries > 150) clearInterval(timer);
  }, 500);
}

  createToggle();

  let scanTimer = null;
  let removeTimer = null;

  const obsTarget = document.body || document.documentElement;

  new MutationObserver(() => {
    if (scanTimer) clearTimeout(scanTimer);
    scanTimer = setTimeout(() => {
      scanTimer = null;
      scan();
    }, 200);

    if (removeTimer) clearTimeout(removeTimer);
    removeTimer = setTimeout(() => {
      removeTimer = null;
      if (!REMOVE_ENABLED) return;

      const root = getScopeRoot();
      if (!root) return;

      const t = root.innerText || "";
      if (VOICE_REGEX.test(t)) {
        removeVoiceLines(root);
      }
    }, 700);
  }).observe(obsTarget, { childList: true, subtree: true, characterData: true });

  console.log("[TAM] bridge ready");
})();

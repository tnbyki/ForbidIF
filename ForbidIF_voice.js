// ==UserScript==
// @name         VOICE Export + UI Clean (No Read on Reload)
// @match        https://chatgpt.com/*
// @match        https://chat.openai.com/*
// @match        https://grok.com/*
// @match        https://gemini.google.com/*
// @run-at       document-idle
// @grant        GM_xmlhttpRequest
// @connect      127.0.0.1
// @connect      localhost
// @connect      127.0.0.1:5000
// @connect      localhost:5000
// ==/UserScript==

(function () {
  "use strict";

  const PY_URL = "http://127.0.0.1:5000/voice_input";
  const LAST_N = 5;
  const COOLDOWN_MS = 800;

  // ★リロード後の「起動ガード」：この秒数の間は絶対送信しない
  const BOOT_GUARD_MS = 8000;

  const sent = new Set();
  let lastSendAt = 0;
  const bootAt = Date.now();
  let primed = false;

  function sendToPython(text) {
    if (!text) return;
    GM_xmlhttpRequest({
      method: "POST",
      url: PY_URL,
      headers: { "Content-Type": "application/json; charset=utf-8" },
      data: JSON.stringify({ line: text })
    });
  }

  function norm(s) {
    return (s || "")
      .replace(/[\u200B-\u200D\uFEFF]/g, "")
      .replace(/\r/g, "")
      .replace(/\u00A0/g, " ")
      .replace(/\u3000/g, " ")
      .replace(/[ \t]+/g, " ")
      .trim();
  }

  const VOICE_LINE_RE =
    /^🔴\s*([^(]+?)\s*\((\d+):([0-9]+(?:\.[0-9]+)?)\)\[VOICE\]([\s\S]*?)\[\/VOICE\]\s*$/;

  function isVoiceLine(s) {
    return VOICE_LINE_RE.test(norm(s));
  }

  function toUiLine(voiceLine) {
    const m = VOICE_LINE_RE.exec(norm(voiceLine));
    if (!m) return null;
    const name = norm(m[1]);
    const txt = norm(m[4]);
    return `🔴${name}：${txt}`;
  }

  function getTailRoot() {
    const articles = document.querySelectorAll("article");
    if (articles && articles.length) return articles[articles.length - 1];
    return document.body;
  }

  function collectVoiceLinesFromDOM(root) {
    const res = [];
    if (!root) return res;

    const nodes = root.querySelectorAll("p, li, div");
    for (const n of nodes) {
      if (n.classList && n.classList.contains("forbidif-voice-ui")) continue;

      const raw = norm(n.textContent || "");
      if (!raw.startsWith("🔴")) continue;
      if (!raw.includes("[VOICE]") || !raw.includes("[/VOICE]")) continue;

      if (isVoiceLine(raw)) res.push(raw);
    }
    return res;
  }

  function replaceVoiceLinesIn(root) {
    if (!root) return;

    const nodes = root.querySelectorAll("p, li, div");
    for (const n of nodes) {
      if (!n) continue;
      if (n.dataset && n.dataset.forbidifVoiceReplaced === "1") continue;

      const raw = norm(n.textContent || "");
      if (!isVoiceLine(raw)) continue;

      const ui = toUiLine(raw);
      if (!ui) continue;

      const next = n.nextElementSibling;
      if (next && next.classList && next.classList.contains("forbidif-voice-ui")) {
        n.dataset.forbidifVoiceReplaced = "1";
        continue;
      }

      n.style.display = "none";
      n.dataset.forbidifVoiceReplaced = "1";

      const uiEl = document.createElement("div");
      uiEl.className = "forbidif-voice-ui";
      uiEl.textContent = ui;
      uiEl.style.whiteSpace = "pre-wrap";
      uiEl.style.margin = "0.25em 0";

      n.insertAdjacentElement("afterend", uiEl);
    }
  }

  // ★起動ガード判定
  function inBootGuard() {
    return (Date.now() - bootAt) < BOOT_GUARD_MS;
  }

  function prime() {
    const tailRoot = getTailRoot();
    replaceVoiceLinesIn(tailRoot);

    const voiceLines = collectVoiceLinesFromDOM(tailRoot);
    // 「いま存在するもの」は全部“送信済み”にする
    for (const l of voiceLines) sent.add(norm(l));

    primed = true;
  }

  function scanAndSend() {
    const now = Date.now();
    if (now - lastSendAt < COOLDOWN_MS) return;

    const tailRoot = getTailRoot();
    replaceVoiceLinesIn(tailRoot);

    // 起動直後は送らない（全部読み上げ防止）
    if (inBootGuard()) return;

    // guardを抜けたら一度だけprimeして“過去分”を確実に潰す
    if (!primed) {
      prime();
      return;
    }

    const voiceLines = collectVoiceLinesFromDOM(tailRoot);
    const last = voiceLines.slice(Math.max(0, voiceLines.length - LAST_N));

    const fresh = [];
    for (const line of last) {
      const key = norm(line);
      if (sent.has(key)) continue;
      sent.add(key);
      fresh.push(line);
    }

    if (fresh.length) {
      lastSendAt = now;
      sendToPython(fresh.join("\n"));
    }
  }

  let timer = null;
  function schedule() {
    if (timer) clearTimeout(timer);
    timer = setTimeout(scanAndSend, 200);
  }

  const observer = new MutationObserver(() => schedule());
  observer.observe(document.body, { childList: true, subtree: true, characterData: true });

  document.addEventListener("visibilitychange", () => {
    if (!document.hidden) scanAndSend();
  });

  // 初回：UIだけ整形して、送信はしない（guard中）
  setTimeout(() => {
    try { replaceVoiceLinesIn(getTailRoot()); } catch {}
  }, 900);

})();

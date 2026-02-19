/* =========================================================
   ForbidIF
   Structure-first AI Narrative System

   Module : CORE / VOICE JS / VOICE PY / GAME PROMPT
   Version: v0.1 (Initial Release)
   Author : yuki (@tnbyki)
   ========================================================= */
/* =========================================================
   ForbidIF VOICE Exporter (Browser → Python)
   Real-time VOICE line detection
   ========================================================= */


// ==UserScript==
// @name         VOICE Export (Last 5 Only)
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
  const LAST_N = 5;              // ★最後N行だけ喋る（ここを変える）
  const COOLDOWN_MS = 800;       // 連打防止（UI更新の揺れ吸収）

  // 「送ったやつ」記録（ページリロードで消える＝過去ログは喋らない方針なのでOK）
  const sent = new Set();
  let lastSendAt = 0;

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
      .replace(/[\u200B-\u200D\uFEFF]/g, "") // ゼロ幅
      .replace(/\r/g, "")
      .replace(/\u00A0/g, " ")
      .replace(/\u3000/g, " ")
      .replace(/[ \t]+/g, " ")
      .trim();
  }

  function extractVoiceLines(text) {
    if (!text || !text.includes("[VOICE]")) return [];
    return text
      .split("\n")
      .map(norm)
      .filter(l =>
        l.includes("[VOICE]") &&
        l.includes("[/VOICE]") &&
        l.includes("(") &&
        l.includes(")")
      );
  }

  // 「最新メッセージっぽい領域」を探す（DOM差異に強い）
  function getTailText() {
    // 1) まず一番下付近のメッセージを狙う（あれば）
    const articles = document.querySelectorAll("article");
    if (articles && articles.length) {
      const tail = articles[articles.length - 1];
      const t = tail?.innerText || "";
      if (t.includes("[VOICE]")) return t;
    }

    // 2) ダメなら「画面末尾のテキストだけ」を使う（過去全体ではなく末尾）
    const whole = document.body?.innerText || "";
    const lines = whole.split("\n");
    // 末尾から200行だけ見る（ここは保険）
    return lines.slice(Math.max(0, lines.length - 200)).join("\n");
  }

  function scanAndSend() {
    const now = Date.now();
    if (now - lastSendAt < COOLDOWN_MS) return;

    const tailText = getTailText();
    const voiceLines = extractVoiceLines(tailText);

    // ★最後N行だけ
    const last = voiceLines.slice(Math.max(0, voiceLines.length - LAST_N));

    // 未送信だけ送る
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

  // 起動直後：過去を喋らないために「いま見えてる最後N行」を送信済みに登録しておく
  function prime() {
    const tailText = getTailText();
    const voiceLines = extractVoiceLines(tailText);
    const last = voiceLines.slice(Math.max(0, voiceLines.length - LAST_N));
    for (const l of last) sent.add(norm(l));
  }

  // 監視（更新があったら末尾だけスキャン）
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

  setTimeout(prime, 900);
})();

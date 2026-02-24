// ==UserScript==
// @name         ForbidIF Gemini -> WAN Voice Bridge (Vtag DEBUG)
// @namespace    https://tnbyki.example/forbidif
// @version      1.0.1
// @description  Detect [V]...[/V] and send to WAN (with debug log)
// @match        https://gemini.google.com/*
// @run-at       document-idle
// @grant        GM_xmlhttpRequest
// @connect      127.0.0.1
// @connect      localhost
// ==/UserScript==

(() => {
  "use strict";

  const WAN_ENDPOINT = "http://127.0.0.1:5000/voice_input";
  const VTAG_REGEX = /\[V\][\s\S]*?\[\/V\]/g;

  const seen = new Set();

  const log = (...a) => console.log("[TAM]", ...a);
  const warn = (...a) => console.warn("[TAM]", ...a);

  function extractV(text) {
    const m = text.match(VTAG_REGEX);
    return m ? m.map(v => v.trim()).filter(Boolean) : [];
  }

  function send(lines) {
    if (!lines.length) return;

    // 🔽 ここが今回のメイン追加
    console.log("========== [TAM → WAN] SEND ==========");
    console.log("lines:");
    lines.forEach((l, i) => console.log(`${i}:`, l));

    const payload = { lines };

    console.log("payload:", payload);
    console.log("=====================================");

    GM_xmlhttpRequest({
      method: "POST",
      url: WAN_ENDPOINT,
      headers: { "Content-Type": "application/json" },
      data: JSON.stringify(payload),
      onload: res => log("WAN response:", res.status),
      onerror: e => warn("WAN error:", e)
    });
  }

  function scan() {
    const text = document.body.innerText;
    const vlines = extractV(text);

    const newLines = vlines.filter(l => !seen.has(l));
    newLines.forEach(l => seen.add(l));

    if (newLines.length) {
      log("detected:", newLines.length);
      send(newLines);
    }
  }

  new MutationObserver(scan).observe(document.body, {
    childList: true,
    subtree: true,
    characterData: true
  });

  log("bridge ready");
})();

// ==UserScript==
// @name         ForbidIF Gemini -> WAN Voice Bridge (Vtag DEBUG + FULL REMOVE)
// @namespace    https://tnbyki.example/forbidif
// @version      1.2.0
// @description  Detect [V]...[/V], send to WAN, and fully remove from browser
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

  // 🔥 [V]...[/V] を丸ごと削除
  function cleanVTags() {
    const walker = document.createTreeWalker(
      document.body,
      NodeFilter.SHOW_TEXT,
      null,
      false
    );

    let node;
    while ((node = walker.nextNode())) {
      if (node.nodeValue.includes("[V]")) {
        node.nodeValue = node.nodeValue.replace(VTAG_REGEX, "");
      }
    }
  }

  function scan() {
    const text = document.body.innerText;
    const vlines = extractV(text);

    const newLines = vlines.filter(l => !seen.has(l));
    newLines.forEach(l => seen.add(l));

    if (newLines.length) {
      log("detected:", newLines.length);
      send(newLines);

      // 👇 表示から完全削除
      cleanVTags();
    }
  }

  new MutationObserver(scan).observe(document.body, {
    childList: true,
    subtree: true,
    characterData: true
  });

  log("bridge ready");
})();

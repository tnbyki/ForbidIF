// ==UserScript==
// @name         ForbidIF Gemini -> WAN Voice Bridge (Vtag DEBUG + ROBUST REMOVE)
// @namespace    https://tnbyki.example/forbidif
// @version      1.4.0
// @description  Detect new [V]...[/V], send to WAN, and robustly remove from browser (across nodes)
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

  // ✅ テキストノードを跨いでも [V]...[/V] を丸ごと削除する
  function removeVBlocksRobust() {
    // 無限ループ防止
    const MAX_PASSES = 50;
    let pass = 0;
    let removedAny = false;

    while (pass++ < MAX_PASSES) {
      const walker = document.createTreeWalker(
        document.body,
        NodeFilter.SHOW_TEXT,
        null,
        false
      );

      // 全テキストノードを順序通りに集める
      const nodes = [];
      let n;
      while ((n = walker.nextNode())) nodes.push(n);

      // [V] の開始を探す
      let startNode = null, startOffset = -1;

      for (let i = 0; i < nodes.length; i++) {
        const idx = nodes[i].nodeValue.indexOf("[V]");
        if (idx !== -1) {
          startNode = nodes[i];
          startOffset = idx;
          break;
        }
      }

      // もう無ければ終わり
      if (!startNode) break;

      // [/V] の終了を startNode以降で探す
      let endNode = null, endOffsetAfter = -1;
      let foundEnd = false;

      const startIndex = nodes.indexOf(startNode);
      for (let i = startIndex; i < nodes.length; i++) {
        const idx = nodes[i].nodeValue.indexOf("[/V]");
        if (idx !== -1) {
          endNode = nodes[i];
          endOffsetAfter = idx + "[/V]".length; // 終了タグまで含めて消す
          foundEnd = true;
          break;
        }
      }

      // 終了タグがまだ来てない(途中)なら何もしない（次の更新で消える）
      if (!foundEnd) break;

      // レンジで削除
      const range = document.createRange();
      range.setStart(startNode, startOffset);
      range.setEnd(endNode, endOffsetAfter);

      range.deleteContents();
      removedAny = true;
    }

    if (removedAny) log("V blocks removed from DOM");
  }

  function scan() {
    // ① 検出（送信用）
    const text = document.body.innerText;
    const vlines = extractV(text);

    const newLines = vlines.filter(l => !seen.has(l));
    newLines.forEach(l => seen.add(l));

    if (newLines.length) {
      log("detected:", newLines.length);
      send(newLines);
    }

    // ② 表示から削除（送信とは独立に毎回やる）
    removeVBlocksRobust();
  }

  // 🔥 起動時：過去ログを既読化＋表示からも一掃
  function init() {
    const text = document.body.innerText;
    extractV(text).forEach(v => seen.add(v));
    log("history cached:", seen.size);

    // リロード直後も消したいので
    removeVBlocksRobust();
  }

  init();

  new MutationObserver(() => {
    // Geminiは更新が細かいので、軽くまとめる
    queueMicrotask(scan);
  }).observe(document.body, {
    childList: true,
    subtree: true,
    characterData: true
  });

  log("bridge ready");
})();

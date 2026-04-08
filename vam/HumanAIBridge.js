
Tampermonkey® by Jan Biniok
v5.4.1
	
TAM Forwarder (Silent Eraser)
1
// ==UserScript==
2
// @name         TAM Forwarder (Silent Eraser)
3
// @namespace    http://tampermonkey.net/
4
// @version      2026-04-05_Final_v6
5
// @match        https://gemini.google.com/app/*
6
// @grant        GM_xmlhttpRequest
7
// @connect      127.0.0.1
8
// @connect      localhost
9
// ==/UserScript==
10
​
11
(function() {
12
    'use strict';
13
    const BRIDGE_URL = "http://localhost:8080/";
14
    const sentCache = new Set();
15
​
16
    const observer = new MutationObserver((mutations) => {
17
        for (const mutation of mutations) {
18
            // 文字の変化や要素の追加からテキストを抽出
19
            const target = mutation.target;
20
            let text = (target.textContent || "").trim();
21
            if (!text) continue;
22
​
23
            const lines = text.split(/\r?\n/);
24
            lines.forEach(line => {
25
                const l = line.trim();
26
​
27
                // ★ 仕様：VOICE/POSEで始まり、かつ末尾が「|#」
28
                if ((l.startsWith("🔊VOICE|") || l.startsWith("💽POSE|")) && l.endsWith("|#")) {
Tam - サービス ボット

Tampermonkey のコンソール出力を見つける際にお手伝いが必要ですか?
無効にする
Tampermonkey Bot
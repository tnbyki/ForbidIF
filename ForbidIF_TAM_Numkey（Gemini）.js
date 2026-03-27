// ==UserScript==
// @name         Gemini Number Bar Stable
// @namespace    http://tampermonkey.net/
// @version      2.2
// @match        https://gemini.google.com/*
// @grant        none
// @noframes
// ==/UserScript==

(function () {
  'use strict';

  const INPUT_SELECTOR = '[contenteditable="true"]';

  function findInput() {
    return document.querySelector(INPUT_SELECTOR);
  }

  function insertText(t) {
    const el = findInput();
    if (!el) return;

    el.focus();
    document.execCommand('insertText', false, t);
    el.dispatchEvent(new Event('input', { bubbles: true }));
  }

  function pressEnter() {
    const el = findInput();
    if (!el) return;

    el.dispatchEvent(new KeyboardEvent('keydown', {
      key: 'Enter',
      code: 'Enter',
      bubbles: true
    }));
  }

  function send(d) {
    insertText(d);
    setTimeout(pressEnter, 120);
  }

  function createBar() {
    if (document.getElementById('gemini-bar')) return;

    const bar = document.createElement('div');
    bar.id = 'gemini-bar';

    Object.assign(bar.style, {
      position: 'fixed',
      bottom: '20px',
      right: '180px',
      display: 'flex',
      gap: '6px',
      padding: '6px',
      background: 'rgba(0,0,0,0.85)',
      borderRadius: '8px',
      zIndex: '2147483646',
      fontFamily: 'monospace'
    });

    const nums = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9'];

    nums.forEach(n => {
      const btn = document.createElement('button');
      btn.textContent = n;

      Object.assign(btn.style, {
        width: '28px',
        height: '28px',
        border: '1px solid #666',
        background: '#111',
        color: '#fff',
        borderRadius: '6px',
        cursor: 'pointer',
        fontSize: '13px'
      });

      btn.onclick = () => send(n);
      bar.appendChild(btn);
    });

    document.body.appendChild(bar);
  }

  function boot() {
    createBar();
  }

  boot();

  const obs = new MutationObserver(() => {
    createBar();
  });

  obs.observe(document.body, {
    childList: true,
    subtree: true
  });
})();

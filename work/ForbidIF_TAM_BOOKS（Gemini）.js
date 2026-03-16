
// ==UserScript==
// @name         Gemini Select Inputer Grid
// @namespace    http://tampermonkey.net/
// @version      1.2
// @description  右端中央に展開式のマクロ入力パネルを表示
// @match        https://gemini.google.com/*
// @grant        none
// ==/UserScript==

(function () {
    'use strict';

    const PANEL_ID = 'gemini-select-inputer-grid';
    const INPUT_SELECTOR = '[contenteditable="true"]';

    // =========================================================
    // ここを編集
    // LABELS = ボタンに表示する文字
    // TEXTS  = 実際に入力する文字
    // MODES  = "send" なら入力して送信 / "insert" なら入力のみ
    // A～E は insert
    // Z は send
    // =========================================================

    const LABELS = [
        ['🗣️', '👩‍🦰', '👄', '📣↵', '💎'],
        ['👈', '👚', '👉', '📢↵', '🐼'],
        ['🍮', '🧍', '🍓', 'D3', '🐯'],
        ['🍑', '🌸', '＊', '🧴', '🐷'],
        ['💉','🥕','🥕', '🥖', '🐮'],
        ['A6', '👠','👠','🗑️','🗨️↵'],
        ['🐢遅', '🦔普','🐑早', '🐐激', '🐖半分']
    ];

    const TEXTS = [
        ['口を ',   '顔を ',   'くちびるを', '音声スピードを普通にして', '美しい'],
        ['彼女の右手 ',  '体を ',     '彼女の左手 ', '音声IDを変更して', '可愛い'],
        ['胸を ', '腰を ',     '乳首を ', 'D3', 'かっこいい'],
        ['尻を',  '秘部を ',      '尻穴を ', '尿道', '豚'],
        ['注射器で液体を',  '彼女の右ももを ',      '彼女の左ももを ', '男性器', '牛'],
        ['A6', '彼女の右足を ',     '彼女の左足を ', '汚い', ],
        ['ゆっくり ', 'ふつうに ','はやく ', '激しく ', '半分 ']
    ];

    const MODES = [
        ['insert', 'insert', 'insert', 'send', 'insert'],
        ['insert', 'insert', 'insert', 'send', 'insert'],
        ['insert', 'insert', 'insert', 'insert', 'insert'],
        ['insert', 'insert', 'insert', 'insert', 'insert'],
        ['insert', 'insert', 'insert', 'insert', 'insert'],
        ['insert', 'insert', 'insert', 'insert', 'send'],
        ['insert', 'insert', 'insert', 'insert', 'insert']
    ];

    const Z_LABELS = [
        ['👆押',  '👅舐',  '👐揉',  '💋接', '👏触','🤏摘','🤌弾'],
        ['👣持上',  '⚠️押倒',  '🧍腰上',  '🦵足広',  '👀見','🔍調'],
        ['👩‍🦰前', '👤後', '🧎座', '🧍立','👐手前','🦵足広'],
        ['🗨️話', '⚕️コド', '⚕️✖', '💧', '💩'],
        ['👚🔓服', '👙🔓ブラ', '🔓👙パンツ', '🔐👙半パンツ'],
        ['👨‍🦰↓↘👩‍🦰🦀駅弁⇕',   '⇕👨‍↓↖👩‍🦰🦵立松','⇕👨‍🦰↓↙👤🦵吊立'],
        ['👨‍🦰↓↓👩‍🦰👫対立⇕',  '⇕👨‍🦰↓↖👩‍🦰🦵膝松','⇕👨‍🦰↓↓👤🐕背立'],
        ['👨‍🦰↙←👩‍🦰🦀正常⇔',  '👨‍↓←👩‍🦰🦵松葉','⇔👨‍🦰↓↙👤🐕背犬'],
        ['👨‍🦰↓↙👩‍🪑正座⇕',  '⇕👨‍🦰→↘👩‍🦰🐎騎乗','⇕👨‍🦰↓↘👤🪑背座'],
        ['👨‍🦰⇇👩‍🦰🦀側位⇕',   '⇕👨‍🦰→↙👩‍🦰🐎背騎',  '⇕👨‍🦰⇉👤🦀側位'],
        ['🌭✖','🎯', '3', '50','80','100','120','💦'],
        ['🔐', '🔓', '🧍🩺','👤📖', '💗📖']
    ];

    const Z_TEXTS = [
        ['指で押さえる','舐める','揉む','キスをする','触る','摘まむ','弾く'],
        ['持ち上げる',  '押し倒す',  '腰を浮かせる',  '足を広げる',  '見る','調べる'],
        ['彼女を前に向かせる', '彼女を後ろに向かせる', '座らせる', '立たせる', '手を前につけさせる', '足を広げさせる'],
        ['と話す', 'コンドームをつける', 'コンドームを外す', '聖水', 'うんこ'],
        ['服を脱がせる', 'ブラを取る', 'パンツを取る', 'パンツをずらす'],
        ['体位を駅弁にする', '体位を立ち松葉にする', '体位を吊りバックにする'],
        ['体位を対面立位にする', '体位を膝たち松葉にする', '体位を立位にする'],
        ['体位を正常位にする', '体位を松葉崩しにする', '体位をバックにする'],
        ['体位を正面座位しにする', '体位を騎乗位にする', '体位を座位にする'],
        ['体位を測位しにする', '体位を背騎乗しにする', '体位をバック側位にする'],
        ['男性器を抜く','に男性器をあてる', '3%挿入', '50%挿入','80%挿入','100%挿入', '120%挿入','射精する'],
        ['ロックする', 'ロックを解除する','現在の体位と体の状態を詳細に説明してください。', '彼女のPROFILEと彼女の経験数を説明して', 'もっと官能的に秘部、乳首、体の状況を説明して']
    ];

    const Z_MODES = [
        ['send', 'send', 'send', 'send', 'send', 'send'],
        ['send', 'send', 'send', 'send', 'send'],
        ['send', 'send', 'send', 'send', 'send','send'],
        ['send', 'send', 'send', 'send', 'send'],
        ['send', 'send', 'send'],
        ['send', 'send', 'send'],
        ['send', 'send', 'send'],
        ['send', 'send', 'send'],
        ['send', 'send', 'send'],
        ['send', 'send', 'send','send', 'send','send', 'send', 'send'],
        ['send', 'send', 'send', 'send', 'send']
    ];

    const HEADER_LEFT = 'SELECT';
    const HEADER_RIGHT = 'INPUTER';
    const TOGGLE_LABEL = 'ON';

    // =========================================================

    function findInput() {
        const list = Array.from(document.querySelectorAll(INPUT_SELECTOR));
        return list.find(el => {
            const s = window.getComputedStyle(el);
            return s.display !== 'none' && s.visibility !== 'hidden';
        }) || null;
    }

    function focusToEnd(el) {
        if (!el) return;

        el.focus();

        const range = document.createRange();
        range.selectNodeContents(el);
        range.collapse(false);

        const sel = window.getSelection();
        if (!sel) return;

        sel.removeAllRanges();
        sel.addRange(range);
    }

    function dispatchInput(el, inputType = 'insertText', data = '') {
        if (!el) return;

        try {
            el.dispatchEvent(new InputEvent('input', {
                bubbles: true,
                inputType,
                data
            }));
        } catch (e) {
            el.dispatchEvent(new Event('input', { bubbles: true }));
        }
    }

    function insertText(text) {
        const el = findInput();
        if (!el) return false;

        focusToEnd(el);

        const ok = document.execCommand('insertText', false, text);

        if (!ok) {
            const sel = window.getSelection();
            if (!sel || sel.rangeCount === 0) return false;

            const range = sel.getRangeAt(0);
            range.deleteContents();
            range.insertNode(document.createTextNode(text));
            range.collapse(false);
            sel.removeAllRanges();
            sel.addRange(range);
        }

        dispatchInput(el, 'insertText', text);
        return true;
    }

    function clearInput() {
        const el = findInput();
        if (!el) return false;

        el.innerHTML = '';
        el.textContent = '';
        focusToEnd(el);
        dispatchInput(el, 'deleteContentBackward', '');
        return true;
    }

    function pressEnter() {
        const el = findInput();
        if (!el) return false;

        focusToEnd(el);

        ['keydown', 'keypress', 'keyup'].forEach(type => {
            el.dispatchEvent(new KeyboardEvent(type, {
                key: 'Enter',
                code: 'Enter',
                keyCode: 13,
                which: 13,
                bubbles: true,
                cancelable: true
            }));
        });

        return true;
    }

    function sendText(text) {
        const ok = insertText(text);
        if (!ok) return;

        setTimeout(() => {
            pressEnter();
        }, 120);
    }

    function runAction(text, mode) {
        if (mode === 'insert') {
            insertText(text || '');
        } else {
            sendText(text || '');
        }
    }

    function makeButton(label, onClick, options = {}) {
        const btn = document.createElement('button');
        btn.type = 'button';
        btn.textContent = label;

        btn.style.minWidth = (options.width || 46) + 'px';
        btn.style.height = (options.height || 28) + 'px';
        btn.style.padding = '0 8px';
        btn.style.fontSize = '13px';
        btn.style.border = '1px solid #444';
        btn.style.borderRadius = '6px';
        btn.style.background = '#111';
        btn.style.color = '#fff';
        btn.style.cursor = 'pointer';
        btn.style.whiteSpace = 'nowrap';
        btn.style.lineHeight = '1';
        btn.style.userSelect = 'none';

        btn.addEventListener('mouseenter', () => {
            btn.style.background = '#222';
        });

        btn.addEventListener('mouseleave', () => {
            btn.style.background = '#111';
        });

        btn.addEventListener('click', (e) => {
            e.preventDefault();
            e.stopPropagation();
            onClick();
        });

        return btn;
    }

    function makeRow() {
        const row = document.createElement('div');
        row.style.display = 'flex';
        row.style.flexWrap = 'nowrap';
        row.style.gap = '6px';
        row.style.alignItems = 'center';
        return row;
    }

    function buildHeader(expandedBox) {
        const row = makeRow();

        const left = document.createElement('span');
        left.textContent = HEADER_LEFT;
        left.style.fontSize = '13px';
        left.style.fontWeight = '700';
        left.style.color = '#fff';

        const right = document.createElement('span');
        right.textContent = HEADER_RIGHT;
        right.style.fontSize = '13px';
        right.style.fontWeight = '700';
        right.style.color = '#fff';

        const toggle = makeButton(TOGGLE_LABEL, () => {
            const isOpen = expandedBox.style.display !== 'none';
            expandedBox.style.display = isOpen ? 'none' : 'flex';
        }, { width: 46, height: 28 });

        row.appendChild(left);
        row.appendChild(right);
        row.appendChild(toggle);

        return row;
    }

    function buildExpanded() {
        const box = document.createElement('div');
        box.style.display = 'none';
        box.style.flexDirection = 'column';
        box.style.gap = '6px';
        box.style.marginTop = '8px';

        for (let r = 0; r < LABELS.length; r++) {
            const row = makeRow();

            for (let c = 0; c < LABELS[r].length; c++) {
                const label = LABELS[r][c] ?? '';
                const text = TEXTS[r]?.[c] ?? label;
                const mode = MODES[r]?.[c] ?? 'insert';

                row.appendChild(
                    makeButton(label, () => runAction(text, mode))
                );
            }

            box.appendChild(row);
        }

        const spacer = document.createElement('div');
        spacer.style.height = '8px';
        box.appendChild(spacer);

        for (let r = 0; r < Z_LABELS.length; r++) {
            const row = makeRow();

            for (let c = 0; c < Z_LABELS[r].length; c++) {
                const label = Z_LABELS[r][c] ?? '';
                const text = Z_TEXTS[r]?.[c] ?? label;
                const mode = Z_MODES[r]?.[c] ?? 'send';

                row.appendChild(
                    makeButton(label, () => runAction(text, mode))
                );
            }

            box.appendChild(row);
        }

        const spacer2 = document.createElement('div');
        spacer2.style.height = '8px';
        box.appendChild(spacer2);

        const controlRow = makeRow();
        controlRow.appendChild(
            makeButton('CLEAR', () => clearInput(), { width: 70 })
        );
        controlRow.appendChild(
            makeButton('ENTER', () => pressEnter(), { width: 70 })
        );
        box.appendChild(controlRow);

        return box;
    }

    function createPanel() {
        if (document.getElementById(PANEL_ID)) return;

        const panel = document.createElement('div');
        panel.id = PANEL_ID;

        panel.style.position = 'fixed';
        panel.style.top = '50%';
        panel.style.right = '18px';
        panel.style.transform = 'translateY(-50%)';
        panel.style.zIndex = '999998';
        panel.style.background = 'rgba(0,0,0,0.82)';
        panel.style.border = '1px solid #333';
        panel.style.borderRadius = '10px';
        panel.style.padding = '8px';
        panel.style.backdropFilter = 'blur(4px)';
        panel.style.boxShadow = '0 4px 12px rgba(0,0,0,0.25)';
        panel.style.display = 'flex';
        panel.style.flexDirection = 'column';
        panel.style.alignItems = 'flex-start';
        panel.style.maxWidth = 'calc(100vw - 40px)';
        panel.style.overflowX = 'auto';

        const expandedBox = buildExpanded();
        const header = buildHeader(expandedBox);

        panel.appendChild(header);
        panel.appendChild(expandedBox);

        document.body.appendChild(panel);
    }

    function boot() {
        createPanel();
    }

    window.addEventListener('load', boot);
    setTimeout(boot, 800);
    setTimeout(boot, 2000);

    const observer = new MutationObserver(() => {
        if (!document.getElementById(PANEL_ID)) {
            createPanel();
        }
    });

    observer.observe(document.documentElement, {
        childList: true,
        subtree: true
    });
})();

// ==UserScript==
// @name         Gemini Number Bar Compact
// @namespace    http://tampermonkey.net/
// @version      1.2
// @match        https://gemini.google.com/*
// @grant        none
// ==/UserScript==

(function () {
'use strict';

const INPUT_SELECTOR='[contenteditable="true"]';

function findInput(){
    return document.querySelector(INPUT_SELECTOR);
}

function insertDigit(d){

    const el=findInput();
    if(!el) return;

    el.focus();

    document.execCommand('insertText',false,d);

    el.dispatchEvent(new Event('input',{bubbles:true}));

}

function pressEnter(){

    const el=findInput();
    if(!el) return;

    el.dispatchEvent(new KeyboardEvent('keydown',{
        key:'Enter',
        code:'Enter',
        bubbles:true
    }));
}

function send(d){

    insertDigit(d);

    setTimeout(pressEnter,150);

}

function createBar(){

    if(document.getElementById("gemini-bar")) return;

    const bar=document.createElement("div");

    bar.id="gemini-bar";

    bar.style.position="fixed";
    bar.style.bottom="20px";
    bar.style.right="110px";
    bar.style.display="flex";
    bar.style.gap="6px";
    bar.style.padding="6px";
    bar.style.background="rgba(0,0,0,0.8)";
    bar.style.borderRadius="8px";
    bar.style.zIndex="999999";

    const nums=['0','1','2','3','4','5','6','7','8','9'];

    nums.forEach(n=>{

        const btn=document.createElement("button");

        btn.textContent=n;

        btn.style.width="40px";
        btn.style.height="24px";
        btn.style.fontSize="14px";
        btn.style.borderRadius="6px";
        btn.style.border="1px solid #444";
        btn.style.background="#111";
        btn.style.color="#fff";
        btn.style.cursor="pointer";

        btn.onclick=()=>send(n);

        bar.appendChild(btn);

    });

    document.body.appendChild(bar);

}

window.addEventListener("load",createBar);
setTimeout(createBar,1000);

})();

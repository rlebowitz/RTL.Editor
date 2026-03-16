const editor = document.getElementById('editor');
const rev = document.getElementById('rev');

function getInnerHTML() {
    return editor ? editor.innerHTML : '';
}

function setDir(dir) {
    editor.style.direction = dir;
    editor.style.textAlign = (dir === 'rtl') ? 'right' : 'left';
}

function notifyChange() {
    const msg = {
        type: 'contentChanged',
        html: editor.innerHTML
    };
    if (window.chrome && window.chrome.webview) {
        window.chrome.webview.postMessage(msg);
    }
}

function modifyClipboardText() {
    try {
        window.chrome.webview.postMessage({ type: 'getClipboardText' });
        //             await navigator.clipboard.writeText(result);
    } catch (err) {
        console.error('Failed to transmit message: ', err);
    }
}

editor.addEventListener('input', notifyChange);
rev.addEventListener('click', modifyClipboardText);


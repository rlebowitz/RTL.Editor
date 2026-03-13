using Microsoft.Web.WebView2.Core;
using System.Text.Json;

namespace RTL.Editor.App
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private async void MainForm_Load(object? sender, EventArgs e)
        {
            await webView.EnsureCoreWebView2Async();

            // Optional: listen for messages from JS
            webView.CoreWebView2.WebMessageReceived += CoreWebView2_WebMessageReceived;

            webView.CoreWebView2.NavigateToString(GetEditorHtml());
        }

        private async void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using var dlg = new OpenFileDialog();
            dlg.Filter = "HTML files (*.html;*.htm)|*.html;*.htm|Text files (*.txt)|*.txt|All files (*.*)|*.*";

            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            string content = System.IO.File.ReadAllText(dlg.FileName);

            // If it's plain text, HTML-encode it
            if (dlg.FileName.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
                content = System.Net.WebUtility.HtmlEncode(content).Replace("\n", "<br>");

            string script = $@"
        (function() {{
            const editor = document.getElementById('editor');
            if (editor) editor.innerHTML = `{EscapeForJs(content)}`;
        }})();";

            await webView.CoreWebView2.ExecuteScriptAsync(script);
        }

        private async void saveFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using var dlg = new SaveFileDialog();
            dlg.Filter = "HTML files (*.html)|*.html|Text files (*.txt)|*.txt";
            dlg.FileName = "document.html";

            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            string script = @"
        (function() {
            const editor = document.getElementById('editor');
            return editor ? editor.innerHTML : '';
        })();";

            string result = await webView.CoreWebView2.ExecuteScriptAsync(script);
            string html = System.Text.Json.JsonSerializer.Deserialize<string>(result) ?? "";

            if (dlg.FileName.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
            {
                // Convert HTML → plain text
                string plain = System.Text.RegularExpressions.Regex.Replace(html, "<.*?>", "");
                System.IO.File.WriteAllText(dlg.FileName, plain);
            }
            else
            {
                System.IO.File.WriteAllText(dlg.FileName, html);
            }
        }

        private void CoreWebView2_WebMessageReceived(object? sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
            try
            {
                var json = e.WebMessageAsJson;
                var msg = JsonSerializer.Deserialize<WebMessage>(json);
                if (msg?.type == "contentChanged")
                {
                    // For now, just write to debug output
                    System.Diagnostics.Debug.WriteLine("Content changed: " + msg.html);
                }
            }
            catch
            {
                // ignore
            }
        }

        private record WebMessage(string type, string html);

        private string GetEditorHtml()
        {
            // Minimal RTL contentEditable editor with basic toolbar
            return @"
<!DOCTYPE html>
<html lang=""he"">
<head>
<meta charset=""UTF-8"">
<title>RTL Editor</title>
<style>
    body {
        margin: 0;
        font-family: 'Segoe UI', sans-serif;
        background: #f0f0f0;
    }
    #toolbar {
        background: #333;
        color: white;
        padding: 6px;
        display: flex;
        gap: 8px;
        align-items: center;
        direction: ltr; /* toolbar itself LTR */
    }
    #toolbar button {
        padding: 4px 8px;
        border: none;
        background: #555;
        color: white;
        cursor: pointer;
    }
    #toolbar button:hover {
        background: #777;
    }
    #editor {
        padding: 12px;
        height: calc(100vh - 40px);
        box-sizing: border-box;
        background: white;
        direction: rtl;
        unicode-bidi: plaintext;
        text-align: right;
        font-size: 18px;
        line-height: 1.6;
    }
    #editor:focus {
        outline: none;
    }
</style>
</head>
<body>
<div id=""toolbar"">
    <button onclick=""document.execCommand('bold')""><b>B</b></button>
    <button onclick=""document.execCommand('italic')""><i>I</i></button>
    <button onclick=""document.execCommand('underline')""><u>U</u></button>
    <button onclick=""setDir('rtl')"">RTL</button>
    <button onclick=""setDir('ltr')"">LTR</button>
</div>
<div id=""editor"" contenteditable=""true"">
    שלום עולם! זהו עורך טקסט RTL בתוך WebView2.
</div>

<script>
    const editor = document.getElementById('editor');

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

    editor.addEventListener('input', notifyChange);
</script>
</body>
</html>";
        }

        private async void loadSampleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (webView.CoreWebView2 == null) return;

            string sample = "שלום, זה טקסט לדוגמה עם עברית ו English ביחד.";
            string script = $@"
                (function() {{
                    const editor = document.getElementById('editor');
                    if (editor) editor.innerHTML = '{EscapeForJs(sample)}';
                }})();";

            await webView.CoreWebView2.ExecuteScriptAsync(script);
        }

        private async void getHtmlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (webView.CoreWebView2 == null) return;

            string script = @"
                (function() {
                    const editor = document.getElementById('editor');
                    return editor ? editor.innerHTML : '';
                })();";

            string result = await webView.CoreWebView2.ExecuteScriptAsync(script);

            // result is JSON-encoded string
            string html = JsonSerializer.Deserialize<string>(result) ?? string.Empty;

            MessageBox.Show(this, html, "Editor HTML", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private static string EscapeForJs(string text)
        {
            return text
                .Replace("\\", "\\\\")
                .Replace("'", "\\'")
                .Replace("\r", "")
                .Replace("\n", "\\n");
        }

    }
}

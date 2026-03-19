using Microsoft.Web.WebView2.Core;
using System.IO;
using System.Text.Json;


namespace RTL.Editor.App
{
    public partial class MainForm : Form
    {
        private JsEventListener? _listener;
        public MainForm()
        {
            InitializeComponent();
        }

        private async void MainForm_Load(object? sender, EventArgs e)
        {
            await webView.EnsureCoreWebView2Async();

            // Map the virtual host name to the local folder
            string hostName = "app.local";
            string appExecutablePath = AppContext.BaseDirectory; // Gets the directory where the app assembly is located
            string relativeFolderPath = "Assets"; // The relative path within the output directory
            string absoluteFolderPath = Path.Combine(appExecutablePath, relativeFolderPath);
            Directory.CreateDirectory(absoluteFolderPath); // Ensure the folder exists

            webView.CoreWebView2.SetVirtualHostNameToFolderMapping(
                hostName,
                absoluteFolderPath,
                CoreWebView2HostResourceAccessKind.Allow
            );
            // Optional: listen for messages from JS
            _listener = new JsEventListener(webView.CoreWebView2);
            webView.CoreWebView2.Navigate($"https://{hostName}/editor.html");
        }

        private void OpenFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using var dlg = new OpenFileDialog();
            dlg.Filter = "HTML files (*.html;*.htm)|*.html;*.htm|Text files (*.txt)|*.txt|All files (*.*)|*.*";

            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            var content = File.ReadAllText(dlg.FileName);

            // If it's plain text, HTML-encode it
            if (dlg.FileName.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
            {
                content = System.Net.WebUtility.HtmlEncode(content)
                .Replace("\\", "\\\\")
                .Replace("'", "\\'")
                .Replace("\r", "")
                .Replace("\n", "<br>");
            }
            var message = JsonSerializer.Serialize<WebMessage>(new WebMessage("textfile", content));
            webView.CoreWebView2.PostWebMessageAsJson(message);
            //    string script = $@"
            //(function() {{
            //    const editor = document.getElementById('editor');
            //    if (editor) editor.innerHTML = `{EscapeForJs(content)}`;
            //}})();";
            //    await webView.CoreWebView2.ExecuteScriptAsync(script);
        }

        private async void SaveFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using var dlg = new SaveFileDialog();
            dlg.Filter = "HTML files (*.html)|*.html|Text files (*.txt)|*.txt";
            dlg.FileName = "document.html";

            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            string result = await webView.CoreWebView2.ExecuteScriptAsync("getInnerHTML");
            string html = JsonSerializer.Deserialize<string>(result) ?? "";

            if (dlg.FileName.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
            {
                // Convert HTML → plain text
                string plain = System.Text.RegularExpressions.Regex.Replace(html, "<.*?>", "");
                File.WriteAllText(dlg.FileName, plain);
            }
            else
            {
                File.WriteAllText(dlg.FileName, html);
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
                    System.Diagnostics.Debug.WriteLine("Content changed: " + msg.text);
                }
            }
            catch
            {
                // ignore
            }
        }

        private record WebMessage(string type, string text);

        //private static string GetHtml()
        //{
        //    string resourceName = "RTL.Editor.App.editor.html"; 
        //    string result = string.Empty;

        //    // Get the assembly that contains the embedded resource
        //    Assembly assembly = Assembly.GetExecutingAssembly();

        //    // Get the resource stream
        //    var resource = assembly.GetManifestResourceNames().FirstOrDefault(rn => rn == resourceName);
        //    if (resource != null)
        //    {
        //        using (Stream? stream = assembly.GetManifestResourceStream(resource))
        //        {
        //            if (stream == null)
        //            {
        //                // Handle the case where the resource is not found (stream will be null)
        //                Console.WriteLine($"Resource '{resourceName}' not found.");
        //            }
        //            else
        //            {
        //                // Read the stream content using a StreamReader
        //                using StreamReader reader = new(stream);
        //                result = reader.ReadToEnd();
        //            }
        //        }

        //    }
        //    return result;
        //}

        //private async void LoadSampleToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    if (webView.CoreWebView2 == null) return;

        //    string sample = "שלום, זה טקסט לדוגמה עם עברית ו English ביחד.";
        //    string script = $@"
        //        (function() {{
        //            const editor = document.getElementById('editor');
        //            if (editor) editor.innerHTML = '{EscapeForJs(sample)}';
        //        }})();";

        //    await webView.CoreWebView2.ExecuteScriptAsync(script);
        //}

        private async void GetHtmlToolStripMenuItem_Click(object sender, EventArgs e)
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

        //private void getClipboardTextButton_Click(object sender, EventArgs e)
        //{
        //    if (Clipboard.ContainsText())
        //    {
        //        string clipboardText = Clipboard.GetText();
        //        MessageBox.Show("Text from clipboard: " + clipboardText);
        //    }
        //    else
        //    {
        //        MessageBox.Show("Clipboard does not contain text.");
        //    }
        //}


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

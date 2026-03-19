using Microsoft.Web.WebView2.Core;
using System.Text.Json.Nodes;

namespace RTL.Editor.App
{
    public class JsEventListener
    {
        private readonly CoreWebView2 _core;

        private readonly Dictionary<char, char> latinToHebrewMap = new()
        {
         {'a','א'}, {'b','ב'}, {'g', 'ג'}, {'d', 'ד'}, {'h','ה' },
            {'v','ו'}, {'z','ז'}, {'x', 'ח'}, {'j', 'ט'}, {'y','י'},
            {'k','כ'}, {'K','ך'}, {'l','ל'}, {'m', 'מ'}, {'M', 'ם'}, {'n', 'נ'}, {'N', 'ן'},{'s', 'ס'},
            {'w', 'ע'}, {'p','פ'},{'P','ף'}, {'c','צ'}, {'C','ץ'}, {'q','ק'}, {'r','ר'},
            {'i','ש'}, {'t', 'ת'}
        };

        public JsEventListener(CoreWebView2 core)
        {
            _core = core;
            _core.WebMessageReceived += Core_WebMessageReceived;
        }

        private void StringMessageReceived(object? sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
            var str = e.TryGetWebMessageAsString();
        }

        private void Core_WebMessageReceived(object? sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
            var json = e.WebMessageAsJson;
            json ??= "{}";
            Console.WriteLine("JS Event: " + json);
            JsonNode? node = JsonNode.Parse(json);
            if (node == null)
            {
                Console.WriteLine("Failed to parse JSON: " + json);
                return;
            }

            // Deserialize if needed
            //dynamic msg = JsonSerializer.Deserialize<dynamic>(json);
            JsonNode? typeNode = node["type"];
            if (typeNode != null)
            {
                switch (typeNode.ToString())
                {
                    case "null":
                        Console.WriteLine("Received null event");
                        break;
                        
                    case "getClipboardText":
                        if (Clipboard.ContainsText())
                        {
                            string clipboardText = Clipboard.GetText();
                            Console.WriteLine("Text from clipboard: " + clipboardText);
                            for(int i = 0; i < clipboardText.Length; i++)
                            {
                                char c = clipboardText[i];
                                if (latinToHebrewMap.TryGetValue(c, out char hebrewChar))
                                {
                                    clipboardText = clipboardText.Remove(i, 1).Insert(i, hebrewChar.ToString());
                                }
                            }
                            _core.PostWebMessageAsString(clipboardText);
                        }
                        else
                        {
                            Console.WriteLine("Clipboard does not contain text data.");
                        }
                        break;

                }
            }
        }

      
    }
}

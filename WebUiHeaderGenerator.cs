using System;
using System.IO;
using System.Linq;
using System.Text;

namespace AmbiBack_USB_Flasher
{
    /// <summary>
    /// Creates ambiwebserver_html_raw.h and ambiwebserver_html_raw_min.h from web\HTML and web\HTML_MIN of the
    /// firmware source. These headers are generated files and are not in the firmware repository.
    /// Same output format as web\convert.py --no-minify (the flasher does not need Python or Node.js).
    /// Text files are stored as UTF-8 bytes.
    /// </summary>
    public static class WebUiHeaderGenerator
    {
        public static void EnsureHeaders(string sourceDir, Action<string> log)
        {
            Generate(sourceDir, "HTML", "ambiwebserver_html_raw.h", log);
            Generate(sourceDir, "HTML_MIN", "ambiwebserver_html_raw_min.h", log);
        }

        public static bool HeadersExist(string sourceDir)
        {
            return File.Exists(Path.Combine(sourceDir, "ambiwebserver_html_raw.h")) &&
                   File.Exists(Path.Combine(sourceDir, "ambiwebserver_html_raw_min.h"));
        }

        private static void Generate(string sourceDir, string inputFolder, string outputFile, Action<string> log)
        {
            string outPath = Path.Combine(sourceDir, outputFile);
            if (File.Exists(outPath)) return;
            string inputDir = Path.Combine(sourceDir, "web", inputFolder);
            if (!Directory.Exists(inputDir))
            {
                log?.Invoke("Web UI folder not found, " + outputFile + " not created: " + inputDir);
                return;
            }

            var sb = new StringBuilder();
            var files = Directory.GetFiles(inputDir, "*", SearchOption.AllDirectories).OrderBy(f => f, StringComparer.Ordinal);
            int count = 0;
            foreach (string full in files)
            {
                string rel = full.Substring(inputDir.Length).TrimStart('\\', '/').Replace('\\', '/');
                string lower = rel.ToLowerInvariant();
                byte[] data;
                if (lower.EndsWith(".js") || lower.EndsWith(".html") || lower.EndsWith(".css"))
                    data = Encoding.UTF8.GetBytes(File.ReadAllText(full, Encoding.UTF8));
                else if (lower.EndsWith(".png") || lower.EndsWith(".jpg") || lower.EndsWith(".gif"))
                    data = File.ReadAllBytes(full);
                else
                    continue;

                string symbolSource = rel.Replace("/", "_");
                int dot = symbolSource.LastIndexOf('.');
                string name = dot >= 0 ? symbolSource.Substring(0, dot) : symbolSource;
                string ext = dot >= 0 ? symbolSource.Substring(dot + 1) : "";
                string symbol = "data_" + name + "_" + ext;
                string path = "/" + rel;

                sb.Append("// ").Append(path).Append("\n");
                sb.Append("static const char* ").Append(symbol).Append("_path PROGMEM = \"").Append(path).Append("\";\n");
                sb.Append("const char ").Append(symbol).Append("[] PROGMEM = {");
                for (int i = 0; i < data.Length; i++)
                {
                    if (i > 0) sb.Append(',');
                    sb.Append("0X").Append(data[i].ToString("X"));
                }
                sb.Append("};\n");
                sb.Append("#define ").Append((symbol + "_len").ToUpperInvariant()).Append(' ').Append(data.Length).Append("\n\n");
                count++;
            }
            File.WriteAllText(outPath, sb.ToString(), new UTF8Encoding(false));
            log?.Invoke("Created " + outputFile + " from web\\" + inputFolder + " (" + count + " files, not minified).");
        }
    }
}

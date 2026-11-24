using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace AmbiBack_USB_Flasher
{
    /// <summary>A firmware library downloaded from GitHub at a fixed tag or commit.</summary>
    public class PinnedLibrary
    {
        public string Folder;
        public string Repo;    // owner/name
        public string Tag;     // tag or commit
        public PinnedLibrary(string folder, string repo, string tag) { Folder = folder; Repo = repo; Tag = tag; }
        public override string ToString() { return Folder + " " + (Tag.Length == 40 ? Tag.Substring(0, 7) : Tag); }
    }

    /// <summary>
    /// Downloads and installs everything needed to build the firmware: arduino-cli or the Arduino IDE 1.8.x
    /// (arduino-builder), the ESP32 Arduino core with its tools, the firmware libraries and the firmware source.
    /// All downloads come from GitHub, except the Windows build of the Arduino IDE 1.8.x, which Arduino only
    /// publishes on downloads.arduino.cc.
    /// </summary>
    public class ToolchainInstaller
    {
        public const string Esp32IndexUrl = "https://raw.githubusercontent.com/espressif/arduino-esp32/gh-pages/package_esp32_index.json";
        public const string FirmwareRepo = "ambiback/ambiback-firmware";

        /// <summary>Library versions known to build the firmware with core 2.0.0, 2.0.8 and 3.3.6.</summary>
        public static readonly PinnedLibrary[] Libraries =
        {
            new PinnedLibrary("FastLED", "FastLED/FastLED", "3.10.3"),
            new PinnedLibrary("AsyncTCP", "ESP32Async/AsyncTCP", "v3.4.10"),
            new PinnedLibrary("ESPAsyncWebServer", "ESP32Async/ESPAsyncWebServer", "v3.9.5"),
            new PinnedLibrary("WiFiClientSecureUdp", "alorbach/wificlientsecureudp", "1.0.0"),
            new PinnedLibrary("ESP8266_and_ESP32_OLED_driver_for_SSD1306_displays", "ThingPulse/esp8266-oled-ssd1306", "4.6.2"),
        };

        /// <summary>Tools from the ESP32 package index that are needed to build for the classic ESP32.</summary>
        private static readonly string[] NeededTools = { "xtensa-esp32-elf-gcc", "esp-x32", "esptool_py", "mkspiffs", "mklittlefs", "esp32-libs" };

        public static readonly string[] FallbackCliVersions = { "1.6.0" };
        public static readonly string[] FallbackIdeVersions = { "1.8.19" };
        public static readonly string[] FallbackCoreVersions = { "3.3.6", "2.0.17", "2.0.14", "2.0.8", "2.0.0", "1.0.6", "1.0.4" };

        private readonly Action<string> _log;
        private readonly Action<int, string> _progress;   // percent (-1 = unknown), status text
        private readonly CancellationToken _cancel;
        private static HttpClient _http;

        /// <summary>Callbacks are always called on the thread that created the installer (the UI thread).</summary>
        public ToolchainInstaller(Action<string> log, Action<int, string> progress, CancellationToken cancel)
        {
            var ctx = SynchronizationContext.Current;
            Action<string> l = log ?? (s => { });
            Action<int, string> pr = progress ?? ((p, s) => { });
            if (ctx != null)
            {
                _log = s => ctx.Post(_ => l(s), null);
                _progress = (p, s) => ctx.Post(_ => pr(p, s), null);
            }
            else
            {
                _log = l;
                _progress = pr;
            }
            _cancel = cancel;
        }

        private static HttpClient Http
        {
            get
            {
                if (_http == null)
                {
                    ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
                    _http = new HttpClient(new HttpClientHandler { AllowAutoRedirect = true });
                    _http.Timeout = TimeSpan.FromMinutes(30);
                    _http.DefaultRequestHeaders.UserAgent.ParseAdd("AmbiBack-USB-Flasher");
                }
                return _http;
            }
        }

        // ------------------------------------------------------------------ version lists

        public static async Task<List<string>> GetCliVersionsAsync()
        {
            string json = await Http.GetStringAsync("https://api.github.com/repos/arduino/arduino-cli/releases?per_page=30");
            var list = new List<string>();
            foreach (JSONNode rel in JSON.Parse(json).Children)
            {
                if (rel["prerelease"].AsBool || rel["draft"].AsBool) continue;
                string tag = rel["tag_name"].Value.TrimStart('v');
                if (Regex.IsMatch(tag, @"^\d+\.\d+\.\d+$")) list.Add(tag);
            }
            return list.OrderByDescending(AppPaths.VersionKey).ToList();
        }

        public static async Task<List<string>> GetIdeVersionsAsync()
        {
            string json = await Http.GetStringAsync("https://api.github.com/repos/arduino/Arduino/tags?per_page=100");
            var list = new List<string>();
            foreach (JSONNode tag in JSON.Parse(json).Children)
            {
                string name = tag["name"].Value;
                if (Regex.IsMatch(name, @"^1\.8\.\d+$")) list.Add(name);
            }
            return list.OrderByDescending(AppPaths.VersionKey).ToList();
        }

        private static JSONNode _esp32Index;

        private static async Task<JSONNode> GetEsp32IndexAsync()
        {
            if (_esp32Index == null)
                _esp32Index = JSON.Parse(await Http.GetStringAsync(Esp32IndexUrl));
            return _esp32Index;
        }

        public static async Task<List<string>> GetCoreVersionsAsync()
        {
            JSONNode idx = await GetEsp32IndexAsync();
            var list = new List<string>();
            foreach (JSONNode pl in idx["packages"][0]["platforms"].Children)
            {
                string v = pl["version"].Value;
                if (Regex.IsMatch(v, @"^\d+\.\d+\.\d+$")) list.Add(v);   // skip release candidates
            }
            return list.Distinct().OrderByDescending(AppPaths.VersionKey).ToList();
        }

        // ------------------------------------------------------------------ installers

        /// <summary>Download arduino-cli to tools\arduino-cli\&lt;version&gt;. Returns the path to arduino-cli.exe.</summary>
        public async Task<string> InstallArduinoCliAsync(string version)
        {
            string dir = Path.Combine(AppPaths.ToolsDir, "arduino-cli", version);
            string exe = Path.Combine(dir, "arduino-cli.exe");
            if (File.Exists(exe))
            {
                _log("arduino-cli " + version + " is already installed.");
                return exe;
            }
            string file = "arduino-cli_" + version + "_Windows_64bit.zip";
            string zip = await DownloadAsync("https://github.com/arduino/arduino-cli/releases/download/v" + version + "/" + file, file, "arduino-cli " + version);
            await Task.Run(() => ExtractZip(zip, dir, false));
            if (!File.Exists(exe)) throw new InvalidOperationException("arduino-cli.exe not found after extracting " + file);
            _log("arduino-cli " + version + " installed: " + dir);
            return exe;
        }

        /// <summary>Download the Arduino IDE 1.8.x (contains arduino-builder) to tools\arduino-ide\&lt;version&gt;.</summary>
        public async Task<string> InstallArduinoIdeAsync(string version)
        {
            string dir = Path.Combine(AppPaths.ToolsDir, "arduino-ide", version);
            if (File.Exists(Path.Combine(dir, "arduino-builder.exe")))
            {
                _log("Arduino IDE " + version + " is already installed.");
                return dir;
            }
            // The GitHub release of the IDE only contains the source; Arduino publishes the Windows build here.
            string file = "arduino-" + version + "-windows.zip";
            string zip = await DownloadAsync("https://downloads.arduino.cc/" + file, file, "Arduino IDE " + version);
            await Task.Run(() => ExtractZip(zip, dir, true));
            if (!File.Exists(Path.Combine(dir, "arduino-builder.exe"))) throw new InvalidOperationException("arduino-builder.exe not found after extracting " + file);
            _log("Arduino IDE " + version + " installed: " + dir);
            return dir;
        }

        /// <summary>Install an ESP32 core through arduino-cli into its own data folder (arduino-cli\&lt;core&gt;).</summary>
        public async Task InstallCoreForCliAsync(string cliExe, string coreVersion)
        {
            if (AppPaths.IsCliCoreInstalled(coreVersion))
            {
                _log("ESP32 core " + coreVersion + " (arduino-cli) is already installed.");
                return;
            }
            WriteCliConfig(coreVersion);
            string cfg = "--config-file \"" + AppPaths.CliConfigFile(coreVersion) + "\" ";
            _progress(-1, "ESP32 core " + coreVersion + ": update index");
            int rc = await ProcessRunner.RunAsync(cliExe, cfg + "core update-index", null, _log, _cancel);
            if (rc != 0) throw new InvalidOperationException("arduino-cli core update-index failed (exit code " + rc + ")");
            _progress(-1, "ESP32 core " + coreVersion + ": download and install");
            rc = await ProcessRunner.RunAsync(cliExe, cfg + "core install esp32:esp32@" + coreVersion, null, _log, _cancel);
            if (rc != 0) throw new InvalidOperationException("arduino-cli core install esp32:esp32@" + coreVersion + " failed (exit code " + rc + ")");
            _log("ESP32 core " + coreVersion + " installed for arduino-cli: " + AppPaths.CliCoreRoot(coreVersion));
        }

        /// <summary>Write the arduino-cli configuration for one core version. All folders stay inside the data root.</summary>
        public static void WriteCliConfig(string coreVersion)
        {
            string root = AppPaths.CliCoreRoot(coreVersion);
            Directory.CreateDirectory(root);
            string Y(string p) { return "'" + p.Replace("'", "''") + "'"; }
            File.WriteAllText(AppPaths.CliConfigFile(coreVersion),
                "board_manager:\n" +
                "  additional_urls:\n" +
                "    - " + Esp32IndexUrl + "\n" +
                "directories:\n" +
                "  data: " + Y(Path.Combine(root, "data")) + "\n" +
                "  downloads: " + Y(Path.Combine(AppPaths.ArduinoCliRoot, "staging")) + "\n" +
                "  user: " + Y(AppPaths.ExtraIncludeDir) + "\n" +
                "updater:\n" +
                "  enable_notification: false\n" +
                "metrics:\n" +
                "  enabled: false\n");
        }

        /// <summary>
        /// Install an ESP32 core for arduino-builder in the layout the flasher already uses:
        /// buildenv\&lt;v&gt;\packages\esp32\hardware\esp32\&lt;v&gt; and buildtools\&lt;v&gt;\tools\&lt;tool&gt;\&lt;toolversion&gt;.
        /// </summary>
        public async Task InstallCoreForBuilderAsync(string coreVersion)
        {
            JSONNode idx = await GetEsp32IndexAsync();
            JSONNode pkg = idx["packages"][0];
            JSONNode platform = null;
            foreach (JSONNode pl in pkg["platforms"].Children)
                if (pl["version"].Value == coreVersion) { platform = pl; break; }
            if (platform == null) throw new InvalidOperationException("ESP32 core " + coreVersion + " not found in the package index.");

            string coreDir = Path.Combine(AppPaths.BuildEnvDir, coreVersion, "packages", "esp32", "hardware", "esp32", coreVersion);
            if (File.Exists(Path.Combine(coreDir, "platform.txt")))
                _log("ESP32 core " + coreVersion + " is already installed (arduino-builder).");
            else
            {
                string url = platform["url"].Value;
                string zip = await DownloadAsync(url, platform["archiveFileName"].Value, "ESP32 core " + coreVersion);
                await Task.Run(() => ExtractZip(zip, coreDir, true));
            }

            foreach (JSONNode dep in platform["toolsDependencies"].Children)
            {
                string name = dep["name"].Value, ver = dep["version"].Value;
                if (!NeededTools.Contains(name)) continue;
                string toolDir = Path.Combine(AppPaths.BuildToolsDir, coreVersion, "tools", name, ver);
                if (Directory.Exists(toolDir) && Directory.EnumerateFileSystemEntries(toolDir).Any())
                {
                    _log("Tool " + name + " " + ver + " is already installed.");
                    continue;
                }
                JSONNode sys = FindToolSystem(pkg, name, ver);
                if (sys == null) throw new InvalidOperationException("No Windows download for tool " + name + " " + ver + " in the package index.");
                string zip = await DownloadAsync(sys["url"].Value, sys["archiveFileName"].Value, name + " " + ver);
                await Task.Run(() => ExtractZip(zip, toolDir, true));
            }
            _log("ESP32 core " + coreVersion + " installed for arduino-builder.");
        }

        private static JSONNode FindToolSystem(JSONNode pkg, string name, string ver)
        {
            foreach (JSONNode tool in pkg["tools"].Children)
            {
                if (tool["name"].Value != name || tool["version"].Value != ver) continue;
                JSONNode best = null;
                foreach (JSONNode sys in tool["systems"].Children)
                {
                    string host = sys["host"].Value;
                    if (host == "x86_64-mingw32") return sys;
                    if (host == "i686-mingw32") best = sys;
                }
                return best;
            }
            return null;
        }

        /// <summary>Download the pinned libraries to &lt;sketchbook&gt;\libraries.</summary>
        public async Task InstallLibrariesAsync(string sketchbookDir)
        {
            string libDir = Path.Combine(sketchbookDir, "libraries");
            Directory.CreateDirectory(libDir);
            foreach (var lib in Libraries)
            {
                string dest = Path.Combine(libDir, lib.Folder);
                string marker = Path.Combine(dest, ".ambiback-tag");
                if (File.Exists(marker) && File.ReadAllText(marker).Trim() == lib.Tag)
                {
                    _log("Library " + lib + " is already installed.");
                    continue;
                }
                string file = lib.Repo.Replace('/', '-') + "-" + lib.Tag + ".zip";
                string zip = await DownloadAsync("https://github.com/" + lib.Repo + "/archive/" + lib.Tag + ".zip", file, "Library " + lib);
                if (Directory.Exists(dest)) Directory.Delete(dest, true);
                await Task.Run(() => ExtractZip(zip, dest, true));
                File.WriteAllText(marker, lib.Tag);
            }
        }

        public static bool AreLibrariesInstalled(string sketchbookDir)
        {
            // Installed in the pinned version (the marker file is written after the download)
            return Libraries.All(l =>
            {
                string marker = Path.Combine(sketchbookDir, "libraries", l.Folder, ".ambiback-tag");
                return File.Exists(marker) && File.ReadAllText(marker).Trim() == l.Tag;
            });
        }

        /// <summary>
        /// Download the firmware source (branch or tag) from GitHub into the default source folder and create the
        /// web UI headers. A folder that is a git checkout is never overwritten.
        /// </summary>
        public async Task DownloadFirmwareSourceAsync(string gitRef, string destDir)
        {
            if (Directory.Exists(Path.Combine(destDir, ".git")))
                throw new InvalidOperationException("The source folder is a git checkout and is not overwritten: " + destDir);
            if (string.IsNullOrWhiteSpace(gitRef)) gitRef = "main";
            string file = "ambiback-firmware-" + Regex.Replace(gitRef, @"[^A-Za-z0-9._-]", "_") + ".zip";
            string zipPath = Path.Combine(AppPaths.DownloadsDir, file);
            if (File.Exists(zipPath)) File.Delete(zipPath);   // a branch changes, always get the current state
            string zip = await DownloadAsync("https://github.com/" + FirmwareRepo + "/archive/" + gitRef + ".zip", file, "Firmware source (" + gitRef + ")");
            if (Directory.Exists(destDir)) Directory.Delete(destDir, true);
            await Task.Run(() => ExtractZip(zip, destDir, true));
            File.WriteAllText(Path.Combine(destDir, ".ambiback-ref"), gitRef + "\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            _log("Firmware source downloaded: " + destDir);
            WebUiHeaderGenerator.EnsureHeaders(destDir, _log);
        }

        // ------------------------------------------------------------------ helpers

        /// <summary>Download a file to the downloads folder (reused when it is already there). Returns the local path.</summary>
        private async Task<string> DownloadAsync(string url, string fileName, string label)
        {
            Directory.CreateDirectory(AppPaths.DownloadsDir);
            string dest = Path.Combine(AppPaths.DownloadsDir, fileName);
            if (File.Exists(dest) && new FileInfo(dest).Length > 0)
            {
                _log("Using downloaded file: " + fileName);
                return dest;
            }
            _log("Download: " + url);
            string part = dest + ".part";
            using (var resp = await Http.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, _cancel))
            {
                if (!resp.IsSuccessStatusCode)
                    throw new InvalidOperationException("Download failed (" + (int)resp.StatusCode + " " + resp.ReasonPhrase + "): " + url);
                long total = resp.Content.Headers.ContentLength ?? -1;
                using (var src = await resp.Content.ReadAsStreamAsync())
                using (var dst = File.Create(part))
                {
                    var buf = new byte[81920];
                    long done = 0;
                    int read, lastPct = -2;
                    while ((read = await src.ReadAsync(buf, 0, buf.Length, _cancel)) > 0)
                    {
                        await dst.WriteAsync(buf, 0, read, _cancel);
                        done += read;
                        int pct = total > 0 ? (int)(done * 100 / total) : -1;
                        if (pct != lastPct)
                        {
                            lastPct = pct;
                            _progress(pct, label + ": " + (done / (1024 * 1024)) + " MB" + (total > 0 ? " / " + (total / (1024 * 1024)) + " MB" : ""));
                        }
                    }
                }
            }
            if (File.Exists(dest)) File.Delete(dest);
            File.Move(part, dest);
            return dest;
        }

        /// <summary>Extract a zip file. With stripTopFolder, a single top-level folder in the archive is removed.</summary>
        private void ExtractZip(string zipFile, string destDir, bool stripTopFolder)
        {
            _progress(-1, "Extract " + Path.GetFileName(zipFile));
            _log("Extract " + Path.GetFileName(zipFile) + " -> " + destDir);
            Directory.CreateDirectory(destDir);
            string fullDest = Path.GetFullPath(destDir);
            using (var archive = ZipFile.OpenRead(zipFile))
            {
                string prefix = "";
                if (stripTopFolder)
                {
                    var tops = archive.Entries.Select(e => e.FullName.Replace('\\', '/').Split('/')[0]).Distinct().ToList();
                    bool allInFolder = archive.Entries.All(e => e.FullName.Replace('\\', '/').Contains("/"));
                    if (tops.Count == 1 && allInFolder) prefix = tops[0] + "/";
                }
                foreach (var entry in archive.Entries)
                {
                    _cancel.ThrowIfCancellationRequested();
                    string name = entry.FullName.Replace('\\', '/');
                    if (prefix.Length > 0) name = name.Substring(prefix.Length);
                    if (name.Length == 0) continue;
                    string target = Path.GetFullPath(Path.Combine(fullDest, name.Replace('/', Path.DirectorySeparatorChar)));
                    if (!target.StartsWith(fullDest, StringComparison.OrdinalIgnoreCase))
                        throw new InvalidOperationException("Unsafe path in archive: " + entry.FullName);
                    if (name.EndsWith("/"))
                    {
                        Directory.CreateDirectory(target);
                        continue;
                    }
                    Directory.CreateDirectory(Path.GetDirectoryName(target));
                    entry.ExtractToFile(target, true);
                }
            }
        }
    }

    /// <summary>Runs a console program and streams its output lines without blocking the UI thread.</summary>
    public static class ProcessRunner
    {
        public static Task<int> RunAsync(string exe, string args, string workDir, Action<string> onLine, CancellationToken cancel)
        {
            var tcs = new TaskCompletionSource<int>();
            var p = new Process();
            p.StartInfo.FileName = exe;
            p.StartInfo.Arguments = args;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.StandardOutputEncoding = Encoding.UTF8;
            p.StartInfo.StandardErrorEncoding = Encoding.UTF8;
            if (!string.IsNullOrEmpty(workDir)) p.StartInfo.WorkingDirectory = workDir;
            p.EnableRaisingEvents = true;
            DataReceivedEventHandler h = (s, e) => { if (e.Data != null) onLine?.Invoke(e.Data); };
            p.OutputDataReceived += h;
            p.ErrorDataReceived += h;
            p.Exited += (s, e) =>
            {
                p.WaitForExit();   // flush the asynchronous output readers
                tcs.TrySetResult(p.ExitCode);
                p.Dispose();
            };
            onLine?.Invoke("> " + Path.GetFileName(exe) + " " + args);
            p.Start();
            p.BeginOutputReadLine();
            p.BeginErrorReadLine();
            cancel.Register(() => { try { if (!p.HasExited) p.Kill(); } catch { } tcs.TrySetCanceled(); });
            return tcs.Task;
        }

        /// <summary>Run a short command and return its standard output (null on failure).</summary>
        public static string Capture(string exe, string args, int timeoutMs = 60000)
        {
            try
            {
                using (var p = new Process())
                {
                    p.StartInfo.FileName = exe;
                    p.StartInfo.Arguments = args;
                    p.StartInfo.CreateNoWindow = true;
                    p.StartInfo.UseShellExecute = false;
                    p.StartInfo.RedirectStandardOutput = true;
                    p.StartInfo.RedirectStandardError = true;
                    p.StartInfo.StandardOutputEncoding = Encoding.UTF8;
                    p.Start();
                    var err = p.StandardError.ReadToEndAsync();
                    string output = p.StandardOutput.ReadToEnd();
                    p.WaitForExit(timeoutMs);
                    return p.ExitCode == 0 ? output : null;
                }
            }
            catch
            {
                return null;
            }
        }
    }
}

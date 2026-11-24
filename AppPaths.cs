using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace AmbiBack_USB_Flasher
{
    /// <summary>
    /// Resolves all folders the flasher works with. Every path setting may be empty; then a default
    /// relative to the data root is used. The data root is the program folder when it is writable
    /// (portable use), otherwise %LOCALAPPDATA%\AmbiBack\USB Flasher.
    /// </summary>
    public static class AppPaths
    {
        public const string DefaultSourceFile = "AmbiBack.Controller.ino";

        private static string _dataRoot;

        /// <summary>Default data root: program folder if writable, else the user profile.</summary>
        public static string DataRoot
        {
            get
            {
                if (_dataRoot == null)
                    _dataRoot = IsWritable(Application.StartupPath)
                        ? Application.StartupPath
                        : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "AmbiBack", "USB Flasher");
                return _dataRoot;
            }
        }

        private static string Setting(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }

        /// <summary>Root for firmware, buildenv, buildtools, tools, arduino-cli, sketchbook and source.</summary>
        public static string Root
        {
            get
            {
                var s = Properties.Settings.Default;
                return Setting(s.szStartupPath) ?? Setting(s.szBuildDir) ?? DataRoot;
            }
        }

        /// <summary>Folder for the _build and _cache subfolders.</summary>
        public static string BuildDir { get { return Setting(Properties.Settings.Default.szBuildDir) ?? Root; } }

        public static string FirmwareDir { get { return Path.Combine(Root, "firmware"); } }
        public static string BuildEnvDir { get { return Path.Combine(Root, "buildenv"); } }
        public static string BuildToolsDir { get { return Path.Combine(Root, "buildtools"); } }
        public static string ToolsDir { get { return Path.Combine(Root, "tools"); } }
        public static string DownloadsDir { get { return Path.Combine(Root, "downloads"); } }
        public static string ArduinoCliRoot { get { return Path.Combine(Root, "arduino-cli"); } }
        public static string DefaultSketchbook { get { return Path.Combine(Root, "sketchbook"); } }
        public static string DefaultSourceDir { get { return Path.Combine(Root, "source", "ambiback-firmware"); } }

        /// <summary>Folder that contains a "libraries" subfolder with the firmware libraries.</summary>
        public static string ExtraIncludeDir { get { return Setting(Properties.Settings.Default.szExtraIncludeDir) ?? DefaultSketchbook; } }

        public static string SourceDir { get { return Setting(Properties.Settings.Default.szSourceDir) ?? DefaultSourceDir; } }

        public static string SourceFile { get { return Setting(Properties.Settings.Default.szSourceFile) ?? DefaultSourceFile; } }

        /// <summary>Arduino IDE 1.8.x folder with arduino-builder.exe.</summary>
        public static string BinDir
        {
            get
            {
                string s = Setting(Properties.Settings.Default.szBinDir);
                if (s != null) return s;
                string local = NewestSubdir(Path.Combine(ToolsDir, "arduino-ide"), "arduino-builder.exe");
                if (local != null) return local;
                return @"C:\Program Files (x86)\Arduino";
            }
        }

        /// <summary>arduino-cli.exe to use, or null if none is installed.</summary>
        public static string ArduinoCliExe
        {
            get
            {
                string ver = Setting(Properties.Settings.Default.szCliVersion);
                if (ver != null)
                {
                    string exe = Path.Combine(ToolsDir, "arduino-cli", ver, "arduino-cli.exe");
                    if (File.Exists(exe)) return exe;
                }
                string newest = NewestSubdir(Path.Combine(ToolsDir, "arduino-cli"), "arduino-cli.exe");
                return newest == null ? null : Path.Combine(newest, "arduino-cli.exe");
            }
        }

        /// <summary>Data folder of one ESP32 core installed through arduino-cli (one folder per core version).</summary>
        public static string CliCoreRoot(string coreVersion) { return Path.Combine(ArduinoCliRoot, coreVersion); }
        public static string CliConfigFile(string coreVersion) { return Path.Combine(CliCoreRoot(coreVersion), "arduino-cli.yaml"); }
        public static string CliPackagesDir(string coreVersion) { return Path.Combine(CliCoreRoot(coreVersion), "data", "packages", "esp32"); }

        public static bool IsCliCoreInstalled(string coreVersion)
        {
            return Directory.Exists(Path.Combine(CliPackagesDir(coreVersion), "hardware", "esp32", coreVersion));
        }

        /// <summary>Device preview image folder that can override the bundled images.</summary>
        public static string DeviceImageOverrideDir { get { return Path.Combine(Root, "devices"); } }
        public static string BundledDeviceImageDir { get { return Path.Combine(Application.StartupPath, "devices"); } }

        public static string NewestSubdir(string parent, string requiredFile)
        {
            if (!Directory.Exists(parent)) return null;
            return Directory.GetDirectories(parent)
                .Where(d => requiredFile == null || File.Exists(Path.Combine(d, requiredFile)))
                .OrderByDescending(d => VersionKey(Path.GetFileName(d)))
                .FirstOrDefault();
        }

        /// <summary>Sort key for version folder names like 1.6.0, 3.3.6, 1.0.4 (non-numeric parts sort last).</summary>
        public static string VersionKey(string name)
        {
            if (name == null) return "";
            var parts = name.Split('.', '-', '_');
            return string.Join(".", parts.Select(p => int.TryParse(p, out int n) ? n.ToString("D6") : "~" + p));
        }

        public static bool IsWritable(string dir)
        {
            try
            {
                if (!Directory.Exists(dir)) return false;
                string probe = Path.Combine(dir, ".write-test-" + Guid.NewGuid().ToString("N"));
                File.WriteAllText(probe, "");
                File.Delete(probe);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void OpenFolder(string dir)
        {
            try
            {
                Directory.CreateDirectory(dir);
                System.Diagnostics.Process.Start("explorer.exe", "\"" + dir + "\"");
            }
            catch (Exception ex)
            {
                Globals.HandleException("OpenFolder", ex, true, false);
            }
        }
    }
}

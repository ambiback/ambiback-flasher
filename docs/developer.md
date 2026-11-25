# AmbiBack USB Flasher – Developer Guide

**English** · [Deutsch](developer.de.md)

Building the tool, releases, the folder layout, downloads and the upload to your own update server. For using the tool, see the [user guide](user-guide.md).

## Building the tool

Requires Windows, Visual Studio with .NET desktop development and the .NET Framework 4.7.2 Targeting Pack.

```powershell
msbuild "AmbiBack USB Flasher.sln" -p:Configuration=Release
```

### Setup and portable ZIP

`installer\build-installer.ps1` builds the release (warnings as errors), stages the files and creates the setup with [Inno Setup 6](https://jrsoftware.org/isinfo.php) and a portable ZIP:

```powershell
.\installer\build-installer.ps1 -Version 1.0.0
```

The output goes to `%TEMP%\ambiback-flasher-build\dist` (change with `-OutRoot`). Without `-Version` the version comes from `Properties\AssemblyInfo.cs`. The script finds MSBuild with `vswhere` and `ISCC.exe` in the usual Inno Setup folders.

The setup (`installer\AmbiBackFlasher.iss`) installs per user without administrator rights by default, or for all users. It offers to install the .NET Framework when it is missing, links the USB drivers on the last page and asks on uninstall whether to delete the data folder and the settings.

## Releases

- **CI** (`.github/workflows/ci.yml`): every pull request and push to `main` builds setup and ZIP and keeps them as a workflow artifact for 14 days.
- **Release** (`.github/workflows/release.yml`): pushing a tag `v1.2.3` builds setup and ZIP and creates a **draft** GitHub release. Tags with a hyphen (`v1.2.3-beta1`) become prereleases. Check the draft and publish it by hand.

```powershell
git tag v1.0.0
git push origin v1.0.0
```

The assembly version is the tag version plus the workflow run number as fourth part.

Optional repository secrets:

| Secret | Effect |
|---|---|
| `SIGN_PFX_BASE64`, `SIGN_PFX_PASSWORD` | Code-sign the program and the setup (PFX as Base64). Without them the release is unsigned. |
| `COPILOT_GITHUB_TOKEN` | Fine-grained PAT with the *Copilot Requests* permission. Writes a German and English changelog with GitHub Copilot CLI. Without it, GitHub's generated notes are used. |

## Folders and downloads

All folders are relative to one data folder: the application folder if it is writable, otherwise `%LOCALAPPDATA%\AmbiBack\USB Flasher`. Every path can be changed in **File > Settings**; an empty field uses the default.

```text
firmware\<device>\<version>\        built images (AmbiBack.Controller.ino.bin, .partitions.bin, .bootloader.bin, buildinfo.txt)
tools\arduino-cli\<version>\        arduino-cli
tools\arduino-ide\<version>\        Arduino IDE 1.8.x with arduino-builder
arduino-cli\<core>\                  ESP32 core installed through arduino-cli (one folder per core version)
buildenv\<core>\, buildtools\<core>\ ESP32 core and tools for arduino-builder
sketchbook\libraries\               firmware libraries
source\ambiback-firmware\           firmware source downloaded from GitHub
downloads\                           downloaded archives (reused)
_build\, _cache\                     build output
devices\<device>.png                 optional picture that replaces the bundled device picture
```

Settings (`user.config`) are stored in `%LOCALAPPDATA%\AmbiBack_USB_Flasher`.

Downloads come from GitHub: [arduino-cli](https://github.com/arduino/arduino-cli/releases), the [ESP32 core](https://github.com/espressif/arduino-esp32) and its tools (package index), the libraries at fixed tags ([FastLED](https://github.com/FastLED/FastLED) 3.10.3, [AsyncTCP](https://github.com/ESP32Async/AsyncTCP) v3.4.10, [ESPAsyncWebServer](https://github.com/ESP32Async/ESPAsyncWebServer) v3.9.5, [WiFiClientSecureUdp](https://github.com/alorbach/wificlientsecureudp) 1.0.0, [esp8266-oled-ssd1306](https://github.com/ThingPulse/esp8266-oled-ssd1306) 4.6.2) and the [firmware](https://github.com/ambiback/ambiback-firmware). The Windows build of the Arduino IDE 1.8.x is only published on `downloads.arduino.cc`.

Easy mode always builds with arduino-cli and the ESP32 core from the settings (3.3.6 by default). It downloads the firmware source again before each build, unless the source folder is a git checkout or a custom folder set in **File > Settings**.

The web UI headers of the firmware (`ambiwebserver_html_raw.h`, `ambiwebserver_html_raw_min.h`) are generated files. If they are missing, the tool creates them from `web\HTML` and `web\HTML_MIN`, like `web/convert.py --no-minify` (without minifying).

For flashing, the tool uses the bootloader built with the image. Older images without one use `firmware\bootloader_qio_80m.bin` and `firmware\boot_app0.bin` if present, otherwise the files of the installed ESP32 core. `esptool.exe` comes from the installed ESP32 core, or from the data folder if it is there. Flash speed comes from the device type (921600 or 1500000 baud).

## Uploading firmware

The **Upload** buttons run `scripts/upload-firmware.ps1`, which uses the OpenSSH client included with Windows. Your normal SSH setup is used: `ssh-agent`, keys in `~/.ssh` and `~/.ssh/config`. Normally you only set the server.

Copy `.env.example` to `.env` in the data folder and set:

```text
AMBIBACK_UPLOAD_HOST=firmware.example.com
```

User, port, remote path and a specific key file are optional. Environment variables override `.env`. The image is stored as `<path>/<device>/<version>/update.bin`.

Connect once with `ssh <server>` to confirm the host key. Uploads run without prompts, so password logins and keys that need a passphrase without `ssh-agent` do not work.

The script also works on its own:

```powershell
.\scripts\upload-firmware.ps1 -Device m5alit -Version 0.5.231 -SourceFile .\firmware\m5alit\0.5.231\AmbiBack.Controller.ino.bin -EnvFile .env
```

Add `-DryRun` to see the planned steps without connecting.

## Serial protocol

The device commands go over the serial port at 115200 baud: `%INFO:`, `%REBOOT:`, `%FACTORYDEFAULT:`, `%WIFI:` (SSID and key, each terminated by `\0`), `%RESETWIFI`, `%LEDMODE:<n>`, `%SETRELAY:` (ID terminated by `\0`) and `%LEDDATA:` (binary LED frame). Strings are sent as plain UTF-8 bytes without a length prefix, for example `%WIFI:Home\0secret\0`; see the handlers in `frmMain.cs`. The device answers `%INFO:` with `%info: {…}` (JSON with `DeviceType`, `DeviceVersion`, `DeviceID`, `DeviceMAC`, `DeviceName`, `DeviceIP`). The firmware side is in `serialhelper.cpp` of the [firmware](https://github.com/ambiback/ambiback-firmware).

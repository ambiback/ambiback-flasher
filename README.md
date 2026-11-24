# AmbiBack USB Flasher

**English** · [Deutsch](README.de.md)

Windows tool for building AmbiBack firmware, flashing it over USB and controlling a connected device through its serial port.

It is part of the [AmbiBack project](https://github.com/ambiback) and works with the [AmbiBack firmware](https://github.com/ambiback/ambiback-firmware).

## Status

- This is an internal development tool published as is. Expect rough edges.
- The Release build of this repository passes.
- **The migrated tool has not been started or tested with real hardware.**
- AmbiBack devices have not been on sale for around five years. This repository is for DIY use and further development, with no product support. More at [ambiback.de](https://www.ambiback.de/).

## Features

- **Easy mode:** pick the device from pictures, choose the USB port and press **Install Firmware**. If no image is there yet, the tool downloads everything it needs, builds the newest firmware from GitHub and flashes it. Wi-Fi can be set right after flashing.
- **Advanced mode:** all details as before: build options, builder and ESP32 core, serial monitor, device info and control, upload.
- Build firmware for all AmbiBack device types with `arduino-cli` (default) or `arduino-builder` (Arduino IDE 1.8.x)
- **Tools > Set Up Arduino and ESP32** downloads arduino-cli and/or the Arduino IDE 1.8.x, an ESP32 core in the chosen version, the firmware libraries and the firmware source
- Flash firmware over USB with `esptool`, with progress
- Upload firmware to your own update server with OpenSSH `sftp`
- Serial monitor
- Device control: info, reboot, Wi-Fi setup, LED test and factory defaults
- English and German user interface (**Language** menu)

## Building the tool

Requires Windows, Visual Studio with .NET desktop development and the .NET Framework 4.7.2 Targeting Pack.

```powershell
msbuild "AmbiBack USB Flasher.sln" -p:Configuration=Release
```

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

Downloads come from GitHub: [arduino-cli](https://github.com/arduino/arduino-cli/releases), the [ESP32 core](https://github.com/espressif/arduino-esp32) and its tools (package index), the libraries at fixed tags ([FastLED](https://github.com/FastLED/FastLED) 3.10.3, [AsyncTCP](https://github.com/ESP32Async/AsyncTCP) v3.4.10, [ESPAsyncWebServer](https://github.com/ESP32Async/ESPAsyncWebServer) v3.9.5) and the [firmware](https://github.com/ambiback/ambiback-firmware). The Windows build of the Arduino IDE 1.8.x is only published on `downloads.arduino.cc`.

The web UI headers of the firmware (`ambiwebserver_html_raw.h`, `ambiwebserver_html_raw_min.h`) are generated files. If they are missing, the tool creates them from `web\HTML` and `web\HTML_MIN`, like `web/convert.py --no-minify` (without minifying).

For flashing, the tool uses the bootloader built with the image. Older images without one use `firmware\bootloader_qio_80m.bin` and `firmware\boot_app0.bin` if present, otherwise the files of the installed ESP32 core. `esptool.exe` comes from the installed ESP32 core, or from the data folder if it is there.

## Uploading firmware

The **Upload** buttons run `scripts/upload-firmware.ps1`, which uses the OpenSSH client included with Windows. Your normal SSH setup is used: `ssh-agent`, keys in `~/.ssh` and `~/.ssh/config`. Normally you only set the server.

Copy `.env.example` to `.env` next to the application and set:

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

## USB drivers

Depending on the module, USB access needs a driver from the chip manufacturer:

- [FTDI VCP Drivers](https://ftdichip.com/drivers/vcp-drivers/)
- [Silicon Labs CP210x Drivers](https://www.silabs.com/developers/usb-to-uart-bridge-vcp-drivers)

## License

Copyright (C) 2020-2026 Andre Lorbach

This program is free software under the [GNU General Public License version 3](LICENSE). It comes with no warranty. Third-party code keeps its own license, see [THIRD_PARTY_NOTICES.md](THIRD_PARTY_NOTICES.md).

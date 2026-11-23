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

- Build firmware for all AmbiBack device types through `arduino-builder`
- Flash firmware over USB with `esptool`
- Upload firmware to your own update server with OpenSSH `sftp`
- Serial monitor
- Device control: info, reboot, Wi-Fi setup, LED test and factory defaults

## Building the tool

Requires Windows, Visual Studio with .NET desktop development and the .NET Framework 4.7.2 Targeting Pack.

```powershell
msbuild "AmbiBack USB Flasher.sln" -p:Configuration=Release
```

## Required files

Compilers, ESP32 cores, tools and firmware images are not included. Place them in the application folder, or in the folder set as the startup path:

```text
esptool.exe
firmware\boot_app0.bin
firmware\bootloader_qio_80m.bin
firmware\<device>\<version>\AmbiBack.Controller.ino.bin
firmware\<device>\<version>\AmbiBack.Controller.ino.partitions.bin
buildenv\<version>\packages\esp32\hardware\esp32\<version>\
buildtools\<version>\tools\
```

- [esptool](https://github.com/espressif/esptool/releases) comes from Espressif.
- ESP32 cores and toolchains come from [arduino-esp32](https://github.com/espressif/arduino-esp32).
- Building firmware also needs Arduino IDE 1.8 with `arduino-builder` and the firmware's libraries. Set the paths in the tool's settings.
- Earlier firmware releases were built with a customized ESP32 core 2.0.0, which is not published yet. See the [firmware README](https://github.com/ambiback/ambiback-firmware) for the current build status.

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

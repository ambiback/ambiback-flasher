# AmbiBack USB Flasher – Agent Guide

## Project Overview

**AmbiBack USB Flasher** is a Windows desktop application (C# WinForms, .NET Framework 4.7.2) for building, flashing, and monitoring firmware for ESP32-based AmbiBack devices (e.g. M5Stack Camera, M5Stack Timer-CAM).

### Key Capabilities

- **Build** – Compile firmware via arduino-builder with custom build environments (1.0.4, 1.0.6, 2.0.0, 2.0.0.custom)
- **Flash** – Flash firmware to device over USB/serial using esptool
- **Monitor** – Serial monitor for device communication
- **Upload** – Upload built firmware with `scripts/upload-firmware.ps1` (OpenSSH sftp, settings in `.env`)
- **Device control** – Get info, reboot, set WiFi, relay, LED modes, factory defaults

### Tech Stack

- **UI**: WinForms (frmMain, frmSettings)
- **Serial**: System.IO.Ports.SerialPort
- **Build**: arduino-builder.exe with `-logger=machine`
- **Flash**: esptool.exe
- **Output**: RichTextBox for colored debug/monitor output

### Important Paths

- `build/buildenv/` – Arduino build environments
- `build/buildtools/` – Compiler toolchains (xtensa-esp32-elf-gcc, esptool_py)
- `build/_build/<device>/` – Build output (e.g. m5camtx)
- `firmware/<device>/<version>/` – Compiled firmware images
- `_usbdrivers/` – FTDI (CDM) and CP210x USB drivers

### Arduino Machine Output Format

Build output uses format: `===info ||| Progress {0} ||| [93.75]` – progress value is in square brackets.

---

## Agent Keywords

### SUMMARIZE

When the user says **SUMMARIZE**, provide a concise summary of:

1. **Recent session changes** – What was implemented or fixed in the current conversation
2. **Squashed commit message** – A ready-to-use git commit message for the changes
3. **Project context** – Brief reminder of what this project does if relevant

---

## Conventions

- **C# style**: Standard .NET conventions, PascalCase for public members
- **Forms**: Main logic in `frmMain.cs`, layout in `frmMain.Designer.cs`
- **Settings**: Stored in `Properties.Settings.Default`
- **Device types**: Defined in `CDeviceHelper.aDeviceTypes`

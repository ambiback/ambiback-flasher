# AmbiBack USB Flasher

[English](README.md) · **Deutsch**

Windows-Werkzeug, um AmbiBack-Firmware zu bauen, per USB zu flashen und ein angeschlossenes Gerät über die serielle Schnittstelle zu steuern.

Es gehört zum [AmbiBack-Projekt](https://github.com/ambiback) und arbeitet mit der [AmbiBack-Firmware](https://github.com/ambiback/ambiback-firmware).

## Status

- Dies ist ein internes Entwicklungswerkzeug, das unverändert im Funktionsumfang veröffentlicht wird. Rechne mit Ecken und Kanten.
- Der Release-Build dieses Repositorys läuft durch.
- **Das migrierte Werkzeug wurde weder gestartet noch mit echter Hardware getestet.**
- AmbiBack-Geräte werden seit rund fünf Jahren nicht mehr verkauft. Dieses Repository ist für Eigenbau und Weiterentwicklung gedacht, ohne Produktsupport. Mehr auf [ambiback.de](https://www.ambiback.de/).

## Funktionen

- Firmware für alle AmbiBack-Gerätetypen über `arduino-builder` bauen
- Firmware per USB mit `esptool` flashen
- Firmware mit OpenSSH-`sftp` auf einen eigenen Update-Server hochladen
- Serieller Monitor
- Gerätesteuerung: Info, Neustart, WLAN-Einrichtung, LED-Test und Werkseinstellungen

## Werkzeug bauen

Voraussetzungen sind Windows, Visual Studio mit .NET-Desktop-Entwicklung und das .NET Framework 4.7.2 Targeting Pack.

```powershell
msbuild "AmbiBack USB Flasher.sln" -p:Configuration=Release
```

## Benötigte Dateien

Compiler, ESP32-Cores, Tools und Firmware-Images sind nicht enthalten. Lege sie in den Anwendungsordner oder in den als Startpfad eingestellten Ordner:

```text
esptool.exe
firmware\boot_app0.bin
firmware\bootloader_qio_80m.bin
firmware\<gerät>\<version>\AmbiBack.Controller.ino.bin
firmware\<gerät>\<version>\AmbiBack.Controller.ino.partitions.bin
buildenv\<version>\packages\esp32\hardware\esp32\<version>\
buildtools\<version>\tools\
```

- [esptool](https://github.com/espressif/esptool/releases) stammt von Espressif.
- ESP32-Cores und Toolchains stammen von [arduino-esp32](https://github.com/espressif/arduino-esp32).
- Zum Bauen der Firmware brauchst du außerdem Arduino IDE 1.8 mit `arduino-builder` und die Bibliotheken der Firmware. Die Pfade stellst du in den Einstellungen des Werkzeugs ein.
- Frühere Firmware-Releases wurden mit einem angepassten ESP32-Core 2.0.0 gebaut, der noch nicht veröffentlicht ist. Den aktuellen Build-Stand beschreibt das [Firmware-README](https://github.com/ambiback/ambiback-firmware).

## Firmware hochladen

Die **Upload**-Buttons starten `scripts/upload-firmware.ps1`. Das Skript nutzt den in Windows enthaltenen OpenSSH-Client und deine normale SSH-Einrichtung: `ssh-agent`, Schlüssel in `~/.ssh` und `~/.ssh/config`. Normalerweise musst du nur den Server angeben.

Kopiere `.env.example` als `.env` neben die Anwendung und trage ein:

```text
AMBIBACK_UPLOAD_HOST=firmware.example.com
```

Benutzer, Port, Zielpfad und eine bestimmte Schlüsseldatei sind optional. Umgebungsvariablen haben Vorrang vor der `.env`. Das Image landet unter `<pfad>/<gerät>/<version>/update.bin`.

Verbinde dich einmal mit `ssh <server>`, um den Host-Key zu bestätigen. Der Upload läuft ohne Rückfragen, deshalb funktionieren Passwort-Logins und Schlüssel mit Passphrase ohne `ssh-agent` nicht.

Das Skript lässt sich auch direkt starten:

```powershell
.\scripts\upload-firmware.ps1 -Device m5alit -Version 0.5.231 -SourceFile .\firmware\m5alit\0.5.231\AmbiBack.Controller.ino.bin -EnvFile .env
```

Mit `-DryRun` siehst du die geplanten Schritte, ohne dass eine Verbindung aufgebaut wird.

## USB-Treiber

Je nach Modul braucht der USB-Zugriff einen Treiber des Chip-Herstellers:

- [FTDI VCP Drivers](https://ftdichip.com/drivers/vcp-drivers/)
- [Silicon Labs CP210x Drivers](https://www.silabs.com/developers/usb-to-uart-bridge-vcp-drivers)

## Lizenz

Copyright (C) 2020-2026 Andre Lorbach

Dieses Programm ist freie Software unter der [GNU General Public License Version 3](LICENSE). Es wird ohne jede Gewährleistung bereitgestellt. Drittcode behält seine eigene Lizenz, siehe [THIRD_PARTY_NOTICES.md](THIRD_PARTY_NOTICES.md).

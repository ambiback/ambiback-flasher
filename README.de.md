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

- **Einfacher Modus:** Gerät per Bild wählen, USB-Anschluss wählen und **Firmware installieren** drücken. Gibt es noch kein Image, lädt das Werkzeug alles Nötige, baut die neueste Firmware von GitHub und flasht sie. Das WLAN lässt sich direkt danach einrichten.
- **Erweiterter Modus:** alle Details wie bisher: Build-Optionen, Builder und ESP32-Core, serieller Monitor, Geräteinfo und -steuerung, Upload.
- Firmware für alle AmbiBack-Gerätetypen mit `arduino-cli` (Standard) oder `arduino-builder` (Arduino IDE 1.8.x) bauen
- **Werkzeuge > Arduino und ESP32 einrichten** lädt arduino-cli und/oder die Arduino IDE 1.8.x, einen ESP32-Core in der gewählten Version, die Firmware-Bibliotheken und den Firmware-Quellcode
- Firmware per USB mit `esptool` flashen, mit Fortschrittsanzeige
- Firmware mit OpenSSH-`sftp` auf einen eigenen Update-Server hochladen
- Serieller Monitor
- Gerätesteuerung: Info, Neustart, WLAN-Einrichtung, LED-Test und Werkseinstellungen
- Oberfläche auf Englisch und Deutsch (Menü **Sprache**)

## Werkzeug bauen

Voraussetzungen sind Windows, Visual Studio mit .NET-Desktop-Entwicklung und das .NET Framework 4.7.2 Targeting Pack.

```powershell
msbuild "AmbiBack USB Flasher.sln" -p:Configuration=Release
```

## Ordner und Downloads

Alle Ordner liegen in einem Datenordner: im Anwendungsordner, wenn er beschreibbar ist, sonst unter `%LOCALAPPDATA%\AmbiBack\USB Flasher`. Jeden Pfad kannst du unter **Datei > Einstellungen** ändern; ein leeres Feld nutzt den Standard.

```text
firmware\<gerät>\<version>\         gebaute Images (AmbiBack.Controller.ino.bin, .partitions.bin, .bootloader.bin, buildinfo.txt)
tools\arduino-cli\<version>\        arduino-cli
tools\arduino-ide\<version>\        Arduino IDE 1.8.x mit arduino-builder
arduino-cli\<core>\                  ESP32-Core für arduino-cli (ein Ordner je Core-Version)
buildenv\<core>\, buildtools\<core>\ ESP32-Core und Tools für arduino-builder
sketchbook\libraries\               Firmware-Bibliotheken
source\ambiback-firmware\           von GitHub geladener Firmware-Quellcode
downloads\                           geladene Archive (werden wiederverwendet)
_build\, _cache\                     Build-Ausgabe
devices\<gerät>.png                  optionales Bild, das das mitgelieferte Gerätebild ersetzt
```

Die Downloads kommen von GitHub: [arduino-cli](https://github.com/arduino/arduino-cli/releases), der [ESP32-Core](https://github.com/espressif/arduino-esp32) mit seinen Tools (Paketindex), die Bibliotheken in festen Versionen ([FastLED](https://github.com/FastLED/FastLED) 3.10.3, [AsyncTCP](https://github.com/ESP32Async/AsyncTCP) v3.4.10, [ESPAsyncWebServer](https://github.com/ESP32Async/ESPAsyncWebServer) v3.9.5, [WiFiClientSecureUdp](https://github.com/alorbach/wificlientsecureudp) 1.0.0, [esp8266-oled-ssd1306](https://github.com/ThingPulse/esp8266-oled-ssd1306) 4.6.2) und die [Firmware](https://github.com/ambiback/ambiback-firmware). Die Windows-Version der Arduino IDE 1.8.x veröffentlicht Arduino nur auf `downloads.arduino.cc`.

Die Web-UI-Header der Firmware (`ambiwebserver_html_raw.h`, `ambiwebserver_html_raw_min.h`) sind generierte Dateien. Fehlen sie, erzeugt das Werkzeug sie aus `web\HTML` und `web\HTML_MIN` wie `web/convert.py --no-minify` (ohne Minifizierung).

Beim Flashen nutzt das Werkzeug den Bootloader, der mit dem Image gebaut wurde. Ältere Images ohne eigenen Bootloader verwenden `firmware\bootloader_qio_80m.bin` und `firmware\boot_app0.bin`, falls vorhanden, sonst die Dateien des installierten ESP32-Cores. `esptool.exe` kommt aus dem installierten ESP32-Core oder aus dem Datenordner, falls es dort liegt.

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

# AmbiBack USB Flasher – Entwicklerhandbuch

[English](developer.md) · **Deutsch**

Werkzeug bauen, Releases, Ordnerstruktur, Downloads und Upload auf einen eigenen Update-Server. Zur Bedienung siehe [Benutzerhandbuch](user-guide.de.md).

## Werkzeug bauen

Voraussetzungen sind Windows, Visual Studio mit .NET-Desktop-Entwicklung und das .NET Framework 4.7.2 Targeting Pack.

```powershell
msbuild "AmbiBack USB Flasher.sln" -p:Configuration=Release
```

### Setup und portable ZIP

`installer\build-installer.ps1` baut das Release (Warnungen als Fehler), stellt die Dateien zusammen und erzeugt mit [Inno Setup 6](https://jrsoftware.org/isinfo.php) das Setup sowie eine portable ZIP:

```powershell
.\installer\build-installer.ps1 -Version 1.0.0
```

Die Ausgabe landet in `%TEMP%\ambiback-flasher-build\dist` (änderbar mit `-OutRoot`). Ohne `-Version` kommt die Version aus `Properties\AssemblyInfo.cs`. Das Skript findet MSBuild über `vswhere` und `ISCC.exe` in den üblichen Inno-Setup-Ordnern.

Das Setup (`installer\AmbiBackFlasher.iss`) installiert standardmäßig ohne Administratorrechte für den aktuellen Benutzer, wahlweise für alle Benutzer. Es bietet die Installation des .NET Frameworks an, wenn es fehlt, verlinkt auf der letzten Seite die USB-Treiber und fragt beim Deinstallieren, ob Datenordner und Einstellungen gelöscht werden sollen.

## Releases

- **CI** (`.github/workflows/ci.yml`): Jeder Pull Request und jeder Push auf `main` baut Setup und ZIP und hebt sie 14 Tage als Workflow-Artefakt auf.
- **Release** (`.github/workflows/release.yml`): Ein Tag `v1.2.3` baut Setup und ZIP und legt einen **Entwurf** eines GitHub-Releases an. Tags mit Bindestrich (`v1.2.3-beta1`) werden Vorabversionen. Den Entwurf prüfen und von Hand veröffentlichen.

```powershell
git tag v1.0.0
git push origin v1.0.0
```

Die Assembly-Version ist die Tag-Version plus die Laufnummer des Workflows als vierte Stelle.

Optionale Repository-Secrets:

| Secret | Wirkung |
|---|---|
| `SIGN_PFX_BASE64`, `SIGN_PFX_PASSWORD` | Programm und Setup signieren (PFX als Base64). Ohne sie ist das Release unsigniert. |
| `COPILOT_GITHUB_TOKEN` | Fine-grained PAT mit der Berechtigung *Copilot Requests*. Schreibt mit der GitHub Copilot CLI ein deutsches und englisches Änderungsprotokoll. Ohne Token werden die von GitHub erzeugten Notes verwendet. |

## Ordner und Downloads

Alle Ordner liegen unter einem Datenordner: dem Anwendungsordner, wenn er beschreibbar ist, sonst `%LOCALAPPDATA%\AmbiBack\USB Flasher`. Jeder Pfad lässt sich unter **Datei > Einstellungen** ändern; ein leeres Feld verwendet den Standard.

```text
firmware\<gerät>\<version>\         gebaute Images (AmbiBack.Controller.ino.bin, .partitions.bin, .bootloader.bin, buildinfo.txt)
tools\arduino-cli\<version>\        arduino-cli
tools\arduino-ide\<version>\        Arduino IDE 1.8.x mit arduino-builder
arduino-cli\<core>\                  ESP32-Core, über arduino-cli installiert (ein Ordner je Core-Version)
buildenv\<core>\, buildtools\<core>\ ESP32-Core und Werkzeuge für arduino-builder
sketchbook\libraries\               Firmware-Bibliotheken
source\ambiback-firmware\           von GitHub geladener Firmware-Quellcode
downloads\                           geladene Archive (werden wiederverwendet)
_build\, _cache\                     Build-Ausgabe
devices\<gerät>.png                  optionales Bild, ersetzt das mitgelieferte Gerätebild
```

Die Einstellungen (`user.config`) liegen in `%LOCALAPPDATA%\AmbiBack_USB_Flasher`.

Die Downloads kommen von GitHub: [arduino-cli](https://github.com/arduino/arduino-cli/releases), der [ESP32-Core](https://github.com/espressif/arduino-esp32) mit seinen Werkzeugen (Package-Index), die Bibliotheken in festen Tags ([FastLED](https://github.com/FastLED/FastLED) 3.10.3, [AsyncTCP](https://github.com/ESP32Async/AsyncTCP) v3.4.10, [ESPAsyncWebServer](https://github.com/ESP32Async/ESPAsyncWebServer) v3.9.5, [WiFiClientSecureUdp](https://github.com/alorbach/wificlientsecureudp) 1.0.0, [esp8266-oled-ssd1306](https://github.com/ThingPulse/esp8266-oled-ssd1306) 4.6.2) und die [Firmware](https://github.com/ambiback/ambiback-firmware). Die Windows-Version der Arduino IDE 1.8.x gibt es nur auf `downloads.arduino.cc`.

Der einfache Modus baut immer mit arduino-cli und dem ESP32-Core aus den Einstellungen (standardmäßig 3.3.6). Vor jedem Build lädt er den Firmware-Quellcode neu, außer der Quellordner ist ein Git-Checkout oder ein eigener Ordner aus **Datei > Einstellungen**.

Die Web-UI-Header der Firmware (`ambiwebserver_html_raw.h`, `ambiwebserver_html_raw_min.h`) sind generierte Dateien. Fehlen sie, erzeugt das Werkzeug sie aus `web\HTML` und `web\HTML_MIN`, wie `web/convert.py --no-minify` (ohne Minifizierung).

Zum Flashen nutzt das Werkzeug den mit dem Image gebauten Bootloader. Ältere Images ohne Bootloader verwenden `firmware\bootloader_qio_80m.bin` und `firmware\boot_app0.bin`, falls vorhanden, sonst die Dateien des installierten ESP32-Cores. `esptool.exe` kommt aus dem installierten ESP32-Core oder aus dem Datenordner, wenn es dort liegt. Die Flash-Geschwindigkeit hängt vom Gerätetyp ab (921600 oder 1500000 Baud).

## Firmware hochladen

Die **Hochladen**-Buttons starten `scripts/upload-firmware.ps1`. Das Skript nutzt den in Windows enthaltenen OpenSSH-Client und die normale SSH-Einrichtung: `ssh-agent`, Schlüssel in `~/.ssh` und `~/.ssh/config`. Normalerweise genügt die Angabe des Servers.

`.env.example` als `.env` in den Datenordner kopieren und eintragen:

```text
AMBIBACK_UPLOAD_HOST=firmware.example.com
```

Benutzer, Port, Zielpfad und eine bestimmte Schlüsseldatei sind optional. Umgebungsvariablen haben Vorrang vor der `.env`. Das Image landet unter `<pfad>/<gerät>/<version>/update.bin`.

Einmal mit `ssh <server>` verbinden, um den Host-Key zu bestätigen. Der Upload läuft ohne Rückfragen, deshalb funktionieren Passwort-Logins und Schlüssel mit Passphrase ohne `ssh-agent` nicht.

Das Skript lässt sich auch direkt starten:

```powershell
.\scripts\upload-firmware.ps1 -Device m5alit -Version 0.5.231 -SourceFile .\firmware\m5alit\0.5.231\AmbiBack.Controller.ino.bin -EnvFile .env
```

Mit `-DryRun` zeigt es die geplanten Schritte, ohne eine Verbindung aufzubauen.

## Serielles Protokoll

Die Gerätebefehle gehen mit 115200 Baud über die serielle Schnittstelle: `%INFO:`, `%REBOOT:`, `%FACTORYDEFAULT:`, `%WIFI:` (SSID und Schlüssel, jeweils mit `\0` abgeschlossen), `%RESETWIFI`, `%LEDMODE:<n>`, `%SETRELAY:` (ID mit `\0` abgeschlossen) und `%LEDDATA:`. Einige werden mit .NET `BinaryWriter.Write(string)` geschrieben, das vor jede Zeichenkette ein Längenbyte setzt; siehe die Handler in `frmMain.cs`. Auf `%INFO:` antwortet das Gerät mit `%info: {…}` (JSON mit `DeviceType`, `DeviceVersion`, `DeviceID`, `DeviceMAC`, `DeviceName`, `DeviceIP`). Die Firmware-Seite steht in `serialhelper.cpp` der [Firmware](https://github.com/ambiback/ambiback-firmware).

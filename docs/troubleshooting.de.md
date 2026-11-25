# AmbiBack USB Flasher – Fehlerbehebung

[English](troubleshooting.md) · **Deutsch**

Die meisten Probleme betreffen die USB-Verbindung. Oben anfangen. Der **erweiterte** Modus zeigt das vollständige Log; **Alles kopieren** kopiert es für eine Fehlermeldung.

## Inhalt

- [Kein USB-Anschluss zu sehen](#kein-usb-anschluss-zu-sehen)
- [USB-Treiber](#usb-treiber)
- [Installieren (Flashen) schlägt fehl](#installieren-flashen-schlägt-fehl)
- [Boot-Modus von Hand](#boot-modus-von-hand)
- [Einrichten der Werkzeuge oder Bauen schlägt fehl](#einrichten-der-werkzeuge-oder-bauen-schlägt-fehl)
- [Das Gerät verbindet sich nicht mit dem WLAN](#das-gerät-verbindet-sich-nicht-mit-dem-wlan)
- [Serieller Monitor zeigt nichts oder Zeichensalat](#serieller-monitor-zeigt-nichts-oder-zeichensalat)
- [Windows warnt vor dem Programm](#windows-warnt-vor-dem-programm)
- [Ein Problem melden](#ein-problem-melden)

## Kein USB-Anschluss zu sehen

Der einfache Modus meldet *Kein USB-Anschluss gefunden. Gerät anschließen.*

1. **Ein Datenkabel verwenden.** Viele USB-Kabel laden nur. Ein anderes Kabel probieren, am besten eines, das nachweislich Daten überträgt.
2. **Einen anderen USB-Anschluss** am PC verwenden, direkt, ohne Hub.
3. **Den Geräte-Manager öffnen** (Rechtsklick auf Start > **Geräte-Manager**) und beim Einstecken unter **Anschlüsse (COM & LPT)** schauen.
   - Ein neuer **COM**-Eintrag erscheint: Der Anschluss funktioniert. Der Flasher erkennt ihn nach wenigen Sekunden.
   - Unter **Andere Geräte** erscheint ein Eintrag mit Warnzeichen, etwa *USB Serial* oder *CP2102 USB to UART*: Der Treiber fehlt, siehe [USB-Treiber](#usb-treiber).
   - Nichts ändert sich: Kabel, USB-Anschluss oder Gerät. Zuerst ein anderes Kabel probieren.
4. Stehen mehrere COM-Anschlüsse in der Liste: Gerät abziehen, Liste merken, wieder einstecken. Der neue Eintrag ist das Gerät.

## USB-Treiber

Die ESP32-Boards haben einen USB-Seriell-Chip. Windows 10 und 11 installieren für viele davon selbst einen Treiber; sonst den Treiber des Chip-Herstellers installieren:

| Chip (Name im Geräte-Manager) | Treiber |
|---|---|
| FTDI (FT231X, FT232R …) | [FTDI VCP Drivers](https://ftdichip.com/drivers/vcp-drivers/) |
| Silicon Labs CP210x (CP2102, CP2104 …) | [Silicon Labs CP210x Drivers](https://www.silabs.com/developers/usb-to-uart-bridge-vcp-drivers) |
| WCH CH340, CH341, CH9102 | [WCH CH341SER](https://www.wch-ic.com/downloads/CH341SER_EXE.html) |

Welchen Chip ein Board nutzt, ist unterschiedlich, sogar zwischen Serien desselben Boards. Typische Beispiele: Allgemeine ESP32-Boards nutzen CP2102 oder CH340, LilyGO-TTGO-Boards CP210x oder CH9102, ältere M5Stack-Geräte FTDI und neuere CH9102. Sicher sagt es der Name im Geräte-Manager oder der Chip auf der Platine.

Nach der Treiberinstallation das Gerät abziehen und wieder einstecken.

## Installieren (Flashen) schlägt fehl

Der einfache Modus meldet *Installation fehlgeschlagen. Kabel und USB-Anschluss prüfen und erneut versuchen.* Das Log im erweiterten Modus zeigt die Meldung von esptool:

| Meldung im Log | Was tun |
|---|---|
| `could not open port` / `Access is denied` | Ein anderes Programm nutzt den Anschluss: ein serieller Monitor, die Arduino IDE, AmbiBack PC, Cura, ein 3D-Drucker-Werkzeug. Programm schließen oder im Flasher **Trennen** klicken. |
| `Failed to connect to ESP32: No serial data received` / `Wrong boot mode detected` | Das Gerät ist nicht von selbst in den Download-Modus gewechselt. Siehe [Boot-Modus von Hand](#boot-modus-von-hand). |
| `A fatal error occurred: … timed out` beim Schreiben | Die Verbindung ist abgebrochen. Kürzeres Kabel, Anschluss direkt am PC, kein Hub. Erneut versuchen. |
| `No image found for …` | Das gewählte Image gibt es nicht. **Neueste Version von GitHub (jetzt bauen)** wählen oder im erweiterten Modus bauen. |
| `esptool.exe wurde nicht gefunden` | Der ESP32-Core ist nicht installiert. **Werkzeuge > Arduino und ESP32 einrichten...** ausführen. |

Erneutes Flashen ist unbedenklich. Ein Gerät, bei dem das Flashen abgebrochen ist, startet nicht, lässt sich aber immer wieder per USB flashen.

## Boot-Modus von Hand

Normalerweise setzt esptool den ESP32 über den USB-Chip in den Download-Modus. Manche Boards und Kabel unterstützen das nicht. Dann:

1. Den Taster **BOOT** (oder **IO0**) auf dem Board suchen.
2. **BOOT** gedrückt halten, **RESET** (oder **EN**) kurz drücken, dann **BOOT** loslassen. Ohne RESET-Taster: **BOOT** gedrückt halten, während das USB-Kabel eingesteckt wird.
3. Sofort **Firmware installieren** (oder **Flashen**) klicken.
4. Nach dem Flashen **RESET** drücken oder das Gerät ab- und wieder anstecken.

M5Stack-Atom-Geräte haben keinen BOOT-Taster; sie wechseln normalerweise automatisch. Falls nicht, ein anderes Kabel oder einen anderen USB-Anschluss probieren.

## Einrichten der Werkzeuge oder Bauen schlägt fehl

- **Der erste Start dauert lange.** Die Einrichtung lädt mehrere hundert MB; die Statuszeile zeigt den Fortschritt. Warten, bis sie fertig ist.
- **Kein Internet oder GitHub nicht erreichbar.** Die Meldung lautet *Einrichten der Werkzeuge fehlgeschlagen* oder *Der Firmware-Quellcode konnte nicht geladen werden*. Verbindung, Proxy oder Firewall prüfen und erneut versuchen. Geladene Archive bleiben im Ordner `downloads` und werden wiederverwendet.
- **Zu wenig Speicherplatz.** Etwa 1 GB auf dem Laufwerk des [Datenordners](user-guide.de.md#einstellungen-datenordner-und-sprache) freimachen.
- **Virenscanner.** Manche Virenscanner blockieren oder bremsen den Compiler. Den Datenordner als Ausnahme eintragen, wenn Builds mit Zugriffsfehlern abbrechen.
- **Der Build schlägt fehl.** Der einfache Modus meldet *Der Build ist fehlgeschlagen*. Den erweiterten Modus öffnen und die erste rote Zeile im Log lesen. Dann versuchen:
  1. **Build leeren** im erweiterten Modus, erneut bauen.
  2. **Werkzeuge > Arduino und ESP32 einrichten...** mit den Standardversionen (ESP32-Core 3.3.6, Firmware-Quellcode `main`).
  3. Ist unter **Datei > Einstellungen...** ein eigener Firmware-Quellordner eingetragen, das Feld leeren, damit der geladene Quellcode verwendet wird.

## Das Gerät verbindet sich nicht mit dem WLAN

- ESP32-Geräte nutzen nur **2,4 GHz**. Ein Router, der 2,4 und 5 GHz unter einem Namen anbietet, funktioniert meist; ein reines 5-GHz-Netz nicht.
- Bei Netzwerkname und Passwort zählt Groß- und Kleinschreibung. Im erweiterten Modus erneut senden: **Verbinden**, beides eingeben, **WLAN senden**, nach einigen Sekunden **Info abrufen**.
- Eine IP-Adresse `0.0.0.0` oder ein leeres Feld bedeutet, dass das Gerät noch nicht verbunden ist. Im seriellen Monitor auf WLAN-Meldungen achten.
- Für einen Neuanfang **WLAN zurücksetzen** klicken und die Daten erneut senden.

## Serieller Monitor zeigt nichts oder Zeichensalat

- Als Baudrate **115200** wählen und erneut **Verbinden** klicken.
- **Neustart** klicken, um die Startmeldungen des Geräts zu sehen.
- Leere Felder unter **Verbundenes Gerät** nach **Info abrufen**: Auf dem Gerät läuft keine oder eine sehr alte AmbiBack-Firmware. Zuerst die Firmware installieren.

## Windows warnt vor dem Programm

Setup und Programm sind nicht signiert. SmartScreen kann *Der Computer wurde durch Windows geschützt* anzeigen: **Weitere Informationen** und dann **Trotzdem ausführen** klicken. Die Dateien nur von der [Release-Seite](https://github.com/ambiback/ambiback-flasher/releases) laden.

## Ein Problem melden

Ein Issue auf [GitHub](https://github.com/ambiback/ambiback-flasher/issues) anlegen, mit:

- der Flasher-Version (**Hilfe > Über**),
- dem Gerätetyp und, falls bekannt, dem USB-Chip,
- was geklickt wurde und welche Meldung erschien,
- dem Log aus dem erweiterten Modus (**Alles kopieren**). Den WLAN-Namen entfernen, falls er nicht öffentlich sein soll.

Es gibt keinen Produktsupport; Antworten kommen aus der Community, wenn Zeit ist.

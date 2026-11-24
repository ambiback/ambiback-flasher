using System;
using System.Collections.Generic;
using System.Globalization;

namespace AmbiBack_USB_Flasher
{
    /// <summary>
    /// UI texts in English and German. The language is stored in the setting szLanguage ("en" or "de");
    /// when it is empty the Windows display language decides. Technical log output stays English.
    /// </summary>
    public static class L
    {
        public static string Language { get; private set; } = "en";

        public static void Init()
        {
            string lang = Properties.Settings.Default.szLanguage;
            if (string.IsNullOrWhiteSpace(lang))
                lang = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName == "de" ? "de" : "en";
            Set(lang);
        }

        public static void Set(string lang)
        {
            Language = lang == "de" ? "de" : "en";
        }

        public static string T(string key)
        {
            if (Texts.TryGetValue(key, out var pair))
                return Language == "de" ? pair.De : pair.En;
            return key;
        }

        public static string F(string key, params object[] args)
        {
            return string.Format(T(key), args);
        }

        private static readonly Dictionary<string, (string En, string De)> Texts = new Dictionary<string, (string, string)>
        {
            // Menu
            ["menu.file"] = ("&File", "&Datei"),
            ["menu.settings"] = ("Settings...", "Einstellungen..."),
            ["menu.openData"] = ("Open Data Folder", "Datenordner öffnen"),
            ["menu.openFirmware"] = ("Open Firmware Folder", "Firmware-Ordner öffnen"),
            ["menu.exit"] = ("Exit", "Beenden"),
            ["menu.view"] = ("&View", "&Ansicht"),
            ["menu.easy"] = ("Easy Mode", "Einfacher Modus"),
            ["menu.advanced"] = ("Advanced Mode", "Erweiterter Modus"),
            ["menu.clearLog"] = ("Clear Output", "Ausgabe leeren"),
            ["menu.device"] = ("&Device", "&Gerät"),
            ["menu.connect"] = ("Connect", "Verbinden"),
            ["menu.disconnect"] = ("Disconnect", "Trennen"),
            ["menu.getInfo"] = ("Get Device Info", "Geräteinfo abrufen"),
            ["menu.reboot"] = ("Restart Device", "Gerät neu starten"),
            ["menu.setWifi"] = ("Send Wi-Fi Settings", "WLAN-Daten senden"),
            ["menu.resetWifi"] = ("Reset Wi-Fi", "WLAN zurücksetzen"),
            ["menu.factory"] = ("Reset to Factory Defaults...", "Auf Werkseinstellungen zurücksetzen..."),
            ["menu.firmware"] = ("F&irmware", "F&irmware"),
            ["menu.build"] = ("Build", "Bauen"),
            ["menu.buildFlash"] = ("Build and Flash", "Bauen und flashen"),
            ["menu.flash"] = ("Flash", "Flashen"),
            ["menu.clean"] = ("Clean Build Folder", "Build-Ordner leeren"),
            ["menu.upload"] = ("Upload Image", "Image hochladen"),
            ["menu.buildUpload"] = ("Build and Upload", "Bauen und hochladen"),
            ["menu.tools"] = ("&Tools", "&Werkzeuge"),
            ["menu.setup"] = ("Set Up Arduino and ESP32...", "Arduino und ESP32 einrichten..."),
            ["menu.downloadSource"] = ("Download Firmware Source from GitHub", "Firmware-Quellcode von GitHub laden"),
            ["menu.language"] = ("&Language", "&Sprache"),
            ["menu.help"] = ("&Help", "&Hilfe"),
            ["menu.projectPage"] = ("Flasher on GitHub", "Flasher auf GitHub"),
            ["menu.firmwarePage"] = ("Firmware on GitHub", "Firmware auf GitHub"),
            ["menu.about"] = ("About AmbiBack USB Flasher", "Über AmbiBack USB Flasher"),

            // Header and status
            ["header.subtitle"] = ("Install AmbiBack firmware on your device", "AmbiBack-Firmware auf das Gerät bringen"),
            ["mode.easy"] = ("Easy", "Einfach"),
            ["mode.advanced"] = ("Advanced", "Erweitert"),
            ["status.ready"] = ("Ready", "Bereit"),
            ["status.building"] = ("Building {0}...", "{0} wird gebaut ..."),
            ["status.buildOk"] = ("Build finished: {0} {1}", "Build fertig: {0} {1}"),
            ["status.buildFail"] = ("Build failed", "Build fehlgeschlagen"),
            ["status.flashing"] = ("Flashing {0} {1}...", "{0} {1} wird geflasht ..."),
            ["status.flashOk"] = ("Flash finished", "Flashen fertig"),
            ["status.flashFail"] = ("Flash failed", "Flashen fehlgeschlagen"),
            ["status.uploading"] = ("Uploading...", "Wird hochgeladen ..."),
            ["port.none"] = ("Not connected", "Nicht verbunden"),
            ["port.connected"] = ("Connected: {0}", "Verbunden: {0}"),

            // Easy mode
            ["easy.step1"] = ("1   Choose your device", "1   Gerät auswählen"),
            ["easy.step2"] = ("2   USB port", "2   USB-Anschluss"),
            ["easy.step3"] = ("3   Firmware", "3   Firmware"),
            ["easy.noDevice"] = ("No device selected", "Kein Gerät ausgewählt"),
            ["easy.wifi"] = ("Set up Wi-Fi after installing", "Nach der Installation WLAN einrichten"),
            ["easy.ssid"] = ("Network name (SSID)", "Netzwerkname (SSID)"),
            ["easy.key"] = ("Password", "Passwort"),
            ["easy.flash"] = ("Install Firmware", "Firmware installieren"),
            ["easy.details"] = ("Show details (Advanced mode)", "Details anzeigen (erweiterter Modus)"),
            ["easy.noPort"] = ("No USB port found. Connect the device.", "Kein USB-Anschluss gefunden. Gerät anschließen."),
            ["easy.buildLatest"] = ("Newest version from GitHub (build now)", "Neueste Version von GitHub (jetzt bauen)"),
            ["easy.existing"] = ("Version {0} (ready to install)", "Version {0} (bereit zur Installation)"),
            ["easy.hint"] = ("Connect the device with a USB data cable.", "Gerät mit einem USB-Datenkabel anschließen."),
            ["easy.prepare"] = ("Setting up tools. The first time this takes several minutes.", "Werkzeuge werden eingerichtet. Beim ersten Mal dauert das einige Minuten."),
            ["easy.source"] = ("Downloading the firmware source...", "Firmware-Quellcode wird geladen ..."),
            ["easy.build"] = ("Building the firmware...", "Firmware wird gebaut ..."),
            ["easy.flashing"] = ("Installing the firmware. Keep the device connected.", "Firmware wird installiert. Gerät nicht trennen."),
            ["easy.wifiSend"] = ("Sending the Wi-Fi settings...", "WLAN-Daten werden gesendet ..."),
            ["easy.done"] = ("Done. Firmware {0} is installed.", "Fertig. Firmware {0} ist installiert."),
            ["easy.doneWifi"] = ("Done. Firmware {0} is installed and the Wi-Fi settings were sent.", "Fertig. Firmware {0} ist installiert, die WLAN-Daten wurden gesendet."),
            ["easy.errNoDevice"] = ("Choose a device first.", "Zuerst ein Gerät auswählen."),
            ["easy.errNoPort"] = ("Choose the USB port of the device.", "USB-Anschluss des Geräts auswählen."),
            ["easy.errBuild"] = ("The build failed. The details are in Advanced mode.", "Der Build ist fehlgeschlagen. Details stehen im erweiterten Modus."),
            ["easy.errFlash"] = ("Installing failed. Check the cable and the USB port, then try again.", "Installation fehlgeschlagen. Kabel und USB-Anschluss prüfen und erneut versuchen."),
            ["easy.errSetup"] = ("Setting up the tools failed: {0}", "Einrichten der Werkzeuge fehlgeschlagen: {0}"),
            ["easy.errSource"] = ("The firmware source could not be downloaded. Check the internet connection.", "Der Firmware-Quellcode konnte nicht geladen werden. Internetverbindung prüfen."),
            ["easy.errNoSsid"] = ("Enter the network name, or turn off the Wi-Fi setup.", "Netzwerknamen eingeben oder die WLAN-Einrichtung ausschalten."),

            // Advanced mode
            ["grp.connection"] = ("Connection", "Verbindung"),
            ["grp.firmware"] = ("Firmware", "Firmware"),
            ["grp.deviceInfo"] = ("Connected Device", "Verbundenes Gerät"),
            ["grp.control"] = ("Device Control", "Gerätesteuerung"),
            ["grp.wifi"] = ("Wi-Fi", "WLAN"),
            ["grp.output"] = ("Output and Serial Monitor", "Ausgabe und serieller Monitor"),
            ["lbl.port"] = ("USB port", "USB-Anschluss"),
            ["lbl.baud"] = ("Baud rate", "Baudrate"),
            ["btn.connect"] = ("Connect", "Verbinden"),
            ["btn.disconnect"] = ("Disconnect", "Trennen"),
            ["btn.getInfo"] = ("Get Info", "Info abrufen"),
            ["btn.restart"] = ("Restart", "Neustart"),
            ["lbl.device"] = ("Device", "Gerät"),
            ["lbl.image"] = ("Image", "Image"),
            ["lbl.source"] = ("Source", "Quellcode"),
            ["lbl.builder"] = ("Builder", "Builder"),
            ["lbl.core"] = ("ESP32 core", "ESP32-Core"),
            ["btn.setup"] = ("Set Up Tools...", "Werkzeuge..."),
            ["chk.coreDebug"] = ("ESP32 core: verbose debug log", "ESP32-Core: ausführliches Debug-Log"),
            ["chk.verbose"] = ("AmbiBack: verbose debug log", "AmbiBack: ausführliches Debug-Log"),
            ["chk.diag"] = ("AmbiBack: memory diagnostics", "AmbiBack: Speicherdiagnose"),
            ["btn.build"] = ("Build", "Bauen"),
            ["btn.buildFlash"] = ("Build and Flash", "Bauen + Flashen"),
            ["btn.flash"] = ("Flash", "Flashen"),
            ["btn.clean"] = ("Clean", "Build leeren"),
            ["btn.buildUpload"] = ("Build and Upload", "Bauen + Hochladen"),
            ["btn.upload"] = ("Upload Image", "Hochladen"),
            ["lbl.fwType"] = ("Firmware / type", "Firmware / Typ"),
            ["lbl.idMac"] = ("ID / MAC", "ID / MAC"),
            ["lbl.name"] = ("Name", "Name"),
            ["lbl.ip"] = ("IP address", "IP-Adresse"),
            ["lbl.ledMode"] = ("LED mode", "LED-Modus"),
            ["btn.off"] = ("Off", "Aus"),
            ["btn.demo"] = ("Demo", "Demo"),
            ["btn.camera"] = ("Camera", "Kamera"),
            ["btn.ledTest"] = ("Send LED Test Data", "LED-Testdaten senden"),
            ["lbl.relay"] = ("Relay ID", "Relay-ID"),
            ["btn.setRelay"] = ("Set Relay", "Relay setzen"),
            ["btn.factory"] = ("Reset to Factory Defaults...", "Auf Werkseinstellungen zurücksetzen..."),
            ["lbl.ssid"] = ("Network name", "Netzwerkname"),
            ["lbl.key"] = ("Password", "Passwort"),
            ["btn.sendWifi"] = ("Send Wi-Fi", "WLAN senden"),
            ["btn.resetWifi"] = ("Reset Wi-Fi", "WLAN zurücksetzen"),
            ["log.clear"] = ("Clear", "Leeren"),
            ["log.copy"] = ("Copy All", "Alles kopieren"),
            ["tip.image"] = ("Built image in the firmware folder that is flashed", "Gebautes Image im Firmware-Ordner, das geflasht wird"),
            ["tip.source"] = ("Version in the firmware source (globalincludes.h)", "Version im Firmware-Quellcode (globalincludes.h)"),
            ["tip.builder"] = ("arduino-cli is the current Arduino build tool. arduino-builder needs the Arduino IDE 1.8.x.", "arduino-cli ist das aktuelle Build-Werkzeug von Arduino. arduino-builder braucht die Arduino IDE 1.8.x."),
            ["tip.upload"] = ("Uploads the image to the update server (needs scripts\\upload-firmware.ps1 and .env)", "Lädt das Image auf den Update-Server (braucht scripts\\upload-firmware.ps1 und .env)"),

            // Dialogs
            ["confirm.factory.title"] = ("Reset to factory defaults?", "Auf Werkseinstellungen zurücksetzen?"),
            ["confirm.factory.text"] = ("All settings on the connected device are deleted, including the Wi-Fi settings. This cannot be undone.\r\n\r\nReset the device?", "Alle Einstellungen auf dem verbundenen Gerät werden gelöscht, auch die WLAN-Daten. Das lässt sich nicht rückgängig machen.\r\n\r\nGerät zurücksetzen?"),
            ["dlg.noEnv"] = ("No ESP32 core is installed for {0}.\r\n\r\nSet up the tools now?", "Für {0} ist kein ESP32-Core installiert.\r\n\r\nWerkzeuge jetzt einrichten?"),
            ["dlg.noEnv.title"] = ("Tools missing", "Werkzeuge fehlen"),
            ["dlg.noSource"] = ("The firmware source was not found in:\r\n{0}\r\n\r\nDownload it from GitHub now?", "Der Firmware-Quellcode wurde nicht gefunden in:\r\n{0}\r\n\r\nJetzt von GitHub laden?"),
            ["dlg.noSource.title"] = ("Firmware source missing", "Firmware-Quellcode fehlt"),
            ["dlg.noImage"] = ("No image found for {0} {1}. Build the firmware first.", "Kein Image für {0} {1} gefunden. Zuerst die Firmware bauen."),
            ["dlg.noEsptool"] = ("esptool.exe was not found. Set up an ESP32 core with Tools > Set Up Arduino and ESP32.", "esptool.exe wurde nicht gefunden. Unter Werkzeuge > Arduino und ESP32 einrichten einen ESP32-Core installieren."),
            ["dlg.about"] = ("AmbiBack USB Flasher {0}\r\n\r\nBuilds and installs the AmbiBack firmware on ESP32 devices.\r\n\r\nData folder:\r\n{1}\r\n\r\nLicense: GPL-3.0", "AmbiBack USB Flasher {0}\r\n\r\nBaut die AmbiBack-Firmware und installiert sie auf ESP32-Geräten.\r\n\r\nDatenordner:\r\n{1}\r\n\r\nLizenz: GPL-3.0"),
            ["dlg.busy"] = ("Please wait until the current task has finished.", "Bitte warten, bis die laufende Aufgabe fertig ist."),

            // Setup dialog
            ["setup.title"] = ("Set Up Arduino and ESP32", "Arduino und ESP32 einrichten"),
            ["setup.intro"] = ("Downloads the tools needed to build the firmware. Everything is stored in:", "Lädt die Werkzeuge, die zum Bauen der Firmware nötig sind. Alles wird hier gespeichert:"),
            ["setup.builder"] = ("Builder", "Builder"),
            ["setup.cli"] = ("arduino-cli (recommended)", "arduino-cli (empfohlen)"),
            ["setup.ide"] = ("Arduino IDE 1.8.x (arduino-builder)", "Arduino IDE 1.8.x (arduino-builder)"),
            ["setup.both"] = ("Install both", "Beide installieren"),
            ["setup.core"] = ("ESP32 core", "ESP32-Core"),
            ["setup.libs"] = ("Firmware libraries ({0}, fixed versions from GitHub)", "Firmware-Bibliotheken ({0}, feste Versionen von GitHub)"),
            ["setup.source"] = ("Firmware source (branch or tag)", "Firmware-Quellcode (Branch/Tag)"),
            ["setup.useDefault"] = ("Use the downloaded tools as default", "Geladene Werkzeuge als Standard verwenden"),
            ["setup.install"] = ("Install", "Installieren"),
            ["setup.cancel"] = ("Cancel", "Abbrechen"),
            ["setup.close"] = ("Close", "Schließen"),
            ["setup.openFolder"] = ("Open Folder", "Ordner öffnen"),
            ["setup.loading"] = ("Loading versions from GitHub...", "Versionen werden von GitHub geladen ..."),
            ["setup.loaded"] = ("Versions loaded.", "Versionen geladen."),
            ["setup.offline"] = ("Could not load the version lists ({0}). Known versions are shown.", "Versionslisten konnten nicht geladen werden ({0}). Bekannte Versionen werden angezeigt."),
            ["setup.running"] = ("Installing...", "Wird installiert ..."),
            ["setup.done"] = ("Setup finished.", "Einrichtung abgeschlossen."),
            ["setup.failed"] = ("Setup failed: {0}", "Einrichtung fehlgeschlagen: {0}"),
            ["setup.canceled"] = ("Canceled.", "Abgebrochen."),

            // Settings dialog
            ["settings.title"] = ("Settings", "Einstellungen"),
            ["settings.binDir"] = ("Arduino IDE 1.8.x folder (arduino-builder)", "Ordner der Arduino IDE 1.8.x (arduino-builder)"),
            ["settings.sourceDir"] = ("Firmware source folder", "Firmware-Quellordner"),
            ["settings.sourceFile"] = ("Sketch file", "Sketch-Datei"),
            ["settings.libDir"] = ("Library folder (contains \"libraries\")", "Bibliotheksordner (enthält \"libraries\")"),
            ["settings.rootDir"] = ("Data folder (firmware, buildenv, buildtools, tools)", "Datenordner (firmware, buildenv, buildtools, tools)"),
            ["settings.buildDir"] = ("Build folder (_build, _cache)", "Build-Ordner (_build, _cache)"),
            ["settings.browse"] = ("Browse...", "Wählen..."),
            ["settings.hint"] = ("Empty fields use the default shown in grey.", "Leere Felder verwenden den grau angezeigten Standard."),
            ["settings.reset"] = ("Use Defaults", "Standardwerte"),
            ["settings.ok"] = ("OK", "OK"),
            ["settings.cancel"] = ("Cancel", "Abbrechen"),
        };
    }
}

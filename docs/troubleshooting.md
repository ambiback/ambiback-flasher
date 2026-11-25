# AmbiBack USB Flasher – Troubleshooting

**English** · [Deutsch](troubleshooting.de.md)

Most problems have to do with the USB connection. Start at the top. **Advanced** mode shows the full log; **Copy All** copies it for a bug report.

## Contents

- [No USB port is shown](#no-usb-port-is-shown)
- [USB drivers](#usb-drivers)
- [Installing (flashing) fails](#installing-flashing-fails)
- [Boot mode by hand](#boot-mode-by-hand)
- [Setting up the tools or building fails](#setting-up-the-tools-or-building-fails)
- [The device does not join the Wi-Fi](#the-device-does-not-join-the-wi-fi)
- [Serial monitor shows nothing or garbage](#serial-monitor-shows-nothing-or-garbage)
- [Windows warns about the program](#windows-warns-about-the-program)
- [Reporting a problem](#reporting-a-problem)

## No USB port is shown

Easy mode says *No USB port found. Connect the device.*

1. **Use a data cable.** Many USB cables only charge. Try another cable, ideally one that you know transfers data.
2. **Use another USB port** on the PC, directly, without a hub.
3. **Open the Device Manager** (right-click the Start button > **Device Manager**) and look under **Ports (COM & LPT)** while you plug the device in.
   - A new **COM** entry appears: the port works. The flasher picks it up within a few seconds.
   - An entry with a warning sign appears under **Other devices**, for example *USB Serial* or *CP2102 USB to UART*: the driver is missing, see [USB drivers](#usb-drivers).
   - Nothing changes: cable, USB port or device. Try another cable first.
4. When several COM ports are listed, unplug the device, note the list, plug it in again: the new entry is the device.

## USB drivers

The ESP32 boards use a USB-to-serial chip. Windows 10 and 11 install drivers for many of them by themselves; if not, install the driver of the chip manufacturer:

| Chip (name in Device Manager) | Driver |
|---|---|
| FTDI (FT231X, FT232R …) | [FTDI VCP Drivers](https://ftdichip.com/drivers/vcp-drivers/) |
| Silicon Labs CP210x (CP2102, CP2104 …) | [Silicon Labs CP210x Drivers](https://www.silabs.com/developers/usb-to-uart-bridge-vcp-drivers) |
| WCH CH340, CH341, CH9102 | [WCH CH341SER](https://www.wch-ic.com/downloads/CH341SER_EXE.html) |

Which chip a board uses varies, even between batches of the same board. Typical examples: generic ESP32 dev boards use CP2102 or CH340, LilyGO TTGO boards CP210x or CH9102, older M5Stack devices FTDI and newer ones CH9102. The name in the Device Manager or the chip on the board tells you for sure.

After installing a driver, unplug the device and plug it in again.

## Installing (flashing) fails

Easy mode says *Installing failed. Check the cable and the USB port, then try again.* The log in Advanced mode shows the esptool message:

| Message in the log | What to do |
|---|---|
| `could not open port` / `Access is denied` | Another program uses the port: a serial monitor, the Arduino IDE, AmbiBack PC, Cura, a 3D printer tool. Close it, or click **Disconnect** in the flasher. |
| `Failed to connect to ESP32: No serial data received` / `Wrong boot mode detected` | The device did not switch to download mode by itself. See [Boot mode by hand](#boot-mode-by-hand). |
| `A fatal error occurred: … timed out` during writing | The connection broke. Use a shorter cable, a port directly on the PC, no hub. Try again. |
| `No image found for …` | The selected image does not exist. Choose **Newest version from GitHub (build now)**, or build in Advanced mode. |
| `esptool.exe was not found` | The ESP32 core is not installed. Run **Tools > Set Up Arduino and ESP32...**. |

Flashing again is safe. A device that stopped in the middle of flashing does not start, but it can always be flashed again over USB.

## Boot mode by hand

Normally esptool resets the ESP32 into download mode through the USB chip. Some boards and cables do not support this. Then:

1. Find the **BOOT** (or **IO0**) button on the board.
2. Hold **BOOT**, press and release **RESET** (or **EN**), then release **BOOT**. Without a RESET button: hold **BOOT** while plugging in the USB cable.
3. Click **Install Firmware** (or **Flash**) right away.
4. After flashing, press **RESET** or unplug and plug in the device.

M5Stack Atom devices have no BOOT button; they normally switch automatically. If not, try another cable or USB port.

## Setting up the tools or building fails

- **First start takes long.** Setting up downloads several hundred MB; the status line shows progress. Wait until it finishes.
- **No internet or GitHub not reachable.** The message says *Setting up the tools failed* or *The firmware source could not be downloaded*. Check the connection, a proxy or a firewall, and try again. Downloaded archives are kept in the `downloads` folder and reused.
- **Not enough disk space.** Free about 1 GB on the drive of the [data folder](user-guide.md#settings-data-folder-and-language).
- **Antivirus.** Some virus scanners block or slow down the compiler. Add the data folder as an exception if builds fail with access errors.
- **The build fails.** Easy mode says *The build failed*. Open Advanced mode and read the first red line of the log. Then try:
  1. **Clean** in Advanced mode, build again.
  2. **Tools > Set Up Arduino and ESP32...** with the default versions (ESP32 core 3.3.6, firmware source `main`).
  3. If a custom firmware source folder is set in **File > Settings...**, clear the field to use the downloaded source.

## The device does not join the Wi-Fi

- ESP32 devices only use **2.4 GHz**. A router that offers 2.4 GHz and 5 GHz under one name usually works; a 5 GHz-only network does not.
- Network name and password are case-sensitive. Send them again in Advanced mode: **Connect**, enter both, **Send Wi-Fi**, then **Get Info** after a few seconds.
- An IP address of `0.0.0.0` or an empty field means the device is not connected yet. Watch the serial monitor for Wi-Fi messages.
- To start over, click **Reset Wi-Fi** and send the settings again.

## Serial monitor shows nothing or garbage

- Choose **115200** as baud rate and click **Connect** again.
- Click **Restart** to see the start messages of the device.
- Empty **Connected Device** fields after **Get Info**: the device runs no AmbiBack firmware, or a very old one. Install the firmware first.

## Windows warns about the program

The setup and the program are not code-signed. SmartScreen may show *Windows protected your PC*: click **More info**, then **Run anyway**. Download the files only from the [Releases page](https://github.com/ambiback/ambiback-flasher/releases).

## Reporting a problem

Open an issue on [GitHub](https://github.com/ambiback/ambiback-flasher/issues) with:

- the flasher version (**Help > About**),
- the device type and, if known, the USB chip,
- what you clicked and the message you saw,
- the log from Advanced mode (**Copy All**). Remove your Wi-Fi name if you do not want to share it.

There is no product support; answers come from the community when time allows.

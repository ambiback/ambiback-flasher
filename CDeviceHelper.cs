using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmbiBack_USB_Flasher
{
    class CDeviceHelper
    {
        public static ArrayList aDeviceTypes = null;

        /// <summary>Map 2.0.0-style board IDs to 3.3.6-style for new ESP32 core. Key=2.0.0 model, Value=(3.3.6 model, 3.3.6 board).</summary>
        private static readonly Dictionary<string, (string Model, string Board)> _boardMapNewEnv = new Dictionary<string, (string, string)>(StringComparer.OrdinalIgnoreCase)
        {
            { "esp32", ("esp32", "ESP32_DEV") },
            { "m5stick-c", ("m5stack_stickc", "M5STACK_STICKC") },
            { "m5stack-timer-cam", ("m5stack_timer_cam", "M5STACK_TIMER_CAM") },
            { "m5stack-fire", ("m5stack_fire", "M5STACK_FIRE") },
            { "m5stack-atom", ("m5stack_atom", "M5STACK_ATOM") },
            { "m5stack-core2", ("m5stack_core2", "M5STACK_CORE2") },
            { "m5stack-coreink", ("m5stack_coreink", "M5STACK_COREINK") },
            { "m5stack-core-esp32", ("m5stack_core", "M5STACK_CORE") },
        };

        /// <summary>Resolve board ID and ARDUINO_BOARD for 3.3.6+ build env. Returns (model, board) for FQBN and preprocessor. UseLegacy=false = 3.3.6 style.</summary>
        public static (string Esp32Model, string Esp32Board) ResolveBoardForBuildEnv(string esp32Model, string esp32Board, bool useLegacyToolchain)
        {
            if (useLegacyToolchain)
                return (esp32Model, esp32Board);
            if (_boardMapNewEnv.TryGetValue(esp32Model, out var mapped))
                return mapped;
            // Fallback: hyphen→underscore for model, uppercase+underscore for board
            string model = esp32Model?.Replace("-", "_") ?? esp32Model;
            string board = string.IsNullOrEmpty(esp32Board) ? "" : esp32Board.Replace("-", "_").ToUpperInvariant();
            return (model, board);
        }
        public static void InitDevices()
        {
            CDeviceHelper.aDeviceTypes = new ArrayList();
            CDeviceHelper.aDeviceTypes.Add(new CDeviceType("AmbiBack Bridge", "bridge", "ESP32_BRIDGE", "esp32", "ESP32_DEV", "CPUFreq=240,FlashMode=qio,FlashFreq=80,FlashSize=4M,", 921600, false, false));
            CDeviceHelper.aDeviceTypes.Add(new CDeviceType("AmbiBack LED V1", "ambiledv1", "ESP32_AMBIBACK_LEDV1", "m5stick-c", "M5Stick_C", "", 1500000, false, false));

            CDeviceHelper.aDeviceTypes.Add(new CDeviceType("AmbiBack Naked ESP32", "naked", "ESP32_NAKED", "esp32", "ESP32_DEV", "CPUFreq=240,FlashMode=qio,FlashFreq=80,FlashSize=4M,", 921600, false, false));
            CDeviceHelper.aDeviceTypes.Add(new CDeviceType("AmbiBack Wroover Cam", "wroover", "ESP32_WROOVER_CAM", "esp32", "ESP32_DEV", "CPUFreq=240,FlashMode=qio,FlashFreq=80,FlashSize=4M,", 921600, true, false));
            CDeviceHelper.aDeviceTypes.Add(new CDeviceType("AmbiBack TTGO Cam", "ttgo", "ESP32_TTGO_CAM", "esp32", "ESP32_DEV", "CPUFreq=240,FlashMode=qio,FlashFreq=80,FlashSize=4M,", 921600, true, false));
            CDeviceHelper.aDeviceTypes.Add(new CDeviceType("AmbiBack TTGO Cam + Mic", "ttgotm", "ESP32_TTGO_CAM_MIC", "esp32", "ESP32_DEV", "CPUFreq=240,FlashMode=qio,FlashFreq=80,FlashSize=4M,", 921600, true, false));
            CDeviceHelper.aDeviceTypes.Add(new CDeviceType("AmbiBack TTGO Journal", "ttgojr", "ESP32_TTGO_CAM_JOURNAL", "esp32", "ESP32_DEV", "CPUFreq=240,FlashMode=qio,FlashFreq=80,FlashSize=4M,", 921600, false, false));
            CDeviceHelper.aDeviceTypes.Add(new CDeviceType("AmbiBack TTGO Cam Plus (Mic)", "ttgopl", "ESP32_TTGO_CAM_PLUS", "esp32", "ESP32_DEV", "CPUFreq=240,FlashMode=qio,FlashFreq=80,FlashSize=4M,", 921600, true, false));
            CDeviceHelper.aDeviceTypes.Add(new CDeviceType("AmbiBack M5Stack esp32cam", "m5esp32", "ESP32_M5ST_CAM", "esp32", "ESP32_DEV", "CPUFreq=240,FlashMode=qio,FlashFreq=80,FlashSize=4M,", 921600, false, false));
            CDeviceHelper.aDeviceTypes.Add(new CDeviceType("AmbiBack M5Stack Camera X(Norm/Wide) OV2640", "m5camx", "ESP32_M5ST_CAM_X", "esp32", "ESP32_DEV", "CPUFreq=240,FlashMode=qio,FlashFreq=80,FlashSize=4M,", 921600, true, false));
            CDeviceHelper.aDeviceTypes.Add(new CDeviceType("AmbiBack M5Stack Camera Timer X(Norm/Wide) OV3660", "m5camtx", "ESP32_M5ST_CAM_TIMER_X", "m5stack-timer-cam", "M5Stack-Timer-CAM", "", 1500000, true, false));
            CDeviceHelper.aDeviceTypes.Add(new CDeviceType("AmbiBack M5Stack Atom Lite", "m5alit", "ESP32_M5ST_ATOMLITE", "m5stick-c", "M5Stick_C", "", 1500000, false, false));
            CDeviceHelper.aDeviceTypes.Add(new CDeviceType("AmbiBack M5Stack Atom Matrix", "m5almx", "ESP32_M5ST_ATOMMATRIX", "m5stick-c", "M5Stick_C", "", 1500000, false, false));
        }
    }

   [Serializable]
    public class CDeviceType
    {
        private string szDisplayLabel;
        private string szDeviceType;
        private string szPreprocessorName;
        private string szEsp32Model;
        private string szEsp32Board;
        private string szEsp32FqbnExtra;
        private int iUploadSpeed;
        private bool bHasPSRam;
        private bool bUsePSRamFix;

        public CDeviceType(string szNewDisplayLabel, string szNewDeviceType, string szNewPreprocessorName, string szNewEsp32Model, string szNewEsp32Board, string szNewEsp32FqbnExtra, int iNewUploadSpeed, bool newbHasPSRam, bool newbUsePSRamFix)
        {

            this.szDisplayLabel = szNewDisplayLabel;
            this.szDeviceType = szNewDeviceType;
            this.szPreprocessorName = szNewPreprocessorName;
            this.szEsp32Model = szNewEsp32Model;
            this.szEsp32Board = szNewEsp32Board;
            this.szEsp32FqbnExtra = szNewEsp32FqbnExtra;
            this.iUploadSpeed = iNewUploadSpeed;
            this.bHasPSRam = newbHasPSRam;
            this.bUsePSRamFix = newbUsePSRamFix;
        }

        public string DisplayLabel
        {
            get
            {
                return szDisplayLabel; 
            }
        }
        public string DeviceType
        {
            get
            {
                return szDeviceType;
            }
        }
        public string PreprocessorName
        {
            get
            {
                return szPreprocessorName;
            }
        }
        public string Esp32Model
        {
            get
            {
                return szEsp32Model;
            }
        }
        public string Esp32Board
        {
            get
            {
                return szEsp32Board;
            }
        }
        public string Esp32FqbnExtra
        {
            get
            {
                return szEsp32FqbnExtra;
            }
        }
        

        public int UploadSpeed
        {
            get
            {
                return iUploadSpeed;
            }
        }
        
        public bool HasPSRam
        {
            get
            {
                return bHasPSRam;
            }
        }
        public bool UsePSRamFix
        {
            get
            {
                return bUsePSRamFix;
            }
        }
        


        public override string ToString()
        {
            return this.szDisplayLabel; 
        }

        public CDeviceType Clone()
        {
            // Clone all members first
            CDeviceType clonedObject = new CDeviceType(this.szDisplayLabel, szDeviceType, szPreprocessorName, szEsp32Model, szEsp32Board, szEsp32FqbnExtra, iUploadSpeed, bHasPSRam, bUsePSRamFix);

            // return cloned object
            return clonedObject;
        }
    }
}

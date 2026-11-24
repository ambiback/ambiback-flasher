using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;

namespace AmbiBack_USB_Flasher
{
    /// <summary>
    /// Preview pictures of the device types. A PNG in &lt;data root&gt;\devices\&lt;type&gt;.png replaces the bundled
    /// picture in &lt;program folder&gt;\devices. Without a picture a neutral placeholder is drawn.
    /// </summary>
    public static class DeviceImages
    {
        private static readonly Dictionary<string, Image> _cache = new Dictionary<string, Image>(StringComparer.OrdinalIgnoreCase);

        public static Image Get(string deviceType)
        {
            if (string.IsNullOrEmpty(deviceType)) return null;
            if (_cache.TryGetValue(deviceType, out Image img)) return img;

            foreach (string dir in new[] { AppPaths.DeviceImageOverrideDir, AppPaths.BundledDeviceImageDir })
            {
                string file = Path.Combine(dir, deviceType + ".png");
                if (!File.Exists(file)) continue;
                try
                {
                    // Load through a copy so the file is not locked
                    using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    using (var tmp = Image.FromStream(fs))
                        img = new Bitmap(tmp);
                    break;
                }
                catch
                {
                    img = null;
                }
            }
            if (img == null) img = Placeholder(deviceType);
            _cache[deviceType] = img;
            return img;
        }

        /// <summary>Scaled copy that keeps the aspect ratio, centered on a transparent square.</summary>
        public static Image Thumbnail(string deviceType, int size)
        {
            Image src = Get(deviceType);
            var bmp = new Bitmap(size, size);
            using (var g = Graphics.FromImage(bmp))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                float scale = Math.Min((float)size / src.Width, (float)size / src.Height);
                float w = src.Width * scale, h = src.Height * scale;
                g.DrawImage(src, (size - w) / 2, (size - h) / 2, w, h);
            }
            return bmp;
        }

        private static Image Placeholder(string deviceType)
        {
            const int size = 256;
            var bmp = new Bitmap(size, size);
            using (var g = Graphics.FromImage(bmp))
            using (var path = new GraphicsPath())
            using (var fill = new SolidBrush(Theme.Surface))
            using (var pen = new Pen(Theme.Separator, 4))
            using (var font = new Font("Segoe UI Semibold", 40F, GraphicsUnit.Pixel))
            using (var textBrush = new SolidBrush(Theme.SecondaryText))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.Clear(Color.White);
                var r = new Rectangle(40, 56, 176, 144);
                int d = 28;
                path.AddArc(r.X, r.Y, d, d, 180, 90);
                path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
                path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
                path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
                path.CloseFigure();
                g.FillPath(fill, path);
                g.DrawPath(pen, path);
                var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                g.DrawString(deviceType.ToUpperInvariant(), font, textBrush, r, sf);
            }
            return bmp;
        }
    }
}

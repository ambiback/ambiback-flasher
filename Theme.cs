using System;
using System.Drawing;
using System.Windows.Forms;

namespace AmbiBack_USB_Flasher
{
    /// <summary>
    /// Small set of colors and helpers for a calm, light look with one accent color.
    /// The accent is the deep blue from the AmbiBack logo; it is only used for the main action.
    /// </summary>
    public static class Theme
    {
        // Accent: white text on it has a contrast of about 5.4:1
        public static readonly Color Accent = Color.FromArgb(0x0B, 0x6F, 0xA8);
        public static readonly Color AccentHover = Color.FromArgb(0x09, 0x5C, 0x8C);
        public static readonly Color AccentDisabled = Color.FromArgb(0xA9, 0xC6, 0xD8);

        public static readonly Color Surface = Color.FromArgb(0xF5, 0xF6, 0xF8);
        public static readonly Color Card = Color.White;
        public static readonly Color Separator = Color.FromArgb(0xDD, 0xE1, 0xE6);
        public static readonly Color SecondaryText = Color.FromArgb(0x5A, 0x63, 0x6E);   // 6.1:1 on white
        public static readonly Color Destructive = Color.FromArgb(0xB3, 0x26, 0x1E);     // 6.6:1 on white
        public static readonly Color Success = Color.FromArgb(0x1E, 0x7B, 0x34);

        // Log output (dark)
        public static readonly Color LogBack = Color.FromArgb(0x1E, 0x1F, 0x22);
        public static readonly Color LogText = Color.FromArgb(0xD8, 0xDA, 0xDE);
        public static readonly Color LogMuted = Color.FromArgb(0x9A, 0xA1, 0xAB);
        public static readonly Color LogInfo = Color.FromArgb(0x6C, 0xC4, 0xF5);
        public static readonly Color LogError = Color.FromArgb(0xF2, 0x8B, 0x82);
        public static readonly Color LogSuccess = Color.FromArgb(0x8B, 0xD3, 0x9A);

        public static readonly Font BaseFont = new Font("Segoe UI", 9F);
        public static readonly Font TitleFont = new Font("Segoe UI Semibold", 14F);
        public static readonly Font SectionFont = new Font("Segoe UI Semibold", 10.5F);
        public static readonly Font BigButtonFont = new Font("Segoe UI Semibold", 11F);
        public static readonly Font LogFont = new Font("Consolas", 9F);

        /// <summary>Map the colors used by the log code to readable colors on the dark log background.</summary>
        public static Color MapLogColor(Color c)
        {
            if (c == Color.Red) return LogError;
            if (c == Color.Gray) return LogMuted;
            if (c == Color.DarkCyan) return LogInfo;
            if (c == Color.Green || c == Color.DarkGreen) return LogSuccess;
            return c;
        }

        /// <summary>Style a button as the one prominent action of its area.</summary>
        public static void StylePrimary(Button b)
        {
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.FlatAppearance.MouseOverBackColor = AccentHover;
            b.FlatAppearance.MouseDownBackColor = AccentHover;
            b.ForeColor = Color.White;
            b.UseVisualStyleBackColor = false;
            b.Cursor = Cursors.Hand;
            EventHandler update = (s, e) => b.BackColor = b.Enabled ? Accent : AccentDisabled;
            b.EnabledChanged += update;
            update(b, EventArgs.Empty);
        }

        /// <summary>Style a toggle (RadioButton with button appearance) for the Easy / Advanced switch.</summary>
        public static void StyleSegment(RadioButton rb)
        {
            rb.Appearance = Appearance.Button;
            rb.FlatStyle = FlatStyle.Flat;
            rb.FlatAppearance.BorderColor = Separator;
            rb.FlatAppearance.CheckedBackColor = Accent;
            rb.FlatAppearance.MouseOverBackColor = Color.FromArgb(0xE8, 0xEE, 0xF3);
            rb.TextAlign = ContentAlignment.MiddleCenter;
            rb.Cursor = Cursors.Hand;
            EventHandler update = (s, e) => rb.ForeColor = rb.Checked ? Color.White : Color.FromArgb(0x22, 0x26, 0x2B);
            rb.CheckedChanged += update;
            update(rb, EventArgs.Empty);
        }
    }
}

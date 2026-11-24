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

        // Border of secondary buttons and cards: 3:1 against white for the buttons (non-text contrast)
        public static readonly Color ControlBorder = Color.FromArgb(0x93, 0x9B, 0xA6);
        public static readonly Color HoverBack = Color.FromArgb(0xEA, 0xF1, 0xF7);
        public static readonly Color PressedBack = Color.FromArgb(0xD9, 0xE6, 0xF0);
        public static readonly Color PrimaryText = Color.FromArgb(0x22, 0x26, 0x2B);
        public static readonly Font CardTitleFont = new Font("Segoe UI Semibold", 9F);

        /// <summary>Style a normal (secondary) button: white, thin border, light blue on hover.</summary>
        public static void StyleSecondary(Button b)
        {
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 1;
            b.FlatAppearance.BorderColor = ControlBorder;
            b.FlatAppearance.MouseOverBackColor = HoverBack;
            b.FlatAppearance.MouseDownBackColor = PressedBack;
            b.BackColor = Card;
            if (b.ForeColor == SystemColors.ControlText) b.ForeColor = PrimaryText;
            b.UseVisualStyleBackColor = false;
            b.Cursor = Cursors.Hand;
        }

        /// <summary>Style a button whose action cannot be undone (red text, secondary look).</summary>
        public static void StyleDestructive(Button b)
        {
            StyleSecondary(b);
            b.ForeColor = Destructive;
        }

        /// <summary>Path of a rectangle with rounded corners.</summary>
        public static System.Drawing.Drawing2D.GraphicsPath RoundedRect(Rectangle r, int radius)
        {
            int d = radius * 2;
            var p = new System.Drawing.Drawing2D.GraphicsPath();
            p.AddArc(r.X, r.Y, d, d, 180, 90);
            p.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            p.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            p.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            p.CloseFigure();
            return p;
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

    /// <summary>
    /// GroupBox drawn as a white card with rounded corners, a light border and a semibold title,
    /// instead of the etched frame. Layout and child handling stay those of the GroupBox.
    /// </summary>
    public class CardGroupBox : GroupBox
    {
        public CardGroupBox()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
            BackColor = Theme.Card;
        }

        /// <summary>Space below the card that shows the parent background (for cards docked on top of each other).</summary>
        [System.ComponentModel.DefaultValue(0)]
        public int BottomGap { get; set; }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(Parent != null ? Parent.BackColor : Theme.Surface);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            using (var path = Theme.RoundedRect(new Rectangle(0, 0, Width - 1, Height - 1 - BottomGap), 6))
            using (var fill = new SolidBrush(Theme.Card))
            using (var pen = new Pen(Theme.Separator))
            {
                g.FillPath(fill, path);
                g.DrawPath(pen, path);
            }
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
            TextRenderer.DrawText(g, Text, Theme.CardTitleFont, new Point(9, 3),
                Enabled ? Theme.PrimaryText : Theme.SecondaryText, TextFormatFlags.NoPrefix);
        }
    }

    /// <summary>Flat tool strip: plain background, no border or gradient.</summary>
    public class FlatToolStripRenderer : ToolStripSystemRenderer
    {
        protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
        {
            using (var b = new SolidBrush(e.ToolStrip.BackColor))
                e.Graphics.FillRectangle(b, e.AffectedBounds);
        }

        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
        {
            using (var p = new Pen(Theme.Separator))
                e.Graphics.DrawLine(p, 0, e.ToolStrip.Height - 1, e.ToolStrip.Width, e.ToolStrip.Height - 1);
        }

        protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
        {
            if (!e.Item.Selected && !e.Item.Pressed) return;
            using (var b = new SolidBrush(e.Item.Pressed ? Theme.PressedBack : Theme.HoverBack))
                e.Graphics.FillRectangle(b, new Rectangle(Point.Empty, e.Item.Size));
        }
    }
}

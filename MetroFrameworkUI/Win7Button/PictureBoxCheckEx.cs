using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using System.Windows.Forms;

using MetroFramework.Components;
using MetroFramework.Drawing;
using MetroFramework.Interfaces;

namespace MetroFramework.Win7Button
{
    /// <summary>
    /// 带选择功能的pictureBox
    /// </summary>
    [ToolboxBitmap(typeof(PictureBox))]
    public class PictureBoxCheckEx : PictureBox, IMetroControl
    {
        #region check text
        public bool Checked {
            get {
                return checkState != CheckState.Unchecked;
            }
            set {
                if (value != Checked) {
                    CheckState = value ? CheckState.Checked : CheckState.Unchecked;
                    Invalidate();
                }
            }
        }
        private CheckState checkState = CheckState.Unchecked;
        /// <summary>
        /// 选中或者没选中 2种状态
        /// </summary>
        public CheckState CheckState {
            get {
                return checkState;
            }
            set {
                // valid values are 0-2 inclusive.
                if (checkState != value) {
                    bool oldChecked = Checked;
                    checkState = value;
                    Invalidate();
                }
            }
        }
        /// <summary>
        /// 文本绘制方式
        /// </summary>
        public ContentAlignment TextAlign {
            get { return textAlign; }
            set {
                if (value != textAlign) {
                    textAlign = value;
                    Invalidate();
                }
            }
        }
        ContentAlignment textAlign = ContentAlignment.TopLeft;
        /// <summary>
        /// 显示的文本
        /// </summary>
        public string Text {
            get { return text; }
            set {
                if (value != text) {
                    text = value;
                    Invalidate();
                }
            }
        }
        string text = "";
        #endregion

        #region Interface

        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public event EventHandler<MetroPaintEventArgs> CustomPaintBackground;
        protected virtual void OnCustomPaintBackground(MetroPaintEventArgs e) {
            if (GetStyle(ControlStyles.UserPaint) && CustomPaintBackground != null) {
                CustomPaintBackground(this, e);
            }
        }

        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public event EventHandler<MetroPaintEventArgs> CustomPaint;
        protected virtual void OnCustomPaint(MetroPaintEventArgs e) {
            if (GetStyle(ControlStyles.UserPaint) && CustomPaint != null) {
                CustomPaint(this, e);
            }
        }

        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public event EventHandler<MetroPaintEventArgs> CustomPaintForeground;
        protected virtual void OnCustomPaintForeground(MetroPaintEventArgs e) {
            if (GetStyle(ControlStyles.UserPaint) && CustomPaintForeground != null) {
                CustomPaintForeground(this, e);
            }
        }

        private MetroColorStyle metroStyle = MetroColorStyle.Default;
        [Category(MetroDefaults.PropertyCategory.Appearance)]
        [DefaultValue(MetroColorStyle.Default)]
        public MetroColorStyle Style {
            get {
                if (DesignMode || metroStyle != MetroColorStyle.Default) {
                    return metroStyle;
                }

                if (StyleManager != null && metroStyle == MetroColorStyle.Default) {
                    return StyleManager.Style;
                }
                if (StyleManager == null && metroStyle == MetroColorStyle.Default) {
                    return MetroDefaults.Style;
                }

                return metroStyle;
            }
            set { metroStyle = value; }
        }

        private MetroThemeStyle metroTheme = MetroThemeStyle.Default;
        [Category(MetroDefaults.PropertyCategory.Appearance)]
        [DefaultValue(MetroThemeStyle.Default)]
        public MetroThemeStyle Theme {
            get {
                if (DesignMode || metroTheme != MetroThemeStyle.Default) {
                    return metroTheme;
                }

                if (StyleManager != null && metroTheme == MetroThemeStyle.Default) {
                    return StyleManager.Theme;
                }
                if (StyleManager == null && metroTheme == MetroThemeStyle.Default) {
                    return MetroDefaults.Theme;
                }

                return metroTheme;
            }
            set { metroTheme = value; }
        }

        private MetroStyleManager metroStyleManager = null;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MetroStyleManager StyleManager {
            get { return metroStyleManager; }
            set { metroStyleManager = value; }
        }

        private bool useCustomBackColor = false;
        [DefaultValue(false)]
        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public bool UseCustomBackColor {
            get { return useCustomBackColor; }
            set { useCustomBackColor = value; }
        }

        private bool useCustomForeColor = false;
        [DefaultValue(false)]
        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public bool UseCustomForeColor {
            get { return useCustomForeColor; }
            set { useCustomForeColor = value; }
        }

        private bool useStyleColors = false;
        [DefaultValue(false)]
        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public bool UseStyleColors {
            get { return useStyleColors; }
            set { useStyleColors = value; }
        }

        [Browsable(false)]
        [Category(MetroDefaults.PropertyCategory.Behaviour)]
        [DefaultValue(false)]
        public bool UseSelectable {
            get { return GetStyle(ControlStyles.Selectable); }
            set { SetStyle(ControlStyles.Selectable, value); }
        }

        #endregion

        #region Fields

        private bool displayFocusRectangle = false;
        [DefaultValue(false)]
        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public bool DisplayFocus {
            get { return displayFocusRectangle; }
            set { displayFocusRectangle = value; }
        }

        private MetroCheckBoxSize metroCheckBoxSize = MetroCheckBoxSize.Small;
        [DefaultValue(MetroCheckBoxSize.Small)]
        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public MetroCheckBoxSize FontSize {
            get { return metroCheckBoxSize; }
            set { metroCheckBoxSize = value; }
        }

        private MetroCheckBoxWeight metroCheckBoxWeight = MetroCheckBoxWeight.Regular;
        [DefaultValue(MetroCheckBoxWeight.Regular)]
        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public MetroCheckBoxWeight FontWeight {
            get { return metroCheckBoxWeight; }
            set { metroCheckBoxWeight = value; }
        }

        [Browsable(false)]
        public override Font Font {
            get {
                return base.Font;
            }
            set {
                base.Font = value;
            }
        }

        private bool isHovered = false;
        private bool isPressed = false;
        private bool isFocused = false;

        #endregion

        #region Constructor

        public PictureBoxCheckEx() {
            SetStyle(ControlStyles.SupportsTransparentBackColor |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.UserPaint, true);
        }

        #endregion

        #region Paint Methods

        //protected override void OnPaintBackground(PaintEventArgs e) {
        //    try {
        //        Color backColor = BackColor;

        //        if (!useCustomBackColor) {
        //            backColor = MetroPaint.BackColor.Form(Theme);
        //            if (Parent is MetroTile) {
        //                backColor = MetroPaint.GetStyleColor(Style);
        //            }
        //        }

        //        if (backColor.A == 255) {
        //            e.Graphics.Clear(backColor);
        //            return;
        //        }

        //        base.OnPaintBackground(e);

        //        OnCustomPaintBackground(new MetroPaintEventArgs(backColor, Color.Empty, e.Graphics));
        //    }
        //    catch {
        //        Invalidate();
        //    }
        //}
        /// <summary>
        /// 使用系统绘制
        /// </summary>
        public bool InBasePaintState = false;
        protected override void OnPaint(PaintEventArgs e) {
            if (InBasePaintState) {
                base.OnPaint(e);
                return;
            }
            try {
                if (GetStyle(ControlStyles.AllPaintingInWmPaint)) {
                    OnPaintBackground(e);
                }
                OnCustomPaint(new MetroPaintEventArgs(Color.Empty, Color.Empty, e.Graphics));
                OnPaintForeground(e);
            }
            catch {
                Invalidate();
            }
        }

        protected virtual void OnPaintForeground(PaintEventArgs e) {
            Color borderColor, foreColor;

            if (useCustomForeColor) {
                foreColor = ForeColor;

                if (isHovered && !isPressed && Enabled) {
                    borderColor = MetroPaint.BorderColor.CheckBox.Hover(Theme);
                }
                else if (isHovered && isPressed && Enabled) {
                    borderColor = MetroPaint.BorderColor.CheckBox.Press(Theme);
                }
                else if (!Enabled) {
                    borderColor = MetroPaint.BorderColor.CheckBox.Disabled(Theme);
                }
                else {
                    borderColor = MetroPaint.BorderColor.CheckBox.Normal(Theme);
                }
            }
            else {
                //if (isHovered && !isPressed && Enabled) {
                //    foreColor = MetroPaint.ForeColor.Tile.Hover(Theme);
                //}
                //else if (isHovered && isPressed && Enabled) {
                //    foreColor = MetroPaint.ForeColor.Tile.Press(Theme);
                //}
                //else if (!Enabled) {
                //    foreColor = MetroPaint.ForeColor.Tile.Disabled(Theme);
                //}
                //else {
                //    foreColor = MetroPaint.ForeColor.Tile.Normal(Theme);
                //}
                if (isHovered && !isPressed && Enabled) {
                    foreColor = MetroPaint.ForeColor.CheckBox.Hover(Theme);
                    borderColor = MetroPaint.BorderColor.CheckBox.Hover(Theme);
                }
                else if (isHovered && isPressed && Enabled) {
                    foreColor = MetroPaint.ForeColor.CheckBox.Press(Theme);
                    borderColor = MetroPaint.BorderColor.CheckBox.Press(Theme);
                }
                else if (!Enabled) {
                    foreColor = MetroPaint.ForeColor.CheckBox.Disabled(Theme);
                    borderColor = MetroPaint.BorderColor.CheckBox.Disabled(Theme);
                }
                else {
                    foreColor = !useStyleColors ? MetroPaint.ForeColor.CheckBox.Normal(Theme) : MetroPaint.GetStyleColor(Style);
                    borderColor = MetroPaint.BorderColor.CheckBox.Normal(Theme);
                }
            }
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            e.Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            if (this.Image != null) {
                //画图
                Rectangle imgDrawRec = new Rectangle(0, 24, ClientRectangle.Width, ClientRectangle.Height - 24);
                if (Checked && Enabled) {
                    GraphicsExtensions.DrawColoredImage(e.Graphics, this.Image, imgDrawRec, Color.White, true);
                    if (isHovered) {
                        GraphicsExtensions.DrawWin7ButtonStyle(4, e.Graphics, ClientRectangle, 100, true);
                    }
                    else {
                        GraphicsExtensions.DrawBorder(e.Graphics, ClientRectangle, Color.GreenYellow);
                    }
                }
                else if (isHovered && !isPressed && Enabled) {
                    //foreColor = MetroPaint.ForeColor.CheckBox.Hover(Theme);
                    //borderColor = MetroPaint.BorderColor.CheckBox.Hover(Theme);
                    GraphicsExtensions.DrawColoredImage(e.Graphics, this.Image, imgDrawRec, Color.WhiteSmoke, true);
                    GraphicsExtensions.DrawWin7ButtonStyle(4, e.Graphics, ClientRectangle, 80, true);
                    //GraphicsExtensions.DrawBorder(e.Graphics, ClientRectangle, borderColor);
                    //GraphicsExtensions.DrawGlow(e.Graphics, ClientRectangle,2,Color.White,2);
                }
                else if (isHovered && isPressed && Enabled) {
                    //foreColor = MetroPaint.ForeColor.CheckBox.Press(Theme);
                    //borderColor = MetroPaint.BorderColor.CheckBox.Press(Theme);
                    GraphicsExtensions.DrawColoredImage(e.Graphics, this.Image, imgDrawRec, borderColor, true);
                    GraphicsExtensions.DrawBorder(e.Graphics, ClientRectangle, borderColor);
                    GraphicsExtensions.DrawWin7ButtonStyle(5, e.Graphics, ClientRectangle, 100, true);
                    GraphicsExtensions.DrawMask(e.Graphics, ClientRectangle);
                }

                else if (!Enabled) {
                    //foreColor = MetroPaint.ForeColor.CheckBox.Disabled(Theme);
                    //borderColor = MetroPaint.BorderColor.CheckBox.Disabled(Theme);
                    GraphicsExtensions.DrawFadedImage(e.Graphics, this.Image, imgDrawRec, true);
                    GraphicsExtensions.DrawBorder(e.Graphics, ClientRectangle, borderColor);
                }
                else {
                    //foreColor = !useStyleColors ? MetroPaint.ForeColor.CheckBox.Normal(Theme) : MetroPaint.GetStyleColor(Style);
                    //borderColor = MetroPaint.BorderColor.CheckBox.Normal(Theme);
                    GraphicsExtensions.DrawCacScaleImage(e.Graphics, this.Image, imgDrawRec);
                }
            }
            GraphicsExtensions.DrawMask(e.Graphics, new Rectangle(16, 0, this.Width - 16, 32));
            //画选择框 要求图片尺寸大于12          
            using (Pen p = new Pen(borderColor)) {
                //Rectangle boxRect = new Rectangle(0, Height / 2 - 6, 12, 12);      
                Rectangle boxRect = new Rectangle(2, 2, 12, 12);
                e.Graphics.DrawRectangle(p, boxRect);
            }

            if (Checked) {
                Color fillColor = CheckState == CheckState.Indeterminate ? borderColor : MetroPaint.GetStyleColor(Style);
                using (SolidBrush b = new SolidBrush(fillColor)) {
                    Rectangle boxRect = new Rectangle(4, 4, 9, 9);
                    e.Graphics.FillRectangle(b, boxRect);
                }
            }

            //Rectangle textRect = new Rectangle(16, 0, Width - 16, Height);
            Rectangle textRect = new Rectangle(16, 0, Width - 16, 24);
            //if (!string.IsNullOrEmpty(Text)) {
            SizeF sf = e.Graphics.MeasureString("字高", MetroFonts.CheckBox(metroCheckBoxSize, metroCheckBoxWeight));
            textRect = new Rectangle(16, 0, Width - 16, Convert.ToInt32(sf.Height + 2) * 2);
            //}
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            TextRenderer.DrawText(e.Graphics, Text, MetroFonts.CheckBox(metroCheckBoxSize, metroCheckBoxWeight), textRect, foreColor, MetroPaint.GetTextFormatFlags(TextAlign));
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            OnCustomPaintForeground(new MetroPaintEventArgs(Color.Empty, foreColor, e.Graphics));

            if (displayFocusRectangle && isFocused)
                ControlPaint.DrawFocusRectangle(e.Graphics, ClientRectangle);


        }

        #endregion

        #region Focus Methods

        protected override void OnGotFocus(EventArgs e) {
            isFocused = true;
            isHovered = true;
            Invalidate();

            base.OnGotFocus(e);
        }

        protected override void OnLostFocus(EventArgs e) {
            isFocused = false;
            isHovered = false;
            isPressed = false;
            Invalidate();

            base.OnLostFocus(e);
        }

        protected override void OnEnter(EventArgs e) {
            isFocused = true;
            isHovered = true;
            Invalidate();

            base.OnEnter(e);
        }

        protected override void OnLeave(EventArgs e) {
            isFocused = false;
            isHovered = false;
            isPressed = false;
            Invalidate();

            base.OnLeave(e);
        }

        #endregion

        #region Keyboard Methods

        protected override void OnKeyDown(KeyEventArgs e) {
            if (e.KeyCode == Keys.Space) {
                isHovered = true;
                isPressed = true;
                Invalidate();
            }

            base.OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyEventArgs e) {
            //Remove this code cause this prevents the focus color
            //isHovered = false;
            //isPressed = false;
            Invalidate();

            base.OnKeyUp(e);
        }

        #endregion

        #region Mouse Methods

        protected override void OnMouseEnter(EventArgs e) {
            isHovered = true;
            Invalidate();

            base.OnMouseEnter(e);
        }

        protected override void OnMouseDown(MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                isPressed = true;
                Invalidate();
            }

            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e) {
            isPressed = false;
            Invalidate();
            if (this.Enabled) {//切换选中状态
                this.Checked = !Checked;
            }
            base.OnMouseUp(e);
        }

        protected override void OnMouseLeave(EventArgs e) {
            //This will check if control got the focus
            //If not thats the only it will remove the focus color
            if (!isFocused) {
                isHovered = false;
            }
            Invalidate();

            base.OnMouseLeave(e);
        }

        #endregion

        #region Overridden Methods

        protected override void OnEnabledChanged(EventArgs e) {
            base.OnEnabledChanged(e);
            Invalidate();
        }

        //protected override void OnCheckedChanged(EventArgs e) {
        //    base.OnCheckedChanged(e);
        //    Invalidate();
        //}

        //public override Size GetPreferredSize(Size proposedSize) {
        //    Size preferredSize;
        //    base.GetPreferredSize(proposedSize);

        //    using (var g = CreateGraphics()) {
        //        proposedSize = new Size(int.MaxValue, int.MaxValue);
        //        preferredSize = TextRenderer.MeasureText(g, Text, MetroFonts.CheckBox(metroCheckBoxSize, metroCheckBoxWeight), proposedSize, MetroPaint.GetTextFormatFlags(TextAlign));
        //        preferredSize.Width += 16;
        //    }

        //    return preferredSize;
        //}

        #endregion
    }

}
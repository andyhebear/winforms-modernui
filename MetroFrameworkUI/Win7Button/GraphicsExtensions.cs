using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace MetroFramework.Win7Button
{
    /// <summary>
    /// 9宫格图片绘制辅助 绘制图4个角不变形，中间部分拉伸绘制
    /// 绘制win7或者xp按钮 内部已经内置好了20张图
    /// win7图-6状态:尺寸11x126,每张0 0 11 21
    /// XP图-5状态:尺寸11x105,每张 0 0 11 17
    /// </summary>
    public static class GraphicsExtensions
    {
        #region Constants
        // adjust disabled image alpha level
        private const float FADE_LEVEL = .7F;
        // adjust mirror image alpha level
        private const float MIRROR_LEVEL = .15F;
        // glow color alpha
        private const int GLOW_LEVEL = 40;
        // alpha maximum before pixel is changed
        private const int GLOW_THRESHHOLD = 96;
        // glow padding
        private const int SIZE_OFFSET = 2;
        #endregion
        /// <summary>着色Draw a colored bitmap</summary>        
        public static void DrawColoredImage(Graphics g, Image img, Rectangle bounds, Color clr, bool cacImgScaleRec) {
            using (ImageAttributes ia = new ImageAttributes()) {
                ColorMatrix cm = new ColorMatrix();
                // convert and refactor color palette
                cm.Matrix00 = ParseColor(clr.R);
                cm.Matrix11 = ParseColor(clr.G);
                cm.Matrix22 = ParseColor(clr.B);
                cm.Matrix33 = ParseColor(clr.A);
                cm.Matrix44 = 1f;
                // set matrix
                ia.SetColorMatrix(cm);
                // draw
                if (cacImgScaleRec) {
                    Rectangle drawRec, imgRec;
                    GetImgScaleSize1(bounds, img, out drawRec, out imgRec);
                    g.DrawImage(img, drawRec, imgRec.X,imgRec.Y,imgRec.Width,imgRec.Height, GraphicsUnit.Pixel, ia);
                }
                else {
                    g.DrawImage(img, bounds, 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, ia);
                }
            }
        }
        /// <summary>灰度draw a faded bitmap</summary>
        public static void DrawFadedImage(Graphics g, Image img, Rectangle bounds,bool cacImgScaleRec) {
            using (ImageAttributes ia = new ImageAttributes()) {
                ColorMatrix cm = new ColorMatrix();
                cm.Matrix00 = 1f;           //r
                cm.Matrix11 = 1f;           //g
                cm.Matrix22 = 1f;           //b
                cm.Matrix33 = FADE_LEVEL;   //a
                cm.Matrix44 = 1f;           //w

                ia.SetColorMatrix(cm);
                if (cacImgScaleRec) {
                    Rectangle drawRec, imgRec;
                    GetImgScaleSize1(bounds, img, out drawRec, out imgRec);
                    g.DrawImage(img, drawRec, imgRec.X, imgRec.Y, imgRec.Width, imgRec.Height, GraphicsUnit.Pixel, ia);
                }
                else {
                    g.DrawImage(img, bounds, 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, ia);
                }
            }
        }
        public static void DrawCacScaleImage(Graphics g, Image img, Rectangle bounds) {
            Rectangle drawRec, imgRec;
            GetImgScaleSize1(bounds, img, out drawRec, out imgRec);
            g.DrawImage(img, drawRec, imgRec.X, imgRec.Y, imgRec.Width, imgRec.Height, GraphicsUnit.Pixel);
        }
        /// <summary>
        /// 使用缩放算法 以最小边为准缩放 其余丢掉
        /// </summary>
        /// <param name="recSrc">要画的区域</param>
        /// <param name="drawRec">要画区域的一部分，如果图片尺寸宽高都大于要画的区域，当前值则为要画的区域全部</param>
        /// <param name="imgRec">要画的图片区域 以小的为准，按比例取大值</param>
        /// <returns></returns>
        static void GetImgScaleSize1(Rectangle recSrc, Image img, out Rectangle drawRec, out Rectangle imgRec) {
            //      
            drawRec = recSrc;
            imgRec = new Rectangle(0, 0, img.Width, img.Height);
            if (img.Width <= recSrc.Width && img.Height <= recSrc.Height) {
                drawRec = imgRec;
                //绘制区域可以包含图片大小 不缩放 绘制
                return;
            }
            else {
                if (img.Width <= recSrc.Width || img.Height <= recSrc.Height) {
                    //图片宽度小于绘制宽度 或者图片高度小于等于绘制高度
                    //不缩放
                    if (img.Width <= recSrc.Width) {
                        //图片宽度小于绘制宽度
                        //求绘制的区域
                        drawRec = new Rectangle(recSrc.X, recSrc.Y, img.Width, recSrc.Height);
                        int offsetY = img.Height - recSrc.Height;
                        if (offsetY > 4) offsetY = 4;
                        imgRec = new Rectangle(0, offsetY, img.Width, recSrc.Height);//上移动4像素
                    }
                    else if (img.Height <= recSrc.Height) {
                        //图片高度小于绘制高度
                        drawRec = new Rectangle(recSrc.X, recSrc.Y, recSrc.Width, img.Height);
                        int offsetX = img.Width - recSrc.Width;
                        if (offsetX > 4) offsetX = 4;
                        imgRec = new Rectangle(offsetX, 0, recSrc.Width, img.Height);//左移动4像素
                    }
                }
                else {
                    //图片比较大的情况,进行缩放 剔除边缘4个像素,图片尺寸不能小于16
                    float scale_W = (float)img.Width / (float)recSrc.Width;
                    float scale_H = (float)img.Height / (float)recSrc.Height;
                    //以小的边为准去取图片区域
                    if (scale_H >= scale_W) {
                        //高度比较大，以宽为准,求画图的高度                       
                        imgRec = new Rectangle(0, 0, img.Width, Convert.ToInt32(img.Width * recSrc.Height / (float)recSrc.Width));
                        drawRec = new Rectangle(recSrc.X, recSrc.Y, recSrc.Width, recSrc.Height);

                    }
                    else {
                        imgRec = new Rectangle(0, 0, img.Height, Convert.ToInt32(img.Height * recSrc.Width / (float)recSrc.Height));
                        drawRec = new Rectangle(recSrc.X, recSrc.Y, recSrc.Width, recSrc.Height);
                    }
                }
            }
        }
        /// <summary>焦点Draw a gradient mask</summary>
        public static void DrawMask(Graphics g, Rectangle bounds) {
            bounds.Inflate(-1, -1);
            // create an interior path
            using (GraphicsPath gp = CreateRoundRectanglePath(g, bounds, 4)) {
                // fill the button with a subtle glow
                using (LinearGradientBrush fillBrush = new LinearGradientBrush(
                    bounds,
                    Color.FromArgb(160, Color.White),
                    Color.FromArgb(5, Color.Silver),
                    75f)) {
                    Blend blnd = new Blend();
                    blnd.Positions = new float[] { 0f, .1f, .2f, .3f, .4f, .5f, 1f };
                    blnd.Factors = new float[] { 0f, .1f, .2f, .4f, .7f, .8f, 1f };
                    fillBrush.Blend = blnd;
                    g.FillPath(fillBrush, gp);
                }
            }
        }
        /// <summary>Draw border on checked style</summary>
        public static void DrawBorder(Graphics g, Rectangle bounds, Color clr) {
            bounds.Inflate(-2, -2);
            using (GraphicsPath borderPath = CreateRoundRectanglePath(g, bounds, 4)) {
                // top-left bottom-right -dark
                using (LinearGradientBrush borderBrush = new LinearGradientBrush(
                    bounds,
                    Color.FromArgb(140, clr),
                    Color.FromArgb(140, Color.White),
                    LinearGradientMode.BackwardDiagonal)) {
                    Blend blnd = new Blend();
                    blnd.Positions = new float[] { 0f, .5f, 1f };
                    blnd.Factors = new float[] { 1f, 0f, 1f };
                    borderBrush.Blend = blnd;
                    using (Pen borderPen = new Pen(borderBrush, 2f))
                        g.DrawPath(borderPen, borderPath);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="src"></param>
        /// <param name="ImageGlowColor">发光色</param>
        /// <param name="ImageGlowFactor">发光倍数，默认2</param>
        /// <returns></returns>
        public static Bitmap CreateGlow(Image src, Color ImageGlowColor, int ImageGlowFactor/*默认2*/) {

            Rectangle imageRect = new Rectangle(0, 0, src.Width + ImageGlowFactor, src.Height + ImageGlowFactor);
            Bitmap _bmpGlow = new Bitmap(imageRect.Width, imageRect.Height);
            using (Graphics g = Graphics.FromImage(_bmpGlow))
                g.DrawImage(src, imageRect);
            int i = 0;
            for (int x = 0; x < imageRect.Height; x++) {
                for (i = 0; i < imageRect.Width; i++) {
                    if (_bmpGlow.GetPixel(i, x).A > GLOW_THRESHHOLD)
                        _bmpGlow.SetPixel(i, x, Color.FromArgb(GLOW_LEVEL, ImageGlowColor));
                }
                i = 0;
            }
            return _bmpGlow;
        }
        public static Image CreateMirror(Image src) {
            int height = (int)(src.Height * .7f);
            int width = (int)(src.Width * 1f);
            Rectangle imageRect = new Rectangle(0, 0, width, height);
            Bitmap _bmpMirror = new Bitmap(imageRect.Width, imageRect.Height);

            using (Graphics g = Graphics.FromImage(_bmpMirror))
                g.DrawImage(src, imageRect);
            _bmpMirror.RotateFlip(RotateFlipType.Rotate180FlipX);
            return _bmpMirror;
        }
        /// <summary>Draw a mirror effect</summary>
        public static void DrawMirror(Graphics g, Image _bmpMirror, Rectangle bounds) {
            // Rectangle imageRect = GetImageBounds(bounds, this.Image);
            bounds.Y = bounds.Bottom;
            bounds.Height = _bmpMirror.Height;
            bounds.Width = _bmpMirror.Width;
            using (ImageAttributes ia = new ImageAttributes()) {
                ColorMatrix cm = new ColorMatrix();
                cm.Matrix00 = 1f;           //r
                cm.Matrix11 = 1f;           //g
                cm.Matrix22 = 1f;           //b
                cm.Matrix33 = MIRROR_LEVEL; //a
                cm.Matrix44 = 1f;           //w

                ia.SetColorMatrix(cm);
                g.DrawImage(_bmpMirror, bounds, 0, 0, _bmpMirror.Width, _bmpMirror.Height, GraphicsUnit.Pixel, ia);
            }
        }
        /// <summary>Create a rounded rectangle GraphicsPath</summary>
        private static GraphicsPath CreateRoundRectanglePath(Graphics g, Rectangle bounds, float radius) {
            // create a path
            GraphicsPath pathBounds = new GraphicsPath();
            // arc top left
            pathBounds.AddArc(bounds.Left, bounds.Top, radius, radius, 180, 90);
            // line top
            pathBounds.AddLine(bounds.Left + radius, bounds.Top, bounds.Right - radius, bounds.Top);
            // arc top right
            pathBounds.AddArc(bounds.Right - radius, bounds.Top, radius, radius, 270, 90);
            // line right
            pathBounds.AddLine(bounds.Right, bounds.Top + radius, bounds.Right, bounds.Bottom - radius);
            // arc bottom right
            pathBounds.AddArc(bounds.Right - radius, bounds.Bottom - radius, radius, radius, 0, 90);
            // line bottom
            pathBounds.AddLine(bounds.Right - radius, bounds.Bottom, bounds.Left + radius, bounds.Bottom);
            // arc bottom left
            pathBounds.AddArc(bounds.Left, bounds.Bottom - radius, radius, radius, 90, 90);
            // line left
            pathBounds.AddLine(bounds.Left, bounds.Bottom - radius, bounds.Left, bounds.Top + radius);
            pathBounds.CloseFigure();
            return pathBounds;
        }
        /// <summary>Get the image size and position</summary>
        private static Rectangle GetImageBounds(Rectangle bounds, Image img, bool ImageMirror) {
            int left = (int)((bounds.Width - img.Width) * .5f);
            int top = (int)((bounds.Height - img.Height) * .5f);
            if (ImageMirror)
                top = (int)((bounds.Height - (img.Height + (int)(img.Height * .7f))) * .5f);
            return new Rectangle(left, top, img.Width, img.Height);
        }

        /// <summary>Convert rgb to float</summary>
        private static float ParseColor(byte clr) {
            return clr == 0 ? 0 : ((float)clr / 255);
        }

        /// <summary>
        /// Draws the glow for the button when the
        /// mouse is inside the client area using
        /// the GlowColor property.
        /// </summary>
        /// <param name="g">The graphics object used in the paint event.</param>
        /// <param name="CornerRadius">圆角像素</param>
        /// <param name="rec">发光区域</param>
        /// <param name="GlowColor">发光色</param>
        /// <param name="mGlowAlpha">发光透明度</param>
        public static void DrawGlow(Graphics g, Rectangle rec, int CornerRadius, Color GlowColor, byte mGlowAlpha) {

            SetClip(g, rec, CornerRadius);
            using (GraphicsPath glow = new GraphicsPath()) {
                glow.AddEllipse(-5, rec.Height / 2 - 10, rec.Width + 11, rec.Height + 11);
                using (PathGradientBrush gl = new PathGradientBrush(glow)) {
                    gl.CenterColor = Color.FromArgb(mGlowAlpha, GlowColor);
                    gl.SurroundColors = new Color[] { Color.FromArgb(0, GlowColor) };
                    g.FillPath(gl, glow);
                }
            }
            g.ResetClip();
        }
        private static void SetClip(Graphics g, Rectangle ClientRectangle, int CornerRadius) {
            Rectangle r = ClientRectangle;
            r.X++; r.Y++; r.Width -= 3; r.Height -= 3;
            using (GraphicsPath rr = RoundRect(r, CornerRadius, CornerRadius, CornerRadius, CornerRadius)) {
                g.SetClip(rr);
            }
        }
        private static GraphicsPath RoundRect(RectangleF r, float r1, float r2, float r3, float r4) {
            float x = r.X, y = r.Y, w = r.Width, h = r.Height;
            GraphicsPath rr = new GraphicsPath();
            rr.AddBezier(x, y + r1, x, y, x + r1, y, x + r1, y);
            rr.AddLine(x + r1, y, x + w - r2, y);
            rr.AddBezier(x + w - r2, y, x + w, y, x + w, y + r2, x + w, y + r2);
            rr.AddLine(x + w, y + r2, x + w, y + h - r3);
            rr.AddBezier(x + w, y + h - r3, x + w, y + h, x + w - r3, y + h, x + w - r3, y + h);
            rr.AddLine(x + w - r3, y + h, x + r4, y + h);
            rr.AddBezier(x + r4, y + h, x, y + h, x, y + h - r4, x, y + h - r4);
            rr.AddLine(x, y + h - r4, x, y + r1);
            return rr;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="style">范围[0,5]</param>
        /// <param name="gr"></param>
        /// <param name="destination"></param>
        /// <param name="alpha">透明度0~255</param>
        /// <param name="smooth">平滑处理</param>
        public static void DrawWin7ButtonStyle(byte style, Graphics gr, Rectangle destination, byte alpha, bool smooth) {
            DrawWin7ButtonStyle(style, gr, destination, new Padding(3), alpha, smooth);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="style">范围[0,5]</param>
        /// <param name="gr"></param>
        /// <param name="destination"></param>
        /// <param name="padding">边缘处理宽度,默认3</param>
        /// <param name="alpha">透明度0~255</param>
        /// <param name="smooth">平滑处理</param>
        public static void DrawWin7ButtonStyle(byte style, Graphics gr, Rectangle destination, ushort padding, byte alpha, bool smooth) {
            DrawWin7ButtonStyle(style, gr, destination, new Padding(padding), alpha, smooth);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="style">范围[0,5]</param>
        /// <param name="gr"></param>
        /// <param name="destination"></param>
        /// <param name="padding">边缘处理宽度,默认3</param>
        /// <param name="alpha">透明度0~255</param>
        /// <param name="smooth">平滑处理</param>
        public static void DrawWin7ButtonStyle(byte style, Graphics gr, Rectangle destination, Padding padding, byte alpha, bool smooth) {
            // e.Graphics.DrawImageWithPadding(Resources.Button, _panel.ClientRectangle,
            //new Rectangle(0, 21, 11, 21), new Padding(3, 3, 3, 3));
            InterpolationMode imold = gr.InterpolationMode;
            PixelOffsetMode pmold = gr.PixelOffsetMode;
            if (smooth) {
                // Disable smoothing of stretched drawings (like in Windows)
                gr.InterpolationMode = InterpolationMode.NearestNeighbor;
                gr.PixelOffsetMode = PixelOffsetMode.Half;
            }
            float alpT = alpha * 10f / 255f;
            int alps = Convert.ToInt32(alpT);
            Image img = null;
            if (style > 5) style = 5;//不要超过范围
            Rectangle imgRec = new Rectangle(0, style * 21, 11, 21);
            switch (alps) {
                case 0:
                case 1:
                    img = Win7Button.Win7ButtonImages.Windows7Button_a10;
                    break;
                case 2:
                    img = Win7Button.Win7ButtonImages.Windows7Button_a20;
                    break;
                case 3:
                    img = Win7Button.Win7ButtonImages.Windows7Button_a30;
                    break;
                case 4:
                    img = Win7Button.Win7ButtonImages.Windows7Button_a40;
                    break;
                case 5:
                    img = Win7Button.Win7ButtonImages.Windows7Button_a50;
                    break;
                case 6:
                    img = Win7Button.Win7ButtonImages.Windows7Button_a60;
                    break;
                case 7:
                    img = Win7Button.Win7ButtonImages.Windows7Button_a70;
                    break;
                case 8:
                    img = Win7Button.Win7ButtonImages.Windows7Button_a80;
                    break;
                case 9:
                    img = Win7Button.Win7ButtonImages.Windows7Button_a90;
                    break;
                case 10:
                    img = Win7Button.Win7ButtonImages.Windows7Button_a100;
                    break;
                default:
                    break;
            }
            DrawImageWithPadding(gr, img, destination, imgRec, padding);
            if (smooth) {//还原
                gr.InterpolationMode = imold;
                gr.PixelOffsetMode = pmold;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="style">范围0~4</param>
        /// <param name="gr"></param>
        /// <param name="destination"></param>
        /// <param name="alpha">范围0~255</param>
        /// <param name="smooth"></param>
        public static void DrawWinXPButtonStyle(byte style, Graphics gr, Rectangle destination, byte alpha, bool smooth) {
            DrawWinXPButtonStyle(style, gr, destination, new Padding(3), alpha, smooth);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="style">范围0~4</param>
        /// <param name="gr"></param>
        /// <param name="destination"></param>
        /// <param name="padding"></param>
        /// <param name="alpha">范围0~255</param>
        /// <param name="smooth"></param>
        public static void DrawWinXPButtonStyle(byte style, Graphics gr, Rectangle destination, ushort padding, byte alpha, bool smooth) {
            DrawWinXPButtonStyle(style, gr, destination, new Padding(padding), alpha, smooth);
        }
        /// <summary>
        /// 5状态
        /// </summary>
        /// <param name="style">范围0~4</param>
        /// <param name="gr"></param>
        /// <param name="destination"></param>
        /// <param name="padding"></param>
        /// <param name="alpha">范围0~255</param>
        /// <param name="smooth"></param>
        public static void DrawWinXPButtonStyle(byte style, Graphics gr, Rectangle destination, Padding padding, byte alpha, bool smooth) {
            // e.Graphics.DrawImageWithPadding(Resources.Button, _panel.ClientRectangle,
            //new Rectangle(0, 21, 11, 21), new Padding(3, 3, 3, 3));
            InterpolationMode imold = gr.InterpolationMode;
            PixelOffsetMode pmold = gr.PixelOffsetMode;
            if (smooth) {
                // Disable smoothing of stretched drawings (like in Windows)
                gr.InterpolationMode = InterpolationMode.NearestNeighbor;
                gr.PixelOffsetMode = PixelOffsetMode.Half;
            }
            float alpT = alpha * 10f / 255f;
            int alps = Convert.ToInt32(alpT);
            Image img = null;
            if (style > 4) style = 4;//不要超过范围
            Rectangle imgRec = new Rectangle(0, style * 21, 11, 21);
            switch (alps) {
                case 0:
                case 1:
                    img = Win7Button.Win7ButtonImages.Windows_XP_Button_10;
                    break;
                case 2:
                    img = Win7Button.Win7ButtonImages.Windows_XP_Button_20;
                    break;
                case 3:
                    img = Win7Button.Win7ButtonImages.Windows_XP_Button_30;
                    break;
                case 4:
                    img = Win7Button.Win7ButtonImages.Windows_XP_Button_40;
                    break;
                case 5:
                    img = Win7Button.Win7ButtonImages.Windows_XP_Button_50;
                    break;
                case 6:
                    img = Win7Button.Win7ButtonImages.Windows_XP_Button_60;
                    break;
                case 7:
                    img = Win7Button.Win7ButtonImages.Windows_XP_Button_70;
                    break;
                case 8:
                    img = Win7Button.Win7ButtonImages.Windows_XP_Button_80;
                    break;
                case 9:
                    img = Win7Button.Win7ButtonImages.Windows_XP_Button_90;
                    break;
                case 10:
                    img = Win7Button.Win7ButtonImages.Windows_XP_Button_100;
                    break;
                default:
                    break;
            }
            DrawImageWithPadding(gr, img, destination, imgRec, padding);

            if (smooth) {//还原
                gr.InterpolationMode = imold;
                gr.PixelOffsetMode = pmold;
            }
        }

        /// <summary>
        /// Draws the image into the specified destination rectangle with the specified sizing
        /// padding for stretched drawing.
        /// </summary>
        /// <param name="gr">The extended Graphics object.</param>
        /// <param name="image">The image which will be drawn.</param>
        /// <param name="destination">目标矩形图像绘制The destination rectangle in which the image will be drawn.</param>
        /// <param name="padding">The padding specifying the image parts which won't be stretched.</param>
        public static void DrawImageWithPadding(Graphics gr, Image image,
            Rectangle destination, Padding padding) {
            if (image == null) {
                throw new ArgumentNullException("image");
            }

            DrawImageWithPadding(gr, image, destination, new Rectangle(Point.Empty, image.Size),
                padding);
        }

        /// <summary>
        /// Draws the part of the image defined by the source rectangle into the specified
        /// destination rectangle with the specified sizing padding for stretched drawing.
        /// </summary>
        /// <param name="gr">The extended Graphics object.</param>
        /// <param name="image">The image which will be drawn.</param>
        /// <param name="destination">The destination rectangle in which the image will be drawn.</param>
        /// <param name="source">要画图片的区域The source rectangle in the image used for clipping parts of it.</param>
        /// <param name="padding">The padding specifying the image parts which won't be stretched.</param>
        public static void DrawImageWithPadding(Graphics gr, Image image,
            Rectangle destination, Rectangle source, Padding padding) {
            if (gr == null) {
                throw new ArgumentNullException("gr");
            }
            if (image == null) {
                throw new ArgumentNullException("image");
            }

            Rectangle[] destinations = GetSizingRectangles(destination, padding);
            Rectangle[] sources = GetSizingRectangles(source, padding);

            for (int i = 0; i < 9; i++) {
                gr.DrawImage(image, destinations[i], sources[i], GraphicsUnit.Pixel);
            }
        }

        private static Rectangle[] GetSizingRectangles(Rectangle rectangle, Padding padding) {
            int leftV = rectangle.X + padding.Left;
            int rightV = rectangle.X + rectangle.Width - padding.Right;
            int topH = rectangle.Y + padding.Top;
            int bottomH = rectangle.Y + rectangle.Height - padding.Bottom;
            int innerW = rectangle.Width - padding.Horizontal;
            int innerH = rectangle.Height - padding.Vertical;

            // Set parts in descending order to draw upper left tiles over bottom right ones
            Rectangle[] rectangles = new Rectangle[9];
            rectangles[8] = new Rectangle(rectangle.X, rectangle.Y, padding.Left, padding.Top);
            rectangles[7] = new Rectangle(leftV, rectangle.Y, innerW, padding.Top);
            rectangles[6] = new Rectangle(rightV, rectangle.Y, padding.Right, padding.Top);
            rectangles[5] = new Rectangle(rectangle.X, topH, padding.Left, innerH);
            rectangles[4] = new Rectangle(leftV, topH, innerW, innerH);
            rectangles[3] = new Rectangle(rightV, topH, padding.Right, innerH);
            rectangles[2] = new Rectangle(rectangle.X, bottomH, padding.Left, padding.Bottom);
            rectangles[1] = new Rectangle(leftV, bottomH, innerW, padding.Bottom);
            rectangles[0] = new Rectangle(rightV, bottomH, padding.Right, padding.Bottom);
            return rectangles;
        }
    }
}

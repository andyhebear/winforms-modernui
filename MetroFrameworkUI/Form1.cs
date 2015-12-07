using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MetroFramework
{
    internal partial class Form1 : MetroFramework.Forms.MetroForm
    {
        public Form1() {
            InitializeComponent();
        }
        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);
            MetroFramework.Fonts.FontResolver fr = new MetroFramework.Fonts.FontResolver();
            Font font = fr.ResolveFont("Open Sans Bold", 12.0f, FontStyle.Bold, GraphicsUnit.Pixel);
            this.Font = font;
        }
    }
}

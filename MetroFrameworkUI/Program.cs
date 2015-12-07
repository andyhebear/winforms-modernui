using System;
using System.Collections.Generic;
using System.Windows.Forms;
using MetroFramework.Demo;

namespace MetroFramework
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
            Form1 f1 = new Form1();
            f1.ShowDialog();
            Application.Run(new MainForm());
        }
    }
}

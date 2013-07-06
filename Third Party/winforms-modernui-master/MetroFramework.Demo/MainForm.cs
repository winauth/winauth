using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

using MetroFramework;
using MetroFramework.Forms;

namespace MetroFramework.Demo
{
    public partial class MainForm : MetroForm
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void metroTileSwitch_Click(object sender, EventArgs e)
        {
            var m = new Random();
            int next = m.Next(0, 13);
            metroStyleManager.Style = (MetroColorStyle)next;
        }

        private void metroTile1_Click(object sender, EventArgs e)
        {
            metroStyleManager.Theme = metroStyleManager.Theme == MetroThemeStyle.Light ? MetroThemeStyle.Dark : MetroThemeStyle.Light;
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            MetroTaskWindow.ShowTaskWindow(this,"SubControl in TaskWindow", new TaskWindowControl(), 10);
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ColorX.Forms
{
    public partial class ColorX_AwakeMenu : Form
    {
        int loadTime = 0;

        public ColorX_AwakeMenu()
        {
            InitializeComponent();
        }

        private void ColorX_AwakeMenu_Load(object sender, EventArgs e)
        {
            Awake_Timer.Start();
        }

        private void Awake_Timer_Tick(object sender, EventArgs e)
        {
            loadTime++;

            if (loadTime == 100)
            {
                Awake_Timer.Stop();
                ColorX_MainMenu colorX_MainMenu = new ColorX_MainMenu();
                this.Hide();
                colorX_MainMenu.Show();
            }
        }
    }
}

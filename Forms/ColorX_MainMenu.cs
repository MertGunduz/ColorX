using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace ColorX.Forms
{
    public partial class ColorX_MainMenu : Form
    {
        // Color & Graphics Related Data
        LinearGradientBrush linearGradientBrush;
        Pen pen;
        Graphics graphics;
        Color color;
        bool isPaint;

        // Location Related Data
        Point pointX, pointY;
        Size size;
        Rectangle rectangle;

        // Time Data
        int timeBrush = 0;
        Random random = new Random();

        // Tool Related Data
        enum Tool
        {
            None,
            Fill, 
            Pen,
            Brush,
            Gradient,
            Eraser
        }

        Tool SelectedTool;

        public ColorX_MainMenu()
        {
            InitializeComponent();
        }

        private void ColorX_MainMenu_Load(object sender, EventArgs e)
        {
            // Load Colors 
            pen = new Pen(Color.Black);

            // Load None Tool
            SelectedTool = Tool.None;
        }

        private void Palette_PictureBox_Click(object sender, EventArgs e)
        {
            if (Palette_ColorDialog.ShowDialog() == DialogResult.OK) ;
            {
                color = Palette_ColorDialog.Color;
                pen.Color = color;
            }
        }

        private void Pencil_PictureBox_Click(object sender, EventArgs e)
        {
            SelectedTool = Tool.Pen;
        }

        private void Brush_PictureBox_Click(object sender, EventArgs e)
        {
            SelectedTool = Tool.Brush;
        }

        private void Canvas_PictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            // Sets The Pen On
            isPaint = true;

            // Takes The Click Location
            pointY = e.Location;
        }

        private void Canvas_PictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (isPaint)
            {
                if (SelectedTool == Tool.Pen)
                {
                    // Take Location
                    pointX = e.Location;

                    // Create Graphics & Initialize
                    graphics = Canvas_PictureBox.CreateGraphics();

                    //  Drawline Graphics
                    graphics.DrawLine(pen, pointX, pointY);

                    pointY = pointX;
                }
                else if (SelectedTool == Tool.Brush)
                {
                    // Sets The Pen Bolder
                    pen.Width = 3;

                    // Start Brush Timer
                    Brush_Timer.Start();

                    // Take Location
                    pointX = e.Location;

                    // Create Graphics & Initialize
                    graphics = Canvas_PictureBox.CreateGraphics();

                    //  Drawline Graphics
                    graphics.DrawLine(pen, pointX, pointY);

                    pointY = pointX;

                    if (timeBrush % 2 == 0)
                    {
                        pen.Color = Color.FromArgb(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));
                    }
                }
            }
        }
        private void Canvas_PictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            // Sets The Pen Off
            isPaint = false;

            // Stops Brush Timer
            Brush_Timer.Stop();
        }

        // Timer Algo
        private void Brush_Timer_Tick(object sender, EventArgs e)
        {
            timeBrush++;
        }

        // Window Control Buttons
        private void Minimize_Button_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }


        private void Exit_Button_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.IO;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace ColorX.Forms
{
    public partial class ColorX_MainMenu : Form
    {
        // Color & Graphics Related Data
        LinearGradientBrush linearGradientBrush;
        Pen pen;
        SolidBrush brush;
        Graphics graphics;
        Color color;
        Bitmap bitmap;
        bool isPaint;

        // Location Related Data
        Point pointX, pointY;

        // File Open & Saving
        OpenFileDialog openFileDialog = new OpenFileDialog();
        SaveFileDialog saveFileDialog = new SaveFileDialog();

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
            brush = new SolidBrush(Color.Black);

            // Load None Tool
            SelectedTool = Tool.None;
        }

        private void Palette_PictureBox_Click(object sender, EventArgs e)
        {
            if (Palette_ColorDialog.ShowDialog() == DialogResult.OK)
            {
                color = Palette_ColorDialog.Color;
                pen.Color = color;
                brush = new SolidBrush(color);
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

        private void Gradient_PictureBox_Click(object sender, EventArgs e)
        {
            linearGradientBrush = new LinearGradientBrush(new Point(0, 0), new Point(Canvas_PictureBox.Width, Canvas_PictureBox.Height), Color.FromArgb(100, random.Next(0, 255), random.Next(0, 255), random.Next(0, 255)), Color.FromArgb(100, random.Next(0, 255), random.Next(0, 255), random.Next(0, 255)));
            graphics = Canvas_PictureBox.CreateGraphics();
            graphics.FillRectangle(linearGradientBrush, new Rectangle(new Point(0, 0), new Size(Canvas_PictureBox.Width, Canvas_PictureBox.Height)));
        }

        private void Eraser_PictureBox_Click(object sender, EventArgs e)
        {
            SelectedTool = Tool.Eraser;
        }

        private void Save_PictureBox_Click(object sender, EventArgs e)
        {
            try
            {
                saveFileDialog.Filter = "PNG File (*.PNG) | *.PNG | JPG File (*.JPG) | *.JPG | BMP File (*.BMP) | *.BMP";
                saveFileDialog.Title = "Save Image";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    Canvas_PictureBox.Image.Save(saveFileDialog.FileName);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show($"{exception.Message}", $"{exception.Data.ToString()}", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Open_PictureBox_Click(object sender, EventArgs e)
        {
            try
            {
                openFileDialog.Filter = "Image Files (*.BMP; *.JPG; *.PNG) | *.JPG; *.PNG; *.BMP | All Files (*.*) | *.*";
                openFileDialog.Title = "Open Image";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    bitmap = new Bitmap(openFileDialog.FileName);
                    Canvas_PictureBox.Image = bitmap;
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show($"{exception.Message}", $"{exception.Data.ToString()}", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
                    // Sets The Pen Thinner & Black Color
                    pen.Width = 2;
                    pen.Color = Color.Black;  

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
                else if (SelectedTool == Tool.Eraser)
                {
                    // Sets The Pen Bolder & Color To White
                    pen.Width = 10;
                    pen.Color = Color.FromArgb(240, 240, 240);

                    // Start Brush Timer
                    Brush_Timer.Start();

                    // Take Location
                    pointX = e.Location;

                    // Create Graphics & Initialize
                    graphics = Canvas_PictureBox.CreateGraphics();

                    //  Drawline Graphics
                    graphics.DrawLine(pen, pointX, pointY);

                    pointY = pointX;
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

        private void Fill_PictureBox_Click(object sender, EventArgs e)
        {
            graphics = Canvas_PictureBox.CreateGraphics();
            graphics.FillRectangle(brush, new Rectangle(0, 0, Canvas_PictureBox.Width, Canvas_PictureBox.Height));
        }

        private void Exit_Button_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}

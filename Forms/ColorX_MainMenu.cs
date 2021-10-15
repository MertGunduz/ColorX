using System;
using System.Drawing;
using System.Drawing.Drawing2D;
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

        private void Fill_PictureBox_Click(object sender, EventArgs e)
        {
            graphics = Canvas_PictureBox.CreateGraphics();
            graphics.FillRectangle(brush, new Rectangle(0, 0, Canvas_PictureBox.Width, Canvas_PictureBox.Height));
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
                    // Sets The Pen Thinner
                    pen.Width = 2;

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
        private void Exit_Button_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // Color Picker Buttons
        private void FirstOrange_PictureBox_Click(object sender, EventArgs e)
        {
            SetColor(FirstOrange_PictureBox, pen, brush);
        }

        private void SecondOrange_PictureBox_Click(object sender, EventArgs e)
        {
            SetColor(SecondOrange_PictureBox, pen, brush);

        }

        private void ThirdOrange_PictureBox_Click(object sender, EventArgs e)
        {
            SetColor(ThirdOrange_PictureBox, pen, brush);

        }

        private void FourthOrange_PictureBox_Click(object sender, EventArgs e)
        {
            SetColor(FourthOrange_PictureBox, pen, brush);
        }
        private void FirstGreen_PictureBox_Click(object sender, EventArgs e)
        {
            SetColor(FirstGreen_PictureBox, pen, brush);
        }

        private void SecondGreen_PictureBox_Click(object sender, EventArgs e)
        {
            SetColor(SecondGreen_PictureBox, pen, brush);
        }

        private void ThirdGreen_PictureBox_Click(object sender, EventArgs e)
        {
            SetColor(ThirdGreen_PictureBox, pen, brush);
        }

        private void FourthGreen_PictureBox_Click(object sender, EventArgs e)
        {
            SetColor(FourthGreen_PictureBox, pen, brush);
        }

        private void FirstBrown_PictureBox_Click(object sender, EventArgs e)
        {
            SetColor(FirstBrown_PictureBox, pen, brush);
        }

        private void SecondBrown_PictureBox_Click(object sender, EventArgs e)
        {
            SetColor(SecondBrown_PictureBox, pen, brush);
        }

        private void ThirdBrown_PictureBox_Click(object sender, EventArgs e)
        {
            SetColor(ThirdBrown_PictureBox, pen, brush);
        }

        private void FourthBrown_PictureBox_Click(object sender, EventArgs e)
        {
            SetColor(FourthBrown_PictureBox, pen, brush);
        }

        private void FirstJungleGrass_PictureBox_Click(object sender, EventArgs e)
        {
            SetColor(FirstJungleGrass_PictureBox, pen, brush);
        }

        private void SecondJungleGrass_PictureBox_Click(object sender, EventArgs e)
        {
            SetColor(SecondJungleGrass_PictureBox, pen, brush);
        }

        private void ThirdJungleGrass_PictureBox_Click(object sender, EventArgs e)
        {
            SetColor(ThirdJungleGrass_PictureBox, pen, brush);
        }

        private void FourthJungleGrass_PictureBox_Click(object sender, EventArgs e)
        {
            SetColor(FourthJungleGrass_PictureBox, pen, brush);
        }

        private void FirstMint_PictureBox_Click(object sender, EventArgs e)
        {
            SetColor(FirstMint_PictureBox, pen, brush);
        }

        private void SecondMint_PictureBox_Click(object sender, EventArgs e)
        {
            SetColor(SecondMint_PictureBox, pen, brush);
        }

        private void ThirdMint_PictureBox_Click(object sender, EventArgs e)
        {
            SetColor(ThirdMint_PictureBox, pen, brush);
        }

        private void FourthMint_PictureBox_Click(object sender, EventArgs e)
        {
            SetColor(FourthMint_PictureBox, pen, brush);
        }

        private void FirstBlue_PictureBox_Click(object sender, EventArgs e)
        {
            SetColor(FirstBlue_PictureBox, pen, brush);
        }

        private void SecondBlue_PictureBox_Click(object sender, EventArgs e)
        {
            SetColor(SecondBlue_PictureBox, pen, brush);
        }

        private void ThirdBlue_PictureBox_Click(object sender, EventArgs e)
        {
            SetColor(ThirdBlue_PictureBox, pen, brush);
        }

        private void FourthBlue_PictureBox_Click(object sender, EventArgs e)
        {
            SetColor(FourthBlue_PictureBox, pen, brush);
        }

        private void FirstNavyBlue_PictureBox_Click(object sender, EventArgs e)
        {
            SetColor(FirstNavyBlue_PictureBox, pen, brush);
        }

        private void SecondNavyBlue_PictureBox_Click(object sender, EventArgs e)
        {
            SetColor(SecondNavyBlue_PictureBox, pen, brush);
        }

        private void ThirdNavyBlue_PictureBox_Click(object sender, EventArgs e)
        {
            SetColor(ThirdNavyBlue_PictureBox, pen, brush);
        }

        private void FourthNavyBlue_PictureBox_Click(object sender, EventArgs e)
        {
            SetColor(FourthNavyBlue_PictureBox, pen, brush);
        }

        private void FirstPurple_PictureBox_Click(object sender, EventArgs e)
        {
            SetColor(FirstPurple_PictureBox, pen, brush);
        }

        private void SecondPurple_PictureBox_Click(object sender, EventArgs e)
        {
            SetColor(SecondPurple_PictureBox, pen, brush);
        }

        private void ThirdPurple_PictureBox_Click(object sender, EventArgs e)
        {
            SetColor(ThirdPurple_PictureBox, pen, brush);
        }

        private void FourthPurple_PictureBox_Click(object sender, EventArgs e)
        {
            SetColor(FourthPurple_PictureBox, pen, brush);
        }

        private void FirstPink_PictureBox_Click(object sender, EventArgs e)
        {
            SetColor(FirstPink_PictureBox, pen, brush);
        }

        private void SecondPink_PictureBox_Click(object sender, EventArgs e)
        {
            SetColor(SecondPink_PictureBox, pen, brush);
        }

        private void ThirdPink_PictureBox_Click(object sender, EventArgs e)
        {
            SetColor(ThirdPink_PictureBox, pen, brush);
        }

        private void FourthPink_PictureBox_Click(object sender, EventArgs e)
        {
            SetColor(FourthPink_PictureBox, pen, brush);
        }

        private void FirstRed_PictureBox_Click(object sender, EventArgs e)
        {
            SetColor(FirstRed_PictureBox, pen, brush);
        }

        private void SecondRed_PictureBox_Click(object sender, EventArgs e)
        {
            SetColor(SecondRed_PictureBox, pen, brush);
        }

        private void ThirdRed_PictureBox_Click(object sender, EventArgs e)
        {
            SetColor(ThirdRed_PictureBox, pen, brush);
        }

        private void FourthRed_PictureBox_Click(object sender, EventArgs e)
        {
            SetColor(FourthRed_PictureBox, pen, brush);
        }

        // Color Taking Algo
        private void SetColor(PictureBox Clicked_PictureBox, Pen Selected_Pen, SolidBrush Selected_Brush)
        {
            Selected_Pen.Color = Clicked_PictureBox.BackColor;
            Selected_Brush.Color = Clicked_PictureBox.BackColor;
        }
    }
}

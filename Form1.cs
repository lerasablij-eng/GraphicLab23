using System;
using System.Drawing;
using System.Windows.Forms;

namespace GraphicLab23
{
    public partial class Form1 : Form
    {
        private double a = 2.0;
        private double b = 2.0;
        
        private TextBox txtA;
        private TextBox txtB;
        private Button btnDraw;
        private Panel panelCanvas;

        public Form1()
        {
            InitializeComponent();
            CreateControls();
        }

        private void CreateControls()
        {
            this.Text = "Параметрична крива (Варіант 22)";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            Label labelA = new Label();
            labelA.Text = "Коефіцієнт a:";
            labelA.Location = new Point(20, 20);
            labelA.Size = new Size(100, 25);
            this.Controls.Add(labelA);

            txtA = new TextBox();
            txtA.Text = "2.0";
            txtA.Location = new Point(130, 18);
            txtA.Size = new Size(80, 25);
            this.Controls.Add(txtA);

            Label labelB = new Label();
            labelB.Text = "Коефіцієнт b:";
            labelB.Location = new Point(240, 20);
            labelB.Size = new Size(100, 25);
            this.Controls.Add(labelB);

            txtB = new TextBox();
            txtB.Text = "2.0";
            txtB.Location = new Point(350, 18);
            txtB.Size = new Size(80, 25);
            this.Controls.Add(txtB);

            btnDraw = new Button();
            btnDraw.Text = "Намалювати";
            btnDraw.Location = new Point(460, 16);
            btnDraw.Size = new Size(100, 30);
            btnDraw.Click += BtnDraw_Click;
            this.Controls.Add(btnDraw);

            panelCanvas = new Panel();
            panelCanvas.Location = new Point(10, 60);
            panelCanvas.Size = new Size(760, 490);
            panelCanvas.BackColor = Color.White;
            panelCanvas.BorderStyle = BorderStyle.FixedSingle;
            panelCanvas.Paint += panelCanvas_Paint;
            this.Controls.Add(panelCanvas);
        }

        private void BtnDraw_Click(object sender, EventArgs e)
        {
            if (!double.TryParse(txtA.Text, out a))
            {
                MessageBox.Show("Введіть коректне значення для a");
                return;
            }
            if (!double.TryParse(txtB.Text, out b))
            {
                MessageBox.Show("Введіть коректне значення для b");
                return;
            }
            panelCanvas.Invalidate();
        }

        private void panelCanvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.Clear(Color.White);

            int width = panelCanvas.Width;
            int height = panelCanvas.Height;

            float centerX = width / 2;
            float centerY = height / 2;

            Pen axisPen = new Pen(Color.Gray, 2);
            g.DrawLine(axisPen, 0, centerY, width, centerY);
            g.DrawLine(axisPen, centerX, 0, centerX, height);

            g.DrawLine(axisPen, width - 10, centerY - 5, width, centerY);
            g.DrawLine(axisPen, width - 10, centerY + 5, width, centerY);
            g.DrawLine(axisPen, centerX - 5, 10, centerX, 0);
            g.DrawLine(axisPen, centerX + 5, 10, centerX, 0);

            Font font = new Font("Arial", 12);
            g.DrawString("X", font, Brushes.Black, width - 20, centerY - 15);
            g.DrawString("Y", font, Brushes.Black, centerX + 10, 10);
            g.DrawString("0", font, Brushes.Black, centerX + 5, centerY + 5);

            DrawCurve(g, centerX, centerY);
        }

        private void DrawCurve(Graphics g, float centerX, float centerY)
        {
            float scale = 150.0f;
            int steps = 800;
            double tMin = 0.01;
            double tMax = 5.0;
            double step = (tMax - tMin) / steps;

            Pen curvePen = new Pen(Color.Blue, 3);
            
            double tPrev = tMin;
            double xPrev = a * (Math.Sqrt(tPrev) - Math.Sin(tPrev));
            double yPrev = b * (tPrev - Math.Cos(tPrev));
            
            PointF prevPoint = new PointF(
                centerX + (float)xPrev * scale,
                centerY - (float)yPrev * scale
            );

            for (int i = 1; i <= steps; i++)
            {
                double t = tMin + i * step;
                double x = a * (Math.Sqrt(t) - Math.Sin(t));
                double y = b * (t - Math.Cos(t));
                
                PointF currentPoint = new PointF(
                    centerX + (float)x * scale,
                    centerY - (float)y * scale
                );
                
                if (currentPoint.X > 0 && currentPoint.X < panelCanvas.Width &&
                    currentPoint.Y > 0 && currentPoint.Y < panelCanvas.Height)
                {
                    g.DrawLine(curvePen, prevPoint, currentPoint);
                }
                
                prevPoint = currentPoint;
            }
            
            Font font = new Font("Arial", 10);
            g.DrawString($"a = {a}, b = {b}", font, Brushes.Black, 20, 20);
        }
    }
}

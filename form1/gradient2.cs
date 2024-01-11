using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace form1
{
    public class gradient2 : Panel
    {
        private int borderRadius = 30;
        private float gradientangle = 90F;
        private Color gradientTopColor = Color.DodgerBlue;
        private Color gradientButtomColor = Color.CadetBlue;


        public gradient2()
        {
            this.BackColor = Color.White;
            this.ForeColor = Color.Black;
            this.Size = new Size(350, 200);
        }

        public int BorderRadius
        {
            get => borderRadius;
            set { borderRadius = value; this.Invalidate(); }
        }
        public float Gradientangle
        {
            get => gradientangle;
            set { gradientangle = value; this.Invalidate(); }
        }
        public Color GradientTopColor
        {
            get => gradientTopColor;
            set { gradientTopColor = value; this.Invalidate(); }
        }
        public Color GradientButtomColor
        {
            get => gradientButtomColor;
            set { gradientButtomColor = value; this.Invalidate(); }
        }
        private GraphicsPath GetgradientPath(RectangleF rectangle, float radius)
        {
            GraphicsPath graphicsPath = new GraphicsPath();
            graphicsPath.StartFigure();
            graphicsPath.AddArc(rectangle.Width - radius, rectangle.Height - radius, radius, radius, 0, 90);
            graphicsPath.AddArc(rectangle.X, rectangle.Height - radius, radius, radius, 90, 90);
            graphicsPath.AddArc(rectangle.X, rectangle.Y, radius, radius, 180, 90);
            graphicsPath.AddArc(rectangle.X - radius, rectangle.Y, radius, radius, 270, 90);
            graphicsPath.CloseFigure();
            return graphicsPath;
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                LinearGradientBrush brushart = new LinearGradientBrush(this.ClientRectangle, this.gradientTopColor, this.gradientButtomColor, this.gradientangle);
                Graphics graphicsart = e.Graphics;
                graphicsart.FillRectangle(brushart, ClientRectangle);
                RectangleF rectangleF = new RectangleF(0, 0, this.Width, this.Height);
                if (borderRadius > 2)
                {
                    using (GraphicsPath graphicsPath = GetgradientPath(rectangleF, borderRadius)) 
                    using (Pen pen = new Pen(this.Parent.BackColor, 2)) 
                    {

                        this.Region = new Region(graphicsPath);
                        e.Graphics.DrawPath(pen, graphicsPath);
                    }
                }
                else this.Region = new Region(rectangleF);
            }
        }
    }
}
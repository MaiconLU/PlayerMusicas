using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace form1
{
    class gradient : Panel
    {
        public Color colortop { get; set; }
        public Color colormidle { get; set; }

        protected override void OnPaint(PaintEventArgs e)
        {
            LinearGradientBrush lgb = new LinearGradientBrush(this.ClientRectangle, this.colortop, this.colormidle, 190f);
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.FillRectangle(lgb, this.ClientRectangle);
        }
    }
}

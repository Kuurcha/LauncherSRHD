using System;
using System.Drawing;
using System.Windows.Forms;

namespace SRHDLauncher
{
	public class CustomProgressBar : ProgressBar
	{
		public ProgressBarDisplayText DisplayStyle
		{
			get;
			set;
		}

		public string CustomText
		{
			get;
			set;
		}

		public CustomProgressBar()
		{
			SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, value: true);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			Rectangle clientRectangle = base.ClientRectangle;
			Graphics graphics = e.Graphics;
			ProgressBarRenderer.DrawHorizontalBar(graphics, clientRectangle);
			clientRectangle.Inflate(-3, -3);
			if (base.Value > 0)
			{
				Rectangle bounds = new Rectangle(clientRectangle.X, clientRectangle.Y, (int)Math.Round((float)base.Value / (float)base.Maximum * (float)clientRectangle.Width), clientRectangle.Height);
				ProgressBarRenderer.DrawHorizontalChunks(graphics, bounds);
			}
			string text = (DisplayStyle == ProgressBarDisplayText.Percentage) ? (base.Value + "%") : CustomText;
			using (Font font = new Font(FontFamily.GenericSerif, 10f))
			{
				SizeF sizeF = graphics.MeasureString(text, font);
				graphics.DrawString(point: new Point(Convert.ToInt32((float)(base.Width / 2) - sizeF.Width / 2f), Convert.ToInt32((float)(base.Height / 2) - sizeF.Height / 2f)), s: text, font: font, brush: Brushes.Black);
			}
		}
	}
}

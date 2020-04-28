using System;
using System.ComponentModel;
using System.Drawing;
using System.Timers;
using System.Windows.Forms;

namespace AlphaUtils
{
	public class AlphaTextBox : TextBox
	{
		private delegate void SMDel(ref Message M);

		private class AlphaPanel : Panel
		{
			private delegate void SMDel(ref Message M);

			private AlphaTextBox MasterTb;

			private Utilities PUtils;

			protected internal Delegate ToMasterDel;

			protected internal AlphaPanel(AlphaTextBox Master)
			{
				MasterTb = Master;
				base.Size = Master.Size;
				base.Location = new Point(0, 0);
				ToMasterDel = MasterTb.STClientDel;
				SetStyle(ControlStyles.AllPaintingInWmPaint, value: true);
				SetStyle(ControlStyles.UserPaint, value: true);
				SetStyle(ControlStyles.DoubleBuffer, value: true);
				SetStyle(ControlStyles.Selectable, value: false);
				PUtils = new Utilities(ToMasterDel, MasterTb);
			}

			protected override void WndProc(ref Message m)
			{
				base.WndProc(ref m);
				if (m.Msg == PUtils.WM_MOUSEMOVE)
				{
					PUtils.SendMessageToMaster(m.Msg, m.WParam, m.LParam, -1);
				}
				else if (m.Msg == PUtils.WM_LBUTTONDOWN)
				{
					PUtils.SendMessageToMaster(m.Msg, m.WParam, m.LParam, -1);
				}
				else if (m.Msg == PUtils.WM_LBUTTONUP)
				{
					PUtils.SendMessageToMaster(m.Msg, m.WParam, m.LParam, -1);
				}
				else if (m.Msg == PUtils.WM_LBUTTONDBLCLK)
				{
					PUtils.SendMessageToMaster(m.Msg, m.WParam, m.LParam, -1);
				}
				else if (m.Msg == PUtils.WM_MOUSELEAVE)
				{
					PUtils.SendMessageToMaster(m.Msg, m.WParam, m.LParam, -1);
				}
				else if (m.Msg == PUtils.WM_RBUTTONDOWN)
				{
					PUtils.SendMessageToMaster(m.Msg, m.WParam, m.LParam, -1);
				}
				else if (m.Msg == PUtils.WM_MOUSEACTIVATE)
				{
					PUtils.SendMessageToMaster(m.Msg, m.WParam, m.LParam, -1);
				}
			}
		}

		private Container components;

		private AlphaPanel APanel;

		private Utilities TBUtils;

		private Bitmap ClientRegionBitmap;

		private PointF CaretPosition;

		private System.Timers.Timer BlinkCaretTimer;

		private bool DrawCaret;

		private bool SelectingText;

		private Color InternalAlphaBackColor;

		private bool InternalBGSet;

		private int InternalAlphaAmount;

		protected internal Delegate STClientDel;

		public override Color BackColor
		{
			get
			{
				return base.BackColor;
			}
			set
			{
				if (!InternalBGSet)
				{
					base.BackColor = Color.Transparent;
				}
				else
				{
					base.BackColor = value;
				}
			}
		}

		public override bool Multiline
		{
			get
			{
				return base.Multiline;
			}
			set
			{
				base.Multiline = value;
				UpdateRegion();
			}
		}

		[Category("Appearance")]
		[Description("The Alpha Amount, or transparency amount, applied to the background.")]
		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public int AlphaAmount
		{
			get
			{
				return InternalAlphaAmount;
			}
			set
			{
				if (value > 255 || value < 0)
				{
					throw new Exception("AlphaAmount must be between 0 and 255!");
				}
				InternalAlphaAmount = value;
				UpdateRegion();
			}
		}

		[Category("Appearance")]
		[Description("The visible background color for the AlphaTextBox.")]
		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public Color AlphaBackColor
		{
			get
			{
				return InternalAlphaBackColor;
			}
			set
			{
				InternalAlphaBackColor = value;
				UpdateRegion();
			}
		}

		public AlphaTextBox()
		{
			InitializeComponent();
		}

		private void InitializeComponent()
		{
			SetStyle(System.Windows.Forms.ControlStyles.SupportsTransparentBackColor, true);
			components = new System.ComponentModel.Container();
			STClientDel = new AlphaUtils.AlphaTextBox.SMDel(DefWndProc);
			TBUtils = new AlphaUtils.Utilities(STClientDel, this);
			InternalBGSet = false;
			DrawCaret = false;
			SelectingText = false;
			APanel = new AlphaUtils.AlphaTextBox.AlphaPanel(this);
			BlinkCaretTimer = new System.Timers.Timer(500.0);
			BlinkCaretTimer.Elapsed += new System.Timers.ElapsedEventHandler(BlinkCaretTimer_Elapsed);
			BlinkCaretTimer.AutoReset = true;
			BackColor = System.Drawing.Color.Transparent;
			base.Controls.Add(APanel);
		}

		private Bitmap CaptureClientRegion()
		{
			InternalBGSet = true;
			Bitmap bitmap = new Bitmap(base.Width, base.Height);
			BackColor = AlphaBackColor;
			Graphics graphics = Graphics.FromImage(bitmap);
			IntPtr hdc = graphics.GetHdc();
			TBUtils.SendMessageToMaster(TBUtils.WM_PRINT, hdc, (IntPtr)(TBUtils.PRF_CLIENT | TBUtils.PRF_ERASEBKGND), -1);
			graphics.ReleaseHdc(hdc);
			graphics.Dispose();
			InternalBGSet = false;
			BackColor = Color.Transparent;
			return bitmap;
		}

		private Bitmap CaptureNonClientRegion()
		{
			Bitmap bitmap = new Bitmap(base.Width, base.Height);
			Graphics graphics = Graphics.FromImage(bitmap);
			IntPtr hdc = graphics.GetHdc();
			TBUtils.SendMessageToMaster(TBUtils.WM_PRINT, hdc, (IntPtr)(TBUtils.PRF_NONCLIENT | TBUtils.PRF_ERASEBKGND), -1);
			graphics.ReleaseHdc(hdc);
			graphics.Dispose();
			return bitmap;
		}

		private void UpdateRegion()
		{
			if (ClientRegionBitmap != null)
			{
				ClientRegionBitmap.Dispose();
			}
			ClientRegionBitmap = TBUtils.MapColors(AlphaBackColor, Color.FromArgb(AlphaAmount, AlphaBackColor), CaptureClientRegion(), Dspse: true);
			APanel.BackgroundImage = (Bitmap)ClientRegionBitmap.Clone();
			SetCaret();
			GC.Collect();
		}

		private void SetCaret()
		{
			if (Text == "")
			{
				CaretPosition = new PointF(2f, 2f);
				DrawCaretToBitmap();
				return;
			}
			int num = base.SelectionStart + SelectionLength;
			bool flag = false;
			float num2 = 0f;
			string text = "";
			int num3 = 0;
			if (num == TextLength && Text[Text.Length - 1] != '\n')
			{
				num--;
			}
			else if (num == TextLength && Text[Text.Length - 1] == '\n')
			{
				flag = true;
				num--;
			}
			num3 = ((IntPtr)TBUtils.SendMessageToMaster(TBUtils.EM_POSFROMCHAR, (IntPtr)num, IntPtr.Zero, 1)).ToInt32();
			if (base.SelectionStart != TextLength)
			{
				CaretPosition = new PointF(num3 & 0xFF, (num3 >> 16) & 0xFF);
				DrawCaretToBitmap();
				return;
			}
			Graphics graphics = CreateGraphics();
			text = Text[num].ToString();
			num2 = graphics.MeasureString(text, Font).Width;
			CaretPosition = new PointF((float)(num3 & 0xFF) + num2 / 2f, (num3 >> 16) & 0xFF);
			graphics.Dispose();
			if (flag)
			{
				CaretPosition = new PointF(2f, CaretPosition.Y + (float)base.FontHeight);
			}
			DrawCaretToBitmap();
		}

		private void DrawCaretToBitmap()
		{
			Graphics graphics = Graphics.FromImage(APanel.BackgroundImage);
			if (DrawCaret)
			{
				graphics.FillRectangle(new SolidBrush(ForeColor), CaretPosition.X, CaretPosition.Y, Font.SizeInPoints / 5f, base.FontHeight);
			}
			else
			{
				APanel.BackgroundImage = (Bitmap)ClientRegionBitmap.Clone();
			}
			APanel.Refresh();
		}

		private void BlinkCaretTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			if (!SelectingText && SelectionLength > 0)
			{
				SelectingText = true;
				DrawCaret = false;
				UpdateRegion();
			}
			else if (!SelectingText)
			{
				DrawCaret = !DrawCaret;
				DrawCaretToBitmap();
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		protected override void OnGotFocus(EventArgs e)
		{
			base.OnGotFocus(e);
			BlinkCaretTimer.Start();
			UpdateRegion();
		}

		protected override void OnLostFocus(EventArgs e)
		{
			base.OnLostFocus(e);
			DrawCaret = false;
			UpdateRegion();
			BlinkCaretTimer.Stop();
		}

		protected override void OnForeColorChanged(EventArgs e)
		{
			if (!InternalBGSet)
			{
				base.OnForeColorChanged(e);
				UpdateRegion();
			}
		}

		protected override void OnFontChanged(EventArgs e)
		{
			base.OnFontChanged(e);
			UpdateRegion();
		}

		protected override void OnBorderStyleChanged(EventArgs e)
		{
			base.OnBorderStyleChanged(e);
			UpdateRegion();
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);
			APanel.Size = base.ClientSize;
			UpdateRegion();
		}

		protected override void OnVisibleChanged(EventArgs e)
		{
			base.OnVisibleChanged(e);
			APanel.Visible = base.Visible;
		}

		protected override void OnTextChanged(EventArgs e)
		{
			base.OnTextChanged(e);
			DrawCaret = true;
			if (!BlinkCaretTimer.Enabled)
			{
				BlinkCaretTimer.Start();
			}
			UpdateRegion();
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			if (SelectionLength == 0 && SelectingText)
			{
				SelectingText = false;
				DrawCaret = true;
			}
			if (!BlinkCaretTimer.Enabled)
			{
				BlinkCaretTimer.Start();
			}
			UpdateRegion();
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Right || e.KeyCode == Keys.Next || e.KeyCode == Keys.Prior || e.KeyCode == Keys.Home || e.KeyCode == Keys.End)
			{
				DrawCaret = true;
				if (!BlinkCaretTimer.Enabled)
				{
					BlinkCaretTimer.Start();
				}
				UpdateRegion();
			}
		}

		protected override void OnKeyUp(KeyEventArgs e)
		{
			base.OnKeyUp(e);
			if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Right || e.KeyCode == Keys.Next || e.KeyCode == Keys.Prior || e.KeyCode == Keys.Home || e.KeyCode == Keys.End)
			{
				UpdateRegion();
			}
			else if (SelectionLength == 0 && SelectingText)
			{
				SelectingText = false;
				DrawCaret = true;
				if (!BlinkCaretTimer.Enabled)
				{
					BlinkCaretTimer.Start();
				}
				UpdateRegion();
			}
		}

		protected override void WndProc(ref Message m)
		{
			base.WndProc(ref m);
			if (m.Msg == TBUtils.WM_HSCROLL || m.Msg == TBUtils.WM_VSCROLL || m.Msg == TBUtils.WM_MOUSEWHEEL)
			{
				DrawCaret = false;
				BlinkCaretTimer.Stop();
				UpdateRegion();
			}
			else if (m.Msg == TBUtils.WM_MOUSEMOVE && (int)m.WParam != 0)
			{
				DrawCaret = false;
				SelectingText = true;
				UpdateRegion();
			}
		}

		public Bitmap GetScreenShot()
		{
			Bitmap bitmap = CaptureNonClientRegion();
			Bitmap bitmap2 = TBUtils.MapColors(AlphaBackColor, Color.FromArgb(AlphaAmount, AlphaBackColor), CaptureClientRegion(), Dspse: true);
			Graphics graphics = Graphics.FromImage(bitmap);
			graphics.DrawImage(bitmap2, 0, 0, base.ClientSize.Width, base.ClientSize.Height);
			graphics.Dispose();
			bitmap2.Dispose();
			return bitmap;
		}
	}
}

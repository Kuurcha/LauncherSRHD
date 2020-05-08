using System;
using System.ComponentModel;
using System.Drawing;
using System.Timers;
using System.Windows.Forms;

namespace AlphaUtils
{
	public class AlphaRichTextBox : RichTextBox
	{
		private delegate void SMDel(ref Message M);

		private class AlphaPanel : Panel
		{
			private delegate void SMDel(ref Message M);

			private AlphaRichTextBox MasterTb;

			private Utilities PUtils;

			protected Delegate ToMasterDel;

			public AlphaPanel(AlphaRichTextBox Master)
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

		private RichTextInformationCollection SelectedRTInfopublic;

		private Color publicAlphaBackColor;

		private Color CaretColorpublic;

		private int publicAlphaAmount;

		private int TmpSelStart;

		private int TmpSelLength;

		private bool Updating;

		private bool DrawingCaret;

		private bool Scrolling;

		private bool Ctrl;

		private bool Dlt;

		private bool NewLine;

		protected Delegate STClientDel;

		public override Color BackColor
		{
			get
			{
				return base.BackColor;
			}
			set
			{
				base.BackColor = Color.Transparent;
			}
		}

		[Category("Appearance")]
		[Description("The Alpha Amount, or transparency amount, applied to the background.")]
		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		protected int AlphaAmount
		{
			get
			{
				return publicAlphaAmount;
			}
			set
			{
				if (value > 255 || value < 0)
				{
					throw new Exception("AlphaAmount must be between 0 and 255!");
				}
				publicAlphaAmount = value;
				UpdateRegion();
				APanel.Refresh();
			}
		}

		[Category("Appearance")]
		[Description("The visible background color for the AlphaTextBox.")]
		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		protected Color AlphaBackColor
		{
			get
			{
				return publicAlphaBackColor;
			}
			set
			{
				for (int i = 0; i < SelectedRTInfopublic.Count; i++)
				{
					if (SelectedRTInfopublic[i].BackColorpublic.ToArgb() == publicAlphaBackColor.ToArgb())
					{
						SelectedRTInfopublic[i].BackColorpublic = value;
					}
				}
				for (int j = 0; j < TextLength; j++)
				{
					Select(j, 1);
					if (SelectionBackColor.ToArgb() == publicAlphaBackColor.ToArgb())
					{
						SelectionBackColor = value;
					}
				}
				Select(TmpSelStart, TmpSelLength);
				publicAlphaBackColor = value;
				UpdateRegion();
				APanel.Refresh();
			}
		}

		[Category("Appearance")]
		[Description("")]
		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		protected RichTextInformationCollection SelectedRTInfo => SelectedRTInfopublic;

		[Category("Appearance")]
		[Description("")]
		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		protected new Color SelectionBackColor
		{
			get
			{
				Color fore = Color.Empty;
				Color back = Color.Empty;
				TBUtils.GetRTHighlight(ref back, ref fore, Color.Empty);
				return back;
			}
			set
			{
				if (!Focused)
				{
					Focus();
				}
				TBUtils.SetRTHighlight(value, SelectionColor);
				for (int i = 0; i < SelectedRTInfopublic.Count; i++)
				{
					SelectedRTInfopublic[i].BackColorpublic = value;
				}
			}
		}

		protected new Color SelectionColor
		{
			get
			{
				return base.SelectionColor;
			}
			set
			{
				if (!Focused)
				{
					Focus();
				}
				base.SelectionColor = value;
				for (int i = 0; i < SelectedRTInfopublic.Count; i++)
				{
					SelectedRTInfopublic[i].ForeColorpublic = value;
				}
				TBUtils.SetRTHighlight(SelectionBackColor, SelectionColor);
			}
		}

		protected new Font SelectionFont
		{
			get
			{
				return base.SelectionFont;
			}
			set
			{
				if (!Focused)
				{
					Focus();
				}
				for (int i = 0; i < SelectedRTInfopublic.Count; i++)
				{
					SelectedRTInfopublic[i].Fontpublic = value;
				}
				base.SelectionFont = value;
				UpdateRegion();
			}
		}

		protected Color CaretColor
		{
			get
			{
				return CaretColorpublic;
			}
			set
			{
				CaretColorpublic = value;
				if (!Focused)
				{
					Focus();
				}
			}
		}

		protected AlphaRichTextBox()
		{
			InitializeComponent();
		}

		private void InitializeComponent()
		{
			SetStyle(System.Windows.Forms.ControlStyles.SupportsTransparentBackColor, true);
			components = new System.ComponentModel.Container();
			STClientDel = new AlphaUtils.AlphaRichTextBox.SMDel(DefWndProc);
			TBUtils = new AlphaUtils.Utilities(STClientDel, this);
			DrawCaret = true;
			Updating = false;
			DrawingCaret = false;
			Scrolling = false;
			Ctrl = false;
			NewLine = false;
			APanel = new AlphaUtils.AlphaRichTextBox.AlphaPanel(this);
			TmpSelStart = -1;
			BlinkCaretTimer = new System.Timers.Timer(500.0);
			BlinkCaretTimer.Elapsed += new System.Timers.ElapsedEventHandler(BlinkCaretTimer_Elapsed);
			BlinkCaretTimer.AutoReset = true;
			BlinkCaretTimer.Enabled = false;
			SelectedRTInfopublic = new AlphaUtils.RichTextInformationCollection();
			base.Controls.Add(APanel);
		}

		private void UpdateRegion()
		{
			if (!Updating)
			{
				IntPtr lParams = (IntPtr)TBUtils.SendMessageToMaster(TBUtils.EM_SETEVENTMASK, IntPtr.Zero, IntPtr.Zero, 1);
				TBUtils.SendMessageToMaster(TBUtils.WM_SETREDRAW, IntPtr.Zero, IntPtr.Zero, -1);
				Updating = true;
				WriteText();
				if (TmpSelLength == 0)
				{
					WriteBitmap();
				}
				TBUtils.SendMessageToMaster(TBUtils.WM_SETREDRAW, (IntPtr)1, IntPtr.Zero, -1);
				TBUtils.SendMessageToMaster(TBUtils.EM_SETEVENTMASK, IntPtr.Zero, lParams, -1);
				if (TmpSelLength > 0)
				{
					WriteBitmap();
				}
				if (DrawCaret && TmpSelLength == 0 && !Scrolling)
				{
					SetCaret();
				}
				Updating = false;
			}
		}

		private void WriteText()
		{
			int selectionStart = base.SelectionStart;
			int selectionLength = SelectionLength;
			if (selectionLength > 0)
			{
				DrawCaret = false;
				BlinkCaretTimer.Stop();
			}
			else if (Focused)
			{
				DrawCaret = true;
				BlinkCaretTimer.Start();
			}
			if (SelectedRTInfopublic.Count != 0 && !Dlt)
			{
				for (int i = 0; i < TmpSelLength; i++)
				{
					int start = i + TmpSelStart;
					Select(start, 1);
					TBUtils.SetRTHighlight(SelectedRTInfopublic[i].BackColor, SelectedRTInfopublic[i].ForeColor);
				}
				Select(selectionStart, selectionLength);
			}
			TmpSelStart = selectionStart;
			TmpSelLength = selectionLength;
			if (selectionLength > 0 && !Dlt)
			{
				SelectedRTInfopublic.Clear();
				for (int j = 0; j < selectionLength; j++)
				{
					Select(j + selectionStart, 1);
					Color fore = Color.Empty;
					Color back = Color.Empty;
					Font font = null;
					TBUtils.GetRTHighlight(ref back, ref fore, AlphaBackColor);
					font = SelectionFont;
					SelectedRTInfopublic.Add(new RichTextInformation(font, fore, back));
					TBUtils.SetRTHighlight(SystemColors.Highlight, SystemColors.HighlightText);
				}
			}
			else if (Dlt)
			{
				Color foreColor = SelectedRTInfopublic[SelectedRTInfopublic.Count - 1].ForeColor;
				Color backColor = SelectedRTInfopublic[SelectedRTInfopublic.Count - 1].BackColor;
				SelectedRTInfopublic.Clear();
				SelectionColor = foreColor;
				SelectionBackColor = backColor;
				Dlt = false;
			}
			else
			{
				SelectedRTInfopublic.Clear();
			}
			Select(TmpSelStart, TmpSelLength);
		}

		private void WriteBitmap()
		{
			base.BackColor = AlphaBackColor;
			Bitmap bitmap = new Bitmap(base.ClientSize.Width, base.ClientSize.Height);
			Graphics g = Graphics.FromImage(bitmap);
			int charIndexFromPosition = GetCharIndexFromPosition(new Point(2, 2));
			int endChar = GetCharIndexFromPosition(new Point(base.ClientSize.Width, base.ClientSize.Height)) + 1;
			TBUtils.FormatRange(g, charIndexFromPosition, endChar);
			ClientRegionBitmap = TBUtils.MapRichTextColors(AlphaBackColor, Color.FromArgb(AlphaAmount, AlphaBackColor), bitmap, Dspse: true);
			base.BackColor = Color.Transparent;
			APanel.BackgroundImage = (Bitmap)ClientRegionBitmap.Clone();
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
			if (TmpSelStart != TextLength)
			{
				CaretPosition = new PointF(num3 & 0xFF, (num3 >> 16) & 0xFF);
			}
			else
			{
				Graphics graphics = CreateGraphics();
				text = Text[num].ToString();
				num2 = graphics.MeasureString(text, SelectionFont).Width;
				CaretPosition = new PointF((float)(num3 & 0xFF) + num2 / 2f, (num3 >> 16) & 0xFF);
				graphics.Dispose();
			}
			if (flag)
			{
				CaretPosition = new PointF(2f, CaretPosition.Y + (float)SelectionFont.Height);
			}
			else
			{
				int i = (int)(IntPtr)TBUtils.SendMessageToMaster(TBUtils.EM_LINEINDEX, (IntPtr)(-1), IntPtr.Zero, 1);
				int num4 = i + (int)(IntPtr)TBUtils.SendMessageToMaster(TBUtils.EM_LINELENGTH, (IntPtr)i, IntPtr.Zero, 1);
				int num5 = 0;
				Color back = Color.Empty;
				Color fore = Color.Empty;
				TBUtils.GetRTHighlight(ref back, ref fore, AlphaBackColor);
				Font selectionFont = SelectionFont;
				for (; i < num4; i++)
				{
					Select(i, 1);
					if (SelectionFont.Height > num5)
					{
						num5 = SelectionFont.Height;
					}
				}
				Select(TmpSelStart, 0);
				base.SelectionFont = selectionFont;
				TBUtils.SetRTHighlight(back, fore);
				CaretPosition.Y = CaretPosition.Y + (float)num5 - (float)(num5 / 5) - (float)selectionFont.Height;
			}
			NewLine = flag;
			DrawCaretToBitmap();
		}

		private void DrawCaretToBitmap()
		{
			DrawingCaret = true;
			if (DrawCaret)
			{
				Graphics graphics = Graphics.FromImage(APanel.BackgroundImage);
				if (NewLine)
				{
					graphics.FillRectangle((CaretColorpublic.ToArgb() == Color.Empty.ToArgb()) ? new SolidBrush(SelectionColor) : new SolidBrush(CaretColorpublic), CaretPosition.X, CaretPosition.Y, SelectionFont.SizeInPoints / 5f, SelectionFont.Height);
					graphics.Dispose();
				}
				else
				{
					graphics.FillRectangle((CaretColorpublic.ToArgb() == Color.Empty.ToArgb()) ? new SolidBrush(SelectionColor) : new SolidBrush(CaretColorpublic), CaretPosition.X, CaretPosition.Y, SelectionFont.SizeInPoints / 5f, SelectionFont.Height);
					graphics.Dispose();
				}
			}
			else
			{
				APanel.BackgroundImage = (Bitmap)ClientRegionBitmap.Clone();
			}
			APanel.Refresh();
			DrawingCaret = false;
		}

		private void BlinkCaretTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			if (!Updating && !DrawingCaret && !Scrolling)
			{
				DrawCaret = !DrawCaret;
				DrawCaretToBitmap();
			}
		}

		protected override void OnVisibleChanged(EventArgs e)
		{
			base.OnVisibleChanged(e);
			if (!Focused)
			{
				DrawCaret = false;
				BlinkCaretTimer.Stop();
			}
			UpdateRegion();
		}

		protected new void Copy()
		{
			IntPtr lParams = (IntPtr)TBUtils.SendMessageToMaster(TBUtils.EM_SETEVENTMASK, IntPtr.Zero, IntPtr.Zero, 1);
			TBUtils.SendMessageToMaster(TBUtils.WM_SETREDRAW, IntPtr.Zero, IntPtr.Zero, -1);
			RichTextInformationCollection selectedRTInfoprotected = SelectedRTInfopublic;
			int tmpSelStart = TmpSelStart;
			int tmpSelLength = TmpSelLength;
			Select(TextLength, 0);
			WriteText();
			Select(tmpSelStart, tmpSelLength);
			Clipboard.SetDataObject(base.SelectedRtf, copy: true);
			SelectedRTInfopublic = selectedRTInfoprotected;
			WriteText();
			TBUtils.SendMessageToMaster(TBUtils.WM_SETREDRAW, (IntPtr)1, IntPtr.Zero, -1);
			TBUtils.SendMessageToMaster(TBUtils.EM_SETEVENTMASK, IntPtr.Zero, lParams, -1);
		}

		protected new void Paste()
		{
			IDataObject dataObject = Clipboard.GetDataObject();
			if (dataObject.GetDataPresent(typeof(string)))
			{
				string text = (string)dataObject.GetData(typeof(string));
				if (text.StartsWith("{"))
				{
					base.SelectedRtf = text;
				}
				else
				{
					SelectedText = text;
				}
			}
			else
			{
				if (dataObject.GetDataPresent(typeof(Bitmap)))
				{
					Bitmap bmp = (Bitmap)dataObject.GetData(typeof(Bitmap));
					Clipboard.SetDataObject(TBUtils.MapColors(AlphaBackColor, Color.FromArgb((AlphaBackColor.R != byte.MaxValue) ? (AlphaBackColor.R + 1) : (AlphaBackColor.R - 1), AlphaBackColor.G, AlphaBackColor.B), bmp, Dspse: true), copy: true);
				}
				base.Paste();
			}
			UpdateRegion();
		}

		protected new void Paste(DataFormats.Format ClipFormat)
		{
			Paste();
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);
			APanel.Size = base.ClientSize;
			if (ClientRegionBitmap != null)
			{
				ClientRegionBitmap.Dispose();
			}
			ClientRegionBitmap = new Bitmap(base.ClientSize.Width, base.ClientSize.Height);
			UpdateRegion();
		}

		protected override void OnKeyUp(KeyEventArgs e)
		{
			base.OnKeyUp(e);
			Ctrl = false;
			UpdateRegion();
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			Scrolling = false;
			UpdateRegion();
		}

		protected override void OnLostFocus(EventArgs e)
		{
			base.OnLostFocus(e);
			BlinkCaretTimer.Stop();
			DrawCaret = false;
			DrawCaretToBitmap();
		}

		protected override void OnGotFocus(EventArgs e)
		{
			base.OnGotFocus(e);
			DrawCaret = true;
			BlinkCaretTimer.Start();
		}

		protected override void WndProc(ref Message m)
		{
			if (m.Msg == TBUtils.WM_KEYDOWN)
			{
				int num = m.WParam.ToInt32();
				if (num == 46 && SelectedRTInfopublic.Count > 0)
				{
					Dlt = true;
					base.WndProc(ref m);
					return;
				}
				int num2;
				switch (num)
				{
					case 17:
						Ctrl = true;
						base.WndProc(ref m);
						return;
					case 67:
						num2 = (Ctrl ? 1 : 0);
						break;
					default:
						num2 = 0;
						break;
				}
				if (num2 != 0)
				{
					Copy();
					Ctrl = false;
					return;
				}
				if (num == 86 && Ctrl)
				{
					Paste();
					Ctrl = false;
					return;
				}
				Ctrl = false;
				base.WndProc(ref m);
				Scrolling = false;
				UpdateRegion();
			}
			else if (m.Msg == TBUtils.WM_MOUSEMOVE)
			{
				int num3 = m.LParam.ToInt32();
				int num4 = m.WParam.ToInt32();
				if (num4 == TBUtils.MK_LBUTTON)
				{
					if (Updating)
					{
						base.WndProc(ref m);
						return;
					}
					int x = num3 & 0xFF;
					int y = (num3 >> 16) & 0xFF;
					int num5 = GetCharIndexFromPosition(new Point(x, y)) + 1;
					if (num5 > base.SelectionStart)
					{
						Select(base.SelectionStart, num5 - base.SelectionStart);
					}
					else
					{
						int num6 = num5 + (TmpSelStart - num5 + TmpSelLength);
						if (base.SelectionStart + SelectionLength == TextLength && num6 > 0)
						{
							Select(num5, num6);
						}
						else if (num6 - num5 > 0)
						{
							Select(num5, num6 - num5);
						}
					}
					UpdateRegion();
				}
				else
				{
					base.WndProc(ref m);
				}
			}
			else if (m.Msg == TBUtils.WM_VSCROLL)
			{
				base.WndProc(ref m);
				int num7 = m.WParam.ToInt32();
				Scrolling = true;
				DrawCaret = false;
				if ((num7 & TBUtils.SB_LINEUP) == 0 || (num7 & TBUtils.SB_LINEDOWN) == 0)
				{
					WriteBitmap();
				}
			}
			else if (m.Msg == TBUtils.WM_MOUSEWHEEL)
			{
				base.WndProc(ref m);
				Scrolling = true;
				DrawCaret = false;
				WriteBitmap();
			}
			else
			{
				base.WndProc(ref m);
			}
		}
	}
}

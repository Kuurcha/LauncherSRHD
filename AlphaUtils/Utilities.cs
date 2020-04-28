using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace AlphaUtils
{
	public class Utilities
	{
		private struct STRUCT_RECT
		{
			public int left;

			public int top;

			public int right;

			public int bottom;
		}

		private struct STRUCT_CHARRANGE
		{
			public int cpMin;

			public int cpMax;
		}

		private struct STRUCT_FORMATRANGE
		{
			public IntPtr hdc;

			public IntPtr hdcTarget;

			public STRUCT_RECT rc;

			public STRUCT_RECT rcPage;

			public STRUCT_CHARRANGE chrg;
		}

		private struct CHARFORMAT2
		{
			public int cbSize;

			public int dwMask;

			public int dwEffects;

			public int yHeight;

			public int yOffset;

			public int crTextColor;

			public byte bCharSet;

			public byte bPitchAndFamily;

			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
			public string szFaceName;

			public ushort wWeight;

			public ushort sSpacing;

			public int crBackColor;

			public int lcid;

			public int dwReserved;

			public ushort sStyle;

			public ushort wKerning;

			public byte bUnderlineType;

			public byte bAnimation;

			public byte bRevAuthor;

			public byte bReserved1;
		}

		private Control MasterControl;

		private Delegate MasterDelegate;

		private ColorMap[] CMap;

		private ImageAttributes IAttribs;

		protected internal readonly int EM_GETLINECOUNT = 186;

		protected internal readonly int EM_LINEINDEX = 187;

		protected internal readonly int EM_LINELENGTH = 193;

		protected internal readonly int EM_LINEFROMCHAR = 201;

		protected internal readonly int EM_GETSEL = 176;

		protected internal readonly int EM_GETFIRSTVISIBLELINE = 206;

		protected internal readonly int EM_SETEVENTMASK = 1073;

		protected internal readonly int EM_POSFROMCHAR = 214;

		protected internal readonly int EN_UPDATE = 1024;

		protected internal readonly int EM_MOUSESELECT = 255;

		protected internal readonly int WM_PRINT = 791;

		protected internal readonly int PRF_ERASEBKGND = 8;

		protected internal readonly int PRF_CLIENT = 4;

		protected internal readonly int PRF_NONCLIENT = 2;

		protected internal readonly int WM_MOUSEMOVE = 512;

		protected internal readonly int WM_LBUTTONDOWN = 513;

		protected internal readonly int WM_LBUTTONUP = 514;

		protected internal readonly int WM_RBUTTONDOWN = 516;

		protected internal readonly int WM_LBUTTONDBLCLK = 515;

		protected internal readonly int WM_MOUSELEAVE = 675;

		protected internal readonly int WM_MOUSEACTIVATE = 33;

		protected internal readonly int WM_HSCROLL = 276;

		protected internal readonly int WM_VSCROLL = 277;

		protected internal readonly int WM_MOUSEWHEEL = 522;

		protected internal readonly int WM_SETREDRAW = 11;

		protected internal readonly int WM_KEYDOWN = 256;

		protected internal readonly int EM_FORMATRANGE = 1081;

		protected internal readonly int EM_SETCHARFORMAT = 1092;

		protected internal readonly int EM_GETCHARFORMAT = 1082;

		protected internal readonly int CFM_BACKCOLOR = 67108864;

		protected internal readonly int CFM_COLOR = 1073741824;

		protected internal readonly int CFE_AUTOBACKCOLOR = 67108864;

		protected internal readonly int SCF_SELECTION = 1;

		protected internal readonly int MK_LBUTTON = 1;

		protected internal readonly int SB_LINEUP = 0;

		protected internal readonly int SB_LINEDOWN = 1;

		protected internal Utilities(Delegate DMaster, Control Master)
		{
			MasterDelegate = DMaster;
			MasterControl = Master;
		}

		private int ToTwips(float amt)
		{
			return (int)(amt * 14.4f);
		}

		protected internal void FormatRange(Graphics g, int startChar, int endChar)
		{
			STRUCT_CHARRANGE chrg = default(STRUCT_CHARRANGE);
			chrg.cpMin = startChar;
			chrg.cpMax = endChar;
			STRUCT_RECT rc = default(STRUCT_RECT);
			rc.top = 0;
			rc.bottom = ToTwips(MasterControl.ClientSize.Height + 40);
			rc.left = 0;
			if (MasterControl.Size.Width - MasterControl.ClientSize.Width == 20)
			{
				rc.right = ToTwips((float)MasterControl.ClientSize.Width + (float)MasterControl.ClientSize.Width / 80f + 4f);
			}
			else
			{
				rc.right = ToTwips((float)MasterControl.ClientSize.Width + (float)MasterControl.ClientSize.Width / 100f + 5f);
			}
			STRUCT_RECT rcPage = default(STRUCT_RECT);
			rcPage.top = 0;
			rcPage.bottom = ToTwips(MasterControl.Size.Height);
			rcPage.left = 0;
			rcPage.right = ToTwips(MasterControl.Size.Width);
			IntPtr hdc = g.GetHdc();
			STRUCT_FORMATRANGE structure = default(STRUCT_FORMATRANGE);
			structure.chrg = chrg;
			structure.hdc = hdc;
			structure.hdcTarget = hdc;
			structure.rc = rc;
			structure.rcPage = rcPage;
			IntPtr intPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(structure));
			Marshal.StructureToPtr(structure, intPtr, fDeleteOld: false);
			SendMessageToMaster(EM_FORMATRANGE, (IntPtr)1, intPtr, 1);
			SendMessageToMaster(EM_FORMATRANGE, IntPtr.Zero, IntPtr.Zero, -1);
			Marshal.FreeCoTaskMem(intPtr);
			g.ReleaseHdc(hdc);
		}

		protected internal void SetRTHighlight(Color back, Color fore)
		{
			CHARFORMAT2 structure = default(CHARFORMAT2);
			structure.dwMask = (CFM_BACKCOLOR | CFM_COLOR);
			structure.cbSize = Marshal.SizeOf(structure);
			structure.crBackColor = ColorTranslator.ToWin32(back);
			structure.crTextColor = ColorTranslator.ToWin32(fore);
			IntPtr intPtr = Marshal.AllocCoTaskMem(structure.cbSize);
			Marshal.StructureToPtr(structure, intPtr, fDeleteOld: false);
			SendMessageToMaster(EM_SETCHARFORMAT, (IntPtr)SCF_SELECTION, intPtr, 1);
			Marshal.FreeCoTaskMem(intPtr);
		}

		protected internal void GetRTHighlight(ref Color back, ref Color fore, Color AlphaBackColor)
		{
			CHARFORMAT2 structure = default(CHARFORMAT2);
			structure.cbSize = Marshal.SizeOf(structure);
			IntPtr intPtr = Marshal.AllocCoTaskMem(structure.cbSize);
			Marshal.StructureToPtr(structure, intPtr, fDeleteOld: false);
			intPtr = (IntPtr)SendMessageToMaster(EM_GETCHARFORMAT, (IntPtr)SCF_SELECTION, intPtr, 3);
			structure = (CHARFORMAT2)Marshal.PtrToStructure(intPtr, typeof(CHARFORMAT2));
			back = ((structure.crBackColor == 0) ? AlphaBackColor : ColorTranslator.FromWin32(structure.crBackColor));
			fore = ColorTranslator.FromWin32(structure.crTextColor);
			Marshal.FreeCoTaskMem(intPtr);
		}

		protected internal object SendMessageToMaster(int Msg, IntPtr WParams, IntPtr LParams, int ReturnInstance)
		{
			Message message = Message.Create(MasterControl.Handle, Msg, WParams, LParams);
			object[] array = new object[1]
			{
				message
			};
			MasterDelegate.DynamicInvoke(array);
			switch (ReturnInstance)
			{
			case 0:
				return array[0];
			case 1:
				return ((Message)array[0]).Result;
			case 2:
				return ((Message)array[0]).WParam;
			case 3:
				return ((Message)array[0]).LParam;
			default:
				return -1;
			}
		}

		protected internal Bitmap MapColors(Color Old, Color New, Bitmap Bmp, bool Dspse)
		{
			Bitmap bitmap = new Bitmap(Bmp.Width, Bmp.Height);
			Graphics graphics = Graphics.FromImage(bitmap);
			CMap = new ColorMap[1];
			CMap[0] = new ColorMap();
			CMap[0].OldColor = Old;
			CMap[0].NewColor = New;
			IAttribs = new ImageAttributes();
			IAttribs.SetRemapTable(CMap, ColorAdjustType.Bitmap);
			graphics.DrawImage(Bmp, new Rectangle(new Point(0, 0), new Size(Bmp.Width, Bmp.Height)), 0, 0, Bmp.Width, Bmp.Height, GraphicsUnit.Pixel, IAttribs);
			graphics.Dispose();
			if (Dspse)
			{
				Bmp.Dispose();
			}
			return bitmap;
		}

		protected internal Bitmap MapRichTextColors(Color Old, Color New, Bitmap Bmp, bool Dspse)
		{
			Bitmap bitmap = new Bitmap(MasterControl.ClientSize.Width, MasterControl.ClientSize.Height);
			Graphics graphics = Graphics.FromImage(bitmap);
			graphics.Clear(New);
			CMap = new ColorMap[1];
			CMap[0] = new ColorMap();
			CMap[0].OldColor = Old;
			CMap[0].NewColor = Color.Transparent;
			IAttribs = new ImageAttributes();
			IAttribs.SetRemapTable(CMap, ColorAdjustType.Bitmap);
			graphics.DrawImage(Bmp, new Rectangle(new Point(0, 0), new Size(Bmp.Width, Bmp.Height)), 0, 0, Bmp.Width, Bmp.Height, GraphicsUnit.Pixel, IAttribs);
			graphics.Dispose();
			if (Dspse)
			{
				Bmp.Dispose();
			}
			return bitmap;
		}
	}
}

using System.Drawing;

namespace AlphaUtils
{
	public class RichTextInformation
	{
		public Font Fontpublic;

		public Color ForeColorpublic;

		public Color BackColorpublic;

		public Font Font => Fontpublic;

		public Color ForeColor => ForeColorpublic;

		public Color BackColor => BackColorpublic;

		public RichTextInformation(Font fnt, Color fore, Color back)
		{
			Fontpublic = fnt;
			ForeColorpublic = fore;
			BackColorpublic = back;
		}

		public RichTextInformation()
		{
		}
	}
}

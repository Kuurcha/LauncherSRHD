using System.Drawing;

namespace AlphaUtils
{
	public class RichTextInformation
	{
		protected internal Font FontInternal;

		protected internal Color ForeColorInternal;

		protected internal Color BackColorInternal;

		public Font Font => FontInternal;

		public Color ForeColor => ForeColorInternal;

		public Color BackColor => BackColorInternal;

		public RichTextInformation(Font fnt, Color fore, Color back)
		{
			FontInternal = fnt;
			ForeColorInternal = fore;
			BackColorInternal = back;
		}

		public RichTextInformation()
		{
		}
	}
}

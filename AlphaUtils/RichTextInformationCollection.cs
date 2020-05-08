using System.Collections;

namespace AlphaUtils
{
    public class RichTextInformationCollection : CollectionBase
    {
        public RichTextInformation this[int index]
        {
            get
            {
                return (RichTextInformation)base.InnerList[index];
            }
            set
            {
                base.InnerList[index] = value;
            }
        }

        public new int Count => base.InnerList.Count;

        public void Add(RichTextInformation value)
        {
            base.InnerList.Add(value);
        }

        public new void Clear()
        {
            base.InnerList.Clear();
        }
    }
}

namespace SRHDLauncher
{
    public class ZipProgress
    {
        public int Total
        {
            get;
        }

        public int Processed
        {
            get;
        }

        public string CurrentItem
        {
            get;
        }

        public ZipProgress(int total, int processed, string currentItem)
        {
            Total = total;
            Processed = processed;
            CurrentItem = currentItem;
        }
    }
}

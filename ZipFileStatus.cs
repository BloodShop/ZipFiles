namespace ZipFiles
{
    public delegate void ZipFileEventHandler(object sender, ZipFileEventArgs e);
    public class ZipFileEventArgs : EventArgs
    {
        public int PercentageComplete { get; set; } = 0;
        public List<string> Archives { get; set; } = new();
        public string Message { get; set; }
    }
}

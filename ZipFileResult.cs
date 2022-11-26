namespace ZipFiles
{
    internal class ZipFileResult
    {
        public string? Message { get; set; }
        public bool Success { get; set; }
        public List<string> ZipFiles { get; set; }

        public override string ToString() => $"Result: {Success}\nFiles: {string.Join('\n', ZipFiles)}";
    }
}
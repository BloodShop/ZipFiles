using System.IO.Compression;
using System.Text;

namespace ZipFiles
{
    internal class ZipFileService
    {
        public event ZipFileEventHandler ZipProgressEvent;

        double _currentArchiveSize = 0;
        double _maxArchiveSize = 4.29 * 10e9;
        //double _maxArchiveSize = 2097152; // 2MB Approx

        List<string> _archives = new();
        List<string> _filesInCurrentArchive = new();

        ZipArchive _zip = null;

        readonly string _basePath;

        public ZipFileService(string basePath)
        {
            this._basePath = basePath;

            if (!Directory.Exists(basePath))
            {
                Directory.CreateDirectory(basePath);
            }
        }

        public ZipFileResult CompressFiles(IEnumerable<string> filesPaths)
        {
            var sb = new StringBuilder();
            bool isError = false;
            try
            {
                var numOfFiles = filesPaths.Count();
                for (int i = 0; i < filesPaths.Count(); i++)
                {
                    var fileInfo = new FileInfo(filesPaths.ElementAt(i));
                    if (!fileInfo.Exists)
                        continue;

                    AddToArchive(fileInfo);

                    //
                    SendProgress(i + 1, numOfFiles);
                }
            }
            catch (Exception e)
            {
                sb.AppendLine(e.Message);
                isError = true;
            }
            finally
            {
                _zip?.Dispose();

                if (isError)
                    DeleteExisitingArchives();
            }

            return new ZipFileResult
            {
                Message = sb.ToString(),
                Success = !isError,
                ZipFiles = _archives
            };
        }

        void SendProgress(int currentFileIndex, int numOfFiles)
        {
            this.ZipProgressEvent?.Invoke(this, new ZipFileEventArgs
            {
                Archives = _archives,
                Message = "",
                PercentageComplete = (currentFileIndex * 100) / numOfFiles
            });
        }

        private void DeleteExisitingArchives()
        {
            foreach (var archive in _archives)
                if (File.Exists(archive))
                    File.Delete(archive);

        }

        void AddToArchive(FileInfo fileInfo)
        {
            if (_zip == null || _currentArchiveSize > _maxArchiveSize || _currentArchiveSize + fileInfo.Length >= _maxArchiveSize)
            {
                // Create new zip archive
                CreateZipArchive();
            }

            var filename = GetUniqueFileName(fileInfo.Name);
            _zip.CreateEntryFromFile(fileInfo.FullName, filename);

            _currentArchiveSize += fileInfo.Length;

            _filesInCurrentArchive.Add(filename);
        }

        string GetZipFilePath()
        {
            return $@"{_basePath}\{Guid.NewGuid()}.zip";
        }

        void CreateZipArchive()
        {
            _zip?.Dispose();


            var zipFilename = GetZipFilePath();
            _zip = ZipFile.Open(zipFilename, ZipArchiveMode.Create);

            _currentArchiveSize = 0;
            _filesInCurrentArchive = new();

            _archives.Add(zipFilename);
        }

        string GetUniqueFileName(string filePath)
        {
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
            var extension = Path.GetExtension(filePath);
            var newFileName = Path.GetFileName(filePath);

            int count = 0;
            // Check if the current archive has the newFileName
            while (_filesInCurrentArchive.Any(r => r == newFileName))
            {
                count++;
                newFileName = $"{fileNameWithoutExtension} ({count}){extension}";
            }

            return newFileName;
        }
    }
}

using System.IO.Compression;

Console.WriteLine("Hello world! This is my zipping program!");

IEnumerable<string> GetFilePaths(List<string> paths)
{
    var filePaths = new List<string>();

    foreach (var path in paths)
        filePaths.AddRange(Directory.GetFiles(path));

    return filePaths;
}

void DeleteExistingZipFiles()
{
    var existingZipFiles = Directory.GetFiles(@"G:\SeLa\Self\ZipFiles", "*.zip", SearchOption.AllDirectories);

    foreach (var file in existingZipFiles)
        if (File.Exists(file))
            File.Delete(file);
}

void ZipFiles()
{
    var sourceFilesDirectory = new List<string> { @"G:\SeLa\Self\ZipFiles\Zip\sounds" };

    DeleteExistingZipFiles();

    var filesPaths = GetFilePaths(sourceFilesDirectory);

    var zipFilename = @"G:\SeLa\Self\ZipFiles\Zip\MyZipFile.zip";
    using var zip = ZipFile.Open(zipFilename, ZipArchiveMode.Create);

    foreach (var filePath in filesPaths)
    {
        var filename = Path.GetFileName(filePath);
        zip.CreateEntryFromFile(filePath, filename);
    }

}

ZipFiles();
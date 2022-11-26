using System.IO.Compression;
using ZipFiles;

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
    var workingDirectory = Environment.CurrentDirectory;
    var basePath = Directory.GetParent(workingDirectory).Parent.Parent.FullName + "\\Zip";

    var sourceFilesDirectory = new List<string> 
    {
        $@"{basePath}\sounds",
        $@"{basePath}\sounds1",
    };

    DeleteExistingZipFiles();

    var filesPaths = GetFilePaths(sourceFilesDirectory);

    var zipFile = new ZipFileService(basePath).CompressFiles(filesPaths);

    Console.WriteLine($"Zipping Complete: {zipFile}");
}

ZipFiles();
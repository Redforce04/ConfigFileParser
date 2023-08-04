using System.IO.Compression;

namespace Publisher;

internal class Program
{
    public static string ExportPath = "";
    private static string[] _unnecessaryFiles = new string[]
    {
        ".pdb",
        ".deps.json",
        ".zip"
    };

    public static void Main(string[] args)
    {
        // See https://aka.ms/new-console-template for more information
        Console.WriteLine("Publishing Solution");
        string path = args.Length > 0 ? Path.GetFullPath(args[0]) : AppDomain.CurrentDomain.BaseDirectory;
        path = path.Replace("\\", "/");
        path += "ConfigFileParser/bin/Release/net7.0/";
        Console.WriteLine($"Base directory path: {path}");
        ExportPath = args.Length > 1 ? Path.GetFullPath(args[1]) : path + "export/";
        ExportPath = ExportPath.Replace("\\", "/");
        Console.WriteLine($"Export directory path: {ExportPath}");
        if (!Directory.Exists(ExportPath))
        {
            Directory.CreateDirectory(ExportPath);
        }

        foreach (var x in Directory.GetFiles(ExportPath))
        {
            try
            {
                File.Delete(x);
            }
            catch (Exception e)
            {
                // unused
            }
        }
        foreach (var directory in Directory.GetDirectories(path))
        {
            var split = directory.Split('/');
            string currentDirectory = split[^1];
            string[] subDirectories = Directory.GetDirectories(directory);
            if (subDirectories.Length > 0)
            {
                var publishDirectory = subDirectories.FirstOrDefault(x => x.EndsWith("publish"));
                if (publishDirectory is null)
                {
                    TrimUnusedFiles(directory);
                    ZipAndRename(directory);

                }
                else
                {
                    TrimUnusedFiles(publishDirectory);
                    ZipAndRename(publishDirectory);
                }
            }
            else
            {
                TrimUnusedFiles(directory);
                ZipAndRename(directory);
            }
        }
    }

    public static void ZipAndRename(string directory)
    {
        try
        {
            string subFileDirectory = directory.Replace("\\", "/").Split('/')[^1];
            if (subFileDirectory == "publish")
            {
                subFileDirectory = directory.Replace("\\", "/").Split('/')[^2];
            }
            string[] files = Directory.GetFiles(directory);
            if (files.Length > 1)
            {
                var zipFile = $"{ExportPath}/{subFileDirectory}.zip";
                ZipFile.CreateFromDirectory(directory, zipFile, CompressionLevel.NoCompression, false);
                return;
            }

            foreach (var File in files)
            {
                string fileName = File.Replace("\\", "/").Split('/')[^1];
                string fileExtension = fileName.Contains(".") ? "." + fileName.Split(".")[^1] : "";
                /*try
                {

                    System.IO.File.Move(File.Replace($"\\", "/"), $"{newFileName.Replace("\\", "/")}");
                }
                catch (DirectoryNotFoundException e)
                {
                    Console.WriteLine($"{e}");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Cannot replace. Caught Exception: {e}");
                }*/

                try
                {
                    int i = 0;
                    while (i < 3 && IsFileLocked(new FileInfo(File)))
                    {
                        i++;
                        Console.WriteLine($"File {fileName} is still being used, waiting 1 second.");
                        Thread.Sleep(1000);
                    }
                    System.IO.File.Copy(File,
                        $"{ExportPath}/{(fileExtension != "" ? fileName.Replace(fileExtension, "") : fileName) + $"-{subFileDirectory}" + $"{fileExtension}"}",
                        true);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Cannot copy to new file. Caught Exception: {e}");
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Exception: {e}");
        }
    }

public static void TrimUnusedFiles(string directory)
{
    var files = Directory.GetFiles(directory);
        foreach (string file in files)
        {
            try
            {

                var fileSplit = file.Replace("\\", "/").Split('/');
                string fileName = fileSplit[^1];
                if (_unnecessaryFiles.Any(x => fileName.Contains(x)))
                {
                    
                    File.Delete(file.Replace("\\", "/"));
                    // Console.WriteLine($"Deleting File: {file}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: {e}");
                // unused
            }
        }
    }
    public static bool IsFileLocked(FileInfo file)
    {
        try
        {
            using (FileStream stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None))
            {
                stream.Close();
            }
        }
        catch (IOException)
        {
            return true;
        }

        return false;
    }
}

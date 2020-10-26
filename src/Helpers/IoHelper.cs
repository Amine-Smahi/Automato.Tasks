using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Automato.Tasks.Helpers
{
    public static class IoHelper
    {
        public static string GetFileContent(string settingsFileLocation)
        {
            return File.ReadAllText(settingsFileLocation);
        }

        public static IEnumerable<string> ReadAllLines(string file)
        {
            return File.ReadAllLines(file);
        }

        public static string GetFileName(string path)
        {
            return Path.GetFileName(path);
        }

        public static bool FileExists(string filePath)
        {
            return File.Exists(filePath);
        }

        public static void RemoveFirstLineFromTextFile(string path)
        {
            var linksList = File.ReadAllLines(path).ToList();
            linksList.RemoveAt(0);
            File.WriteAllLines(path, linksList.ToArray());
        }

        public static string CreatePath(string url, string downloadDirectory)
        {
            return Path.Combine(downloadDirectory, GetFileName(url));
        }

        public static void WriteAllText(string fileLocation, string content)
        {
            File.WriteAllText(fileLocation, content);
        }

        public static void OpenOrCreateFile(string filePath)
        {
            File.CreateText(filePath);
        }

        public static bool DirectoryExists(string directoryPath)
        {
            return Directory.Exists(directoryPath);
        }

        public static void CreateDirectory(string directoryPath)
        {
            Directory.CreateDirectory(directoryPath);
        }

        public static string GetFullPath(string fileOrFolderPath)
        {
            return Path.GetFullPath(fileOrFolderPath);
        }
    }
}

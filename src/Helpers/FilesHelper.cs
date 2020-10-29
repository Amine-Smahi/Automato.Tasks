using System.Collections.Generic;
using System.IO;

namespace Automato.Tasks.Helpers
{
    public static class FilesHelper
    {
        public static string GetFileContent(string settingsFileLocation)
        {
            return File.ReadAllText(settingsFileLocation);
        }

        public static IEnumerable<string> ReadAllLines(string file)
        {
            return File.ReadAllLines(file);
        }

        public static bool FileExists(string filePath)
        {
            return File.Exists(filePath);
        }

        public static void WriteAllText(string fileLocation, string content)
        {
            File.WriteAllText(fileLocation, content);
        }

        public static void OpenOrCreateFile(string filePath)
        {
            File.CreateText(filePath);
        }
    }
}

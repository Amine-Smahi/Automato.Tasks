using System.IO;

namespace Automato.Tasks.Helpers
{
    public static class DirectoriesHelper
    {
        public static bool DirectoryExists(string directoryPath)
        {
            return Directory.Exists(directoryPath);
        }

        public static void CreateDirectory(string directoryPath)
        {
            Directory.CreateDirectory(directoryPath);
        }
    }
}
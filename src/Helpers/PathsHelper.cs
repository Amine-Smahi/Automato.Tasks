using System.IO;

namespace Automato.Tasks.Helpers
{
    public static class PathsHelper
    {
        public static string GetFileNameFromPath(string path)
        {
            return Path.GetFileName(path);
        }

        public static string CreatePath(string path, string directory)
        {
            return Path.Combine(directory, GetFileNameFromPath(path));
        }
    }
}
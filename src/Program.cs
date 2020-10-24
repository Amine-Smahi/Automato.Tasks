using PleaseDownload.Handlers;
using PleaseDownload.Helpers;

namespace PleaseDownload
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            TaskHandler.ExecuteTasks();
            SystemHelper.Finish(args);
        }
    }
}
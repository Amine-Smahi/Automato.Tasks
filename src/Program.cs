using PleaseDownload.Handlers;
using PleaseDownload.Helpers;

namespace PleaseDownload
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            TaskHandler.InitiateDownloads();
            SystemHelper.Finish(args);
        }
    }
}
using System.Linq;
using PleaseDownload.Configuration;
using PleaseDownload.Enums;
using PleaseDownload.Helpers;

namespace PleaseDownload.Handlers
{
    public static class TaskHandler
    {
        private static readonly Settings Settings = new Settings(true);

        public static void InitiateDownloads()
        {
            var files = IoHelper.ReadAllLines(Settings.DownloadList).ToList();
            if (files.Count <= 0) return;
            Messages.ShowMessage(Messages.Welcome(files.Count, Settings.DownloadList));
            foreach (var file in files) ProcessTask(TaskType.DownloadFile, file);
        }

        private static void ProcessTask(TaskType type, string url)
        {
            while (true)
            {
                NetworkHelper.WaitForDecentInternetConnection(Settings.MinimumInternetSpeed,
                    Settings.MinimumGoodPings,
                    Settings.MinimumGoodPings);
                if (type.ToString().Equals(TaskType.DownloadFile.ToString()))
                    if (DownloadFileHandler(url))
                        continue;

                break;
            }
        }

        private static bool DownloadFileHandler(string url)
        {
            var doesSucceed = IoHelper.StartDownload(url, Settings.DownloadLocation);
            if (doesSucceed)
            {
                Messages.ShowMessage(Messages.SuccessfulDownload(IoHelper.GetFileName(url)));
                IoHelper.RemoveLinkFromTheList(Settings.DownloadList);
            }
            else
            {
                Messages.ShowMessage(Messages.FailedDownload(IoHelper.GetFileName(url)));
                Messages.ShowMessage(Messages.StartAgain);
                return true;
            }

            return false;
        }
    }
}
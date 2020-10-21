using System.Collections.Generic;
using System.Linq;
using PleaseDownload.Configuration;
using PleaseDownload.Helpers;

namespace PleaseDownload
{
    internal static class Program
    {
        private static Settings _settings;

        private static void Main(string[] args)
        {
            _settings = new Settings(true);
            Initiate();
            Finish(args);
        }

        private static void Finish(IReadOnlyList<string> args)
        {
            Messages.ShowMessage(Messages.Finish);
            SystemHelper.MakeItSleepIfTrue(args);
        }

        private static void Initiate()
        {
            var files = IoHelper.ReadAllLines(_settings.DownloadList).ToList();
            if (files.Count <= 0) return;
            Messages.ShowMessage(Messages.Welcome(files.Count, _settings.DownloadList));
            foreach (var file in files) Process(file);
        }

        private static void Process(string url)
        {
            while (true)
            {
                NetworkHelper.WaitForDecentInternetConnection(_settings.MinimumInternetSpeed,
                    _settings.MinimumGoodPings,
                    _settings.MinimumGoodPings);
                var doesSucceed = IoHelper.StartDownload(url, _settings.DownloadLocation);
                if (doesSucceed)
                {
                    Messages.ShowMessage(Messages.SuccessfulDownload(IoHelper.GetFileName(url)));
                    IoHelper.RemoveLinkFromTheList(_settings.DownloadList);
                }
                else
                {
                    Messages.ShowMessage(Messages.FailedDownload(IoHelper.GetFileName(url)));
                    Messages.ShowMessage(Messages.StartAgain);
                    continue;
                }

                break;
            }
        }
    }
}
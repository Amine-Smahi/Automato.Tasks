using System;
using System.ComponentModel;
using System.Net;
using Automato.Tasks.Constants;

namespace Automato.Tasks.Helpers
{
    public static class NetworkHelper
    {
        private static readonly WebClient WebClient = new WebClient();

        public static bool DownloadFile(string url, string downloadDirectory)
        {
            try
            {
                var filePath = PathsHelper.CreatePath(url, downloadDirectory);
                WebClient.DownloadProgressChanged += HandleDownloadProgress;
                WebClient.DownloadFileCompleted += HandleDownloadComplete;
                var syncObject = new object();
                lock (syncObject)
                {
                    WebClient.DownloadFileAsync(new Uri(url), filePath, syncObject);
                    ThreadsHelper.MonitorWait(syncObject);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        private static double CheckInternetSpeed()
        {
            try
            {
                var timeBeforeDownloadingFile = DateTime.Now;
                var data = WebClient.DownloadData("http://google.com");
                var timeAfterDownloadingFile = DateTime.Now;
                return Math.Round(
                    data.Length / 1024.0 / (timeAfterDownloadingFile - timeBeforeDownloadingFile).TotalSeconds, 2);
            }
            catch
            {
                NotificationsHelper.DisplayMessage(Messages.NoInternet);
                return 0d;
            }
        }

        public static void WaitForDecentInternetConnection(double minimumInternetSpeed, int goodPings,
            int minimumGoodPings, int waitingTime)
        {
            while (true)
            {
                if (goodPings >= 0)
                {
                    double myConnectionSpeed;
                    do
                    {
                        myConnectionSpeed = CheckInternetSpeed();
                        if (!(myConnectionSpeed < minimumInternetSpeed)) continue;
                        goodPings = minimumGoodPings;
                        NotificationsHelper.DisplayDynamicMessage(Messages.WaitForBetterInternet(myConnectionSpeed));
                        ThreadsHelper.Sleep(waitingTime);
                    } while (myConnectionSpeed < minimumInternetSpeed);

                    goodPings--;
                    continue;
                }

                break;
            }
        }

        private static void HandleDownloadComplete(object sender, AsyncCompletedEventArgs eventArgs)
        {
            ThreadsHelper.PulseMonitor(eventArgs);
        }

        private static void HandleDownloadProgress(object sender, DownloadProgressChangedEventArgs eventArgs)
        {
            var percentage = Math.Round(eventArgs.BytesReceived / (float) eventArgs.TotalBytesToReceive * 100);
            NotificationsHelper.DisplayDynamicMessage(Messages.DownloadProgress(percentage));
        }
    }
}
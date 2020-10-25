using System;
using System.ComponentModel;
using System.Net;
using System.Threading;
using Automato.ValueObjects;

namespace Automato.Helpers
{
    public static class NetworkHelper
    {
        private static readonly WebClient WebClient = new WebClient();

        public static void DownloadFile(string url, string downloadFolder)
        {
            var filePath = IoHelper.CreatePath(url, downloadFolder);
            WebClient.DownloadProgressChanged += HandleDownloadProgress;
            WebClient.DownloadFileCompleted += HandleDownloadComplete;
            var syncObject = new object();
            lock (syncObject)
            {
                WebClient.DownloadFileAsync(new Uri(url), filePath, syncObject);
                Monitor.Wait(syncObject);
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
            catch (Exception)
            {
                MessagesHelper.DisplayMessage(Messages.NoInternet);
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
                        MessagesHelper.DisplayDynamicMessage(Messages.WaitForBetterInternet(myConnectionSpeed));
                        Thread.Sleep(waitingTime);
                    } while (myConnectionSpeed < minimumInternetSpeed);

                    goodPings--;
                    continue;
                }

                break;
            }
        }

        private static void HandleDownloadComplete(object sender, AsyncCompletedEventArgs e)
        {
            lock (e.UserState)
            {
                Monitor.Pulse(e.UserState);
            }
        }

        private static void HandleDownloadProgress(object sender, DownloadProgressChangedEventArgs args)
        {
            var percentage = Math.Round(args.BytesReceived / (float) args.TotalBytesToReceive * 100);
            MessagesHelper.DisplayDynamicMessage(Messages.DownloadProgress(percentage));
        }
    }
}

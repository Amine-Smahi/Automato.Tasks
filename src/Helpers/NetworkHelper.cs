using System;
using System.Net;
using System.Threading;
using Automato.Constants;

namespace Automato.Helpers
{
    public static class NetworkHelper
    {
        private static readonly WebClient WebClient = new WebClient();

        public static void DownloadFile(string url, string downloadFolder)
        {
            var filePath = IoHelper.CreatePath(url, downloadFolder);
            WebClient.DownloadFile(url, filePath);
        }

        private static double CheckInternetSpeed()
        {
            try
            {
                var timeBeforeDownloadingFile = DateTime.Now;
                var data = WebClient.DownloadData("http://google.com");
                var timeAfterDownloadingFile = DateTime.Now;
                return Math.Round(data.Length / 1024.0 / (timeAfterDownloadingFile - timeBeforeDownloadingFile).TotalSeconds, 2);
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
                        MessagesHelper.DisplayMessage(Messages.WaitForBetterInternet(myConnectionSpeed));
                        Thread.Sleep(waitingTime);
                    } while (myConnectionSpeed < minimumInternetSpeed);

                    goodPings--;
                    continue;
                }

                break;
            }
        }
    }
}
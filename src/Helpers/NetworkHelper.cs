using System;
using System.Net;
using System.Threading;

namespace PleaseDownload.Helpers
{
    public static class NetworkHelper
    {
        private static readonly WebClient Wc = new WebClient();

        public static void DownloadFile(string url, string downloadFolder)
        {
            var filePath = IoHelper.CreatePath(url, downloadFolder);
            Wc.DownloadFile(url, filePath);
        }

        private static double CheckInternetSpeed()
        {
            try
            {
                var dt1 = DateTime.Now;
                var data = Wc.DownloadData("http://google.com");
                var dt2 = DateTime.Now;
                return Math.Round(data.Length / 1024.0 / (dt2 - dt1).TotalSeconds, 2);
            }
            catch (Exception)
            {
                Messages.ShowMessage(Messages.NoInternet);
                return 0d;
            }
        }

        public static void WaitForDecentInternetConnection(double minimumInternetSpeed, int goodPings,
            int minimumGoodPings)
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
                        Messages.ShowMessage(Messages.WaitForBetterInternet(myConnectionSpeed));
                        Thread.Sleep(1000);
                    } while (myConnectionSpeed < minimumInternetSpeed);

                    goodPings--;
                    continue;
                }

                break;
            }
        }
    }
}
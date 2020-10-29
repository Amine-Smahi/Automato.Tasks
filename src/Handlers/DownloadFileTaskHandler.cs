using Automato.Tasks.Constants;
using Automato.Tasks.Helpers;
using Automato.Tasks.Interfaces;

namespace Automato.Tasks.Handlers
{
    public class DownloadFileTaskHandler : IDownloadFileTaskHandler
    {
        public bool DownloadFileAndReturnStatus(string url, string downloadLocation)
        {
            var doesSucceed = NetworkHelper.DownloadFile(url, downloadLocation);
            if (doesSucceed)
            {
                NotificationsHelper.DisplayMessage(Messages.SuccessfulDownload(PathsHelper.GetFileNameFromPath(url)));
            }
            else
            {
                NotificationsHelper.DisplayMessage(Messages.FailedDownload(PathsHelper.GetFileNameFromPath(url)));
                NotificationsHelper.DisplayMessage(Messages.StartAgain);
                return true;
            }

            return false;
        }
    }
}

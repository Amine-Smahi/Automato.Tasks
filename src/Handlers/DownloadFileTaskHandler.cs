using Automato.Tasks.Constants;
using Automato.Tasks.Helpers;

namespace Automato.Tasks.Handlers
{
    public class DownloadFileTaskHandler
    {
        private readonly TasksHandler _tasksHandler;

        public DownloadFileTaskHandler(TasksHandler tasksHandler)
        {
            _tasksHandler = tasksHandler;
        }

        public bool DownloadFile(string url)
        {
            var doesSucceed = NetworkHelper.DownloadFile(url, _tasksHandler.Settings.DownloadLocation);
            if (doesSucceed)
            {
                NotificationsHelper.DisplayMessage(Messages.SuccessfulDownload(PathsHelper.GetFileNameFromPath(url)));
                FilesHelper.RemoveFirstLineFromTextFile(_tasksHandler.Settings.TasksLocation);
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

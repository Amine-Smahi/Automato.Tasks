using System.Linq;
using PleaseDownload.Configuration;
using PleaseDownload.Enums;
using PleaseDownload.Helpers;

namespace PleaseDownload.Handlers
{
    public static class TaskHandler
    {
        private static readonly Settings Settings = new Settings(true);

        public static void ExecuteTasks()
        {
            var tasks = IoHelper.ReadAllLines(Settings.TasksLocation).ToList();
            if (tasks.Count <= 0) return;
            Messages.ShowMessage(Messages.Welcome(tasks.Count, Settings.TasksLocation));
            foreach (var task in tasks) ProcessTask(task);
        }

        private static void ProcessTask(string task)
        {
            while (true)
            {
                NetworkHelper.WaitForDecentInternetConnection(Settings.MinimumInternetSpeed,
                    Settings.MinimumGoodPings,
                    Settings.MinimumGoodPings);

                if (task.Contains(TaskType.Download.ToString()))
                {
                    Messages.ShowMessage(Messages.StartsDownloading);
                    if (DownloadFileHandler(RemoveTaskType(task, TaskType.Download)))
                        continue;
                }
                else
                {
                    Messages.ShowMessage(Messages.ExecutingTask);
                    SystemHelper.ExecuteCommand(RemoveTaskType(task, TaskType.Cmd));
                    IoHelper.RemoveTaskFromTheList(Settings.TasksLocation);
                }

                break;
            }
        }

        private static string RemoveTaskType(string task, TaskType type)
        {
            return task.Replace(type + " ", string.Empty);
        }

        private static bool DownloadFileHandler(string url)
        {
            var doesSucceed = IoHelper.StartDownload(url, Settings.DownloadLocation);
            if (doesSucceed)
            {
                Messages.ShowMessage(Messages.SuccessfulDownload(IoHelper.GetFileName(url)));
                IoHelper.RemoveTaskFromTheList(Settings.TasksLocation);
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
using System.Linq;
using Automato.Configuration;
using Automato.Enums;
using Automato.Helpers;

namespace Automato.Handlers
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

                if (GetValueFromTask(task, 0).Contains(TaskType.Download.ToString()))
                {
                    Messages.ShowMessage(Messages.StartsDownloading);
                    if (DownloadFileHandler(GetValueFromTask(task, 1)))
                        continue;
                }
                else
                {
                    Messages.ShowMessage(Messages.ExecutingTask);
                    SystemHelper.ExecuteCommand(GetValueFromTask(task, 1));
                    IoHelper.RemoveTaskFromTheList(Settings.TasksLocation);
                }

                break;
            }
        }

        private static string GetValueFromTask(string task, int index)
        {
            var attributes = task.Split(Settings.TaskTypeSplitter);
            return attributes[index];
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
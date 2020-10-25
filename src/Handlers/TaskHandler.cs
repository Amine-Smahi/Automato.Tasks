using System.Linq;
using Automato.Entities;
using Automato.Enums;
using Automato.Helpers;
using Automato.ValueObjects;

namespace Automato.Handlers
{
    public static class TaskHandler
    {
        private static readonly Settings Settings = new Settings(true);

        public static void ExecuteTasks()
        {
            var tasks = IoHelper.ReadAllLines(Settings.TasksLocation).ToList();
            if (tasks.Count <= 0) return;
            MessagesHelper.DisplayMessage(Messages.Welcome(tasks.Count, Settings.TasksLocation));
            foreach (var task in tasks) ProcessTask(task);
        }

        private static void ProcessTask(string task)
        {
            while (true)
            {
                NetworkHelper.WaitForDecentInternetConnection(Settings.MinimumInternetSpeed,
                    Settings.MinimumGoodPings,
                    Settings.MinimumGoodPings, Settings.WaitFewSecondsForAnotherTry);

                if (GetValueFromTask(task, 0).ToLower().Contains(TaskType.Download.ToString().ToLower()))
                {
                    MessagesHelper.DisplayMessage(Messages.StartsDownloading);
                    if (DownloadFileHandler(GetValueFromTask(task, 1)))
                        continue;
                }
                else if (GetValueFromTask(task, 0).ToLower().Contains(TaskType.Cmd.ToString().ToLower()))
                {
                    MessagesHelper.DisplayMessage(Messages.ExecutingTask);
                    SystemHelper.ExecuteCommand(GetValueFromTask(task, 1));
                    IoHelper.RemoveTaskFromTheList(Settings.TasksLocation);
                }
                else
                {
                    MessagesHelper.DisplayMessage(Messages.TaskNotRecognized(GetValueFromTask(task, 0)));
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
                MessagesHelper.DisplayMessage(Messages.SuccessfulDownload(IoHelper.GetFileName(url)));
                IoHelper.RemoveTaskFromTheList(Settings.TasksLocation);
            }
            else
            {
                MessagesHelper.DisplayMessage(Messages.FailedDownload(IoHelper.GetFileName(url)));
                MessagesHelper.DisplayMessage(Messages.StartAgain);
                return true;
            }

            return false;
        }
    }
}

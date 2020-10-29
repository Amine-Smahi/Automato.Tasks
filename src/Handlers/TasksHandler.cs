using System.Linq;
using Automato.Tasks.Constants;
using Automato.Tasks.Enums;
using Automato.Tasks.Helpers;
using Automato.Tasks.Models;

namespace Automato.Tasks.Handlers
{
    public static class TasksHandler
    {
        private static readonly Settings Settings = new Settings {LoadingSettings = true};

        public static void ExecuteTasks()
        {
            if (CommandsHelper.ShouldOpenSettings()) SystemsHelper.OpenFile(Settings.SettingsFileLocation);
            if (CommandsHelper.ShouldOpenTasks()) SystemsHelper.OpenFile(Settings.TasksLocation);
            if (CommandsHelper.ShouldOpenDownloadsDirectory()) SystemsHelper.OpenDirectory(Settings.DownloadLocation);
            if (CommandsHelper.ShouldExecuteTasks())
            {
                var tasks = FilesHelper.ReadAllLines(Settings.TasksLocation).ToList();
                if (tasks.Count <= 0) return;
                NotificationsHelper.DisplayMessage(Messages.Welcome(tasks.Count, Settings.TasksLocation));
                foreach (var task in tasks) ProcessTask(task);
            }
            else
            {
                NotificationsHelper.DisplayMessage(Messages.CommandNotRecognized);
            }
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
                    NotificationsHelper.DisplayMessage(Messages.StartsDownloading);
                    if (DownloadFileHandler(GetValueFromTask(task, 1)))
                        continue;
                }
                else if (GetValueFromTask(task, 0).ToLower().Contains(TaskType.Cmd.ToString().ToLower()))
                {
                    NotificationsHelper.DisplayMessage(Messages.ExecutingTask);
                    SystemsHelper.ExecuteCommand(GetValueFromTask(task, 1));
                    FilesHelper.RemoveFirstLineFromTextFile(Settings.TasksLocation);
                }
                else
                {
                    NotificationsHelper.DisplayMessage(Messages.TaskNotRecognized(GetValueFromTask(task, 0)));
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
            var doesSucceed = NetworkHelper.DownloadFile(url, Settings.DownloadLocation);
            if (doesSucceed)
            {
                NotificationsHelper.DisplayMessage(Messages.SuccessfulDownload(PathsHelper.GetFileNameFromPath(url)));
                FilesHelper.RemoveFirstLineFromTextFile(Settings.TasksLocation);
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

using System.Linq;
using Automato.Tasks.Enums;
using Automato.Tasks.Helpers;
using Automato.Tasks.Models;
using Automato.Tasks.ValueObjects;

namespace Automato.Tasks.Handlers
{
    public static class TaskHandler
    {
        private static readonly Settings Settings = new Settings {LoadingSettings = true};

        public static void ExecuteTasks()
        {
            if (CommandsHelper.ShouldOpenSettings())
            {
                SystemHelper.OpenFile(Settings.SettingsFileLocation);
            }
            else if (CommandsHelper.ShouldOpenTasks())
            {
                SystemHelper.OpenFile(Settings.TasksLocation);
            }
            else if (CommandsHelper.ShouldOpenDownloadsDirectory())
            {
                SystemHelper.OpenDirectory(Settings.DownloadLocation);
            }
            else if (CommandsHelper.ShouldExecuteTasks())
            {
                var tasks = IoHelper.ReadAllLines(Settings.TasksLocation).ToList();
                if (tasks.Count <= 0) return;
                MessagesHelper.DisplayMessage(Messages.Welcome(tasks.Count, Settings.TasksLocation));
                foreach (var task in tasks) ProcessTask(task);
            }
            else
            {
                MessagesHelper.DisplayMessage(Messages.CommandNotRecognized);
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
                    MessagesHelper.DisplayMessage(Messages.StartsDownloading);
                    if (DownloadFileHandler(GetValueFromTask(task, 1)))
                        continue;
                }
                else if (GetValueFromTask(task, 0).ToLower().Contains(TaskType.Cmd.ToString().ToLower()))
                {
                    MessagesHelper.DisplayMessage(Messages.ExecutingTask);
                    SystemHelper.ExecuteCommand(GetValueFromTask(task, 1));
                    IoHelper.RemoveFirstLineFromTextFile(Settings.TasksLocation);
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
            var doesSucceed = NetworkHelper.DownloadFile(url, Settings.DownloadLocation);
            if (doesSucceed)
            {
                MessagesHelper.DisplayMessage(Messages.SuccessfulDownload(IoHelper.GetFileName(url)));
                IoHelper.RemoveFirstLineFromTextFile(Settings.TasksLocation);
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

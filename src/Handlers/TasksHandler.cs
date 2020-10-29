using System.Linq;
using Automato.Tasks.Constants;
using Automato.Tasks.Enums;
using Automato.Tasks.Helpers;
using Automato.Tasks.Interfaces;
using Automato.Tasks.Models;

namespace Automato.Tasks.Handlers
{
    public class TasksHandler : ITasksHandler
    {
        private readonly Settings _settings = new Settings {LoadingSettings = true};

        public void ExecuteTasks()
        {
            if (CommandsHelper.ShouldOpenSettings()) SystemsHelper.OpenFile(_settings.SettingsFileLocation);
            if (CommandsHelper.ShouldOpenTasks()) SystemsHelper.OpenFile(_settings.TasksLocation);
            if (CommandsHelper.ShouldOpenDownloadsDirectory()) SystemsHelper.OpenDirectory(_settings.DownloadLocation);
            if (CommandsHelper.ShouldExecuteTasks())
            {
                var tasks = FilesHelper.ReadAllLines(_settings.TasksLocation).ToList();
                if (tasks.Count <= 0) return;
                NotificationsHelper.DisplayMessage(Messages.Welcome(tasks.Count, _settings.TasksLocation));
                foreach (var task in tasks) ProcessTask(task);
            }
            else
            {
                NotificationsHelper.DisplayMessage(Messages.CommandNotRecognized);
            }
        }

        private void ProcessTask(string task)
        {
            while (true)
            {
                NetworkHelper.WaitForDecentInternetConnection(_settings.MinimumInternetSpeed,
                    _settings.MinimumGoodPings,
                    _settings.MinimumGoodPings, _settings.WaitFewSecondsForAnotherTry);

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
                    FilesHelper.RemoveFirstLineFromTextFile(_settings.TasksLocation);
                }
                else
                {
                    NotificationsHelper.DisplayMessage(Messages.TaskNotRecognized(GetValueFromTask(task, 0)));
                }

                break;
            }
        }

        private string GetValueFromTask(string task, int index)
        {
            var attributes = task.Split(_settings.TaskTypeSplitter);
            return attributes[index];
        }

        private bool DownloadFileHandler(string url)
        {
            var doesSucceed = NetworkHelper.DownloadFile(url, _settings.DownloadLocation);
            if (doesSucceed)
            {
                NotificationsHelper.DisplayMessage(Messages.SuccessfulDownload(PathsHelper.GetFileNameFromPath(url)));
                FilesHelper.RemoveFirstLineFromTextFile(_settings.TasksLocation);
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

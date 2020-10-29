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
                foreach (var taskString in tasks)
                {
                    var task = new Task();
                    task.ParseTask(taskString, _settings.TaskTypeSplitter);
                    ProcessTask(task);
                }
            }
            else
            {
                NotificationsHelper.DisplayMessage(Messages.CommandNotRecognized);
            }
        }

        private void ProcessTask(Task task)
        {
            while (!task.IsDone && !task.IsExecuted)
            {
                NetworkHelper.WaitForDecentInternetConnection(_settings.MinimumInternetSpeed,
                    _settings.MinimumGoodPings,
                    _settings.MinimumGoodPings, _settings.WaitFewSecondsForAnotherTry);

                switch (task.TaskType)
                {
                    case TaskType.Download:
                    {
                        NotificationsHelper.DisplayMessage(Messages.StartsDownloading);
                        if (DownloadFileHandler(task.Value)) task.IsDone = true;
                        break;
                    }
                    case TaskType.Cmd:
                        NotificationsHelper.DisplayMessage(Messages.ExecutingTask);
                        SystemsHelper.ExecuteCommand(task.Value);
                        task.IsDone = true;
                        break;
                    case TaskType.NotSupported:
                        NotificationsHelper.DisplayMessage(Messages.TaskNotRecognized(task.Value));
                        task.IsExecuted = true;
                        break;
                    default:
                        NotificationsHelper.DisplayMessage(Messages.NoTaskIdentified(task.Value));
                        task.IsExecuted = true;
                        break;
                }
            }
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

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
        private readonly DownloadFileTaskHandler _downloadFileTaskHandler;
        public readonly Settings Settings = new Settings {LoadingSettings = true};

        public TasksHandler()
        {
            _downloadFileTaskHandler = new DownloadFileTaskHandler(this);
        }

        public void ExecuteTasks()
        {
            if (CommandsHelper.ShouldOpenSettings()) SystemsHelper.OpenFile(Settings.SettingsFileLocation);
            if (CommandsHelper.ShouldOpenTasks()) SystemsHelper.OpenFile(Settings.TasksLocation);
            if (CommandsHelper.ShouldOpenDownloadsDirectory()) SystemsHelper.OpenDirectory(Settings.DownloadLocation);
            if (CommandsHelper.ShouldExecuteTasks())
            {
                var tasks = FilesHelper.ReadAllLines(Settings.TasksLocation).ToList();
                if (tasks.Count <= 0) return;
                NotificationsHelper.DisplayMessage(Messages.Welcome(tasks.Count, Settings.TasksLocation));
                foreach (var taskString in tasks)
                {
                    var task = new Task();
                    task.ParseTask(taskString, Settings.TaskTypeSplitter);
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
                NetworkHelper.WaitForDecentInternetConnection(Settings.MinimumInternetSpeed,
                    Settings.MinimumGoodPings,
                    Settings.MinimumGoodPings, Settings.WaitFewSecondsForAnotherTry);

                switch (task.TaskType)
                {
                    case TaskType.Download:
                    {
                        NotificationsHelper.DisplayMessage(Messages.StartsDownloading);
                        if (_downloadFileTaskHandler.DownloadFile(task.Value)) task.IsDone = true;
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
    }
}

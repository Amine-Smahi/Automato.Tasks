using System.Collections.Generic;
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
        private readonly IDownloadFileTaskHandler _downloadFileTaskHandler;
        private readonly Settings _settings = new Settings {LoadingSettings = true};

        public TasksHandler()
        {
            _downloadFileTaskHandler = new DownloadFileTaskHandler();
            _downloadFileTaskHandler = DependencyInjectionHelper.InjectDependency<IDownloadFileTaskHandler>();
        }

        public void ExecuteTasks()
        {
            var executedTasks = new List<Task>();
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
                    executedTasks.Add(task);
                }

                UpdateExecutedTasksStatusInUserTasksList(executedTasks);
            }
            else
            {
                NotificationsHelper.DisplayMessage(Messages.CommandNotRecognized);
            }
        }

        public void UpdateExecutedTasksStatusInUserTasksList(IEnumerable<Task> tasks)
        {
            FilesHelper.RemoveFileContent(_settings.TasksLocation);
            foreach (var task in tasks)
                FilesHelper.AddLineToFile(_settings.TasksLocation, task.Stringify(_settings.TaskTypeSplitter));
        }

        private void ProcessTask(Task task)
        {
            while (task.TaskStatus == TaskStatus.NotDone)
            {
                NetworkHelper.WaitForDecentInternetConnection(_settings.MinimumInternetSpeed,
                    _settings.MinimumGoodPings,
                    _settings.MinimumGoodPings, _settings.WaitFewSecondsForAnotherTry);
                switch (task.TaskType)
                {
                    case TaskType.Download:
                    {
                        NotificationsHelper.DisplayMessage(Messages.StartsDownloading);
                        if (_downloadFileTaskHandler.DownloadFileAndReturnStatus(task.Value, _settings.DownloadLocation)
                        ) task.TaskStatus = TaskStatus.Done;
                        break;
                    }
                    case TaskType.Cmd:
                        NotificationsHelper.DisplayMessage(Messages.ExecutingTask);
                        SystemsHelper.ExecuteCommand(task.Value);
                        task.TaskStatus = TaskStatus.Done;
                        break;
                    case TaskType.NotSupported:
                        NotificationsHelper.DisplayMessage(Messages.TaskNotRecognized(task.Value));
                        task.TaskStatus = TaskStatus.HasErrors;
                        break;
                    default:
                        NotificationsHelper.DisplayMessage(Messages.NoTaskIdentified(task.Value));
                        task.TaskStatus = TaskStatus.HasErrors;
                        break;
                }
            }
        }
    }
}

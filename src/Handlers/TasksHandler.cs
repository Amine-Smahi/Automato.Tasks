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
            if (CommandsHelper.ShouldOpenSettings()) SystemsHelper.OpenFile(_settings.SettingsFileLocation);
            if (CommandsHelper.ShouldOpenTasks()) SystemsHelper.OpenFile(_settings.TasksLocation);
            if (CommandsHelper.ShouldOpenDownloadsDirectory()) SystemsHelper.OpenDirectory(_settings.DownloadLocation);
            if (CommandsHelper.ShouldExecuteTasks()) ExtractTasks();
            else NotificationsHelper.DisplayMessage(Messages.CommandNotRecognized);
        }

        private void ExtractTasks()
        {
            var tasks = FilesHelper.ReadAllLines(_settings.TasksLocation).ToList();
            if (tasks.Count <= 0) return;
            NotificationsHelper.DisplayMessage(Messages.Welcome(tasks.Count, _settings.TasksLocation));
            foreach (var taskString in tasks)
            {
                var task = new Task();
                task.Parse(taskString, _settings.TaskTypeSplitter);
                ProcessTask(task, tasks.IndexOf(taskString));
            }
        }

        private void ProcessTask(Task task, int taskIndex)
        {
            while (task.TaskStatus == TaskStatus.NotDone)
            {
                NetworkHelper.WaitForDecentInternetConnection(_settings.MinimumInternetSpeed,
                    _settings.MinimumGoodPings,
                    _settings.MinimumGoodPings, _settings.WaitFewSecondsForAnotherTry);
                switch (task.TaskType)
                {
                    case TaskType.Download:
                        ExecuteDownloadTask(task);
                        break;
                    case TaskType.Cmd:
                        ExecuteCmdTask(task);
                        break;
                    case TaskType.NotSupported:
                        TaskNotExecuted(task, Messages.TaskNotRecognized(task.Value));
                        break;
                    default:
                        TaskNotExecuted(task, Messages.NoTaskIdentified(task.Value));
                        break;
                }
            }

            UpdateTaskStatusInUserTasksList(task, taskIndex);
        }

        private static void ExecuteCmdTask(Task task)
        {
            NotificationsHelper.DisplayMessage(Messages.ExecutingTask);
            SystemsHelper.ExecuteCommand(task.Value);
            task.TaskStatus = TaskStatus.Done;
        }

        private void ExecuteDownloadTask(Task task)
        {
            NotificationsHelper.DisplayMessage(Messages.StartsDownloading);
            if (_downloadFileTaskHandler.DownloadFileAndReturnStatus(task.Value, _settings.DownloadLocation))
                task.TaskStatus = TaskStatus.Done;
        }

        private void UpdateTaskStatusInUserTasksList(Task task, int taskIndex)
        {
            FilesHelper.UpdateLineInFile(_settings.TasksLocation, task.Stringify(_settings.TaskTypeSplitter),
                taskIndex);
        }

        private static void TaskNotExecuted(Task task, string messageType)
        {
            NotificationsHelper.DisplayMessage(messageType);
            task.TaskStatus = TaskStatus.HasErrors;
        }
    }
}

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
        private readonly UserSettings _userSettings = new UserSettings {LoadingSettings = true};

        public TasksHandler()
        {
            _downloadFileTaskHandler = new DownloadFileTaskHandler();
            _downloadFileTaskHandler = DependencyInjectionHelper.InjectDependency<IDownloadFileTaskHandler>();
        }

        public void ExecuteTasks()
        {
            if (CommandsHelper.ShouldOpenSettings()) SystemsHelper.OpenFile(_userSettings.SettingsFileLocation);
            if (CommandsHelper.ShouldOpenTasks()) SystemsHelper.OpenFile(_userSettings.TasksLocation);
            if (CommandsHelper.ShouldOpenDownloadsDirectory())
                SystemsHelper.OpenDirectory(_userSettings.DownloadLocation);
            if (CommandsHelper.ShouldExecuteTasks()) ExtractTasks();
            else NotificationsHelper.DisplayMessage(Messages.CommandNotRecognized);
        }

        private void ExtractTasks()
        {
            var tasks = FilesHelper.ReadAllLines(_userSettings.TasksLocation).ToList();
            if (tasks.Count <= 0) return;
            NotificationsHelper.DisplayMessage(Messages.Welcome(tasks.Count, _userSettings.TasksLocation));
            foreach (var taskString in tasks)
            {
                var task = new Task();
                task.Parse(taskString, _userSettings.TaskTypeSplitter);
                ProcessTask(task, tasks.IndexOf(taskString));
            }
        }

        private void ProcessTask(Task task, int taskIndex)
        {
            while (task.TaskStatus == TaskStatus.NotDone)
            {
                NetworkHelper.WaitForDecentInternetConnection(_userSettings.MinimumInternetSpeed,
                    _userSettings.MinimumGoodPings,
                    _userSettings.MinimumGoodPings, _userSettings.WaitFewSecondsForAnotherTry);
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
            if (_downloadFileTaskHandler.DownloadFileAndReturnStatus(task.Value, _userSettings.DownloadLocation))
                task.TaskStatus = TaskStatus.Done;
        }

        private void UpdateTaskStatusInUserTasksList(Task task, int taskIndex)
        {
            FilesHelper.UpdateLineInFile(_userSettings.TasksLocation, task.Stringify(_userSettings.TaskTypeSplitter),
                taskIndex);
        }

        private static void TaskNotExecuted(Task task, string messageType)
        {
            NotificationsHelper.DisplayMessage(messageType);
            task.TaskStatus = TaskStatus.HasErrors;
        }
    }
}

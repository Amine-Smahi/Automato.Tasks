using Automato.Tasks.Enums;

namespace Automato.Tasks.Models
{
    public class Task
    {
        public Task()
        {
            IsDone = false;
        }

        public TaskType TaskType { get; private set; }
        public string Value { get; private set; }
        public bool IsDone { get; set; }
        public bool IsExecuted { get; set; }

        public void ParseTask(string task, string taskTypeSplitter)
        {
            TaskType = GetTaskType(task, taskTypeSplitter);
            Value = GeTaskValue(task, taskTypeSplitter);
        }

        private static TaskType GetTaskType(string task, string taskTypeSplitter)
        {
            var type = task.Split(taskTypeSplitter)[0];
            if (type == TaskType.Download.ToString()) return TaskType.Download;
            if (type == TaskType.Cmd.ToString()) return TaskType.Cmd;
            return TaskType.NotSupported;
        }

        private static string GeTaskValue(string task, string taskTypeSplitter)
        {
            var attributes = task.Split(taskTypeSplitter);
            return attributes[1];
        }
    }
}

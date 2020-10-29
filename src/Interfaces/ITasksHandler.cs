using System.Collections.Generic;
using Automato.Tasks.Models;

namespace Automato.Tasks.Interfaces
{
    public interface ITasksHandler
    {
        void ExecuteTasks();
        void UpdateTasksStatusInUserTasksList(IEnumerable<Task> tasks);
    }
}

using Automato.Tasks.Handlers;
using Automato.Tasks.Helpers;

namespace Automato.Tasks
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            TaskHandler.ExecuteTasks();
            SystemHelper.Finish(args);
        }
    }
}
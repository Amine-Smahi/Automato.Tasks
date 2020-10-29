using Automato.Tasks.Handlers;
using Automato.Tasks.Helpers;

namespace Automato.Tasks
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            CommandsHelper.AnalyseCommandArgs(args);
            TaskHandler.ExecuteTasks();
            SystemsHelper.Finish(args);
        }
    }
}

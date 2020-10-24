using Automato.Handlers;
using Automato.Helpers;

namespace Automato
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
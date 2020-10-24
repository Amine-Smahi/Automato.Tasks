using Autotomato.Handlers;
using Autotomato.Helpers;

namespace Autotomato
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
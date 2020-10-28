using System.Collections.Generic;
using System.Linq;

namespace Automato.Tasks.Helpers
{
    public static class CommandsHelper
    {
        private static string[] CommandArgs { get; set; }

        public static void AnalyseCommandArgs(IEnumerable<string> args)
        {
            CommandArgs = args.Where(x => !string.IsNullOrWhiteSpace(x) || !string.IsNullOrEmpty(x)).ToArray();
        }

        public static bool ShouldSleep()
        {
            return CommandArgs.Any(x => x == true.ToString());
        }

        public static bool ShouldExecuteTasks()
        {
            return CommandArgs.Length == 0 || ShouldSleep() && CommandArgs.Length == 1;
        }

        public static bool ShouldOpenSettings()
        {
            return CommandArgs.Any(x => x == "settings");
        }

        public static bool ShouldOpenTasks()
        {
            return CommandArgs.Any(x => x == "tasks");
        }

        public static bool ShouldOpenDownloadsDirectory()
        {
            return CommandArgs.Any(x => x == "downloads");
        }
    }
}
using Automato.Tasks.Constants;
using Automato.Tasks.Handlers;
using Automato.Tasks.Helpers;
using Automato.Tasks.Interfaces;

namespace Automato.Tasks
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            RegisterServices();
            var tasksHandler = DependencyInjectionHelper.InjectDependency<ITasksHandler>();
            CommandsHelper.AnalyseCommandArgs(args);
            tasksHandler.ExecuteTasks();
            NotificationsHelper.DisplayMessage(Messages.Finish);
            SystemsHelper.Sleep();
        }

        private static void RegisterServices()
        {
            DependencyInjectionHelper.RegisterDependency<IUserSettingsHandler, UserSettingsHandler>();
            DependencyInjectionHelper.RegisterDependency<ITasksHandler, TasksHandler>();
            DependencyInjectionHelper.RegisterDependency<IDownloadFileTaskHandler, DownloadFileTaskHandler>();
        }
    }
}

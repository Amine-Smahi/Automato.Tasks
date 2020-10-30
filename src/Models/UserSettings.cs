using Automato.Tasks.Helpers;
using Automato.Tasks.Interfaces;

namespace Automato.Tasks.Models
{
    public class UserSettings
    {
        private readonly IUserSettingsHandler _userSettingsHandler;

        public UserSettings()
        {
            _userSettingsHandler = DependencyInjectionHelper.InjectDependency<IUserSettingsHandler>();
        }

        public bool LoadingSettings
        {
            set => _userSettingsHandler.LoadSettings(this, value);
        }

        public string SettingsFileLocation { get; } = "./settings.json";
        public string DownloadLocation { get; set; } = "./downloads";
        public int MinimumInternetSpeed { get; set; } = 30;
        public int MinimumGoodPings { get; set; } = 5;
        public string TasksLocation { get; set; } = "./MyTasks.txt";
        public string TaskTypeSplitter { get; set; } = "=>";
        public int WaitFewSecondsForAnotherTry { get; set; } = 2000;
    }
}

using Automato.Tasks.Handlers;

namespace Automato.Tasks.Models
{
    public class UserSettings
    {
        private readonly UserSettingsHandler _userSettingsHandler;

        public UserSettings()
        {
            _userSettingsHandler = new UserSettingsHandler(this);
        }

        public bool LoadingSettings
        {
            set => _userSettingsHandler.LoadSettings(value);
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

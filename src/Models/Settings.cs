using Automato.Behaviors;

namespace Automato.Models
{
    public class Settings
    {
        private readonly SettingsBehavior _settingsBehavior;

        public Settings()
        {
            _settingsBehavior = new SettingsBehavior(this);
        }

        public bool LoadingSettings
        {
            set => _settingsBehavior.LoadSettings(value);
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

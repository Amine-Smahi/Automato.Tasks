using Automato.Helpers;

namespace Automato.Entities
{
    public class Settings
    {
        public Settings()
        {
        }

        public Settings(bool shouldLoad)
        {
            var settingsHelper = new SettingsHelper(this);
            if (shouldLoad) settingsHelper.LoadSettings();
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
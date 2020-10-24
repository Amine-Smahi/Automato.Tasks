using System;
using PleaseDownload.Helpers;

namespace PleaseDownload.Configuration
{
    public class Settings
    {
        private Settings()
        {
        }

        public Settings(bool shouldLoad)
        {
            if (shouldLoad) LoadSettings();
        }

        public string SettingsFileLocation { get; } = "./settings.json";
        public string DownloadLocation { get; private set; } = "./downloads";
        public int MinimumInternetSpeed { get; private set; } = 5;
        public int MinimumGoodPings { get; private set; } = 5;
        public string TasksLocation { get; private set; } = "./MyTasks.txt";

        private void LoadSettings()
        {
            try
            {
                var localSettings = IoHelper.Deserialize(SettingsFileLocation);
                DownloadLocation = localSettings.DownloadLocation;
                MinimumGoodPings = localSettings.MinimumGoodPings;
                MinimumInternetSpeed = localSettings.MinimumInternetSpeed;
                TasksLocation = localSettings.TasksLocation;
            }
            catch (Exception)
            {
                if (!IoHelper.FileExists(SettingsFileLocation))
                {
                    Messages.ShowMessage(Messages.Preparing);
                    IoHelper.PrepareEnvironment(new Settings());
                }
                else
                {
                    Messages.ShowMessage(Messages.ErrorInSettings);
                }
            }
        }
    }
}
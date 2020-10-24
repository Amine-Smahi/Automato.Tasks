using System;
using Automato.Helpers;

namespace Automato.Configuration
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
        public string TaskTypeSplitter { get; private set; } = "=>";

        private void LoadSettings()
        {
            try
            {
                var localSettings = IoHelper.Deserialize(SettingsFileLocation);
                DownloadLocation = localSettings.DownloadLocation;
                MinimumGoodPings = localSettings.MinimumGoodPings;
                MinimumInternetSpeed = localSettings.MinimumInternetSpeed;
                TasksLocation = localSettings.TasksLocation;
                TaskTypeSplitter = TaskTypeSplitter;
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
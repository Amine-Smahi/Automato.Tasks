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
        public int MinimumInternetSpeed { get; private set; } = 30;
        public int MinimumGoodPings { get; private set; } = 5;
        public string TasksLocation { get; private set; } = "./MyTasks.txt";
        public string TaskTypeSplitter { get; private set; } = "=>";
        public int WaitFewSecondsForAnotherTry { get; private set; } = 2000;

        private void LoadSettings()
        {
            try
            {
                var localSettings = IoHelper.Deserialize(SettingsFileLocation);
                DownloadLocation = localSettings.DownloadLocation;
                MinimumGoodPings = localSettings.MinimumGoodPings;
                MinimumInternetSpeed = localSettings.MinimumInternetSpeed;
                TasksLocation = localSettings.TasksLocation;
                TaskTypeSplitter = localSettings.TaskTypeSplitter;
                WaitFewSecondsForAnotherTry = localSettings.WaitFewSecondsForAnotherTry;
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
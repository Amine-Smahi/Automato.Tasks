using System;
using Automato.Helpers;
using Automato.ValueObjects;

namespace Automato.Models
{
    public class Settings
    {
        public string SettingsFileLocation { get; } = "./settings.json";
        public string DownloadLocation { get; private set; } = "./downloads";
        public int MinimumInternetSpeed { get; private set; } = 30;
        public int MinimumGoodPings { get; private set; } = 5;
        public string TasksLocation { get; private set; } = "./MyTasks.txt";
        public string TaskTypeSplitter { get; private set; } = "=>";
        public int WaitFewSecondsForAnotherTry { get; private set; } = 2000;

        public void LoadSettings()
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
                    MessagesHelper.DisplayMessage(Messages.Preparing);
                    IoHelper.PrepareEnvironment(new Settings());
                }
                else
                {
                    MessagesHelper.DisplayMessage(Messages.ErrorInSettings);
                }
            }
        }
    }
}

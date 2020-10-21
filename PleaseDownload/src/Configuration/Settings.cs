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
        public int MinimumInternetSpeed { get; private set; } = 30;
        public int MinimumGoodPings { get; private set; } = 5;
        public string DownloadList { get; private set; } = "./files.txt";

        private void LoadSettings()
        {
            try
            {
                var localSettings = IoHelper.Deserialize<Settings>(SettingsFileLocation);
                DownloadLocation = localSettings.DownloadLocation;
                MinimumGoodPings = localSettings.MinimumGoodPings;
                MinimumInternetSpeed = localSettings.MinimumInternetSpeed;
                DownloadList = localSettings.DownloadList;
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
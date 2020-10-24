using System;
using Automato.Constants;
using Automato.Entities;

namespace Automato.Helpers
{
    public class SettingsHelper
    {
        private readonly Settings _settings;

        public SettingsHelper(Settings settings)
        {
            _settings = settings;
        }

        public void LoadSettings()
        {
            try
            {
                var localSettings = IoHelper.Deserialize(_settings.SettingsFileLocation);
                _settings.DownloadLocation = localSettings.DownloadLocation;
                _settings.MinimumGoodPings = localSettings.MinimumGoodPings;
                _settings.MinimumInternetSpeed = localSettings.MinimumInternetSpeed;
                _settings.TasksLocation = localSettings.TasksLocation;
                _settings.TaskTypeSplitter = localSettings.TaskTypeSplitter;
                _settings.WaitFewSecondsForAnotherTry = localSettings.WaitFewSecondsForAnotherTry;
            }
            catch (Exception)
            {
                if (!IoHelper.FileExists(_settings.SettingsFileLocation))
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
using System;
using Automato.Helpers;
using Automato.Models;
using Automato.ValueObjects;

namespace Automato.Behaviors
{
    public class SettingsBehavior
    {
        private readonly Settings _settings;

        public SettingsBehavior(Settings settings)
        {
            _settings = settings;
        }

        public void LoadSettings(bool load)
        {
            if (!load) return;
            try
            {
                SetSettings(IoHelper.Deserialize(_settings.SettingsFileLocation));
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

        private void SetSettings(Settings localSettings)
        {
            _settings.DownloadLocation = localSettings.DownloadLocation;
            _settings.MinimumGoodPings = localSettings.MinimumGoodPings;
            _settings.MinimumInternetSpeed = localSettings.MinimumInternetSpeed;
            _settings.TasksLocation = localSettings.TasksLocation;
            _settings.TaskTypeSplitter = localSettings.TaskTypeSplitter;
            _settings.WaitFewSecondsForAnotherTry = localSettings.WaitFewSecondsForAnotherTry;
        }
    }
}

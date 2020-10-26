using System;
using Automato.Tasks.Helpers;
using Automato.Tasks.Models;
using Automato.Tasks.ValueObjects;

namespace Automato.Tasks.Behaviors
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
                SetSettings(JsonHelper.Deserialize<Settings>(_settings.SettingsFileLocation));
            }
            catch (Exception)
            {
                if (!IoHelper.FileExists(_settings.SettingsFileLocation))
                {
                    MessagesHelper.DisplayMessage(Messages.Preparing);
                    PrepareEnvironment(new Settings());
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

        private static void PrepareEnvironment(Settings settings)
        {
            try
            {
                if (!IoHelper.FileExists(settings.TasksLocation)) IoHelper.OpenOrCreateFile(settings.TasksLocation);
                if (!IoHelper.FileExists(settings.SettingsFileLocation))
                {
                    IoHelper.OpenOrCreateFile(settings.SettingsFileLocation);
                    var content = IoHelper.GetFileContent(settings.SettingsFileLocation);
                    IoHelper.WriteAllText(settings.SettingsFileLocation, JsonHelper.Serialize(content));
                }

                if (!IoHelper.DirectoryExists(settings.DownloadLocation))
                    IoHelper.CreateDirectory(settings.DownloadLocation);
            }
            catch
            {
                MessagesHelper.DisplayMessage(Messages.ErrorInitiatingConfiguration);
            }
        }
    }
}

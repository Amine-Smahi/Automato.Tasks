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
                SetSettings(
                    JsonHelper.Deserialize<Settings>(FilesHelper.GetFileContent(_settings.SettingsFileLocation)));
            }
            catch (Exception)
            {
                if (!FilesHelper.FileExists(_settings.SettingsFileLocation))
                {
                    MessagesHelper.DisplayMessage(Messages.Preparing);
                    PrepareEnvironment(new Settings());
                }
                else if (!CommandsHelper.ShouldExecuteTasks())
                {
                    MessagesHelper.DisplayMessage(Messages.Ready);
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
                if (!FilesHelper.FileExists(settings.TasksLocation))
                    FilesHelper.OpenOrCreateFile(settings.TasksLocation);
                if (!FilesHelper.FileExists(settings.SettingsFileLocation))
                {
                    FilesHelper.OpenOrCreateFile(settings.SettingsFileLocation);
                    FilesHelper.WriteAllText(settings.SettingsFileLocation, JsonHelper.Serialize(settings));
                }

                if (!DirectoriesHelper.DirectoryExists(settings.DownloadLocation))
                    DirectoriesHelper.CreateDirectory(settings.DownloadLocation);
            }
            catch
            {
                MessagesHelper.DisplayMessage(Messages.ErrorInitiatingConfiguration);
            }
        }
    }
}

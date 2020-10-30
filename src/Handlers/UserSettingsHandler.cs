using System;
using Automato.Tasks.Constants;
using Automato.Tasks.Helpers;
using Automato.Tasks.Models;

namespace Automato.Tasks.Handlers
{
    public class UserSettingsHandler
    {
        private readonly UserSettings _userSettings;

        public UserSettingsHandler(UserSettings userSettings)
        {
            _userSettings = userSettings;
        }

        public void LoadSettings(bool load)
        {
            if (!load) return;
            try
            {
                SetSettings(
                    JsonHelper.Deserialize<UserSettings>(
                        FilesHelper.GetFileContent(_userSettings.SettingsFileLocation)));
            }
            catch (Exception)
            {
                if (!FilesHelper.FileExists(_userSettings.SettingsFileLocation))
                {
                    NotificationsHelper.DisplayMessage(Messages.Preparing);
                    PrepareEnvironment(new UserSettings());
                }
                else if (!CommandsHelper.ShouldExecuteTasks())
                {
                    NotificationsHelper.DisplayMessage(Messages.Ready);
                }
                else
                {
                    NotificationsHelper.DisplayMessage(Messages.ErrorInSettings);
                }
            }
        }

        private void SetSettings(UserSettings localUserSettings)
        {
            _userSettings.DownloadLocation = localUserSettings.DownloadLocation;
            _userSettings.MinimumGoodPings = localUserSettings.MinimumGoodPings;
            _userSettings.MinimumInternetSpeed = localUserSettings.MinimumInternetSpeed;
            _userSettings.TasksLocation = localUserSettings.TasksLocation;
            _userSettings.TaskTypeSplitter = localUserSettings.TaskTypeSplitter;
            _userSettings.WaitFewSecondsForAnotherTry = localUserSettings.WaitFewSecondsForAnotherTry;
        }

        private static void PrepareEnvironment(UserSettings userSettings)
        {
            try
            {
                if (!FilesHelper.FileExists(userSettings.TasksLocation))
                    FilesHelper.OpenOrCreateFile(userSettings.TasksLocation);
                if (!FilesHelper.FileExists(userSettings.SettingsFileLocation))
                {
                    FilesHelper.OpenOrCreateFile(userSettings.SettingsFileLocation);
                    FilesHelper.WriteAllText(userSettings.SettingsFileLocation, JsonHelper.Serialize(userSettings));
                }

                if (!DirectoriesHelper.DirectoryExists(userSettings.DownloadLocation))
                    DirectoriesHelper.CreateDirectory(userSettings.DownloadLocation);
            }
            catch
            {
                NotificationsHelper.DisplayMessage(Messages.ErrorInitiatingConfiguration);
            }
        }
    }
}

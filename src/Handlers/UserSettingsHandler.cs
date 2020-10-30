using System;
using Automato.Tasks.Constants;
using Automato.Tasks.Helpers;
using Automato.Tasks.Interfaces;
using Automato.Tasks.Models;

namespace Automato.Tasks.Handlers
{
    public class UserSettingsHandler : IUserSettingsHandler
    {
        public void LoadSettings(UserSettings userSettings, bool load)
        {
            if (!load) return;
            try
            {
                userSettings = GetLocalUserSettings(userSettings);
            }
            catch (Exception)
            {
                if (!FilesHelper.FileExists(userSettings.SettingsFileLocation))
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

        private static UserSettings GetLocalUserSettings(UserSettings userSettings)
        {
            var localUserSettings = JsonHelper.Deserialize<UserSettings>(
                FilesHelper.GetFileContent(userSettings.SettingsFileLocation));
            userSettings = new UserSettings
            {
                DownloadLocation = localUserSettings.DownloadLocation,
                MinimumGoodPings = localUserSettings.MinimumGoodPings,
                MinimumInternetSpeed = localUserSettings.MinimumInternetSpeed,
                TasksLocation = localUserSettings.TasksLocation,
                TaskTypeSplitter = localUserSettings.TaskTypeSplitter,
                WaitFewSecondsForAnotherTry = localUserSettings.WaitFewSecondsForAnotherTry
            };
            return userSettings;
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

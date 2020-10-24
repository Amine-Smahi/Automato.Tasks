using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Automato.Entities;
using Automato.ValueObjects;

namespace Automato.Helpers
{
    public static class IoHelper
    {
        private static string GetFileContent(string settingsFileLocation)
        {
            return File.ReadAllText(settingsFileLocation);
        }

        public static IEnumerable<string> ReadAllLines(string file)
        {
            return File.ReadAllLines(file);
        }

        public static string GetFileName(string path)
        {
            return Path.GetFileName(path);
        }

        public static bool FileExists(string filePath)
        {
            return File.Exists(filePath);
        }

        public static void RemoveTaskFromTheList(string path)
        {
            var linksList = File.ReadAllLines(path).ToList();
            linksList.RemoveAt(0);
            File.WriteAllLines(path, linksList.ToArray());
        }

        public static bool StartDownload(string url, string downloadFolder)
        {
            try
            {
                NetworkHelper.DownloadFile(url, downloadFolder);
                return true;
            }
            catch (Exception)
            {
                PrepareEnvironment(new Settings(false));
                return false;
            }
        }

        public static string CreatePath(string url, string downloadFolder)
        {
            var filename = GetFileName(url);
            var newPath = Path.Combine(downloadFolder, filename);
            return newPath;
        }

        public static void PrepareEnvironment(Settings settings)
        {
            try
            {
                if (!FileExists(settings.TasksLocation)) File.CreateText(settings.TasksLocation);
                if (!FileExists(settings.SettingsFileLocation))
                {
                    var jsonOptions = new JsonSerializerOptions {WriteIndented = true};
                    File.CreateText(settings.SettingsFileLocation);
                    File.WriteAllText(settings.SettingsFileLocation, JsonSerializer.Serialize(settings, jsonOptions));
                }

                if (!Directory.Exists(settings.DownloadLocation)) Directory.CreateDirectory(settings.DownloadLocation);
            }
            catch (Exception)
            {
                MessagesHelper.DisplayMessage(Messages.ErrorInitiatingConfiguration);
            }
        }

        public static Settings Deserialize(string settingsFileLocation)
        {
            var json = GetFileContent(settingsFileLocation);
            return JsonSerializer.Deserialize<Settings>(json);
        }
    }
}
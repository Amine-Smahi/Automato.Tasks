using System;

namespace Automato.Helpers
{
    public static class Messages
    {
        public const string Finish = "Finished!";
        public const string Preparing = "Preparing...";
        public const string StartAgain = "Starting again...";
        public const string ExecutingTask = "Executing command...";
        public const string StartsDownloading = "Starts Downloading...";
        public const string OsNotDetected = "Your operating system is not supported";
        public const string NoInternet = "No internet connection, Make sure you are connected to a wifi";

        public const string ErrorInSettings = "Error while loading the settings, " +
                                              "Make sure the settings.json file exists and in correct format ";

        public const string ErrorInitiatingConfiguration = "Error when initiating the configuration, " +
                                                           "Make sure you have the permission to create files in this location";

        public static string WaitForBetterInternet(double speed)
        {
            return $"Waiting for a better internet connexion speed........({speed} Kb/s)";
        }

        public static string Welcome(in int tasksCount, string tasksLocation)
        {
            return $"Found ({tasksCount}) tasks in {tasksLocation}, Lets Go!\n";
        }

        public static string SuccessfulDownload(string fileName)
        {
            return "\nSuccessfully downloaded " + fileName + "\n";
        }

        public static string FailedDownload(string fileName)
        {
            return "\nFailed downloading " + fileName + "\n";
        }

        public static void ShowMessage(string message)
        {
            Console.WriteLine(message);
        }
    }
}
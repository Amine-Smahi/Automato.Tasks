using System.Diagnostics;
using System.Runtime.InteropServices;
using Automato.Tasks.Constants;

namespace Automato.Tasks.Helpers
{
    public static class SystemsHelper
    {
        private static bool IsWindows()
        {
            return RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        }

        private static bool IsMac()
        {
            return RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
        }

        private static bool IsLinux()
        {
            return RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
        }

        public static void ExecuteCommand(string command)
        {
            if (IsLinux())
                ExecuteCommandForLinux(command);
            else if (IsWindows())
                ExecuteCommandForWindows("");
            else if (IsMac())
                ExecuteCommandForMac("");
            else
                NotificationsHelper.DisplayMessage(Messages.OsNotDetected);
        }

        public static void Sleep()
        {
            if (!CommandsHelper.ShouldSleep()) return;
            if (IsLinux())
                ExecuteCommandForLinux("systemctl suspend");
            else if (IsWindows())
                ExecuteCommandForWindows("");
            else if (IsMac())
                ExecuteCommandForMac("");
            else
                NotificationsHelper.DisplayMessage(Messages.OsNotDetected);
        }

        private static void StartProcessWithResult(Process process)
        {
            var result = string.Empty;
            process.Start();
            result += process.StandardOutput.ReadToEnd();
            result += process.StandardError.ReadToEnd();
            process.WaitForExit();
            NotificationsHelper.DisplayMessage(Messages.DisplayProcessExecutionResult(result));
        }

        private static void ExecuteCommandForMac(string command)
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = "-c \"" + command + "\"",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                }
            };
            StartProcessWithResult(process);
        }

        private static void ExecuteCommandForWindows(string command)
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = "-c \"" + command + "\"",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                }
            };
            StartProcessWithResult(process);
        }

        private static void ExecuteCommandForLinux(string command)
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = "-c \"" + command + "\"",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                }
            };
            StartProcessWithResult(process);
        }

        public static void OpenFile(string filePath)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = filePath,
                UseShellExecute = true,
                Verb = "open"
            });
        }

        public static void OpenDirectory(string directoryPath)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = directoryPath,
                UseShellExecute = true,
                Verb = "open"
            });
        }
    }
}

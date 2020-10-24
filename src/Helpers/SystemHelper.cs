using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Automato.Helpers
{
    public static class SystemHelper
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

        public static void Finish(IReadOnlyList<string> args)
        {
            Messages.ShowMessage(Messages.Finish);
            MakeItSleepIfTrue(args);
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
                Messages.ShowMessage(Messages.OsNotDetected);
        }

        private static void MakeItSleepIfTrue(IReadOnlyList<string> args)
        {
            if (args.Count <= 0) return;
            if (args[0] != "true") return;

            if (IsLinux())
                ExecuteCommandForLinux("systemctl suspend");
            else if (IsWindows())
                ExecuteCommandForWindows("");
            else if (IsMac())
                ExecuteCommandForMac("");
            else
                Messages.ShowMessage(Messages.OsNotDetected);
        }

        private static void StartProcessWithResult(Process process)
        {
            var result = string.Empty;
            process.Start();
            result += process.StandardOutput.ReadToEnd();
            result += process.StandardError.ReadToEnd();
            process.WaitForExit();
            Messages.ShowMessage(Messages.DisplayProcessExecutionResult(result));
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
    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Autotomato.Helpers
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
                SuspendForWindows();
            else if (IsMac())
                SuspendForMac();
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
                SuspendForWindows();
            else if (IsMac())
                SuspendForMac();
            else
                Messages.ShowMessage(Messages.OsNotDetected);
        }

        private static ProcessStartInfo SuspendForMac()
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = "",
                Arguments = "",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            return startInfo;
        }

        private static ProcessStartInfo SuspendForWindows()
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = "",
                Arguments = "",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            return startInfo;
        }

        private static string ExecuteCommandForLinux(string command)
        {
            var result = "";
            using var process = new Process
            {
                StartInfo =
                {
                    FileName = "/bin/bash",
                    Arguments = "-c \"" + command + "\"",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                }
            };
            process.Start();

            result += process.StandardOutput.ReadToEnd();
            result += process.StandardError.ReadToEnd();

            process.WaitForExit();

            Console.WriteLine("\n" + result + "\n");
            return result;
        }
    }
}
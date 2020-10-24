using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace PleaseDownload.Helpers
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

        private static void MakeItSleepIfTrue(IReadOnlyList<string> args)
        {
            if (args.Count <= 0) return;
            if (args[0] != "true") return;

            var process = new Process();

            if (IsLinux())
                process.StartInfo = ExecuteCommandForLinux("systemctl suspend");
            else if (IsWindows())
                process.StartInfo = SuspendForWindows();
            else if (IsMac())
                process.StartInfo = SuspendForMac();
            else
                Messages.ShowMessage(Messages.OsNotDetected);
            process.Start();
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

        private static ProcessStartInfo ExecuteCommandForLinux(string command)
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = "/bin/bash",
                Arguments = $"-c \"{command}\"",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            return startInfo;
        }
    }
}
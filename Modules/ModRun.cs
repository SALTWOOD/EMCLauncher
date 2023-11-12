using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static EMCL.Modules.ModLogger;

namespace EMCL.Modules
{
    public static class ModRun
    {
        public static bool isExited = false;

        public static string RunProcess(string executable, string arg = "", int timeout = -1, string? runAt = null)
        {
            ProcessStartInfo info = new ProcessStartInfo() {
                Arguments = arg,
                FileName = executable,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                WorkingDirectory = runAt ?? ModPath.path.TrimEnd('/')
            };
            if (runAt != null)
            {
                if (info.EnvironmentVariables.ContainsKey("appdata")) { info.EnvironmentVariables["appdata"] = runAt; }
                else { info.EnvironmentVariables.Add("appdata", runAt); }
            }
            Log($"[System] 执行外部命令并等待：{executable} {arg}");
            using (Process process = new Process() { StartInfo = info })
            {
                process.Start();
                process.WaitForExit(timeout);
                if (!process.HasExited) { process.Kill(); }
                return $"{process.StandardOutput.ReadToEnd()} {process.StandardError.ReadToEnd()}";
            }
        }
    }
}

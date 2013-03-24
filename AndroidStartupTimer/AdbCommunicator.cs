using System;
using System.Diagnostics;
using System.Threading;

namespace AndroidStartupTimer
{
    abstract class AdbCommunicator
    {
        private const string AdbPath = @"C:\Users\kapta\AppData\Local\Android\android-sdk\platform-tools\adb.exe";

        protected Process Process;

        protected void ExecuteCommand(string command, bool observeOutput, bool waitForFinish)
        {
            Process = new Process();
            var startInfo = new ProcessStartInfo(AdbPath)
                {
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    Arguments = command
                };
            Process.StartInfo = startInfo;
            Process.Start();

            if (observeOutput)
            {
                ObserveOutput();
            }

            if (waitForFinish)
            {
                while (!Process.HasExited)
                {
                    Thread.Sleep(1);
                }
            }
        }

        private void ObserveOutput()
        {
            Process.OutputDataReceived += (sender, args) => OutputDataReceived(args.Data);
            Process.ErrorDataReceived += (sender, args) => ErrorDataReceived(args.Data);

            Process.BeginOutputReadLine();
            Process.BeginErrorReadLine();
        }

        protected void StopCommand()
        {
            Process.Kill();
        }

        protected virtual void OutputDataReceived(string data)
        {

        }

        protected virtual void ErrorDataReceived(string data)
        {

        }
    }
}

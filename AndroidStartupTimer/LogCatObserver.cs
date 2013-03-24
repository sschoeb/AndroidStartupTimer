using System;
using System.Globalization;

namespace AndroidStartupTimer
{
    class LogCatObserver : AdbCommunicator
    {
        private readonly string _tag;
        private readonly string _packageName;

        public LogCatObserver(string tag, string packageName)
        {
            _tag = tag;
            _packageName = packageName;
        }

        public void Start()
        {
            // First clean existing stuff
            ExecuteCommand("logcat -c", false, true);

            // Then start bserving the device
            ExecuteCommand("logcat -v time", true, true);
        }

        public void Stop()
        {
            StopCommand();
        }

        protected override void OutputDataReceived(string data)
        {
            if (data == null)
            {
                // Happens on disconnect
                return;
            }

            if (data.Contains(string.Format("Start proc {0}", _packageName)))
            {
                AppStartDetected(this, new DateTimeEventArgs { DateTime = ExtractDateTime(data) });
            }

            if (data.Contains("ON CREATE FINISHED"))
            {
                AppStartCompletedDetected(this, new DateTimeEventArgs{DateTime = ExtractDateTime(data)});
            }

            MessageReceived(this, EventArgs.Empty);
        }


        private DateTime ExtractDateTime(string data)
        {
            //03-21 07:46:38.374
            string timeData = data.Substring(0, 18);
            return DateTime.ParseExact(timeData, "MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
        }

        public event EventHandler<DateTimeEventArgs> AppStartDetected;
        public event EventHandler<DateTimeEventArgs> AppStartCompletedDetected;
        public event EventHandler MessageReceived;
    }

    public class DateTimeEventArgs : EventArgs
    {
        public DateTime DateTime { get; set; }
    }
}

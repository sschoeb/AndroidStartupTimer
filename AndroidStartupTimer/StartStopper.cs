using System;

namespace AndroidStartupTimer
{
    class StartStopper : AdbCommunicator
    {
        private readonly string _packageName;
        private readonly string _activityName;
        private readonly bool _startWithPackage;

        public StartStopper(string packageName, string activityName, bool startWithPackage)
        {
            _packageName = packageName;
            _activityName = activityName;
            _startWithPackage = startWithPackage;
        }

        public void StartApp()
        {
            if (_startWithPackage)
            {
                ExecuteCommand(string.Format("shell am start -n {0}/{1}", _packageName, _activityName), false, true);
            }
            else
            {
                ExecuteCommand(string.Format("shell am start -n {0}", _activityName), false, true);
            }
        }

        public void KillApp()
        {
            ExecuteCommand(string.Format("shell am force-stop {0}", _packageName), false, true);
        }
    }
}

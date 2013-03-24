using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace AndroidStartupTimer
{
    class Runner
    {
        private readonly List<StartRun> _runs = new List<StartRun>();
        private StartRun _currentRun;

        private readonly string _packageName;
        private readonly string _activityName;

        private int _remainingStarts = 5;
        private bool _canStartNewApp = true;


        private Thread _observableThread;
        private Thread _measurementThread;
        private bool _observerInitialized;
        private bool _canStopApp;
        private LogCatObserver _logger;
        private readonly bool _startWithPackage;
        private int _startCount;

        public Runner(string packageName, string activityName, bool startWithPackage)
        {
            _packageName = packageName;
            _activityName = activityName;
            _startWithPackage = startWithPackage;
        }

        public void Run()
        {

            _observableThread = new Thread(ObservableThread);
            _measurementThread = new Thread(MeasurementThread);

            _observableThread.Start();
            Console.WriteLine("Wait for LogCat to be attached...");
            while (!_observerInitialized)
            {
                Thread.Sleep(10);
            }
            Console.WriteLine("LogCat ready... start measurements");
            _measurementThread.Start();

            Console.WriteLine("Measurement currently running... ");
            _measurementThread.Join();

            Console.WriteLine("**********************************************");
            Console.WriteLine("Measurement complete, Results:");

            Console.WriteLine("Average:  {0} ms", _runs.Average(x => x.Duration));
            Console.WriteLine("Longest:  {0} ms", _runs.Max(x => x.Duration));
            Console.WriteLine("Shortest: {0} ms", _runs.Min(x => x.Duration));

            Console.WriteLine("**********************************************");


            _logger.Stop();
            _observableThread.Abort();

            _runs.Clear();
        }

        private void MeasurementThread()
        {
            var appStarter = new StartStopper(_packageName, _activityName, _startWithPackage);
            _startCount = _remainingStarts;
            while (_remainingStarts > 0)
            {
                if (_canStartNewApp)
                {
                    appStarter.StartApp();
                    _canStartNewApp = false;
                }

                if (_canStopApp)
                {
                    appStarter.KillApp();
                    _canStartNewApp = true;
                    _canStopApp = false;
                    _remainingStarts--;
                }

                Thread.Sleep(10);
            }
        }

        private void ObservableThread()
        {
            _logger = new LogCatObserver("", _packageName);
            _logger.AppStartDetected += LogCatObserverOnAppStartDetected;
            _logger.AppStartCompletedDetected += LogCatObserverOnAppStartCompletedDetected;
            _logger.MessageReceived += LoggerOnMessageReceived;
            _logger.Start();
        }

        private void LoggerOnMessageReceived(object sender, EventArgs eventArgs)
        {
            _observerInitialized = true;
        }

        private void LogCatObserverOnAppStartCompletedDetected(object sender, DateTimeEventArgs eventArgs)
        {
            if (_currentRun == null)
            {
                return;
            }

            _currentRun.CompleteTime = eventArgs.DateTime;
            _runs.Add(_currentRun);
            Console.WriteLine("Run complete ({0}/{1}): {2}ms", _startCount - _remainingStarts + 1, _startCount, _currentRun.Duration);
            _currentRun = null;

            _canStopApp = true;
        }

        private void LogCatObserverOnAppStartDetected(object sender, DateTimeEventArgs eventArgs)
        {
            _currentRun = new StartRun { StartTime = eventArgs.DateTime };
        }
    }

    class StartRun
    {
        public DateTime StartTime { get; set; }
        public DateTime CompleteTime { get; set; }

        public long Duration
        {
            get
            {
                TimeSpan diff = (CompleteTime - StartTime);
                return diff.Milliseconds + 1000 * diff.Seconds;
            }
        }
    }
}

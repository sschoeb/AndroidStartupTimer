using System;

namespace AndroidStartupTimer
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine();
            Console.WriteLine("*************************************************");
            Console.WriteLine("* App Startup Measurement Tool by Stefan Schoeb *");
            Console.WriteLine("*************************************************");
            Console.WriteLine();

            while (true)
            {
                Console.WriteLine("Write a Command (m=mono/d=dalvik/e=exit):");
                var cmd = Console.ReadLine();
                switch (cmd)
                {
                    case "m":
                        RunMonoTest();
                        break;
                    case "d":
                        RunDalvikTest();
                        break;
                    case "e":
                        return;
                    default:
                        Console.WriteLine("Unknown option, you can use one of the following the following:");
                        Console.WriteLine(" 'm' -> Run C#/Mono-Test");
                        Console.WriteLine(" 'd' -> Run Java/Dalvik-Test");
                        Console.WriteLine(" 'e' -> Exit Test-Application");
                        Console.WriteLine();
                        break;
                }
            }

        }

        private static void RunMonoTest()
        {
            const string packageName = "AndroidAppUnderTest.AndroidAppUnderTest";
            const string activityName = "androidappundertest.Activity1";
            var runner = new Runner(packageName, activityName, true);
            runner.Run();
        }
        private static void RunDalvikTest()
        {
            const string activityName = "com.schoeb.javastartup/.MainActivity";
            const string packageName = "com.schoeb.javastartup";
            var runner = new Runner(packageName, activityName, false);
            runner.Run();
        }
    }
}

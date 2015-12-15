using System;
using System.IO;

namespace DllLog
{
    public static class Log
    {
        private static string LogFile = "SFTClassicLog.txt";
        public enum LogLevel {Info, Warning, Error};

        /// <summary>
        /// Log file
        /// </summary>
        /// <param name="logLvl">The message level. [LogLevel.Info, LogLevel.Warning, LogLevel.Error]</param>
        /// <param name="message">Message to log</param>
        private static void LogAppend(LogLevel logLvl, string message)
        {
            System.IO.StreamWriter sw = new StreamWriter(LogFile, true);
            try
            {
                string logLine = System.String.Format("{0}\t[{1}] - {2}",
                    DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff"), logLvl.ToString(), message); 
                sw.WriteLine(logLine);
            }
            finally
            {
                sw.Close();
            }
        }

        /// <summary>
        /// Log Pass Result
        /// </summary>
        /// <param name="message">Pass message</param>
        public static void LogPass(string message)
        {
            LogAppend(LogLevel.Info, String.Format("PASS\t{0}", message));
        }

        /// <summary>
        /// Log Fail Result.
        /// </summary>
        /// <param name="message">Fail message</param>
        public static void LogFail(string message)
        {
            LogAppend(LogLevel.Info, String.Format("FAIL\t{0}", message));

        }

        /// <summary>
        /// Log comment
        /// </summary>
        /// <param name="message">Comment message</param>
        public static void LogComment(LogLevel logLevel, string message)
        {
            LogAppend(logLevel, String.Format("{0}", message));
        }

        /// <summary>
        /// Log error
        /// </summary>
        /// <param name="message">Error message</param>
        public static void LogError(string message)
        {
            LogAppend(LogLevel.Error, String.Format("{0}", message));
        }

        /// <summary>
        /// Start to log
        /// </summary>
        /// <param name="testName">Test item name to log</param>
        public static void LogStart(string testName)
        {
            LogAppend(LogLevel.Info, String.Format("START\t{0}", testName));
        }

        /// <summary>
        /// Finish to log
        /// </summary>
        /// <param name="testName">Test item name to log</param>
        public static void LogFinish(string testName)
        {
            LogAppend(LogLevel.Info, String.Format("FINISH\t{0}", testName));
        }

    }
}

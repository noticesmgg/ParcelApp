using Serilog;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SharedCore.Utilities
{
    public static class Logger
    {
        private static ConcurrentDictionary<string, DateTime> MsgMap = new ConcurrentDictionary<string, DateTime>();
        private static string AssemblyName { get; set; }
        public static string FullFilePath { get; private set; }
        public static string FileName { get; private set; }
        static Logger()
        {
            Initialize();
        }

        private static void Initialize()
        {
            //Get the calling assembly name


            AssemblyName = Assembly.GetExecutingAssembly()?.GetName().Name;

            MsgMap.Clear();

            var AppName = AssemblyName;

            if (Overrides.AppName != null)
                AppName = Overrides.AppName;

            var filePath = $@"\\corp.mggcap.com\dfs\Shared\MGGINV\Logs\";
            var fileName = AppName;

            if (Overrides.LogFilePath != null)
                filePath = Overrides.LogFilePath;

            if (Overrides.LogFileName != null)
                fileName = Overrides.LogFileName;

            var logDestination = Path.Combine(filePath, fileName + $"_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.log");

            if (Overrides.LogToConsole)
            {
                Log.Logger = new LoggerConfiguration()
                     .WriteTo.File(
                    path: logDestination,
                    rollingInterval: RollingInterval.Day,
                    rollOnFileSizeLimit: true,
                    retainedFileCountLimit: 25,
                    shared: true)
                     .WriteTo.Console()
                     .CreateLogger();
            }
            else
            {
                Log.Logger = new LoggerConfiguration()
                     .WriteTo.File(
                    path: logDestination,
                    rollingInterval: RollingInterval.Day,
                    rollOnFileSizeLimit: true,
                    retainedFileCountLimit: 25,
                    shared: true)
                     .CreateLogger();
            }

            FullFilePath = Path.Combine(Path.GetDirectoryName(logDestination), Path.GetFileNameWithoutExtension(logDestination) + $"{DateTime.Now.ToString("yyyyMMdd")}.log");
            FileName = Path.GetFileName(logDestination);

            Info($" Starting the {AppName}. UserName: {Environment.UserName}");
        }

        static string GetParentCallerName()
        {
            var stackTrace = new StackTrace();
            // Getting the 3rd frame because:
            // 0: GetParentCallerName
            // 1: LogInformation
            // 2: Caller of LogInformation (what we want)
            var frame = stackTrace.GetFrame(2);
            var method = frame?.GetMethod();
            return method?.Name ?? "Unknown";
        }

        public static void Info(string Message)
        {
            Log.Information(Message);
        }

        /// <summary>
        /// Informational Logging
        /// </summary>
        /// <param name="Message">Log Message only once for the current context</param>
        /// <param name="WriteToConsole">Will log to Console,if this flag is true</param>
        public static void Once(string Message)
        {
            if (MsgMap.ContainsKey(Message))
                return;

            Info(Message);

            MsgMap[Message] = DateTime.Now;
        }

        /// <summary>
        /// Logs block of messages
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="WriteToConsole"></param>
        public static void BulkLog(StringBuilder sb)
        {
            var Message = sb.ToString();

            Info(Message.ToString());
        }


        /// <summary>
        /// Informational Logging
        /// </summary>
        /// <param name="Message">Log Message</param>
        /// <param name="WriteToConsole">Will log to Console,if this flag is true</param>
        public static void Debug(string Message)
        {
            Log.Debug(Message);
        }

        /// <summary>
        /// Information
        /// </summary>
        /// <param name="obj">Log Message</param>
        public static void Debug(string obj, params object[] args)
        {
            Debug(string.Format(obj, args));
        }


        /// <summary>
        /// Information
        /// </summary>
        /// <param name="obj">Log Message</param>
        public static void Info(string obj, params object[] args)
        {
            Info(string.Format(obj, args));
        }


        /// <summary>
        /// Warning Logging
        /// </summary>
        /// <param name="Message">Log Message</param>
        /// <param name="WriteToConsole">Will log to Console,if this flag is true</param>
        public static void Warning(string Message)
        {
            Log.Warning(Message);
        }


        /// <summary>
        /// Information
        /// </summary>
        /// <param name="obj">Log Message</param>
        public static void Warning(string obj, params object[] args)
        {
            Warning(string.Format(obj, args));
        }

        /// <summary>
        /// Error Logging
        /// </summary>
        /// <param name="Message">Log Message</param>
        /// <param name="WriteToConsole">Will log to Console,if this flag is true</param>
        public static void Error(string Message)
        {
            Log.Error($"{GetParentCallerName()}: {Message}");

        }

        /// <summary>
        /// Error
        /// </summary>
        /// <param name="obj">Log Message</param>
        public static void Error(string obj, params object[] args)
        {
            Error(string.Format(obj, args));
        }

#pragma warning disable RECS0154 // Parameter is never used

        /// <summary>
        /// Method for MTA
        /// </summary>
        /// <param name="Message">Log Message</param>
        /// <param name="t">Trace Type</param>
        private static void WriteLog(string Message)
        {
            Info(Message);
        }
    }
}

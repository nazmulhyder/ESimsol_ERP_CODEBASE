using System;
using System.IO;
using System.Net;
using System.Xml;
using System.Diagnostics;
using System.Configuration;
using ICS.Core.Utility;
using ICS.Core.Framework;

namespace ICS.Core.Utility
{
    #region Utility: Exception Log
    public class ExceptionLog
    {
        private static ConfigStringParser _config;
        static ExceptionLog()
        {
            _config = new ConfigStringParser();

            if (ConfigurationSettings.AppSettings["ICS.ExceptionHandler"] != null)
                _config.Parse(ConfigurationSettings.AppSettings["ICS.ExceptionHandler"]);
        }

        private static void WriteToEventLog(string log)
        {
            string source = _config["source"];
            try
            {
                EventLog.WriteEntry(source, log, EventLogEntryType.Error);
            }
            catch // Write to file if log failed
            {
                WriteToFile(log);
            }
        }

        private static void WriteToFile(string log)
        {
            string source = _config["source"];

            string path = Path.Combine(Environment.CurrentDirectory, @"Exception Log\" + source);
            string logFileName = Path.Combine(path, "Exception.log");

            int maxSize = 1000 * 1024; // 1 MB
            int maxFiles = 10;

            if (_config["maxFileSize"] != null)
            {
                try
                {
                    maxSize = int.Parse(_config["maxFileSize"]) * 1024; // KB
                }
                catch { }
            }

            if (_config["maxFiles"] != null)
            {
                try
                {
                    maxFiles = int.Parse(_config["maxFiles"]); // KB
                }
                catch { }
            }

            try
            {
                lock (_config)
                {
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    StreamWriter writer;
                    if (!File.Exists(logFileName))
                    {
                        writer = File.CreateText(logFileName);
                    }
                    else
                    {
                        FileInfo fi = new FileInfo(Path.Combine(path, logFileName));
                        if (fi.Length > maxSize)
                        {
                            // Check no of files
                            System.Collections.ArrayList fileList = new System.Collections.ArrayList(Directory.GetFiles(path, "~*.log"));
                            if (fileList.Count >= maxFiles - 1)
                            {
                                fileList.Sort();
                                for (int i = 0; i <= fileList.Count - maxFiles + 1; i++)
                                {
                                    File.Delete((string)fileList[i]);
                                }
                            }

                            fi.MoveTo(Path.Combine(path, "~" + DateTime.Now.Ticks.ToString() + ".log"));
                            writer = File.CreateText(logFileName);
                        }
                        else
                        {
                            writer = File.AppendText(logFileName);
                        }
                    }

                    writer.WriteLine(log);
                    writer.Flush();

                    writer.Close();
                }
            }
            catch { }
        }

        public static void Write(Exception ex)
        {
            string log = new string('#', 80) + Environment.NewLine;
            log += " TimeStamp: " + DateTime.Today.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + Environment.NewLine;
            log += new string('#', 80) + Environment.NewLine;

            while (ex != null)
            {
                log += Environment.NewLine;
                log += "Exception: " + ex.GetType().Name + Environment.NewLine;
                log += "Source: " + ex.Source + Environment.NewLine;
                log += "Message: " + ex.Message + Environment.NewLine;
                log += "Stack Trace: " + Environment.NewLine;
                log += ex.StackTrace;
                ex = ex.InnerException;
                log += Environment.NewLine + new string('-', 80) + Environment.NewLine;
            }

            if (_config["mode"] == "EventLog")
            {
                WriteToEventLog(log);
            }
            else if (_config["mode"] == "File")
            {
                WriteToFile(log);
            }
        }

        public static void Write(ServiceException exception)
        {
            Write((Exception)exception);
        }

        private ExceptionLog() { }
    }
    #endregion
}

using System;
using System.IO;
using System.Text;
using System.Reflection;

namespace BgImgUsingWinSpotlight
{
    internal static class Log
    {
        public static void WriteLog(string message)
        {
            var logFilePath = GetLogFilePath();
            using (var stream = new FileStream(logFilePath, FileMode.Append, FileAccess.Write, FileShare.Read))
            using (var writer = new StreamWriter(stream, Encoding.UTF8))
            {
                writer.WriteLine(BuildLogMessage(message));
            }
        }

        private static string GetLogFilePath()
        {
            var location = Assembly.GetEntryAssembly().Location;
            return Path.Combine(Directory.GetCurrentDirectory(), Path.GetFileNameWithoutExtension(location) + ".log");
        }

        private static string BuildLogMessage(string message)
        {
            var builder = new StringBuilder();
            builder.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            builder.Append(" ");
            builder.Append(message);
            return builder.ToString();
        }
    }
}

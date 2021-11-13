using System.Text;

namespace spotlightbg
{
    internal static class Logger
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
            const string LogFileName = "spotlightbg.log";
            return Path.Combine(Path.GetTempPath(), LogFileName);
        }

        private static string BuildLogMessage(string message)
        {
            return (new StringBuilder())
                .Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                .Append(' ')
                .Append(message)
                .ToString();
        }
    }
}

using System.Text;
using System.ComponentModel;

namespace spotlightbg
{
    internal static class ExceptionHandler
    {
        public static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = (Exception)e.ExceptionObject;
            var exceptionInfoText = BuildExceptionInformationText(ex);
            Logger.WriteLog("**** EXCEPTION ****");
            Logger.WriteLog(exceptionInfoText);
        }

        private static string BuildExceptionInformationText(Exception exception)
        {
            var builder = new StringBuilder();
            var ex = exception;
            while (true)
            {
                builder.AppendLine(string.Format("{0}: {1}", ex.GetType().FullName, ex.Message));

                // Special handling for the specific exceptions.
                switch (ex)
                {
                    case Win32Exception win32Exception:
                        builder
                            .AppendLine(string.Format("NativeErrorCode: {0}", win32Exception.NativeErrorCode))
                            .AppendLine(string.Format("ErrorCode: 0x{0:x8}", win32Exception.ErrorCode))
                            .AppendLine(string.Format("HResult: 0x{0:x8}", win32Exception.HResult));
                        break;
                }

                if (ex.Data.Count != 0)
                {
                    builder.AppendLine("Data:");
                    foreach (string key in ex.Data.Keys)
                    {
                        builder.AppendLine(string.Format("   {0}: {1}", key, ex.Data[key]));
                    }
                }

                builder
                    .AppendLine("Stack Trace:")
                    .AppendLine(ex.StackTrace);

                if (ex.InnerException == null) break;

                ex = ex.InnerException;
                builder.AppendLine(@"--- Inner exception is below ---");
            }

            return builder.ToString();
        }
    }
}

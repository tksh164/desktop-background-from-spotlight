namespace spotlightbg
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(ExceptionHandler.UnhandledExceptionHandler);
            var imageFilePath = WindowsSpotlightHelper.GetSpotlightImageFilePath();
            DesktopBackgroundImageChanger.SetDesktopBackgroundImage(imageFilePath);
        }
    }
}

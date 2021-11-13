using System.ComponentModel;
using System.Runtime.InteropServices;

namespace spotlightbg
{
    internal static class DesktopBackgroundImageChanger
    {
        public static void SetDesktopBackgroundImage(string newBackgroundIamgeFilePath)
        {
            if (!File.Exists(newBackgroundIamgeFilePath))
            {
                throw new FileNotFoundException(string.Format(@"Specified new background image file ""{0}"" does not exist.", newBackgroundIamgeFilePath), newBackgroundIamgeFilePath);
            }

            var result = NativeMethods.SystemParametersInfo(NativeMethods.SystemParametersInfoAction.SPI_SETDESKWALLPAPER, 0, newBackgroundIamgeFilePath, NativeMethods.SystemParametersInfoWinIni.SPIF_UPDATEINIFILE | NativeMethods.SystemParametersInfoWinIni.SPIF_SENDWININICHANGE);
            if (!result)
            {
                throw new Win32Exception(NativeMethods.GetLastError(), "Failed SystemParametersInfo function.");
            }
        }

        private static class NativeMethods
        {
            public enum SystemParametersInfoAction : uint
            {
                SPI_SETDESKWALLPAPER = 0x0014,
            };

            [Flags]
            public enum SystemParametersInfoWinIni : uint
            {
                SPIF_UPDATEINIFILE = 0x01,
                SPIF_SENDWININICHANGE = 0x02,
            };

            [DllImport("user32.dll", CharSet = CharSet.Unicode, EntryPoint = "SystemParametersInfoW", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool SystemParametersInfo([In] SystemParametersInfoAction uiAction, [In] uint uiParam, [In] string pvParam, [In] SystemParametersInfoWinIni fWinIni);

            [DllImport("kernel32.dll", ExactSpelling = true, SetLastError = false)]
            public static extern int GetLastError();
        }
    }
}

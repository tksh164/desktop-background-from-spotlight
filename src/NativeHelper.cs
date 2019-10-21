using System.Runtime.InteropServices;

namespace BgImgUsingWinSpotlight
{
    internal class NativeHelper
    {
        internal static class NativeMethods
        {
            public const uint SPI_SETDESKWALLPAPER = 0x0014;
            public const uint SPIF_UPDATEINIFILE = 0x01;
            public const uint SPIF_SENDWININICHANGE = 0x02;

            [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool SystemParametersInfo(uint uiAction, uint uiParam, string pvParam, uint fWinIni);
        }

        public static bool ChangeDesktopBackgroundImage(string desktopBackgroundIamgeFilePath)
        {
            return NativeMethods.SystemParametersInfo(NativeMethods.SPI_SETDESKWALLPAPER, 0, desktopBackgroundIamgeFilePath, NativeMethods.SPIF_UPDATEINIFILE | NativeMethods.SPIF_SENDWININICHANGE);
        }
    }
}

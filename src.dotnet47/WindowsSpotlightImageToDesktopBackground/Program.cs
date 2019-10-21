using System;
using System.Collections.Generic;
using System.Security.Principal;
using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace WindowsSpotlightImageToDesktopBackground
{
    class Program
    {
        static void Main(string[] args)
        {
            var imageFilePath = GetWindowsSpotlightLandscapeImageFilePath();
            NativeHelper.ChangeDesktopBackgroundImage(imageFilePath);
        }

        private static string GetWindowsSpotlightLandscapeImageFilePath()
        {
            string imageFilePath;

            using (var regKeyLocalMachine = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
            {
                var latestImageSubKeyName = GetLatestImageSubKeyName(regKeyLocalMachine);
                using (var regKey = regKeyLocalMachine.OpenSubKey(latestImageSubKeyName))
                {
                    imageFilePath = (string)regKey.GetValue("landscapeImage", null, RegistryValueOptions.DoNotExpandEnvironmentNames);
                }
            }

            if (string.IsNullOrWhiteSpace(imageFilePath))
            {
                throw new InvalidOperationException("Cannot get landscapeImage value.");
            }

            return imageFilePath;
        }

        private static string GetLatestImageSubKeyName(RegistryKey regKeyLocalMachine)
        {
            var userSid = WindowsIdentity.GetCurrent().User.Value;
            var subKeyName = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Authentication\LogonUI\Creative\" + userSid;

            using (var regKey = regKeyLocalMachine.OpenSubKey(subKeyName))
            {
                var subKeyNames = new List<string>(regKey.GetSubKeyNames());
                subKeyNames.Sort();

                return subKeyName + @"\" + subKeyNames[subKeyNames.Count - 1];
            }
        }
    }

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

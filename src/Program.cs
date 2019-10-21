using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Principal;
using Microsoft.Win32;

[assembly: AssemblyCompany("Takeshi Katano")]
[assembly: AssemblyCopyright("Copyright (C) 2017 Takeshi Katano. All rights reserved.")]
[assembly: AssemblyDescription("Utility tool for setting the desktop background image using the Windows Spotlight image.")]
[assembly: AssemblyTitle("Utility tool for setting the desktop background image using the Windows Spotlight image.")]
[assembly: AssemblyProduct("BgImgUsingWinSpotlight")]
[assembly: AssemblyFileVersion("1.2.0.0")]
[assembly: AssemblyInformationalVersion("1.2.0")]
[assembly: AssemblyVersion("1.2.0.0")]
namespace BgImgUsingWinSpotlight
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            var imageFilePath = GetWindowsSpotlightImageFilePath();
            NativeHelper.ChangeDesktopBackgroundImage(imageFilePath);
        }

        private static string GetWindowsSpotlightImageFilePath()
        {
            using (var regKeyLocalMachine = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
            {
                var latestImageSubKeyPath = GetLatestImageSubKeyPath(regKeyLocalMachine);
                return GetLandscapeImageFilePath(regKeyLocalMachine, latestImageSubKeyPath);
            }
        }

        private static string GetLatestImageSubKeyPath(RegistryKey regKeyLocalMachine)
        {
            var userSid = WindowsIdentity.GetCurrent().User.Value;
            var parentSubKeyPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Authentication\LogonUI\Creative\" + userSid;

            using (var regKey = regKeyLocalMachine.OpenSubKey(parentSubKeyPath))
            {
                var subKeyNames = new List<string>(regKey.GetSubKeyNames());
                subKeyNames.Sort();

                return parentSubKeyPath + @"\" + subKeyNames[subKeyNames.Count - 1];
            }
        }

        private static string GetLandscapeImageFilePath(RegistryKey regKeyLocalMachine, string subKeyPath)
        {
            using (var regKey = regKeyLocalMachine.OpenSubKey(subKeyPath))
            {
                var imageFilePath = (string)regKey.GetValue("landscapeImage", null, RegistryValueOptions.DoNotExpandEnvironmentNames);
                if (string.IsNullOrWhiteSpace(imageFilePath))
                {
                    throw new InvalidOperationException("Cannot get landscapeImage value.");
                }
                return imageFilePath;
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
}
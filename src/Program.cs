using System;
using System.Collections.Generic;
using System.Reflection;
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
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(ExceptionTrapper.UnhandledExceptionTrapper);

            var imageFilePath = GetWindowsSpotlightImageFilePath();
            if (imageFilePath != null)
            {
                NativeHelper.ChangeDesktopBackgroundImage(imageFilePath);
            }
        }

        private static string GetWindowsSpotlightImageFilePath()
        {
            using (var regKeyLocalMachine = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
            {
                var latestImageSubKeyPath = GetLatestImageSubKeyPath(regKeyLocalMachine);
                return GetLandscapeImageFilePath(regKeyLocalMachine, latestImageSubKeyPath);
            }
        }

        private static string GetLatestImageSubKeyPath(RegistryKey regKey)
        {
            var userSid = WindowsIdentity.GetCurrent().User.Value;
            var parentSubKeyPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Authentication\LogonUI\Creative\" + userSid;

            using (var regSubKey = regKey.OpenSubKey(parentSubKeyPath))
            {
                var subKeyNames = new List<string>(regSubKey.GetSubKeyNames());

                if (subKeyNames.Count == 0)
                {
                    Log.WriteLog(string.Format("Couldn't find the latest image subkey under \"HKEY_LOCAL_MACHINE\\{0}\".", parentSubKeyPath));
                    return null;
                }

                subKeyNames.Sort();
                return parentSubKeyPath + @"\" + subKeyNames[subKeyNames.Count - 1];
            }
        }

        private static string GetLandscapeImageFilePath(RegistryKey regKey, string subKeyPath)
        {
            if (string.IsNullOrWhiteSpace(subKeyPath)) return null;

            using (var regSubKey = regKey.OpenSubKey(subKeyPath))
            {
                var imageFilePath = (string)regSubKey.GetValue("landscapeImage", null, RegistryValueOptions.DoNotExpandEnvironmentNames);
                if (string.IsNullOrWhiteSpace(imageFilePath))
                {
                    Log.WriteLog(string.Format("Couldn't get the image file path from the landscapeImage value under \"HKEY_LOCAL_MACHINE\\{0}\".", subKeyPath));
                }
                return imageFilePath;
            }
        }
    }
}

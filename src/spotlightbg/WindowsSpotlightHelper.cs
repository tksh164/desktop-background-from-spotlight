using System.Security.Principal;
using Microsoft.Win32;
using spotlightbg.Exceptions;

namespace spotlightbg
{
    internal static class WindowsSpotlightHelper
    {
        public static string GetSpotlightImageFilePath()
        {
            // TODO: Support x86.
            using (var hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
            {
                var latestImageRegKeyPath = GetLatestSpotlightImageRegistryKeyPath(hklm);
                return GetProperSpotlightImageFilePath(hklm, latestImageRegKeyPath);
            }
        }

        private static string GetLatestSpotlightImageRegistryKeyPath(RegistryKey hklm)
        {
            var userSid = GetCurrentUserSID();
            var userRegKeyPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Authentication\LogonUI\Creative\" + userSid;
            using (var userRegKey = hklm.OpenSubKey(userRegKeyPath))
            {
                if (userRegKey == null)
                {
                    throw new RegistryKeyCouldNotOpenException(Path.Combine(hklm.Name, userRegKeyPath));
                }

                var imageRegKeyNames = userRegKey.GetSubKeyNames().ToList();
                if (imageRegKeyNames.Count == 0)
                {
                    throw new SpotlightImageRegistryKeyNotFoundException(userRegKey.Name);
                }
                return Path.Combine(userRegKeyPath, imageRegKeyNames.Last());
            }
        }

        private static string GetCurrentUserSID()
        {
            var userSid = WindowsIdentity.GetCurrent()?.User?.Value;
            if (userSid == null)
            {
                throw new CurrentUserSidCouldNotGetException();
            }
            return userSid;
        }

        private static string GetProperSpotlightImageFilePath(RegistryKey regKey, string imageRegKeyPath)
        {
            using (var imageRegKey = regKey.OpenSubKey(imageRegKeyPath))
            {
                if (imageRegKey == null)
                {
                    throw new RegistryKeyCouldNotOpenException(Path.Combine(regKey.Name, imageRegKeyPath));
                }

                var regValueName = GetImageRegValueName();
                var imageFilePath = (string?)imageRegKey.GetValue(regValueName, null, RegistryValueOptions.None);
                if (string.IsNullOrWhiteSpace(imageFilePath))
                {
                    throw new InvalidSpotlightImageRegistryValueException(regValueName, imageRegKey.Name);
                }
                return imageFilePath;
            }
        }

        private static string GetImageRegValueName()
        {
            // NOTE: SystemInformation.ScreenOrientation returns the main display's orientation.
            if (SystemInformation.ScreenOrientation == ScreenOrientation.Angle0 || SystemInformation.ScreenOrientation == ScreenOrientation.Angle180)
            {
                return "landscapeImage";
            }
            else if (SystemInformation.ScreenOrientation == ScreenOrientation.Angle90 || SystemInformation.ScreenOrientation == ScreenOrientation.Angle270)
            {
                return "portraitImage";
            }
            else
            {
                throw new NotImplementedException(string.Format(@"""{0}"" is the unexpected value.", SystemInformation.ScreenOrientation.ToString()));
            }
        }
    }
}

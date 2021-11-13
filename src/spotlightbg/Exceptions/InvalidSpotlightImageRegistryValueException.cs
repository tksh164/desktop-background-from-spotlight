using System.Runtime.Serialization;

namespace spotlightbg.Exceptions
{
    [Serializable]
    public class InvalidSpotlightImageRegistryValueException : Exception
    {
        private const string DefaultMessage = "Couldn't get the image file path.";

        public string? RegistryValueName { get; private set; }
        public string? RegistryKeyPath { get; private set; }

        public InvalidSpotlightImageRegistryValueException()
            : base(DefaultMessage)
        { }

        public InvalidSpotlightImageRegistryValueException(string registryValueName, string registryKeyPath)
            : base(string.Format(@"Couldn't get the image file path from the ""{0}"" under ""{1}"".", registryValueName, registryKeyPath))
        {
            RegistryKeyPath = registryKeyPath;
            RegistryValueName = registryValueName;
        }

        public InvalidSpotlightImageRegistryValueException(string? message, string? registryValueName, string? registryKeyPath)
            : base(message ?? DefaultMessage)
        {
            RegistryValueName = registryValueName;
            RegistryKeyPath = registryKeyPath;
        }

        public InvalidSpotlightImageRegistryValueException(string? message, Exception? innerException)
            : base(message ?? DefaultMessage, innerException)
        { }

        public InvalidSpotlightImageRegistryValueException(string? message, string? registryValueName, string? registryKeyPath, Exception? innerException)
            : base(message ?? DefaultMessage, innerException)
        {
            RegistryValueName = registryValueName;
            RegistryKeyPath = registryKeyPath;
        }

        protected InvalidSpotlightImageRegistryValueException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
    }
}

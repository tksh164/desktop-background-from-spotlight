using System.Runtime.Serialization;

namespace spotlightbg.Exceptions
{
    [Serializable]
    public class RegistryKeyCouldNotOpenException : Exception
    {
        private const string DefaultMessage = "Couldn't open the registry key.";

        public string? RegistryKeyPath { get; private set; }

        public RegistryKeyCouldNotOpenException()
            : base(DefaultMessage)
        { }

        public RegistryKeyCouldNotOpenException(string registryKeyPath)
            : base(string.Format(@"Couldn't open the registry key ""{0}"".", registryKeyPath))
        {
            RegistryKeyPath = registryKeyPath;
        }

        public RegistryKeyCouldNotOpenException(string? message, string? registryKeyPath)
            : base(message ?? DefaultMessage)
        {
            RegistryKeyPath = registryKeyPath;
        }

        public RegistryKeyCouldNotOpenException(string? message, Exception? innerException)
            : base(message ?? DefaultMessage, innerException)
        { }

        public RegistryKeyCouldNotOpenException(string? message, string? registryKeyPath, Exception? innerException)
            : base(message ?? DefaultMessage, innerException)
        {
            RegistryKeyPath = registryKeyPath;
        }

        protected RegistryKeyCouldNotOpenException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
    }
}

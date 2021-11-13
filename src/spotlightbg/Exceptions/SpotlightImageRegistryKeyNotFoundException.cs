using System.Runtime.Serialization;

namespace spotlightbg.Exceptions
{
    [Serializable]
    public class SpotlightImageRegistryKeyNotFoundException : Exception
    {
        private const string DefaultMessage = "There were no spotlight image registry keys.";

        public string? ParentRegistryKeyPath { get; private set; }

        public SpotlightImageRegistryKeyNotFoundException() : base(DefaultMessage)
        { }

        public SpotlightImageRegistryKeyNotFoundException(string parentRegistryKeyPath)
            : base(string.Format(@"There were no spotlight image registry keys under ""{0}"".", parentRegistryKeyPath))
        {
            ParentRegistryKeyPath = parentRegistryKeyPath;
        }

        public SpotlightImageRegistryKeyNotFoundException(string? message, string? parentRegistryKeyPath)
            : base(message ?? DefaultMessage)
        {
            ParentRegistryKeyPath = parentRegistryKeyPath;
        }

        public SpotlightImageRegistryKeyNotFoundException(string? message, Exception? innerException)
            : base(message ?? DefaultMessage, innerException)
        { }

        public SpotlightImageRegistryKeyNotFoundException(string? message, string? parentRegistryKeyPath, Exception? innerException)
            : base(message ?? DefaultMessage, innerException)
        {
            ParentRegistryKeyPath = parentRegistryKeyPath;
        }

        protected SpotlightImageRegistryKeyNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        { }
    }
}

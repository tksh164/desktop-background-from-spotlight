using System.Runtime.Serialization;

namespace spotlightbg.Exceptions
{
    [Serializable]
    public class CurrentUserSidCouldNotGetException : Exception
    {
        private const string DefaultMessage = "Couldn't get current user's SID.";

        public CurrentUserSidCouldNotGetException()
            : base(DefaultMessage)
        { }

        public CurrentUserSidCouldNotGetException(string? message)
            : base(message ?? DefaultMessage)
        { }

        public CurrentUserSidCouldNotGetException(string? message, Exception? innerException)
            : base(message ?? DefaultMessage, innerException)
        { }

        protected CurrentUserSidCouldNotGetException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
    }
}

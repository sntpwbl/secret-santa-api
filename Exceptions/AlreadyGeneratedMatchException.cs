using System;

namespace SecretSanta.Exceptions{
    public class AlreadyGeneratedMatchException : Exception
    {
        public AlreadyGeneratedMatchException(string message) : base(message) { }
    }
}
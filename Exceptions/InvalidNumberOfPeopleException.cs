using System;

namespace SecretSanta.Exceptions{
    public class InvalidNumberOfPeopleException : Exception
    {
        public InvalidNumberOfPeopleException(string message) : base(message) { }
    }
}
using System;

namespace SpotFinder.Exceptions
{
    public class EditUserException : Exception
    {
        public EditUserException() : base() { }
        public EditUserException(string message) : base(message) { }

        public string ServerErrorMessage { get; private set; }
        public string EmailOccupidMessage { get; private set; }

        public EditUserException(string serverError, string emailOccupid)
        {
            ServerErrorMessage = serverError;
            EmailOccupidMessage = emailOccupid;
        }
    }
}

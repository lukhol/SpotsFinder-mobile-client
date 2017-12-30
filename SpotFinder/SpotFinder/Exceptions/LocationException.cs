using System;

namespace SpotFinder.Exceptions
{
    public class LocationException : Exception
    {
        public LocationException() : base() { }

        public LocationException(string message) : base(message) { }
    }
}

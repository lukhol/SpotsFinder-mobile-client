using System;

namespace SpotFinder.SQLite
{
    public class NotFoundPlaceException : Exception
    {
        public NotFoundPlaceException() : base() { }

        public NotFoundPlaceException(string message) : base(message) { }
    }
}

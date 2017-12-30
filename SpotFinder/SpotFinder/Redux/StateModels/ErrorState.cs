using System;

namespace SpotFinder.Redux.StateModels
{
    public class ErrorState
    {
        public Exception Error { get; private set; }
        public string WhereOccurred { get; private set; }

        [Obsolete]
        public ErrorState() { }

        public ErrorState(Exception error, string whereOccurred)
        {
            Error = error;
            WhereOccurred = whereOccurred;
        }
    }
}

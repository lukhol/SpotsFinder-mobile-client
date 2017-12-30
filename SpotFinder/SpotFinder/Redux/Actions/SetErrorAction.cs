using Redux;
using System;

namespace SpotFinder.Redux.Actions
{
    public class SetErrorAction : IAction
    {
        public Exception Error { get; private set; }
        public string WhereOccurred { get; private set; }

        public SetErrorAction(Exception error, string whereOccurred)
        {
            if (error == null || whereOccurred == null)
                throw new ArgumentException("You cannot pass null to this action!");

            Error = error;
            WhereOccurred = whereOccurred;
        }
    }
}

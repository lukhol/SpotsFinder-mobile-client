using Redux;
using SpotFinder.Core.Enums;
using SpotFinder.Redux.StateModels;
using System;

namespace SpotFinder.Redux.Actions.WrongPlaceReports
{
    public class SetWrongPlaceReportCompleteAction : IAction
    {
        public WrongPlaceReport WrongPlaceReport { get; private set; }
        public Exception Error { get; private set; }
        public Status Status { get; private set; }

        public SetWrongPlaceReportCompleteAction(WrongPlaceReport wrongPlaceReport)
        {
            WrongPlaceReport = wrongPlaceReport ?? throw new ArgumentException("WrongPlaceReport cannot be null.");
            Status = Status.Success;
        }

        public SetWrongPlaceReportCompleteAction(Exception error)
        {
            Error = error ?? throw new ArgumentException("Exception cannot be null.");
            Status = Status.Error;
        }
    }
}

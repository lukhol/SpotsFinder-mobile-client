using Redux;
using SpotFinder.Core.Enums;
using SpotFinder.Redux.StateModels;
using System;

namespace SpotFinder.Redux.Actions.WrongPlaceReports
{
    public class SetWrongPlaceReportStartAction : IAction
    {
        public WrongPlaceReport WrongPlaceReport { get; private set; }
        public Status Status { get; private set; }

        public SetWrongPlaceReportStartAction(WrongPlaceReport wrongPlaceReport)
        {
            if (wrongPlaceReport == null)
                throw new ArgumentException("You cannot set null as WrongPlaceReport.");

            Status = Status.Setting;
        }
    }
}

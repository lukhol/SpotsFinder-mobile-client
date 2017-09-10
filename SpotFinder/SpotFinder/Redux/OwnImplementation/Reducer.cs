using System;

namespace SpotFinder.Redux.OwnImplementation
{
    public static class Reducer
    {
        public static State Reduce(State state, object action)
        {
            if(action is GoNextAction)
            {
                var goNextAction = action as GoNextAction;
                return new State
                {
                    PageType = goNextAction.PageType,
                    ReportManager = state.ReportManager,
                };
            }
            return state;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotFinder.Redux
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
                    Settings = state.Settings
                };
            }
            return state;
        }
    }
}

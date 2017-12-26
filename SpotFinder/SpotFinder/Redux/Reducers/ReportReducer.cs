using Redux;
using SpotFinder.Redux.StateModels;

namespace SpotFinder.Redux.Reducers
{
    public class ReportReducer : IReducer<Report>
    {
        public Report Reduce(Report previousState, IAction action)
        {
            return previousState;
        }
    }
}

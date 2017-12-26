using Redux;
using SpotFinder.Models.Core;

namespace SpotFinder.Redux.Reducers
{
    public class CriteriaReducer : IReducer<Criteria>
    {
        public Criteria Reduce(Criteria previousState, IAction action)
        {
            return previousState;
        }
    }
}

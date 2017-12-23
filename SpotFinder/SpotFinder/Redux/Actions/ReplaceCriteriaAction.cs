using Redux;
using SpotFinder.Models.Core;

namespace SpotFinder.Redux.Actions
{
    class ReplaceCriteriaAction : IAction
    {
        public Criteria Criteria { get; private set; }

        public ReplaceCriteriaAction(Criteria criteria)
        {
            Criteria = criteria;
        }
    }
}

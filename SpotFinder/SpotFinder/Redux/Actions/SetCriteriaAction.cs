using Redux;
using SpotFinder.Models.Core;

namespace SpotFinder.Redux.Actions
{
    class SetCriteriaAction : IAction
    {
        public Criteria Criteria { get; private set; }

        public SetCriteriaAction(Criteria criteria)
        {
            Criteria = criteria;
        }
    }
}

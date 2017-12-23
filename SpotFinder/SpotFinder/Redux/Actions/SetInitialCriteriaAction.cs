using Redux;
using SpotFinder.Models.Core;

namespace SpotFinder.Redux.Actions
{
    public class SetInitialCriteriaAction : IAction
    {
        public Criteria Criteria { get; private set; }

        public SetInitialCriteriaAction(Criteria criteria)
        {
            Criteria = criteria;
        }
    }
}

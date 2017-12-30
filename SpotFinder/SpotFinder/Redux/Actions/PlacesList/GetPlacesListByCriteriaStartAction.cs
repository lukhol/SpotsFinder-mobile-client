using Redux;
using SpotFinder.Core.Enums;
using SpotFinder.Models.Core;

namespace SpotFinder.Redux.Actions.PlacesList
{
    public class GetPlacesListByCriteriaStartAction : IAction
    {
        public Criteria Criteria { get; private set; }
        public Status Status { get; private set; }

        public GetPlacesListByCriteriaStartAction(Criteria criteria)
        {
            Criteria = criteria;
            Status = Status.Getting;
        }
    }
}

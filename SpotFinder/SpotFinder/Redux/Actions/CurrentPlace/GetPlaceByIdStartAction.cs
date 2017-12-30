using Redux;

namespace SpotFinder.Redux.Actions.CurrentPlace
{
    public class GetPlaceByIdStartAction : IAction
    {
        public int Id { get; private set; }

        public GetPlaceByIdStartAction(int id)
        {
            Id = id;
        }
    }
}

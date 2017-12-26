using Redux;

namespace SpotFinder.Redux.Actions.CurrentPlace
{
    public class DownloadPlaceByIdStartAction : IAction
    {
        public int Id { get; private set; }

        public DownloadPlaceByIdStartAction(int id)
        {
            Id = id;
        }
    }
}

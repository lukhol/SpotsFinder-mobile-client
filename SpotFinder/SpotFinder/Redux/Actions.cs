using Redux;

namespace SpotFinder.Redux
{
    public class InitAction : IAction { }
    public class DownloadSinglePlaceAction : IAction
    {
        public int Id { get; private set; }

        public DownloadSinglePlaceAction(int id)
        {
            Id = id;
        }
    }
}

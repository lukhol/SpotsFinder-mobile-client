using Redux;

namespace SpotFinder.Redux.Actions
{
    public class RequestDownloadSpotAction : IAction
    {
        public int Id { get; }

        public RequestDownloadSpotAction(int id)
        {
            Id = id;
        }
    }
}

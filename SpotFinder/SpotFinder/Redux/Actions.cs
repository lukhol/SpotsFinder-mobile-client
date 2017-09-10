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

    public class SaveSettingsAction : IAction
    {
        public int Distance { get; private set; }
        public string City { get; private set; }

        public SaveSettingsAction(string city, int distance)
        {
            Distance = distance;
            City = city;
        }
    }
}

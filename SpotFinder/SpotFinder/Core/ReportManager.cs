using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpotFinder.Core
{
    public class ReportManager
    {
        public delegate void Start();
        public delegate void Stop();

        public event Stop StopEvent;
        public event Start StartEvent;

        private IPlaceRepository PlaceRepository;

        public ReportManager(IPlaceRepository placeRepository)
        {
            PlaceRepository = placeRepository ?? throw new ArgumentNullException("placeRepository is null in PlaceRepository");
        }

        private Place place;
        private Criteria criteria;
        public List<Place> DownloadedPlaces { get; set; }

        public Place Place
        {
            get => place;
            set { place = value; }
        }

        public Criteria Criteria
        {
            get => criteria;
            set
            {
                criteria = value;
                StartEvent?.Invoke();
                Task.Run(async () => 
                {
                    DownloadedPlaces = await PlaceRepository.GetPlacesByCriteriaAsync(criteria);
                    //DownloadedPlaces = await PlaceRepository.GetAllPlaceAsync();
                })
                .ContinueWith((t) =>
                {
                    StopEvent?.Invoke();
                });
            }
        }

        public void CreateNewReport()
        {
            place = new Place();
        }
    }
}

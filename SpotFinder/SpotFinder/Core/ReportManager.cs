using SpotFinder.DataServices;
using SpotFinder.Models.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpotFinder.Core
{
    public class ReportManager
    {
        public delegate void Start();
        public delegate void Stop();

        public event Start DownloadFinished;

        public event Stop StopEvent;
        public event Start StartEvent;

        private IPlaceService PlaceRepository;

        public ReportManager(IPlaceService placeRepository)
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

        public Place CurrentPlaceToShow { get; set; }

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

        public void RequestDownloadPlace(int id)
        {
            Task.Run(async () =>
            {
                CurrentPlaceToShow = await PlaceRepository.GetPlaceById(id);
            })
            .ContinueWith((task) =>
            {
                DownloadFinished?.Invoke();
            });
        }
    }
}

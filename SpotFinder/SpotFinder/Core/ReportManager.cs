using SpotFinder.DataServices;
using SpotFinder.Models.Core;
using SpotFinder.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpotFinder.Core
{
    public class ReportManager
    {
        public event Action PlaceDownloaded;

        public event Action StopEvent;
        public event Action StartEvent;

        private IPlaceService PlaceService;
        private ILocalPlaceRepository LocalPlaceRepository;

        public ReportManager(IPlaceService placeRepository)
        {
            PlaceService = placeRepository ?? throw new ArgumentNullException("placeRepository is null in PlaceRepository");
            LocalPlaceRepository = new LocalPlaceRepository();
        }

        private Place place;
        private Criteria criteria;
        public List<Place> DownloadedPlaces { get; set; }

        public Place AddingPlace
        {
            get => place;
            set { place = value; }
        }

        public Place ShowingPlace { get; set; }

        public Criteria Criteria
        {
            get => criteria;
            set
            {
                criteria = value;
                StartEvent?.Invoke();
                Task.Run(async () =>
                {
                    DownloadedPlaces = await PlaceService.GetPlacesByCriteriaAsync(criteria);
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
                //Try to get from localRepository
                var placeFromLocalRepository = LocalPlaceRepository.GetPlaceOryginal(id);
                if (placeFromLocalRepository == null)
                {
                    //Download if place does not exist in local db
                    ShowingPlace = await PlaceService.GetPlaceById(id);
                }
                else
                {
                    ShowingPlace = placeFromLocalRepository;
                }

            })
            .ContinueWith((task) =>
            {
                //Save downloaded spot to local db
                if (ShowingPlace != null)
                {
                    LocalPlaceRepository.InsertPlaceOryginal(ShowingPlace);
                }

                PlaceDownloaded?.Invoke();
            });
        }

        public Location Location { get; set; }

    }
}

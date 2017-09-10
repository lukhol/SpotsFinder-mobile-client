using SpotFinder.DataServices;
using SpotFinder.Models.Core;
using SpotFinder.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpotFinder.Redux
{
    public class ApplicationState
    {
        private IPlaceService PlaceService;
        private ILocalPlaceRepository LocalPlaceRepository;

        public event Action PlaceDownloaded;
        public event Action StopEvent;
        public event Action StartEvent;

        public List<Place> DownloadedPlaces { get; set; }

        private Place addingPlace;
        public Place AddingPlace
        {
            get => addingPlace;
            set { addingPlace = value; }
        }

        private Place showingPlace;
        public Place ShowingPlace
        {
            get => showingPlace;
            set
            {
                showingPlace = value;
            }
        }

        private Criteria criteria;
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

        private int globalDistance;
        public int GlobalDistance
        {
            get => globalDistance;
            set
            {
                globalDistance = value;
            }
        }

        private string mainCity;
        public string MainCity
        {
            get => mainCity;
            set
            {
                mainCity = value;
            }
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

        public ApplicationState(IPlaceService placeService, ILocalPlaceRepository localPlaceRepository)
        {
            PlaceService = placeService ?? throw new ArgumentNullException("placeService is null in ApplicationState constructor,");
            LocalPlaceRepository = localPlaceRepository ?? throw new ArgumentNullException("localPlaceRepository is null in ApplicationState constructor.");
        }
    }
}

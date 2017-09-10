using SpotFinder.DataServices;
using SpotFinder.Models.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpotFinder.Redux
{
    public class ApplicationState
    {
        private IPlaceService PlaceService;

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
                ShowingPlace = await PlaceService.GetPlaceById(id);
            })
            .ContinueWith((task) =>
            {
                PlaceDownloaded?.Invoke();
            });
        }

        public ApplicationState(IPlaceService placeService)
        {
            PlaceService = placeService ?? throw new ArgumentNullException("placeService is null in ApplicationState constructor");
        }
    }
}

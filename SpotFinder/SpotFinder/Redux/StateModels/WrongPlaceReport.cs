using System;

namespace SpotFinder.Redux.StateModels
{
    public class WrongPlaceReport
    {
        public int PlaceId { get; private set; }
        public long PlaceVersion { get; private set; }
        public long UserId { get; private set; }
        public string DeviceId { get; private set; }
        public string ReasonComment { get; private set; }
        public bool IsNotSkateboardPlace { get; private set; }

        [Obsolete]
        public WrongPlaceReport() { }

        public WrongPlaceReport(
            int placeId, 
            long placeVersion, 
            long userId,
            string deviceId, 
            string reasonComment,
            bool isNotSkateboardPlace
            )
        {
            PlaceId = placeId;
            PlaceVersion = placeVersion;
            UserId = userId;
            DeviceId = deviceId;
            ReasonComment = reasonComment;
            IsNotSkateboardPlace = isNotSkateboardPlace;
        }
    }
}

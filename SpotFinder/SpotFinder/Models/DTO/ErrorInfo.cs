using System;

namespace SpotFinder.Models.DTO
{
    public class ErrorInfo
    {
        public Exception Exception { get; set; }
        public string WhereOccurred { get; set; }

        public string Idiom { get; set; }
        public string Model { get; set; }
        public string Version { get; set; }
        public string VersionNumber { get; set; }
        public string Platform { get; set; }
    }
}

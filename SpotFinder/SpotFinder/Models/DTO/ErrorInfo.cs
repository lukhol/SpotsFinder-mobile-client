namespace SpotFinder.Models.DTO
{
    public class ErrorInfo
    {
        public string ClassName { get; set; }
        public string Message { get; set; }
        public string StackTraceString { get; set; }
        public string WhereOccurred { get; set; }

        public DeviceInfo DeviceInfo { get; set; }
    }
}

using SpotFinder.Core;

namespace SpotFinder.Redux
{
    public class State
    {
        public ReportManager ReportManager { get; set; }
        public Settings Settings { get; set; }
        public System.Type PageType { get; set; }
    }
}

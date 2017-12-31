namespace SpotFinder.Config
{
    public interface IBootstrapper
    {
        void OnStart();
        void OnSleep();
        void OnResume();
    }
}

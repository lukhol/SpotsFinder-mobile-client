using System;

namespace SpotFinder.Redux
{
    public static class ObservableExtension
    {
        public static IDisposable SubscribeWithError<T>(this IObservable<T> observable, Action<T> action, Action<Exception> errorHandler)
        {
            return observable
                .Subscribe(t =>
                {
                    try { action(t); }
                    catch (Exception e) { errorHandler(e); }
                });
        }
    }
}

using System;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Redux
{
    public static class StoreExtensions
    {
        public delegate Task AsyncActionCreator<TState>(Dispatcher dispatcher, Func<TState> getState);
        public delegate void ActionsCreator<TState>(Dispatcher dispatch, Func<TState> getState);

        public static Task DispatchAsync<TState>(this IStore<TState> store, AsyncActionCreator<TState> actionsCreator)
        {
            return actionsCreator(store.Dispatch, store.GetState);
        }

        public static void Dispatch<TState>(this IStore<TState> store, ActionsCreator<TState> actionsCreator)
        {
            actionsCreator(store.Dispatch, store.GetState);
        }

        public static IObservable<T> ObserveState<T>(this IStore<T> store)
        {
            return Observable
                .FromEvent(
                    h => store.StateChanged += h,
                    h => store.StateChanged -= h)
                .Select(_ => store.GetState());
        }
    }
}

using Redux;
using System;
using System.Threading.Tasks;

namespace SpotFinder.Redux
{
    public static class StoreExtension
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
    }
}

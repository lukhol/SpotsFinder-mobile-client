using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotFinder.Redux
{
    using ActionType = System.Object;
    public delegate T Reducer<T>(T state, ActionType action);

    public class Store<T>
    {
        Reducer<T> reducer;
        public T State { get; private set; }

        public delegate void SubscribeDelegate(T state);
        public event SubscribeDelegate Subscribe;

        public Store(Reducer<T> reducer, T initialState)
        {
            this.reducer = reducer;
            State = initialState;
        }

        public ActionType Dispatch(ActionType action)
        {
            State = reducer(State, action);
            Subscribe?.Invoke(State);
            return action;
        }
    }
}

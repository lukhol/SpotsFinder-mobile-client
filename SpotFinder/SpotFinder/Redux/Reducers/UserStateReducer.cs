using Redux;
using SpotFinder.Redux.StateModels;
using System;

namespace SpotFinder.Redux.Reducers
{
    public class UserStateReducer : IReducer<UserState>
    {
        public UserState Reduce(UserState previousState, IAction action)
        {
            return previousState;
        }
    }
}

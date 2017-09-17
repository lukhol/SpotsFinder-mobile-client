using System;
using System.Collections.Generic;
using Redux;
using SpotFinder.Redux.StateModels;

namespace SpotFinder.Redux.Reducers
{
    public class NavigationReducer : IReducer<Stack<PageName>>
    {
        public Stack<PageName> Reduce(Stack<PageName> localState, IAction action)
        {
            return localState;
        }
    }
}

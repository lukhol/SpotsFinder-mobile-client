using System;
using System.Collections.Generic;
using Redux;
using SpotFinder.Redux.StateModels;
using SpotFinder.Redux.Actions;

namespace SpotFinder.Redux.Reducers
{
    public class NavigationReducer : IReducer<Stack<PageName>>
    {
        public Stack<PageName> Reduce(Stack<PageName> localState, IAction action)
        {
            if(action is GoBackAction)
            {
                App.Current.MainPage.Navigation.PopAsync();
                return localState;
            }
            return localState;
        }
    }
}

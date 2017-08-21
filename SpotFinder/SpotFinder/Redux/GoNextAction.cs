using System;

namespace SpotFinder.Redux
{
    public class GoNextAction : IAction
    {
        private Type pageType;

        public Type PageType { get => pageType; set { pageType = value; } }

        public GoNextAction(Type pageType)
        {

        }
    }
}

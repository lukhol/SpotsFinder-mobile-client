using System;
using System.Collections.Generic;
using System.Text;

namespace SpotFinder.Config
{
    public interface IBootstrapper
    {
        void OnStart();
        void OnSleep();
        void OnResume();
    }
}

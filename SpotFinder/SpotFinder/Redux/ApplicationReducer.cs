using Redux;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotFinder.Redux
{
    public class ApplicationReducer
    {
        public static ApplicationState Reducer(ApplicationState state, IAction action)
        {
            if(action is InitAction)
            {
                var initAction = (InitAction)action;

            }

            if(action is DownloadSinglePlaceAction)
            {
                var downloadSinglePlaceAction = (DownloadSinglePlaceAction)action;
                state.RequestDownloadPlace(downloadSinglePlaceAction.Id);
            }

            return state;
        }
    }
}

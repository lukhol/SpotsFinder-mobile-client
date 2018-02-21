using System;
using System.Collections.Generic;
using System.Text;

namespace SpotFinder.Redux.Actions.Users
{
    public interface IUserAccessActionCreator
    {
        StoreExtensions.AsyncActionCreator<ApplicationState> CheckTokens();
    }
}

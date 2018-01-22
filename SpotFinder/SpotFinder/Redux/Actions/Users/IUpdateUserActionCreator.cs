using System.Collections.Generic;

namespace SpotFinder.Redux.Actions.Users
{
    public interface IUpdateUserActionCreator
    {
        StoreExtensions.AsyncActionCreator<ApplicationState> UpdateUser(IDictionary<string, string> fields);
    }
}

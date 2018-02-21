using SpotFinder.DataServices;
using SpotFinder.Redux.StateModels;
using SpotFinder.Views;
using System;
using BuilderImmutableObject;
using System.Collections.Generic;
using System.Text;

namespace SpotFinder.Redux.Actions.Users
{
    class UserAccessActionCreator : IUserAccessActionCreator
    {
        private readonly IUserService userService;

        public UserAccessActionCreator(IUserService userService)
        {
            this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        public StoreExtensions.AsyncActionCreator<ApplicationState> CheckTokens()
        {
            return async (dispatch, getState) =>
            {
                User user = getState().UserState.User;
                if (user == null)
                {
                    await App.Current.MainPage.Navigation.PushModalAsync(new LoginPage());
                }

                var isTokenValid = await userService.IsAccessTokenStillValid(user.AccessToken);

                if (isTokenValid)
                    return;

                try
                {
                    var refreshedTokensTuple = await userService.RefreshAccessToken(user);

                    var updatedUser = user
                        .Set(v => v.AccessToken, refreshedTokensTuple.Item1)
                        .Set(v => v.RefreshToken, refreshedTokensTuple.Item2)
                        .Build();

                    dispatch(new SetLoggedInUserAction(updatedUser));
                }
                catch (Exception e)
                {
                    await App.Current.MainPage.Navigation.PushModalAsync(new LoginPage());
                    return;
                }
            };
        }
    }
}

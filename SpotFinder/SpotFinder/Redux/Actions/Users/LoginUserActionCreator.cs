using SpotFinder.DataServices;
using SpotFinder.Redux.StateModels;
using System;
using BuilderImmutableObject;
using SpotFinder.Core.Enums;

namespace SpotFinder.Redux.Actions.Users
{
    public class LoginUserActionCreator : ILoginUserActionCreator
    {
        private readonly IUserService userService;

        public LoginUserActionCreator(IUserService userService)
        {
            this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        public StoreExtensions.AsyncActionCreator<ApplicationState> Login(string email, string password)
        {
            return async (dispatch, getState) =>
            {
                dispatch(new SetLoginStartAction(AccessProvider.SpotsFinderService));

                User loggedInUser;

                try
                {
                    loggedInUser = await userService.LoginAsync(email, password);
                    var tokens = await userService.GetTokensAsync(loggedInUser.Id.ToString(), password);

                    var userWithTokens = loggedInUser
                        .Set(v => v.AccessToken, tokens.Item1)
                        .Set(v => v.RefreshToken, tokens.Item2)
                        .Build();

                    dispatch(new SetLoggedInUserAction(userWithTokens));
                    dispatch(new SetLoginCompleteAction(userWithTokens));
                }
                catch(Exception e)
                {
                    dispatch(new SetLoginCompleteAction(e));
                }
            };
        }
    }
}

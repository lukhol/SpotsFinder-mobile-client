using SpotFinder.DataServices;
using SpotFinder.Redux.StateModels;
using System;
using BuilderImmutableObject;

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
                ///Ustaw na stanie Usera, którym chcemy się zalogować:
                dispatch(new SetLoginStartAction(email, password));

                User loggedInUser;

                try
                {
                    loggedInUser = await userService.LoginAsync(email, password);
                    var tokens = await userService.GetTokensAsync(email, password);

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

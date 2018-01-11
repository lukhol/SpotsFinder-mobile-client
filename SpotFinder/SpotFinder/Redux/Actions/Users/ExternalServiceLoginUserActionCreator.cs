using BuilderImmutableObject;
using SpotFinder.Core.Enums;
using SpotFinder.DataServices;
using SpotFinder.Redux.StateModels;
using SpotFinder.Services;
using System;

namespace SpotFinder.Redux.Actions.Users
{
    public class ExternalServiceLoginUserActionCreator : IExternalServiceLoginUserActionCreator
    {
        private readonly IUserService userService;
        private readonly IExternalServiceUserProvider externalServiceUserProvider;

        public ExternalServiceLoginUserActionCreator(IUserService userService, IExternalServiceUserProvider externalServiceUserProvider)
        {
            this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
            this.externalServiceUserProvider = externalServiceUserProvider ?? throw new ArgumentNullException(nameof(externalServiceUserProvider));
        }

        public StoreExtensions.AsyncActionCreator<ApplicationState> Login(string uri, AccessProvider accessProvider)
        {
            return async (dispatch, getStat) =>
            {
                try
                {
                    dispatch(new SetLoginStartAction(accessProvider));

                    Tuple<User, string> externalServerUserTuple = await externalServiceUserProvider.GetUser(uri, accessProvider);

                    User userFromExternalService = externalServerUserTuple.Item1;
                    string externalAccessToken = externalServerUserTuple.Item2;

                    var loggedInUserWithoutTokens = await userService.LoginOrRegisterAndLoginExternalUserAsync(userFromExternalService, externalAccessToken);
                    var tokens = await userService.GetTokensAsync(loggedInUserWithoutTokens.Id.ToString(), "externaluser");

                    var loggedInUserWithTokens = loggedInUserWithoutTokens
                        .Set(v => v.AccessToken, tokens.Item1)
                        .Set(v => v.RefreshToken, tokens.Item2)
                        .Build();


                    dispatch(new SetLoggedInUserAction(loggedInUserWithTokens));
                    dispatch(new SetLoginCompleteAction(loggedInUserWithTokens));
                }
                catch (Exception e)
                {
                    dispatch(new SetLoginCompleteAction(new Exception("Sorry, something went wrong :(. Try again later.")));
                }
            };
        }
    }
}

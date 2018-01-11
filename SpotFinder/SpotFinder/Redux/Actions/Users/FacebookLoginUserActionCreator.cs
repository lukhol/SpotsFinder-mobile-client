using BuilderImmutableObject;
using SpotFinder.DataServices;
using SpotFinder.Redux.StateModels;
using System;

namespace SpotFinder.Redux.Actions.Users
{
    public class FacebookLoginUserActionCreator : IExternalServiceLoginUserActionCreator
    {
        private readonly IUserService userService;
        private readonly IFacebookService facebookService;

        public FacebookLoginUserActionCreator(IUserService userService, IFacebookService facebookService)
        {
            this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
            this.facebookService = facebookService ?? throw new ArgumentNullException(nameof(facebookService));
        }

        public StoreExtensions.AsyncActionCreator<ApplicationState> Login(string uri)
        {
            return async (dispatch, getStat) =>
            {
                try
                {
                    dispatch(new SetLoginStartAction(string.Empty));

                    var accessToken = ExtractAccessTokenFromUrl(uri);
                    var simpleFacebookDTO = await facebookService.GetSimpleFacebookInfoAsync(accessToken);

                    var userWithFacebookInfo = new User(
                        id: 0,
                        firstname: simpleFacebookDTO.Firstname,
                        lastname: simpleFacebookDTO.Lastname,
                        email: simpleFacebookDTO.Email,
                        facebookId: simpleFacebookDTO.Id,
                        accessToken: null,
                        refreshToken: null
                    );

                    var loggedInUser = await userService.LoginFacebookAsync(userWithFacebookInfo, accessToken);
                    var tokens = await userService.GetTokensAsync(loggedInUser.FacebookId, "facebookUserPassword");

                    var userWithTokens = loggedInUser
                        .Set(v => v.AccessToken, tokens.Item1)
                        .Set(v => v.RefreshToken, tokens.Item2)
                        .Build();

                    dispatch(new SetLoggedInUserAction(userWithTokens));
                    dispatch(new SetLoginCompleteAction(userWithTokens));
                }
                catch (Exception e)
                {
                    dispatch(new SetLoginCompleteAction(new Exception("Sorry, something went wrong :(. Try again later.")));
                }
            };
        }

        private string ExtractAccessTokenFromUrl(string url)
        {
            if (url.Contains("access_token") && url.Contains("&expires_in="))
            {
                var at = url.Remove(0, url.IndexOf("access_token=") + "access_token=".Length);
                var accessToken = at.Remove(at.IndexOf("&expires_in="));
                return accessToken;
            }

            return string.Empty;
        }
    }
}

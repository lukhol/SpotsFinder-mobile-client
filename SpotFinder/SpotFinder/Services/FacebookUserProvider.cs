using SpotFinder.DataServices;
using SpotFinder.Models.DTO;
using SpotFinder.Redux.StateModels;
using System;
using System.Threading.Tasks;

namespace SpotFinder.Services
{
    public class FacebookUserProvider
    {
        private readonly IExternalUserService<SimpleFacebookUserDTO> facebookService;

        public FacebookUserProvider(IExternalUserService<SimpleFacebookUserDTO> facebookService)
        {
            this.facebookService = facebookService ?? throw new ArgumentNullException(nameof(facebookService));
        }

        public async Task<Tuple<User, string>> GetUserAndAccessToken(string uri)
        {
            var accessToken = ExtractAccessTokenFromUrl(uri);
            var simpleFacebookDTO = await facebookService.GetExternalUserInfoAsync(accessToken);

            var userWithFacebookInfo = new User(
                id: 0,
                firstname: simpleFacebookDTO.Firstname,
                lastname: simpleFacebookDTO.Lastname,
                email: simpleFacebookDTO.Email,
                facebookId: simpleFacebookDTO.Id,
                googleId: null,
                accessToken: null,
                refreshToken: null,
                avatarUrl: simpleFacebookDTO.AvatarUrl,
                password: null
            );

            var tupleToReturn = new Tuple<User, string>(userWithFacebookInfo, accessToken);
            return tupleToReturn;
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

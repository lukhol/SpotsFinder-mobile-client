using SpotFinder.DataServices;
using SpotFinder.Models.DTO;
using SpotFinder.Redux.StateModels;
using System;
using System.Threading.Tasks;

namespace SpotFinder.Services
{
    public class GoogleUserProvider
    {
        private readonly IExternalUserService<SimpleGoogleUserDTO> googleService;

        public GoogleUserProvider(IExternalUserService<SimpleGoogleUserDTO> googleService)
        {
            this.googleService = googleService ?? throw new ArgumentNullException(nameof(googleService));
        }

        public async Task<Tuple<User, string>> GetUserAndAccessToken(string uri)
        {
            var code = ExtractCodeFromGoogleReturnUrl(uri);
            var accessToken = await googleService.GetAccessTokenAsync(code);
            var googleUser = await googleService.GetExternalUserInfoAsync(accessToken);

            var userWithGoogleInfo = new User(
                id: 0,
                firstname: googleUser.Firstname,
                lastname: googleUser.Lastname,
                email: googleUser.Email,
                facebookId: null,
                googleId: googleUser.Id,
                accessToken: null,
                refreshToken: null
            );

            var userTupleAndAccessCodeTuple = new Tuple<User, string>(userWithGoogleInfo, accessToken);
            return userTupleAndAccessCodeTuple;
        }

        private string ExtractCodeFromGoogleReturnUrl(string url)
        {
            var codePhrase = "?code=";

            if (!url.Contains(codePhrase))
                throw new ArgumentException("Url is not valid. Google url should contains '?code='.");

            var urlWithoutFirstPart = url.Remove(0, url.IndexOf(codePhrase) + codePhrase.Length);
            var code = urlWithoutFirstPart.Substring(0, urlWithoutFirstPart.IndexOf("&"));

            return code;
        }
    }
}

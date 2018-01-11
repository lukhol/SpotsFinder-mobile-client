using SpotFinder.Core.Enums;
using SpotFinder.Redux.StateModels;
using System;
using System.Threading.Tasks;

namespace SpotFinder.Services
{
    public class ExternalServiceUserProvider : IExternalServiceUserProvider
    {
        private readonly FacebookUserProvider facebookUserProvider;
        private readonly GoogleUserProvider googleUserProvider;

        public ExternalServiceUserProvider(FacebookUserProvider facebookUserProvider, GoogleUserProvider googleUserProvider)
        {
            this.facebookUserProvider = facebookUserProvider ?? throw new ArgumentNullException(nameof(facebookUserProvider));
            this.googleUserProvider = googleUserProvider ?? throw new ArgumentNullException(nameof(googleUserProvider));
        }

        public Task<Tuple<User, string>> GetUser(string uri, AccessProvider accessProvider)
        {
            if (accessProvider == AccessProvider.Facebook)
                return facebookUserProvider.GetUserAndAccessToken(uri);
            else if (accessProvider == AccessProvider.Google)
                return googleUserProvider.GetUserAndAccessToken(uri);

            throw new ArgumentException();
        }
    }
}

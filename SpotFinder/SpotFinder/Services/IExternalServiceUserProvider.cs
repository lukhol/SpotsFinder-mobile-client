using SpotFinder.Core.Enums;
using SpotFinder.Redux.StateModels;
using System;
using System.Threading.Tasks;

namespace SpotFinder.Services
{
    public interface IExternalServiceUserProvider
    {
        Task<Tuple<User, string>> GetUser(string uri, AccessProvider accessProvider);
    }
}

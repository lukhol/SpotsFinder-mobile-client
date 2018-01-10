using SpotFinder.Redux.StateModels;
using System;
using System.Threading.Tasks;

namespace SpotFinder.DataServices
{
    public interface IUserService
    {
        Task<User> LoginFacebookAsync(User userToRegister, string accessToken);
        Task<User> RegisterAsync(User userToRegister);
        Task<User> LoginAsync(string email, string password);
        Task<Tuple<string, string>> GetTokensAsync(string username, string password);
    }
}

using SpotFinder.Redux.StateModels;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SpotFinder.DataServices
{
    public interface IUserService
    {
        Task<User> LoginOrRegisterAndLoginExternalUserAsync(User userToRegister, string accessToken);
        Task<User> RegisterAsync(User userToRegister, string password);
        Task<User> LoginAsync(string email, string password);
        Task<Tuple<string, string>> GetTokensAsync(string username, string password);
        Task<string> SetAvatarAsync(long userId, Stream avatarStream);
        Task<bool> IsEmailFreeAsync(string email);
        Task<User> UpdateUserAsync(User userToUpdate);
        Task<Tuple<string, string>> RefreshAccessToken(User user);
        Task<bool> IsAccessTokenStillValid(String accessToken);
    }
}

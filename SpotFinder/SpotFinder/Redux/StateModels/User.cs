using System;

namespace SpotFinder.Redux.StateModels
{
    public class User
    {
        public long Id { get; private set; }
        public string Firstname { get; private set; }
        public string Lastname { get; private set; }
        public string Email { get; private set; }
        public string FacebookId { get; private set; }

        public string AccessToken { get; private set; }
        public string RefreshToken { get; private set; }

        [Obsolete]
        public User() { }

        public User(
            long id, 
            string firstname,
            string lastname, 
            string email, 
            string facebookId, 
            string accessToken, 
            string refreshToken
            )
        {
            Id = id;
            Firstname = firstname;
            Lastname = lastname;
            Email = email;
            FacebookId = facebookId;
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }
    }
}

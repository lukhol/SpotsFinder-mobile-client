using SpotFinder.Redux.StateModels;
using System;
using System.IO;
using BuilderImmutableObject;
using SpotFinder.DataServices;
using System.Threading.Tasks;

namespace SpotFinder.Redux.Actions.Users
{
    public class RegisterUserActionCreator : IRegisterUserActionCreator
    {
        private readonly IUserService userService;

        public RegisterUserActionCreator(IUserService userService)
        {
            this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        public StoreExtensions.AsyncActionCreator<ApplicationState> Register(User user, string password, Stream avatarStream)
        {
            return async (dispatch, getState) =>
            {
                dispatch(new SetRegisterUserStartAction(user));

                User userToRegister = user;

                if(avatarStream != null && avatarStream.Length > 0)
                {
                    var base64Avatar = StreamToBase64Image(avatarStream);
                    userToRegister = user.Set(v => v.AvatarUrl, base64Avatar).Build();
                }

                try
                {
                    var registeredUser = await userService.RegisterAsync(userToRegister, password);
                    dispatch(new SetRegisterUserCompleteAction(registeredUser));
                }
                catch (Exception ex)
                {
                    dispatch(new SetRegisterUserCompleteAction(ex));
                }
            };
        }

        public string StreamToBase64Image(Stream stream)
        {
            if (stream.Position != 0)
                stream.Position = 0;

            byte[] imageAsByteArray = new byte[(int)stream.Length];
            stream.Read(imageAsByteArray, 0, (int)stream.Length);
            var base64Image = Convert.ToBase64String(imageAsByteArray);
            return base64Image;
        }
    }
}

using SpotFinder.DataServices;
using SpotFinder.Redux.StateModels;
using System;

namespace SpotFinder.Redux.Actions.Users
{
    public class LoginUserActionCreator : ILoginUserActionCreator
    {
        private readonly IUserService userService;

        public LoginUserActionCreator(IUserService userService)
        {
            this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        public StoreExtensions.AsyncActionCreator<ApplicationState> Login(string email, string password)
        {
            return async (dispatch, getState) =>
            {
                ///Ustaw na stanie Usera, którym chcemy się zalogować:
                dispatch(new SetLoginStartAction(email, password));

                User loggedInUser;

                try
                {
                    loggedInUser = await userService.LoginAsync(email, password);

                    dispatch(new SetLoggedInUserAction(loggedInUser));
                    dispatch(new SetLoginCompleteAction(loggedInUser));
                }
                catch(Exception e)
                {
                    dispatch(new SetLoginCompleteAction(e));
                }

            };
        }
    }
}

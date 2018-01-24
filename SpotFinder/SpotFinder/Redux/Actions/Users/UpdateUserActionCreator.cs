using SpotFinder.DataServices;
using SpotFinder.Redux.StateModels;
using System;
using System.Collections.Generic;
using BuilderImmutableObject;
using System.Threading.Tasks;
using SpotFinder.Exceptions;

namespace SpotFinder.Redux.Actions.Users
{
    public class UpdateUserActionCreator : IUpdateUserActionCreator
    {
        private readonly IUserService userService;

        public UpdateUserActionCreator(IUserService userService)
        {
            this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        public StoreExtensions.AsyncActionCreator<ApplicationState> UpdateUser(IDictionary<string, string> fields)
        {
            return async (dispatch, getState) =>
            {
                dispatch(new SetUpdateUserStartAction());

                User actualUser = getState().UserState.User;

                var email = fields["email"];

                bool isEmailFree = false;

                try
                {
                    if (actualUser.Email != null && !actualUser.Email.Equals(email))
                        isEmailFree = await userService.IsEmailFreeAsync(email);
                    else
                        isEmailFree = true;
                }
                catch (Exception e)
                {
                    dispatch(new SetUpdateUserCompleteAction(new EditUserException("Server error. Try again later.", null)));
                }

                if (!isEmailFree)
                {
                    var errorEmailMessage = "Email is occupied. Please choose another one.";
                    dispatch(new SetUpdateUserCompleteAction(new EditUserException(null, errorEmailMessage)));
                    return;
                }

                User user = actualUser
                    .Set(v => v.Email, email)
                    .Set(v => v.Lastname, fields["lastname"])
                    .Set(v => v.Firstname, fields["firstname"])
                    .Build();

                try
                {
                    await userService.UpdateUserAsync(user);
                }
                catch (Exception e)
                {
                    dispatch(new SetUpdateUserCompleteAction(new EditUserException("Server error. Try again later.", null)));
                    return;
                }

                dispatch(new SetUpdateUserCompleteAction(user));
                dispatch(new SetLoggedInUserAction(user));
            };
        }
    }
}

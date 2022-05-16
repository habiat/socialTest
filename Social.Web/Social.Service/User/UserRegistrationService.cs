using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Social.Core;
using Social.Core.ViewModel.Users;
using Social.Service.Authentication;
using Social.Service.Security;

namespace Social.Service.User
{
    public partial class UserRegistrationService : IUserRegistrationService
    {
        #region Fields
        private readonly IUserService _userService;
        private readonly IEncryptionService _encryptionService;
        private readonly IAuthenticationService _authenticationService;
        #endregion

        #region Ctor
        public UserRegistrationService(IUserService userService, IEncryptionService encryptionService, IAuthenticationService authenticationService)
        {
            _userService = userService;
            _encryptionService = encryptionService;
            _authenticationService = authenticationService;
        }

        #endregion


        public async Task<IActionResult> SignInCustomerAsync(Core.Domain.User user, string returnUrl, bool isPersist = false)
        {
            //sign in new user
            await _authenticationService.SignInAsync(user, isPersist);

            //redirect to the return URL if it's specified
            if (!string.IsNullOrEmpty(returnUrl))
                return new RedirectResult(returnUrl);

            return new RedirectToRouteResult("Privacy", null);
        }
        /// <summary>
        /// Validate user
        /// </summary>
        /// <param name="usernameOrEmail">Username or email</param>
        /// <param name="password">Password</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the result
        /// </returns>
        public virtual async Task<UserLoginResults> ValidateUserAsync(string usernameOrEmail, string password)
        {
            var user = CommonHelper.IsValidEmail(usernameOrEmail) ?
                await _userService.GetUserByEmailAsync(usernameOrEmail) :
                await _userService.GetCustomerByMobileAsync(usernameOrEmail);

            if (user == null)
                return UserLoginResults.UserNotExist;
            if (user.Deleted)
                return UserLoginResults.Deleted;
            if (!user.Active)
                return UserLoginResults.NotActive;
            

            if (!PasswordsMatch(user, password))
            {
                return UserLoginResults.WrongPassword;
            }

            return UserLoginResults.Successful;
        }

        /// <summary>
        /// Check whether the entered password matches with a saved one
        /// </summary>
        /// <param name="currentUser">user</param>
        /// <param name="enteredPassword">The entered password</param>
        /// <returns>True if passwords match; otherwise false</returns>
        protected bool PasswordsMatch(Core.Domain.User currentUser, string enteredPassword)
        {
            if (currentUser == null || string.IsNullOrEmpty(enteredPassword))
                return false;

            var savedPassword = string.Empty;
            switch (currentUser.PasswordFormat)
            {
                case PasswordFormat.Clear:
                    savedPassword = enteredPassword;
                    break;
                case PasswordFormat.Encrypted:
                    savedPassword = _encryptionService.EncryptText(enteredPassword);
                    break;
                case PasswordFormat.Hashed:
                    savedPassword = _encryptionService.CreatePasswordHash(enteredPassword, currentUser.PasswordSalt, "SHA1");
                    break;
            }

            if (currentUser.Password == null)
                return false;

            return currentUser.Password.Equals(savedPassword);
        }
    }
}

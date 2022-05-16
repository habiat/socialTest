using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Social.Core.ViewModel.Users;

namespace Social.Service.User
{
    /// <summary>
    /// user registration interface
    /// </summary>
  public  interface IUserRegistrationService
    {
        /// <summary>
        /// Validate user
        /// </summary>
        /// <param name="usernameOrEmail">Username or email</param>
        /// <param name="password">Password</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the result
        /// </returns>
        Task<UserLoginResults> ValidateUserAsync(string usernameOrEmail, string password);
        /// <summary>
        /// Register user
        /// </summary>
        /// <param name="request">Request</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the result
        /// </returns>
        //Task<UserRegistrationResult> RegisterUserAsync(CustomerRegistrationRequest request);



        /// <summary>
        /// Login passed user
        /// </summary>
        /// <param name="user">User to login</param>
        /// <param name="returnUrl">URL to which the user will return after authentication</param>
        /// <param name="isPersist">Is remember me</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the result of an authentication
        /// </returns>
        Task<IActionResult> SignInCustomerAsync(Core.Domain.User user, string returnUrl, bool isPersist = false);

    }
}

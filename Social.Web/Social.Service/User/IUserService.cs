using System;
using System.Threading.Tasks;
using Social.Core.Paging;
using Social.Core.ViewModel;
using Social.Core.ViewModel.Users;

namespace Social.Service.User
{
    public interface IUserService
    {
        /// <summary>
        /// Get customer by username
        /// </summary>
        /// <param name="username">Username</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the customer
        /// </returns>
        Task<Core.Domain.User> GetCustomerByMobileAsync(string username);
        /// <summary>
        /// Get user by email
        /// </summary>
        /// <param name="email">Email</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the customer
        /// </returns>
        Task<Core.Domain.User> GetUserByEmailAsync(string email);
        /// <summary>
        /// register user
        /// </summary>
        /// <param name="model">user information</param>
        /// <returns></returns>
        UserRegisterViewModel Register(UserRegistrationRequest model);
        /// <summary>
        /// get users list by name
        /// </summary>
        /// <param name="name">firstname of user</param>
        /// <param name="pageNo">indicator page number of list</param>
        /// <returns>
        /// list of users with pagable list
        /// </returns>
        PagedResult<UserResponseModel> Search(string name, int pageNo = 1);

        /// <summary>
        /// get user info by guid
        /// </summary>
        /// <param name="id">guid of profile</param>
        /// <param name="currentUserId">userId</param>
        /// <returns>user model</returns>
        UserProfile UserGetByGuid(Guid id, int currentUserId);
        /// <summary>
        /// request to be friend with s/m
        /// </summary>
        /// <param name="userId">current user id</param>
        /// <param name="guidUser">person to be friend with</param>
        void RequestFriendAdd(int userId, Guid guidUser);
        /// <summary>
        /// cancel request friend current user
        /// </summary>
        /// <param name="userId">user id</param>
        /// <param name="guidUser">guid user</param>
        void RequestFriendRemove(int userId, Guid guidUser);
    }
}

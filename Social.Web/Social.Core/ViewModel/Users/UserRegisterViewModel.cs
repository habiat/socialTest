using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Social.Core.Domain;

namespace Social.Core.ViewModel.Users
{
    public class UserRegistrationRequest
    {
        /// <summary>
        /// User
        /// </summary>
        public User User { get; set; }

        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string Email { get; set; }


        /// <summary>
        /// Password
        /// </summary>
        [Required]
        public string Password { get; set; }

        /// <summary>
        /// Password format
        /// </summary>
        public PasswordFormat PasswordFormat { get; set; }

        /// <summary>
        /// Store identifier
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        /// Is approved
        /// </summary>
        public bool IsApproved { get; set; }
    }
    public class UserRegisterViewModel
    {
        public UserRegisterViewModel()
        {
            IsValidEmail = false;
        }
        public RegisterStatus RegisterStatus { get; set; }
        public string Message { get; set; }
        public string Roles { get; set; }
        public bool IsValidEmail { get; set; }
        public int UserId { get; set; }
        public string ReturnUrl { get; set; }
        public string Email { get; set; }
    }
    public enum RegisterStatus
    {
        Unknown = 0,
        Success = 1,
        AlreadyExsist,
        SiteUsers,
        NotActive,
        NotRegistered,
        Failed,
        FoundedUser,
        MobileConfirm = 8,
        NotValidPhone = 9,
        EmailNotValid = 10,


    }
}

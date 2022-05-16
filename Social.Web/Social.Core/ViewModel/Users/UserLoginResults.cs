using System;
using System.Collections.Generic;
using System.Text;

namespace Social.Core.ViewModel.Users
{
    /// <summary>
    /// Represents the user login result enumeration
    /// </summary>
    public enum UserLoginResults
    {
        /// <summary>
        /// Login successful
        /// </summary>
        Successful = 1,

        /// <summary>
        /// user does not exist (email or username)
        /// </summary>
        UserNotExist = 2,

        /// <summary>
        /// Wrong password
        /// </summary>
        WrongPassword = 3,

        /// <summary>
        /// Account have not been activated
        /// </summary>
        NotActive = 4,

        /// <summary>
        /// user has been deleted 
        /// </summary>
        Deleted = 5,

        /// <summary>
        /// user not registered 
        /// </summary>
        NotRegistered = 6,

    }
}

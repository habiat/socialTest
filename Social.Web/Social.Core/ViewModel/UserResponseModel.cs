using System;

namespace Social.Core.ViewModel
{
    public class UserResponseModel
    {
        public long UserId { get; set; }
        public Guid UserGuid { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }
        public DateTime CreatedOnUtc { get; set; }
    }
}

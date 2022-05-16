using Social.Core.ViewModel;

namespace Social.Core.Domain
{
    public class UsersRequest : BaseEntity<int>
    {
        /// <summary>
        /// Gets or sets user has act activity
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// Gets or sets user has wants to be friend
        /// </summary>
        public int RequestUserId { get; set; }
        /// <summary>
        /// indicating state of request
        /// 1=friend/followed ,2=pending ,3=reject ,4=block ,....
        /// </summary>
        public int StateRequest { get; set; }
    }
}

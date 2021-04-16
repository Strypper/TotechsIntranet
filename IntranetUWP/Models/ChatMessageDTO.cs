using System;

namespace IntranetUWP.Models
{
    public class ChatMessageDTO
    {
        public string UserName { get; set; }
        public string UserProfileImage { get; set; }
        public string MessageContent { get; set; }
        public string Color { get; set; }
        public bool IsFromSelf { get; set; }
        public DateTime SentTime { get; set; }
    }
}

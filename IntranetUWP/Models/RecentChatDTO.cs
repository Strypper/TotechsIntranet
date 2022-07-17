using System;

namespace IntranetUWP.Models
{
    public class RecentChatDTO
    {
        public UserDTO TargetUser { get; set; }
        public string MessageContent { get; set; } = String.Empty;
        public DateTime? LastInteractionTime { get; set; }
    }
}

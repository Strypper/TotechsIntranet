using System;

namespace IntranetUWP.Models
{
    public class ChatMessageDTO
    {
        public UserDTO User { get; set; }
        public string MessageContent { get; set; }
        public string Color { get; set; }
        public bool IsFromSelf { get; set; }
        public DateTime SentTime { get; set; }
    }

    public class SendMessageDTO
    {
        public ChatMessageDTO ChatMessage { get; set; }
        public ConversationDTO Conversation { get; set; }
        public UserDTO FromUser { get; set; }
        public UserDTO ToUser { get; set; }
    }
}

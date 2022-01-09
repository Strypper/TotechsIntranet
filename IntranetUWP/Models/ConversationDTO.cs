using System.Collections.Generic;

namespace IntranetUWP.Models
{
    public class ConversationDTO : BaseDTO
    {
        public ICollection<ChatMessageDTO> ChatMessages { get; set; } = new HashSet<ChatMessageDTO>();
        public ICollection<UserDTO> Users { get; set; } = new HashSet<UserDTO>();
    }
}

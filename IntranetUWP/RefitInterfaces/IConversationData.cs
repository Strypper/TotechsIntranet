using IntranetUWP.Models;
using Refit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IntranetUWP.RefitInterfaces
{
    public interface IConversationData
    {
        [Get("/Conversation/GetByUserIdDirectMode/{userId}/{pageIndex}")]
        Task<List<ConversationDTO>> GetByUserIdDirectMode(int userId, int pageIndex);
    }
}

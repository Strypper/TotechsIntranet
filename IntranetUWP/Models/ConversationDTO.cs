using IntranetUWP.RefitInterfaces;
using Microsoft.Toolkit.Collections;
using Refit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace IntranetUWP.Models
{
    public class ConversationDTO : BaseDTO
    {
        public ICollection<ChatMessageDTO>  ChatMessages        { get; set; } = new ObservableCollection<ChatMessageDTO>();
        public ICollection<UserDTO>         Users               { get; set; } = new HashSet<UserDTO>();
        public DateTime                     DateCreated         { get; set; }
        public DateTime                     LastInteractionTime { get; set; }
        public string                       LastMessageContent  { get; set; }
    }

    public class CreateUpdateUserConversationDTO
    {
        public string MessageContent { get; set; }
        public string CurrentUserGuid { get; set; }
        public string TargetUserGuid { get; set; }
    }

    public class ConversationSupportIncrementalLoading : IIncrementalSource<ConversationDTO>
    {
        private readonly IConversationData conversationData = RestService.For<IConversationData>(App.BaseUrl);
        public async Task<IEnumerable<ConversationDTO>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        {
            var result = await conversationData.GetByUserIdDirectMode((int)App.localSettings.Values["UserId"], pageIndex);

            return result;
        }
    }


}

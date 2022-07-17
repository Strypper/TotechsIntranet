using IntranetUWP.ViewModels.PagesViewModel;

namespace IntranetUWP.Models.MixModels
{
    public class ConversationWithViewModel
    {
        public ConversationDTO conversation { get; set; }
        public ChatHubPageViewModel vm { get; set; }
    }
}

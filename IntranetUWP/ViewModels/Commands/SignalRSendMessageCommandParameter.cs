using IntranetUWP.Models;
using IntranetUWP.ViewModels.PagesViewModel;
using System;
using System.Windows.Input;

namespace IntranetUWP.ViewModels.Commands
{
    public class SignalRSendMessageCommandParameter : ICommand
    {
        public ChatHubPageViewModel ViewModel { get; set; }
        public event EventHandler CanExecuteChanged;
        public SignalRSendMessageCommandParameter(ChatHubPageViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
           return parameter == null ? false : true;
        }

        public async void Execute(object parameter)
        {
             var chatMessage = new ChatMessageDTO() { 
                MessageContent = parameter as String, 
                UserName = "Strypper" 
            };
            await ViewModel.SendMessage(chatMessage);
        }
    }
}

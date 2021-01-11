using IntranetUWP.Models;
using IntranetUWP.ViewModels.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace IntranetUWP.ViewModels.PagesViewModel
{
    public class FoodOrderPageViewModel : ViewModelBase
    {
        private string getUserDataUrl = "https://intranetcloudapi.azurewebsites.net/api/User/GetAll";
        private HttpClient httpClient = new HttpClient();
        public ICommand getAllFoodCommand { get; set; }
        public ICommand createFoodCommand { get; set; }
        public ICommand getUserCommand { get; set; }
        public ObservableCollection<UserDTO> Users { get; set; }
        public FoodOrderPageViewModel()
        {
            Users = new ObservableCollection<UserDTO>();
            GetUserData();
            getAllFoodCommand = new RelayCommand(async() => await GetAllFood());
            createFoodCommand = new RelayCommand(async() => await CreateFood());
            getUserCommand = new RelayCommand(async () => await GetUserData());
        }
        private async Task GetAllFood()
        {

        }

        private async Task CreateFood()
        {

        }

        private async Task GetUserData()
        {
            if (Users.Count > 0) Users.Clear();
            var response = await httpClient.GetAsync(getUserDataUrl);
            var result = await response.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<ObservableCollection<UserDTO>>(result);
            foreach (var user in users) { Users.Add(user); await Task.Delay(1000); }
        }
    }
}

using IntranetUWP.Models;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IntranetUWP.ViewModels.PagesViewModel
{
	public class LoginPageViewModel : ViewModelBase
	{
		private readonly HttpClient httpClient;
		private readonly string loginUrl = "https://intranetapi.azurewebsites.net/api/User/Login";
		private bool showError;

		public string Username { get; set; }
		public string Password { get; set; }
		public bool? RemainSignIn { get; set; }
		public bool ShowError { get => showError; private set => Set(ref showError, value); }

		public LoginPageViewModel()
		{
			Username = Password = string.Empty;
			RemainSignIn = true;
			showError = false;
			IsBusy = false;
			httpClient = new HttpClient();
		}

		public async Task LoginAsync()
		{
			IsBusy = true;
			ShowError = false;
			try
			{
				var logininfo = new LoginModel() { userName = Username, password = Password };
				var content = new StringContent(JsonConvert.SerializeObject(logininfo), Encoding.UTF8, "application/json");
				var response = await httpClient.PostAsync(loginUrl, content);
				var result = await response.Content.ReadAsStringAsync();
				_ = response.EnsureSuccessStatusCode();
				if (response.StatusCode == HttpStatusCode.OK)
				{
					var userInfo = JsonConvert.DeserializeObject<UserDTO>(result);
					if (RemainSignIn == true)
					{
						App.localSettings.Values["UserId"] = userInfo.id;
						App.localSettings.Values["UserName"] = userInfo.userName;
						App.localSettings.Values["FirstName"] = userInfo.firstName;
						App.localSettings.Values["LastName"] = userInfo.lastName;
						App.localSettings.Values["Password"] = Password;
						App.localSettings.Values["ProfilePic"] = userInfo.profilePic;
						App.localSettings.Values["Company"] = userInfo.company;
						var foodRequest = await httpClient.GetAsync(GetFoodUrl(userInfo.id));
						var foodResult = await foodRequest.Content.ReadAsStringAsync();
						var foodInfo = JsonConvert.DeserializeObject<UserFoodDTO>(foodResult);
						if (foodInfo != null)
							App.localSettings.Values["FoodId"] = foodInfo.food.id;
					}
				}
				else
				{
					throw new Exception();
				}
			}
			catch
			{
				ShowError = true;
				throw;
			}
			finally
			{
				IsBusy = false;
			}
		}

		private string GetFoodUrl(int id) => $"https://intranetapi.azurewebsites.net/api/UserFood/GetUserSelectedFood/{id}";
	}
}

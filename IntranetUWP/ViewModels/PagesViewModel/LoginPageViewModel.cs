using IntranetUWP.Constanst;
using IntranetUWP.Contracts;
using IntranetUWP.Models;
using IntranetUWP.RefitInterfaces;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Newtonsoft.Json;
using Refit;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IntranetUWP.ViewModels.PagesViewModel
{
	public class LoginPageViewModel : ViewModelBase
	{
		private bool showError;

		public string Username { get; set; }
		public string Password { get; set; }
		public bool? RemainSignIn { get; set; }
		public bool ShowError { get => showError; private set => Set(ref showError, value); }

		private readonly ITotechsIdentity	totechIdentity	  = RestService.For<ITotechsIdentity>(HttpConstants.IdentityBaseUrl);
		private readonly IUserData			userData		  = RestService.For<IUserData>(HttpConstants.BaseUrl);
		private readonly IFoodData			foodData		  = RestService.For<IFoodData>(HttpConstants.BaseUrl);

		public LoginPageViewModel()
		{
			Username = Password = string.Empty;
			RemainSignIn = true;
			showError = false;
			IsBusy = false;
		}

		public async Task<UserIdentityDTO> LoginAsync()
		{
			IsBusy = true;
			ShowError = false;
			try
			{
				var logininfo = new LoginModel() { userName = Username, password = Password };
				var identityInfo = await totechIdentity.Authenticate(logininfo);
				if(identityInfo != null)
                {
                    var userIdentityInfo = identityInfo.UserInfo;
                    if (RemainSignIn == true)
                    {
                        App.localSettings.Values["UserGuid"] = userIdentityInfo.Guid;
                        App.localSettings.Values["Password"] = Password;
                        //var foodInfo = await foodData.Get(UserInfo.id);
                        //if (foodInfo != null)
                        //	App.localSettings.Values["FoodId"] = foodInfo.food.id;
                        return userIdentityInfo;
                    }
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
			return null;
		}

		private string GetFoodUrl(int id) => $"https://intranetapi.azurewebsites.net/api/UserFood/GetUserSelectedFood/{id}";
	}
}

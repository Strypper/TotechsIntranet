using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IntranetUWP.Helpers.HttpHelperV2
{
    public class HttpHelperV2<T> : HttpClient
    {
        public HttpHelperV2()
        {
            this.BaseAddress = new Uri("https://intranetapi.azurewebsites.net/api/");
        }
        public async Task<T> GetDataAsync(string url)
        {
            var response = await base.GetAsync(url);
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(result);
        }

        public async Task<T> GetByIdAsync(string url, int id)
        {
            string finalGetByIdUrl(int entityId) => $"{url}/{entityId}";
            var response = await base.GetAsync(finalGetByIdUrl(id));
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(result);
        }

        public async Task<T> CreateAsync(string url, object o)
        {
            var content = new StringContent(JsonConvert.SerializeObject(o), Encoding.UTF8, "application/json");
            var response = await PostAsync(url, content);
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(result);
        }

        public async Task<bool> UpdateAsync(string url, object o)
        {
            var content = new StringContent(JsonConvert.SerializeObject(o), Encoding.UTF8, "application/json");
            var response = await this.PutAsync(url, content);
            return response.StatusCode == HttpStatusCode.NoContent ? true : false;
        }

        public async Task<bool> RemoveAsync(string url, int id)
        {
            string finalDeleteUrl(int entityId) => $"{url}/{entityId}";
            var response = await DeleteAsync(finalDeleteUrl(id));
            return response.StatusCode == HttpStatusCode.NoContent ? true : false;
        }
    }
}

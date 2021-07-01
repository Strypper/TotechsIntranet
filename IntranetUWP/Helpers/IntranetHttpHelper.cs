using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IntranetUWP.Helpers
{
    public class IntranetHttpHelper : HttpClient
    {
        public IntranetHttpHelper()
        {
            this.BaseAddress = new Uri("https://intranetapi.azurewebsites.net/api/");
            //this.BaseAddress = new Uri("https://localhost:44371/api/");
            //var handler = new HttpClientHandler();
            //handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            //handler.ServerCertificateCustomValidationCallback =
            //    (httpRequestMessage, cert, cetChain, policyErrors) =>
            //    {
            //        return true;
            //    };

            //var httpClient = new HttpClient(handler);
        }

        public async Task<T> GetAsync<T>(string url)
        {
            var response = await GetAsync(url);
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(result);
        }

        public async Task<T> GetByIdAsync<T>(string url, int id)
        {
            string finalGetByIdUrl(int entityId) => $"{url}/{entityId}";
            var response = await GetAsync(finalGetByIdUrl(id));
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(result);
        }

        public async Task<T> CreateAsync<T>(string url, object o)
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
            return response.StatusCode.ToString() == "NoContent" ? true : false;
        }

        public async Task<bool> RemoveAsync(string url, int id)
        {
            string finalDeleteUrl(int entityId) => $"{url}/{entityId}";
            var response = await DeleteAsync(finalDeleteUrl(id));
            return response.StatusCode.ToString() == "NoContent" ? true : false;
        }
    }
}

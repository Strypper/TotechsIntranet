using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IntranetUWP.Helpers
{
    public class IntranetHttpHelper : HttpClient
    {
        private HttpMessageHandler _handler;

        public IntranetHttpHelper(string baseAddress = "https://intranetapi.azurewebsites.net/api/") : this(baseAddress, new HttpClientHandler()) {}

        public IntranetHttpHelper(string baseAddress, HttpMessageHandler handler) : base(handler)
        {
            _handler = handler;

            //BaseAddress = new Uri("https://localhost:5001/api/");
            BaseAddress = new Uri(baseAddress);

            if (BaseAddress.Host.StartsWith("localhost"))
            {
                var clientHandler = handler as HttpClientHandler;
                if (clientHandler != null)
                {
                    clientHandler.ClientCertificateOptions = ClientCertificateOption.Manual;
                    clientHandler.ServerCertificateCustomValidationCallback =
                        (httpRequestMessage, cert, cetChain, policyErrors) =>
                        {
                            return true;
                        };

                }
            }
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

        public async Task<T> CreateAsync<T>(string url,object o)
        {
            var content = new StringContent(JsonConvert.SerializeObject(o), Encoding.UTF8, "application/json");
            var response = await PostAsync(url, content);
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(result);
        }

        public async Task<bool> CreateAsyncWithoutDTO<T>(string url,object o)
        {
            var content = new StringContent(JsonConvert.SerializeObject(o), Encoding.UTF8, "application/json");
            var response = await PostAsync(url, content);
            return response.StatusCode == HttpStatusCode.OK ? true : false;
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

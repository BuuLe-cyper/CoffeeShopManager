using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using CoffeeShop.Helper;

namespace CoffeeShop.Services
{
    public class ApiClientService
    {
        private readonly HttpClient _httpClient;
        public ApiClientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public void SetBearerToken(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<ServiceResponse<T>> GetAsync<T>(string endpoint) where T : class
        {
            var response = await _httpClient.GetAsync(endpoint);
            return await HandleResponse<T>(response);
        }

        public async Task<ServiceResponse<T>> PostAsync<T>(string endpoint, object data) where T : class
        {
            var jsonContent = JsonDeserializeHelper.SerializeObject(data);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(endpoint, content);
            return await HandleResponse<T>(response);
        }

        public async Task<ServiceResponse<string>> PutAsync(string endpoint, object data)
        {
            var jsonContent = JsonDeserializeHelper.SerializeObject(data);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(endpoint, content);
            return await HandleResponse<string>(response);
        }

        public async Task<ServiceResponse<string>> DeleteAsync(string endpoint)
        {
            var response = await _httpClient.DeleteAsync(endpoint);
            return await HandleResponse<string>(response);
        }

        private async Task<ServiceResponse<T>> HandleResponse<T>(HttpResponseMessage response) where T:class
        {
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                if (typeof(T) == typeof(string))
                {
                    return ServiceResponse<T>.Success((T)(object)(string.IsNullOrEmpty(content) ? "Success" : content), (int)response.StatusCode);
                }
                else
                {
                    var data = JsonDeserializeHelper.DeserializeObject<T>(content);
                    return ServiceResponse<T>.Success(data, (int)response.StatusCode);
                }
            }
            else
            {
                return ServiceResponse<T>.Failure(
                    string.IsNullOrEmpty(content) ? response.ReasonPhrase : content,
                    (int)response.StatusCode
                );
            }
        }
    }
}
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MoneyManager.Client.Services
{
    public class HttpException : Exception
    {
        public HttpResponseMessage Response { get; private set; }

        public HttpException(HttpResponseMessage response, string message)
            : base(message)
        {
            Response = response;
        }
    }

    public class HttpAuthException : HttpException
    {

        public HttpAuthException(HttpResponseMessage response, string message)
            : base(response, message)
        {
        }
    }

    public class HttpErrorContent
    {
        public string? Message { get; set; }
    }


    public class HttpClient
    {
        private readonly static System.Net.Http.HttpClient _client = new System.Net.Http.HttpClient();

        private readonly static JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };

        private readonly string _baseUrl;

        public delegate Task<bool> AccessTokenExpiredHandler();
        public event AccessTokenExpiredHandler? AccessTokenExpired;

        public HttpClient(IConfiguration configuration)
        {
            _baseUrl = configuration.GetSection("BaseUrl").Value;
        }
        

        public async Task<T> GetAsync<T>(string resource) =>
            await DoRequestAsync<T>(HttpMethod.Get, resource);

        public async Task<T> PostAsync<T>(string resource, object data) =>
            await DoRequestAsync<T>(HttpMethod.Post, resource, data);

        public async Task<T> PutAsync<T>(string resource, object data) =>
            await DoRequestAsync<T>(HttpMethod.Put, resource, data);

        public async Task<T> DeleteAsync<T>(string resource) =>
            await DoRequestAsync<T>(HttpMethod.Delete, resource);


        public void SetAuthHeader(string token)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }


        private async Task<T> DoRequestAsync<T>(HttpMethod method, string resource, object? data = null, bool isRetry = false)
        {
            using var request = new HttpRequestMessage(method, GetFullUri(resource));

            if (data != null)
            {
                string dataStr = ToJson(data);
                request.Content = GetRequestContent(dataStr);
            }

            var response = await _client.SendAsync(request);
            string responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var content = FromErrorJson(responseContent);
                var message = string.IsNullOrEmpty(content.Message) ? response.ReasonPhrase : content.Message;

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    if (response.Headers.Contains("Token-Expired") && !isRetry)
                    {
                        bool tokenRefreshed = await (AccessTokenExpired?.Invoke() ?? Task.FromResult(false));
                        if (tokenRefreshed)
                        {
                            return await DoRequestAsync<T>(method, resource, data, true);
                        }
                    }
                    throw new HttpAuthException(response, message);
                }
                throw new HttpException(response, message);
            }
            return FromJson<T>(responseContent);
        }

        private Uri GetFullUri(string resource)
        {
            return new Uri(_baseUrl + resource);
        }

        private string ToJson(object data) =>
            JsonSerializer.Serialize(data, _jsonOptions);

        private T FromJson<T>(string data) =>
            JsonSerializer.Deserialize<T>(data, _jsonOptions);

        private HttpErrorContent FromErrorJson(string? data) =>
            string.IsNullOrEmpty(data)
                ? new HttpErrorContent()
                : FromJson<HttpErrorContent>(data);

        private static HttpContent GetRequestContent(string content) =>
            new StringContent(content, Encoding.UTF8, "application/json");
    }
}

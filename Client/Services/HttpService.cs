using System;
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

        private readonly string _baseUrl = "https://localhost:5501/api";



        public async Task<T> Get<T>(string resource) =>
            await DoRequest<T>(HttpMethod.Get, resource);

        public async Task<T> Post<T>(string resource, object data) =>
            await DoRequest<T>(HttpMethod.Post, resource, data);

        public async Task<T> Put<T>(string resource, object data) =>
            await DoRequest<T>(HttpMethod.Put, resource, data);

        public async Task<T> Delete<T>(string resource) =>
            await DoRequest<T>(HttpMethod.Delete, resource);


        public void SetAuthHeader(string token)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        private async Task<T> DoRequest<T>(HttpMethod method, string resource, object? data = null)
        {
            Console.WriteLine("hash: " + _client.GetHashCode());
            var request = new HttpRequestMessage(method, GetFullUri(resource));

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer");

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
                throw new HttpException(response, content.Message ?? response.ReasonPhrase);
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

        private HttpErrorContent FromErrorJson(string data) =>
            FromJson<HttpErrorContent>(data);

        private HttpContent GetRequestContent(string content) =>
            new StringContent(content, Encoding.UTF8, "application/json");
    }
}

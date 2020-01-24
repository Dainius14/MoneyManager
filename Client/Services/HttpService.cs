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

        private readonly string _baseUrl = "https://localhost:5501/api";

        private readonly AuthService _authService;

        public Func<Task<bool>>? OnAuthTokenExpired { get; set; }
        

        public HttpClient(AuthService authService)
        {
            _authService = authService;
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
            Console.WriteLine("SetAuthHeader. Setting header: " + token.Substring(token.LastIndexOf(".") + 1, 5));
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }


        private async Task<T> DoRequestAsync<T>(HttpMethod method, string resource, object? data = null, bool isRetry = false)
        {
            var request = new HttpRequestMessage(method, GetFullUri(resource));

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

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized
                    && response.Headers.Contains("Token-Expired"))
                {
                    Console.WriteLine("DoRequestAsync. Token expired: " + GetHashCode());
                    Console.WriteLine(OnAuthTokenExpired == null);

                    bool refreshedAuthToken = await _authService.RefreshAuthToken();
                    if (refreshedAuthToken)
                    {
                        if (!isRetry)
                        {
                            return await DoRequestAsync<T>(method, resource, data, true);
                        }
                    }
                    else
                    {
                        throw new HttpAuthException(response, message);
                    }
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    Console.WriteLine("DoRequestAsync. Unauthorized");
                    throw new HttpAuthException(response, message);
                }
                Console.WriteLine("DoRequestAsync. Other error: " + message);
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

        private HttpContent GetRequestContent(string content) =>
            new StringContent(content, Encoding.UTF8, "application/json");
    }
}

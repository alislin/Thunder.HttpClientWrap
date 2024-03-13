using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using static System.Net.WebRequestMethods;

namespace Thunder.HttpClientWrap
{
    public class HttpWrap
    {
        private readonly HttpClient _httpClient;
        private INoty logger;
        private string baseUrl;
        private string token;

        public string BaseUrl => _httpClient.BaseAddress.ToString();

        public HttpWrap(HttpClient httpClient, INoty? logger = null)
        {
            _httpClient = httpClient;
            this.logger = logger ?? new Logger();
        }

        public void SetBaseUrl(string baseUrl)
        {
            if (string.IsNullOrWhiteSpace(_httpClient.BaseAddress?.ToString()))
            {
                _httpClient.BaseAddress = new Uri(baseUrl);
            }
            this.baseUrl = _httpClient.BaseAddress.ToString();

        }

        public void SetToken(string token)
        {
            //var authorKey = "login-key";
            var authorKey = "Authorization";
            this.token = token;
            if (_httpClient.DefaultRequestHeaders.Contains(authorKey))
            {
                _httpClient.DefaultRequestHeaders.Remove(authorKey);

            }
            _httpClient.DefaultRequestHeaders.Add(authorKey, "Bearer " + token);
        }

        private async Task<T> ReadContent<T>(HttpResponseMessage? response)
        {
            var d_r = default(T);
            if (response == null)
            {
                logger?.Log("错误", "网络异常");
                return d_r;
            }

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                logger?.Log("错误", $"[StatusCode = {(int)response.StatusCode}] {response.StatusCode.ToString()}");
                return d_r;
            }
            try
            {
                if (typeof(T).Name == typeof(string).Name)
                {
                    return (T)(object)await response.Content.ReadAsStringAsync();
                }

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };
                // 否则，尝试将响应内容反序列化为指定的类型 T
                var json = await response.Content.ReadAsStringAsync();
                var r = JsonSerializer.Deserialize<T>(json, options);
                return r;

                var result = await response.Content.ReadFromJsonAsync<T>();
                return result;
            }
            catch (Exception ex)
            {
                logger?.Error("错误", ex.Message);
            }

            return default(T);
        }

        public async Task<T?> Get<T>(string url)
        {
            var response = await _httpClient.GetAsync(url);
            return await ReadContent<T>(response);
        }

        public async Task<T?> Post<T>(string url, object? data = null)
        {
            var response = await _httpClient.PostAsJsonAsync(url, data ?? new { });
            return await ReadContent<T>(response);
        }

        public async Task<T?> Put<T>(string url, object? data = null)
        {
            var response = await _httpClient.PutAsJsonAsync(url, data ?? new { });
            return await ReadContent<T>(response);
        }

        public async Task<T?> Patch<T>(string url, object? data = null)
        {
            var json = JsonSerializer.Serialize(data ?? new { });
            var response = await _httpClient.PatchAsync(url, new StringContent(json, Encoding.UTF8, "application/json"));
            return await ReadContent<T>(response);
        }

        public async Task<T?> Delete<T>(string url, object? data = null)
        {
            var response = await _httpClient.DeleteAsync(url);
            return await ReadContent<T>(response);
        }
    }
}

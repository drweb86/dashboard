using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Dashboard.Tests.Core
{
    static class HttpClientExtensions
    {
        private static string _token;
        public static void UseTokenForNowOn(this HttpClient client, string token)
        {
            _token = token;
        }

        private static void BeforeRequest(HttpClient client)
        {
            if (_token != null)
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", _token);
        }

        private static async Task<TResponse> ParseResultAsync<TResponse>(HttpResponseMessage response, string url)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
                throw new Exception($"Api '{url}' call failed with error code {response.StatusCode} and message {responseContent}");

            return JsonConvert.DeserializeObject<TResponse>(responseContent);
        }

        public static async Task<TResponse> PostAsync<TBody, TResponse>(this HttpClient client, string url, TBody body)
        {
            BeforeRequest(client);
            var response = await client.PostAsync(url, ToJsonContent(body));
            return await ParseResultAsync<TResponse>(response, url);
        }

        public static async Task<TResponse> PutAsync<TBody, TResponse>(this HttpClient client, string url, TBody body)
        {
            BeforeRequest(client);
            var response = await client.PutAsync(url, ToJsonContent(body));
            return await ParseResultAsync<TResponse>(response, url);
        }

        public static async Task<TResponse> GetAsync<TResponse>(this HttpClient client, string url)
        {
            BeforeRequest(client);
            var response = await client.GetAsync(url);
            return await ParseResultAsync<TResponse>(response, url);
        }

        public static async Task DeleteAsync(this HttpClient client, string url)
        {
            BeforeRequest(client);
            var response = await client.GetAsync(url);
            if (response.StatusCode != System.Net.HttpStatusCode.NoContent)
                throw new Exception($"Api '{url}' call failed with error code {response.StatusCode}");
        }

        private static ByteArrayContent ToJsonContent<TBody>(TBody body)
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(body));
            var jsonContent = new ByteArrayContent(bytes);
            jsonContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return jsonContent;
        }
    }
}

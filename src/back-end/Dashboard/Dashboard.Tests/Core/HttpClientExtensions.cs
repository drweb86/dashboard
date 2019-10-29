using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Dashboard.Tests.Core
{
    static class HttpClientExtensions
    {
        public static async Task<TResponse> PostAsync<TBody, TResponse>(this HttpClient client, string url, TBody body)
        {
            var jsonContent = ToJsonContent(body);

            var response = await client.PostAsync(url, jsonContent);
            var responseContent = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
                throw new Exception($"Api '{url}' call failed with error code {response.StatusCode} and message {responseContent}");

            return JsonConvert.DeserializeObject<TResponse>(responseContent);
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

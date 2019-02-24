using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Ulis.Net.Recognizer.Translation
{
    public class MicrosoftTranslatorClient : ITranslatorClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _subscriptionKey, _sourceLanguage;

        public MicrosoftTranslatorClient(HttpClient httpClient, string subscriptionKey, string sourceLangugage)
        {
            _httpClient = httpClient;
            _subscriptionKey = subscriptionKey;
            _sourceLanguage = sourceLangugage;
        }

        private string Uri => $"https://api.cognitive.microsofttranslator.com/translate?api-version=3.0&to=en&from={_sourceLanguage}";

        public async Task<string> TranslateAsync(string text)
        {
            using (var request = new HttpRequestMessage())
            {
                request.Method = HttpMethod.Post;

                request.RequestUri = new Uri(Uri);

                request.Headers.Add("Ocp-Apim-Subscription-Key", _subscriptionKey);

                var body = new[] { new { Text = text } };
                var requestBody = JsonConvert.SerializeObject(body);
                request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");

                var response = _httpClient.SendAsync(request).Result;
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var definition = new[] { new { Translations = new[] { new { Text = string.Empty } } } };
                var objectResponse = JsonConvert.DeserializeAnonymousType(jsonResponse, definition);

                return objectResponse[0].Translations[0].Text;
            }
        }
    }
}
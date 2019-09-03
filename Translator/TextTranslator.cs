using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using QnABot.Translator.Model;

namespace QnABot.Translator
{
    public class TextTranslator
    {
        private readonly HttpClient _httpClient;
        private readonly TextTranslatorEndpoint _endpoint;

        public TextTranslator(TextTranslatorEndpoint endpoint, HttpClient httpClient)
        {
            _httpClient = httpClient;
            _endpoint = endpoint;
        }

        public async Task<TranslationResult> TranslateTextRequest(ITurnContext<IMessageActivity> turnContext)
        {
            return await this.TranslateTextRequest(turnContext.Activity.Text);
        }

        public async Task<TranslationResult> TranslateTextRequest(string inputText)
        {
            object[] body = new object[] { new { Text = inputText } };
            var requestBody = JsonConvert.SerializeObject(body);
            using (var client = _httpClient)
            using (var request = new HttpRequestMessage())
            {
                request.Method = HttpMethod.Post;
                request.RequestUri = new Uri(_endpoint.endpoint + _endpoint.route);
                request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                request.Headers.Add("Ocp-Apim-Subscription-Key", _endpoint.subscriptionKey);

                HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false);
                string result = await response.Content.ReadAsStringAsync();
                TranslationResult[] deserializedOutput = JsonConvert.DeserializeObject<TranslationResult[]>(result);
                if (deserializedOutput == null || deserializedOutput.Length <= 0)
                {
                    return null;
                }
                return deserializedOutput[0];
            }
        }
    }
}

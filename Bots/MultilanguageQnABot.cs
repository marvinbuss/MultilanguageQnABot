// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.AI.QnA;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using QnABot.Translator;
using QnABot.Translator.Model;

namespace Microsoft.BotBuilderSamples
{
    public class MultilanguageQnABot : ActivityHandler
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<MultilanguageQnABot> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public MultilanguageQnABot(IConfiguration configuration, ILogger<MultilanguageQnABot> logger, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    var welcomeCard = CreateAdaptiveCardAttachment();
                    var response = MessageFactory.Attachment(welcomeCard);
                    await turnContext.SendActivityAsync(response, cancellationToken);
                    await turnContext.SendActivityAsync(MessageFactory.Text($"Hello and welcome {member.Name}!"), cancellationToken);
                }
            }
        }

        private static Attachment CreateAdaptiveCardAttachment()
        {
            string[] paths = { ".", "Cards", "welcomeCard.json" };
            string fullPath = Path.Combine(paths);
            var adaptiveCard = File.ReadAllText(fullPath);
            return new Attachment()
            {
                ContentType = "application/vnd.microsoft.card.adaptive",
                Content = JsonConvert.DeserializeObject(adaptiveCard),
            };
        }

        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Received message {turnContext.Activity.Text}");
            var translationResult = TranslateMessage("en", turnContext.Activity.Text);
            turnContext.Activity.Text = translationResult.Translations[0].Text;

            var qnaResult = FindQnAAsync(turnContext);
            if (String.Equals(translationResult.DetectedLanguage.Language, "en"))
            {
                await turnContext.SendActivityAsync(MessageFactory.Text(qnaResult.Result), cancellationToken);
            }
            else
            {
                var translatedQnaResult = TranslateMessage(translationResult.DetectedLanguage.Language, qnaResult.Result).Translations[0].Text;
                await turnContext.SendActivityAsync(MessageFactory.Text(translatedQnaResult), cancellationToken);
            }
        }

        private TranslationResult TranslateMessage(string toLanguage, string inputText)
        {
            _logger.LogInformation("Calling Text Translator");
            var httpClient = _httpClientFactory.CreateClient();
            var textTranslator = new TextTranslator(new TextTranslatorEndpoint
            {
                subscriptionKey = _configuration["TranslatorTextSubscriptionKey"],
                endpoint = _configuration["TranslatorTextEndpoint"],
                route = _configuration["TranslatorTextRoute"] + toLanguage
            },
            httpClient);

            var translationResult = textTranslator.TranslateTextRequest(inputText);
            return translationResult.Result;
        }

        private async Task<string> FindQnAAsync(ITurnContext<IMessageActivity> turnContext)
        {
            _logger.LogInformation("Calling QnA Maker");
            var httpClient = _httpClientFactory.CreateClient();
            var qnaMaker = new QnAMaker(new QnAMakerEndpoint
            {
                KnowledgeBaseId = _configuration["QnAKnowledgebaseId"],
                EndpointKey = _configuration["QnAAuthKey"],
                Host = _configuration["QnAEndpointHostName"]
            },
            null,
            httpClient);

            var response = await qnaMaker.GetAnswersAsync(turnContext);

            if (!(response != null && response.Length > 0 && response[0].Score > float.Parse(_configuration["QnAThreshold"], CultureInfo.InvariantCulture.NumberFormat)))
            {
                return "I am sorry, but I could not find relevant answers. Could you please rephrase your question?";
            }
            return response[0].Answer;
        }
    }
}

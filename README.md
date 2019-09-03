[![Build Status](https://dev.azure.com/marvinbuss/MultilanguageQnABot/_apis/build/status/qna-bot-poc%20-%20CI?branchName=master)](https://dev.azure.com/marvinbuss/MultilanguageQnABot/_build/latest?definitionId=5&branchName=master)

# Multilanguage QnA Bot

This Bot was created using the [Bot Framework v4](https://dev.botframework.com), an AI based cognitive service, to implement simple Question and Answer conversational patterns, and can answer questions in more than 60 languages. The Bot uses the [QnA Maker](https://www.qnamaker.ai) as well as the [Text Translator API](https://azure.microsoft.com/en-us/services/cognitive-services/translator-text-api/).
The [QnA Maker Service](https://www.qnamaker.ai) enables you to build, train and publish a simple question and answer bot based on FAQ URLs, structured documents or editorial content in minutes. In this sample, I demonstrate how to use the QnA Maker service in combination with the Text Translator to answer questions asked in more than 60 languages.

## Prerequisites

The solution was written in [.NET Core SDK](https://dotnet.microsoft.com/download) version 2.1.
You can check your installed version the following way:

```bash
# determine dotnet version
dotnet --version
```

### Deployment of required resources on Azure

TODO: Add script for automatic deployment of required resources

### Create QnA Knowledge Base

To answer questions from the users a knowledge base must be created using QnA Maker. QnA Maker enables you to power a question and answer service from your semi-structured content. One of the basic requirements in writing your own bot is to seed it with questions and answers. In many cases, the questions and answers already exist in content like FAQ URLs/documents, product manuals, etc. With QnA Maker, users can query your application in a natural, conversational manner. QnA Maker uses machine learning to extract relevant question-answer pairs from your content. It also uses powerful matching and ranking algorithms to provide the best possible match between the user query and the questions.
How this can be done is decribed here:

- [Create a knowledge base using the QnA maker and publish the knowledge base](https://docs.microsoft.com/en-us/azure/cognitive-services/qnamaker/tutorials/create-publish-query-in-portal)

## Run the Multilanguage QnA Bot

### Clone the repository

```bash
git clone https://github.com/marvinbuss/MultilanguageQnABot.git
```

- If you downloaded the repository as zip-File, then please unzip the archive.
- In a terminal, navigate to the project folder.

### Update `appsettings.json`

Your Bot has to connect to the deployed services to function properly. Therefore you have to update the `appsettings.json`.

- `MicrosoftAppId`: For local testing not required.
- `MicrosoftAppPassword`: For local testing not required.
- `QnAKnowledgebaseId`: Base ID of your published Knowledge Base
- `QnAAuthKey`: Authentication Key of your published Knowledge Base
- `QnAEndpointHostName`: Endpoint of your QnA service
- `QnAThreshold`: Threshold/Accuracy of accepted answers (e.g. 0.6 or 0.7)
- `TranslatorTextSubscriptionKey`: Subscription Key of your Text Translator API
- `TranslatorTextEndpoint`: Endpoint of your Text Translator service (e.g. https://api.cognitive.microsofttranslator.com/translate?api-version=3.0)
- `TranslatorTextRoute`: Should not be changed

### Run the Bot
- Run the bot from a terminal or from Visual Studio, choose option A or B.

#### A) From a terminal

```bash
# run the bot
dotnet run
```

#### B) From Visual Studio

- Launch Visual Studio
- File -> Open -> Project/Solution
- Navigate to the project folder.
- Select `MultilanguageQnABot.csproj` file
- Press `F5` to run the project

## Testing the Multilanguage QnA Bot using Bot Framework Emulator

[Bot Framework Emulator](https://github.com/microsoft/botframework-emulator) is a desktop application that allows bot developers to test and debug their bots on localhost or running remotely through a tunnel.
Follow [this](https://github.com/Microsoft/BotFramework-Emulator/releases) link to install the Bot Framework Emulator version 4.3.0 or greater.

### Connect to the bot using Bot Framework Emulator

- Launch Bot Framework Emulator
- File -> Open Bot
- Enter a Bot URL of `http://localhost:3978/api/messages`

## Deploy the bot to Azure

To learn more about deploying a bot to Azure, see [Deploy your bot to Azure](https://aka.ms/azuredeployment) for a complete list of deployment instructions.

## Further reading

- [Bot Framework Documentation](https://docs.botframework.com)
- [Bot Basics](https://docs.microsoft.com/azure/bot-service/bot-builder-basics?view=azure-bot-service-4.0)
- [QnA Maker Documentation](https://docs.microsoft.com/en-us/azure/cognitive-services/qnamaker/overview/overview)
- [Activity processing](https://docs.microsoft.com/en-us/azure/bot-service/bot-builder-concept-activity-processing?view=azure-bot-service-4.0)
- [Azure Bot Service Introduction](https://docs.microsoft.com/azure/bot-service/bot-service-overview-introduction?view=azure-bot-service-4.0)
- [Azure Bot Service Documentation](https://docs.microsoft.com/azure/bot-service/?view=azure-bot-service-4.0)
- [.NET Core CLI tools](https://docs.microsoft.com/en-us/dotnet/core/tools/?tabs=netcore2x)
- [Azure CLI](https://docs.microsoft.com/cli/azure/?view=azure-cli-latest)
- [QnA Maker CLI](https://github.com/Microsoft/botbuilder-tools/tree/master/packages/QnAMaker)
- [Azure Portal](https://portal.azure.com)
- [Language Understanding using LUIS](https://docs.microsoft.com/en-us/azure/cognitive-services/luis/)
- [Channels and Bot Connector Service](https://docs.microsoft.com/en-us/azure/bot-service/bot-concepts?view=azure-bot-service-4.0)

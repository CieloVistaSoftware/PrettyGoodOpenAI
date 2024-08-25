# Azure Open AI Tips
## 08/24/2024
There are tons of chatbot articles on the web for C# projects.
Many preceded OpenAI based chatbot. 

This project zeros in on Azure's OpenAi ChatBot solution and how it's done in August/2024 using a console application.

## Steaming

```csharp
 static async Task Main(string[] args)
        {
            Console.WriteLine("OpenAI Test");
            //await NonStreamingChat();
            await StreamingChat();
        }

```
We want to favor the streaming chat methods.  This is due to the 
desire to have answers returned as soon as possible. The effect is that it appears the chatbot is typing the answer in over a period of time. We don't like synchronous responses because it can look like nothing is happening until the whole
answer is posted back.

## Streaming Chat Logic
As of this writing 08/25/24 this is the nuget package we imported.
```xml
<PackageReference Include="Azure.AI.OpenAI" Version="1.0.0-beta.5" />
```
Here is where it came from: [Nuget's Azure.AI.OpenAI](https://www.nuget.org/packages?q=Azure.AI.OpenAI)

## Using Statements

We must reference both Azure and Azure.AI.OpenAI. 
```csharp
using Azure.AI.OpenAI;
using Azure;
```

## 1) Creating a new OpenAIClient
```csharp
 static async Task StreamingChat()
        {
            OpenAIClient client = new OpenAIClient(
                new Uri(uri),
                new AzureKeyCredential(APIKEY));

```
You need the APIKEY and the URI of your Azure Open AI Service. Both come from your OpenAI service in Azure. 

[OpenAI Service Overview](https://learn.microsoft.com/en-us/azure/ai-services/openai/overview)

### Create your OpenAI service in Azure

To use the APIKey and URL you must create an OpenAI Service in Azure

![OpenAI Service](./images/AzureOpenAI.jpg)

Once the service is  created there will be KEYs and URL values found here under the Develop tab. Both keys work but only one is required.

<img src="./images/AzureKeys.jpg" alt="Azure Keys location" width="600"/> 

## Where do I store my keys and url?
Don't store them in this project as somebody can easily use them. The recommendation is to use the "UserEnvironment" instead. 
## 2. Chat Completion Options

After the chat client is created, its behavior can be altered using 

```csharp

ChatCompletionsOptions options = new ChatCompletionsOptions()
 {
    Messages = { new ChatMessage(ChatRole.System, @"You are an AI assistant that helps people find information.") },
    Temperature = (float)0.7,
    MaxTokens = 800,
    NucleusSamplingFactor = (float)0.95,
    FrequencyPenalty = 0,
    PresencePenalty = 0,
 };          
```
Read about ChatCompletionOptions [here](https://learn.microsoft.com/en-us/javascript/api/@azure/openai/chatcompletions?view=azure-node-preview). 


## Interface with User

In console applications the user input is via the <code>Console.ReadLine()</code> method.

```csharp
while (true)
{
    Console.Write("Chat Prompt:");
    string line = Console.ReadLine()!;
    if (line.Equals("quit", StringComparison.OrdinalIgnoreCase))
    {
        break;
    }
    // This keeps the chat history allowing for "Smart" conversations.
    options.Messages.Add(new ChatMessage(ChatRole.User, line));

    Console.WriteLine("Response:");
    // This async Get routine is the result of the OpenAI model and engine
    Response<ChatCompletions> response =
    await client.GetChatCompletionsAsync(
        deploymentOrModelName: deployment,
        options);

    ChatCompletions completions  = response.Value;
    string fullresponse = completions.Choices[0].Message.Content;
    Console.WriteLine(fullresponse);
    options.Messages.Add(completions.Choices[0].Message);

}
```
[Chat Completions]("https://learn.microsoft.com/en-us/javascript/api/@azure/openai/chatcompletions?view=azure-node-preview")
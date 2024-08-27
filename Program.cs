using Azure.AI.OpenAI;
using Azure;
using System.Diagnostics;

namespace PrettyGoodOpenAPI
{
    public class Program
    {
        public static string AzureKeyCredential => Environment.GetEnvironmentVariable("AzureAPIKEY", EnvironmentVariableTarget.User).ToString();
        public static string AzureDeployment => Environment.GetEnvironmentVariable("AzureDeployment", EnvironmentVariableTarget.User).ToString();
        public static string AzureUri => Environment.GetEnvironmentVariable("AzureUrl", EnvironmentVariableTarget.User).ToString();

        private static async Task Main(string[] args)
        {
            Console.WriteLine("OpenAI Test");
            //await NonStreamingChat();
            await StreamingChat();
        }

        public static async Task StreamingChat()
        {
            OpenAIClient client = new OpenAIClient(
                new Uri(AzureUri),
                new AzureKeyCredential(AzureKeyCredential));
            // Debugger.Break();
            ChatCompletionsOptions options = new ChatCompletionsOptions()
            {
                Messages = { new ChatMessage(ChatRole.System, @"You are an AI assistant that helps people write code.") },
                Temperature = (float)0.7,
                MaxTokens = 800,
                NucleusSamplingFactor = (float)0.95,
                FrequencyPenalty = 0,
                PresencePenalty = 0,
            };

            while (true)
            {
                Console.Write("Chat Prompt:");
                string line = Console.ReadLine()!;
                if (line.Equals("quit", StringComparison.OrdinalIgnoreCase))
                {
                    break;
                }
                options.Messages.Add(new ChatMessage(ChatRole.User, line));

                Console.WriteLine("Response:");
                // The deployment name of the model to query. This is the name of the model you deployed to the Azure portal.
                Response<StreamingChatCompletions> response =
                await client.GetChatCompletionsStreamingAsync(
                    deploymentOrModelName: AzureDeployment,
                    options);

                using StreamingChatCompletions streamingChatCompletions = response.Value;
                string fullresponse = "";
                await foreach (StreamingChatChoice choice in streamingChatCompletions.GetChoicesStreaming())
                {
                    await foreach (ChatMessage message in choice.GetMessageStreaming())
                    {
                        fullresponse += message.Content;
                        Console.Write(message.Content);
                    }
                    Console.WriteLine();
                }
                options.Messages.Add(new ChatMessage(ChatRole.Assistant, fullresponse));
            }
        }

        public static async Task NonStreamingChat()
        {
            OpenAIClient client = new OpenAIClient(
                new Uri(AzureUri),
                new AzureKeyCredential(AzureKeyCredential));

            ChatCompletionsOptions options = new ChatCompletionsOptions()
            {
                Messages = { new ChatMessage(ChatRole.System, @"You are an AI assistant that helps people find information.") },
                Temperature = (float)0.7,
                MaxTokens = 800,
                NucleusSamplingFactor = (float)0.95,
                FrequencyPenalty = 0,
                PresencePenalty = 0,
            };

            while (true)
            {
                Console.Write("Chat Prompt:");
                string line = Console.ReadLine()!;
                if (line.Equals("quit", StringComparison.OrdinalIgnoreCase))
                {
                    break;
                }
                options.Messages.Add(new ChatMessage(ChatRole.User, line));

                Console.WriteLine("Response:");
                Response<ChatCompletions> response =
                await client.GetChatCompletionsAsync(
                    deploymentOrModelName: AzureDeployment,
                    options);

                ChatCompletions completions = response.Value;
                string fullresponse = completions.Choices[0].Message.Content;
                Console.WriteLine(fullresponse);
                options.Messages.Add(completions.Choices[0].Message);
            }
        }
    }
}
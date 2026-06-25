using OpenAI;
using OpenAI.Chat;

namespace dotnet_ai_with_aspnet.Service;

public class ChatService
{
    private readonly OpenAIClient _openAIClient;
    private readonly String _model;

    public ChatService(OpenAIClient openAIClient, IConfiguration configuration)
    {
        _openAIClient = openAIClient;
        _model = configuration["OpenAi:ChatModel"] ?? "deepseek-chat";
    }

    public async Task<string> GetResponseAsync(string prompt)
    {
        var chatClient = _openAIClient.GetChatClient(_model);
        var response = await chatClient.CompleteChatAsync(prompt);

        // Pode retornar nullo por isso ?? "No response for AI"
        return response.Value.Content[^1].Text ?? "No response for AI";
    }

    public async Task<string> GetResponserWithOptionsAsync(string prompt)
    {
        var chatClient = _openAIClient.GetChatClient(_model);

        //primeiro parametro as messages
        var messages = new List<ChatMessage>
        {
            new UserChatMessage(prompt)
        };

        //options
        var options = new ChatCompletionOptions
        {
            Temperature = 0.4f, // Sempre ficar abaixo de 0.8 pra não alucinar
            MaxOutputTokenCount = 200
        };

        var response = await chatClient.CompleteChatAsync(messages, options);

        // Pode retornar nullo por isso ?? "No response for AI"
        return response.Value.Content[^1].Text ?? "No response for AI";
    }

}

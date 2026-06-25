using OpenAI;
using OpenAI.Audio;

namespace dotnet_ai_with_aspnet.Service;

public class TranscriptionService
{
    private readonly OpenAIClient _openAIClient;
    private readonly String _model;

    public TranscriptionService(OpenAIClient openAIClient, IConfiguration configuration)
    {
        _openAIClient = openAIClient;
        _model = configuration["OpenAi:AudioModel"] ?? "whisper-1";
    }

    public async Task<string> TranscribeAudioAsync(IFormFile file)
    {
        var tempFilePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.mp3");

        try
        {
            var stream = new FileStream(tempFilePath, FileMode.Create);
            await file.CopyToAsync(stream);
            stream.Close();

            var options = new AudioTranscriptionOptions
            {
                ResponseFormat = AudioTranscriptionFormat.Text,
                Language = "pt",
                Temperature = 0.0f //Transcreve e envia sem inventar nada, sem alucinacao
            };

            var audioClient = _openAIClient.GetAudioClient(_model);
            var response = await audioClient.TranscribeAudioAsync(tempFilePath, options);

            return response.Value.Text ?? "No transcription generated!";

        }
        catch (Exception ex)
        {
            return $"Error: {ex.Message}";
        }
        finally
        {
            // Apos isso deleta o arquivo para não consumir memoria
            if (File.Exists(tempFilePath))
                File.Delete(tempFilePath);
        }
    }
}

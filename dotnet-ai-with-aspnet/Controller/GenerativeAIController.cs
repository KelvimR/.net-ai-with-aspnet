using dotnet_ai_with_aspnet.Service;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_ai_with_aspnet.Controller
{
    [ApiController]
    public class GenerativeAIController : ControllerBase
    {
        private readonly ChatService _chatService;
        private readonly ImageService _imageService;
        private readonly TranscriptionService _transcriptionService;

        public GenerativeAIController(ChatService chatService, ImageService imageService, TranscriptionService transcriptionService)
        {
            _chatService = chatService;
            _imageService = imageService;
            _transcriptionService = transcriptionService;
        }

        [HttpGet("ask-ai")]
        public async Task<IActionResult> AskAI([FromQuery] string prompt)
        {
            if (string.IsNullOrWhiteSpace(prompt))
                return BadRequest("The 'prompt' parameter is required and cannot be empty.");

            var response = await _chatService.GetResponseAsync(prompt);
            return Ok(response);
        }

        [HttpGet("ask-ai-options")]
        public async Task<IActionResult> AskAIWithOptions([FromQuery] string prompt)
        {
            if (string.IsNullOrWhiteSpace(prompt))
                return BadRequest("The 'prompt' parameter is required and cannot be empty.");

            var response = await _chatService.GetResponserWithOptionsAsync(prompt);
            return Ok(response);
        }

        [HttpGet("Generate-image")]
        public async Task<IActionResult> GenerateImage(
            [FromQuery] string prompt,
            [FromQuery] string quality = "hd",
            [FromQuery] int n = 1,
            [FromQuery] int height = 1024,
            [FromQuery] int width = 1024)
        {
            if (string.IsNullOrWhiteSpace(prompt))
                return BadRequest("The 'prompt' parameter is required and cannot be empty.");

            var response = await _imageService.GenerateImage(prompt, quality, n, height, width);
            return Ok(response);
        }


        [HttpGet("Transcription-audio")]
        public async Task<IActionResult> TranscriptionAI(
            [FromQuery] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded or file is empty!");

            try
            {
                var transcription = await _transcriptionService.TranscribeAudioAsync(file);
                return Ok(transcription);
            }
            catch
            {
                return StatusCode(500, "An error occurred while processing your audio file!");
            }
        }
    }
}

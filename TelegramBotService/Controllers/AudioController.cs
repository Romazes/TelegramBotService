using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;
using TelegramBotService.Services.Interface;

namespace TelegramBotService.Controllers
{
    [Route("api/audio/update")]
    [ApiController]
    public class AudioController : ControllerBase
    {
        private readonly IAudioService _audioService;

        public AudioController(IAudioService audioService)
        {
            _audioService = audioService;
        }

        // GET api/audio/update
        [HttpGet]
        public string Get()
        {
            return "Method GET unuvalable";
        }

        // POST api/audio/update
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Update update)
        {
            if (update == null) return BadRequest();
            await _audioService.GetAudio(update);
            return Ok();
        }
    }
}

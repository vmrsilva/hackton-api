using Hackton.Api.Controllers.Video.Dto;
using Hackton.Domain.Video.Entity;
using Hackton.Domain.Video.Service;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace Hackton.Api.Controllers.Video.Http
{
    [ApiController]
    [Route("[controller]")]
    public class VideoController : Controller
    {
        private readonly IVideoService _videoService;
        public VideoController(IVideoService videoService)
        {
            _videoService = videoService;
        }

        [HttpPost]
        public async Task<IActionResult> UploadVideo([FromForm] CreateVideoDto videoDto, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Arquivo de vídeo inválido.");

            var permittedExtensions = new[] { ".mp4", ".avi" };
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (string.IsNullOrEmpty(extension) || !permittedExtensions.Contains(extension))
                return BadRequest("Tipo de arquivo não permitido. Apenas .mp4 e .avi são aceitos.");

            var videoEntity = videoDto.Adapt<VideoEntity>();

            await _videoService.PostNewVideo(videoEntity, file).ConfigureAwait(false);

            return Ok();
        }

        [HttpGet("{id}/status")]
        public IActionResult GetStatus(string id)
        {
            return Ok();
        }
        [HttpGet("{id}/result")]
        public IActionResult GetResult(string id)
        {
            return Ok();
        }
    }
}

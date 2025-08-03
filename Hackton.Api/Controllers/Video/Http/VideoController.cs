using Hackton.Api.Controllers.Video.Dto;
using Hackton.Domain.Video.Service;
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

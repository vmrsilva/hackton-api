using Hackton.Api.Controllers.Video.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Hackton.Api.Controllers.Video.Http
{
    [ApiController]
    [Route("[controller]")]
    public class VideoController : Controller
    {
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
        [HttpGet("{id}/results")]
        public IActionResult GetResults(string id)
        {
            return Ok();
        }
    }
}

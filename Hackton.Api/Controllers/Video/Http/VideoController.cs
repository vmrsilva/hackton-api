using Hackton.Api.Controllers.Video.Dto;
using Hackton.Api.Response;
using Hackton.Domain.Interfaces.Abstractions.UseCaseAbstraction;
using Hackton.Domain.Video.Entity;
using Hackton.Domain.Video.UseCases.CommandDtos;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace Hackton.Api.Controllers.Video.Http
{
    [ApiController]
    [Route("[controller]")]
    public class VideoController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> UploadVideo([FromServices] IUseCaseCommandHandler<PostNewVideoCommandDto> _videoPostUseCase, [FromForm] CreateVideoDto videoDto, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Arquivo de vídeo inválido.");

            var permittedExtensions = new[] { ".mp4", ".avi" };
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (string.IsNullOrEmpty(extension) || !permittedExtensions.Contains(extension))
                return BadRequest("Tipo de arquivo não permitido. Apenas .mp4 e .avi são aceitos.");

            var videoEntity = videoDto.Adapt<VideoEntity>();

            var commandDto = new PostNewVideoCommandDto
            {
                FileName = file.FileName,
                FileStream = file.OpenReadStream(),
                VideoEntity = videoEntity
            };

            await _videoPostUseCase.Handle(commandDto).ConfigureAwait(false);

            return StatusCode(StatusCodes.Status200OK, new BaseResponseDto<string>
            {
                Data = $"Id do video {videoEntity.Id.ToString()}"
            });
        }

        [HttpGet("{id}/status")]
        public async Task<IActionResult> GetStatus([FromServices] IUseCaseQueryHandler<Guid, VideoEntity> _useCaseGet, Guid id)
        {
            var video = await _useCaseGet.Handle(id).ConfigureAwait(false);

            var result = video.Adapt<ResponseVideoDto>();

            return StatusCode(StatusCodes.Status200OK, new BaseResponseDto<ResponseVideoDto>
            {
                Data = result
            });
        }

        [HttpGet("{id}/result")]
        public IActionResult GetResult(string id)
        {
            return Ok();
        }
    }
}

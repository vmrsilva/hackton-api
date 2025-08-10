using Hackton.Api.Controllers.Video.Dto;
using Hackton.Api.Response;
using Hackton.Domain.Interfaces.Abstractions.UseCaseAbstraction;
using Hackton.Domain.Video.Entity;
using Hackton.Domain.Video.Exceptions;
using Hackton.Domain.Video.UseCases.CommandDtos;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace Hackton.Api.Controllers.Video.Http
{
    [ApiController]
    [Route("api/[controller]")]
    public class VideoController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> UploadVideo([FromServices] IUseCaseCommandHandler<PostNewVideoCommandDto> _videoPostUseCase, [FromForm] CreateVideoDto videoDto, IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return StatusCode(StatusCodes.Status400BadRequest, new BaseResponse
                    {
                        Error = "Arquivo inválido."
                    });

                var permittedExtensions = new[] { ".mp4", ".avi" };
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

                if (string.IsNullOrEmpty(extension) || !permittedExtensions.Contains(extension))
                    return StatusCode(StatusCodes.Status400BadRequest, new BaseResponse
                    {
                        Error = "Tipo de arquivo não permitido. Apenas .mp4 e .avi são aceitos."
                    });

                var videoEntity = videoDto.Adapt<VideoEntity>();

                var fileName = videoEntity.Id.ToString()  + extension;

                var commandDto = new PostNewVideoCommandDto
                {
                    FileName = fileName,
                    FileStream = file.OpenReadStream(),
                    VideoEntity = videoEntity
                };

                await _videoPostUseCase.Handle(commandDto).ConfigureAwait(false);

                return StatusCode(StatusCodes.Status201Created, new BaseResponseDto<string>
                {
                    Data = videoEntity.Id.ToString()
                });
            }
            catch (VideoFilePathEmptyException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new BaseResponse
                {
                    Error = ex.Message
                });
            }
            catch (VideoBrokerMessageFailException ex)
            {

                return StatusCode(StatusCodes.Status400BadRequest, new BaseResponse
                {
                    Error = ex.Message
                });
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
                {
                    Error = "Erro ao processar o vídeo."
                });
            }
        }

        [HttpGet("{id}/status")]
        public async Task<IActionResult> GetStatus([FromServices] IUseCaseQueryHandler<Guid, VideoEntity> _useCaseGet, Guid id)
        {
            try
            {
                var video = await _useCaseGet.Handle(id).ConfigureAwait(false);

                var result = video.Adapt<ResponseVideoDto>();

                return StatusCode(StatusCodes.Status200OK, new BaseResponseDto<ResponseVideoDto>
                {
                    Data = result
                });
            }
            catch (VideoNotFoundException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new BaseResponseDto<ResponseVideoDto>
                {
                    Error = ex.Message
                });
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponseDto<ResponseVideoDto>
                {
                    Error = "Error ao consultar video."
                });
            }
        }

        [HttpGet("{id}/result")]
        public IActionResult GetResult(string id)
        {
            return Ok();
        }
    }
}

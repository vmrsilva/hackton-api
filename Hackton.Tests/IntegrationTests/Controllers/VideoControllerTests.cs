using Hackton.Api.Controllers.Video.Dto;
using Hackton.Api.Response;
using Hackton.Shared.Dto.Video;
using Hackton.Tests.IntegrationTests.Setup;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Text.Json;

namespace Hackton.Tests.IntegrationTests.Controllers
{
    public class VideoControllerTests(HacktonApplicationFactory factory) : BaseIntegrationTest(factory)
    {
        const string routeBase = "api/video";

        [Fact(DisplayName = "Should Get Status Return Video")]
        public async Task ShouldGetStatusReturnVideo()
        {
            var client = factory.CreateClient();

            var videoDb = await _dbContext.Video.FirstOrDefaultAsync();

            var response = await client.GetAsync($"{routeBase}/{videoDb.Id}/status");

            var responseParsed = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<BaseResponseDto<ResponseVideoDto>>(responseParsed,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.Equal(StatusCodes.Status200OK, (int)response.StatusCode);
            Assert.NotNull(result.Data);
            Assert.Equal(videoDb.Id, result.Data.Id);
        }

        [Fact(DisplayName = "Should Get Status Return Video Not Found Exception When It Does Not Exist")]
        public async Task ShouldGetStatusReturnVideoNotFoundExceptionWhenItDoesNotExist()
        {
            var idMock = Guid.NewGuid();
            var client = factory.CreateClient();

            var response = await client.GetAsync($"{routeBase}/{idMock}/status");

            var responseParsed = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<BaseResponseDto<ResponseVideoDto>>(responseParsed,
                     new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.Equal(StatusCodes.Status400BadRequest, (int)response.StatusCode);
            Assert.Equal("Video não encontrado.", result?.Error);
        }

        [Theory(DisplayName = "Should Post New Video With Success")]
        [InlineData("mp4")]
        [InlineData("avi")]
        public async Task ShouldPostNewVideoWithSuccessWhenFileIsValid(string fileExtension)
        {

            var _messageServiceMock = factory.GetMessagingServiceMocked();
            var _uploadServiceMock = factory.GetUploadFileServiceMocked();
            _messageServiceMock.Reset();
            _uploadServiceMock.Reset();

            _uploadServiceMock.Setup(x => x.UploadVideoAsync(It.IsAny<Stream>(), It.IsAny<string>()))
                .ReturnsAsync("https://blob.fake.com/video.mp4");

            _messageServiceMock.Setup(x => x.SendMessage(It.IsAny<string>(), It.IsAny<VideoMessageDto>()))
                .ReturnsAsync(true);

            var client = factory.CreateClient();

            MultipartFormDataContent formData = GeneratePostPayload(10, fileExtension);

            var response = await client.PostAsync($"{routeBase}", formData);

            var responseParsed = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<BaseResponseDto<string>>(responseParsed,
                     new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var videoPostedId = Guid.Parse(result?.Data);

            var videoDb = await _dbContext.Video.AsNoTracking().FirstOrDefaultAsync(v => v.Id == videoPostedId);

            Assert.Equal(StatusCodes.Status201Created, (int)response.StatusCode);
            Assert.NotNull(videoDb);
            _uploadServiceMock.Verify(_uploadServiceMock => _uploadServiceMock.UploadVideoAsync(It.IsAny<Stream>(), It.IsAny<string>()), Times.Once);
            _messageServiceMock.Verify(_messageServiceMock => _messageServiceMock.SendMessage(It.IsAny<string>(), It.IsAny<VideoMessageDto>()), Times.Once);
        }

        [Fact(DisplayName = "Should Post New Video Return BadRequest When File Is Invalid")]
        public async Task ShouldPostNewVideoReturnBadRequestWhenFileIsInvalid()
        {
            //var _messageServiceMock = factory.GetMessagingServiceMocked();
            //var _uploadServiceMock = factory.GetUploadFileServiceMocked();

            //_uploadServiceMock.Setup(x => x.UploadVideoAsync(It.IsAny<Stream>(), It.IsAny<string>()))
            //    .ReturnsAsync("https://blob.fake.com/video.mp4");

            //_messageServiceMock.Setup(x => x.SendMessage(It.IsAny<string>(), It.IsAny<VideoMessageDto>()))
            //    .ReturnsAsync(true);

            var client = factory.CreateClient();

            MultipartFormDataContent formData = GeneratePostPayload(0, "mp4");

            var response = await client.PostAsync($"{routeBase}", formData);

            var responseParsed = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<BaseResponseDto<string>>(responseParsed,
                     new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.Equal(StatusCodes.Status400BadRequest, (int)response.StatusCode);
            Assert.Equal("Arquivo inválido.", result?.Error);
        }

        [InlineData("mov")]
        [InlineData("wmv")]
        [Theory(DisplayName = "Should Post New Video Return BadRequest When File Extension Is Not Expetected")]
        public async Task ShouldPostNewVideoReturnBadRequestWhenFileExtensionIsNotExpetected(string fileExtension)
        {
            var client = factory.CreateClient();

            MultipartFormDataContent formData = GeneratePostPayload(10, fileExtension);

            var response = await client.PostAsync($"{routeBase}", formData);

            var responseParsed = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<BaseResponseDto<string>>(responseParsed,
                     new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.Equal(StatusCodes.Status400BadRequest, (int)response.StatusCode);
            Assert.Equal("Tipo de arquivo não permitido. Apenas .mp4 e .avi são aceitos.", result?.Error);
        }

        [Fact(DisplayName = "Should Post Video Return VideoFilePathEmptyException When File Upload Fail")]
        public async Task ShouldPostVideoReturnVideoFilePathEmptyExceptionWhenFileUploadFail()
        {

            var _messageServiceMock = factory.GetMessagingServiceMocked();
            var _uploadServiceMock = factory.GetUploadFileServiceMocked();
            _messageServiceMock.Reset();
            _uploadServiceMock.Reset();

            _uploadServiceMock.Setup(x => x.UploadVideoAsync(It.IsAny<Stream>(), It.IsAny<string>()))
                .ReturnsAsync(string.Empty);

            _messageServiceMock.Setup(x => x.SendMessage(It.IsAny<string>(), It.IsAny<VideoMessageDto>()))
                .ReturnsAsync(true);

            var client = factory.CreateClient();

            MultipartFormDataContent formData = GeneratePostPayload(10, "mp4");

            var response = await client.PostAsync($"{routeBase}", formData);

            var responseParsed = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<BaseResponseDto<string>>(responseParsed,
                     new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.Equal(StatusCodes.Status400BadRequest, (int)response.StatusCode);
            Assert.Equal("Diretório do video não encontrado.", result.Error);
            _uploadServiceMock.Verify(_uploadServiceMock => _uploadServiceMock.UploadVideoAsync(It.IsAny<Stream>(), It.IsAny<string>()), Times.Once);
            _messageServiceMock.Verify(_messageServiceMock => _messageServiceMock.SendMessage(It.IsAny<string>(), It.IsAny<VideoMessageDto>()), Times.Never);
        }

        [Fact(DisplayName = "Should Post Video Return VideoFilePathEmptyException When File Upload Fail")]
        public async Task ShouldPostVideoReturnVideoBrokerMessageFailExceptionWhenMessageBrokerFail()
        {

            var _messageServiceMock = factory.GetMessagingServiceMocked();
            var _uploadServiceMock = factory.GetUploadFileServiceMocked();
            _messageServiceMock.Reset();
            _uploadServiceMock.Reset();

            _uploadServiceMock.Setup(x => x.UploadVideoAsync(It.IsAny<Stream>(), It.IsAny<string>()))
                .ReturnsAsync("https://blob.fake.com/video.mp4");

            _messageServiceMock.Setup(x => x.SendMessage(It.IsAny<string>(), It.IsAny<VideoMessageDto>()))
                .ReturnsAsync(false);

            var client = factory.CreateClient();

            MultipartFormDataContent formData = GeneratePostPayload(10, "mp4");

            var response = await client.PostAsync($"{routeBase}", formData);

            var responseParsed = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<BaseResponseDto<string>>(responseParsed,
                     new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.Equal(StatusCodes.Status400BadRequest, (int)response.StatusCode);
            Assert.Equal("Falha o enviar video para prcessamento.", result.Error);
            _uploadServiceMock.Verify(_uploadServiceMock => _uploadServiceMock.UploadVideoAsync(It.IsAny<Stream>(), It.IsAny<string>()), Times.Once);
            _messageServiceMock.Verify(_messageServiceMock => _messageServiceMock.SendMessage(It.IsAny<string>(), It.IsAny<VideoMessageDto>()), Times.Once);
        }

        #region Private Methods
        private static MultipartFormDataContent GeneratePostPayload(int fileSize, string fileExtension)
        {
            var fileContent = new StreamContent(new MemoryStream(new byte[fileSize]));
            fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue($"video/{fileExtension}");

            var formData = new MultipartFormDataContent();
            formData.Add(fileContent, "file", $"video.{fileExtension}");

            formData.Add(new StringContent("Meu Título"), "Title");
            formData.Add(new StringContent("Minha Descrição"), "Description");
            return formData;
        }
        #endregion
    }
}

namespace Hackton.Api.Controllers.Video.Dto
{
    public record CreateVideoDto
    {
        public required string Title { get; init; }
        public string Description { get; init; }
    }
}

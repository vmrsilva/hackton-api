namespace Hackton.Shared.Dto.Video
{
    public record VideoMessageDto
    {
        public Guid VideoId { get; set; }
        public string FileUrl { get; set; }
    }
}

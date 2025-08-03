namespace Hackton.Domain.Video.Message
{
    public record VideoMessageDto
    {
        public Guid MyProperty { get; set; }
        public string FileUrl { get; set; }
    }
}

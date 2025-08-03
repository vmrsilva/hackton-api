using Hackton.Domain.Video.Entity;

namespace Hackton.Domain.Video.UseCases.CommandDtos
{
    public class PostNewVideoCommandDto
    {
        public VideoEntity VideoEntity { get; set; }
        public Stream FileStream { get; set; }
        public string FileName { get; set; }
    }
}

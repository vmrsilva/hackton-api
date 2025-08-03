using Hackton.Domain.Enums;

namespace Hackton.Domain.Video.Entity
{
    public class VideoEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public VideoStatusEnum Status { get; set; }
    }
}

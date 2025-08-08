using Hackton.Domain.Base.Entity;
using Hackton.Domain.Enums;

namespace Hackton.Domain.Video.Entity
{
    public class VideoEntity : BaseEntity
    {
        public required string Title { get; set; }
        public string Description { get; set; }
        public string FilePath { get; set; }
        public VideoStatusEnum Status { get; set; }

        public VideoEntity() { }

        public VideoEntity(string title, string description, string filePath, VideoStatusEnum status)
        {
            Title = title;
            Description = description;
            FilePath = filePath;
            Status = status;
        }
    }
}

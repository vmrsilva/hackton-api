using Hackton.Domain.Base.Entity;
using Hackton.Domain.Enums;

namespace Hackton.Domain.Video.Entity
{
    public class VideoEntity : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public VideoStatusEnum Status { get; set; }
    }
}

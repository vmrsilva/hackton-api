using Hackton.Domain.Enums;

namespace Hackton.Api.Controllers.Video.Dto
{
    public record ResponseVideoDto
    {
        public Guid Id { get; init; }
        public required string Title { get; init; }
        public string Description { get; init; }
        public bool Active { get; init; }
        public DateTime CreateAt { get; init; }
        public String Status { get; init; }
    }
}

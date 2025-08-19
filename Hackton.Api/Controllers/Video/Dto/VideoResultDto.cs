namespace Hackton.Api.Controllers.Video.Dto
{
    public class VideoResultDto
    {
        public Guid VideoId { get; set; }

        public IList<ResultItem> Results { get; set; }
        public DateTime ProcessmentDate { get; } = DateTime.UtcNow;
    }

    public class ResultItem
    {
        public TimeSpan Time { get; set; }
        public string Description { get; set; }
    }
}

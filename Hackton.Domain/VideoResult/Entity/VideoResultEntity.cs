using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Hackton.Domain.VideoResult.Entity
{
    public class VideoResultEntity
    {
        [BsonId]
        public ObjectId Id { get; } = ObjectId.GenerateNewId();
        [BsonRepresentation(BsonType.String)]
        public Guid VideoId { get; set; }

        public IList<ResultItem> Results { get; set; }
        public DateTime ProcessmentDate { get; } = DateTime.UtcNow;

        public VideoResultEntity(Guid videoId, IList<ResultItem> result)
        {
            this.VideoId = videoId;
            this.Results = result ?? new List<ResultItem>();
        }
    }

    public class ResultItem
    {
        public TimeSpan Time { get; set; }
        public string Description { get; set; }
    }
}

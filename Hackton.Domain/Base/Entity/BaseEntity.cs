namespace Hackton.Domain.Base.Entity
{
    public class BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public bool Active { get; set; } = false;

        public DateTime CreateAt { get; } = DateTime.UtcNow;

        public void MarkAsDeleted()
        {
            Active = true;
        }
    }
}

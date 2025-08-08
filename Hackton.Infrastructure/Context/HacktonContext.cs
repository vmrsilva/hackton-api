using Hackton.Domain.Video.Entity;
using Microsoft.EntityFrameworkCore;

namespace Hackton.Infrastructure.Context
{
    public class HacktonContext : DbContext
    {
        public DbSet<VideoEntity> Video { get; set; }
        public HacktonContext(DbContextOptions<HacktonContext> dbContextOptions) : base(dbContextOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}

using Hackton.Domain.Video.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackton.Infrastructure.EntitiesConfiguration
{
    public class VideoEntityConfiguration : IEntityTypeConfiguration<VideoEntity>
    {
        void IEntityTypeConfiguration<VideoEntity>.Configure(EntityTypeBuilder<VideoEntity> builder)
        {
            builder.ToTable("Video");

            builder.Property(p => p.Title)
                .HasColumnName("Title")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(p => p.Description)
                .HasColumnName("Description")
                .HasMaxLength(250);

            builder.Property(p => p.FilePath)
                .HasColumnName("FilaPath")
                .IsRequired();

            builder.Property(p => p.Status)
                .HasColumnName("Status")
                .IsRequired();

            builder.Property(p => p.CreateAt)
                .HasColumnName("CreateAt")
                .IsRequired();

        }
    }
}

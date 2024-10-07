using DevMagicMemesWebApi.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace DevMagicMemesWebApi.Dal
{
    public class TagEntityTypeConfiguration : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder
                .HasKey(x => x.Id);

            builder
                .HasMany(x => x.Memes)
                .WithMany(x => x.Tags)
                .UsingEntity<Tag.MemesMap>(
                    x => x
                        .HasOne<Meme>()
                        .WithMany(x => x.TagsMaps)
                        .HasForeignKey(x => x.MemeId)
                        .OnDelete(DeleteBehavior.Cascade),
                    x => x
                        .HasOne<Tag>()
                        .WithMany(x => x.MemesMaps)
                        .HasForeignKey(x => x.TagId)
                        .OnDelete(DeleteBehavior.Cascade))
                .ToTable("TagMemesMap");

            builder
                .Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .HasComment("Data unique identifier.");
        }
    }
}

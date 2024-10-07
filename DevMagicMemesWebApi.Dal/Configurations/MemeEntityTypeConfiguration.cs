using DevMagicMemesWebApi.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace DevMagicMemesWebApi.Dal
{
    public class MemeEntityTypeConfiguration : IEntityTypeConfiguration<Meme>
    {
        public void Configure(EntityTypeBuilder<Meme> builder)
        {
            builder
                .HasKey(x => x.Id);

            builder
                .HasIndex(x => x.Image)
                .IsUnique()
                .HasDatabaseName("Image");

            builder
                .Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .HasComment("Data unique identifier.");
        }
    }
}

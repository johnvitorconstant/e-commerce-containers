﻿using ECC.Catalog.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECC.Catalog.API.Data.Mappings;

public class ProductMapping : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasColumnType("varchar(250)");

        builder.Property(x => x.Description)
            .IsRequired()
            .HasColumnType("varchar(500)");

        builder.Property(x => x.Image)
            .IsRequired()
            .HasColumnType("varchar(250)");

        builder.ToTable("Products");
    }
}
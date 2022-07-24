using ECC.Catalog.API.Models;
using ECC.Core.Data;
using ECC.Core.Messages;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;

namespace ECC.Catalog.API.Data;

public class CatalogContext : DbContext, IUnityOfWork
{
    public CatalogContext(DbContextOptions<CatalogContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }

    public async Task<bool> Commit()
    {
        return await base.SaveChangesAsync() > 0;
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Ignore<ValidationResult>();
        modelBuilder.Ignore<Event>();

        foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(
                     e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
            property.SetColumnType("varchar(1000)");

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogContext).Assembly);
    }
}
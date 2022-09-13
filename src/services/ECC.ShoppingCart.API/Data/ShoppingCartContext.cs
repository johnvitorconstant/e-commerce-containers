using ECC.ShoppingCart.API.Model;
using Microsoft.EntityFrameworkCore;

namespace ECC.ShoppingCart.API.Data;

public class ShoppingCartContext : DbContext
{
    public ShoppingCartContext(DbContextOptions<ShoppingCartContext> options) : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        ChangeTracker.AutoDetectChangesEnabled = false;

    }

    public DbSet<CartClient> CartClients { get; set; }
    public DbSet<CartItem> CartItens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(
                     e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
            property.SetColumnType("varchar(100)");

        modelBuilder.Entity<CartClient>()
            .HasIndex(c => c.ClientId)
            .HasDatabaseName("IDX_Client");

        modelBuilder.Entity<CartClient>()
            .HasMany(c => c.Itens)
            .WithOne(i => i.CartClient)
            .HasForeignKey(c => c.CartId);


        foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;

    }
}
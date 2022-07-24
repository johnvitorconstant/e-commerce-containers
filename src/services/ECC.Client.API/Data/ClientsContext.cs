using System.ComponentModel.DataAnnotations;
using ECC.Client.API.Models;
using ECC.Core.Data;
using Microsoft.EntityFrameworkCore;

namespace ECC.Client.API.Data
{
    public class ClientsContext : DbContext, IUnityOfWork
    {
        public ClientsContext(DbContextOptions<ClientsContext> options): base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ChangeTracker.AutoDetectChangesEnabled = false;
        }

        public DbSet<Models.Client> Clients { get; set; }
        public DbSet<Address> Address { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            

            foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(
                         e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
                property.SetColumnType("varchar(100)");

            foreach (var relationship in modelBuilder.Model.GetEntityTypes()
                         .SelectMany(e => e.GetForeignKeys())) relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ClientsContext).Assembly);
        }
        
        public async Task<bool> Commit()
        {
         return await base.SaveChangesAsync() > 0;
        }
    }
}

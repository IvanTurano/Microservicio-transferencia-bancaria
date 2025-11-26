using Bank.Transfer.Api.Domain.Entities.Transfer;
using Bank.Transfer.API.Application.Database;
using Microsoft.EntityFrameworkCore;

namespace Bank.Transfer.API.Persistence.Database
{
    public class DatabaseService: DbContext, IDatabaseService
    {
        public DatabaseService(DbContextOptions options): base(options)
        {
            
        }

        public DbSet<TransferEntity> Transfer { get; set; }

        public async Task<bool> SaveAsync()
        {
            return await SaveChangesAsync() > 0;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}

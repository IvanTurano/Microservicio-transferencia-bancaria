using Bank.Balance.API.Application.Database;
using Bank.Balance.API.Domain.Entities.Balance;
using Microsoft.EntityFrameworkCore;

namespace Bank.Balance.API.Persistence.Database
{
    public class DatabaseService: DbContext, IDatabaseService
    {
        public DatabaseService(DbContextOptions options): base(options)
        {
            
        }

        public DbSet<BalanceEntity> Balance { get; set; }

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

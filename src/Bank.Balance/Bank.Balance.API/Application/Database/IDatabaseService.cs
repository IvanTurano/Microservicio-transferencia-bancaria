using Bank.Balance.API.Domain.Entities.Balance;
using Microsoft.EntityFrameworkCore;

namespace Bank.Balance.API.Application.Database
{
    public interface IDatabaseService
    {
        DbSet<BalanceEntity> Balance { get; set; }
        Task<bool> SaveAsync();
    }
}

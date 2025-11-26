using Bank.Transaction.API.Domain.Entities.Transaction;
using Microsoft.EntityFrameworkCore;

namespace Bank.Transaction.API.Application.Database
{
    public interface IDatabaseService
    {
        DbSet<TransactionEntity> Transaction { get; set; }
        Task<bool> SaveAsync();
    }
}

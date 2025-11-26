using Bank.Transfer.Api.Domain.Entities.Transfer;
using Microsoft.EntityFrameworkCore;

namespace Bank.Transfer.API.Application.Database
{
    public interface IDatabaseService
    {
        DbSet<TransferEntity> Transfer { get; set; }
        Task<bool> SaveAsync();
    }
}

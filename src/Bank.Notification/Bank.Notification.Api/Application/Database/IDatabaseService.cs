
using Bank.Notification.Api.Domain.Entities.Notification;

namespace Bank.Notification.API.Application.Database
{
    public interface IDatabaseService
    {
        Task<bool> AddAsync(NotificationEntity entity);
        Task<List<NotificationEntity>> GetAllAsync();
    }
}

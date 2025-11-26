using Bank.Notification.Api.Domain.Entities.Notification;
using Bank.Notification.API.Application.Database;
using Microsoft.Azure.Cosmos;

namespace Bank.Notification.API.Persistence.Database
{
    public class DatabaseService: IDatabaseService
    {
        private readonly CosmosClient cosmosClient;
        private readonly Container container;

        public DatabaseService(IConfiguration configuration)
        {
            string connectionString = configuration["NOTIFICATIONDBCONSTR"];
            string dataBase = configuration["NOTIFICATIONDBNAME"];
            string containerName = configuration["NOTIFICATIONDBCONTAINER"];

            cosmosClient = new CosmosClient(connectionString);
            container = cosmosClient.GetContainer(dataBase, containerName);
        }

        public async Task<bool> AddAsync(NotificationEntity entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            entity.NotificationDate = DateTime.UtcNow;

            var response = await container.CreateItemAsync(entity, new PartitionKey(entity.CorrelationId));

            if(response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                return true;
            }

            return false;
        }

        public async Task<List<NotificationEntity>> GetAllAsync()
        {
            var query = container.GetItemQueryIterator<NotificationEntity>("SELECT * FROM c");
            var list = new List<NotificationEntity>();

            while(query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                list.AddRange(response);
            }

            return list;
        }
    }
}

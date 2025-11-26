using Bank.Transaction.Api.Application.External.ServiceBusSender;
using Bank.Transaction.API.Application.Database;
using Bank.Transaction.API.Domain.Constants;
using Bank.Transaction.API.Domain.Entities.Transaction;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Bank.Transaction.API.Application.Features.Process
{
    public class ProcessService : IProcessService
    {

        private readonly IDatabaseService _databaseService;
        private readonly IServiceBusSenderService _serviceBusSenderService;

        public ProcessService(IDatabaseService databaseService, IServiceBusSenderService serviceBusSenderService)
        {
            _databaseService = databaseService;
            _serviceBusSenderService = serviceBusSenderService;
        }

        public async Task Execute(string message, string subscription)
        {
            switch (subscription)
            {
                case ReceivedSubscriptionsConstants.TRANSACTION_INITIATED:
                    await TransactionInitiated(message);
                    break;
                case ReceivedSubscriptionsConstants.BALANCE_CONFIRMED:
                    await BalanceConfirmed(message);
                    break;

                case ReceivedSubscriptionsConstants.BALANCE_FAILED:
                    await BalanceFailed(message);
                    break;

                case ReceivedSubscriptionsConstants.TRANSFER_CONFIRMED:
                    await TransferConfirmed(message);
                    break;

                case ReceivedSubscriptionsConstants.TRANSFER_FAILED:
                    await TransferFailed(message);
                    break;
            }
        }

        private async Task TransactionInitiated(string message)
        {
            var entity = JsonConvert.DeserializeObject<TransactionEntity>(message);
            entity.CurrentState = CurrentStateConstants.PENDING;
            var saveEntity = await ProcessDatabase(entity);

            var eventModel = new { saveEntity.CorrelationId, saveEntity.CustomerId };

            if (saveEntity.Id != 0)
            {
                await _serviceBusSenderService.Execute(eventModel, SendSubscriptionConstants.BALANCE_INITIATED);
            }
            else
            {
                await _serviceBusSenderService.Execute(eventModel, SendSubscriptionConstants.TRANSACTION_FAILED);
            }
        }

        private async Task BalanceConfirmed(string message)
        {
            var entity = JsonConvert.DeserializeObject<TransactionEntity>(message);
            entity.CurrentState = CurrentStateConstants.PENDING;
            var saveEntity = await ProcessDatabase(entity);

            var eventModel = new
            {
                saveEntity.CorrelationId,
                saveEntity.Amount,
                saveEntity.SourceAccount,
                saveEntity.DestinationAccount,
                saveEntity.CustomerId
            };

            await _serviceBusSenderService.Execute(eventModel, SendSubscriptionConstants.TRANSFER_INITIATED);
        }

        private async Task BalanceFailed(string message)
        {
            var entity = JsonConvert.DeserializeObject<TransactionEntity>(message);
            entity.CurrentState = CurrentStateConstants.CANCELED;
            var saveEntity = await ProcessDatabase(entity);

            var eventModel = new
            {
                entity.CorrelationId,
                entity.Amount,
                entity.CustomerId
            };


            //MICROSERVICIO NOTIFICATION
            await _serviceBusSenderService.Execute(eventModel, SendSubscriptionConstants.TRANSACTION_FAILED);

        }

        private async Task TransferFailed(string message)
        {
            var entity = JsonConvert.DeserializeObject<TransactionEntity>(message);
            entity.CurrentState = CurrentStateConstants.CANCELED;
            var saveEntity = await ProcessDatabase(entity);

            var eventModel = new
            {
                entity.CorrelationId,
                entity.Amount,
                entity.CustomerId
            };

            //MICROSERVICIO NOTIFICATION
            await _serviceBusSenderService.Execute(eventModel, SendSubscriptionConstants.TRANSACTION_FAILED);

            //MICROSERVICIO BALANCE
            await _serviceBusSenderService.Execute(eventModel, SendSubscriptionConstants.TRANSFER_FAILED_BALANCE);
        }

        private async Task TransferConfirmed(string message)
        {
            var entity = JsonConvert.DeserializeObject<TransactionEntity>(message);
            entity.CurrentState = CurrentStateConstants.COMPLETED;
            var saveEntity = await ProcessDatabase(entity);

            var eventModel = new
            {
                entity.CorrelationId,
                entity.Amount,
                entity.CustomerId
            };

            //MICROSERVICIO NOTIFICATION
            await _serviceBusSenderService.Execute(eventModel, SendSubscriptionConstants.TRANSACTION_COMPLETED);

            //MICROSERVICIO BALANCE
            await _serviceBusSenderService.Execute(eventModel, SendSubscriptionConstants.TRANSFER_CONFIRMED_BALANCE);
        }

        public async Task<TransactionEntity> ProcessDatabase(TransactionEntity entity)
        {
            var existEntity = await _databaseService.Transaction
                .FirstOrDefaultAsync(x => x.CorrelationId == entity.CorrelationId);

            if(existEntity == null)
            {
                entity.TransactionDate = DateTime.UtcNow;
                await _databaseService.Transaction.AddAsync(entity);
                await _databaseService.SaveAsync();
                return entity;
            }
            else
            {
                existEntity.TransactionDate = DateTime.UtcNow;
                existEntity.CurrentState = entity.CurrentState;
                _databaseService.Transaction.Update(existEntity);
                await _databaseService.SaveAsync();
                return existEntity;
            }

        }
    }
}

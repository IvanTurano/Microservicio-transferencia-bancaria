using Bank.Balance.Api.Application.External.ServiceBusSender;
using Bank.Balance.API.Application.Database;
using Bank.Balance.API.Domain.Constants;
using Bank.Balance.API.Domain.Entities.Balance;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Bank.Balance.API.Application.Features.Process
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
                case ReceivedSubscriptionsConstants.BALANCE_INITIATED:
                    await BalanceInitiated(message);
                    break;
                case ReceivedSubscriptionsConstants.TRANSFER_CONFIRMED_BALANCE:
                    await TransferConfirmedBalance(message);
                    break;
                case ReceivedSubscriptionsConstants.TRANSFER_FAILED_BALANCE:
                    await TransferFailedBalance(message);
                    break;
            }
        }


        private async Task BalanceInitiated(string message)
        {
            var entity = JsonConvert.DeserializeObject<BalanceEntity>(message);
            entity.CurrentState = CurrentStateConstants.PENDING;
            var saveEntity = await ProcessDatabase(entity);


            var eventModel = new
            {
                entity.CorrelationId,
                entity.CustomerId
            };

            if (saveEntity.Id != 0)
            {
                //Microservicio Transaction
                await _serviceBusSenderService.Execute(eventModel, SendSubscriptionConstants.BALANCE_CONFIRMED);
            }
            else
            {
                //Microservicio Transaction
                await _serviceBusSenderService.Execute(eventModel, SendSubscriptionConstants.BALANCE_FAILED);

            }
        }

        private async Task TransferConfirmedBalance(string message)
        {
            var entity = JsonConvert.DeserializeObject<BalanceEntity>(message);
            entity.CurrentState = CurrentStateConstants.COMPLETED;
            await ProcessDatabase(entity);
        }

        private async Task TransferFailedBalance(string message)
        {
            var entity = JsonConvert.DeserializeObject<BalanceEntity>(message);
            entity.CurrentState = CurrentStateConstants.CANCELED;
            await ProcessDatabase(entity);
        }

        public async Task<BalanceEntity> ProcessDatabase(BalanceEntity entity)
        {
            var existEntity = await _databaseService.Balance
                .FirstOrDefaultAsync(x => x.CorrelationId == entity.CorrelationId);

            if (existEntity == null)
            {
                entity.BalanceDate = DateTime.UtcNow;
                await _databaseService.Balance.AddAsync(entity);
                await _databaseService.SaveAsync();
                return entity;
            }
            else
            {
                existEntity.BalanceDate = DateTime.UtcNow;
                existEntity.CurrentState = entity.CurrentState;
                _databaseService.Balance.Update(existEntity);
                await _databaseService.SaveAsync();
                return existEntity;
            }

        }


    }
}

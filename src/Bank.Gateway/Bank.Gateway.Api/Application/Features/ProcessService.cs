using Bank.Gateway.Api.Application.External.ServiceBusSender;
using Bank.Gateway.Api.Application.Models;
using Bank.Gateway.Api.Domain.Constants;

namespace Bank.Gateway.Api.Application.Features
{
    public class ProcessService : IProcessService
    {
        private readonly IServiceBusSenderService _serviceBusSender;

        public ProcessService(IServiceBusSenderService serviceBusSender)
        {
            _serviceBusSender = serviceBusSender;
        }

        public async Task Execute(EndPointModel model)
        {

            var modelEvent = new
            {
                CorrelationId = Guid.NewGuid().ToString(),
                Amount = model.Amount,
                SourceAccount = model.SourceAccount,
                DestinationAccount = model.DestinationAccount,
                CustomerId = model.CustomerId
            };

            await _serviceBusSender.Execute(modelEvent, SendSubscriptionConstants.TRANSACTION_INITIATED);
        }
    }
}

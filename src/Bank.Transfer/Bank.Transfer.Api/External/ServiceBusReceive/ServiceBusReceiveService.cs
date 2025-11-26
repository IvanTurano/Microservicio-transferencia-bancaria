using Azure.Messaging.ServiceBus;
using Bank.Transfer.Api.Domain.Constants;
using Bank.Transfer.API.Domain.Events;
using MediatR;

namespace Bank.Transfer.API.External.ServiceBusReceive
{
    public class ServiceBusReceiveService : BackgroundService
    {
        private readonly ServiceBusClient _client;
        private readonly List<ServiceBusProcessor> _processors;
        private readonly IMediator _mediator;

        public ServiceBusReceiveService(IConfiguration configuration, IMediator mediator)
        {
            _mediator = mediator;
            _client = new ServiceBusClient(configuration["SERVICEBUSCONSTR"]);
            var subscriptions = new[]
            {
                ReceivedSubscriptionsConstants.TRANSFER_INITIATED,

            };

            _processors = subscriptions.Select(subscription =>
            {
                var processor = _client.CreateProcessor(configuration["SERVICEBUSTOPIC"], subscription);
                processor.ProcessMessageAsync += async args => await Process(args, subscription);
                processor.ProcessErrorAsync += ProcessError;

                return processor;
            }).ToList();
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.WhenAll(_processors.Select(p => p.StartProcessingAsync()));
            await Task.Run( () => stoppingToken.WaitHandle.WaitOne(), stoppingToken);
        }

        private async Task Process(ProcessMessageEventArgs args, string subscription)
        {
            string body = args.Message.Body.ToString();

            await _mediator.Publish(new ProcessEvent(body, subscription));
            await args.CompleteMessageAsync(args.Message);
        }

        private Task ProcessError(ProcessErrorEventArgs args)
        {
            return Task.CompletedTask;
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.WhenAll(_processors.Select(p => p.StopProcessingAsync()));
            await base.StopAsync(cancellationToken);
        }

        public override async void Dispose()
        {
            await Task.WhenAll(_processors.Select(p => p.DisposeAsync().AsTask()));
            await _client.DisposeAsync();
            base.Dispose();
        }

        
    }
}

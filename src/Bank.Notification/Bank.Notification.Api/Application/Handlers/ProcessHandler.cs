
using Bank.Notification.Api.Application.Features.Process;
using Bank.Notification.API.Domain.Events;
using MediatR;

namespace Bank.Notification.API.Application.Handlers
{
    public class ProcessHandler : INotificationHandler<ProcessEvent>
    {
        private readonly IServiceProvider _serviceProvider;
        public ProcessHandler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public async Task Handle(ProcessEvent eventModel, CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var processService = scope.ServiceProvider.GetRequiredService<IProcessService>();

            await processService.Execute(eventModel.Message, eventModel.Subscription);
        }
    }
}

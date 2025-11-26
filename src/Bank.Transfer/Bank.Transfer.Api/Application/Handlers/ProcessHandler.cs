
using Bank.Transfer.Api.Application.Features.Process;
using Bank.Transfer.API.Domain.Events;
using MediatR;

namespace Bank.Transfer.API.Application.Handlers
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

            await processService.Execute(eventModel.Message);
        }
    }
}

namespace Bank.Transaction.Api.Application.External.ServiceBusSender
{
    public interface IServiceBusSenderService
    {
        Task Execute(object eventModel, string subscription);
    }
}

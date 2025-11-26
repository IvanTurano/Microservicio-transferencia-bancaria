namespace Bank.Transaction.API.Application.Features.Process
{
    public interface IProcessService
    {
        Task Execute(string message, string subscription);
    }
}

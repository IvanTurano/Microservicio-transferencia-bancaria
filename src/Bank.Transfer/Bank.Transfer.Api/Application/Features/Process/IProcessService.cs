namespace Bank.Transfer.Api.Application.Features.Process
{
    public interface IProcessService
    {
        Task Execute(string message);
    }
}

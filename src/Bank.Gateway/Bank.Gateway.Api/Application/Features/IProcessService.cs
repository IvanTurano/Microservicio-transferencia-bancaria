using Bank.Gateway.Api.Application.Models;

namespace Bank.Gateway.Api.Application.Features
{
    public interface IProcessService
    {
        Task Execute(EndPointModel model);
    }
}

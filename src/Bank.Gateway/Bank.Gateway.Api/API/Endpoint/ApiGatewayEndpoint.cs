using Bank.Gateway.Api.Application.Features;
using Bank.Gateway.Api.Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Gateway.Api.API.Endpoint
{
    public static class ApiGatewayEndpoint
    {
        public static void GatewayEndpoint(WebApplication app)
        {
            app.MapPost("/api-gateway", async ([FromBody] EndPointModel modelRequest,
                [FromServices] IProcessService processService) =>
            {
                await processService.Execute(modelRequest);
                return modelRequest;
            });
        }
    }
}

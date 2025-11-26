using Bank.Gateway.Api.API.Endpoint;
using Bank.Gateway.Api.Application.External.ServiceBusSender;
using Bank.Gateway.Api.Application.Features;
using Bank.Gateway.Api.External.ServiceBusSender;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IServiceBusSenderService, ServiceBusSenderService>();
builder.Services.AddSingleton<IProcessService, ProcessService>();

var app = builder.Build();

ApiGatewayEndpoint.GatewayEndpoint(app);

app.Run();


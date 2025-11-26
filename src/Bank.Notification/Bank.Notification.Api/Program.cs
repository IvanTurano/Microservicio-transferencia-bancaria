using Bank.Notification.Api.Application.External.SendGridEmail;
using Bank.Notification.Api.Application.Features.Process;
using Bank.Notification.Api.External.SendGridEmail;
using Bank.Notification.API.Application.Database;
using Bank.Notification.API.Application.Handlers;
using Bank.Notification.API.External.ServiceBusReceive;
using Bank.Notification.API.Persistence.Database;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddScoped<IDatabaseService, DatabaseService>();
builder.Services.AddScoped<IProcessService, ProcessService>();

builder.Services.AddSingleton<ISendGridEmailService, SendGridEmailService>();

builder.Services.AddHostedService<ServiceBusReceiveService>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(ProcessHandler).Assembly));


var app = builder.Build();

app.Run();

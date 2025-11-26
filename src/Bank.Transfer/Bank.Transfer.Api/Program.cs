using Bank.Transfer.Api.Application.External.ServiceBusSender;
using Bank.Transfer.Api.Application.Features.Process;
using Bank.Transfer.Api.External.ServiceBusSender;
using Bank.Transfer.API.Application.Database;
using Bank.Transfer.API.Application.Handlers;
using Bank.Transfer.API.External.ServiceBusReceive;
using Bank.Transfer.API.Persistence.Database;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<DatabaseService>(options =>
    options.UseSqlServer(builder.Configuration["TRANSFERSQLDBCONSTR"]));

builder.Services.AddScoped<IDatabaseService,DatabaseService>();
builder.Services.AddScoped<IProcessService, ProcessService>();
builder.Services.AddSingleton<IServiceBusSenderService, ServiceBusSenderService>();

builder.Services.AddHostedService<ServiceBusReceiveService>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(ProcessHandler).Assembly));


var app = builder.Build();

app.Run();

using Bank.Transaction.Api.Application.External.ServiceBusSender;
using Bank.Transaction.Api.External.ServiceBusSender;
using Bank.Transaction.API.Application.Database;
using Bank.Transaction.API.Application.Features.Process;
using Bank.Transaction.API.Application.Handlers;
using Bank.Transaction.API.External.ServiceBusReceive;
using Bank.Transaction.API.Persistence.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<DatabaseService>(options =>
{
    options.UseSqlServer(builder.Configuration["TRANSACTIONSQLDBCONSTR"]);
});
builder.Services.AddScoped<IDatabaseService,DatabaseService>();
builder.Services.AddScoped<IProcessService, ProcessService>();
builder.Services.AddSingleton<IServiceBusSenderService, ServiceBusSenderService>();


builder.Services.AddHostedService<ServiceBusReceiveService>();


builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(ProcessHandler).Assembly));

var app = builder.Build();

app.Run();

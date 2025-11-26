using Bank.Balance.Api.Application.External.ServiceBusSender;
using Bank.Balance.Api.External.ServiceBusSender;
using Bank.Balance.API.Application.Database;
using Bank.Balance.API.Application.Features.Process;
using Bank.Balance.API.Application.Handlers;
using Bank.Balance.API.External.ServiceBusReceive;
using Bank.Balance.API.Persistence.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddDbContext<DatabaseService>(options =>
options.UseSqlServer(builder.Configuration["BALANCESQLDBCONSTR"]));

builder.Services.AddScoped<IDatabaseService,DatabaseService>();
builder.Services.AddScoped<IProcessService, ProcessService>();
builder.Services.AddSingleton<IServiceBusSenderService, ServiceBusSenderService>();

builder.Services.AddHostedService<ServiceBusReceiveService>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(ProcessHandler).Assembly));

var app = builder.Build();

app.Run();


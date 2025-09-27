using LoanApplication.Api.Middleware;
using LoanApplication.Application.CommandHandlers;
using LoanApplication.Application.Persistence;
using LoanApplication.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(SubmitLoanApplicationCommandHandler).Assembly)); 
builder.Services.AddScoped<ILoanApplicationRepository, LoanApplicationRepository>();

builder.Logging.SetMinimumLevel(LogLevel.Information);

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseMiddleware<ExceptionMiddleware>(); // Add our exception handler

app.UseAuthorization();

app.MapControllers();

app.Run();

using Hangfire;
using HangFireApp.Context;
using HangFireApp.Controllers;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string connectionString = builder.Configuration.GetConnectionString("SqlServer");

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

builder.Services.AddHangfire(config =>
{
    config.UseSqlServerStorage(connectionString);
});
builder.Services.AddHangfireServer();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHangfireDashboard();

RecurringJob.AddOrUpdate("test-job", () => BackgroundTestServices.Test(), Cron.MinuteInterval(19));


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

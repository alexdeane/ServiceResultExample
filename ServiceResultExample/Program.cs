using ServiceResultExample.Client;
using ServiceResultExample.Example1_Beginner;
using ServiceResultExample.Example2_Intermediate;
using ServiceResultExample.Example3_Advanced;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IService1, Service1>();
builder.Services.AddScoped<IService2, Service2>();
builder.Services.AddScoped<IService3, Service3>();
builder.Services.AddScoped<IClient, Client>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
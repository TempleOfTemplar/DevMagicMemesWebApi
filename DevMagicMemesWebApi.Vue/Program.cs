using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.ConfigureServices();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.Configure();

app.Run();

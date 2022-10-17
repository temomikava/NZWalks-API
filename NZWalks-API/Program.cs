using NZWalks_API.Models.Domain;
using NZWalks_API.Repositories;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ISqlRepository<Region,Guid>, RegionsRepository>();
builder.Services.AddScoped<ISqlRepository<Walk,Guid>, WalksRepository>();
builder.Services.AddScoped<ISqlRepository<WalkDifficulty,Guid>, WalkdifficultyRepo>();
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddFluentValidation(options => options.RegisterValidatorsFromAssemblyContaining<Program>());



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

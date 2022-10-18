using NZWalks_API.Models.Domain;
using NZWalks_API.Repositories;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TokenHandler = NZWalks_API.Repositories.TokenHandler;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "JWT Authentication",
        Description = "Enter a valid JWT bearer token",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };
    opt.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {securityScheme,new string[]{} }
    });
});
builder.Services.AddScoped<ISqlRepository<Region,Guid>, RegionsRepository>();
builder.Services.AddScoped<ISqlRepository<Walk,Guid>, WalksRepository>();
builder.Services.AddScoped<ISqlRepository<WalkDifficulty,Guid>, WalkdifficultyRepo>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITokenHandler, TokenHandler>();
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddFluentValidation(options => options.RegisterValidatorsFromAssemblyContaining<Program>());
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt => opt.TokenValidationParameters =
                                   new TokenValidationParameters
                                   {
                                       ValidateIssuer = true,
                                       ValidateAudience=true,
                                       ValidateLifetime=true,
                                       ValidateIssuerSigningKey=true,
                                       ValidIssuer = builder.Configuration["Jwt:Issuer"],
                                       ValidAudience = builder.Configuration["Jwt:Audience"],
                                       IssuerSigningKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                                   });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

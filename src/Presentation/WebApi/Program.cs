using Application.Interfaces;
using Application.Mappings;
using Application.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infrastructure.Configuration;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.HttpsPolicy;
using User.Domain.Repositories;
using WebApi.Validators;

var builder = WebApplication.CreateBuilder(args);

// Configure HTTPS redirection options
builder.Services.Configure<HttpsRedirectionOptions>(options =>
{
    options.HttpsPort = 7255; // Your HTTPS port from launch settings
});

// Bind MongoDB settings
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));

// Register AutoMapper
builder.Services.AddAutoMapper(typeof(UserProfile));

// Register the UserRepository
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddControllers();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<UserDtoValidator>();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.UseMiddleware<WebApi.Middleware.ExceptionMiddleware>();


app.Run();



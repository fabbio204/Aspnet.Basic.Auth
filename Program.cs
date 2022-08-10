using Aspnet.Basic.Auth.Handlers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Versionamento da API
builder.Services.AddApiVersioning(config =>
{
    config.DefaultApiVersion = new ApiVersion(1, 0);
    config.AssumeDefaultVersionWhenUnspecified = true;
});

// Adiciona a autenticação Basic
builder.Services.AddAuthentication(BasicAuthenticationHandler.SCHEMA)
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>(BasicAuthenticationHandler.SCHEMA, null);

// Compila o container de injeção de dependência
WebApplication app = builder.Build();

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
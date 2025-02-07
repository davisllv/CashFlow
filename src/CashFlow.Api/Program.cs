using CashFlow.Api.Filters;
using CashFlow.Api.Middleware;
using CashFlow.Api.Token;
using CashFlow.Domain.Security.Tokens;
using CashFlow.Infraestructure.Extensions;
using CashFlow.Infraestructure.Migrations;
using CashFlow.Infrastructure;
using CashFlow.Infrastructure.DataAccess;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// Forma de adicionar um formato de colocar o bearer no swagger
builder.Services.AddSwaggerGen(c => { 
    c.SwaggerDoc("v1", 
        new OpenApiInfo 
        { 
            Title = "My API", 
            Version = "v1" 
        }); 
    c.AddSecurityDefinition("Bearer", 
        new OpenApiSecurityScheme 
        { 
            In = ParameterLocation.Header, 
            Description = "Por favor, insira 'Bearer' [espa�o] e ent�o o token JWT na caixa de texto abaixo.", 
            Name = "Authorization", 
            Type = SecuritySchemeType.ApiKey, 
            Scheme = "Bearer" 
        }); 
    c.AddSecurityRequirement(
        new OpenApiSecurityRequirement 
        { 
            { 
                new OpenApiSecurityScheme 
                { Reference = new OpenApiReference 
                { Type = ReferenceType.SecurityScheme, 
                    Id = "Bearer" } }, 
                new string[] { } 
            } 
        }); 
});

builder.Services.AddMvc(option => option.Filters.Add(typeof(ExceptionFilter))); // Para definir onde estar� os errors;

// DependencyInjectionExtension.AddInfrastructure(builder.Services); A forma a baixo � uma forma de extender as funcionalidades do IServiceCollection, colocando a interface e um this antes, a classe e o m�todo tem que ser est�ticos.
 
builder.Services.AddInfrastructure(builder.Configuration); // Fica mais f�cil de gerir a inje��o de depend�ncia, porque eu apenas adiciono os reposit�rios dentro desse m�todo. Extendido a interface IServiceCollection
builder.Services.AddApplication();
// Formato de ter acesso, via inje��o de depend�ncia, l� em infraestrutura do token, que est� no projeto de API
builder.Services.AddScoped<ITokenProvider, HttpContextTokenValue>();
// Sinalizo para o services dar acesso ao context acessor
builder.Services.AddHttpContextAccessor();

//Passos para Valida��o do TOken
var signKey = builder.Configuration.GetValue<string>("Settings:Jwt:SigninKey");

builder.Services.AddAuthentication(config =>
{
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(config =>
{
    config.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = new TimeSpan(0),
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signKey!))
    };
});

builder.Services.AddHealthChecks().AddDbContextCheck<CashFlowDbContext>();

var app = builder.Build();

app.MapHealthChecks("/Health", new HealthCheckOptions
{
    AllowCachingResponses = false,
    ResultStatusCodes =
    {
        [HealthStatus.Healthy] = StatusCodes.Status200OK,
        [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable,

    }
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<CultureMiddleware>(); // Forma de inserir um middleware na API

app.UseHttpsRedirection();


app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllers();

if(!builder.Configuration.IsTestEnvironment())
    await MigrateDataBase();

app.Run();

// Forma de criar um fun��o para poder criar e gerar as migrations toda vez que o projeto for rodado.
// Preciso remover a execu��o dessa fun��o caso seja test.
async Task MigrateDataBase()
{
    // Feito a simula��o de uma inje��o de depend�ncia com a inser��o de um scopo, para inserir o DbContext
    await using var scope = app.Services.CreateAsyncScope();
    // Fa�o esse formato para que eu n�o tenha acesso direto ao dbContext dentro do program, visto que ele � internal
    await DataBaseMigration.MigrateDatabase(scope.ServiceProvider);
}
// Preciso definir para eu ter acesso ao prgram no test de integra��o
public partial class Program { }
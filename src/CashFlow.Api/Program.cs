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
            Description = "Por favor, insira 'Bearer' [espaço] e então o token JWT na caixa de texto abaixo.", 
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

builder.Services.AddMvc(option => option.Filters.Add(typeof(ExceptionFilter))); // Para definir onde estará os errors;

// DependencyInjectionExtension.AddInfrastructure(builder.Services); A forma a baixo é uma forma de extender as funcionalidades do IServiceCollection, colocando a interface e um this antes, a classe e o método tem que ser estáticos.
 
builder.Services.AddInfrastructure(builder.Configuration); // Fica mais fácil de gerir a injeção de dependência, porque eu apenas adiciono os repositórios dentro desse método. Extendido a interface IServiceCollection
builder.Services.AddApplication();
// Formato de ter acesso, via injeção de dependência, lá em infraestrutura do token, que está no projeto de API
builder.Services.AddScoped<ITokenProvider, HttpContextTokenValue>();
// Sinalizo para o services dar acesso ao context acessor
builder.Services.AddHttpContextAccessor();

//Passos para Validação do TOken
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

// Forma de criar um função para poder criar e gerar as migrations toda vez que o projeto for rodado.
// Preciso remover a execução dessa função caso seja test.
async Task MigrateDataBase()
{
    // Feito a simulação de uma injeção de dependência com a inserção de um scopo, para inserir o DbContext
    await using var scope = app.Services.CreateAsyncScope();
    // Faço esse formato para que eu não tenha acesso direto ao dbContext dentro do program, visto que ele é internal
    await DataBaseMigration.MigrateDatabase(scope.ServiceProvider);
}
// Preciso definir para eu ter acesso ao prgram no test de integração
public partial class Program { }
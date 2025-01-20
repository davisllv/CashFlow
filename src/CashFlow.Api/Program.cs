using CashFlow.Api.Filters;
using CashFlow.Api.Middleware;
using CashFlow.Infraestructure.Migrations;
using CashFlow.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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

// DependencyInjectionExtension.AddInfrastructure(builder.Services); A forma a baixo é uma forma de extender as funcionalidades do IServiceCollection
 
builder.Services.AddInfrastructure(builder.Configuration); // Fica mais fácil de gerir a injeção de dependência, porque eu apenas adiciono os repositórios dentro desse método.
builder.Services.AddApplication();

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

var app = builder.Build();

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

await MigrateDataBase();

app.Run();

// Forma de criar um função para poder criar e gerar as migrations toda vez que o projeto for rodado.
async Task MigrateDataBase()
{
    // Feito a simulação de uma injeção de dependência com a inserção de um scopo, para inserir o DbContext
    await using var scope = app.Services.CreateAsyncScope();
    // Faço esse formato para que eu não tenha acesso direto ao dbContext dentro do program, visto que ele é internal
    await DataBaseMigration.MigrateDatabase(scope.ServiceProvider);
}
// Preciso definir para eu ter acesso ao prgram no test de integração
public partial class Program { }
using CashFlow.Api.Filters;
using CashFlow.Api.Middleware;
using CashFlow.Infraestructure.Migrations;
using CashFlow.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMvc(option => option.Filters.Add(typeof(ExceptionFilter))); // Para definir onde estará os errors;

// DependencyInjectionExtension.AddInfrastructure(builder.Services); A forma a baixo é uma forma de extender as funcionalidades do IServiceCollection
 
builder.Services.AddInfrastructure(builder.Configuration); // Fica mais fácil de gerir a injeção de dependência, porque eu apenas adiciono os repositórios dentro desse método.
builder.Services.AddApplication(); 

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<CultureMiddleware>(); // Forma de inserir um middleware na API

app.UseHttpsRedirection();

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
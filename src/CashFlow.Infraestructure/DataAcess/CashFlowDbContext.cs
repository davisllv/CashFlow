using CashFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.DataAccess;

public class CashFlowDbContext : DbContext
{
    public CashFlowDbContext(DbContextOptions options) : base(options) { }
    // Eu passo para o construtor da classe base.
    // Daqui que a Migration tira para criar as tabelas. Eu posso também, colocar o db set dentro das tabelas aqui dentro, isto é, dentro de expense, eu consigo dar um dbset e ai já basta.
    public DbSet<Expense> Expenses { get; set; }
    public DbSet<User> Users { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Forma de remodelar as tabelas pelas migrations.
        modelBuilder.Entity<Tag>().ToTable("Tags");
    }
}
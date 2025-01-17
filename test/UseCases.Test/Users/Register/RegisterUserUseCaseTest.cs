using CashFlow.Application.UseCase.Users.Register;
using CommomTestUtilities.Mapper;
using CommomTestUtilities.Repositories;
using CommomTestUtilities.Request;
using FluentAssertions;

namespace UseCases.Test.Users.Register;
public class RegisterUserUseCaseTest
{
    [Fact]
    public async Task Execute()
    {
        var useCase = CreateUseCase();
        var request = RequestRegisterUserJsonBuilder.Build();

        var result = await useCase.Execute(request);

        result.Should().NotBeNull();
        result.Name.Should().Be(request.Name);
        result.Token.Should().NotBeNullOrWhiteSpace();
    }

    private RegisterUserUseCase CreateUseCase()
    {
        var mapper = MapperBuilder.Build(); // Eu vou colocando algumas coisas de forma real ou não.
        // São criado repositórios fakes, com nenhuma implementação real. É só algo fake, pois só preciso testar a regra de negócio, não a inserção no banco de dados.
        // São Formatos simples pois não retornam nenhum valor.
        var unitOfWork = UnitOfWorkBuilder.Build();
        var writeRepository = UserWriteOnlyRepositoryBuilder.Build();

        return new RegisterUserUseCase(mapper, unitOfWork, writeRepository, null, null, null);
    }
}

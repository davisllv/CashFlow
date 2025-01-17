using AutoMapper;
using CashFlow.Application.UseCase.Users.Register;
using CashFlow.Communication.Request;
using CashFlow.Communication.Responses.Users;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Users;
using CashFlow.Domain.Security.Cryptography;
using CashFlow.Domain.Security.Tokens;
using CommomTestUtilities.Cryptography;
using CommomTestUtilities.Mapper;
using CommomTestUtilities.Repositories;
using CommomTestUtilities.Request;
using CommomTestUtilities.Token;
using FluentAssertions;

namespace UseCases.Test.Users.Register;
public class RegisterUserUseCaseTest
{
    [Fact]
    public async Task Execute()
    {
        RegisterUserUseCase useCase = CreateUseCase();
        RequestUserJson request = RequestRegisterUserJsonBuilder.Build();

        ResponseRegisterUserJson result = await useCase.Execute(request);

        result.Should().NotBeNull();
        result.Name.Should().Be(request.Name);
        result.Token.Should().NotBeNullOrWhiteSpace();
    }

    private RegisterUserUseCase CreateUseCase()
    {
        IMapper mapper = MapperBuilder.Build(); 
        // Eu vou colocando algumas coisas de forma real ou não.
        // São criado repositórios fakes, com nenhuma implementação real. É só algo fake, pois só preciso testar a regra de negócio, não a inserção no banco de dados.
        // São Formatos simples pois não retornam nenhum valor. Mock - São implementações fakes
        // Posso fazer mock em classes, mas ai eu preciso fazer implementações específicas para aquela clase - Errado

        IUnitOfWork unitOfWork = UnitOfWorkBuilder.Build();
        IUserWriteOnlyRepository writeRepository = UserWriteOnlyRepositoryBuilder.Build();

        // Esse é o formato para criar um mock com retornos que não void. São retornos fixos.
        // Dessa forma ele faz com que todos os retornos seja o valor default.
        IPasswordEncripter passwordEncripter = PasswordEncripterBuilder.Build();
        IAcessTokenGenerator acessTokenGenerator = AcessTokenGeneratorBuilder.Build();

        IUserReadOnlyRepository readRepository = new UserReadOnlyRepositoryBuilder().Build();

        return new RegisterUserUseCase(mapper, unitOfWork, writeRepository, readRepository, passwordEncripter, acessTokenGenerator);
    }
}

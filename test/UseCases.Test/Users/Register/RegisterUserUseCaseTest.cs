using AutoMapper;
using CashFlow.Application.UseCase.Users.Register;
using CashFlow.Communication.Request;
using CashFlow.Communication.Responses.Users;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Users;
using CashFlow.Domain.Security.Cryptography;
using CashFlow.Domain.Security.Tokens;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionBase;
using CommomTestUtilities.Cryptography;
using CommomTestUtilities.Mapper;
using CommomTestUtilities.Repositories;
using CommomTestUtilities.Request;
using CommomTestUtilities.Token;
using FluentAssertions;

namespace UseCases.Test.Users.Register;
public class RegisterUserUseCaseTest
{
    private RegisterUserUseCase CreateUseCase(string? email = null)
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
        IPasswordEncripter passwordEncripter = new PasswordEncripterBuilder().Build();
        IAcessTokenGenerator acessTokenGenerator = AcessTokenGeneratorBuilder.Build();

        UserReadOnlyRepositoryBuilder readRepository = new UserReadOnlyRepositoryBuilder();

        // Por isso foi criado o new, para eu conseguir acessar outros métodos
        if(!string.IsNullOrEmpty(email))
            readRepository.ExistActiveUserWithEmail(email);

        return new RegisterUserUseCase(mapper, unitOfWork, writeRepository, readRepository.Build(), passwordEncripter, acessTokenGenerator);
    }
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
    [Fact]
    public async Task Error_Name_Empty()
    {
        RegisterUserUseCase useCase = CreateUseCase();
        RequestUserJson request = RequestRegisterUserJsonBuilder.Build();
        request.Name = string.Empty;

        var act = async () => await useCase.Execute(request);
        // A função vai estourar um erro e não retornar nada.
        // A forma abaixo é uma maneira de especificar mais ainda para obter a exceção.
        var result = await act.Should().ThrowAsync<ErrorOnValidationException>();

        result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.NAME_EMPTY));
    }
    [Fact]
    public async Task Error_Email_Empty()
    {
        RegisterUserUseCase useCase = CreateUseCase();
        RequestUserJson request = RequestRegisterUserJsonBuilder.Build();
        request.Email = string.Empty;

        var act = async () => await useCase.Execute(request);
        var result = await act.Should().ThrowAsync<ErrorOnValidationException>();

        result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.EMAIL_EMPTY));
    }
    [Fact]
    public async Task Error_Email_Already_Exists()
    {
        RequestUserJson request = RequestRegisterUserJsonBuilder.Build();
        RegisterUserUseCase useCase = CreateUseCase(request.Email);

        var act = async () => await useCase.Execute(request);

        var result = await act.Should().ThrowAsync<ErrorOnValidationException>();

        result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.EMAIL_ALREADY_REGISTER));
    }

}

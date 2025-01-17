using CashFlow.Application.UseCase.Users.Register;
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
        return new RegisterUserUseCase();
    }
}

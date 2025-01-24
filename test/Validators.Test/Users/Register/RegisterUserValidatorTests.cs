using CashFlow.Application.UseCase.Users;
using CashFlow.Application.UseCase.Users.Register;
using CashFlow.Exception;
using CommomTestUtilities.Request;
using FluentAssertions;

namespace Validators.Test.Users.Register;
public class RegisterUserValidatorTests
{
    [Fact]
    public void Sucess()
    {
        var validator = new UserUseValidator();
        var request = RequestRegisterUserJsonBuilder.Build();

        var result = validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Error_Name_Empty(string name)
    {
        var validator = new UserUseValidator();
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = name;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.NAME_EMPTY));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Error_Email_Empty(string email)
    {
        var validator = new UserUseValidator();
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Email = email;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.EMAIL_EMPTY));
    }

    [Fact]
    public void Error_Email_Invalid()
    {
        var validator = new UserUseValidator();
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Email = "DaviDaSilvaQualquer";

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.EMAIL_INVALID));
    }
}

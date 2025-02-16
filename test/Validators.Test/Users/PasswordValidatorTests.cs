﻿using CashFlow.Application.UseCase.Users;
using CashFlow.Communication.Request;
using FluentAssertions;
using FluentValidation;

namespace Validators.Test.Users;
public class PasswordValidatorTests
{
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("         ")]
    [InlineData(null)]
    [InlineData("a")]
    [InlineData("AAAAAAAA")]
    [InlineData("aaaaaaaa")]
    [InlineData("Aaaaaaa1")]
    public void Error_Password_Invalid(string password)
    {
        var validator = new PasswordValidator<RequestRegisterUserJson>();

        var result = validator.IsValid(new ValidationContext<RequestRegisterUserJson>(new RequestRegisterUserJson()), password);

        result.Should().BeFalse();
    }
}

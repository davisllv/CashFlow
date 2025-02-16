﻿using CashFlow.Application.UseCase.Expenses;
using CashFlow.Communication.Enums;
using CashFlow.Exception;
using CommomTestUtilities.Request;
using FluentAssertions;

namespace Validators.Test.Expenses;

public class ExpenseValidatorTests
{
    [Fact] // Forma de definir que a função será um test
    public void Sucess()
    {
        // Arrange - Configuração das instâncias dos testes
        var validator = new ExpenseValidator();

        var request = RequestExpenseJsonBuilder.Build();
        // Act - Validações sendo feita, as ações

        var result = validator.Validate(request);

        // Assert

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void ErrorTitleEmpty()
    {
        // Arrange
        var validator = new ExpenseValidator();

        var request = RequestExpenseJsonBuilder.Build();
        request.Title = string.Empty;

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.TITLE_REQUIRED));
    }
    [Theory] // Forma de eu definir qual o parametro vai receber no test de forma dinâmica. A ordem, não necessariamente, será essa.
    [InlineData(0)]
    [InlineData(-10)] // Cada um desse aqui será o test.
    public void ErrorAmountGreatherThan(decimal amount)
    {
        // Arrange
        var validator = new ExpenseValidator();

        var request = RequestExpenseJsonBuilder.Build();
        request.Amount = amount;

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.AMOUNT_MUST_BE_GREATHER_THAN_ZERO));
    }
    [Fact]
    public void ErrorDateFuture()
    {
        // Arrange
        var validator = new ExpenseValidator();

        var request = RequestExpenseJsonBuilder.Build();
        request.Date = DateTime.UtcNow.AddDays(10);

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.EXPENSES_CANNOT_FOR_THE_FUTERE));
    }
    [Fact]
    public void ErrorPaymentTypeInvalid()
    {
        // Arrange
        var validator = new ExpenseValidator();

        var request = RequestExpenseJsonBuilder.Build();
        request.PaymentType = (PaymentType)10;

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.PAYMENT_TYPE_INVALID));
    }

    [Fact]
    public void Error_Tag_Invalid()
    {
        // Arrange
        var validator = new ExpenseValidator();

        var request = RequestExpenseJsonBuilder.Build();
        request.Tags.Add((Tag)10);

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.TAG_TYPE_NOT_SUPORTED));
    }
}

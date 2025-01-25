using CashFlow.Communication.Request;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Users;
using CashFlow.Domain.Security.Cryptography;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionBase;
using FluentValidation.Results;

namespace CashFlow.Application.UseCase.Users.ChangePassword;
public class ChangePasswordUserUseCase : IChangePasswordUserUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserUpdateOnlyRepository _userUpdateOnlyRepository;
    private readonly IPasswordEncripter _passwordEncripter;

    public ChangePasswordUserUseCase(ILoggedUser loggedUser, IUnitOfWork unitOfWork, IUserUpdateOnlyRepository userUpdateOnlyRepository, IPasswordEncripter passwordEncripter)
    {
        _loggedUser = loggedUser;
        _unitOfWork = unitOfWork;
        _userUpdateOnlyRepository = userUpdateOnlyRepository;
        _passwordEncripter = passwordEncripter;
    }

    public async Task Execute(RequestChangePasswordJson request)
    {
        User loggedUser = await _loggedUser.Get(); // Esse tem asNoTracking

        Validate(request, loggedUser);

        User user = await _userUpdateOnlyRepository.GetById(loggedUser.Id); // Esse não tem
        user.Password = _passwordEncripter.Encrypt(request.NewPassword);


        _userUpdateOnlyRepository.Update(user);

        await _unitOfWork.Commit();
    }

    public void Validate(RequestChangePasswordJson request, User user)
    {
        var validation = new ChangePasswordUserValidator().Validate(request);

        bool passwordMatch = _passwordEncripter.Verify(request.Password, user.Password);

        if (!passwordMatch)
            validation.Errors.Add(new ValidationFailure(string.Empty, ResourceErrorMessages.PASSWORD_DIFERENT));

        if (!validation.IsValid)
        {
            var errorMessages = validation.Errors.Select(p => p.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessages);
        }
    }
}

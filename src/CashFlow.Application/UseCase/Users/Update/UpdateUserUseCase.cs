using CashFlow.Communication.Request;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Users;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionBase;
using FluentValidation.Results;

namespace CashFlow.Application.UseCase.Users.Update;
public class UpdateUserUseCase : IUpdateUserUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILoggedUser _loggedUser;
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;
    private readonly IUserUpdateOnlyRepository _userUpdateOnlyRepository;

    public UpdateUserUseCase(IUserReadOnlyRepository userReadOnlyRepository, IUserUpdateOnlyRepository userUpdateOnlyRepository, ILoggedUser loggedUser, IUnitOfWork unitOfWork)
    {
        _userReadOnlyRepository = userReadOnlyRepository;
        _userUpdateOnlyRepository = userUpdateOnlyRepository;
        _loggedUser = loggedUser;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(RequestUpdateUserJson request)
    {
        User userLogged = await _loggedUser.Get();

        Validate(request, userLogged.Email);

        userLogged.Name = request.Name;
        userLogged.Email = request.Email;

        _userUpdateOnlyRepository.Update(userLogged);

        await _unitOfWork.Commit();
    }

    private async void Validate(RequestUpdateUserJson request, string currentEmail)
    {
        var result = new UpdateUseValidator().Validate(request);

        if (!currentEmail.Equals(request.Email))
        {
            var userExist = await _userReadOnlyRepository.ExistActiveUserWithEmail(request.Email);
            if(userExist)
                result.Errors.Add(new ValidationFailure(string.Empty, ResourceErrorMessages.EMAIL_ALREADY_REGISTER));
        }

        if (!result.IsValid)
        {
            var errorMessages = result.Errors.Select(p => p.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessages);
        }
    }

}

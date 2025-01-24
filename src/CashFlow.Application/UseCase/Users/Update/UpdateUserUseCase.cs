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

        await Validate(request, userLogged.Email);

        User user = await _userReadOnlyRepository.GetById(userLogged.Id); // Preciso usar esse getBy porque o _logged tem AsNoTrACKING

        user.Name = request.Name;
        user.Email = request.Email;

        _userUpdateOnlyRepository.Update(user);

        await _unitOfWork.Commit();
    }

    private async Task Validate(RequestUpdateUserJson request, string currentEmail)
    {
        var result = new UpdateUserValidator().Validate(request);

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

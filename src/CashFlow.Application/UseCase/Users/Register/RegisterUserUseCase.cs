using AutoMapper;
using CashFlow.Communication.Request;
using CashFlow.Communication.Responses.Users;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Repositories.Users;
using CashFlow.Domain.Security.Cryptography;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionBase;
using FluentValidation.Results;

namespace CashFlow.Application.UseCase.Users.Register;
public class RegisterUserUseCase : IRegisterUserUseCase
{
    private readonly IMapper _mapper; // Para fazer o mapeamento dos dados vindos da request;
    private readonly IPasswordEncripter _passwordEncripter;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserWriteOnlyRepository _repository;
    private readonly IUserReadOnlyRepository _repositoryReadOnly;

    public RegisterUserUseCase(IMapper mapper, IUnitOfWork unitOfWork, IUserWriteOnlyRepository repository, IUserReadOnlyRepository repositoryReadOnly, IPasswordEncripter passwordEncripter)
    {
        _mapper = mapper;
        _unitOfWork  = unitOfWork;
        _repository = repository;
        _passwordEncripter = passwordEncripter;
        _repositoryReadOnly = repositoryReadOnly;
    }

    public Task<ResponseRegisterUserJson> Execute(RequestUserJson request)
    {
        Validate(request);

        User user = _mapper.Map<User>(request);
        user.Password = _passwordEncripter.Encrypt(user.Password);
        
        throw new NotImplementedException();
    }

    private async void Validate(RequestUserJson request)
    {
        ValidationResult validation = new UserUseValidator().Validate(request);

        bool emailExists = await _repositoryReadOnly.ExistActiveUserWithEmail(request.Email);

        if (emailExists)
            validation.Errors.Add(new ValidationFailure(string.Empty, ResourceErrorMessages.EMAIL_ALREADY_REGISTER)); // Estou adicionado que está dando erro, ele torna o isvalid como false, para reutilizar a validação abaixo.

        if (!validation.IsValid)
        {
            var errorMessages = validation.Errors.Select(p => p.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessages);
        }

    }
}

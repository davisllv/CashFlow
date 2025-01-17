using AutoMapper;
using CashFlow.Communication.Request;
using CashFlow.Communication.Responses.Users;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Users;
using CashFlow.Domain.Security.Cryptography;
using CashFlow.Domain.Security.Tokens;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionBase;
using FluentValidation.Results;

namespace CashFlow.Application.UseCase.Users.Register;
public class RegisterUserUseCase : IRegisterUserUseCase
{
    private readonly IMapper _mapper; // Para fazer o mapeamento dos dados vindos da request;
    private readonly IPasswordEncripter _passwordEncripter;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserWriteOnlyRepository _repositoryWriteOnly;
    private readonly IUserReadOnlyRepository _repositoryReadOnly;
    private readonly IAcessTokenGenerator _tokenGenerator;

    public RegisterUserUseCase(IMapper mapper, IUnitOfWork unitOfWork, IUserWriteOnlyRepository repository, IUserReadOnlyRepository repositoryReadOnly, IPasswordEncripter passwordEncripter, IAcessTokenGenerator tokenGenerator)
    {
        _mapper = mapper;
        _unitOfWork  = unitOfWork;
        _repositoryWriteOnly = repository;
        _passwordEncripter = passwordEncripter;
        _repositoryReadOnly = repositoryReadOnly;
        _tokenGenerator = tokenGenerator;
    }

    public async Task<ResponseRegisterUserJson> Execute(RequestUserJson request)
    {
        await Validate(request);

        User user = _mapper.Map<User>(request);
        user.Password = _passwordEncripter.Encrypt(request.Password);
        user.UserIdentifier = Guid.NewGuid(); // GUID é para utilizar o GUID para gerar o TOKEN

        await _repositoryWriteOnly.Add(user);

        await _unitOfWork.Commit();


        return new ResponseRegisterUserJson
        {
            Name = user.Name,
            Token = _tokenGenerator.Generate(user)
        };
    }

    private async Task Validate(RequestUserJson request)
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

using AutoMapper;
using CashFlow.Communication.Request;
using CashFlow.Communication.Responses.Users;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Repositories.Users;
using CashFlow.Exception.ExceptionBase;
using FluentValidation.Results;

namespace CashFlow.Application.UseCase.Users.Register;
public class RegisterUserUseCase : IRegisterUserUseCase
{
    private readonly IMapper _mapper; // Para fazer o mapeamento dos dados vindos da request;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserWriteOnlyRepository _repository;

    public RegisterUserUseCase(IMapper mapper, IUnitOfWork unitOfWork, IUserWriteOnlyRepository repository)
    {
        _mapper = mapper;
        _unitOfWork  = unitOfWork;
        _repository = repository;
    }

    public Task<ResponseRegisterUserJson> Execute(RequestUserJson request)
    {
        Validate(request);

        User expense = _mapper.Map<User>(request);
        throw new NotImplementedException();
    }

    private void Validate(RequestUserJson request)
    {
        ValidationResult validation = new UserUseValidator().Validate(request);
        if (!validation.IsValid)
        {
            var errorMessages = validation.Errors.Select(p => p.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessages);
        }

    }
}

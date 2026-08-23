using ExpenseTrackerApi.Application.Auth.Dtos;
using MediatR;

namespace ExpenseTrackerApi.Application.Auth.Queries.GetAccountInfo;

public class GetAccountInfoQuery : IRequest<AccountDto>;
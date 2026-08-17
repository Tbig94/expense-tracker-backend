using ExpenseTrackerApi.Application.Auth.Dtos;
using MediatR;

namespace ExpenseTrackerApi.Application.Auth.Commands.Login;

public record LoginCommand(LoginDto User) : IRequest<LoginResultDto>;

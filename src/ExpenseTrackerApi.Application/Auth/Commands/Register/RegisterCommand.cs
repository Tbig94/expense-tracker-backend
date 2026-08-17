using ExpenseTrackerApi.Application.Auth.Dtos;
using MediatR;

namespace ExpenseTrackerApi.Application.Auth.Commands.Register;

public record RegisterCommand(RegisterDto User) : IRequest;

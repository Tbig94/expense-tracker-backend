using MediatR;

namespace ExpenseTrackerApi.Application.Auth.Commands.DeleteAccount;

public record DeleteAccountCommand() : IRequest;
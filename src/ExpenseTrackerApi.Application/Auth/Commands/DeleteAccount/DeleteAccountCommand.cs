using ExpenseTrackerApi.Application.Auth.Dtos;
using MediatR;

namespace ExpenseTrackerApi.Application.Auth.Commands.DeleteAccount;

public record DeleteAccountCommand() : IRequest
{
    //public DeleteAccountDto Account { get; set; }
}

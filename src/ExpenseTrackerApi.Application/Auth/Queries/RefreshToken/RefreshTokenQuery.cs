using ExpenseTrackerApi.Application.Auth.Dtos;
using MediatR;

namespace ExpenseTrackerApi.Application.Auth.Queries.RefreshToken;

public class RefreshTokenQuery : IRequest<AuthResponse>
{
    public string? RefreshToken { get; set; }
}

namespace ExpenseTrackerApi.Application.Common.Interfaces;

public interface ICurrentUserService
{
    Guid UserId { get; }
    string Email { get; }
    List<string> Roles { get; }


    string HashPassword(string password);

    bool VerifyPassword(string rawPassword, string storedHashFromDb);

    void SetCurrentUser(Guid userId, string email, List<string> roles);
}

namespace ExpenseTrackerApi.Application.Auth.Dtos;

public class RegisterDto
{
    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;
}

public class LoginDto
{
    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;
}

public class LoginResponse
{
    public LoginResultDto UserDto { get; set; } = default!;
    public string AccessToken { get; set; } = string.Empty;
}

public class LoginResultDto
{
    public DateTime ExpiresAt { get; set; }

    public Guid UserId { get; set; }

    public string Email { get; set; } = null!;
}

public class AccountDto
{
    public string? Email { get; set; }

    public string? Name { get; set; }
}


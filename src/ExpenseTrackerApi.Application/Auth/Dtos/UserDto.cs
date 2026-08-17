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


public class LoginResultDto
{
    public string Token { get; set; } = null!;

    public DateTime ExpiresAt { get; set; }

    public Guid UserId { get; set; }

    public string Email { get; set; } = null!;
}



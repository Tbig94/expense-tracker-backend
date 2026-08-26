namespace ExpenseTrackerApi.Application.Common.Interfaces;

public interface ITokenService
{
    string GenerateAccessToken(Guid userId, string email, List<string> roles = null);

    string GenerateRefreshToken();

    TokenValidationResult ValidateToken(string token);

    IDictionary<string, string> DecodeToken(string token);
}

public class TokenValidationResult
{
    public bool IsValid { get; set; }
    public string? Error { get; set; }
}

namespace Shared.Models;

public class LoginRequest
{
    public string RegistrationNumber { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
namespace WebApp.Models;

public class AdminLoginModel
{
    public required string Password { get; set; } = default!;
    public string?  Message { get; set; } = default!;
}
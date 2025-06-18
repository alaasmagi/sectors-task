using Microsoft.AspNetCore.Mvc;
using BCrypt.Net;

namespace WebApp.Controllers;

public class BaseController() : Controller
{
    public bool IsTokenValidAsync(HttpContext httpContext)
    {
        var storedUsername = Environment.GetEnvironmentVariable("ADMIN_USERNAME");
        var sessionToken = HttpContext.Session.GetString("token");
        
        if (string.IsNullOrEmpty(storedUsername) || string.IsNullOrEmpty(sessionToken))
        {
            return false;
        }
        
        return BCrypt.Net.BCrypt.Verify(storedUsername, sessionToken);
    }

    public bool VerifyPassword(string password)
    {
        var storedBcryptPass = Environment.GetEnvironmentVariable("ADMIN_KEY_BCRYPT");
        
        if (string.IsNullOrEmpty(storedBcryptPass) || string.IsNullOrEmpty(password))
        {
            return false;
        }

        if (!BCrypt.Net.BCrypt.Verify(password, storedBcryptPass))
        {
            return false;
        }
        
        return true;
    }

    public void SetToken()
    {
        var storedUsername = Environment.GetEnvironmentVariable("ADMIN_USERNAME");
        var newSessionToken = BCrypt.Net.BCrypt.HashPassword(storedUsername);      
        HttpContext.Session.SetString("token", newSessionToken);
    }
}
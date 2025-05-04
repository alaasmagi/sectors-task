using Azure;
using BLL;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.ApiControllers;

[ApiController]
[Route("api/[controller]")]
public class PersonController(PersonService personService) : ControllerBase
{

    [HttpGet("Sectors")]
    public async Task<IActionResult> GetAllSectors()
    {
        var sectors = await personService.GetAllSectors();

        if (sectors.Count == 0)
        {
            return NotFound(new { message = "Sectors not found"});
        }

        return Ok(sectors);
    }
    
    /*[HttpGet("Person/{id}")]
    public async Task<IActionResult> GetPersonById(int id)
    {
        var user = await userManagementService.GetUserByUniIdAsync(model.UniId);

        if (user == null)
        {
            return NotFound(new { message = "User not found", messageCode = "user-not-found" });
        }

        var userAuthData = await userManagementService.AuthenticateUserAsync(user.Id, model.Password);
        if (userAuthData == null || !ModelState.IsValid)
        {
            logger.LogWarning($"Form data is invalid");
            return Unauthorized(new
                { message = "Invalid UNI-ID or password", messageCode = "invalid-uni-id-password" });
        }

        var token = authService.GenerateJwtToken(user);
        Response.Cookies.Append("token", token, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            MaxAge = TimeSpan.FromDays(60)
        });

        logger.LogInformation($"User with UNI-ID {model.UniId} was logged in successfully");
        return Ok(new { Token = token });
    }
    
    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        logger.LogInformation($"{HttpContext.Request.Method.ToUpper()} - {HttpContext.Request.Path}");
        var user = await userManagementService.GetUserByUniIdAsync(model.UniId);

        if (user == null)
        {
            return NotFound(new { message = "User not found", messageCode = "user-not-found" });
        }

        var userAuthData = await userManagementService.AuthenticateUserAsync(user.Id, model.Password);
        if (userAuthData == null || !ModelState.IsValid)
        {
            logger.LogWarning($"Form data is invalid");
            return Unauthorized(new
                { message = "Invalid UNI-ID or password", messageCode = "invalid-uni-id-password" });
        }

        var token = authService.GenerateJwtToken(user);
        Response.Cookies.Append("token", token, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            MaxAge = TimeSpan.FromDays(60)
        });

        logger.LogInformation($"User with UNI-ID {model.UniId} was logged in successfully");
        return Ok(new { Token = token });
    }*/
}
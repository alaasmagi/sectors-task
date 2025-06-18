using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using WebApp.Models;

namespace WebApp.Controllers;

public class HomeController : BaseController
{
    [HttpGet]
    public IActionResult Index(string? message)
    {
        var model = new AdminLoginModel
        {
            Password = string.Empty,
            Message = message ?? string.Empty
        };

        return View(model);
    }

    [HttpPost]
    public IActionResult Index([Bind("Username", "Password")] AdminLoginModel model)
    {
        if (!VerifyPassword(model.Password))
        {
            return Index("Wrong username or password!");
        }
        
        SetToken();
        return  RedirectToAction("Home");
    }

    public IActionResult LogOut()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index");
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Home(string? message)
    {
        var tokenValidity = IsTokenValidAsync(HttpContext);
        if (!tokenValidity)
        {
            return Unauthorized("You cannot access admin panel without logging in!");
        }
        
        var model = new AdminLoginModel
        {
            Password = string.Empty,
            Message = message ?? string.Empty
        };
        return View(model);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
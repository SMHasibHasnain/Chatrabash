using System;
using API.ExtraDtos;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class AccountController : BaseController
{
    private readonly SignInManager<User> _signInManager;

    public AccountController(SignInManager<User> signInManager)
    {
        _signInManager = signInManager;
    }

    [AllowAnonymous] 
    [HttpPost("register")]
    public async Task<ActionResult> Register(RegisterDto registerDto)
    {
        var user = new User
        {
            DisplayName = registerDto.DisplayName,
            Email = registerDto.Email,
            UserName = registerDto.Email
        };

        var result = await _signInManager.UserManager.CreateAsync(user, registerDto.Password);

        if (result.Succeeded)
        {
            return Ok("Registration successful! Please login.");
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(error.Code, error.Description);
        }

        return ValidationProblem();
}
}
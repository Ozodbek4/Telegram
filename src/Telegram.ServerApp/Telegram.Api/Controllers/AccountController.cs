using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Telegram.Application.Common.Models.Dtos;
using Telegram.Application.Common.Services;
using Telegram.Application.Common.Settings;

namespace Telegram.Api.Controllers;

public class AccountController(IAccountService accountService, IOptions<JwtSettings> jwtSettings) : Controller
{
    [HttpPost("singUp")]
    public async ValueTask<IActionResult> SignUp([FromBody] SignUpDetails user)
    {
        var result = await accountService.SignUpAsync(user, true, HttpContext.RequestAborted);

        return result ? Ok() : BadRequest();
    }

    public IActionResult Login()
    {
        if (User.Identity.IsAuthenticated)
            return Ok();
        return NotFound();
    }
    [HttpPost("signIn")]
    public async ValueTask<IActionResult> SignIn([FromBody] SignInDetails signInDto)
    {
        var result = await accountService.SignInAsync(signInDto, true, HttpContext.RequestAborted);
        Response.Cookies.Append("token", result, new CookieOptions 
        {
            HttpOnly = true,
            Expires = DateTime.UtcNow.AddMinutes(jwtSettings.Value.ExpressionTimeInMinutes)
        });

        return result is not null ? Ok(result) : BadRequest();
    }
}
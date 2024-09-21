using Microsoft.AspNetCore.Mvc;
using Telegram.Application.Common.Models.Dtos;
using Telegram.Application.Common.Services;

namespace Telegram.Api.Controllers;

public class AccountController(IAccountService accountService, IUserService userService) : Controller
{
    [HttpGet("me")]
    public async ValueTask<IActionResult> GetCurrentUser()
    {
        if (!User.Identity.IsAuthenticated)
            return NotFound();

        return Ok(await userService.GetByIdAsync(GetRequestUserId()));
    }

    [HttpPost("sign-up")]
    public async ValueTask<IActionResult> SignUp([FromBody] SignUpDetails user)
    {
        var result = await accountService.SignUpAsync(user, true, HttpContext.RequestAborted);

        return result ? Ok() : BadRequest();
    }

    //[HttpGet("me")]
    public IActionResult Login()
    {
        if (User.Identity.IsAuthenticated)
            return Ok();
        return NotFound();
    }
    [HttpPost("sign-in")]
    public async ValueTask<IActionResult> SignIn([FromBody] SignInDetails signInDto)
    {
        var result = await accountService.SignInAsync(signInDto, true, HttpContext.RequestAborted);
        Response.Cookies.Append("token", result, new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTime.UtcNow.AddMinutes(accountService.JwtSettings.ExpirationTimeInMinutes)
        });

        return result is not null ? Ok(result) : BadRequest();
    }

    private Guid GetRequestUserId() => Guid.Parse(HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == "UserId")!.Value);
}
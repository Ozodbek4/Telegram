using Microsoft.AspNetCore.Mvc;
using Telegram.Application.Common.Models.Dtos;
using Telegram.Application.Common.Services;

namespace Telegram.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController(IAccountService accountService) : ControllerBase
{
    [HttpPost("singUp")]
    public async ValueTask<IActionResult> SignUp([FromBody] SignUpDetails user)
    {
        var result = await accountService.SignUpAsync(user, true, HttpContext.RequestAborted);

        return result ? Ok() : BadRequest();
    }

    [HttpPost("signIn")]
    public async ValueTask<IActionResult> SignIn([FromBody] SignInDetails signInDto)
    {
        var result = await accountService.SignInAsync(signInDto, true, HttpContext.RequestAborted);

        return result is not null ? Ok(result) : BadRequest();
    }
}
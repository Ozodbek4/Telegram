using Microsoft.AspNetCore.Mvc;
using Telegram.Application.Common.Identity;
using Telegram.WebUI.Models.Login;

namespace Telegram.WebUI.Controllers;

public class AuthController(IAccountService accountService) : BaseController
{
    [HttpPost("sing-in")]
    public async ValueTask<IActionResult> SignIn([FromBody] SignInModel model)
    {
        var result = await accountService.SignInAsync(model.UserName, model.Password);

        return Ok(result);
    }
}
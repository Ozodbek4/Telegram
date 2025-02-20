using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Telegram.Application.Common.Identity;
using Telegram.WebUI.Models.Login;
using Telegram.WebUI.Models.Users;

namespace Telegram.WebUI.Controllers;

public class AuthController(IAccountService accountService, IMapper mapper) : BaseController
{
    [HttpPost("sing-in")]
    public async ValueTask<IActionResult> SignIn([FromBody] SignInModel model)
    {
        var token = await accountService.SignInAsync(model.UserName, model.Password);

        var result = new
        {
            User = mapper.Map<UserViewModel>(token.User),
            Token = token.Token,
        };

        return Ok(result);
    }
}
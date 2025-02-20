using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Telegram.Application.Common.Models;
using Telegram.Application.Services;
using Telegram.Domain.Entities;
using Telegram.WebUI.Extensions;
using Telegram.WebUI.Models.Users;

namespace Telegram.WebUI.Controllers;

public class UserController(IUserService userService, IMapper mapper) : BaseController
{
    [HttpGet]
    public async ValueTask<IActionResult> GetAll(
        [FromQuery] PaginationParameters pagination,
        [FromQuery] SortingParameters sorting,
        [FromQuery] string? search = null
        )
    {
        var exists = await userService.GetAllAsync(pagination, sorting, search, cancellationToken: HttpContext.RequestAborted);

        HttpContext.AddPaginationMetaData(exists.PaginationInfo);

        return Ok(mapper.Map<IEnumerable<UserViewModel>>(exists.Data));
    }

    [HttpGet("{id:long}")]
    public async ValueTask<IActionResult> Get([FromRoute] long id)
    {
        var exist = await userService.GetByIdAsync(id, cancellationToken: HttpContext.RequestAborted);

        return Ok(mapper.Map<UserViewModel>(exist));
    }

    [HttpPost]
    public async ValueTask<IActionResult> Post([FromBody] CreateUserModel model)
    {
        var created = await userService.CreateAsync(mapper.Map<User>(model), HttpContext.RequestAborted);

        return Ok(mapper.Map<UserViewModel>(created));
    }

    [HttpPut("{id:long}")]
    public async ValueTask<IActionResult> Put([FromRoute] long id, [FromBody] UpdateUserModel model)
    {
        model.Id = id;

        var updated = await userService.UpdateAsync(mapper.Map<User>(model), HttpContext.RequestAborted);

        return Ok(mapper.Map<UserViewModel>(updated));
    }

    [HttpDelete("{id:long}")]
    public async ValueTask<IActionResult> Delete([FromRoute] long id)
    {
        var deleted = await userService.DeleteAsync(id, HttpContext.RequestAborted);

        return Ok(deleted);
    }
}
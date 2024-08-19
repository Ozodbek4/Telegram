using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Telegram.Api.Models.Dtos;
using Telegram.Application.Common.Services;
using Telegram.Domain.Entities;

namespace Telegram.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UserController(IUserService userService, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async ValueTask<IActionResult> GetAll()
    {
        return Ok(userService.Get());
    }

    [HttpGet("{id:guid}")]
    public async ValueTask<IActionResult> GetById([FromRoute] Guid id)
    {
        var result = await userService.GetByIdAsync(id, true, HttpContext.RequestAborted);

        return result is not null ? Ok(result) : NotFound();
    }

    [HttpPost]
    public async ValueTask<IActionResult> Create([FromBody] UserDto dto)
    {
        return Ok(await userService.CreateAsync(mapper.Map<User>(dto), true, HttpContext.RequestAborted));
    }

    [HttpPut]
    public async ValueTask<IActionResult> Update([FromBody] UserDto dto)
    {
        var result = await userService.GetByIdAsync(dto.Id, true, HttpContext.RequestAborted);

        if (result is null)
            return NotFound();

        return Ok(await userService.UpdateAsync(mapper.Map(dto, result), true, HttpContext.RequestAborted));
    }

    [HttpDelete("{id:guid}")]
    public async ValueTask<IActionResult> DeleteById([FromRoute] Guid id)
    {
        var result = await userService.GetByIdAsync(id, true, HttpContext.RequestAborted);

        if (result is null)
            return NotFound();

        return Ok(await userService.DeleteByIdAsync(id, true, HttpContext.RequestAborted));
    }
}
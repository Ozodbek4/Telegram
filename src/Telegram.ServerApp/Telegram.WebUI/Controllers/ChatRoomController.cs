using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Telegram.Application.Common.Extensions;
using Telegram.Application.Common.Models;
using Telegram.Application.Services;
using Telegram.Domain.Entities;
using Telegram.WebUI.Models.ChatRooms;

namespace Telegram.WebUI.Controllers;

public class ChatRoomController(
    IChatRoomService chatRoomService,
    IMapper mapper,
    IValidator<CreateChatRoomModel> createValidator,
    IValidator<UpdateChatRomModel> updateValidator
    ) : BaseController
{
    [HttpGet]
    public async ValueTask<IActionResult> GetAll(
        [FromQuery] PaginationParameters pagination,
        [FromQuery] SortingParameters sorting,
        [FromQuery] string? search = null
        )
    {
        return Ok();
    }

    [HttpGet("{id:long}")]
    public async ValueTask<IActionResult> Get([FromRoute] long id)
    {
        var exist = await chatRoomService.GetByIdAsync(id, ["FirstUser", "SecondUser", "LastMessage"], cancellationToken: HttpContext.RequestAborted);

        return Ok(mapper.Map<ChatRoomViewModel>(exist));
    }

    [HttpGet("user/{userId:long}")]
    public async ValueTask<IActionResult> GetAll(
        [FromRoute] long userId,
        [FromQuery] PaginationParameters pagination,
        [FromQuery] SortingParameters sorting,
        [FromQuery] string? search = null
        )
    {
        var entities = await chatRoomService.GetByUserIdAsync(userId, ["FirstUser", "SecondUser", "LastMessage"]);

        return Ok(mapper.Map<IEnumerable<ChatRoomViewModel>>(entities));
    }

    [HttpGet("{firstUserId:long}/{secondUserId:long}")]
    public async ValueTask<IActionResult> GetByUsersId([FromRoute] long firstUserId, [FromRoute] long secondUserId)
    {
        var entities = await chatRoomService.GetByUsersIdAsync(firstUserId, secondUserId, ["FirstUser", "SecondUser", "LastMessage"]);

        return Ok(mapper.Map<ChatRoomViewModel>(entities));
    }

    [HttpPost]
    public async ValueTask<IActionResult> Post([FromBody] CreateChatRoomModel model)
    {
        await createValidator.EnsureValidationAsync(model);
        var created = await chatRoomService.CreateAsync(mapper.Map<ChatRoom>(model));

        return Ok(mapper.Map<ChatRoomViewModel>(created));
    }

    [HttpPut]
    public async ValueTask<IActionResult> Put([FromBody] UpdateChatRomModel model)
    {
        await updateValidator.EnsureValidationAsync(model);
        var created = await chatRoomService.UpdateAsync(mapper.Map<ChatRoom>(model));

        return Ok(mapper.Map<ChatRoomViewModel>(created));
    }

    [HttpDelete("{id:long}")]
    public async ValueTask<IActionResult> Delete([FromRoute] long id)
    {
        return Ok(await chatRoomService.DeleteAsync(id));
    }
}
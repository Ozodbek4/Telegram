using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Telegram.Application.Services;
using Telegram.Domain.Entities;
using Telegram.WebUI.Models.ChatRooms;

namespace Telegram.WebUI.Controllers;

public class ChatRoomController(IChatRoomService chatRoomService, IMapper mapper) : BaseController
{
    [HttpGet("{id:long}")]
    public async ValueTask<IActionResult> Get([FromRoute] long id)
    {
        var exist = await chatRoomService.GetByIdAsync(id, ["FirstUser", "SecondUser", "LastMessage"], cancellationToken: HttpContext.RequestAborted);

        return Ok(mapper.Map<ChatRoomViewModel>(exist));
    }

    [HttpGet("user/{userId:long}")]
    public async ValueTask<IActionResult> GetAll(long userId)
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
        var created = await chatRoomService.CreateAsync(mapper.Map<ChatRoom>(model));

        return Ok(mapper.Map<ChatRoomViewModel>(created));
    }

    [HttpPut]
    public async ValueTask<IActionResult> Put([FromBody] UpdateChatRomModel model)
    {
        var created = await chatRoomService.UpdateAsync(mapper.Map<ChatRoom>(model));

        return Ok(mapper.Map<ChatRoomViewModel>(created));
    }

    [HttpDelete("{id:long}")]
    public async ValueTask<IActionResult> Delete([FromRoute] long id)
    {
        return Ok(await chatRoomService.DeleteAsync(id));
    }
}
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Telegram.Application.Services;
using Telegram.Domain.Entities;
using Telegram.WebUI.Models.Messages;

namespace Telegram.WebUI.Controllers;

public class MessageController(IMessageService messageService, IMapper mapper) : BaseController
{
    [HttpGet("{id:long}")]
    public async ValueTask<IActionResult> Get([FromRoute] long id)
    {
        var exist = await messageService.GetByIdAsync(id, ["Sender", "Receiver"]);

        return Ok(mapper.Map<MessageViewModel>(exist));
    }

    [HttpGet("chat-room/{id:long}")]
    public async ValueTask<IActionResult> GetByChatRoomId([FromRoute] long id)
    {
        var exist = await messageService.GetByChatRoomIdAsync(id, ["Sender", "Receiver"]);

        return Ok(mapper.Map<IEnumerable<MessageViewModel>>(exist));
    }

    [HttpPost]
    public async ValueTask<IActionResult> Post([FromBody] CreateMessageModel model)
    {
        var exist = await messageService.CreateAsync(mapper.Map<Message>(model), HttpContext.RequestAborted);

        return Ok(mapper.Map<MessageViewModel>(exist));
    }

    [HttpPut("{id:long}")]
    public async ValueTask<IActionResult> Put([FromRoute] long id, UpdateMessageModel model)
    {
        model.Id = id;
        var exist = await messageService.UpdateAsync(mapper.Map<Message>(model), HttpContext.RequestAborted);

        return Ok(mapper.Map<MessageViewModel>(exist));
    }

    [HttpDelete("{id:long}")]
    public async ValueTask<IActionResult> Delete([FromRoute] long id)
    {
        var exist = await messageService.DeleteAsync(id, HttpContext.RequestAborted);

        return Ok(exist);
    }
}
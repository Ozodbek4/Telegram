using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Telegram.Api.Models.Dtos;
using Telegram.Application.Services;
using Telegram.Domain.Entities;

namespace Telegram.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class MessageController(IMessageService messageService, IChatService chatService, IChatOrchestrationService chatOrchestrationService, IMapper mapper) : ControllerBase
{
    [HttpGet("{secondUserId:guid}")]
    public async ValueTask<IActionResult> Get(Guid secondUserId)
    {
        var firstUserId = GetRequestUserId();
        var chat = await chatService.GetByUsersIdAsync(firstUserId, secondUserId, true, HttpContext.RequestAborted);

        if (chat is null)
            return NotFound();

        if (chat.FirstUserId == firstUserId)
            chat.FirstUserUnReadMessageCount = 0;
        if (chat.SecondUserId == firstUserId)
            chat.SecondUserUnReadMessageCount = 0;
        await chatService.UpdateAsync(chat);

        var messages = await messageService.GetByUsersIdAsync(firstUserId, secondUserId, true, HttpContext.RequestAborted);
        var result = new List<MessageDto>();

        messages.ToList().ForEach(mes =>
        {
            result.Add(mapper.Map<MessageDto>(mes));
        });

        return Ok(result);
    }

    [HttpPost]
    public async ValueTask<IActionResult> Create(Guid receiverId, string body)
    {
        var result = await chatOrchestrationService
            .SaveMessageToChatAsync(new Message { SenderId = GetRequestUserId(), ReceiverId = receiverId, Body = body }, true, HttpContext.RequestAborted);

        return Ok(mapper.Map<MessageDto>(result));
    }

    [HttpPut("{id:guid}")]
    public async ValueTask<IActionResult> Update([FromRoute] Guid id, [FromBody] string body)
    {
        var message = await messageService.GetByIdAsync(id, GetRequestUserId(), true, HttpContext.RequestAborted)
            ?? throw new ArgumentNullException();
        message.Body = body;

        await messageService.UpdateAsync(message);

        return Ok(mapper.Map<MessageDto>(message));
    }

    [HttpDelete("{id:guid}")]
    public async ValueTask<IActionResult> Delete([FromRoute] Guid id)
    {
        var result = await messageService.DeleteByIdAsync(id, true, HttpContext.RequestAborted);

        return result is not null ? Ok() : NotFound();
    }

    private Guid GetRequestUserId() => Guid.Parse(HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == "UserId")!.Value);
}
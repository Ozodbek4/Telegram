using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Telegram.Api.Models.Dtos;
using Telegram.Application.Services;
using Telegram.Domain.Entities;

namespace Telegram.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ChatController(IMessageService messageService, IChatService chatService, IChatOrchestrationService chatOrchestrationService, IMapper mapper)
    : ControllerBase
{
    [HttpGet("chats")]
    public async ValueTask<IActionResult> GetAllByUserId()
    {
        var chats = await chatService.GetByUserIdAsync(GetRequestUserId(), true, HttpContext.RequestAborted);
        var result = new List<ChatDto>();
        
        chats.ToList().ForEach(chat =>
        {
            result.Add(mapper.Map<ChatDto>(chat));
        });

        return Ok(result);
    }

    [HttpGet("chatMessages")]
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

    [HttpPost("{secondUserId:guid}")]
    public async ValueTask<IActionResult> Create(Guid secondUserId)
    {
        var chat = await chatService.CreateAsync(GetRequestUserId(), secondUserId, true, HttpContext.RequestAborted);

        return Ok(mapper.Map<ChatDto>(chat));
    }

    [HttpDelete("{id:guid}")]
    public async ValueTask<IActionResult> Delete([FromRoute] Guid id)
    {
        var result = await chatService.DeleteByIdAsync(id);

        return result is  not null ? Ok() : NotFound();
    }

    [HttpPost("message")]
    public async ValueTask<IActionResult> CreateMessage([FromBody] MessageDto message)
    {
        var result = await chatOrchestrationService.SaveMessageToChatAsync(mapper.Map<Message>(message), true, HttpContext.RequestAborted);

        return Ok(mapper.Map<MessageDto>(result));
    }

    private Guid GetRequestUserId() => Guid.Parse(HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == "UserId")!.Value);
}
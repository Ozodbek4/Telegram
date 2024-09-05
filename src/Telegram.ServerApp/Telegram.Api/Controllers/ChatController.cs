using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Telegram.Api.Models.Dtos;
using Telegram.Application.Services;

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

        return result is not null ? Ok() : NotFound();
    }

    private Guid GetRequestUserId() => Guid.Parse(HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == "UserId")!.Value);
}
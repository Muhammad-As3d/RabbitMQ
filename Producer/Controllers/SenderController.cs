using Microsoft.AspNetCore.Mvc;
using Producer.Contracts;
using Producer.Services;

namespace Producer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SenderController(IMessageProducer messageProducer) : ControllerBase
{
    [HttpPost("")]
    public async Task<IActionResult> SendMessage([FromBody] MessageRequest request)
    {
        try
        {
            await messageProducer.SendingMessageAsync(request);
            return Ok("send message successfully");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}

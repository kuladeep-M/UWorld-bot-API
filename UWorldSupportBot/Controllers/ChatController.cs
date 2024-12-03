using ChatHubSignalR.Models;
using ChatHubSignalR.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace UWorldSupportBot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly ChatContext _context;

        public ChatController(ChatContext context)
        {
            _context = context;

        }

        [HttpPost("messages")]
        public async Task<IActionResult> PostMessage(ChatMessage message)
        {
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quarter.Core.Dto;
using Quarter.Core.Entites;
using Quarter.Core.Entities.Identity;
using Quarter.Repostory.Identity.Contexts; // أو اللي فيه AppDbContext
using System;
using System.Linq;
using System.Threading.Tasks;

namespace QuarterEstate.APIS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly StoreIdentityDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public ChatController(StoreIdentityDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] ChatMessageDto dto)
        {
            var message = new ChatMessage
            {
                SenderId = dto.SenderId,
                ReceiverId = dto.ReceiverId,
                Content = dto.Content,
                Timestamp = DateTime.UtcNow,
                IsRead = false
            };

            _context.ChatMessages.Add(message);
            await _context.SaveChangesAsync();

            return Ok("Message sent.");
        }

        [HttpGet("history")]
        public async Task<IActionResult> GetMessages(string user1Id, string user2Id)
        {
            var messages = await _context.ChatMessages
                .Where(m => (m.SenderId == user1Id && m.ReceiverId == user2Id) ||
                            (m.SenderId == user2Id && m.ReceiverId == user1Id))
                .OrderBy(m => m.Timestamp)
                .ToListAsync();

            return Ok(messages);
        }
    }
}

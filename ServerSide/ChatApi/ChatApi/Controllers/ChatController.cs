using ChatApi.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatApi.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {

        private readonly ChatDbContext _context;

        public ChatController(ChatDbContext context) {
        
            _context = context;
        }

        [HttpGet("send")]
        [Authorize]
        public IActionResult SendMessage() {


            var username = HttpContext.User.Identity.Name;

            return Ok(new { message =  $"Your username: {username}"});
        }
    }
}

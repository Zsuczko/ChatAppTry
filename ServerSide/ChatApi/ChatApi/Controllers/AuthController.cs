using ChatApi.Data;
using Microsoft.AspNetCore.Mvc;

namespace ChatApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {

        private readonly ChatDbContext _context;

        public AuthController(ChatDbContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginReq req) {
            return null;
        }
    }


    public class LoginReq { 
    
        public string Username { get; set; }
        public string Password { get; set; }
    }
}

using ChatApi.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    public class WsController : ControllerBase
    {

        private readonly ChatDbContext _context;

        public WsController(ChatDbContext context) {
        
            _context = context;
        }

        [HttpGet("connect")]
        public async Task Connect() {

            if (!HttpContext.WebSockets.IsWebSocketRequest) {
                HttpContext.Response.StatusCode = 400;
                return;
            
            }

            using var websockets = await HttpContext.WebSockets.AcceptWebSocketAsync();

            //var username = HttpContext.User.Identity.Name;

            Console.WriteLine("Jo");


          
        }
    }
}

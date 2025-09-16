using System.Threading.Tasks;
using ChatApi.Data;
using ChatApi.Models;
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

        //[HttpGet("send")]
        //[Authorize]
        //public IActionResult SendMessage() {


        //    var username = HttpContext.User.Identity.Name;

        //    return Ok(new { message =  $"Your username: {username}"});
        //}

        [HttpGet("history")]
        [Authorize]
        public IActionResult History([FromQuery]string username) {


            var YourName = HttpContext.User.Identity.Name;

            var history = _context.Chats.Where(x=> x.Sender.Userame == YourName && x.Receiver.Userame == username)
                                        .OrderBy(chat => chat.TimeStamp)
                                        .Select(y => new {y.Message, y.TimeStamp });

            return Ok(new { history });
        }

        [HttpPost("send")]
        [Authorize]
        public async Task<IActionResult> SendMessage(SendClass send) {


            try {

                User receiver = _context.Users.FirstOrDefault(x => x.Userame == send.username);

                string senderUsername = HttpContext.User.Identity.Name;

                User sender = _context.Users.FirstOrDefault(x => x.Userame == senderUsername);

                Chat chat = new Chat
                {
                    Sender = sender,
                    Receiver = receiver,
                    Message = send.message
                };

                _context.Chats.Add(chat);

                await _context.SaveChangesAsync();

                return Ok(new {message = "Everything went right" });
            }
            catch (Exception ex) {
                return StatusCode(500, new { message = "Something went wrong", error = ex});
            }

           
        }
    }

    public class SendClass {

        public string username { get; set; }
        public string message { get; set; }

    }
}

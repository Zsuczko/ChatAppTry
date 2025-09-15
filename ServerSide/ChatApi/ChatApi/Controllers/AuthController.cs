using ChatApi.Data;
using ChatApi.Models;
using ChatApi.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ChatApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {

        private readonly ChatDbContext _context;
        private readonly JWTService _jwt;

        public AuthController(ChatDbContext context, JWTService jwt)
        {
            _context = context;
            _jwt = jwt;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginReq req) {
            
            var user = _context.Users.FirstOrDefault(x => x.Userame == req.Username && x.Password == req.Password);

            if (user == null)
            {
                return Unauthorized("Wrong username or password!");
                
            }

            var token = _jwt.GenerateToken(req.Username);

            return Ok(new {token});
        }


        [HttpPost("sign-up")]
        public async Task<IActionResult> SignUp([FromBody] LoginReq req) {


            if (string.IsNullOrWhiteSpace(req.Username) || string.IsNullOrWhiteSpace(req.Password)) { 
            
                return BadRequest(new {message= "Username and password are required" });
            }

            if (_context.Users.Any(x=>x.Userame == req.Username)) {

                return Conflict(new {message = "Username already exits" });
            }

            try {

                var user = new User
                {

                    Userame = req.Username,
                    Password = req.Password
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();



                return Ok(new { message = "User successfully created" });

            }

            catch (Exception ex) {

                return StatusCode(500, new {message = "Something went wrong", error = ex });
            }

       
        }
    }


    public class LoginReq { 
    
        public string Username { get; set; }
        public string Password { get; set; }
    }
}

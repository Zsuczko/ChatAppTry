using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using ChatApi.Data;
using ChatApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ChatApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    public class WsController : ControllerBase
    {

        private readonly ChatDbContext _context;

        private static readonly ConcurrentDictionary<string, WebSocket> _sockets = new();

        //private string username;

        public WsController(ChatDbContext context) {
        
            _context = context;
        }

        [HttpGet("connect")]
        public async Task Connect() {

            if (!HttpContext.WebSockets.IsWebSocketRequest) {
                HttpContext.Response.StatusCode = 400;
                return;
            
            }

            var authHeader = HttpContext.Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                HttpContext.Response.StatusCode = 401;
                return;
            }

            var token = authHeader.Substring("Bearer ".Length).Trim();

            var principal = ValidateToken(token, out var username);
            if (principal == null)
            {
                HttpContext.Response.StatusCode = 401;
                return;
            }


            using var websockets = await HttpContext.WebSockets.AcceptWebSocketAsync();

            _sockets[username] = websockets;



            var buffer = new byte[1024 * 4];

            foreach (var usernames in _sockets.Keys) {

                _sockets.TryGetValue(usernames, out var receiverSocket);

                var users = _sockets.Keys.Where(x=>x != usernames).ToList();

                string json = JsonSerializer.Serialize(users);

                var bytes43 = Encoding.UTF8.GetBytes(json);

                await receiverSocket.SendAsync(new ArraySegment<byte>(bytes43), WebSocketMessageType.Text, true, CancellationToken.None);
            }

            while (websockets.State == WebSocketState.Open)
            {

                var result = await websockets.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Close) {

                    _sockets.TryRemove(username, out _);
                    await websockets.CloseAsync(WebSocketCloseStatus.NormalClosure, "Bye", CancellationToken.None);
                    Console.WriteLine("vami");
                    break;
                }

                var receivedText = Encoding.UTF8.GetString(buffer, 0, result.Count);
                var JsonMessage = JsonSerializer.Deserialize<SendClass>(receivedText, new JsonSerializerOptions { 
                    PropertyNameCaseInsensitive = true
                });


                try {


                    User receiver = _context.Users.FirstOrDefault(x => x.Userame == JsonMessage.username);


                    User sender = _context.Users.FirstOrDefault(x => x.Userame == username);

                    if (receiver == null || sender == null)
                        continue;


                    Chat chat = new Chat
                    {
                        Sender = sender,
                        Receiver = receiver,
                        Message = JsonMessage.message
                    };

                    _context.Chats.Add(chat);

                    await _context.SaveChangesAsync();
                    var bytes12 = Encoding.UTF8.GetBytes("Nagyon jó");
                    await websockets.SendAsync(new ArraySegment<byte>(bytes12), WebSocketMessageType.Text, true, CancellationToken.None);

                    if (_sockets.TryGetValue(JsonMessage.username, out var receiverSocket) &&
                        receiverSocket.State == WebSocketState.Open)
                    {

                        var options = new JsonSerializerOptions
                        {
                            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                        };

                        var payload = JsonSerializer.Serialize(new
                        {
                            from = username,
                            message = JsonMessage.message
                        }, options);

                        var bytes = Encoding.UTF8.GetBytes(payload);
                        await receiverSocket.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
                    }
                }
                catch {
                
                }

            }




        }


        private ClaimsPrincipal? ValidateToken(string token, out string username)
        {
            username = null;
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes("my-super-secret-key111111111111111111111111111111111");

            try
            {
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "localhost",
                    ValidAudience = "localhost",
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                username = principal.Identity?.Name;
                return principal;
            }
            catch (Exception ex)
            {
              
                return null;
            }
        }
    }
}

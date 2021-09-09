using System.Threading.Tasks;
using Domain;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Server;
using ServerAdmin.Models;

namespace ServerAdmin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UsersController : Controller
    {
        private readonly IConfiguration _config;
        private Greeter.GreeterClient client;

        public UsersController(IConfiguration config)
        {
            _config = config;
            Channel channel = new Channel(_config.GetSection("GRPCUrl").Value, ChannelCredentials.Insecure);
            client = new Greeter.GreeterClient(channel);
        }


        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] UserModel newUser)
        {
            if (newUser == null)
            {
                return BadRequest("Usuario es requerido.");
            }

            var user = new AddUserRequest
            {
                Username = newUser.Username,
                Password = newUser.Password
            };
            var response = await client.AddUserAsync(user);
            return Ok(response.Message);
        }
        [HttpPut("{username}")]

        public async Task<IActionResult> PutAsync(string username, [FromBody] UserModel updateUser)
        {
            if (updateUser == null)
            {
                return BadRequest("Usuario es requerido.");
            }

            var user = new UpdateUserRequest
            {
                OldUsername = username,
                Username = updateUser.Username,
                Password = updateUser.Password
            };
            var response = await client.UpdateUserAsync(user);
            return Ok(response.Message);
        }
        [HttpDelete("{username}")]
        public async Task<IActionResult> DeleteAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return BadRequest("Usuario es requerido.");
            }

            var user = new Username
            {        
                Username_ = username
            };
            var response = await client.DeleteUserAsync(user);
            return Ok(response.Message);
        }
    }
    
}
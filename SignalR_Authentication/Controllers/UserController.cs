using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SignalR_Authentication.Interfaces;
using SignalR_Authentication.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace SignalR_Authentication.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : Controller
    {
        private DataContext context;
        private ITokenService tokenService;

        public UserController(DataContext context, ITokenService tokenService)
        {
            this.context = context;
            this.tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginData data)
        {
            User user;

            user = await context.Users.FirstAsync(u => u.Name == data.Name && u.Password == data.Password);
            string token = tokenService
                    .GenerateToken(new LoginModel() { Name = user.Name, Email = user.Email, Role = user.Role });

            Console.WriteLine(token);

            HttpContext.Session.SetString("token", token);

            var answer = new
            {
                name = user.Name,
                token = token
            };

            return Ok(answer);
        }
    }
}

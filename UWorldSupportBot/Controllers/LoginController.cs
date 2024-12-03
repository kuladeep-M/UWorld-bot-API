using ChatHubSignalR.Models;
using ChatHubSignalR.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace UWorldSupportBot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ChatContext _chatContext;
        private readonly IConfiguration _configuration;

        public LoginController(ChatContext chatContext,IConfiguration configuration)
        {
            _chatContext = chatContext;
            _configuration = configuration;
        }
        [HttpPost()]
        public async Task<ActionResult<string>> Login([FromBody]LoginModel model)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            User user = await _chatContext.Users.SingleOrDefaultAsync(x => (x.Username == model.UserName && x.Password == model.Password));
            // Authenticate the user (replace with your authentication logic)
            if (user != null)
            {

                return Ok(await GenerateJwtToken(user));
            }
            else
            {
                return Unauthorized("Invalid credentials");
            }
        }




        private async Task<string> GenerateJwtToken(User user)
        {
            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Username.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };


            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
                
        }
    }
}

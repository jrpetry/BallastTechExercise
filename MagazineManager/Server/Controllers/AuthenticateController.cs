using MagazineManager.Server.Controllers.BLL;
using MagazineManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MagazineManager.Server.Data.Repositories.Abstraction;
using Microsoft.AspNetCore.SignalR;

namespace MagazineManager.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationUserBLL _applicationUserBLL;

        public AuthenticateController(IConfiguration configuration, BaseRepository<ApplicationUser> repo)
        {
            _configuration = configuration;
            _applicationUserBLL = new ApplicationUserBLL(repo);
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginAsync([FromBody] ApplicationUser user)
        {
            var checkedUser = _applicationUserBLL.Login(user);

            if (checkedUser != null)
            {
                var authClaims = new List<Claim>
                {
                    new (ClaimTypes.Name, checkedUser.UserName),
                    new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                authClaims.Add(new(ClaimTypes.Role, checkedUser.Role));

                var token = GetToken(authClaims, checkedUser);
                return Ok(token);
            }

            return Unauthorized(new { message= "Unauthorized" });
        }

        private TokenModel GetToken(List<Claim> authClaims, ApplicationUser applicationUser)
        {
            var authSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(1), //1 hour valid
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return new()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                ValidTo = token.ValidTo ,
                UserName = applicationUser.UserName,
                Role = applicationUser.Role,
                Id = applicationUser.Id
            };

        }
    }
}

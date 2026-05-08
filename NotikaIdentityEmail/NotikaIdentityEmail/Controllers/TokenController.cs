using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NotikaIdentityEmail.Models.JwtModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NotikaIdentityEmail.Controllers
{
    public class TokenController : Controller
    {
        private readonly JwtSettingModels _jwtSettingModels;

        public TokenController(IOptions<JwtSettingModels> jwtSettingModels)
        {
            _jwtSettingModels = jwtSettingModels.Value;
        }

        public IActionResult Generate()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Generate(SimpleUserViewModels simpleUserViewModel)
        {
            var claim = new[]
            {
                new Claim("name", simpleUserViewModel.Name),
                new Claim("surname", simpleUserViewModel.Surname),
                new Claim("city", simpleUserViewModel.City),
                new Claim("username", simpleUserViewModel.Username),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettingModels.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettingModels.Issuer,
                audience: _jwtSettingModels.Audience,
                claims: claim,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettingModels.ExpireMinutes),
                signingCredentials: creds);

            simpleUserViewModel.Token = new JwtSecurityTokenHandler().WriteToken(token);
            return View(simpleUserViewModel);
        }
    }
}

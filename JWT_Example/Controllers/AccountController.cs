using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using JWT_Example.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace JWT_Example.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private List<User> users = new List<User>
        {
            new User {Login="admin@gmail.com", Password="12345", Role = "admin" },
            new User { Login="qwerty@gmail.com", Password="55555", Role = "user" }
        };

        [HttpGet]
        public string Get()
        {
            return "Get";
        }


        [HttpGet]
        [Route("Test")]
        public string Test()
        {
            return "test";
        }



        [HttpPost]
        [Route("Auth")]
        public IActionResult Auth(AuthRequest auth)
        //public IActionResult Auth(string login, string password)
        {
            
            var claims = GetClaims(auth.Login, auth.Password);
            //var claims = GetClaims(login, password);
            if (claims == null)
            {
                return BadRequest(new { errorText = "Invalid username or password." });
            }

            var currentTime = DateTime.UtcNow;

            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                notBefore: currentTime,
                claims: claims.Claims,
                expires: currentTime.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var user = new UserDTO
            {
                Login = claims.NameClaimType,
                Role = claims.RoleClaimType,
                Token = encodedJwt
            };

            return Ok(user);
        }

        private ClaimsIdentity GetClaims(string login, string password)
        {

            var user = users.FirstOrDefault(u => u.Login == login && u.Password == password);

            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role)
                };

                return new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
            }

            return null;
        }
    }
}
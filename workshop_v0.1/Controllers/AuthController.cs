using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using workshop_v0._1.DAL;
using workshop_v0._1.Models;

namespace workshop_v0._1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("MyPolicy")]
    public class AuthController : ControllerBase
    {
        UserLoginDataContext _context;
        UserContext _userContext;

        public static UserLoginData user = new UserLoginData();
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration, UserLoginDataContext context, UserContext userContext)
        {
            _configuration = configuration;
            _context = context;
            _userContext = userContext;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserLoginData>> Register(UserDto request)
        {
            CreatePasswordHash(request.userPassword, out byte[] passwordHash, out byte[] passwordSalt);
            
            //add check for existing username

            User parentUser = new User();
            UserLoginData loginData = new UserLoginData();

            loginData.username = request.userLogin;
            loginData.userPassword = passwordHash;
            loginData.userSalt = passwordSalt;

            parentUser.name = request.name;
            parentUser.surname = request.surname;


            parentUser.creds = new HashSet<UserLoginData>();
            parentUser.creds.Add(loginData);

            _userContext.User.Add(parentUser);
            await _userContext.SaveChangesAsync();

            return Ok(parentUser);
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserDto request)
        {
            UserLoginData tmpUser = await _context.UserLoginData.FirstOrDefaultAsync(x => x.username == request.userLogin);

            if (tmpUser.username != request.userLogin)
            {
                return BadRequest("Wrong credentials");
            }

            if(!VerifyPasswordHash(request.userPassword, tmpUser.userPassword, tmpUser.userSalt))
            {
                return BadRequest("Wrong credentials 2");
            }

            string token = CreateToken(tmpUser);

            var refreshToken = GenerateRefreshToken();
            SetRefreshToken(refreshToken);
            tmpUser.RefreshToken = refreshToken.Token;
            tmpUser.TokenCreated = refreshToken.Created;
            tmpUser.TokenExpires = refreshToken.Expires;

            await _context.SaveChangesAsync();

            return Ok(token);
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<string>> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if (!user.RefreshToken.Equals(refreshToken))
            {
                return Unauthorized("Invalid refresh token");
            }
            else if (user.TokenExpires < DateTime.Now)
            {
                return Unauthorized("Token expired");
            }

            string token = CreateToken(user);
            var newRefreshToken = GenerateRefreshToken();
            SetRefreshToken(newRefreshToken);

            return Ok(token);
        }

        private RefreshToken GenerateRefreshToken()
        {
            byte[] intBytes = new byte[64];
            RandomNumberGenerator rng = new RNGCryptoServiceProvider();
            rng.GetBytes(intBytes);
            
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(intBytes),
                Expires = DateTime.Now.AddDays(7),
                Created = DateTime.Now
            };

            return refreshToken;
        }

        private void SetRefreshToken(RefreshToken newRefreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = newRefreshToken.Expires
            };
            Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);

            user.RefreshToken = newRefreshToken.Token;
            user.TokenCreated = newRefreshToken.Created;
            user.TokenExpires = newRefreshToken.Expires;
        }

        private string CreateToken(UserLoginData user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.username),
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim(ClaimTypes.NameIdentifier, user.id_user.ToString())
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("CredentialsSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims : claims,
                expires : DateTime.Now.AddDays(7),
                signingCredentials : creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }


        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using(var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using(var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

    }
}

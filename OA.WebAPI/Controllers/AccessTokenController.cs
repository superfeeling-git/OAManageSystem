using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OA.WebAPI.Auth;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OA.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [AllowAnonymous]
    public class AccessTokenController : ControllerBase
    {
        [HttpPost]
        public IActionResult GetToken()
        {
            var user = new User()
            {
                Id = "D21D099B-B49B-4604-A247-71B0518A0B1C",
                UserName = "Jeffcky",
                Email = "2752154844@qq.com"
            };


            var refreshToken = GenerateRefreshToken();

            user.CreateRefreshToken(refreshToken, user.Id);


            //定义许多种的声明Claim,信息存储部分,Claims的实体一般包含用户和一些元数据
            IEnumerable<Claim> claims = new Claim[]
            {
                    new Claim(ClaimTypes.Name,"Jeffcky"),
                    new Claim(ClaimTypes.Role,"RoleAdmin"),
                    new Claim(JwtClaimTypes.Id,"D21D099B-B49B-4604-A247-71B0518A0B1C"),
                    new Claim(JwtClaimTypes.Name,"Jeffcky"),
                    new Claim(JwtClaimTypes.Email,"2752154844@qq.com"),
                    new Claim(JwtClaimTypes.Subject, "D21D099B-B49B-4604-A247-71B0518A0B1C"),
                    new Claim(JwtClaimTypes.Role, "RoleAdmin"),
                    new Claim(JwtClaimTypes.NickName, "feeling")
            };

            return Ok(new {
                Bearer = "Bearer ",
                accessToken = $"Bearer {GenerateAccessToken(claims.ToArray())}",
                refreshToken = refreshToken
            });
        }

        /// <summary>
        /// 生成访问令牌
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        public string GenerateAccessToken(Claim[] claims)
        {
            //定义发行人issuer
            string iss = "JWTBearer.Auth";
            //定义受众人audience
            string aud = "api.auth";


            var nbf = DateTime.UtcNow;
            var Exp = DateTime.UtcNow.AddSeconds(1000);
            string sign = "q2xiARx$4x3TKqBJ"; //SecurityKey 的长度必须 大于等于 16个字符
            var secret = Encoding.UTF8.GetBytes(sign);
            var key = new SymmetricSecurityKey(secret);
            var signcreds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


            var jwt = new JwtSecurityToken(
                issuer: iss,
                audience: aud,
                claims: claims,
                notBefore: nbf,
                expires: Exp,
                signingCredentials: signcreds);

            var JwtHander = new JwtSecurityTokenHandler();
            var token = JwtHander.WriteToken(jwt);

            return token;
        }

        /// <summary>
        /// 生成刷新Token
        /// </summary>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        /// <summary>
        /// 从Token中获取用户身份
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        public ClaimsPrincipal GetPrincipalFromAccessToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();

            try
            {
                return handler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("q2xiARx$4x3TKqBJ")),
                    ValidateLifetime = false
                }, out SecurityToken validatedToken);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}

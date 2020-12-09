using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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
        [HttpGet]
        public IActionResult GetToken()
        {
            try
            {
                //定义发行人issuer
                string iss = "JWTBearer.Auth";
                //定义受众人audience
                string aud = "api.auth";

                //定义许多种的声明Claim,信息存储部分,Claims的实体一般包含用户和一些元数据
                IEnumerable<Claim> claims = new Claim[]
                {
                    new Claim(ClaimTypes.Name,"i3abced"),
                    new Claim(ClaimTypes.Role,"RoleAdmin"),
                    new Claim(JwtClaimTypes.Id,"1"),
                    new Claim(JwtClaimTypes.Name,"i3yuan"),
                    new Claim(JwtClaimTypes.Role, "RoleAdmin"),
                    new Claim(JwtClaimTypes.NickName, "feeling")
                };



                //notBefore  生效时间
                // long nbf =new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds();
                var nbf = DateTime.UtcNow;
                //expires   //过期时间
                // long Exp = new DateTimeOffset(DateTime.Now.AddSeconds(1000)).ToUnixTimeSeconds();
                var Exp = DateTime.UtcNow.AddSeconds(1000);
                //signingCredentials  签名凭证
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
                return Ok(new
                {
                    access_token = token,
                    token_type = "Bearer",
                });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 生成访问令牌
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        public string GenerateAccessToken(Claim[] claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("q2xiARx$4x3TKqBJ"));

            var token = new JwtSecurityToken(
                issuer: "JWTBearer.Auth",
                audience: "api.auth",
                claims: claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddMinutes(1),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// 生成刷新Token
        /// </summary>
        /// <returns></returns>
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
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey)),
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

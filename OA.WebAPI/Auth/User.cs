using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OA.WebAPI.Auth
{
    public class User
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }

        private readonly List<UserRefreshToken> _userRefreshTokens = new List<UserRefreshToken>();

        public IEnumerable<UserRefreshToken> UserRefreshTokens => _userRefreshTokens;

        /// <summary>
        /// 验证刷新token是否存在或过期
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        public bool IsValidRefreshToken(string refreshToken)
        {
            return _userRefreshTokens.Any(d => d.Token.Equals(refreshToken) && d.Active);
        }

        /// <summary>
        /// 创建刷新Token
        /// </summary>
        /// <param name="token"></param>
        /// <param name="userId"></param>
        /// <param name="minutes"></param>
        public void CreateRefreshToken(string token, string userId, double minutes = 1)
        {
            _userRefreshTokens.Add(new UserRefreshToken() { Token = token, UserId = userId, Expires = DateTime.Now.AddMinutes(minutes) });
        }

        /// <summary>
        /// 移除刷新token
        /// </summary>
        /// <param name="refreshToken"></param>
        public void RemoveRefreshToken(string refreshToken)
        {
            _userRefreshTokens.Remove(_userRefreshTokens.FirstOrDefault(t => t.Token == refreshToken));
        }
    }
}

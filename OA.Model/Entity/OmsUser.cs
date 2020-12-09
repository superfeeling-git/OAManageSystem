using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace OA.Model.Entity
{
    public class OmsUser : IdentityUser<long>
    {
        #region 扩展自定义属性
        public string Province { get; set; }
        public string City { get; set; }
        public string Area { get; set; }
        /// <summary>
        /// 刷新令牌
        /// </summary>
        public string RefreshToken { get; set; }
        #endregion
    }
}

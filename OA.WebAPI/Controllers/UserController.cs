using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OA.Model.Model;
using OA.Model.Entity;
using OA.Utility;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace OA.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private UserManager<OmsUser> _userManager;
        private SignInManager<OmsUser> _signInManager;
        private ILogger<UserController> _logger;

        public UserController(UserManager<OmsUser> userManager, SignInManager<OmsUser> signInManager, ILogger<UserController> logger)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._logger = logger;
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="omsUser"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Register(OmsUserModel omsUser)
        {
            if (ModelState.IsValid)
            {
                var user = omsUser.MapTo<OmsUser>();

                //不能在参数中进行映射，否则会报错
                //报错：_userManager.CreateAsync(omsUser.MapTo<OmsUser>()

                var result = await _userManager.CreateAsync(user, omsUser.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return Ok(true);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
            }

            foreach (var s in ModelState.Values)
            {
                foreach (var p in s.Errors)
                {
                    _logger.LogInformation(p.ErrorMessage);
                }
            }

            //输出错误消息
            var msg = ModelState.Values.SelectMany(m => m.Errors).Select(s => s.ErrorMessage);

            return Ok(msg);
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Login(OmsUserModel userModel)
        {
            if(ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(userName: userModel.UserName, password: userModel.Password, isPersistent: false, lockoutOnFailure: false);
                if(result.Succeeded)
                {
                    _logger.LogInformation("登录成功");
                    return Ok("登录成功");
                }
            }

            //输出错误消息
            var msg = ModelState.Values.SelectMany(m => m.Errors).Select(s => s.ErrorMessage);

            return Ok(msg);
        }

        /// <summary>
        /// 获取用户
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetUserAsync(string Email)
        {
            var result = await _userManager.FindByEmailAsync(Email);
            
            return Ok(result);
        }
    }
}

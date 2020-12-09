using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OA.IService;
using OA.Model.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Docs.Samples;

namespace OA.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Policy = "BlogController")]
    public class BlogController : ControllerBase
    {
        private IOmsBlogService OmsBlogService;

        public BlogController(IOmsBlogService _OmsBlogService)
        {
            this.OmsBlogService = _OmsBlogService;
        }

        /// <summary>
        /// 添加博客
        /// </summary>
        /// <returns></returns>
        
        [HttpPost("/api/[controller]/Add",Name = "tt")]
        public async Task<IActionResult> CreateAsync(OmsBlog omsBlog)
        {
            await OmsBlogService.CreateAsync(omsBlog);

            return Ok(nameof(Task<IActionResult>));
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Test()
        {
            return new JsonResult(new { student = "张三" });
        }
    }
}

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

            return Ok();
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> List()
        {
            return new JsonResult(await OmsBlogService.GetAllAsync());
        }

        [HttpGet("/api/test")]
        public IActionResult Test()
        {
            return Ok(Guid.NewGuid().ToString("N"));
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OA.IService;
using OA.Model.Entity;

namespace OA.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BlogCategoryController : ControllerBase
    {
        private IOmsBlogCategoryService omsBlogCategoryService;

        public BlogCategoryController(IOmsBlogCategoryService _omsBlogCategoryService)
        {
            omsBlogCategoryService = _omsBlogCategoryService;
        }

        /// <summary>
        /// 添加博客分类
        /// </summary>
        /// <param name="omsBlogCategory">实体类参数</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create(OmsBlogCategory omsBlogCategory)
        {
            await omsBlogCategoryService.CreateAsync(omsBlogCategory);
            return Ok();
        }
    }
}

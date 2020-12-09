using System;
using System.Collections.Generic;
using System.Text;
using OA.IService;
using OA.IRepository;
using OA.Model.Entity;
using System.Threading.Tasks;

namespace OA.Service
{
    public class OmsBlogCategoryService : Service<OmsBlogCategory, long>, IOmsBlogCategoryService
    {
        //注入仓储
        private IOmsBlogCategoryRepository OmsBlogCategoryRepository;

        public OmsBlogCategoryService(IOmsBlogCategoryRepository _OmsBlogCategoryRepository, IUnitOfWork _UnitOfWork)
            : base(_OmsBlogCategoryRepository, _UnitOfWork)
        {
            this.OmsBlogCategoryRepository = _OmsBlogCategoryRepository;
            this.UnitOfWork = _UnitOfWork;
        }

        /// <summary>
        /// 如果需要在服务端有新的业务处理，可以重写基类方法
        /// </summary>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public override Task CreateAsync(OmsBlogCategory Entity)
        {
            return base.CreateAsync(Entity);
        }

        /// <summary>
        /// 重写基类方法，采用工作单元方式进行一次性提交
        /// </summary>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public async override Task<int> Update(OmsBlogCategory Entity)
        {
            repository.Update(Entity);
            return await UnitOfWork.SaveChangesAsync();
        }
    }
}


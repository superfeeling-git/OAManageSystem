using System;
using System.Collections.Generic;
using System.Text;
using OA.Model.Entity;
using OA.IService;
using OA.IRepository;

namespace OA.Service
{
    public class OmsBlogService : Service<OmsBlog,long>,IOmsBlogService
    {
        //注入仓储
        private IOmsBlogRepository OmsBlogRepository;

        public OmsBlogService(IOmsBlogRepository _OmsBlogRepository, IUnitOfWork _UnitOfWork)
            : base(_OmsBlogRepository, _UnitOfWork)
        {
            this.OmsBlogRepository = _OmsBlogRepository;
            this.UnitOfWork = _UnitOfWork;
        }
    }
}

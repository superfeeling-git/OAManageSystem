using System;
using System.Collections.Generic;
using System.Text;
using OA.IRepository;
using OA.Model;
using OA.Model.Entity;

namespace OA.Repository
{
    public class OmsBlogCategoryRepository : BaseRepository<OmsBlogCategory,long>, IOmsBlogCategoryRepository
    {
        public OmsBlogCategoryRepository(OADbContext DbContext)
            :base(DbContext)
        {

        }
    }
}

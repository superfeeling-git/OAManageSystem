using OA.IRepository;
using OA.Model;
using OA.Model.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace OA.Repository
{
    public class OmsBlogRepository : BaseRepository<OmsBlog, long>, IOmsBlogRepository
    {
        public OmsBlogRepository(OADbContext oADbContext)
            :base(oADbContext)
        {

        }
    }
}

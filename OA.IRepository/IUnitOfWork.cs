using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OA.IRepository
{
    /// <summary>
    /// 工作单元
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// 提交所有更改
        /// </summary>
        Task<int> SaveChangesAsync();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using OA.IRepository;

namespace OA.IService
{
    public interface IService<TEntity, TKey>
        where TEntity : class, new()
        where TKey : struct
    {
        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="Entity"></param>
        /// <returns></returns>
        Task CreateAsync(TEntity Entity);

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="Entity"></param>
        Task<int> Update(TEntity Entity);

        /// <summary>
        /// 根据条件进行更新
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(Expression<Func<TEntity, bool>> whereLambda, Expression<Func<TEntity, TEntity>> entity);

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="Entity"></param>
        Task<int> DeletedAsync(TEntity Entity);

        /// <summary>
        /// 根据条件进行删除
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(Expression<Func<TEntity, bool>> whereLambda);

        /// <summary>
        /// 判断指定实体是否存在
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        Task<bool> IsExist(Expression<Func<TEntity, bool>> whereLambda);

        /// <summary>
        /// 根据条件表达式获取集合
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<List<TEntity>> FindByAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 根据条件表达式获取集合-延迟查询
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IQueryable<TEntity> FindQueryable(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 根据主键值返回单条实体
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        Task<TEntity> FindAsync(TKey ID);

        /// <summary>
        /// 根据条件表达式返回第一个对象
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 异步获取所有数据
        /// </summary>
        /// <returns></returns>
        Task<List<TEntity>> GetAllAsync();

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns></returns>
        List<TEntity> GetAll();

        /// <summary>
        /// 根据条件表示分页获取数据集合
        /// </summary>
        /// <param name="predicate">断言表达式</param>
        /// <param name="sortPredicate">排序断言</param>
        /// <param name="sortOrder">排序方式</param>
        /// <param name="skip">跳过序列中指定数量的元素，然后返回剩余的元素</param>
        /// <param name="take">从序列的开头返回指定数量的连续元素</param>
        /// <returns>item1：数据集合；item2：数据总数</returns>
        Task<Tuple<List<TEntity>, int>> GetAllAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, dynamic>> sortPredicate, SortOrder sortOrder, int skip, int take);
    }
}

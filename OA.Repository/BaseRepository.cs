using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OA.IRepository;
using OA.Model;
using Z.EntityFramework.Plus;

namespace OA.Repository
{
    public abstract class BaseRepository<TEntity, TKey> : IBaseRepository<TEntity, TKey>
        where TEntity : class, new()
        where TKey : struct
    {
        protected readonly OADbContext Context;

        protected BaseRepository(OADbContext _context)
        {
            this.Context = _context;
        }

        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public virtual async Task CreateAsync(TEntity Entity)
        {
            await Context.Set<TEntity>().AddAsync(Entity);
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public virtual void Update(TEntity Entity)
        {
            Context.Set<TEntity>().Update(Entity);
        }

        /// <summary>
        /// 根据条件进行更新(批量)
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async Task<int> UpdateAsync(Expression<Func<TEntity, bool>> whereLambda, Expression<Func<TEntity, TEntity>> entity)
        {
            return await Context.Set<TEntity>().Where(whereLambda).UpdateAsync(entity);
        }

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public virtual void Deleted(TEntity Entity)
        {
            Context.Entry<TEntity>(Entity).State = EntityState.Deleted;
        }

        /// <summary>
        /// 根据条件进行删除
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        public virtual async Task<int> DeleteAsync(Expression<Func<TEntity, bool>> whereLambda)
        {
            return await Context.Set<TEntity>().Where(whereLambda).DeleteAsync();
        }

        /// <summary>
        /// 判断指定实体是否存在
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        public virtual async Task<bool> IsExist(Expression<Func<TEntity, bool>> whereLambda)
        {
            return await Context.Set<TEntity>().AnyAsync(whereLambda);
        }

        /// <summary>
        /// 根据条件表达式获取集合
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual async Task<List<TEntity>> FindByAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Context.Set<TEntity>().Where(predicate).ToListAsync();
        }

        /// <summary>
        /// 根据条件表达式获取集合-延迟查询
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual IQueryable<TEntity> FindQueryable(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().Where(predicate);
        }

        /// <summary>
        /// 根据主键值返回单条实体
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public virtual async Task<TEntity> FindAsync(TKey ID)
        {
            return await Context.Set<TEntity>().FindAsync(ID);
        }

        /// <summary>
        /// 根据条件表达式返回第一个对象
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual async Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Context.Set<TEntity>().FirstOrDefaultAsync(predicate);
        }

        /// <summary>
        /// 异步获取所有数据
        /// </summary>
        /// <returns></returns>
        public virtual async Task<List<TEntity>> GetAllAsync()
        {
            return await Context.Set<TEntity>().ToListAsync();
        }

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns></returns>
        public virtual List<TEntity> GetAll()
        {
            return Context.Set<TEntity>().ToList();
        }

        /// <summary>
        /// 根据条件表示分页获取数据集合
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="sortPredicate"></param>
        /// <param name="sortOrder"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public virtual async Task<Tuple<List<TEntity>, int>> GetAllAsync(Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, dynamic>> sortPredicate, SortOrder sortOrder, int skip, int take)
        {
            var result = Context.Set<TEntity>().Where(predicate);
            var total = result.Count();
            switch (sortOrder)
            {

                case SortOrder.Ascending:
                    var resultAscPaged = await
                        Context.Set<TEntity>().Where(predicate)
                            .OrderBy(sortPredicate)
                            .Skip(skip)
                            .Take(take).ToListAsync();
                    return new Tuple<List<TEntity>, int>(resultAscPaged, total);


                case SortOrder.Descending:
                    var resultDescPaged = await
                        Context.Set<TEntity>().Where(predicate)
                            .OrderByDescending(sortPredicate)
                            .Skip(skip)
                            .Take(take).ToListAsync();
                    return new Tuple<List<TEntity>, int>(resultDescPaged, total);
            }
            throw new InvalidOperationException("基于分页功能的查询必须指定排序字段和排序顺序。");
        }
    }
}

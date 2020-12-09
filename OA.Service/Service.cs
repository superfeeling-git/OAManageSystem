using System;
using OA.Model.Entity;
using OA.IRepository;
using OA.IService;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Linq;

namespace OA.Service
{
    public abstract class Service<TEntity, TKey> : IService<TEntity, TKey>
        where TEntity : class, new()
        where TKey : struct
    {
        /// <summary>
        /// 可供工作单元使用
        /// </summary>
        protected IBaseRepository<TEntity, TKey> repository;

        protected IUnitOfWork UnitOfWork;

        public Service(IBaseRepository<TEntity, TKey> _repository, IUnitOfWork _UnitOfWork)
        {
            this.repository = _repository;
            this.UnitOfWork = _UnitOfWork;
        }

        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public virtual async Task CreateAsync(TEntity Entity)
        {
            await repository.CreateAsync(Entity);
            await UnitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// 根据条件进行删除
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        public virtual async Task<int> DeleteAsync(Expression<Func<TEntity, bool>> whereLambda)
        {
            return await repository.DeleteAsync(whereLambda);
        }

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="Entity"></param>
        public virtual async Task<int> DeletedAsync(TEntity Entity)
        {
            repository.Deleted(Entity);
            return await UnitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// 根据主键值返回单条实体
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public virtual async Task<TEntity> FindAsync(TKey ID)
        {
            return await repository.FindAsync(ID);
        }

        /// <summary>
        /// 根据条件表达式获取集合
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual async Task<List<TEntity>> FindByAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await repository.FindByAsync(predicate);
        }

        /// <summary>
        /// 根据条件表达式获取集合-延迟查询
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual IQueryable<TEntity> FindQueryable(Expression<Func<TEntity, bool>> predicate)
        {
            return repository.FindQueryable(predicate);
        }

        /// <summary>
        /// 根据条件表达式返回第一个对象
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual async Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await repository.FirstAsync(predicate);
        }

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns></returns>
        public virtual List<TEntity> GetAll()
        {
            return repository.GetAll();
        }

        /// <summary>
        /// 异步获取所有数据
        /// </summary>
        /// <returns></returns>
        public virtual async Task<List<TEntity>> GetAllAsync()
        {
            return await repository.GetAllAsync();
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
        public virtual async Task<Tuple<List<TEntity>, int>> GetAllAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, dynamic>> sortPredicate, SortOrder sortOrder, int skip, int take)
        {
            return await repository.GetAllAsync(predicate, sortPredicate, sortOrder, skip, take);
        }

        /// <summary>
        /// 判断指定实体是否存在
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        public virtual async Task<bool> IsExist(Expression<Func<TEntity, bool>> whereLambda)
        {
            return await repository.IsExist(whereLambda);
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="Entity"></param>
        public virtual async Task<int> Update(TEntity Entity)
        {
            repository.Update(Entity);
            return await UnitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// 根据条件进行更新
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async Task<int> UpdateAsync(Expression<Func<TEntity, bool>> whereLambda, Expression<Func<TEntity, TEntity>> entity)
        {
            return await repository.UpdateAsync(whereLambda, entity);
        }
    }
}

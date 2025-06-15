using System.Linq.Expressions;
using Be.Common.Responses;
using Be.Core.BaseEntities;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace Be.Data.Repository
{
    public interface IRepository
    {
        #region AddEntity
        Task AddAsync<TEntity, TKey>(TEntity entity) where TEntity : class, IEntity<TKey> where TKey : IEquatable<TKey>;
        Task AddAsync<TEntity>(TEntity entity) where TEntity : class, IEntity<long>;
        Task AddRangeAsync<TEntity, TKey>(IEnumerable<TEntity> entities) where TEntity : class, IEntity<TKey> where TKey : IEquatable<TKey>;
        Task AddRangeAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : class, IEntity<long>;
        #endregion

        #region Update

        Task UpdateAsync<TEntity>(TEntity entity) where TEntity : class, IEntity<long>;
        Task UpdateAsync<TEntity, TKey>(TEntity entity) where TEntity : class, IEntity<TKey> where TKey : IEquatable<TKey>;

        #endregion

        #region DeleteEntity
        Task DeleteAsync<TEntity>(TEntity entity) where TEntity : class, IEntity<long>;
        Task DeleteAsync<TEntity, TKey>(TEntity entity) where TEntity : class, IEntity<TKey> where TKey : IEquatable<TKey>;
        Task DeleteAsync<TEntity>(params object[] ids) where TEntity : class, IEntity<long>;
        Task DeleteAsync<TEntity, TKey>(params object[] ids) where TEntity : class, IEntity<TKey>
            where TKey : IEquatable<TKey>;
        
        Task DeleteRangeAsync<TEntity, TKey>(IEnumerable<object> ids) where TEntity : class, IEntity<TKey>
            where TKey : IEquatable<TKey>;

        Task DeleteRangeAsync<TEntity>(IEnumerable<object> ids) where TEntity : class, IEntity<long>;

        Task DeleteRangeAsync<TEntity, TKey>(IEnumerable<TEntity> entities) where TEntity : class, IEntity<TKey>
            where TKey : IEquatable<TKey>;

        Task DeleteRangeAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : class, IEntity<long>;
        #endregion

        Task SaveChangeAsync();

		#region GetEntity                   
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="query">Data queryable</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="selector">selector</param>
        /// <returns></returns>
        Task<PagedResult<TEntity>> FindPagedAsync<TEntity>(IQueryable<TEntity> query, int pageIndex = 1, int pageSize = 10, Expression<Func<TEntity, TEntity>> selector = null) where TEntity : class;

        IQueryable<TEntity> GetQueryable<TEntity>() where TEntity : class, IEntity<long>;
        IQueryable<TEntity> GetQueryable<TEntity, TKey>() where TEntity : class, IEntity<TKey> where TKey : IEquatable<TKey>;

        Task<TEntity> FindAsync<TEntity, TKey>(TKey id, IEnumerable<string> includes = null) where TEntity : class, IEntity<TKey> where TKey : IEquatable<TKey>;
        Task<TEntity> FindAsync<TEntity, TKey>(Expression<Func<TEntity, bool>> where = null, IEnumerable<string> includes = null) where TEntity : class, IEntity<TKey> where TKey : IEquatable<TKey>;
        Task<TEntity> FindAsync<TEntity, TKey>(params object[] ids) where TEntity : class, IEntity<TKey> where TKey : IEquatable<TKey>;
        Task<TEntity> FindAsync<TEntity>(params object[] ids) where TEntity : class, IEntity<long>;
        Task<TEntity> FindAsync<TEntity>(Expression<Func<TEntity, bool>> where = null, IEnumerable<string> includes = null) where TEntity : class, IEntity<long>;
        Task<TEntity> FindAsync<TEntity>(long id, IEnumerable<string> includes = null) where TEntity : class, IEntity<long>; 

        #endregion
	}
}

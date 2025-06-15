using System.Linq.Expressions;
using Azure.Core;
using Be.Common.Responses;
using Be.Core.BaseEntities;
using Be.Data.BaseApp;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

namespace Be.Data.Repository
{
    public class Repository<TContext> : IRepository where TContext : DbContext
    {
        protected readonly TContext DbContext;
        protected readonly ICurrentUserService CurrentUserService;
        protected readonly ILogger<IRepository> Logger;

        public Repository(TContext dbContext, ICurrentUserService currentUserService, ILogger<IRepository> logger)
        {
            DbContext = dbContext;
            CurrentUserService = currentUserService;
            Logger = logger;
        }

        #region AddAsync

        /// <summary>
        /// Insert Entity
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>

        public Task AddAsync<TEntity>(TEntity entity) where TEntity : class, IEntity<long>
        {
            return AddAsync<TEntity, long>(entity);
        }

        public Task AddAsync<TEntity, TKey>(TEntity entity)
            where TEntity : class, IEntity<TKey>
            where TKey : IEquatable<TKey>
        {
            return DbContext.AddAsync(entity).AsTask();
        }

        public Task AddRangeAsync<TEntity>(IEnumerable<TEntity> entities)
            where TEntity : class, IEntity<long>
        {
            return AddRangeAsync<TEntity, long>(entities);
        }

        public Task AddRangeAsync<TEntity, TKey>(IEnumerable<TEntity> entities)
            where TEntity : class, IEntity<TKey>
            where TKey : IEquatable<TKey>
        {
            return DbContext.AddRangeAsync(entities);
        }

        #endregion

        #region UpdateAsync

        public Task UpdateAsync<TEntity>(TEntity entity) where TEntity : class, IEntity<long>
        {
            return UpdateAsync<TEntity, long>(entity);
        }

        public Task UpdateAsync<TEntity, TKey>(TEntity entity) where TEntity : class, IEntity<TKey> where TKey : IEquatable<TKey>
        {
            DbContext.Update(entity);
            return Task.CompletedTask;
        }

        #endregion

        #region DeleteAsync

        /// <summary>
        /// Delete Entity
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Task DeleteAsync<TEntity>(TEntity entity) where TEntity : class, IEntity<long>
        {
            return DeleteAsync<TEntity, long>(entity);
        }

        public virtual Task DeleteAsync<TEntity, TKey>(TEntity entity) where TEntity : class,
            IEntity<TKey> where TKey : IEquatable<TKey>
        {
            if (entity is ISoftDelete deleteEntity)
            {
                deleteEntity.IsDelete = true;
                DbContext.Update(entity);
                return Task.CompletedTask;
            }

            DbContext.Remove(entity);
            return Task.CompletedTask;

        }

        public Task DeleteAsync<TEntity>(params object[] ids) where TEntity : class, IEntity<long>
        {
            return DeleteAsync<TEntity, long>(ids);
        }

        public async Task DeleteAsync<TEntity, TKey>(params object[] ids) where TEntity : class, IEntity<TKey> where TKey : IEquatable<TKey>
        {
            var entity = await DbContext.FindAsync<TEntity>(ids);
            await DeleteAsync<TEntity, TKey>(entity);
        }

        public Task DeleteRangeAsync<TEntity>(IEnumerable<object> ids) where TEntity : class, IEntity<long>
        {
            return DeleteRangeAsync<TEntity, long>(ids);
        }

        public async Task DeleteRangeAsync<TEntity, TKey>(IEnumerable<object> ids) where TEntity : class, IEntity<TKey> where TKey : IEquatable<TKey>
        {
            foreach (var id in ids)
            {
                await DeleteAsync<TEntity, TKey>(id);
            }
        }

        public Task DeleteRangeAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : class, IEntity<long>
        {
            return DeleteRangeAsync<TEntity, long>(entities);
        }

        public Task DeleteRangeAsync<TEntity, TKey>(IEnumerable<TEntity> entities) where TEntity : class, IEntity<TKey> where TKey : IEquatable<TKey>
        {
            throw new NotImplementedException();
        }

        #endregion
     
        public virtual IQueryable<TEntity> GetQueryable<TEntity>() where TEntity : class, IEntity<long>
        {
            return GetQueryable<TEntity, long>();
        }

        public virtual IQueryable<TEntity> GetQueryable<TEntity, TKey>()
            where TEntity : class, IEntity<TKey>
            where TKey : IEquatable<TKey>
        {
            return DbContext.Set<TEntity>();
        }

        /// <summary>
        /// FindAsync
        /// </summary>
        /// <returns></returns>
        /// 
		public Task SaveChangeAsync()
		{
            var currentDetectChangesSetting = DbContext.ChangeTracker.AutoDetectChangesEnabled;
            try
            {
                DbContext.ChangeTracker.AutoDetectChangesEnabled = false;
                DbContext.ChangeTracker.DetectChanges();
                var modifiedEntries = DbContext.ChangeTracker.Entries<IAuditedEntity>()
                    .Where(x => x.State is EntityState.Added or EntityState.Modified
                                || x.State == EntityState.Deleted);
                AuditEntity(modifiedEntries);
                DbContext.ChangeTracker.DetectChanges();
                return DbContext.SaveChangesAsync();
            }
            finally
            {
                DbContext.ChangeTracker.AutoDetectChangesEnabled = currentDetectChangesSetting;
            }
		}

        private void AuditEntity(IEnumerable<EntityEntry<IAuditedEntity>> mofifiedEntries)
        {
            var now = DateTime.UtcNow;
            //Tạm thời hack dòng này để có userId:
            //var userId = CurrentUserService.GetId();
            var userId = 1;
            foreach (var entry in mofifiedEntries)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = now;
                        entry.Entity.CreatedBy = userId;
                        AuditUpdatingField(entry);
                        Logger.LogInformation("Added entity {@Entity} by user {@UserId}", entry.Entity,
                            userId);
                        break;
                    case EntityState.Deleted:
                        AuditUpdatingField(entry);
                        Logger.LogInformation("Deleted entity to {@Entity} by user {@UserId}", entry, userId);
                        break;
                    case EntityState.Modified:
                        AuditUpdatingField(entry);
                        Logger.LogInformation("Updated entity to {@Entity} by user {@UserId}", entry, userId);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return;

            void AuditUpdatingField(EntityEntry<IAuditedEntity> entry)
            {
                entry.Entity.UpdatedAt = DateTime.UtcNow;
                entry.Entity.UpdatedBy = userId;
            }
        }

        public virtual Task<TEntity> FindAsync<TEntity>(params object[] ids)
            where TEntity : class, IEntity<long>
        {
            return FindAsync<TEntity, long>(ids);
        }

        public virtual Task<TEntity> FindAsync<TEntity>(long id, IEnumerable<string> includes = null)
            where TEntity : class, IEntity<long>
        {
            return FindAsync<TEntity, long>(id, includes);
        }

        public virtual Task<TEntity> FindAsync<TEntity, TKey>(params object[] ids)
            where TEntity : class, IEntity<TKey>
            where TKey : IEquatable<TKey>
        {
            return DbContext.FindAsync<TEntity>(ids).AsTask();
        }

        public virtual async Task<TEntity> FindAsync<TEntity, TKey>(TKey id, IEnumerable<string> includes = null)
           where TEntity : class, IEntity<TKey>
           where TKey : IEquatable<TKey>
        {
            var query = BuildIncludeQuery<TEntity, TKey>(includes);
            query = ExcludeDeleteEntity<TEntity, TKey>(query);
            return await query.FirstOrDefaultAsync(e => e.Id.Equals(id));
        }

        public virtual Task<TEntity> FindAsync<TEntity>(Expression<Func<TEntity, bool>> where = null, IEnumerable<string> includes = null) 
            where TEntity : class, IEntity<long>
        {
            return FindAsync<TEntity, long>(where, includes);
        }

        public virtual Task<TEntity> FindAsync<TEntity, TKey>(Expression<Func<TEntity, bool>> where = null, IEnumerable<string> includes = null)
            where TEntity : class, IEntity<TKey>
            where TKey : IEquatable<TKey>
        {
            var query = BuildIncludeQuery<TEntity, TKey>(includes);
            query = ExcludeDeleteEntity<TEntity, TKey>(query);
            return query.FirstOrDefaultAsync(where);
        }
        
        public virtual async Task<PagedResult<TEntity>> FindPagedAsync<TEntity>(
            IQueryable<TEntity> query = null,
            int pageIndex = 1,
            int pageSize = 10,
            Expression<Func<TEntity, TEntity>> selector = null) 
            where TEntity : class
        {
            pageIndex = pageIndex <= 0 ? 1 : pageIndex;
            pageSize = pageSize <= 0 ? 10 : pageSize;

            pageIndex -= 1;
            var skip = pageSize * pageIndex;            
            query = query.AsNoTracking();
            var totalCount = await query.CountAsync();
            query = skip == 0 ? query.Take(pageSize) : query.Skip(skip).Take(pageSize);

            return new PagedResult<TEntity>()
            {
                PageIndex = pageIndex + 1,
                PageSize = pageSize,
                Items = selector == null ? await query.ToListAsync() : await query.Select(selector).ToListAsync(),
                TotalCount = totalCount
            };
        }

        private IQueryable<TEntity> BuildIncludeQuery<TEntity, TKey>(IEnumerable<string> includes)
            where TEntity : class, IEntity<TKey>
            where TKey : IEquatable<TKey>
        {
            var query = DbContext.Set<TEntity>().AsQueryable();
            if (includes?.Any() != true)
            {
                return query;
            }

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return query;
        }

        private static IQueryable<TEntity> ApllyDefaultOrderby<TEntity, TKey>(IQueryable<TEntity> query)
            where TEntity : class, IEntity<TKey>
            where TKey : IEquatable<TKey>
        {
            if (typeof(TEntity).GetInterfaces().Contains(typeof(IAuditedEntity)))
            {
                query = query.OrderByDescending(x => ((IAuditedEntity)x).CreatedAt);
            }
            else
            {
                query = query.OrderBy(e => e.Id);
            }
            return query;
        }

        private static IQueryable<TEntity> ExcludeDeleteEntity<TEntity, TKey>(IQueryable<TEntity> query) 
            where TEntity : class, IEntity<TKey>
            where TKey : IEquatable<TKey>
        {
            if(typeof(TEntity).GetInterfaces().Contains(typeof(ISoftDelete)))
            {
                query = query.Where(x => !((ISoftDelete)x).IsDelete);
            }
            return query;
        }
    }
}
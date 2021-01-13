using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Appoint.EntityFramework.Rep
{
    public interface IRepository<TEntity> where TEntity : class
    {
        #region Select/Get/Query

        IQueryable<TEntity> GetAll();
        DbSqlQuery<TEntity> ExecuteQuerySql(string sql, params object[] parameters);
      IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] propertySelectors);

        List<TEntity> GetAllList();


        Task<List<TEntity>> GetAllListAsync();

        List<TEntity> GetAllList(Expression<Func<TEntity, bool>> predicate);

        Task<List<TEntity>> GetAllListAsync(Expression<Func<TEntity, bool>> predicate);


        T Query<T>(Func<IQueryable<TEntity>, T> queryMethod);



        TEntity Single(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate);







        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate);


        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);



        #endregion

        #region Insert

        TEntity Insert(TEntity entity);

        Task<TEntity> InsertAsync(TEntity entity);

        void Insert(List<TEntity> entitys);

        #endregion

        #region Update

        TEntity Update(TEntity entity);

        Task<TEntity> UpdateAsync(TEntity entity);



        #endregion

        #region Delete

        void Delete(TEntity entity);


        Task DeleteAsync(TEntity entity);






        void Delete(Expression<Func<TEntity, bool>> predicate);

        Task DeleteAsync(Expression<Func<TEntity, bool>> predicate);

        #endregion

        #region Aggregates


        int Count();


        Task<int> CountAsync();

        int Count(Expression<Func<TEntity, bool>> predicate);


        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);




        #endregion
    }
}

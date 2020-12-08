using Appoint.EntityFramework.DbContextProvider;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appoint.EntityFramework.Uow
{
    public class UnitOfWork<TDbContext, IDbContextProvider> : IUnitOfWork
      where TDbContext : DbContext
    {
        public readonly IDbContextProvider<TDbContext> _dbContextProvider;
        public UnitOfWork(IDbContextProvider<TDbContext> provider)
        {
            _dbContextProvider = provider;
        }

        public TDbContext context => _dbContextProvider.GetDbContext();

        public int SaveChange()
        {
            int res = context.SaveChanges();
            return res;
        }
    }
}

using Appoint.EntityFramework.DbContextProvider;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appoint.EntityFramework.Uow
{
    public class UnitOfWork<TDbContext> : IUnitOfWork<TDbContext>
      where TDbContext : DbContext
    {
        public IDbContextProvider<TDbContext> _dbContextProvider { get; set; }
        public TDbContext context => _dbContextProvider.GetDbContext();

        public int SaveChange()
        {
            int res = context.SaveChanges();
            return res;
        }
    }
}

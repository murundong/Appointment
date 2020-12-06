using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Appoint.EntityFramework.DbContextProvider
{
    public class DbContextProvider<TdbContext> : IDbContextProvider<TdbContext>
      where TdbContext : DbContext
    {

        static string key = "DbContext_Single";
        public TdbContext GetDbContext()
        {
            DbContext temp = CallContext.LogicalGetData(key) as DbContext;
            if (temp == null)
            {
                temp = Activator.CreateInstance<TdbContext>();
                CallContext.LogicalSetData(key, temp);
            }
            return temp as TdbContext;
        }

        public void Release()
        {
            var dbContext = CallContext.LogicalGetData(key) as DbContext;
            if (dbContext != null)
            {
                dbContext.SaveChanges();
                dbContext.Dispose();
                dbContext = null;
                CallContext.LogicalSetData(key, dbContext);
            }
        }


      
    }
}

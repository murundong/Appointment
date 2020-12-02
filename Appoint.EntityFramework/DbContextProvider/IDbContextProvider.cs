using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appoint.EntityFramework.DbContextProvider
{
    public interface IDbContextProvider<out TDbContext>
    where TDbContext : DbContext
    {
        TDbContext GetDbContext();

        void Release();
    }
}

using Appoint.EntityFramework.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appoint.EntityFramework
{
    public class App_DbContext: DbContext
    {
        public App_DbContext():base("name=default")
        {

        }

        public DbSet<UserInfo> Set_UserInfo { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}

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
        public DbSet<CardTemplate> Set_CardTemplete { get; set; }
        public DbSet<UserInfos> Set_UserInfo { get; set; }
        public DbSet<Banners> Set_Banners { get; set; }
        public DbSet<Doors> Set_Doors { get; set; }
        public DbSet<Subjects> Set_Subjects { get; set; }
        public DbSet<Courses> Set_Courses { get; set; }
        public DbSet<DoorUsersCards> Set_DoorUsersCards { get; set; }
        public DbSet<DoorUsers> Set_DoorUsers { get; set; }
        public DbSet<DoorUsersAppoints> Set_DoorUserAppoints { get; set; }
        public DbSet<DoorUsersQueueAppoints> Set_DoorUserQueueAppoints { get; set; }
        public DbSet<DoorUsersCourseComments> SetF_DoorUsersCourseComments { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}

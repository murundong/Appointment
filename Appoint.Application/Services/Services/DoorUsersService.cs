using Appoint.EntityFramework;
using Appoint.EntityFramework.Data;
using Appoint.EntityFramework.Enum;
using Appoint.EntityFramework.Rep;
using Appoint.EntityFramework.Uow;
using Appoint.EntityFramework.ViewData;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appoint.Application.Services
{
    public class DoorUsersService : IDoorUsersService
    {
        public IRepository<App_DbContext, DoorUsers> _repository { get; set; }
        public IRepository<App_DbContext, View_DoorUserInfoOutput> _repositoryDoorUserInfo { get; set; }
        public IUnitOfWork<App_DbContext> uof { get; set; }

        public void AddUserAttention(string openid, int doorid)
        {
            string sql = @"merge into [dbo].[DoorUsers] T
            using (select door_id=@door_id,uid=(select top 1 uid from [dbo].[UserInfos] where open_id=@open_id)) S
            on T.door_id = S.door_id and T.uid = S.uid
            when not matched then
	            insert (door_id,uid,role,create_time) values(S.door_id,s.uid,0,getdate());";
            var sqlParam = new SqlParameter[] {
                new SqlParameter("@door_id",doorid),
                new SqlParameter("@open_id",openid)
            };
            _repository.ExecuteSqlCommand(sql, sqlParam);
        }

        public bool AllocRole(int id, Enum_UserRole role)
        {
            var entity = _repository.FirstOrDefault(s => s.id == id);
            if (entity != null && entity.uid > 0)
            {
                entity.role = role;
                _repository.Update(entity);
                return uof.SaveChange() > 0;
            }
            return false;
        }

        public bool CheckHasAdminMenu(int uid)
        {
            return _repository.Count(s=>s.role == Enum_UserRole.Admin || s.role == Enum_UserRole.Curator || s.role == Enum_UserRole.Teacher) > 0;
        }

        public bool CheckUserBlackList(string openid, int doorid)
        {
            string sql = @"select * from  [dbo].[DoorUsers] 
	where door_id = @door_id and uid = (select uid  from  [dbo].[UserInfos] where open_id=@open_id )";
            var SqlParam = new SqlParameter[]
          {
                new SqlParameter("@door_id",doorid),
                new SqlParameter("@open_id",openid)
          };
            var query= _repository.ExecuteSqlQuery(sql,SqlParam).FirstOrDefault();
            return query?.role == Enum_UserRole.Black;
        }

        public View_DoorUserInfoOutput GetDoorUserInfo(int id)
        {
            string sql = @"select B.avatar,B.nick_name,A.* from [dbo].[DoorUsers] A
			left join [dbo].[UserInfos] B
			on A.uid	 = B.uid
    		where A.id= @id";
            var SqlParam = new SqlParameter[]
            {
                new SqlParameter("@id",id)
            };
            return _repositoryDoorUserInfo.ExecuteSqlQuery(sql, SqlParam).FirstOrDefault();
        }

        public DoorUsers GetDoorUsersByUID(int door_id, int uid)
        {
            return _repository.FirstOrDefault(s => s.door_id == door_id && uid == uid);
        }

        public bool SetUserRemark(int id, string remark)
        {
            var entity = _repository.FirstOrDefault(s => s.id == id);
            if(entity!=null && entity.uid > 0)
            {
                entity.remark = remark;
                _repository.Update(entity);
                return uof.SaveChange() > 0;
            }
            return false;
        }
    }
}

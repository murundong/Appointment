using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Appoint.EntityFramework;
using Appoint.EntityFramework.Data;
using Appoint.EntityFramework.Rep;
using Appoint.EntityFramework.Uow;
using Appoint.EntityFramework.ViewData;

namespace Appoint.Application.Services
{
    public class DoorUsersQueueAppointsService : IDoorUsersQueueAppointsService
    {
        public IRepository<App_DbContext, DoorUsersQueueAppoints> _repository { get; set; }
        public IUnitOfWork<App_DbContext> uof { get; set; }

        public bool CancelQueue(int uid, int courseid)
        {
            var entity = _repository.FirstOrDefault(s => s.uid == uid && s.course_id == courseid);
            if (entity == null || entity.id <= 0) return false;
            _repository.Delete(entity);
            return uof.SaveChange() > 0;
        }

        public bool CheckUserAlreadyQuee(int uid, int courseid)
        {
            return _repository.Count(s => s.uid == uid && s.course_id == courseid) > 0;
        }

        public bool DeleteUserQueue(DoorUsersQueueAppoints entity)
        {
            _repository.Delete(entity);
            return uof.SaveChange() > 0;
        }

        public DoorUsersQueueAppoints GetQueueUser(int courseid)
        {
            var entity= _repository.GetAll().Where(s => s.course_id == courseid).OrderBy(s => s.create_time).FirstOrDefault();
            return entity;
        }

        public bool Queue(View_AppointCourseInput input)
        {
            if (input.uid <= 0 || input.course_id <= 0 || input.door_id <= 0) return false;
            string sql = @"merge into [dbo].[DoorUsersQueueAppoints] T
                    using ( select du_id=(select top 1 id from [dbo].[DoorUsers] where door_id=@door_id and uid=@uid)
                    ,uid=@uid
                    ,course_id=@course_id
                    ,user_card_id=@user_card_id
                    ,create_time=getdate()) S
                    on T.uid = S.uid and T.course_id= S.course_id
                    when not matched then 
                    insert ([du_id]
                    ,[uid]
                    ,[course_id]
                    ,[user_card_id]
                    ,[create_time])
                    values( S.[du_id]
                    ,S.[uid]
                    ,S.[course_id]
                    ,S.[user_card_id]
                    ,S.[create_time]);";
            try
            {
                SqlParameter[] SqlParm = new SqlParameter[]
                {
                    new SqlParameter("@door_id",input.door_id),
                    new SqlParameter("@uid",input.uid),
                    new SqlParameter("@course_id",input.course_id),
                    new SqlParameter("@user_card_id",input.card_id),
                };
                return _repository.ExecuteSqlCommand(sql, SqlParm)>0;
            }
            catch (Exception ex)
            {
                
            }
            return false;
        }

        
    }
}

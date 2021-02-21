using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Appoint.EntityFramework;
using Appoint.EntityFramework.Data;
using Appoint.EntityFramework.Enum;
using Appoint.EntityFramework.Rep;
using Appoint.EntityFramework.Uow;
using Appoint.EntityFramework.ViewData;
using BaseClasses;

namespace Appoint.Application.Services.Services
{
    public class DoorUsersAppointsService : IDoorUsersAppointsService
    {
        public IRepository<App_DbContext, DoorUsersAppoints> _repository { get; set; }
        public IRepository<App_DbContext, View_MyAppointWaitOutput> _repositoryMyWaitAppoint { get; set; }
        public IRepository<App_DbContext, View_MyAppointCompOutput> _repositoryMyCompAppoint { get; set; }
        public IRepository<App_DbContext, View_MyAppointCompOutput_Detail> _repositoryMyCompDetailAppoint { get; set; }
        public IRepository<App_DbContext, Courses> _repositoryCourse { get; set; }
        public IUnitOfWork<App_DbContext> uof { get; set; }

        public bool AddUserAppoint(int uid, int doorid, int courseid, int? cardid)
        {

            string sql = @"merge into [dbo].[DoorUsersAppoints] T
                            using ( select du_id=(select top 1 id from [dbo].[DoorUsers] where door_id=@door_id and uid=@uid)
                                    ,uid=@uid
                                    ,course_id=@course_id
                                    ,user_card_id=@user_card_id
                                    ,create_time=getdate()) S
                            on T.uid = S.uid and T.course_id= S.course_id
                            when matched then 
	                            update set T.is_signed=0,T.signed_time=null,T.is_canceled=0,T.is_returncard=0,T.create_time=S.create_time
                            when not matched then 
	                            insert ([du_id]
                                       ,[uid]
                                       ,[course_id]
                                       ,[user_card_id]
                                       ,[is_signed]
                                       ,[is_canceled]
                                       ,[create_time])
	                            values( S.[du_id]
                                       ,S.[uid]
                                       ,S.[course_id]
                                       ,S.[user_card_id]
                                       ,0
                                       ,0
                                       ,S.[create_time]);";
            try
            {
                SqlParameter[] SqlParm = new SqlParameter[] {
                new SqlParameter("@door_id",doorid),
                new SqlParameter("@uid",uid),
                new SqlParameter("@course_id",courseid),
                new SqlParameter("@user_card_id",cardid),
                };
                return _repository.ExecuteSqlCommand(sql, SqlParm) > 0;
            }
            catch (Exception ex)
            {
            }
            return false;
        }

        public bool CancelAppoint(int? uid, int? courseid)
        {
            var entity = _repository.FirstOrDefault(s => s.uid == uid && s.course_id == courseid);
            if(entity!=null && entity.id > 0)
            {
                entity.is_canceled = true;
                _repository.Update(entity);
                return uof.SaveChange() > 0;
            }
            return false;
        }
        public bool ReturnAppoint(int? uid, int? courseid)
        {
            var entity = _repository.FirstOrDefault(s => s.uid == uid && s.course_id == courseid);
            if (entity != null && entity.id > 0)
            {
                entity.is_returncard = true;
                _repository.Update(entity);
                return uof.SaveChange() > 0;
            }
            return false;
        }

        public bool CopyQueueAppoint(DoorUsersQueueAppoints model)
        {
            DoorUsersAppoints entity = new DoorUsersAppoints()
            {
                course_id = model.course_id,
                du_id = model.du_id,
                uid = model.uid,
                user_card_id = model.user_card_id,
            };
            _repository.Insert(entity);
            return uof.SaveChange() > 0;
        }

        public int GetCourseAppointCount(int cid)
        {
            return _repository.Count(s => s.course_id == cid && !s.is_canceled && !s.is_returncard);
        }

        public Enum_AppointStatus GetCourseAppointStatus(int uid, int cid)
        {
            if (uid <= 0) return Enum_AppointStatus.SHOW_NULL;
            if (_repository.Count(s => s.uid == uid && s.course_id == cid && !s.is_canceled && !s.is_returncard) > 0)  return Enum_AppointStatus.SHOW_CANCEL;
            var CourseItem = _repositoryCourse.FirstOrDefault(s => s.id == cid && s.active);
            if (CourseItem == null || CourseItem.id <= 0) return Enum_AppointStatus.SHOW_NULL;
            //时间
            DateTime sttime;
            if (!DateTime.TryParse($"{CourseItem.course_date} {CourseItem.course_time}", out sttime)) return Enum_AppointStatus.SHOW_NULL;
            if (sttime.AddMinutes((int.Parse(CourseItem.limit_appoint_duration?.ToString() ?? "0") * -1)) <= DateTime.Now) return Enum_AppointStatus.SHOW_TIMEEXPIRED;
            if (!sttime.TheSameDayAs(DateTime.Now) && CourseItem.only_today_appoint) return Enum_AppointStatus.SHOW_ONLYTODY;
            //人数
            int ct = GetCourseAppointCount(cid);
            if (ct >= CourseItem.max_allow && CourseItem.allow_queue)  return Enum_AppointStatus.SHOW_QUEUE;
            if (ct >= CourseItem.max_allow) return Enum_AppointStatus.SHOW_FULLPEOPLE;
            return Enum_AppointStatus.SHOW_APPOINT;
        }
        public bool IsUserAlreadyCancel(int? uid, int? courseid)
        {
            return _repository.Count(s => s.uid == uid && s.course_id == courseid && s.is_canceled) > 0;
        }
        
        public Base_PageOutput<List<View_MyAppointWaitOutput>> GetMyAppointWait(View_MyAppointWaitInput input)
        {
            Base_PageOutput<List<View_MyAppointWaitOutput>> res = new Base_PageOutput<List<View_MyAppointWaitOutput>>();
            try
            {
                string sql = $@"select  
		                [now_user]=(
			                select count(1) from  [dbo].[DoorUsersAppoints] where course_id= A.course_id and is_canceled=0
		                ),
		                A.*,
		                B.door_id,door_name,door_address,
		                D.course_date,D.course_time,D.max_allow,D.min_allow,
		                E.subject_name,E.subject_duration,E.subject_img
		                from  [dbo].[DoorUsersAppoints] A
		                left join  [dbo].[DoorUsers] B
		                on A.du_id = B.id
		                left join [dbo].[Doors] C
		                on B.door_id = C.id
		                left join  [dbo].[Courses] D
		                on A.course_id = D.id
		                left join [dbo].[Subjects] E
		                on D.subject_id = E.id
                where A.uid = {input.uid} and A.is_canceled=0  
                and  dateadd(mi,E.subject_duration,  CONVERT(datetime,D.course_date+' '+D.course_time,20 )) > GETDATE() ;";
                var query = _repositoryMyWaitAppoint.ExecuteSqlQuery(sql);
                res.total = query.Count();
                res.data = query.OrderBy(s => s.course_date).OrderBy(s=>s.course_time)
                      .Skip((input.page_index - 1) * input.page_size)
                      .Take(input.page_size)?.ToList();
            }
            catch (Exception ex)
            {
            }
            return res;
        }

        public Base_PageOutput<List<View_MyAppointCompOutput>> GetMyAppointComp(View_MyAppointWaitInput input)
        {
            Base_PageOutput<List<View_MyAppointCompOutput>> res = new Base_PageOutput<List<View_MyAppointCompOutput>>();
            try
            {
                string sql = $@"select  [judge]= (select count(1) from [dbo].[DoorUsersCourseComments] where uid = A.uid and course_id = A.course_id),
										dt=LEFT(D.course_date,7),
		                                A.*,
		                                B.door_id,door_name,
		                                D.course_date,D.course_time,
		                                E.subject_name,E.subject_duration,E.subject_img
		                                from  [dbo].[DoorUsersAppoints] A
		                                left join  [dbo].[DoorUsers] B
		                                on A.du_id = B.id
		                                left join [dbo].[Doors] C
		                                on B.door_id = C.id
		                                left join  [dbo].[Courses] D
		                                on A.course_id = D.id
		                                left join [dbo].[Subjects] E
		                                on D.subject_id = E.id
                                where A.uid = {input.uid} and A.is_canceled=0 
								and dateadd(mi, E.subject_duration, CONVERT(datetime, D.course_date+' ' + D.course_time,20 )) < GETDATE()";
                var query = _repositoryMyCompDetailAppoint.ExecuteSqlQuery(sql);
                res.total = query.Count();
                res.data = query?.GroupBy(s => s.dt)?.Select(s => new View_MyAppointCompOutput() { dt = s.Key, ct = s.Count() })?.ToList();
                var queryPage = query.OrderByDescending(s=>s.course_date).ThenByDescending(s=>s.course_time)
                                .Skip((input.page_index - 1) * input.page_size)
                                .Take(input.page_size);
                if(res.data!=null && res.data.Count > 0)
                {
                    string maxdt = Convert.ToDateTime($"{ res.data[0].dt}-01").AddMonths(1).ToString("yyyy-MM-dd");
                    string mindt = $"{ res.data[res.data.Count() - 1].dt}-01";

                    res.data.ForEach(s =>
                    {
                        s.courses = queryPage.Where(p =>
                         Convert.ToDateTime(p.st_time) >= Convert.ToDateTime(s.dt)
                         && Convert.ToDateTime(p.st_time) < (Convert.ToDateTime(s.dt).AddMonths(1) < DateTime.Now ? Convert.ToDateTime(s.dt).AddMonths(1) : DateTime.Now)
                        )?.ToList();
                    });
                }
            }
            catch (Exception ex)
            {

            }
            return res;

        }

       






        //public Base_PageOutput<List<View_MyAppointCompOutput>> GetMyAppointComp(View_MyAppointWaitInput input)
        //{
        //    Base_PageOutput<List<View_MyAppointCompOutput>> res = new Base_PageOutput<List<View_MyAppointCompOutput>>();
        //    try
        //    {
        //        string sqlComp = $@"select  dt=LEFT(B.course_date,7),[ct]=count(1) from  [dbo].[DoorUsersAppoints] A
        //                  left join [dbo].[Courses] B
        //                  on A.course_id = B.id
        //                        left join [dbo].[Subjects] C
        //on B.subject_id = C.id
        //                  where A.uid={input.uid} and A.is_canceled=0  and dateadd(mi,C.subject_duration,  CONVERT(datetime,B.course_date+' '+B.course_time,20 )) <GETDATE()
        //                  group by LEFT(B.course_date,7)";
        //        var queryComp = _repositoryMyCompAppoint.ExecuteSqlQuery(sqlComp);
        //        res.total = queryComp.Count();
        //        res.data = queryComp.OrderByDescending(s => s.dt)
        //                    .Skip((input.page_index - 1) * input.page_size)
        //                    .Take(input.page_size)?.ToList();
        //        if(res.data!=null && res.data.Count > 0)
        //        {
        //            string maxdt = Convert.ToDateTime($"{ res.data[0].dt}-01").AddMonths(1).ToString("yyyy-MM-dd");
        //            string mindt =  $"{ res.data[res.data.Count() - 1].dt}-01";

        //            string sqlDetail = $@"select  [judge]= (select count(1) from [dbo].[DoorUsersCourseComments] where uid = A.uid and course_id = A.course_id),
        //                          A.*,
        //                          B.door_id,door_name,
        //                          D.course_date,D.course_time,
        //                          E.subject_name,E.subject_duration,E.subject_img
        //                          from  [dbo].[DoorUsersAppoints] A
        //                          left join  [dbo].[DoorUsers] B
        //                          on A.du_id = B.id
        //                          left join [dbo].[Doors] C
        //                          on B.door_id = C.id
        //                          left join  [dbo].[Courses] D
        //                          on A.course_id = D.id
        //                          left join [dbo].[Subjects] E
        //                          on D.subject_id = E.id
        //                        where A.uid = {input.uid} and A.is_canceled=0 
        //            and dateadd(mi, E.subject_duration, CONVERT(datetime, D.course_date+' ' + D.course_time,20 )) < '{maxdt}'
        //            and CONVERT(datetime, D.course_date+' ' + D.course_time,20 ) >= '{mindt}'; ";
        //            var queryDetail = _repositoryMyCompDetailAppoint.ExecuteSqlQuery(sqlDetail).ToList();
        //            if(queryDetail!=null && queryDetail.Count > 0)
        //            {
        //                res.data.ForEach(s =>
        //                {
        //                    s.courses = queryDetail.Where(p => 
        //                       Convert.ToDateTime(p.st_time) >= Convert.ToDateTime(s.dt)
        //                    && Convert.ToDateTime(p.st_time) < (Convert.ToDateTime(s.dt).AddMonths(1)<DateTime.Now? Convert.ToDateTime(s.dt).AddMonths(1):DateTime.Now)
        //                    )?.ToList();
        //                });
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return res;

        //}

    }
}

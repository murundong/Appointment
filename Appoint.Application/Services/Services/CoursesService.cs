using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Appoint.EntityFramework;
using Appoint.EntityFramework.Data;
using Appoint.EntityFramework.DbContextProvider;
using Appoint.EntityFramework.Rep;
using Appoint.EntityFramework.Uow;
using Appoint.EntityFramework.ViewData;

namespace Appoint.Application.Services
{
    public class CoursesService : ICoursesService
    {
        
        public IRepository<App_DbContext, Courses> _repository { get; set; }
        public IRepository<App_DbContext, Subjects> _repositorySubject { get; set; }
        public IRepository<App_DbContext, CardTemplate> _repositoryCards { get; set; }
        public IRepository<App_DbContext, View_CourseShowOutput_AppointUser> _repositoryAppointUser { get; set; }
        public IRepository<App_DbContext, View_JudgeCourseOutput> _repositoryJudgeCourse { get; set; }
        public IUnitOfWork<App_DbContext> uof { get; set; }

        public bool CheckCourseCanCancel(int courseid)
        {
            try
            {
                var entity = _repository.FirstOrDefault(s => s.id == courseid);
                if (entity == null || entity.id <= 0) return false;
                var entitySubject = _repositorySubject.FirstOrDefault(s => s.id == entity.subject_id);
                if (entitySubject == null || entitySubject.id <= 0) return false;
                if (entity.cancel_duration.HasValue)
                {
                    DateTime sttime;
                    if (!DateTime.TryParse($"{entity.course_date} {entity.course_time}", out sttime)) return false;
                    if (DateTime.Now >= sttime.AddMinutes(Convert.ToDouble((entity.cancel_duration * -1)))) return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                
            }
            return false;
        }

        public bool CheckCourseNeedCards(int course_id)
        {
            var entity= _repository.FirstOrDefault(s => s.id == course_id);
            if (entity == null || entity.id <= 0) return false;
            return !string.IsNullOrWhiteSpace(entity.need_cards);
        }

        public Courses CreateCourse(Courses model)
        {
            _repository.Insert(model);
            if (uof.SaveChange() > 0) return model;
            return null;
        }

        public bool DeleteCourse(int cid)
        {
            _repository.Delete(new Courses() { id = cid });
            return uof.SaveChange() > 0;
        }

        public Courses GetCourseById(int id)
        {
            return _repository.FirstOrDefault(s => s.id == id);
        }

        public List<int> GetCourseCards(int courseid)
        {
            var query = _repository.FirstOrDefault(s => s.id == courseid);
            if (query == null || string.IsNullOrWhiteSpace(query.need_cards)) return null;
            return query.need_cards.Split(',')?.Select<string,int>(s=>Convert.ToInt32(s))?.ToList();
        }

        public Base_PageOutput<List<View_CoursesOutput>> GetCourses(View_CoursesInput input)
        {
            Base_PageOutput<List<View_CoursesOutput>> res = new Base_PageOutput<List<View_CoursesOutput>>();

            var query = _repository.GetAll().Where(s => s.door_id == input.door_id);
            if (!string.IsNullOrWhiteSpace(input.date))
            {
                query = query.Where(s => s.course_date == input.date);
            }
            res.total = query.Count();
            var query_end = query.OrderBy(s => s.course_time)
                .Skip((input.page_index - 1) * input.page_size)
                .Take(input.page_size);
            var lst = AutoMapper.Mapper.Map<List<View_CoursesOutput>>(query_end);
            if (lst != null && lst.Count > 0)
            {
                //已预约名单
                string sql = $@"select  A.id,du_id,course_id,A.uid,avatar,[door_remark]= B.remark,nick_name from  [dbo].[DoorUsersAppoints] A
			                        Left join [dbo].[DoorUsers] B
			                        on A.du_id = B.id
			                        left join  [dbo].[UserInfos] C
			                        on A.uid = C.uid
			                        where course_id in ({string.Join(",", lst.Select(s => s.id))}) and is_canceled = 0 and is_returncard=0 order by A.create_time;";
                var queryAppointUser = _repositoryAppointUser.ExecuteSqlQuery(sql)?.ToList();
                //排队名单
                string sqlQueue = $@"select  A.id,du_id,course_id,A.uid,avatar,[door_remark]= B.remark,nick_name from  [dbo].[DoorUsersQueueAppoints]  A
			                        Left join [dbo].[DoorUsers] B
			                        on A.du_id = B.id
			                        left join  [dbo].[UserInfos] C
			                        on A.uid = C.uid
			                        where course_id in ({string.Join(",", lst.Select(s => s.id))}) order by A.create_time;";
                var queryQueueAppointUser = _repositoryAppointUser.ExecuteSqlQuery(sqlQueue)?.ToList();
                lst.ForEach(s =>
                {
                    var sub_item = _repositorySubject.FirstOrDefault(p => p.id == s.subject_id);
                    if (sub_item != null && sub_item.id > 0)
                    {
                        s.Subject = AutoMapper.Mapper.Map<View_SubjectsOutput>(sub_item);
                    }
                    s.AppointUsers = queryAppointUser?.Where(p => p.course_id == s.id)?.ToList();
                    if (s.allow_queue)
                    {
                        s.QueueAppointUsers = queryQueueAppointUser?.Where(p => p.course_id == s.id)?.ToList();
                    }
                });
            }
            res.data = lst;
            return res;
        }

        public Base_PageOutput<List<View_CourseShowOutput>> GetDoorAppointCourse(View_GetAppointCourseInput input)
        {
            Base_PageOutput<List<View_CourseShowOutput>> return_res = new Base_PageOutput<List<View_CourseShowOutput>>();
            var query =  _repository.GetAll().Where(s => s.door_id == input.doorId && s.course_date == input.date );
            var querySubject = _repositorySubject.GetAll().Where(s => s.door_id == input. doorId);
            var lstCourse = AutoMapper.Mapper.Map<List<View_CourseShowOutput>>(query.ToList());
            if (lstCourse?.Count > 0)
            {
                lstCourse.ForEach(s =>
                {
                    var subject_item = querySubject.FirstOrDefault(p => p.id == s.subject_id);
                    s.Subject = AutoMapper.Mapper.Map<View_SubjectsOutput>(subject_item);
                    s.NeedCardNames = _repositoryCards.GetAll().Where(p => s.need_cards.Contains(p.id.ToString())).Select(p => p.card_name).ToList();
                });
                if (!string.IsNullOrWhiteSpace(input.tag))
                {
                    lstCourse = lstCourse?.Where(s => s.Subject.subject_tag?.Contains(input.tag)==true)?.ToList();
                }
                return_res.total = lstCourse.Count;
                return_res.data= lstCourse.OrderBy(s=>s.course_time)
                     .Skip((input.page_index - 1) * input.page_size)
                     .Take(input.page_size)?.ToList();
                if (return_res.data?.Count > 0)
                {
                    //已预约名单
                    string sql = $@"select  A.id,du_id,course_id,A.uid,avatar,[door_remark]= B.remark,nick_name from  [dbo].[DoorUsersAppoints] A
			                        Left join [dbo].[DoorUsers] B
			                        on A.du_id = B.id
			                        left join  [dbo].[UserInfos] C
			                        on A.uid = C.uid
			                        where course_id in ({string.Join(",", return_res.data.Select(s=>s.id))}) and is_canceled = 0 and is_returncard = 0 order by A.create_time;";
                    var queryAppointUser = _repositoryAppointUser.ExecuteSqlQuery(sql)?.ToList();
                  
                    //排队名单
                    string sqlQueue = $@"select  A.id,du_id,course_id,A.uid,avatar,[door_remark]= B.remark,nick_name from  [dbo].[DoorUsersQueueAppoints]  A
			                        Left join [dbo].[DoorUsers] B
			                        on A.du_id = B.id
			                        left join  [dbo].[UserInfos] C
			                        on A.uid = C.uid
			                        where course_id in ({string.Join(",", return_res.data.Select(s => s.id))}) order by A.create_time;";
                    var queryQueueAppointUser = _repositoryAppointUser.ExecuteSqlQuery(sqlQueue)?.ToList();
                    return_res.data.ForEach(s =>
                    {
                        s.AppointUsers = queryAppointUser?.Where(p => p.course_id == s.id)?.ToList();
                        if (s.allow_queue)
                        {
                            s.QueueAppointUsers = queryQueueAppointUser?.Where(p => p.course_id == s.id)?.ToList();
                        }
                    });

                }
            }
            return return_res;
        }

        public View_JudgeCourseOutput GetJudgeCourseInfo(int? cid)
        {
            string sql = $@"select B.subject_name,subject_teacher,subject_img,subject_duration, A.* from [dbo].[Courses] A
	                        left join [dbo].[Subjects] B
	                        on A.subject_id = B.id
	                        where A.id={cid} ;";
            return _repositoryJudgeCourse.ExecuteSqlQuery(sql).FirstOrDefault();
        }

        public View_CoursesOutput GetSignCourseById(int course_Id)
        {
            View_CoursesOutput res = new View_CoursesOutput();
            var query = _repository.FirstOrDefault(s => s.id == course_Id);
            res = AutoMapper.Mapper.Map<View_CoursesOutput>(query);
            if(res!=null)
            {
                var query_subject = _repositorySubject.FirstOrDefault(p => p.id == res.subject_id);
                res.Subject = AutoMapper.Mapper.Map<View_SubjectsOutput>(query_subject);
                string sql = $@"select  A.id,du_id,course_id,A.uid,avatar,[door_remark]= B.remark,nick_name from  [dbo].[DoorUsersAppoints] A
			                        Left join [dbo].[DoorUsers] B
			                        on A.du_id = B.id
			                        left join  [dbo].[UserInfos] C
			                        on A.uid = C.uid
			                        where course_id ={course_Id} and is_canceled = 0 and is_returncard=0 order by A.create_time;";
                res.AppointUsers = _repositoryAppointUser.ExecuteSqlQuery(sql).ToList();
            }
            return res;
        }

        public List<View_WeekCourseOutput> GetWeekCourse(View_WeekCourseInput input)
        {
            DateTime st_dt , ed_dt ;
            DateTime dt = DateTime.Now;
            
            if(input.tp == -1)
            {
                st_dt = dt.AddDays(1 - Convert.ToInt32(dt.DayOfWeek.ToString("d")) - 7);
                ed_dt = dt.AddDays(1 - Convert.ToInt32(dt.DayOfWeek.ToString("d")) + 6 - 7);
            }
            else if (input.tp == 1)
            {
                st_dt = dt.AddDays(1 - Convert.ToInt32(dt.DayOfWeek.ToString("d")) + 7);
                ed_dt = dt.AddDays(1 - Convert.ToInt32(dt.DayOfWeek.ToString("d")) + 6 + 7);
            }
            else
            {
                st_dt = dt.AddDays(1 - Convert.ToInt32(dt.DayOfWeek.ToString("d")));
                ed_dt = dt.AddDays(1 - Convert.ToInt32(dt.DayOfWeek.ToString("d")) + 6);
            }
            List<View_WeekCourseOutput> res = new List<View_WeekCourseOutput>();
            string[] Day = new string[] { "周日", "周一", "周二", "周三", "周四", "周五", "周六" };
            
            var query = _repository.SqlQuery($"select * from [dbo].[Courses] where door_id={input.door_id} and course_date >='{st_dt.ToString("yyyy-MM-dd")}'  and course_date <= '{ed_dt.ToString("yyyy-MM-dd")}' order by course_date,course_time");


            for (int i = 0; i < 7; i++)
            {
                View_WeekCourseOutput itemModel = new View_WeekCourseOutput()
                {

                    date = st_dt.AddDays(i).ToString("yyyy-MM-dd"),
                    week = Day[(int)st_dt.AddDays(i).DayOfWeek],
                    Courses = AutoMapper.Mapper.Map<List<View_CoursesOutput>>(query?.Where(s => s.course_date == st_dt.AddDays(i).ToString("yyyy-MM-dd")))
                };
                if (itemModel.Courses?.Count > 0)
                {
                    itemModel.Courses.ForEach(s =>
                    {
                        s.Subject = AutoMapper.Mapper.Map<View_SubjectsOutput>(_repositorySubject.FirstOrDefault(p => p.id == s.subject_id));
                    });
                }
                res.Add(itemModel);
            }
            return res;

        }

     

        public bool QuickCourse(string sdate, string cdate, int doorid, string openid)
        {
            List<Courses> insertLst = new List<Courses>();
            var lstCourse = _repository.GetAll().Where(s => s.course_date == sdate && s.create_openid == openid && s.door_id == doorid);
            if (lstCourse.Count() <= 0)
            {
                return false;
            }
            lstCourse.ToList().ForEach(s =>
            {
                Courses itemCourse = new Courses();
                AutoMapper.Mapper.Map(s, itemCourse);
                itemCourse.course_date = cdate;
                itemCourse.temp_teacher = string.Empty;
                itemCourse.create_time = DateTime.Now;
                itemCourse.active = true;
                insertLst.Add(itemCourse);
            });
            _repository.Insert(insertLst);
            return uof.SaveChange() > 0;
        }

        public bool UpdateCourse(Courses model)
        {
            var entity = _repository.FirstOrDefault(s => s.id == model.id);
            var oid = entity.create_openid;
            var time = entity.create_time;
            AutoMapper.Mapper.Map(model, entity);
            entity.create_time = time;
            entity.create_openid = oid;
            _repository.Update(entity);
            return uof.SaveChange() > 0;
        }


    }
}

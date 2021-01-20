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
        public IUnitOfWork<App_DbContext> uof { get; set; }

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
                lst.ForEach(s =>
                {
                    var sub_item = _repositorySubject.FirstOrDefault(p => p.id == s.subject_id);
                    if (sub_item != null && sub_item.id > 0)
                    {
                        s.Subject = AutoMapper.Mapper.Map<View_SubjectsOutput>(sub_item);
                    }
                });
            }
            res.data = lst;
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
            //var query = _repository.GetAll().Where(s => s.door_id == input.door_id
            //&& DbFunctions.CreateDateTime(Convert.ToInt32( s.course_date.Split('-')[0]), Convert.ToInt32(s.course_date.Split('-')[1]), Convert.ToInt32(s.course_date.Split('-')[2]), 0,0,0) > st_dt
            ////&& DbFunctions.TruncateTime(DateTimeOffset.Parse(s.course_date)) >= st_dt
            ////&& DbFunctions.TruncateTime(DateTimeOffset.Parse(s.course_date)) <= ed_dt
            //);
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

            //if (query.Count() > 0)
            //{
            //    for (int i = 0; i < 7; i++)
            //    {
            //        View_WeekCourseOutput itemModel = new View_WeekCourseOutput()
            //        {

            //            date = st_dt.AddDays(i).ToString("yyyy-MM-dd"),
            //            week = Day[(int)st_dt.AddDays(i).DayOfWeek],
            //            Courses = AutoMapper.Mapper.Map<List<View_CoursesOutput>>(query.Where(s => s.course_date == st_dt.AddDays(i).ToString("yyyy-MM-dd")))
            //        };
            //        if (itemModel.Courses.Count > 0)
            //        {
            //            itemModel.Courses.ForEach(s =>
            //            {
            //                s.Subject = AutoMapper.Mapper.Map<View_SubjectsOutput>(_repositorySubject.FirstOrDefault(p => p.id == s.subject_id));
            //            });
            //        }
            //        res.Add(itemModel);
            //    }
            //}
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

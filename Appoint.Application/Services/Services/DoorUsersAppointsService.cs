using System;
using System.Collections.Generic;
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
        public IRepository<App_DbContext, Courses> _repositoryCourse { get; set; }
        public IUnitOfWork<App_DbContext> uof { get; set; }

        public int GetCourseAppointCount(int cid)
        {
            return _repository.Count(s => s.course_id == cid);
        }

        public Enum_AppointStatus GetCourseAppointStatus(int uid, int cid)
        {
            if (uid <= 0) return Enum_AppointStatus.SHOW_NULL;
            if (_repository.Count(s => s.uid == uid && s.course_id == cid) > 0)
                return Enum_AppointStatus.SHOW_CANCEL;
            var CourseItem = _repositoryCourse.FirstOrDefault(s => s.id == cid && s.active);
            if (CourseItem == null || CourseItem.id <= 0) return Enum_AppointStatus.SHOW_NULL;
            //时间
            DateTime sttime;
            if (!DateTime.TryParse($"{CourseItem.course_date} {CourseItem.course_time}", out sttime)) return Enum_AppointStatus.SHOW_NULL;
            if (sttime.AddMinutes((int.Parse(CourseItem.limit_appoint_duration?.ToString() ?? "0") * -1)) <= DateTime.Now) return Enum_AppointStatus.SHOW_TIMEEXPIRED;
            if (!sttime.TheSameDayAs(DateTime.Now) && CourseItem.only_today_appoint) return Enum_AppointStatus.SHOW_ONLYTODY;
            //人数
            int ct = GetCourseAppointCount(cid);
            if (ct >= CourseItem.max_allow && CourseItem.allow_queue) return Enum_AppointStatus.SHOW_QUEUE;
            if (ct >= CourseItem.max_allow) return Enum_AppointStatus.SHOW_FULLPEOPLE;
            return Enum_AppointStatus.SHOW_APPOINT;
        }
    }
}

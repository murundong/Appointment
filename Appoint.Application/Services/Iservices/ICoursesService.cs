using Appoint.EntityFramework.Data;
using Appoint.EntityFramework.ViewData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appoint.Application.Services
{
    public interface ICoursesService : IApplicationService
    {
        Base_PageOutput<List<View_CoursesOutput>> GetCourses(View_CoursesInput input);
        View_CoursesOutput GetSignCourseById(int course_Id);
        Courses CreateCourse(Courses model);
        bool UpdateCourse(Courses model);
        Courses GetCourseById(int id);
        bool QuickCourse(string sdate, string cdate,int doorid, string openid);
        bool DeleteCourse(int cid);

        bool CheckCourseNeedCards(int course_id);

        List<View_WeekCourseOutput> GetWeekCourse(View_WeekCourseInput input);

        Base_PageOutput< List<View_CourseShowOutput>> GetDoorAppointCourse(View_GetAppointCourseInput input);

        List<int> GetCourseCards(int courseid);

        bool CheckCourseCanCancel(int courseid);

        View_JudgeCourseOutput GetJudgeCourseInfo(int? cid);
    }
}

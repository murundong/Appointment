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

        Courses CreateCourse(Courses model);
        bool UpdateCourse(Courses model);
        Courses GetCourseById(int id);
    }
}

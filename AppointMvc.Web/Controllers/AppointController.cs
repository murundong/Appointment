using Appoint.EntityFramework.Enum;
using Appoint.EntityFramework.ViewData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppointMvc.Web.Controllers
{
    public class AppointController : ControllerBase
    {
        
        public ActionResult GetAppointLessons(View_AppointCourseInput input)
        {
            if(input==null || input.doorId <= 0 || string.IsNullOrWhiteSpace(input.date)) ReturnJsonResult("参数错误！", Enum_ReturnRes.Fail);
            if (input.tag == "全部课程") input.tag = null;
            var res= _courseService.GetDoorAppointCourse(input);
            if(res!=null && res.total > 0)
            {
                res.data.ForEach(s =>
                {
                    s.AppointStatus = _doorUserAppointService.GetCourseAppointStatus(input.uid,s.id);
                });
            }
            return ReturnJsonResult(res);
        }

        public ActionResult GetDoorTags(int? doorId)
        {
            if (!doorId.HasValue || doorId <= 0) return ReturnJsonResult("参数错误！", Enum_ReturnRes.Fail);
            var res= _subjectService.GetDoorTags(doorId);
            if (res.Count > 0) res.Insert(0, "全部课程");
            return ReturnJsonResult(res);
        }
    }
}
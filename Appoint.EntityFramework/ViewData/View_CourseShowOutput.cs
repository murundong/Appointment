using Appoint.EntityFramework.Data;
using Appoint.EntityFramework.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appoint.EntityFramework.ViewData
{
    [NotMapped]
    public class View_CourseShowOutput : Courses
    {
        //public string subject_name { get; set; }
        //public string subject_tag { get; set; }
        //public string subject_teacher { get; set; }
        //public int subject_duration { get; set; }
        //public string subject_img { get; set; }
        //public string subject_desc { get; set; }
        //public string need_cards { get; set; }
        public View_SubjectsOutput Subject { get; set; } = new View_SubjectsOutput();
        public Enum_AppointStatus AppointStatus { get; set; }
        public List<View_CourseShowOutput_AppointUser> AppointUsers = new List<View_CourseShowOutput_AppointUser>();
        public List<View_CourseShowOutput_AppointUser> QueueAppointUsers = new List<View_CourseShowOutput_AppointUser>();
        public List<string> NeedCardNames = new List<string>();
    }

    public class View_CourseShowOutput_AppointUser {
        public int id { get; set; }
        public int course_id { get; set; }
        public int du_id { get; set; }
        public int uid { get; set; }
        public string avatar { get; set; }
        public string door_remark { get; set; }
        public string nick_name { get; set; }

    }

}

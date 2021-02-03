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
        public int NowAppointCount { get; set; }
    }
}

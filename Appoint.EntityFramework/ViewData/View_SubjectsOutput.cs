using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appoint.EntityFramework.ViewData
{
    public class View_SubjectsOutput
    {

        public int id { get; set; }
        public string subject_name { get; set; }
        public string subject_teacher { get; set; }
        public int subject_duration { get; set; }
        public int? subject_price { get; set; }
        public string subject_img { get; set; }
        
        public string subject_desc { get; set; }
        public string need_cards { get; set; }
        public string create_time { get; set; }
    }
}

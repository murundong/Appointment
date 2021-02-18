using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appoint.EntityFramework.Data
{
    public class DoorUsersCourseComments
    {
        [Key]
        public int id { get; set; }
        public int uid { get; set; }
        public int course_id { get; set; }
        public int star1 { get; set; }
        public int star2 { get; set; }
        public int star3 { get; set; }
        public string comment { get; set; }
        public DateTime create_time { get; set; } = DateTime.Now;

    }
}

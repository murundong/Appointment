using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appoint.EntityFramework.ViewData
{
    public class View_AppointCourseInput : Base_PageInput
    {
        public string date { get; set; }
        public string tag { get; set; }
        public int doorId { get; set; }
    }
}

using Appoint.EntityFramework.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appoint.EntityFramework.ViewData
{
    public class View_DoorsOutput
    {
        public int id { get; set; }
        public string door_name { get; set; }
        public string door_desc { get; set;  }
        public string door_img { get; set; }
        public string door_address { get; set; }
        public Enum_DoorStatus status { get; set; }
    }
}

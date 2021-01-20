using Appoint.EntityFramework.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appoint.EntityFramework.ViewData
{
    public class View_UinfoOutput
    {
        public int uid { get; set; }
        public string open_id { get; set; }

        public string nick_name { get; set; }
        public string avatar { get; set; }
        public Enum_Gender gender { get; set; }

        public string tel { get; set; }
        public string initial { get; set; }
        public Enum_UserRole role { get; set; }
        public string real_name { get; set; }
        public string birthday { get; set; }
        public string remark { get; set; }

    }
}

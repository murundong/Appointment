using Appoint.EntityFramework.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appoint.EntityFramework.Data
{
    public class UserInfo
    {
        [Key]
        public int uid   { get; set; }
        public string nick_name { get; set; }
        public string avatar { get; set; }
        public Enum_Gender gender { get; set; }

        public string open_id { get; set; }
        public string tel { get; set; }
        public int age { get; set; }
        public DateTime create_time { get; set; }
        
    }
}

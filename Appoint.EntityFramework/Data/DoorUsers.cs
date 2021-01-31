using Appoint.EntityFramework.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appoint.EntityFramework.Data
{
    public class DoorUsers
    {
        [Key]
        public int id { get; set; }
        public int door_id { get; set; }
        public int uid { get; set; }
        public Enum_UserRole role { get; set; }
        public string remark { get; set; }
        public DateTime create_time { get; set; } = DateTime.Now;

    }
}

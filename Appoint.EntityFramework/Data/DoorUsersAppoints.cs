using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appoint.EntityFramework.Data
{
    public class DoorUsersAppoints
    {
        [Key]
        public int id { get; set; }
        public int du_id { get; set; }
        public int uid { get; set; }
        public int course_id { get; set; }
        public int? user_card_id { get; set; }
        public bool is_signed { get; set; }
        public DateTime? signed_time { get; set; }
        public bool is_canceled { get; set; }
        public DateTime create_time { get; set; } = DateTime.Now;
    }
}

using Appoint.EntityFramework.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appoint.EntityFramework.Data
{
    public class UserCards
    {
        [Key]
        public int id { get; set; }
        public int door_id { get; set; }
        public int uid { get; set; }
        public Enum_UserRole role { get; set; }
        public int cid { get; set; }
        public Enum_CardType ctype { get; set; }
        public DateTime? card_sttime { get; set; }
        public DateTime? card_edtime { get; set; }
        public int? stop_day { get; set; }
        public int? extend_day { get; set; }
        public int? effective_time { get; set; }
        public int? limit_week_time { get; set; }
        public int? limit_day_time { get; set; }
        public bool is_freeze { get; set; }
        public DateTime create_time { get; set; } = DateTime.Now;

    }
}

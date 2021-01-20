using Appoint.EntityFramework.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appoint.EntityFramework.ViewData
{
    public class View_UserCardsInfoOutput
    {
        public int id { get; set; }
        public int uid { get; set; }
        public Enum_UserRole role { get; set; }
        public string open_id { get; set; }
        public string nick_name { get; set; }
        public string avatar { get; set; }
        public Enum_Gender gender { get; set; }

        public string tel { get; set; }
        public string initial { get; set; }

        public string real_name { get; set; }
        public string birthday { get; set; }
        public Enum_UserRole door_role { get; set; }
        public string door_remark { get; set; }

        public int? cid { get; set; }
        public Enum_CardType? ctype { get; set; }
        public DateTime? card_sttime { get; set; }
        public DateTime? card_edtime { get; set; }
        public int? stop_day { get; set; }
        public int? extend_day { get; set; }
        public int? effective_time { get; set; }
        public int? limit_week_time { get; set; }
        public int? limit_day_time { get; set; }
        public bool is_freeze { get; set; }
    }
}

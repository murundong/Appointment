using Appoint.EntityFramework.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appoint.EntityFramework.Data
{
    [Table("CardTemplate")]
   public class CardTemplate
    {
        [Key]
        public int id { get; set; }
        public int door_id { get; set; }
        public string card_name { get; set; }
        public Enum_CardType card_type { get; set; }
        public string card_desc { get; set; }
        public string create_openid { get; set; }
        public DateTime create_time { get; set; } = DateTime.Now;
    }
}

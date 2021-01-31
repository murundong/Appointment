using Appoint.EntityFramework.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appoint.EntityFramework.ViewData
{
    [NotMapped]
    public  class View_DoorUserInfoOutput: DoorUsers
    {
        public string avatar { get; set; }
        public string nick_name { get; set; }
    }
}

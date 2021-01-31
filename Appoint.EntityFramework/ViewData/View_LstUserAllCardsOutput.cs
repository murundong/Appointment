using Appoint.EntityFramework.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appoint.EntityFramework.ViewData
{
    public class View_LstUserAllCardsOutput
    {
        public int door_id { get; set; }
        public string door_name { get; set; }
        public string door_img { get; set; }

        public List<View_UserCardsInfoOutput> CardsInfo = new List<View_UserCardsInfoOutput>();

    }
    
}

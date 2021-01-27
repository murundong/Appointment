using Appoint.EntityFramework.Data;
using System;
using System.Collections.Generic;
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

        public List<View_LstUserAllCardsOutput_CardsInfo> CardsInfo = new List<View_LstUserAllCardsOutput_CardsInfo>();

    }

    public class View_LstUserAllCardsOutput_CardsInfo: UserCards
    {
        
        public string card_name { get; set; }
        
        public string card_desc { get; set; }

    }
}

using Appoint.EntityFramework.ViewData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appoint.Application.Services
{
    public interface IUserCardService:IApplicationService
    {
        void AddUserAttention(string openid, int doorid);
        View_InitialUserCardsInfoOutput GetUserLst_Door(int doorid,string nick);
    }
}

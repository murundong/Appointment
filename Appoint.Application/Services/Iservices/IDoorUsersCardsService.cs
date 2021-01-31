using Appoint.EntityFramework.Data;
using Appoint.EntityFramework.Enum;
using Appoint.EntityFramework.ViewData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appoint.Application.Services
{
    public interface IDoorUsersCardsService :IApplicationService
    {
        View_InitialUserCardsInfoOutput GetUserLst_Door(int doorid,string nick);
        List<View_LstUserAllCardsOutput> GetUserALlCards(string openid, Enum_CardStatus cardStatus);
        List<View_UserCardsInfoOutput> GetUserDoorCards(int uid, int doorId);

        View_UserCardsInfoOutput GetUserInfoById(int? id);

        bool AddUserCards(DoorUsersCards model);
        //bool UpdateUserCardsInfo(DoorUsersCards model);
    }
}

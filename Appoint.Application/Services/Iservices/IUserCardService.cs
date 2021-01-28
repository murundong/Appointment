using Appoint.EntityFramework.Enum;
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

        bool SetUSerRemark(int uid, string remark);
        bool AllocRole(int uid, Enum_UserRole role);

        List<View_LstUserAllCardsOutput> GetUserALlCards(string openid, Enum_CardStatus cardStatus);

        List<View_LstUserAllCardsOutput_CardsInfo> GetUserDoorCards(string openid, int doorId);
    }
}

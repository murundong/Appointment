using Appoint.EntityFramework.Enum;
using Appoint.EntityFramework.ViewData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appoint.Application.Services
{
    public interface IDoorUsersService:IApplicationService
    {
        void AddUserAttention(string openid, int doorid);
        bool SetUSerRemark(int id, string remark);
        bool AllocRole(int id, Enum_UserRole role);

        View_DoorUserInfoOutput GetDoorUserInfo(int id);
    }
}

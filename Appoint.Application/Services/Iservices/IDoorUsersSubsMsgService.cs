using Appoint.EntityFramework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appoint.Application.Services
{
    public interface IDoorUsersSubsMsgService:IApplicationService
    {
        bool AddData(DoorUsersSubsMsg model);
        List<DoorUsersSubsMsg> GetNeedSubs(); 
        bool UpdateQueen(int id);
        bool UpdateCancel(int id);
        bool UpdateNotice(int id);
    }
}

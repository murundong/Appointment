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
    public interface IDoorUsersAppointsService : IApplicationService
    {
        Enum_AppointStatus GetCourseAppointStatus(int uid, int cid);

        int GetCourseAppointCount(int cid);

        bool AddUserAppoint(int uid, int doorid, int courseid,int? cardid);

        bool IsUserAlreadyCancel(int? uid, int? courseid);
        bool CancelAppoint(int? uid, int? courseid);
        bool ReturnAppoint(int? uid, int? courseid);
        bool CopyQueueAppoint(DoorUsersQueueAppoints model);

        Base_PageOutput< List<View_MyAppointWaitOutput>> GetMyAppointWait(View_MyAppointWaitInput input);

        Base_PageOutput< List<View_MyAppointCompOutput>> GetMyAppointComp(View_MyAppointWaitInput input);

        bool CancselCourse(int course_id);

        void CancselCourse(out Dictionary<int, List<string>> usercids);

        View_UserStatisticOutput GetUserStatistic(int uid);

        bool UpdateNoticeAppoint(string ids);
    }
}

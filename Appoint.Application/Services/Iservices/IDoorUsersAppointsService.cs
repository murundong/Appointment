using Appoint.EntityFramework.Enum;
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
    }
}

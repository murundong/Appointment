using Appoint.EntityFramework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appoint.Application.Services
{
    public interface IDoorUsersCourseCommentsService:IApplicationService
    {
        bool AddJudge(DoorUsersCourseComments input);
    }
}

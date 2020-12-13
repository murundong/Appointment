using Appoint.EntityFramework.Data;
using Appoint.EntityFramework.ViewData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appoint.Application.Services
{
    public interface IDoorsService:IApplicationService
    {
        List<View_DoorsOutput> GetDoors(View_DoorInput input);

        List<Doors> GetTeacherDoors(View_TeacherDoorInput input);
        Doors CreateDoors(Doors model);

        bool UpdateDoors(Doors model);

        Doors GetDoorsById(int id);
    }
}

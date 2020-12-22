using Appoint.EntityFramework.Data;
using Appoint.EntityFramework.ViewData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appoint.Application.Services
{
    public interface ISubjectsService:IApplicationService
    {
        Base_PageOutput<List<View_SubjectsOutput>> GetSubjects(View_SubjectsInput input);

        Subjects CreateSubject(Subjects model);
        bool UpdateSubject(Subjects model);
        Subjects GetSubjectById(int id);
    }
}

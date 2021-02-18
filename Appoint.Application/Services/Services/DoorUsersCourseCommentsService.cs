using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Appoint.EntityFramework;
using Appoint.EntityFramework.Data;
using Appoint.EntityFramework.Rep;
using Appoint.EntityFramework.Uow;

namespace Appoint.Application.Services
{
    public class DoorUsersCourseCommentsService : IDoorUsersCourseCommentsService
    {
        public IRepository<App_DbContext, DoorUsersCourseComments> _repository { get; set; }
        public IUnitOfWork<App_DbContext> uof { get; set; }

        public bool AddJudge(DoorUsersCourseComments input)
        {
            try
            {
                _repository.Insert(input);
                return uof.SaveChange() > 0;
            }
            catch (Exception  ex)
            {
                
            }
            return false;
        }
    }
}

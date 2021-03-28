using Appoint.EntityFramework;
using Appoint.EntityFramework.Data;
using Appoint.EntityFramework.Rep;
using Appoint.EntityFramework.Uow;
using Appoint.EntityFramework.ViewData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appoint.Application.Services
{
    public class DoorUsersSubsMsgService : IDoorUsersSubsMsgService
    {
        public IRepository<App_DbContext, DoorUsersSubsMsg> _repository { get; set; }
        public IUnitOfWork<App_DbContext> uof { get; set; }

        public bool AddData(DoorUsersSubsMsg model)
        {
             _repository.Insert(model);
            return uof.SaveChange() > 0;
        }

        public List<DoorUsersSubsMsg> GetNeedSubs()
        {
            return _repository.GetAll().Where(s => !s.is_cancel || !s.is_notice ).ToList();
        }

        public bool UpdateCancel(int id)
        {
            var entity = _repository.FirstOrDefault(s => s.id == id);
            if(entity!=null && entity.id > 0)
            {
                entity.is_cancel = true;
            }
            return false;
        }

        public bool UpdateNotice(int id)
        {
            var entity = _repository.FirstOrDefault(s => s.id == id);
            if (entity != null && entity.id > 0)
            {
                entity.is_notice = true;
            }
            return false;
        }

        public bool UpdateQueen(int id)
        {
            var entity = _repository.FirstOrDefault(s => s.id == id);
            if (entity != null && entity.id > 0)
            {
                entity.is_queen = true;
            }
            return false;
        }
    }
}

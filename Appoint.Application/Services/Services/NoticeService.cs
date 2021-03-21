using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Appoint.EntityFramework;
using Appoint.EntityFramework.Data;
using Appoint.EntityFramework.Rep;
using Appoint.EntityFramework.Uow;
using Appoint.EntityFramework.ViewData;

namespace Appoint.Application.Services
{
    public class NoticeService : INoticeService
    {
        public IRepository<App_DbContext, Notice> _repository { get; set; }
        public IRepository<App_DbContext, View_NoticeOutput> _repositoryView { get; set; }
        public IUnitOfWork<App_DbContext> uof { get; set; }
        public Notice CreateNotice(Notice model)
        {
            _repository.Insert(model);
            if (uof.SaveChange() > 0) return model;
            return null;
        }

        public bool DeleteNotice(int id)
        {
            _repository.Delete(new Notice() { id = id });
            return uof.SaveChange() > 0;
        }

        public Notice GetNewestNotice()
        {
            return _repository.GetAll().OrderByDescending(s => s.create_time).FirstOrDefault();
        }

        public Base_PageOutput<List<View_NoticeOutput>> GetNotice(View_NoticeInput input)
        {
            Base_PageOutput<List<View_NoticeOutput>> res = new Base_PageOutput<List<View_NoticeOutput>>();
            if (input == null) input = new View_NoticeInput();
            string sql = $@"select [role]=B.role,B.nick_name, A.* from [dbo].[Notice] A
		                    left join  [dbo].[UserInfos] B
		                    on A.uid= B.uid";
            var query = _repositoryView.ExecuteSqlQuery(sql);
            res.total = query.Count();
            var queryPage = query.OrderByDescending(s => s.create_time)
                            .Skip((input.page_index - 1) * input.page_size)
                            .Take(input.page_size);
            res.data = queryPage.ToList();
            return res;
        }

        public Notice GetNoticeItem(int id)
        {
            return _repository.FirstOrDefault(s => s.id == id);
        }

        public bool UpdateNotice(Notice model)
        {
            var entity = _repository.FirstOrDefault(s => s.id == model.id);
            entity.title = model.title;
            entity.msg = model.msg;
            _repository.Update(entity);
            return uof.SaveChange() > 0;
        }
    }
}

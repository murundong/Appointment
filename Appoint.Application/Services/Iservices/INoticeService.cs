using Appoint.EntityFramework.Data;
using Appoint.EntityFramework.ViewData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appoint.Application.Services
{
    public interface INoticeService : IApplicationService
    {
        Base_PageOutput<List<View_NoticeOutput>> GetNotice(View_NoticeInput input);

        Notice CreateNotice(Notice model);
        bool UpdateNotice(Notice model);

        bool DeleteNotice(int id);

        Notice GetNoticeItem(int id);
    }
}

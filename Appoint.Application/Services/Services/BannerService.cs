using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Appoint.EntityFramework;
using Appoint.EntityFramework.Data;
using Appoint.EntityFramework.DbContextProvider;
using Appoint.EntityFramework.Rep;
using Appoint.EntityFramework.Uow;
using Appoint.EntityFramework.ViewData;

namespace Appoint.Application.Services
{
    public class BannerService : IBannerService
    {
        public static IDbContextProvider<App_DbContext> _provider = new DbContextProvider<App_DbContext>();
        public IRepository<Banners> _repository = new RepositoryBase<App_DbContext, Banners>(_provider);
        public IUnitOfWork uof = new UnitOfWork<App_DbContext, IDbContextProvider<App_DbContext>>(_provider);

        public List<View_BannerOutput> GetBanners()
        {
            var res= _repository.GetAll().Where(s => s.active);
            return AutoMapper.Mapper.Map<List<View_BannerOutput>>(res);
        }
    }
}

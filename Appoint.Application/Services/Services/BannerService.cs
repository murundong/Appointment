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
        public IRepository<App_DbContext, Banners> _repository { get; set; }
        public IUnitOfWork<App_DbContext> uof { get; set; }

        public Banners CreateBanners(Banners model)
        {
            _repository.Insert(model);
            if (uof.SaveChange() > 0) return model;
            return null;
        }

        public bool DeleteBanner(int id)
        {
            _repository.Delete(new Banners() { id = id });
            return uof.SaveChange() > 0;
        }

        public Banners GetBannerById(int id)
        {
            return _repository.GetAll().FirstOrDefault(s => s.id == id);
        }

        public List<View_BannerOutput> GetBanners()
        {
            var res= _repository.GetAll().Where(s => s.active);
            return AutoMapper.Mapper.Map<List<View_BannerOutput>>(res);
        }

        public Base_PageOutput<List<Banners>> PageBanners(Base_PageInput input)
        {
            Base_PageOutput<List<Banners>> res = new Base_PageOutput<List<Banners>>();
            if (input == null) input = new Base_PageInput();
            var query = _repository.GetAll();
            res.total = query.Count();
            res.data = query.OrderByDescending(s => s.create_time)
                            .Skip((input.page_index - 1) * input.page_size)
                            .Take(input.page_size).ToList(); 
            return res;
        }

        public bool UpdateBanners(Banners model)
        {
            var entity = _repository.FirstOrDefault(s => s.id == model.id);
            entity.img = model.img;
            entity.img_type = model.img_type;
            entity.url = model.url;
            entity.active = model.active;
            _repository.Update(entity);
            return uof.SaveChange() > 0;
        }
    }
}

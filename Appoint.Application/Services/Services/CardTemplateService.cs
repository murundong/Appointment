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
    public class CardTemplateService : ICardTemplateService
    {
        public static IDbContextProvider<App_DbContext> _provider = new DbContextProvider<App_DbContext>();
        public IRepository<CardTemplate> _repository = new RepositoryBase<App_DbContext, CardTemplate>(_provider);
        public IRepository<Doors> _repositoryDoors = new RepositoryBase<App_DbContext, Doors>(_provider);
        public IUnitOfWork uof = new UnitOfWork<App_DbContext, IDbContextProvider<App_DbContext>>(_provider);

        public CardTemplate CreateCardTemplate(CardTemplate model)
        {
            _repository.Insert(model);
            if (uof.SaveChange() > 0) return model;
            return null;
        }

        public CardTemplate GetCardsTemplateById(int id)
        {
            return _repository.FirstOrDefault(s => s.id == id);
        }

        public List<ViewDoorCardsSelect> GetDoorCards(int doorId)
        {
            var res= _repository.GetAll().Where(s => s.door_id == doorId);
            return AutoMapper.Mapper.Map<List<ViewDoorCardsSelect>>(res.ToList());
        }

        public Base_PageOutput<View_CardTemplateOutputItem> PageCardTemplate(View_CardTemplateInput input)
        {
            Base_PageOutput<View_CardTemplateOutputItem> res = new Base_PageOutput<View_CardTemplateOutputItem>() {  data = new View_CardTemplateOutputItem() {  temps = new List<View_CardTemplateOutput>()} };
            if (input == null || input.door_id <= 0) return res;
            var query = _repository.GetAll()
                .Where(s => s.door_id == input.door_id);
            res.total = query.Count();
            var query_end = query.OrderByDescending(s=>s.create_time)
                .Skip((input.page_index - 1) * input.page_size)
                .Take(input.page_size);
            res.data.temps = AutoMapper.Mapper.Map<List<View_CardTemplateOutput>>(query_end.ToList());
            res.data.img = _repositoryDoors.FirstOrDefault(s=>s.id == input.door_id)?.door_img;
            return res;
        }

        public bool UpdateCardTemplate(CardTemplate model)
        {
            var entity = _repository.FirstOrDefault(s => s.id == model.id);
            if (entity == null || entity.id <= 0) return false;

            var time = entity.create_time;
            var oid = entity.create_openid;

            AutoMapper.Mapper.Map(model, entity);
            entity.create_time = time;
            entity.create_openid = oid;
            _repository.Update(entity);
            return uof.SaveChange() > 0;

        }
    }
}

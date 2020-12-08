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
    public class DoorsService : IDoorsService
    {
        public static IDbContextProvider<App_DbContext> _provider = new DbContextProvider<App_DbContext>();
        public IRepository<Doors> _repository = new RepositoryBase<App_DbContext, Doors>(_provider);
        public IUnitOfWork uof = new UnitOfWork<App_DbContext, IDbContextProvider<App_DbContext>>(_provider);

        public Doors CreateDoors(Doors model)
        {
            _repository.Insert(model);
            if (uof.SaveChange() > 0) return model;
            return null;
        }

        public List<View_DoorsOutput> GetDoors(View_DoorInput input)
        {
            if (input == null) input = new View_DoorInput();
            var res = _repository.GetAll().Where(s => s.active)
                .OrderByDescending(s=>s.create_time)
                .Skip((input.page_index - 1) * input.page_size)
                .Take(input.page_size);
            return AutoMapper.Mapper.Map<List<View_DoorsOutput>>(res.ToList());
        }

        public List<Doors> GetTeacherDoors(View_TeacherDoorInput input)
        {
            if (input == null || string.IsNullOrWhiteSpace(input.open_id)) return null;

            var res= _repository.GetAll()
                .Where(s=>s.create_openid == input.open_id)
                .OrderByDescending(s => s.create_time)
                 .Skip((input.page_index - 1) * input.page_size)
                .Take(input.page_size);
            return res.ToList();
        }
    }
}

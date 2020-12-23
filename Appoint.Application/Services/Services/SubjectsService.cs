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
    public class SubjectsService : ISubjectsService
    {

        public static IDbContextProvider<App_DbContext> _provider = new DbContextProvider<App_DbContext>();
        public IRepository<Subjects> _repository = new RepositoryBase<App_DbContext, Subjects>(_provider);
        //public IRepository<UserInfos> _repositoryUInfos = new RepositoryBase<App_DbContext, UserInfos>(_provider);
        public IUnitOfWork uof = new UnitOfWork<App_DbContext, IDbContextProvider<App_DbContext>>(_provider);

        public Subjects CreateSubject(Subjects model)
        {
            _repository.Insert(model);
            if (uof.SaveChange() > 0) return model;
            return null;

        }

        public Subjects GetSubjectById(int id)
        {
            return _repository.FirstOrDefault(s => s.id == id);
        }

        public Base_PageOutput<List<View_SubjectsOutput>> GetSubjects(View_SubjectsInput input)
        {
            Base_PageOutput<List<View_SubjectsOutput>> res = new Base_PageOutput<List<View_SubjectsOutput>>();
            var query = _repository.GetAll().Where(s => s.door_id == input.door_id);
            res.total = query.Count();
            var query_end = query.OrderByDescending(s => s.create_time)
                .Skip((input.page_index - 1) * input.page_size)
                .Take(input.page_size);
            var lst = AutoMapper.Mapper.Map<List<View_SubjectsOutput>>(query_end.ToList());
            res.data = lst;
            return res;
        }

        public bool UpdateSubject(Subjects model)
        {
            var entity = _repository.FirstOrDefault(s => s.id == model.id);
            var oid = entity.create_openid;
            var time = entity.create_time;
            AutoMapper.Mapper.Map(model, entity);
            entity.create_time = time;
            entity.create_openid = oid;
            _repository.Update(entity);
            return uof.SaveChange() > 0;
        }
    }
}

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
        public IRepository<App_DbContext, Subjects> _repository { get; set; }
        public IUnitOfWork<App_DbContext> uof { get; set; }

        public Subjects CreateSubject(Subjects model)
        {
            _repository.Insert(model);
            if (uof.SaveChange() > 0) return model;
            return null;

        }

        public List<string> GetDoorTags(int? doorId)
        {
            List<string> res = new List<string>();
            var query= _repository.GetAll().Where(s => s.door_id == doorId && s.active && s.subject_tag!=null && s.subject_tag!="")?.Select(s => s.subject_tag);
            if(query!=null && query.Count() > 0)
            {
                query.ToList().ForEach(s =>
                {
                    res.AddRange(s.Split(','));
                });
            }
            return res;
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
            var lst = AutoMapper.Mapper.Map<List<View_SubjectsOutput>>(query_end);
            res.data = lst;
            return res;
        }

        public List<View_SubjectsOutput> GetSubjectsByDoorID(int doorId)
        {
            var query= _repository.GetAll().Where(s => s.door_id == doorId);
            return AutoMapper.Mapper.Map<List<View_SubjectsOutput>>(query);
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

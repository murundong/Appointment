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
    public class CoursesService : ICoursesService
    {
        public static IDbContextProvider<App_DbContext> _provider = new DbContextProvider<App_DbContext>();
        public IRepository<Courses> _repository = new RepositoryBase<App_DbContext, Courses>(_provider);
        public IRepository<Subjects> _repositorySubject = new RepositoryBase<App_DbContext, Subjects>(_provider);
        public IUnitOfWork uof = new UnitOfWork<App_DbContext, IDbContextProvider<App_DbContext>>(_provider);

        public Courses CreateCourse(Courses model)
        {
            _repository.Insert(model);
            if (uof.SaveChange() > 0) return model;
            return null;
        }

        public Courses GetCourseById(int id)
        {
            return _repository.FirstOrDefault(s => s.id == id);
        }

        public Base_PageOutput<List<View_CoursesOutput>> GetCourses(View_CoursesInput input)
        {
            Base_PageOutput<List<View_CoursesOutput>> res = new Base_PageOutput<List<View_CoursesOutput>>();

            var query = _repository.GetAll().Where(s => s.door_id == input.door_id);
            if (!string.IsNullOrWhiteSpace(input.date))
            {
                query = query.Where(s => s.course_date == input.date);
            }
            res.total = query.Count();
            var query_end = query.OrderByDescending(s => s.create_time)
                .Skip((input.page_index - 1) * input.page_size)
                .Take(input.page_size);
            var lst = AutoMapper.Mapper.Map<List<View_CoursesOutput>>(query_end.ToList());
            if(lst!=null && lst.Count > 0)
            {
                lst.ForEach(s =>
                {
                    var sub_item = _repositorySubject.FirstOrDefault(p => p.id == s.subject_id);
                    if(sub_item!=null && sub_item.id > 0)
                    {
                        s.Subject = AutoMapper.Mapper.Map<View_SubjectsOutput>(sub_item);
                    }
                });
            }
            res.data = lst;
            return res;
        }

        public bool UpdateCourse(Courses model)
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

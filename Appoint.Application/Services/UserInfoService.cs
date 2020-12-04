using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Appoint.EntityFramework.ViewData;
using Appoint.EntityFramework.DbContextProvider;
using Appoint.EntityFramework;
using Appoint.EntityFramework.Rep;
using Appoint.EntityFramework.Data;
using Appoint.EntityFramework.Uow;
using AutoMapper;

namespace Appoint.Application.Services
{
    public class UserInfoService : IUserInfoService
    {
        public static IDbContextProvider<App_DbContext> _provider = new DbContextProvider<App_DbContext>();
        public IRepository<UserInfo> _repository = new RepositoryBase<App_DbContext, UserInfo>(_provider);
        public IUnitOfWork uof = new UnitOfWork<App_DbContext, IDbContextProvider<App_DbContext>>(_provider);


        public View_UinfoOutput GetUserInfo(string openid)
        {
            var res = _repository.FirstOrDefault(s => s.open_id == openid);
            return Mapper.Map<View_UinfoOutput>(res);
        }

        public bool CheckHasUser(string openid)
        {
            return _repository.Count(s => s.open_id == openid) > 0;
        }

        public void AddUser(UserInfo model)
        {
            _repository.Insert(model);
        }

        public bool SaveUserInfo(UserInfo model)
        {
             _repository.Insert(model);
            return uof.SaveChange() > 0;
        }
    }
}

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
        public IRepository<UserInfos> _repository = new RepositoryBase<App_DbContext, UserInfos>(_provider);
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

        public void AddUser(UserInfos model)
        {
            _repository.Insert(model);
        }

        public bool SaveUserInfo(UserInfos model)
        {
             _repository.Insert(model);
            return uof.SaveChange() > 0;
        }

      
        public View_UinfoOutput UpdateUserInfo_home(UserInfos model)
        {
            var item = _repository.FirstOrDefault(s => s.open_id == model.open_id);
            if (item==null || item.uid<=0)
            {
                _repository.Insert(model);
                uof.SaveChange();
                return Mapper.Map<View_UinfoOutput>(model);
            }
            else
            {
                item.avatar = model.avatar;
                item.gender = model.gender;
                item.nick_name = model.nick_name;
                _repository.Update(item);
                return Mapper.Map<View_UinfoOutput>(item);
            }
            
        }

        public View_UinfoOutput UpdateUserInfo_setting(UserInfos model)
        {
            var item = _repository.FirstOrDefault(s => s.open_id == model.open_id);
            if (item?.uid <= 0)
            {
                _repository.Insert(model);
                uof.SaveChange();
                return Mapper.Map<View_UinfoOutput>(model);
            }
            else
            {
                item.avatar = model.avatar;
                item.birthday =  model.birthday ;
                item.tel = model.tel ;
                item.real_name = model.real_name ;
                item.gender = model.gender;
                item.nick_name = model.nick_name;
                _repository.Update(item);
                return Mapper.Map<View_UinfoOutput>(item);
            }
            
        }

        public View_UinfoOutput GetUinfoByOpenid(string openid)
        {
            var res = _repository.FirstOrDefault(s => s.open_id == openid);
            return Mapper.Map<View_UinfoOutput>(res);
        }
    }
}

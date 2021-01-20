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
using Appoint.EntityFramework.Enum;

namespace Appoint.Application.Services
{
    public class UserInfoService : IUserInfoService
    {
        public IRepository<App_DbContext, UserInfos> _repository { get; set; }
        public IUnitOfWork<App_DbContext> uof { get; set; }


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
                item.initial = model.initial;
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

        public View_InitialUserInfoOutput GetUserLst_Admin(string nick)
        {
            View_InitialUserInfoOutput return_res = new View_InitialUserInfoOutput();
            var query = _repository.GetAll();
            if (!string.IsNullOrWhiteSpace(nick)) query = query.Where(s => s.nick_name.Contains(nick));
            var res = Mapper.Map<List<View_UinfoOutput>>(query.OrderBy(s=>s.initial));
            List<string> lstLetters = res.Select(s => s.initial)?.Distinct()?.ToList();
            return_res.initials = lstLetters;
            if (lstLetters?.Count > 0)
            {
                lstLetters.ForEach(s =>
                {
                    View_InitialUserInfoItemOutput item = new View_InitialUserInfoItemOutput()
                    {
                        initial = s,
                        uinfos = res.Where(p => p.initial == s)?.ToList()
                    };
                    return_res.uinfos.Add(item);
                });
            }
            return return_res;
        }

        public bool AllocRole(int uid, Enum_UserRole role)
        {
            var entity = _repository.FirstOrDefault(s => s.uid == uid);
            if(entity!=null && entity.uid > 0)
            {
                entity.role = role;
                _repository.Update(entity);
                return uof.SaveChange()>0;
            }
            return false;
        }
    }
}

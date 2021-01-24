using Appoint.EntityFramework;
using Appoint.EntityFramework.Data;
using Appoint.EntityFramework.Enum;
using Appoint.EntityFramework.Rep;
using Appoint.EntityFramework.Uow;
using Appoint.EntityFramework.ViewData;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appoint.Application.Services
{
    public class UserCardService : IUserCardService
    {
        public IRepository<App_DbContext, UserCards> _repository { get; set; }
        public IRepository<App_DbContext, View_UserCardsInfoOutput> _repositorySql { get; set; }
        public IUnitOfWork<App_DbContext> uof { get; set; }
        public void AddUserAttention(string openid, int doorid)
        {
            string sql = @"merge into  [dbo].[UserCards] T
            using (select door_id=@door_id,uid=(select top 1 uid from [dbo].[UserInfos] where open_id=@open_id)) S
            on T.door_id = S.door_id and T.uid = S.uid
            when not matched then
	            insert (door_id,uid,role,create_time) values(S.door_id,s.uid,0,getdate());";
            var sqlParam = new SqlParameter[] {
                new SqlParameter("@door_id",doorid),
                new SqlParameter("@open_id",openid)
            };
            _repository.ExecuteSqlCommand(sql, sqlParam);
        }

       
        public View_InitialUserCardsInfoOutput GetUserLst_Door(int doorid, string nick)
        {
            View_InitialUserCardsInfoOutput return_res = new View_InitialUserCardsInfoOutput();
            string sql = @"select id,A.uid,B.role,[door_role]=A.role,[door_remark]=A.remark,cid,ctype,card_sttime,card_edtime,stop_day,extend_day,effective_time,limit_week_time,limit_day_time,is_freeze,
	                             open_id,nick_name,avatar,gender,tel,birthday,real_name,initial from  [dbo].[UserCards] A
                            left join [dbo].[UserInfos] B
                            on A.uid = B.uid
                            where door_id=@doorid and ( nick_name like @nick or A.remark like @nick ) order by B.initial;";
            var sqlParm = new SqlParameter[] {
                new SqlParameter("@doorid",doorid),
                new SqlParameter("@nick",$"%{nick}%"),
            };
            var query = _repositorySql.ExecuteSqlQuery(sql, sqlParm)?.ToList();
            List<string> LstLetters= query.Select(s => s.initial)?.Distinct()?.ToList();
            return_res.initials = LstLetters;
            if (LstLetters?.Count > 0)
            {
                LstLetters.ForEach(s =>
                {
                    View_InitialUserCardsInfoItemOutput item = new View_InitialUserCardsInfoItemOutput() {
                        initial = s,
                        uinfos = query.Where(p => p.initial == s)?.ToList()
                    };
                    return_res.uinfos.Add(item); 
                });
            }
            return return_res;
        }

        public bool SetUSerRemark(int uid, string remark)
        {
            var entity = _repository.FirstOrDefault(s => s.id == uid);
            if (entity != null && entity.uid > 0)
            {
                entity.remark = remark;
                _repository.Update(entity);
                return uof.SaveChange() > 0;
            }
            return false;
        }
        public bool AllocRole(int uid, Enum_UserRole role)
        {
            var entity = _repository.FirstOrDefault(s => s.id == uid);
            if (entity != null && entity.uid > 0)
            {
                entity.role = role;
                _repository.Update(entity);
                return uof.SaveChange() > 0;
            }
            return false;
        }

    }
}


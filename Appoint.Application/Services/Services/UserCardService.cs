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
        public IRepository<App_DbContext,Doors> _repositoryDoors { get; set; }
        public IRepository<App_DbContext, View_UserCardsInfoOutput> _repositorySql { get; set; }
        public IRepository<App_DbContext, View_LstUserAllCardsOutput_CardsInfo> _repositoryUserCardSql { get; set; }
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

        public List<View_LstUserAllCardsOutput> GetUserALlCards(string openid,Enum_CardStatus cardStatus)
        {
            List<View_LstUserAllCardsOutput> res = new List<View_LstUserAllCardsOutput>();

            string sql = @"select card_name,card_desc, A.* from [dbo].[UserCards] A 
	            left join  [dbo].[CardTemplate] B on A.cid = B.id 
	            Left join  [dbo].[UserInfos] C on A.uid = C.uid
            where A.cid <> null and C.open_id = @open_id ";

            switch (cardStatus)
            {
                case Enum_CardStatus.Expired:
                    sql += " and stop_day <= GETDATE()";
                    break;
                case Enum_CardStatus.Freezed:
                    sql += " and is_freeze=1";
                    break;
                default:
                    break;
            }

            var sqlParm = new SqlParameter[] {
                new SqlParameter("@open_id",openid),
            };
            List<View_LstUserAllCardsOutput_CardsInfo> uCardLst = _repositoryUserCardSql.ExecuteSqlQuery(sql, sqlParm)?.ToList();
            
            List<int> lstDoors = new List<int>();
            if (uCardLst?.Count > 0)
            {
                lstDoors = uCardLst.Select(s => s.door_id).Distinct().ToList();
            }
            if (lstDoors?.Count > 0)
            {
                var query = _repositoryDoors.GetAll().Where(s => lstDoors.Contains(s.id));
                lstDoors.ForEach(s =>
                {
                    View_LstUserAllCardsOutput item = new View_LstUserAllCardsOutput();
                    var queryItem = query.FirstOrDefault(p => p.id == s);
                    item.door_id = s;
                    item.door_name = queryItem?.door_name;
                    item.door_img = queryItem?.door_img;
                    item.CardsInfo = uCardLst.Where(p => p.door_id == s)?.ToList();
                });
            }
            return res;
        }
    }
}


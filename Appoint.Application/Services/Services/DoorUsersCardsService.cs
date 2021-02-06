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
    public class DoorUsersCardsService : IDoorUsersCardsService 
    {
        public IRepository<App_DbContext, DoorUsersCards> _repository { get; set; }
        public IRepository<App_DbContext,Doors> _repositoryDoors { get; set; }
        public IRepository<App_DbContext, DoorUsers> _repositoryDoorUsers { get; set; }
        public IRepository<App_DbContext, View_UserCardsInfoOutput> _repositorySql { get; set; }
        public IUnitOfWork<App_DbContext> uof { get; set; }


       
        public View_InitialUserCardsInfoOutput GetUserLst_Door(int doorid, string nick)
        {
            View_InitialUserCardsInfoOutput return_res = new View_InitialUserCardsInfoOutput();
            string sql = @"select A.id,A.door_id,A.uid,[door_role]=A.role,[door_remark]=A.remark,B.open_id,B.nick_name,B.avatar,B.gender,B.role,B.tel,B.initial,B.real_name,B.birthday from [dbo].[DoorUsers] A
		    left join [dbo].[UserInfos] B
		    on A.uid = B.uid
		    where A.door_id = @doorid and (nick_name like @nick or A.remark like @nick) order by initial;";
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

      

        public List<View_LstUserAllCardsOutput> GetUserALlCards(string openid,Enum_CardStatus cardStatus)
        {
            List<View_LstUserAllCardsOutput> res = new List<View_LstUserAllCardsOutput>();

            string sql = @"select  A.id,A.door_id,[uid]=A.du_id,A.cid,A.ctype,A.card_sttime,A.card_edtime,A.effective_time,A.limit_week_time,A.limit_day_time,A.is_freeze,A.freeze_edtime
                        ,C.card_name,C.card_desc,
                        B.door_img from [dbo].[DoorUsersCards] A
                        left join [dbo].[Doors] B
                        on A.door_id = B.id
                        left join [dbo].[CardTemplate] C
                        on A.cid = C.id
                        left join [dbo].[UserInfos] E
                        on A.uid = E.uid
                        where  A.cid is not null and E.open_id = @openid and A.is_delete=0
                      ";
            switch (cardStatus)
            {
                case Enum_CardStatus.Expired:
                    sql += " and card_edtime <= GETDATE()";
                    break;
                case Enum_CardStatus.Freezed:
                    sql += " and is_freeze=1";
                    break;
                default:
                    sql += " and (card_edtime > GETDATE() or card_edtime is null) and is_freeze =0";
                    break;
            }
            sql += "  order by A.id desc ;";
            var sqlParm = new SqlParameter[] {
                new SqlParameter("@openid",openid),
            };
            List<View_UserCardsInfoOutput> uCardLst = _repositorySql.ExecuteSqlQuery(sql, sqlParm)?.ToList();
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
                    res.Add(item);
                });
            }
            return res;
        }



        public List<View_UserCardsInfoOutput> GetUserDoorCards(int uid, int doorId)
        {
            List<View_UserCardsInfoOutput> res = new List<View_UserCardsInfoOutput>();
            string sql = @"select A.id,A.door_id,[uid]=A.du_id,A.cid,A.ctype,A.card_sttime,A.card_edtime,A.effective_time,A.limit_week_time,A.limit_day_time,A.is_freeze,A.freeze_edtime
                        ,C.card_name,C.card_desc,
                        B.door_img from [dbo].[DoorUsersCards] A
                        left join [dbo].[Doors] B
                        on A.door_id = B.id
                        left join [dbo].[CardTemplate] C
                        on A.cid = C.id
                        where A.cid is not null and A.uid = @uid and A.door_id = @doorId and A.is_delete=0
                        order by A.id desc ;";
            var sqlParm = new SqlParameter[] {
                new SqlParameter("@uid",uid),
                new SqlParameter("@doorId",doorId),
            };
            res = _repositorySql.ExecuteSqlQuery(sql, sqlParm)?.ToList();
            return res;
        }

        public View_UserCardsInfoOutput GetUserCardsInfo(int? id)
        {
            string sql = @"select  A.* ,[door_remark]= B.remark,avatar,nick_name
			                from  [dbo].[DoorUsersCards] A
			                left join [dbo].[DoorUsers] B
			                on A.du_id = B.id
			                left join [dbo].[UserInfos] C
			                on B.uid = C.uid
			                where A.id=@id ";
            var sqlParm = new SqlParameter[] {
                new SqlParameter("@id",id),
            };
            return _repositorySql.ExecuteSqlQuery(sql, sqlParm)?.FirstOrDefault();
        }

        public bool AddUserCards(DoorUsersCards model)
        {
            _repository.Insert(model);
            var entity = _repositoryDoorUsers.FirstOrDefault(s => s.id == model.du_id);
            if(entity!=null && entity.id > 0)
            {
                if(entity.role == Enum_UserRole.Tourist)
                {
                    entity.role = Enum_UserRole.Member;
                    _repositoryDoorUsers.Update(entity);
                }
            }
            return uof.SaveChange() > 0;
        }
        public bool DeleteUserCards(int? id)
        {
            var entity = _repository.FirstOrDefault(s => s.id == id);
            if(entity!=null && entity.id > 0)
            {
                entity.is_delete = true;
                _repository.Update(entity);
            }
            return uof.SaveChange() > 0;
        }
        public bool UpdateUserCardsInfo(DoorUsersCards model)
        {
            var entity = _repository.FirstOrDefault(s => s.id == model.id);
            if (entity != null && entity.id > 0)
            {
                entity.cid = model.cid;
                entity.ctype = model.ctype;
                entity.card_sttime = model.card_sttime;
                entity.card_edtime = model.card_edtime;
                entity.effective_time = model.effective_time;
                entity.limit_week_time = model.limit_week_time;
                entity.limit_day_time = model.limit_day_time;
                entity.freeze_edtime = model.freeze_edtime;
                entity.is_freeze = model.is_freeze;
                _repository.Update(entity);
                return uof.SaveChange() > 0;
            }
            return false;
        }
    }
}


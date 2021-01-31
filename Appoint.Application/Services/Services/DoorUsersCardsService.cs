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
                        left join [dbo].[DoorUsers] D
                        on A.du_id = D.id
                        left join [dbo].[UserInfos] E
                        on D.uid = E.uid
                        where  A.cid is not null and E.open_id = @openid  ";
            switch (cardStatus)
            {
                case Enum_CardStatus.Expired:
                    sql += " and card_edtime <= GETDATE()";
                    break;
                case Enum_CardStatus.Freezed:
                    sql += " and is_freeze=1";
                    break;
                default:
                    break;
            }

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
                        left join [dbo].[DoorUsers] D
                        on A.du_id = D.id
                        where A.cid is not null and D.uid = @uid and A.door_id = @doorId";
            var sqlParm = new SqlParameter[] {
                new SqlParameter("@uid",uid),
                new SqlParameter("@doorId",doorId),
            };
            res = _repositorySql.ExecuteSqlQuery(sql, sqlParm)?.ToList();
            return res;
        }

        public View_UserCardsInfoOutput GetUserInfoById(int? id)
        {
            string sql = @"select A.id,A.door_id,A.uid,[door_role]=A.role, [door_remark]= A.remark,
                        open_id,nick_name,avatar,gender,C.role,tel,initial,real_name,birthday,
                        B.cid,E.card_name,E.card_desc,D.door_img,
                        B.ctype,B.card_sttime,B.card_edtime,B.effective_time,B.limit_week_time,B.limit_day_time,B.is_freeze,B.freeze_edtime
                        from [dbo].[DoorUsers] A
                        left join [dbo].[DoorUsersCards] B
                        on A.id = B.du_id
                        left join [dbo].[UserInfos] C
                        on A.uid = C.uid
                        left join [dbo].[Doors] D
                        on A.door_id = D.id
                        left join [dbo].[CardTemplate] E
                        on B.cid = E.id
                        where A.id=@id ";
            var sqlParm = new SqlParameter[] {
                new SqlParameter("@id",id),
            };
            return _repositorySql.ExecuteSqlQuery(sql, sqlParm)?.FirstOrDefault();
        }

        public bool AddUserCards(DoorUsersCards model)
        {
            _repository.Insert(model);
            return uof.SaveChange() > 0;
        }

        //public bool UpdateUserCardsInfo(DoorUsersCards model)
        //{
        //    var entity= _repository.FirstOrDefault(s => s.id == model.id); 
        //    if(entity!=null && entity.id > 0)
        //    {
        //        entity.cid = model.cid;
        //        entity.role = Enum_UserRole.Member;
        //        entity.ctype = model.ctype;
        //        entity.card_sttime = model.card_sttime;
        //        entity.card_edtime = model.card_edtime;
        //        entity.effective_time = model.effective_time;
        //        entity.limit_week_time = model.limit_week_time;
        //        entity.limit_day_time = model.limit_day_time;
        //        entity.freeze_edtime = model.freeze_edtime;
        //        entity.is_freeze = model.is_freeze;
        //        _repository.Update(entity);
        //        return uof.SaveChange() > 0;
        //    }
        //    return false;
        //}
    }
}


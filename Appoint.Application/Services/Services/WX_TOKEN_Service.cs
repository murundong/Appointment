using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Appoint.EntityFramework;
using Appoint.EntityFramework.Data;
using Appoint.EntityFramework.Rep;
using Appoint.EntityFramework.Uow;

namespace Appoint.Application.Services
{
    public class WX_TOKEN_Service : IWX_TOKEN_Service
    {
        public IRepository<App_DbContext, WX_TOKEN> _repository { get; set; }
        public IUnitOfWork<App_DbContext> uof { get; set; }

        public WX_TOKEN GetToken(string appid)
        {
            return _repository.FirstOrDefault(s=>s.appid==appid);
        }

        public bool InsertOrUpdateToken(WX_TOKEN model)
        {
            string sql = @"merge into  [dbo].[WX_TOKEN] T
                using (select access_token=@access_token,expires_in=@expires_in,appid=@appid) S
				on T.appid=S.appid
				when matched then
					update set T.access_token=S.access_token,T.expires_in =S.expires_in,T.create_time=getdate()
                when not matched then
	                insert (appid,access_token,expires_in,create_time) values(S.appid,S.access_token,S.expires_in,getdate());";
            try
            {
                SqlParameter[] SqlParm = new SqlParameter[]
                {
                    new SqlParameter("@access_token",model.access_token),
                    new SqlParameter("@expires_in",model.expires_in),
                    new SqlParameter("@appid",model.appid),
                };
                return _repository.ExecuteSqlCommand(sql, SqlParm) > 0;
            }
            catch (Exception ex) 
            {
            }
            return false;
        }
    }
}

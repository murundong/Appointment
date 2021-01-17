using Appoint.Application.IWeixinApi;
using Appoint.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using Newtonsoft.Json;
using System.Text;
using Appoint.Web.Models;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using Appoint.EntityFramework.DbContextProvider;
using Appoint.EntityFramework;
using BaseClasses;

namespace Appoint.Web.Base
{
    [ControllerBase]
    public class ApiControllerBase: ApiController
    {

        public IWeixinService _weixinService = new WeixinService();
        public IUserInfoService _userInfoService = new UserInfoService();
        public IBannerService _bannerService = new BannerService();
        public IDoorsService _doorService = new DoorsService();
        public ICardTemplateService _cardTemplateService = new CardTemplateService();
        public ISubjectsService _subjectService = new SubjectsService();
        public ICoursesService _courseService = new CoursesService();
        public IUserCardService _userCardService = new UserCardService();
        public static string errImg = ConfigurationHelper.GetAppSetting<string>("ErrorImg");
        IDbContextProvider<App_DbContext> _provider = new DbContextProvider<App_DbContext>();

        public IHttpActionResult ReturnJsonResult()
        {
            return Json(new RRETURN<object>() { errCode = 0, msg = null, data = null });
        }
        public IHttpActionResult ReturnJsonResult(string msg,int errCode)
        {
            return Json(new RRETURN<object>() { errCode = errCode, msg = msg, data = null });
        }

        public IHttpActionResult ReturnJsonResult( object t = null)
        {
            return Json(new RRETURN<object>() { errCode = 0, msg = null, data = t });
        }


        public IHttpActionResult ReturnJsonResult(int errCode=0,string msg=null ,object t =null )
        {
            return Json(new RRETURN<object>() { errCode = errCode, msg = msg, data = t });
        }

        protected override JsonResult<T> Json<T>(T content, JsonSerializerSettings serializerSettings, Encoding encoding)
        {
            
            _provider.Release();
            return base.Json(content, serializerSettings, encoding);
        }
    }
}
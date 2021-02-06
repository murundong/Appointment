using Appoint.Application.IWeixinApi;
using Appoint.Application.Services;
using Appoint.EntityFramework.Enum;
using BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace AppointMvc.Web
{
    public class ControllerBase : Controller
    {

        public IWeixinService _weixinService { get; set; }
        public IUserInfoService _userInfoService { get; set; }
        public IBannerService _bannerService { get; set; }
        public IDoorsService _doorService { get; set; }
        public ICardTemplateService _cardTemplateService { get; set; }
        public ISubjectsService _subjectService { get; set; }
        public ICoursesService _courseService { get; set; }
        public IDoorUsersService _doorUserService { get; set; }
        public IDoorUsersCardsService  _doorUserCardService { get; set; }
        public IDoorUsersAppointsService _doorUserAppointService { get; set; }
        public static string errImg = ConfigurationHelper.GetAppSetting<string>("ErrorImg");

        //protected override JsonResult Json(object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior)
        //{
        //    return new JsonNetResult(data, contentType, contentEncoding, behavior);
        //}
        protected override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);

            if (filterContext.Exception != null)
            {
                Log.Error(filterContext.Exception.Message, filterContext.Exception);
                filterContext.Result = new JsonResult()
                {
                    Data = new { errCode = Enum_ReturnRes.Fail, msg = filterContext.Exception.Message },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
        }

        public JsonResult ReturnJsonResult(object data, string msg = "", Enum_ReturnRes errCode = Enum_ReturnRes.Success)
        {
            return Json(new { errCode = errCode, msg = (string.IsNullOrWhiteSpace(msg) ? errCode.ToString() : msg), data = data }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult ReturnPageResult<T>(T data)
        {
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ReturnJsonResult(string msg = "", Enum_ReturnRes errCode = Enum_ReturnRes.Success)
        {
            return ReturnJsonResult(null, msg, errCode);
        }

        public JsonResult ReturnJsonResult()
        {
            return ReturnJsonResult("", Enum_ReturnRes.Success);
        }
    }
}
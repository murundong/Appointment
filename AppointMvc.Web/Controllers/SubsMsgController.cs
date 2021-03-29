using Appoint.EntityFramework;
using Appoint.EntityFramework.Data;
using Appoint.EntityFramework.Enum;
using Appoint.EntityFramework.WeixData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppointMvc.Web.Controllers
{
    public class SubsMsgController : ControllerBase
    {
        public ActionResult AddUserSubs(DoorUsersSubsMsg model)
        {
            if (!_subsMsgService.AddData(model)) return ReturnJsonResult("订阅失败！", Enum_ReturnRes.Fail);
            return ReturnJsonResult();
        }

        public ActionResult TestSubs()
        {

            var res= _courseService.GetAllCourse();
            //W_SUBS_DATA_INPUT data = new W_SUBS_DATA_INPUT()
            //{
            //    touser = "odMBJ49aydSHPVtW1VmrlanhFj14",
            //    page = $"pages/Lesson/Lesson?doorId=10000&doorName=是个场馆",
            //    access_token = GetNowToken(),
            //    template_id = ConstConfig.template_cancel,
            //    data =new   { thing6 = new { value = "是个课程" } , date2 =new { value= "2021年3月28日" }, thing4 =new {value= "人数不足自动取消" } },
            //};
            //W_SUBS_DATA_INPUT data = new W_SUBS_DATA_INPUT()
            //{
            //    touser = "odMBJ49aydSHPVtW1VmrlanhFj14",
            //    page = $"pages/appointment/appointment",
            //    template_id = ConstConfig.template_notice,
            //    access_token = GetNowToken(),
            //    data = new { thing1 = new { value = "是个课程" }, thing3 = new { value = "还远瑜伽" }, character_string2 = new { value =DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") }, thing4 = new { value = $"课程开始前60分钟内不可取消" } }
            //};
            //var res = _weixinService.SendSubsCribe(data);
            return ReturnJsonResult(res);
        }

        string GetNowToken()
        {
            string token = null, appid = ConstConfig.APPID;
            var TOKENITEM = _tokenService.GetToken(appid);
            if (TOKENITEM == null || string.IsNullOrWhiteSpace(TOKENITEM?.access_token) || TOKENITEM.create_time.AddSeconds(TOKENITEM.expires_in) <= DateTime.Now.AddMinutes(-10))
            {
                var WXTOKEN = _weixinService.GetToken();
                if (WXTOKEN != null && WXTOKEN.errcode == 0)
                {
                    token = WXTOKEN.access_token;
                    _tokenService.InsertOrUpdateToken(new WX_TOKEN() { appid = appid, access_token = WXTOKEN.access_token, expires_in = WXTOKEN.expires_in });
                }
            }
            else token = TOKENITEM.access_token;
            return token;
        }
    }
}
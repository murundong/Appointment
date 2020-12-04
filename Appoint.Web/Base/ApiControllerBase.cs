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

namespace Appoint.Web.Base
{
    [ControllerBase]
    public class ApiControllerBase: ApiController
    {

        public IWeixinService _weixinService = new WeixinService();
        public IUserInfoService _userInfoService = new UserInfoService();


        protected override JsonResult<T> Json<T>(T content, JsonSerializerSettings serializerSettings, Encoding encoding)
        {
            return base.Json<T>(content, serializerSettings, encoding);

        }
        
    }
}
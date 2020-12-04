using Appoint.EntityFramework;
using Appoint.EntityFramework.DbContextProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Appoint.Web.Base
{
    public class ControllerBase: ActionFilterAttribute
    {
        public IDbContextProvider<App_DbContext> _provider = new DbContextProvider<App_DbContext>();
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            base.OnActionExecuting(actionContext);
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);
            _provider.Release();
        }
    }
}
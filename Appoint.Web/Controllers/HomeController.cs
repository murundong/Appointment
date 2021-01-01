using Appoint.Web.Base;
using BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.IO;
using System.Web.Hosting;
using Appoint.Application.Services;

namespace Appoint.Web.Controllers
{
    public class HomeController : ApiControllerBase
    {
      
        public IHttpActionResult UploadFile()
        {
            string dir = ConfigurationHelper.GetAppSetting<string>("UploadFile");
            string date = DateTime.Now.ToString("yyyyMMdd");
            string realPath = Path.Combine(dir, date);
            string dirName= HostingEnvironment.MapPath(realPath);
            if (!Directory.Exists(dirName))
            {
                Directory.CreateDirectory(dirName);
            }
            var files = this.ReadFileCollection();
            foreach (var item in files)
            {
                item.SaveAs(Path.Combine(dirName, item.fileName));
            }
            return ReturnJsonResult(t:string.Join(",",files.Select(s=> Path.Combine(realPath+ s.fileName))));
        }
    }
}

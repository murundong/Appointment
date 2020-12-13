using Appoint.Web.Base;
using BaseClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Appoint.Web.Controllers
{
    public class HomemvcController : Controller
    {
        // GET: Homemvc
        public ActionResult UploadFile()
        {
            string dir = ConfigurationHelper.GetAppSetting<string>("UploadFile");
            string date = DateTime.Now.ToString("yyyyMMdd");
            string realPath = Path.Combine(dir, date);
            string dirName = Request.MapPath(realPath);
            List<string> lstRealNames = new List<string>();
            if (!Directory.Exists(dirName))
            {
                Directory.CreateDirectory(dirName);
            }
            var files = Request.Files;
            if(files.Count>0)
            {
                foreach (string item in files)
                {
                    HttpPostedFileBase itemFile = Request.Files[item] as HttpPostedFileBase;
                    // item.SaveAs(Path.Combine(dirName, item.fileName));
                    string fileName = Path.Combine(dirName, itemFile.FileName);
                    lstRealNames.Add(Path.Combine(realPath , itemFile.FileName).Replace(@"\",@"//"));
                    itemFile.InputStream.SaveFile(fileName);
                }
            }
            return Json(new { data = string.Join(",", lstRealNames) }, JsonRequestBehavior.AllowGet);
        }
    }
}
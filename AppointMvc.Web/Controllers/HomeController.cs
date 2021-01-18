using Appoint.Application.Services;
using BaseClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppointMvc.Web.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
       public IOssService ossService { get; set; }
        // GET: Homemvc
        public ActionResult UploadFile()
        {
            string dir = ConfigurationHelper.GetAppSetting<string>("UploadFile");
            string OssDir = ConfigurationHelper.GetAppSetting<string>("BucketDir");
            bool UseOSS = ConfigurationHelper.GetAppSetting<bool>("UseOSS");
            string date = DateTime.Now.ToString("yyyyMMdd");
            string realPath = Path.Combine(dir, date);
            string dirName = Request.MapPath(realPath);
            List<string> lstRealNames = new List<string>();
            var files = Request.Files;
            if (files.Count > 0)
            {
                foreach (string item in files)
                {
                    HttpPostedFileBase itemFile = Request.Files[item] as HttpPostedFileBase;
                    string fileName = Path.Combine(dirName, itemFile.FileName);

                    if (UseOSS)
                    {
                        string ossUploadPath = $"{OssDir}{itemFile.FileName}";
                        ossService.UploadFile(ossUploadPath, itemFile.InputStream);
                        lstRealNames.Add(ossUploadPath);
                    }
                    else
                    {
                        if (!Directory.Exists(dirName))
                        {
                            Directory.CreateDirectory(dirName);
                        }
                        itemFile.InputStream.SaveFile(fileName);
                        lstRealNames.Add(Path.Combine(realPath, itemFile.FileName).Replace(@"\", @"//"));
                    }
                }
            }
            return Json(new { data = string.Join(",", lstRealNames) }, JsonRequestBehavior.AllowGet);
        }
    }
}
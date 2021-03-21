using Appoint.EntityFramework.Data;
using Appoint.EntityFramework.Enum;
using Appoint.EntityFramework.ViewData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppointMvc.Web.Controllers
{
    public class BannerController : ControllerBase
    {

        [HttpPost]
        public ActionResult CreateBanner(Banners model)
        {
            if (model == null || string.IsNullOrWhiteSpace( model.img)) return ReturnJsonResult("新增失败，参数错误！", Enum_ReturnRes.Fail);
            var res = _bannerService.CreateBanners(model);
            if (res != null) return ReturnJsonResult(res);
            return ReturnJsonResult("新增失败！", Enum_ReturnRes.Fail);
        }


        [HttpPost]
        public ActionResult UpdateBanner(Banners model)
        {
            if (model == null|| model.id<=0 || string.IsNullOrWhiteSpace(model.img)) return ReturnJsonResult("新增失败，参数错误！", Enum_ReturnRes.Fail);
            if (_bannerService.UpdateBanners(model)) return ReturnJsonResult();
            return ReturnJsonResult("更新失败！", Enum_ReturnRes.Fail);
        }

        public ActionResult DeleteBanner(int? id)
        {
            if (!id.HasValue || id <= 0) return ReturnJsonResult("参数错误！", Enum_ReturnRes.Fail);
            if (!_bannerService.DeleteBanner((int)id)) return ReturnJsonResult("删除失败！", Enum_ReturnRes.Fail);
            return ReturnJsonResult();
        }

        public ActionResult GetBannerItem(int? id)
        {
            if (!id.HasValue || id <= 0) return ReturnJsonResult("参数错误！", Enum_ReturnRes.Fail);
            var res = _bannerService.GetBannerById((int)id);
            return ReturnJsonResult(res);
        }

      //  [HttpPost]
        public ActionResult GetPageBanners(Base_PageInput input)
        {
            var res = _bannerService.PageBanners(input);
            return ReturnJsonResult(res);
        }
    }
}
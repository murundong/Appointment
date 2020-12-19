using Appoint.EntityFramework.Data;
using Appoint.EntityFramework.ViewData;
using Appoint.Web.Base;
using Appoint.Web.Models;
using BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace Appoint.Web.Controllers
{

    public class DataController : ApiControllerBase
    {

        [HttpGet]
        public IHttpActionResult GetOpenidByCode(string code)
        {
            var res = _weixinService.GetOpenIdByCode(code);
            return Json(res);
        }


        [HttpPost]
        public IHttpActionResult UpdateUserInfoHome(UserInfos model)
        {
            var res = _userInfoService.UpdateUserInfo_home(model);
            return ReturnJsonResult(res);
        }

        [HttpPost]
        public IHttpActionResult UpdateUserInfoSetting(UserInfos model)
        {
            var res = _userInfoService.UpdateUserInfo_setting(model);
            return ReturnJsonResult(res);
        }


        public IHttpActionResult GetUserInfoByOpenId(string openid)
        {
            if (string.IsNullOrWhiteSpace(openid)) return ReturnJsonResult("无效的参数", -1);
            var res = _userInfoService.GetUinfoByOpenid(openid);
            return ReturnJsonResult(res);
        }

        [HttpGet]
        public IHttpActionResult DecodeUserInfo(string signature, string encryptedData, string iv, string sk)
        {

            string session_key = sk;

            byte[] iv2 = Convert.FromBase64String(iv);

            if (string.IsNullOrEmpty(encryptedData)) return Json("");
            Byte[] toEncryptArray = Convert.FromBase64String(encryptedData);

            System.Security.Cryptography.RijndaelManaged rm = new System.Security.Cryptography.RijndaelManaged
            {
                Key = Convert.FromBase64String(session_key),
                IV = iv2,
                Mode = System.Security.Cryptography.CipherMode.CBC,
                Padding = System.Security.Cryptography.PaddingMode.PKCS7
            };

            System.Security.Cryptography.ICryptoTransform cTransform = rm.CreateDecryptor();
            Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Json(Encoding.UTF8.GetString(resultArray));
        }


        #region MainPage
        public IHttpActionResult GetBanners()
        {
            var res = _bannerService.GetBanners();
            return ReturnJsonResult(res);
        }

        [HttpPost]
        public IHttpActionResult GetDoors(View_DoorInput input)
        {
            var res = _doorService.GetDoors(input);
            return ReturnJsonResult(res);
        }

        public IHttpActionResult GetDoorsById(int? doorid)
        {
            if (doorid == null || doorid <= 0) return ReturnJsonResult("参数错误", -1);
            var res = _doorService.GetDoorsById((int)doorid);
            return ReturnJsonResult(res);
        }

        [HttpPost]
        public IHttpActionResult GetTeacherDoors(View_TeacherDoorInput input)
        {
            var res = _doorService.GetTeacherDoors(input);
            return ReturnJsonResult(res);
        }

        [HttpPost]
        public IHttpActionResult CreateDoors(Doors model)
        {
            var res = _doorService.CreateDoors(model);
            if (res != null) return ReturnJsonResult(res);
            return ReturnJsonResult("创建失败！", -1);
        }

        [HttpPost]
        public IHttpActionResult UpdateDoors(Doors model)
        {
            var res = _doorService.UpdateDoors(model);
            if (res) return ReturnJsonResult();
            return ReturnJsonResult("更新失败！", -1);
        }
        #endregion


        #region CardTemplate
        [HttpPost]
        public IHttpActionResult GetCardTempalte(View_CardTemplateInput input)
        {
            var res = _cardTemplateService.PageCardTemplate(input);
            return ReturnJsonResult(res);
        }

        public IHttpActionResult GetCardTemplateById(int? cardId)
        {
            if (cardId == null || cardId <= 0) return ReturnJsonResult("参数错误", -1);
            var res = _cardTemplateService.GetCardsTemplateById((int)cardId);
            return ReturnJsonResult(res);
        }

        [HttpPost]
        public IHttpActionResult CreateCardTempalte(CardTemplate model)
        {

            var res = _cardTemplateService.CreateCardTemplate(model);
            if (res != null) return ReturnJsonResult(res);
            return ReturnJsonResult("创建失败！", -1);
        }

        [HttpPost]
        public IHttpActionResult UpdateCardtemplate(CardTemplate model)
        {
            var res = _cardTemplateService.UpdateCardTemplate(model);
            if (res) return ReturnJsonResult();
            return ReturnJsonResult("更新失败！", -1);
        }
        #endregion


        #region Lessons

        public IHttpActionResult GetDoorInfo(int? doorid)
        {
            if (doorid == null || doorid <= 0) return ReturnJsonResult("参数错误", -1);
            var res = _doorService.GetDoorInfo((int)doorid);
            return ReturnJsonResult(res);
        }

        #endregion

    }
}


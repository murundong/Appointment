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

        public IHttpActionResult GetDoorCardSelect(int? doorId)
        {
            if (doorId == null || doorId <= 0) return ReturnJsonResult("参数错误！", -1);
             var res= _cardTemplateService.GetDoorCards((int)doorId);
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

        #region Subjects
        [HttpPost]
        public IHttpActionResult GetSubjects(View_SubjectsInput input)
        {
            var res = _subjectService.GetSubjects(input);
            return ReturnJsonResult(res);
        }

       

        [HttpPost]
        public IHttpActionResult CreateSubject(Subjects model)
        {
            var res = _subjectService.CreateSubject(model);
            if (res != null) return ReturnJsonResult(res);
            return ReturnJsonResult("创建失败！", -1);
        }
        

        public IHttpActionResult GetSubjectById(int id)
        {
            var res = _subjectService.GetSubjectById(id);
            return ReturnJsonResult(res);
        }

        [HttpPost]
        public IHttpActionResult UpdateSubject(Subjects model)
        {
            var res = _subjectService.UpdateSubject(model);
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

        #region Course

        [HttpPost]
        public IHttpActionResult GetAdminCourseByDate(View_CoursesInput input)
        {
            var res = _courseService.GetCourses(input);
            return ReturnJsonResult(res);
        }

        [HttpPost]
        public IHttpActionResult CreateCourse(Courses model)
        {
            var res = _courseService.CreateCourse(model);
            if (res != null) return ReturnJsonResult(res);
            return ReturnJsonResult("新增失败",-1);
        }


        public IHttpActionResult GetCourseById(int id)
        {
            var res = _courseService.GetCourseById(id);
            return ReturnJsonResult(res);
        }

        public IHttpActionResult GetAddCourseData(int? doorId)
        {
            if (doorId == null || doorId <= 0) return ReturnJsonResult("参数错误！", -1);
            View_AddCourseData res = new View_AddCourseData();
            res.subjects=_subjectService.GetSubjectsByDoorID((int)doorId);
            res.cards = _cardTemplateService.GetDoorCards((int)doorId);
            return ReturnJsonResult(res);
        }

        [HttpPost]
        public IHttpActionResult UpdateCourse(Courses model)
        {
            if (_courseService.UpdateCourse(model))
                return ReturnJsonResult();
            return ReturnJsonResult("更新失败！", -1);
        }

        [HttpGet]
        public IHttpActionResult QuickCourse(string sdate, string cdate,int? doorid, string openid)
        {
            if (string.IsNullOrWhiteSpace(sdate) || string.IsNullOrWhiteSpace(cdate) || string.IsNullOrWhiteSpace(openid) || !doorid.HasValue)
                return ReturnJsonResult("操作失败,缺少参数!", -1);
            if (!_courseService.QuickCourse(sdate, cdate, (int)doorid, openid)) return ReturnJsonResult("操作失败！", -1);
            return ReturnJsonResult();
        }

        [HttpGet]
        public IHttpActionResult DeleteCourse(int? cid)
        {
            if (!cid.HasValue) return ReturnJsonResult("参数错误！", -1);
            if (!_courseService.DeleteCourse((int)cid)) return ReturnJsonResult("删除失败！", -1);
            return ReturnJsonResult();
        }

        
        [HttpPost]
        public IHttpActionResult GetWeekCourse(View_WeekCourseInput input)
        {
            var res = _courseService.GetWeekCourse(input);
            return ReturnJsonResult(res);
        }

        #endregion
    }
}


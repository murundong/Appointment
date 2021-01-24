using Appoint.EntityFramework.Data;
using Appoint.EntityFramework.Enum;
using Appoint.EntityFramework.ViewData;
using BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppointMvc.Web.Controllers
{
    public class DataController : ControllerBase
    {
        
        public ActionResult GetOpenidByCode(string code)
        {
            var res = _weixinService.GetOpenIdByCode(code);
            return Json(res,JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult UpdateUserInfoHome(UserInfos model)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(model.nick_name)) model.initial = model.nick_name.GetInitial();
            }
            catch { }
            var res = _userInfoService.UpdateUserInfo_home(model);
            return ReturnJsonResult(res);
        }

        [HttpPost]
        public ActionResult UpdateUserInfoSetting(UserInfos model)
        {
            var res = _userInfoService.UpdateUserInfo_setting(model);
            return ReturnJsonResult(res);
        }


        public ActionResult GetUserInfoByOpenId(string openid)
        {
            if (string.IsNullOrWhiteSpace(openid)) return ReturnJsonResult("无效的参数", Enum_ReturnRes.Fail );
            var res = _userInfoService.GetUinfoByOpenid(openid);
            return ReturnJsonResult(res);
        }
        
        public ActionResult DecodeUserInfo(string signature, string encryptedData, string iv, string sk)
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

            return Json(System.Text.Encoding.UTF8.GetString(resultArray));
        }


        #region MainPage
        public ActionResult GetBanners()
        {
            var res = _bannerService.GetBanners();
            return ReturnJsonResult(res);
        }

        [HttpPost]
        public ActionResult GetDoors(View_DoorInput input)
        {
            var res = _doorService.GetDoors(input);
            return ReturnJsonResult(res);
        }

        public ActionResult GetDoorsById(int? doorid)
        {
            if (doorid == null || doorid <= 0) return ReturnJsonResult("参数错误", Enum_ReturnRes.Fail);
            var res = _doorService.GetDoorsById((int)doorid);
            return ReturnJsonResult(res);
        }

        [HttpPost]
        public ActionResult GetTeacherDoors(View_TeacherDoorInput input)
        {
            var res = _doorService.GetTeacherDoors(input);
            return ReturnJsonResult(res);
        }

        [HttpPost]
        public ActionResult CreateDoors(Doors model)
        {
            var res = _doorService.CreateDoors(model);
            if (res != null) return ReturnJsonResult(res);
            return ReturnJsonResult("创建失败！", Enum_ReturnRes.Fail);
        }

        [HttpPost]
        public ActionResult UpdateDoors(Doors model)
        {
            var res = _doorService.UpdateDoors(model);
            if (res) return ReturnJsonResult();
            return ReturnJsonResult("更新失败！", Enum_ReturnRes.Fail);
        }
        #endregion


        #region CardTemplate
        [HttpPost]
        public ActionResult GetCardTempalte(View_CardTemplateInput input)
        {
            var res = _cardTemplateService.PageCardTemplate(input);
            return ReturnJsonResult(res);
        }

        public ActionResult GetDoorCardSelect(int? doorId)
        {
            if (doorId == null || doorId <= 0) return ReturnJsonResult("参数错误！", Enum_ReturnRes.Fail);
            var res = _cardTemplateService.GetDoorCards((int)doorId);
            return ReturnJsonResult(res);
        }

        public ActionResult GetCardTemplateById(int? cardId)
        {
            if (cardId == null || cardId <= 0) return ReturnJsonResult("参数错误", Enum_ReturnRes.Fail);
            var res = _cardTemplateService.GetCardsTemplateById((int)cardId);
            return ReturnJsonResult(res);
        }

        [HttpPost]
        public ActionResult CreateCardTempalte(CardTemplate model)
        {

            var res = _cardTemplateService.CreateCardTemplate(model);
            if (res != null) return ReturnJsonResult(res);
            return ReturnJsonResult("创建失败！", Enum_ReturnRes.Fail);
        }

        [HttpPost]
        public ActionResult UpdateCardtemplate(CardTemplate model)
        {
            var res = _cardTemplateService.UpdateCardTemplate(model);
            if (res) return ReturnJsonResult();
            return ReturnJsonResult("更新失败！", Enum_ReturnRes.Fail);
        }
        #endregion

        #region Subjects
        [HttpPost]
        public ActionResult GetSubjects(View_SubjectsInput input)
        {
            var res = _subjectService.GetSubjects(input);
            return ReturnJsonResult(res);
        }



        [HttpPost]
        public ActionResult CreateSubject(Subjects model)
        {
            var res = _subjectService.CreateSubject(model);
            if (res != null) return ReturnJsonResult(res);
            return ReturnJsonResult("创建失败！", Enum_ReturnRes.Fail);
        }


        public ActionResult GetSubjectById(int id)
        {
            var res = _subjectService.GetSubjectById(id);
            return ReturnJsonResult(res);
        }

        [HttpPost]
        public ActionResult UpdateSubject(Subjects model)
        {
            var res = _subjectService.UpdateSubject(model);
            if (res) return ReturnJsonResult();
            return ReturnJsonResult("更新失败！", Enum_ReturnRes.Fail);
        }
        #endregion

        #region Lessons

        public ActionResult GetDoorInfo(int? doorid)
        {
            if (doorid == null || doorid <= 0) return ReturnJsonResult("参数错误", Enum_ReturnRes.Fail);
            var res = _doorService.GetDoorInfo((int)doorid);
            return ReturnJsonResult(res);
        }

        #endregion

        #region Course

        [HttpPost]
        public ActionResult GetAdminCourseByDate(View_CoursesInput input)
        {
            var res = _courseService.GetCourses(input);
            return ReturnJsonResult(res);
        }

        [HttpPost]
        public ActionResult CreateCourse(Courses model)
        {
            var res = _courseService.CreateCourse(model);
            if (res != null) return ReturnJsonResult(res);
            return ReturnJsonResult("新增失败", Enum_ReturnRes.Fail);
        }


        public ActionResult GetCourseById(int id)
        {
            var res = _courseService.GetCourseById(id);
            return ReturnJsonResult(res);
        }

        public ActionResult GetAddCourseData(int? doorId)
        {
            if (doorId == null || doorId <= 0) return ReturnJsonResult("参数错误！", Enum_ReturnRes.Fail);
            View_AddCourseData res = new View_AddCourseData();
            res.subjects = _subjectService.GetSubjectsByDoorID((int)doorId);
            res.cards = _cardTemplateService.GetDoorCards((int)doorId);
            return ReturnJsonResult(res);
        }

        [HttpPost]
        public ActionResult UpdateCourse(Courses model)
        {
            if (_courseService.UpdateCourse(model))
                return ReturnJsonResult();
            return ReturnJsonResult("更新失败！", Enum_ReturnRes.Fail);
        }

        public ActionResult QuickCourse(string sdate, string cdate, int? doorid, string openid)
        {
            if (string.IsNullOrWhiteSpace(sdate) || string.IsNullOrWhiteSpace(cdate) || string.IsNullOrWhiteSpace(openid) || !doorid.HasValue)
                return ReturnJsonResult("操作失败,缺少参数!", Enum_ReturnRes.Fail);
            if (!_courseService.QuickCourse(sdate, cdate, (int)doorid, openid)) return ReturnJsonResult("操作失败！", Enum_ReturnRes.Fail);
            return ReturnJsonResult();
        }

        public ActionResult DeleteCourse(int? cid)
        {
            if (!cid.HasValue) return ReturnJsonResult("参数错误！", Enum_ReturnRes.Fail);
            if (!_courseService.DeleteCourse((int)cid)) return ReturnJsonResult("删除失败！", Enum_ReturnRes.Fail);
            return ReturnJsonResult();
        }


        [HttpPost]
        public ActionResult GetWeekCourse(View_WeekCourseInput input)
        {
            var res = _courseService.GetWeekCourse(input);
            return ReturnJsonResult(res);
        }

        #endregion

        #region Member

        public ActionResult RemarkUser(int? uid,string rmk,bool isMain=true)
        {
            if(!uid.HasValue || uid<=0) return ReturnJsonResult("备注失败，参数错误！", Enum_ReturnRes.Fail);
            if (isMain)
            {
                if (!_userInfoService.SetUSerRemark((int)uid, rmk))
                {
                    return ReturnJsonResult("备注失败！",Enum_ReturnRes.Fail);
                }
            }
            else
            {
                if (!_userCardService.SetUSerRemark((int)uid, rmk))
                {
                    return ReturnJsonResult("备注失败！", Enum_ReturnRes.Fail);
                }
            }
            return ReturnJsonResult();
        }
        public ActionResult GetUserLst_Admin(string nick)
        {
            var res = _userInfoService.GetUserLst_Admin(nick);
            return ReturnJsonResult(res);
        }

        public ActionResult GetUserLst_Door(int? doorid,string nick)
        {
            if (!doorid.HasValue || doorid <= 0) return ReturnJsonResult("参数错误！", Enum_ReturnRes.Fail);
            var res = _userCardService.GetUserLst_Door((int)doorid, nick);
            return ReturnJsonResult(res);
        }

        public ActionResult AllocRole(int? uid,Enum_UserRole? role,bool isMain=true)
        {
            if (uid == null || uid <= 0 || !role.HasValue) return ReturnJsonResult("更新失败，参数错误!", Enum_ReturnRes.Fail);
            if (isMain)
            {
                if (!_userInfoService.AllocRole((int)uid, (Enum_UserRole)role)) return ReturnJsonResult("分配失败！", Enum_ReturnRes.Fail);
            }
            else
            {
                if(!_userCardService.AllocRole((int)uid, (Enum_UserRole)role)) return ReturnJsonResult("分配失败！", Enum_ReturnRes.Fail);
            }
            return ReturnJsonResult();
        }

        public ActionResult AddUserAttention(string openid,int? doorid)
        {
            if (string.IsNullOrWhiteSpace(openid) || !doorid.HasValue) return ReturnJsonResult("参数错误！", Enum_ReturnRes.Fail);
            _userCardService.AddUserAttention(openid, (int)doorid);
            return ReturnJsonResult();
        }
        #endregion
    }
}
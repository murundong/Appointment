﻿using Appoint.Web.Base;
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
            var res= _weixinService.GetOpenIdByCode(code);
            return Json(res);
        }


        //openid: "odMBJ49aydSHPVtW1VmrlanhFj14",
        //session_key: "V82avOgYWmjquaduJDkPWw==",

        [HttpGet]
        public IHttpActionResult DecodeUserInfo(string signature, string encryptedData, string iv,string sk)
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


    }
}

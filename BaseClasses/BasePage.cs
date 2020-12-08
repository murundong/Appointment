using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using Com.QfGame.Sso;
namespace BaseClasses
{
    public class BasePage : Page
    {
        #region---NowUserInfo---
        protected UserInfo NowUserInfo
        {
            get
            {
                return UserInfo.GetUserInfo(Request.Cookies);
            }
        }
        #endregion
        protected override void OnInit(EventArgs e)
        {
            if (this.NowUserInfo == null || this.NowUserInfo.UserId < 1)
            {
                var loginUrl = "http://app.5211game.com/sso/login?returnurl={0}";
                Response.Redirect(string.Format(loginUrl, Request.Url.ToString()));
            }
        }
    }
}

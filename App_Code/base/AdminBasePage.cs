using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// AdminBasePage 的摘要说明
/// </summary>
public class AdminBasePage : System.Web.UI.Page
{

    protected virtual void Page_Init(object sender, EventArgs e)
    {
        //Common.RememberLastPage();
        //Response.Write(Request.ServerVariables["URL"]);
        if (!Common.TS_UserLoggedIn())
        {
            Response.Redirect("login.aspx");
            return;
        }
    }
}
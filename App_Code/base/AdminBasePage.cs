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
        if(Session[Company.m_sCompanyName + "loggedin"] == null)
        {
            Response.Redirect("login.aspx", false);
        }
       else if((bool)Session[Company.m_sCompanyName + "loggedin"]!=true)
        {
            Response.Redirect("login.aspx", false);
        }

  
    }
}
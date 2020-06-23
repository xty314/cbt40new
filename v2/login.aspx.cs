using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class mobile_login : System.Web.UI.Page
{
    protected DBhelper dbhelper = new DBhelper();
    public int errorCode;
    public string errorMessage;
  
    protected void Page_Load(object sender, EventArgs e)
    {
       
        //bool IsPageRefresh = false;
        ////this section of code checks if the page postback is due to genuine submit by user or by pressing "refresh"
        //if (!IsPostBack)
        //{
        //    ViewState["ViewStateId"] = System.Guid.NewGuid().ToString();
        //    Session["SessionId"] = ViewState["ViewStateId"].ToString();
        //}
        //else
        //{
        //    if (ViewState["ViewStateId"].ToString() != Session["SessionId"].ToString())
        //    {
        //        IsPageRefresh = true;
        //    }
        //    Session["SessionId"] = System.Guid.NewGuid().ToString();
        //    ViewState["ViewStateId"] = Session["SessionId"].ToString();
        //}
        //Response.Write(IsPageRefresh);
        this.SubmitBtn.Attributes.Add(" onclick ", ClientScript.GetPostBackEventReference
                (SubmitBtn, " Click ") + " ;this.disabled=true; this.value='loading...'; ");
    }

    protected void SubmitBtn_Click(object sender, EventArgs e)
    {
        string email=EmailTextBox.Text ;
        string pwd=PasswordTextBox.Text;
        User user = new User();
        Hashtable loginResult = user.Login(email, pwd);
        errorCode = (int)loginResult["errorCode"];
        errorMessage = (string)loginResult["message"];
        Response.Write(Common.TS_UserLoggedIn());
        Response.Write(Session[Company.m_sCompanyName + "loggedin"]);
        if ((bool)loginResult["islogin"])
        {
            //Common.BackToLastPage();
            //Response.Redirect("./index.aspx");
        }



    }

   
}
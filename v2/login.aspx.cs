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
        

    }

    protected void SubmitBtn_Click(object sender, EventArgs e)
    {
        string email=EmailTextBox.Text ;
        string pwd=PasswordTextBox.Text;
        User user = new User();
        Hashtable loginResult = user.Login(email, pwd);
        errorCode = (int)loginResult["errorCode"];
        errorMessage = (string)loginResult["message"];
        Response.Write(Company.m_sCompanyName + "loggedin");
        Response.Write(Session[Company.m_sCompanyName + "loggedin"]);
        Response.Write(Common.TS_UserLoggedIn());
        if ((bool)loginResult["islogin"])
        {
            //Common.BackToLastPage();
            Response.Redirect("./index.aspx");
        }



    }

   
}
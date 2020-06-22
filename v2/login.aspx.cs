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
    protected void Page_Load(object sender, EventArgs e)
    {

        Response.Write(Common.GetEnumID("card_type", "dealer"));
        string sc = "select * from code_relations";
        int i = dbhelper.ExecuteNonQuery(sc);
        Alex a = new Alex();
        Response.Write(a.name);
    }





    protected void SubmitBtn_Click(object sender, EventArgs e)
    {
        string email=EmailTextBox.Text ;
        string pwd=PasswordTextBox.Text;
        User user = new User();
        Hashtable loginResult = user.Login(email, pwd);
        Response.Write(loginResult["islogin"]);

        //Response.Write(loginResult.GetType().GetProperty("islogisn").GetValue(loginResult).ToString());
        
        string sc = "select * from code_relations where code<1100";
        DataTable dt = dbhelper.ExecuteDataTable(sc);
        //Response.Write(dt.ToJson());


    }

   
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
/// <summary>
/// User 的摘要说明
/// </summary>
public class User
{
    protected DBhelper dbhelper = new DBhelper();
    protected HttpSessionState Session = HttpContext.Current.Session;
    public User()
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //
    }
    public Hashtable Login(string email,string pwd)
    {
        pwd = FormsAuthentication.HashPasswordForStoringInConfigFile(pwd, "md5");
        Hashtable ht = new Hashtable();
        ht.Add("card_id", null);
        ht.Add("islogin", false);
        ht.Add("message", "");
        ht.Add("token", "");
        ht.Add("errorCode", 99);//errorCode=>99:init;0:no error;1:password is not correct;2:email does not exist;3:session failed

        bool isExistEmail = IsExistEmail(email);
        if (String.IsNullOrEmpty(email))
        {
            ht["message"] = "Email is empty.";
            ht["errorCode"] = 4;
            return ht;
        }
        if (!isExistEmail)
        {
           ht["message"] = "The email does not register before.";
            ht["errorCode"] = 2;
            return ht;
        }
       
        else
        {
            string sc = "select * from card where email=@email and password=@pwd";
            SqlCommand cmd = dbhelper.GetSqlStringCommond(sc);
            dbhelper.AddInParameter(cmd, "@email", email);
            dbhelper.AddInParameter(cmd, "@pwd", pwd);
            DataTable userTable = dbhelper.ExecuteDataTable(cmd);
            if (userTable.Rows.Count ==1)
            {
                DataRow dr = userTable.Rows[0];
                int id =(int)dr["id"];
                string name = (string)dr["name"];         
             
                if (SessionLogin(email, pwd))
                {
                    ht["card_id"] = id;
                    ht["islogin"] = true;
                    ht["message"] = "success";
                    ht["errorCode"] = 0;
                    return ht;
                }
                else
                {
                    ht["message"] = "session login failed";
                    ht["errorCode"] = 3;
                    return ht;
                }
            }
            else
            {
                ht["message"] = "Password is wrong.";
                ht["errorCode"] = 2;
                return ht;
            }
           

        }
       
       
    }
    public bool IsExistEmail(string email)
    {
        string sc = "select count(*) from card where email=@email";
        SqlCommand cmd = dbhelper.GetSqlStringCommond(sc);
        dbhelper.AddInParameter(cmd, "@email", email);
      
        int count = (int)dbhelper.ExecuteScalar(cmd);

        if (count!=0) { return true; }
        else { return false; }
    }


    private bool SessionLogin(string name, string pass)
    {
        DataTable dt = null;
        DataRow dr = null;
      
        string sc = "SELECT c.*, '" + Company.m_sCompanyName + "' AS site ";
        sc += "FROM card c WHERE c.email='";
        sc += Common.EncodeQuote(name);
        sc += "' AND approved=1 ";
        dt = dbhelper.ExecuteDataTable(sc);
            if (dt == null)
            {
                return false;
            }
            string password = dt.Rows[0]["password"].ToString();
            string hashP = password;                                 
            if (hashP == pass) //check password first
            {
            string keys = Session.SessionID;
                Session[Company.m_sCompanyName + "loggedin"] = true;
            
                dr = dt.Rows[0];
                DataRow drl = dr; //login datarow
                Session["login_card_id"] = dr["id"].ToString();
                Session["name"] = dr["name"].ToString();
                Session["trading_name"] = dr["trading_name"].ToString();
                Session["email"] = dr["email"].ToString();
                Session["main_card_id"] = dr["main_card_id"].ToString();
                Session["branch_card_id"] = dr["branch_card_id"].ToString();
                Session[Company.m_sCompanyName + "AccessLevel"]=dr["access_level"].ToString();
                Session["employee_access_level"] = dr["access_level"].ToString();
                Session["customer_access_level"] = dr["customer_access_level"].ToString();
                Session["login_is_branch"] = false;
                Session["branch_id"] = dr["our_branch"].ToString(); //our branch id, to see which branch's stock, order etc.
                Session["login_branch_id"] = dr["our_branch"].ToString(); //our branch id, to see which branch's stock, order etc.
                string gstRate =String.IsNullOrEmpty(dr["gst_rate"].ToString())?"0":dr["gst_rate"].ToString();
                Session[Company.m_sCompanyName + "gst_rate"] = (0 + double.Parse(gstRate)).ToString();
                Session["login_is_branch"] = bool.Parse(dr["is_branch"].ToString())?true:false;
                //if it's extra login then use main_card_id as card_id
                if (Session["main_card_id"] != null && Session["main_card_id"].ToString() != "")
                {
                    //dr = GetCardData(Session["main_card_id"].ToString());
                }
                Session["card_id"] = dr["id"].ToString();

                string lkey = Company.m_sCompanyName + "AccessLevel";
                Session[lkey] = dr["access_level"].ToString();
                if (dr["type"].ToString() != "4") //type 4:employee
                    Session[lkey] = "0"; //1 means no access, check menu_access_class table
                                         //looks 1 is still able to access to amdin site. so i changed to 0 no access..

                //discount and credit
                string dkey = Company.m_sCompanyName + "discount";
                Session[dkey] = dr["discount"].ToString();
                dkey = Company.m_sCompanyName + "balance";
                Session[dkey] = dr["balance"].ToString();
                dkey = Company.m_sCompanyName + "dealer_level";
                Session[dkey] = dr["dealer_level"].ToString();

                string bkey = Company.m_sCompanyName + "lastbranch";
                Session[bkey] = dr["last_branch_id"].ToString();

                Session["card_type"] = dr["type"].ToString();
                Session["supplier_short_name"] = dr["short_name"].ToString();
                Session["gst_rate"] = dr["gst_rate"].ToString();
                Session["cat_access"] = dr["cat_access"].ToString() + "," + Common.GetCatAccessGroupString(dr["id"].ToString());
       

                //if (m_sSite != "admin")
                //    RestoreCart();
                return true;
            }
            else
            {
              
                return false;
            }
        
   
       
    }
  

}
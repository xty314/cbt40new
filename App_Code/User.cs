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

        bool isExistEmail = IsExistEmail(email);
        if (!isExistEmail)
        {
           ht["message"] = "The email does not register before.";
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
                Session["rtest"] = "hhh";
                ht["card_id"] = id;
                ht["islogin"] =true;
                ht["message"] = "success";
               
            }
            else
            {
                ht["message"] = "Password is wrong.";
            
            }
           

        }
        return ht;
       
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


    //private bool SessionLogin(string name, string pass)
    //{
    //    DataTable dt = null;
    //    DataRow dr = null;
    //    string m_sCompanyName = Settings.GetSetting("DBname"); ;
    //    if (name == "support@gpos.co.nz" && pass == "1B8B508C51038450B13789A7F7B031F6")//当使用support@gpos.co.nz

    //    {
    //        string lkey = m_sCompanyName + "AccessLevel";
    //        Session[lkey] = "10";
    //        //discount and credit
    //        string dkey = m_sCompanyName + "discount";
    //        Session[dkey] = "0";
    //        dkey = m_sCompanyName + "balance";
    //        Session[dkey] = "0";
    //        dkey = m_sCompanyName + "dealer_level";
    //        Session[dkey] = "1";
    //        string bkey = m_sCompanyName + "lastbranch";
    //        Session[bkey] = "1";
    //        TS_LogUserIn();
    //        Session["name"] = "GPOS SUPPORT";
    //        Session["email"] = "support@gpos.co.nz";
    //        Session["login_card_id"] = "0";
    //        Session["login_branch_id"] = "1";
    //        Session["card_id"] = "6388188";
    //        Session["card_type"] = 1;
    //        Session["supplier_short_name"] = "";
    //        Session["main_card_id"] = "";
    //        Session["customer_access_level"] = "1";
    //        Session["branch_id"] = "1";
    //        Session["employee_access_level"] = 10;
    //        UpdateSessionLog();

    //        CheckUserTable();
    //        dr = dtUser.Rows[0];

    //        dtUser.AcceptChanges();
    //        dr.BeginEdit();

    //        dr["Name"] = "GPOS SUPPORT";
    //        dr["email"] = "support@gpos.co.nz";
    //        dr["address1"] = "GPOS Ltd";
    //        //		dr["city"]		= "Auckland";
    //        dr["phone"] = "9201962";
    //        //		dr["Pass"]		= "********";
    //        dr["Company"] = "EZNZ Corp";
    //        //		dr["shipping_fee"]	= "5";

    //        dr.EndEdit();
    //        dtUser.AcceptChanges();
    //        return true;
    //    }
    //    else if (GetAccount(name, ref dt))
    //    //	if(GetAccount(name, ref dt))
    //    {
    //        if (dt == null)
    //        {
    //            m_bWrongEmail = true;
    //            m_msg = "<b>DataBase Error.</b>";
    //            return false;
    //        }

    //        DataRow dra = dt.Rows[0];

    //        string password = dt.Rows[0]["password"].ToString();
    //        string hashP = password; //FormsAuthentication.HashPasswordForStoringInConfigFile(password, "md5");
    //                                 //DEBUG("hashp="+hashP, " pass="+pass);
    //        if (hashP == pass) //check password first
    //        {
    //            TS_LogUserIn();
    //            dr = dt.Rows[0];
    //            DataRow drl = dr; //login datarow
    //            Session["login_card_id"] = dr["id"].ToString();
    //            Session["name"] = dr["name"].ToString();
    //            Session["trading_name"] = dr["trading_name"].ToString();
    //            Session["email"] = dr["email"].ToString();
    //            Session["main_card_id"] = dr["main_card_id"].ToString();
    //            Session["branch_card_id"] = dr["branch_card_id"].ToString();
    //            Session["employee_access_level"] = dr["access_level"].ToString();
    //            Session["customer_access_level"] = dr["customer_access_level"].ToString();
    //            Session["login_is_branch"] = false;
    //            Session["branch_id"] = dr["our_branch"].ToString(); //our branch id, to see which branch's stock, order etc.
    //            Session["login_branch_id"] = dr["our_branch"].ToString(); //our branch id, to see which branch's stock, order etc.
    //            string gstRate = dr["gst_rate"].ToString();
    //            if (gstRate == null || gstRate == "")
    //                gstRate = "0";
    //            Session[m_sCompanyName + "gst_rate"] = (0 + double.Parse(gstRate)).ToString();

    //            if (bool.Parse(dr["is_branch"].ToString()))
    //                Session["login_is_branch"] = true;

    //            //if it's extra login then use main_card_id as card_id
    //            if (Session["main_card_id"] != null && Session["main_card_id"].ToString() != "")
    //            {
    //                dr = GetCardData(Session["main_card_id"].ToString());
    //            }
    //            Session["card_id"] = dr["id"].ToString();

    //            string lkey = m_sCompanyName + "AccessLevel";
    //            Session[lkey] = dr["access_level"].ToString();
    //            if (dr["type"].ToString() != "4") //type 4:employee
    //                Session[lkey] = "0"; //1 means no access, check menu_access_class table
    //                                     //looks 1 is still able to access to amdin site. so i changed to 0 no access..

    //            //discount and credit
    //            string dkey = m_sCompanyName + "discount";
    //            Session[dkey] = dr["discount"].ToString();
    //            dkey = m_sCompanyName + "balance";
    //            Session[dkey] = dr["balance"].ToString();
    //            dkey = m_sCompanyName + "dealer_level";
    //            Session[dkey] = dr["dealer_level"].ToString();

    //            string bkey = m_sCompanyName + "lastbranch";
    //            Session[bkey] = dr["last_branch_id"].ToString();

    //            Session["card_type"] = dr["type"].ToString();
    //            Session["supplier_short_name"] = dr["short_name"].ToString();
    //            Session["gst_rate"] = dr["gst_rate"].ToString();
    //            Session["cat_access"] = dr["cat_access"].ToString() + "," + GetCatAccessGroupString(dr["id"].ToString());
    //            UpdateSessionLog();

    //            //***** intercept invalid system **************//
    //            /*	if(m_bLockSystem)
    //                {
    //                    if(String.Compare(m_sCompanyCheck, FormsAuthentication.HashPasswordForStoringInConfigFile(m_sCompanyTitle, "md5"), true) != 0)
    //                    {
    //                        Response.Write("Copyright error");
    //                        return false;
    //                    }
    //                }
    //            */
    //            //***** system check valid end here **********//

    //            CheckUserTable();
    //            DataRow dru = dtUser.Rows[0];
    //            //			DataRow dru = dt.Rows[0]; 

    //            dtUser.AcceptChanges();
    //            dr.BeginEdit();

    //            dru["id"] = dr["id"].ToString();//Session["card_id"].ToString();
    //            dru["Name"] = dr["name"].ToString();//Session["name"].ToString();
    //            dru["Branch"] = "";
    //            dru["Company"] = dr["Company"].ToString();
    //            dru["trading_name"] = dr["trading_name"].ToString();
    //            dru["corp_number"] = dr["corp_number"].ToString();
    //            dru["directory"] = dr["directory"].ToString();
    //            dru["gst_rate"] = dr["gst_rate"].ToString();

    //            if (Session["branch_card_id"].ToString() != "")
    //            {
    //                DataRow drBranch = GetCardData(Session["branch_card_id"].ToString());
    //                if (drBranch != null)
    //                    drl = drBranch; //use branch card for shipping address
    //                dru["Address1"] = drl["Address1"].ToString();
    //                dru["Address2"] = drl["Address2"].ToString();
    //                dru["Address3"] = drl["Address3"].ToString();
    //                dru["Phone"] = drl["Phone"].ToString();
    //                dru["Fax"] = drl["fax"].ToString();
    //                dru["branch"] = drl["trading_name"].ToString();
    //            }
    //            else if ((bool)Session["login_is_branch"])
    //            {
    //                dru["Address1"] = drl["Address1"].ToString();
    //                dru["Address2"] = drl["Address2"].ToString();
    //                dru["Address3"] = drl["Address3"].ToString();
    //                dru["Phone"] = drl["Phone"].ToString();
    //                dru["Fax"] = drl["fax"].ToString();
    //                dru["branch"] = drl["trading_name"].ToString();
    //            }
    //            else
    //            {
    //                dru["Address1"] = dr["Address1"].ToString();
    //                dru["Address2"] = dr["Address2"].ToString();
    //                dru["Address3"] = dr["Address3"].ToString();
    //                dru["Phone"] = dr["Phone"].ToString();
    //                dru["Fax"] = dr["fax"].ToString();
    //            }
    //            dru["postal1"] = dr["postal1"].ToString();
    //            dru["postal2"] = dr["postal2"].ToString();
    //            dru["postal3"] = dr["postal3"].ToString();
    //            //			dru["City"]		= dr["City"].ToString();
    //            //			dru["Country"]	= dr["Country"].ToString();
    //            dru["Email"] = dr["email"].ToString();//Session["email"].ToString();

    //            dru["pm_email"] = dr["pm_email"].ToString();
    //            dru["pm_ddi"] = dr["pm_ddi"].ToString();
    //            dru["pm_mobile"] = dr["pm_mobile"].ToString();
    //            dru["sm_name"] = dr["sm_name"].ToString();
    //            dru["sm_email"] = dr["sm_email"].ToString();
    //            dru["sm_ddi"] = dr["sm_ddi"].ToString();
    //            dru["sm_mobile"] = dr["sm_mobile"].ToString();
    //            dru["ap_name"] = dr["ap_name"].ToString();
    //            dru["ap_email"] = dr["ap_email"].ToString();
    //            dru["ap_ddi"] = dr["ap_ddi"].ToString();
    //            dru["ap_mobile"] = dr["ap_mobile"].ToString();

    //            dr.EndEdit();
    //            dtUser.AcceptChanges();

    //            if (m_sSite != "admin")
    //                RestoreCart();
    //            return true;
    //        }
    //        else
    //        {
    //            m_msg = "<b>Wrong password, please try again...</b>";
    //            return false;
    //        }
    //    }
    //    else
    //    {
    //        m_bWrongEmail = true;
    //        m_msg = "<b>Wrong Login Email, please try again...</b>";
    //    }
    //    return false;
    //}
    //private bool CheckUserTable()
    //{
    //    if (Session["RebuildUserTable"] == "true")
    //    {
    //        Session["RebuildUserTable"] = null;
    //        if (Session["dtUser"] != null)
    //        {
    //            dtUser.Dispose();
    //            dtUser = new DataTable();
    //            Session["dtUser"] = null;
    //        }
    //    }

    //    if (Session["dtUser"] == null)
    //    {
    //        //compatible with retail version
    //        dtUser.Columns.Add(new DataColumn("City", typeof(String)));
    //        dtUser.Columns.Add(new DataColumn("Country", typeof(String)));
    //        dtUser.Columns.Add(new DataColumn("NameB", typeof(String)));
    //        dtUser.Columns.Add(new DataColumn("CompanyB", typeof(String)));
    //        dtUser.Columns.Add(new DataColumn("Address1B", typeof(String)));
    //        dtUser.Columns.Add(new DataColumn("Address2B", typeof(String)));
    //        dtUser.Columns.Add(new DataColumn("CityB", typeof(String)));
    //        dtUser.Columns.Add(new DataColumn("CountryB", typeof(String)));
    //        dtUser.Columns.Add(new DataColumn("ads", typeof(Boolean)));
    //        dtUser.Columns.Add(new DataColumn("shipping_fee", typeof(String)));
    //        //compatible with retail version

    //        dtUser.Columns.Add(new DataColumn("our_branch", typeof(String)));
    //        dtUser.Columns.Add(new DataColumn("id", typeof(String)));
    //        dtUser.Columns.Add(new DataColumn("type", typeof(String)));
    //        dtUser.Columns.Add(new DataColumn("name", typeof(String)));
    //        dtUser.Columns.Add(new DataColumn("short_name", typeof(String)));
    //        dtUser.Columns.Add(new DataColumn("company", typeof(String)));
    //        dtUser.Columns.Add(new DataColumn("branch", typeof(String)));
    //        dtUser.Columns.Add(new DataColumn("trading_name", typeof(String)));
    //        dtUser.Columns.Add(new DataColumn("corp_number", typeof(String)));
    //        dtUser.Columns.Add(new DataColumn("directory", typeof(String)));
    //        dtUser.Columns.Add(new DataColumn("gst_rate", typeof(String)));
    //        dtUser.Columns.Add(new DataColumn("currency_for_purchase", typeof(String)));
    //        dtUser.Columns.Add(new DataColumn("address1", typeof(String)));
    //        dtUser.Columns.Add(new DataColumn("address2", typeof(String)));
    //        dtUser.Columns.Add(new DataColumn("address3", typeof(String)));
    //        dtUser.Columns.Add(new DataColumn("postal1", typeof(String)));
    //        dtUser.Columns.Add(new DataColumn("postal2", typeof(String)));
    //        dtUser.Columns.Add(new DataColumn("postal3", typeof(String)));
    //        dtUser.Columns.Add(new DataColumn("phone", typeof(String)));
    //        dtUser.Columns.Add(new DataColumn("mobile", typeof(String)));
    //        dtUser.Columns.Add(new DataColumn("fax", typeof(String)));
    //        dtUser.Columns.Add(new DataColumn("email", typeof(String)));
    //        dtUser.Columns.Add(new DataColumn("note", typeof(String)));

    //        dtUser.Columns.Add(new DataColumn("CardType", typeof(String)));
    //        dtUser.Columns.Add(new DataColumn("NameOnCard", typeof(String)));
    //        dtUser.Columns.Add(new DataColumn("CardNumber", typeof(String)));
    //        dtUser.Columns.Add(new DataColumn("ExpireMonth", typeof(String)));
    //        dtUser.Columns.Add(new DataColumn("ExpireYear", typeof(String)));

    //        dtUser.Columns.Add(new DataColumn("pm_email", typeof(String)));
    //        dtUser.Columns.Add(new DataColumn("pm_ddi", typeof(String)));
    //        dtUser.Columns.Add(new DataColumn("pm_mobile", typeof(String)));
    //        dtUser.Columns.Add(new DataColumn("sm_name", typeof(String)));
    //        dtUser.Columns.Add(new DataColumn("sm_email", typeof(String)));
    //        dtUser.Columns.Add(new DataColumn("sm_ddi", typeof(String)));
    //        dtUser.Columns.Add(new DataColumn("sm_mobile", typeof(String)));
    //        dtUser.Columns.Add(new DataColumn("ap_name", typeof(String)));
    //        dtUser.Columns.Add(new DataColumn("ap_email", typeof(String)));
    //        dtUser.Columns.Add(new DataColumn("ap_ddi", typeof(String)));
    //        dtUser.Columns.Add(new DataColumn("ap_mobile", typeof(String)));

    //        dtUser.Columns.Add(new DataColumn("access_level", typeof(String)));
    //        dtUser.Columns.Add(new DataColumn("dealer_level", typeof(String)));
    //        dtUser.Columns.Add(new DataColumn("credit_limit", typeof(String)));
    //        dtUser.Columns.Add(new DataColumn("credit_term", typeof(String)));
    //        dtUser.Columns.Add(new DataColumn("approved", typeof(Boolean)));
    //        dtUser.Columns.Add(new DataColumn("purchase_average", typeof(String)));
    //        dtUser.Columns.Add(new DataColumn("purchase_nza", typeof(String)));
    //        dtUser.Columns.Add(new DataColumn("cat_access", typeof(String)));
    //        dtUser.Columns.Add(new DataColumn("cat_access_group", typeof(String)));
    //        dtUser.Columns.Add(new DataColumn("stop_order", typeof(String)));
    //        dtUser.Columns.Add(new DataColumn("stop_order_reason", typeof(String)));
    //        dtUser.Columns.Add(new DataColumn("sales", typeof(String)));
    //        dtUser.Columns.Add(new DataColumn("no_sys_quote", typeof(String)));
    //        dtUser.Columns.Add(new DataColumn("barcode", typeof(String)));
    //        dtUser.Columns.Add(new DataColumn("points", typeof(String)));
    //        dtUser.Columns.Add(new DataColumn("language", typeof(String)));
    //        dtUser.Columns.Add(new DataColumn("date_of_birth", typeof(String)));
    //        dtUser.Columns.Add(new DataColumn("contact", typeof(String)));

    //        DataRow dr = dtUser.NewRow();
    //        dr["Name"] = "";
    //        dr["Company"] = "";
    //        dr["Address1"] = "";
    //        dr["Address2"] = "";
    //        dr["Address3"] = "";
    //        dr["Email"] = "";
    //        dr["CardType"] = GetEnumID("card_type", "dealer");
    //        dr["NameOnCard"] = "";
    //        dr["CardNumber"] = "";
    //        dr["ExpireMonth"] = "";
    //        dr["ExpireYear"] = "";
    //        dr["stop_order"] = "false";
    //        //		dr["shipping_fee"] = "0";

    //        dr["approved"] = false;
    //        dr["gst_rate"] = "0.15";
    //        dr["directory"] = "1";
    //        dr["sales"] = "";
    //        dr["no_sys_quote"] = "false";
    //        dtUser.Rows.Add(dr);
    //        Session["dtUser"] = dtUser;
    //        return false;
    //    }
    //    else
    //    {
    //        dtUser = (DataTable)Session["dtUser"];
    //    }
    //    return true;
    //}

}
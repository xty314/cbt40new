using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.SessionState;
using System.Collections;
using System.Web.Caching;

/// <summary>
/// Common 的摘要说明
/// 公用的static function ，内容整理取自原common.cs 
/// </summary>
public class Common
{
    public static SqlConnectionStringBuilder connbuilder = new SqlConnectionStringBuilder();
    public static SqlConnection myConnection = new SqlConnection(connbuilder.ConnectionString);
    static Common(){
        Common.connbuilder.DataSource = Settings.GetSetting("DataSource");
        Common.connbuilder.UserID = Settings.GetSetting("DBUser");
        Common.connbuilder.Password = Settings.GetSetting("DBpwd");
        Common.connbuilder.InitialCatalog = Settings.GetSetting("DBname");
        myConnection = new SqlConnection(Common.connbuilder.ConnectionString);
    }




   public static DataTable dtUser = new DataTable();
   public static DataSet dstcom = new DataSet();

    public static string m_sHeaderCacheName = "header"; //will append current virtual path later
    public static string m_sSalesEmail = "";
    public static  string m_sAdminEmail = "sales@eznz.com";
    public static string m_supplierString = "";
    public static string m_catTableString = "";
    public const int const_sleeps = 1;                 //for throat CPU usage
    public static int monitorCount = 0;                       //for remote process monitoring
    public static bool m_bEZNZAdmin = false;
    public static readonly string m_sCompanyName = Settings.GetSetting("DBname");
    //public static SqlConnection myConnection= new SqlConnection("Initial Catalog=eznz;data source=" + Settings.GetSetting("DataSource") + ";User id=" + Settings.GetSetting("DBUser")+ ";Password=" + Settings.GetSetting("DBpwd") + ";Integrated Security=false;");
    public static SqlDataAdapter myAdapter;
    public static SqlCommand myCommand;
    public static string m_sCompanyTitle = "";
    public static bool g_bSystemDemo = true;
    public static bool g_bRetailVersion = true;
    public static bool g_bRentalVersion = true;
    public static bool g_bOrderOnlyVersion = true;
    public static bool g_bEnableQuotation = false;
    public static bool g_bSysQuoteAddHardwareLabourCharge = false;
    public static bool g_bSysQuoteAddSoftwareLabourCharge = false;
    public static bool g_bDemo = true;
    public static bool g_bUseSystemQuotation = false;
    public static string g_cat = "";
    public static string g_scat = "";
    public static string g_sscat = "";
    public static bool g_bUseAVGCost = false;
    public static bool g_bPDA = false;
    public static bool g_bIpad = false;
    public static bool g_bIphone = false;
    public static bool g_bAccessLoginBranchOnly = false;
    /// <summary>
    /// 获得EnumId
    /// </summary>
    /// <param name="sClass"> enum表class字段 例如：card_type</param>
    /// <param name="sValue"> enum表name字段 例如：dealer</param>
    /// <returns>返回enum id</returns>
    public static string GetEnumID(string sClass, string sValue)
    {
        DataSet dsEnum = new DataSet();
        HttpResponse Response = HttpContext.Current.Response;
        DBhelper dbhelper = new DBhelper();
        string sc = "SELECT id FROM enum WHERE class=@class AND name=@name";
        SqlCommand cmd = dbhelper.GetSqlStringCommond(sc);
        dbhelper.AddInParameter(cmd, "@class", sClass);
        dbhelper.AddInParameter(cmd, "@name", sValue);
        DataTable enumTable = dbhelper.ExecuteDataTable(cmd);

        string sID = enumTable.Rows[0]["id"].ToString();
        return sID;
    }
   public static void ShowExp(string query, Exception e)
    {
        HttpSessionState Session = HttpContext.Current.Session;
        HttpResponse Response = HttpContext.Current.Response;
        HttpRequest Request = HttpContext.Current.Request;
        {
            Response.Write("Execute SQL Query Error.<br>\r\nQuery = ");
            Response.Write(query);
            Response.Write("<br>\r\n Error: ");
            Response.Write(e);
            Response.Write("<br>\r\n");

        }

        string msg = "\r\n<font color=red><b>EXP</b></font><br>\r\n";

        msg += e.ToString();
        msg += "<br><br><font color=red><b>QUERY</b></font><br>\r\n";
        msg += query;
        msg += "<br><br>\r\n\r\n";
        msg += "ip : " + Session["ip"] + "<br>\r\n";
        msg += "login : " + Session["name"] + "<br>\r\n";
        msg += "email : " + Session["email"] + "<br>\r\n";
        msg += "url : " + Request.ServerVariables["URL"] + "?" + Request.ServerVariables["QUERY_STRING"] + "<br>\r\n";

        //AlertAdmin(msg);
    }
   public static bool SetSiteSettings(string name, string value)
    {
    
        string sc = " IF NOT EXISTS (SELECT value FROM settings WHERE name = '" + name + "') ";
        sc += " INSERT INTO settings(name, value) VALUES('" + name + "', '" + EncodeQuote(value) + "') ";
        sc += " ELSE UPDATE settings SET value = '" + EncodeQuote(value) + "' WHERE name='" + name + "' ";

        try
        {
            myCommand = new SqlCommand(sc);
            myCommand.Connection = myConnection;
            myCommand.Connection.Open();
            myCommand.ExecuteNonQuery();
            myCommand.Connection.Close();
        }
        catch (Exception e)
        {
            ShowExp(sc, e);
            return false;
        }
        return true;
    }


    public static string p(string key)
    {
       //System.Web.HttpResponse Response;
     HttpRequest Request=HttpContext.Current.Request;
    string sRet = "";
        if (key == null || key == "")
            return sRet;
        if (Request.Form[key] == null)
            return "";
        sRet = Request.Form[key];
        if (!CheckSQLAttack(sRet))
            sRet = "";
        return sRet.Trim();
    }
   public static string g(string key)
    {
        HttpRequest Request = HttpContext.Current.Request;
        string sRet = "";
        if (key == null || key == "")
            return sRet;
        if (Request.QueryString[key] != null)
            sRet = Request.QueryString[key];
        if (!CheckSQLAttack(sRet))
            sRet = "";
        return sRet.Trim();
    }

    private static bool CheckSQLAttack(string str)
    {
        
       
        if (str == null || str == "")
        {
            return true;
        }

        string s = str.ToLower();
        Trim(ref s);
        bool bUpdate = false;//(s.IndexOf("update") >= 0);
        bool bDelete = false;//(s.IndexOf("delete") >= 0);
        bool bDrop = false;//(s.IndexOf("drop") >= 0);
        bool bCreate = false;//(s.IndexOf("create") >= 0);
        bool bSelect = false;//(s.IndexOf("select") >= 0);
        bool bQuote = (s.IndexOf("'") >= 0);
        //	bool bSpace = (s.IndexOf(" ") >= 0);

        if (bUpdate || bDelete || bDrop || bCreate || bSelect || bQuote)
        {
            HttpRequest Request = HttpContext.Current.Request;
            HttpSessionState Session = HttpContext.Current.Session;
            string manager_email = GetSiteSettings("manager_email", "alert@eznz.com");
            string ip = Request.ServerVariables["REMOTE_ADDR"]; //cache ip
            string rip = ""; //real ip
            if (Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
                rip = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            else
                rip = ip;

            string sbody = "SQL Injection Attack detected and blocked. <br>";
            sbody += "ip : " + rip + "<br>";
            sbody += "user : " + Session["name"] + "<br>";
            sbody += "email : " + Session["email"] + "<br>";
            sbody += "Account# : " + Session["login_card_id"] + "<br>";
            sbody += "URI : " + Request.ServerVariables["URL"] + "<br>";
            sbody += "Parameter : " + str + "<br><br>";

            /*		sbody += "This attack is potential, the attacker was trying take control of you database useing<br>";
                    sbody += "a technic called 'SQL Injection Attack', which could issentially destory your database if succeeded.<br>";
                    sbody += "<br>We strongly suggest that you investigate this accoun/person if account# or user name is showing.<br>";
                    sbody += " Detailed log is available in database if evidence is needed to take legal action.<br>";

                    sbody += "<br>EZNZ Team";
            */
            return false;
        }
        return true;
    }


   public static string GetSiteSettings(string name)
    {
        return GetSiteSettings(name, "");
    }

    public static string GetSiteSettings(string name, string sDefault)
    {
        return GetSiteSettings(name, sDefault, false);
    }

    public static string GetSiteSettings(string name, string sDefault, bool bHide)
    {
        return GetSiteSettings(name, sDefault, bHide, "");
    }

    public static string GetSiteSettings(string name, string sDefault, bool bHide, string sDescription)
    {
        string s = "";
        string sc = "SELECT value FROM settings WHERE name='";
        sc += name;
        sc += "'";
        int rows = 0;
        DBhelper dbhelper = new DBhelper();
        DataTable dt = dbhelper.ExecuteDataTable(sc);
        rows = dt.Rows.Count;
        if (rows > 0)
                s = dt.Rows[0].ItemArray[0].ToString();
        if (rows == 0)
        {
            string sHide = "0";
            if (bHide)
                sHide = "1";
            if (name == "next_cheque_number")
                s = "100000";
            else
                s = sDefault;
            s = EncodeQuote(s);
            sc = " INSERT INTO settings (name, value, hidden, description) VALUES('" + name + "', '" + s + "', " + sHide + ", '" + sDescription + "') ";
            dbhelper.ExecuteNonQuery(sc);
        }
        //DEBUG("s=",s);
        return s;
    }
    public static bool TSIsDigit(string s) //is this string valid for int.parse
    {
        if (s == null || s == "")
            return false;
        Boolean bRet = true;
        for (int i = 0; i < s.Length; i++)
        {
            if (Char.IsDigit(s[i]) == false)
            {
                if (s[i] != '.' && s[i] != '-' && s[i] != '$')
                {
                    bRet = false;
                    break;
                }
            }
        }
        return bRet;
    }
   public static string TSGetUserCompanyByID(string id) //get user name from account table according to user id
    {
        DBhelper dbhelper = new DBhelper();
        string sc = "SELECT company, trading_name FROM card WHERE id='" + id + "'";
        DataTable dt = dbhelper.ExecuteDataTable(sc);
        int rows = dt.Rows.Count;
        if (rows > 0)
            return dt.Rows[0]["trading_name"].ToString();


        return "user not found";
    }
    public static bool IsInteger(string s) //is this string valid for int.parse
    {
        if (!TSIsDigit(s))
            return false;

        bool bRet = true;
        for (int i = 0; i < s.Length; i++)
        {
            if (s[i] == '.')
            {
                bRet = false;
                break;
            }
        }
        return bRet;
    }
    public static string EncodeQuote(string s) //double single quote for sql statements
    {
        if (s == null)
            return null;
        string ss = "";
        for (int i = 0; i < s.Length; i++)
        {
            if (s[i] == '\'')
                ss += '\''; //double it for SQL query
            ss += s[i];
        }
        return ss;
    }

    public static string DecodeQuote(string s) //reverse of EncodeQuote
    {
        if (s == null)
            return null;
        string ss = "";
        for (int i = 0; i < s.Length; i++)
        {
            if (s[i] == '\'')
                if (i < s.Length - 1)
                    if (s[i + 1] == '\'')
                        continue; //skip one	
        }
        return ss;
    }
   public static void Trim(ref string s)
    {
        if (s == null)
            return;
        s = s.TrimStart(null);
        s = s.TrimEnd(null);
    }
    public static long MyLongParse(string s)
    {
        Trim(ref s);
        if (s == null || s == "")
            return 0;

        long n = 0;
        try
        {
            n = long.Parse(s);
        }
        catch (Exception e)
        {
            ShowParseException(s);
        }
        return n;
    }

    public static int MyIntParse(string s)
    {
        Trim(ref s);
        if (s == null || s == "")
            return 0;

        return (int)MyDoubleParse(s);
    }

    public static double MyDoubleParse(string s)
    {
        Trim(ref s);

        if (s == null || s == "")
            return 0;
        if (s.IndexOf("(") == 0 && s.IndexOf(")") == s.Length - 1)
        {
            s = s.Replace("(", "");
            s = s.Replace(")", "");
            s = "-" + s;
        }

        double d = 0;

        try
        {
            d = double.Parse(s);
        }
        catch (Exception e)
        {
            ShowParseException(s);
        }
        return d;
    }

    public static bool MyBooleanParse(string s)
    {
        Trim(ref s);
        if (s == null || s == "" || s == "0")
            return false;
        else if (s == "1")
            return true;
        else if (s == "on")
            return true;
        else if (s == "true")
            return true;
        else if (s == "True")
            return true;
        else if (s == "On")
            return true;
        else if (s == "ON")
            return true;
        else if (s == "TRUE")
            return true;
        else if (s == "off")
            return false;

        bool b = false;
        try
        {
            b = Boolean.Parse(s);
        }
        catch (Exception e)
        {
            ShowParseException(s);
        }
        return b;
    }
    public static void ShowParseException(string s)
    {
         HttpResponse Response  = HttpContext.Current.Response;
        string s1 = "<br><br><center><h3>Error, input string \"<font color=red>" + s + "</font>\" was not in a correct format</h3></center>";
        s1 += Environment.StackTrace;
        Response.Write(s1);
        s1 += Environment.StackTrace;
        //	AlertAdmin(s1);
        Response.End();
    }
    public static string GetCatAccessGroupString(string card_id)
    {

        DBhelper dbhelper = new DBhelper();
        string sc = " SELECT limit FROM view_limit v JOIN card c ON v.id=c.cat_access_group WHERE c.id=" + card_id;
        DataTable dt = dbhelper.ExecuteDataTable(sc);
        if (dt.Rows.Count <= 0)
        {
            return "";
        }
       
        return dt.Rows[0]["limit"].ToString();
    }
    public static bool TS_UserLoggedIn()
    {

        HttpSessionState Session = HttpContext.Current.Session;
        if (Session[Company.m_sCompanyName + "loggedin"] != null)
        {
            return true;
        }
        //else if ((bool)Session[Company.m_sCompanyName + "loggedin"] != true)
        //{
        //    return false;
        //}
        //else if ((bool)Session[Company.m_sCompanyName + "loggedin"] == true)
        //{
        //    return true;
        //}
        else
        {
            return false;
        }

            
        
    }
    public static void RememberLastPage()
    {
          HttpRequest Request = HttpContext.Current.Request;
         HttpResponse Response  = HttpContext.Current.Response;
         HttpSessionState Session = HttpContext.Current.Session;
        string sl = "http";
        
        if (String.Compare(Request.ServerVariables["HTTPS"].ToString(), "on", true) == 0)
            sl += "s";

        sl += "://";
        sl += Request.ServerVariables["SERVER_NAME"];
        string sPort = Request.ServerVariables["SERVER_PORT"].ToString();
        if (sPort != "" && sPort != "80")
            sl += ":" + sPort;
        sl += Request.ServerVariables["URL"].ToString();
        sl += "?";
        sl += Request.ServerVariables["QUERY_STRING"];
        Session["LastPage"] = sl;
        //DEBUG("port=", sPort);
    }
    public static bool CheckAccess(string class_id)
    {

        HttpRequest Request = HttpContext.Current.Request;
        HttpResponse Response = HttpContext.Current.Response;
        HttpSessionState Session = HttpContext.Current.Session;
        if (class_id == GetAccessClassID("Administrator"))
            return true;

        string uri = Request.ServerVariables["URL"];
        uri = uri.Substring(0, uri.IndexOf(".aspx") + 5); //strip off parameters
        int i = uri.Length - 1;
        for (; i >= 0; i--)
        {
            if (uri[i] == '/')
                break;
        }
        uri = uri.Substring(i + 1, uri.Length - i - 1);
        return CheckAccess(class_id, uri);
    }
    public static bool CheckAccess(string class_id, string uri)
    {
        HttpSessionState Session = HttpContext.Current.Session;
        if (class_id == GetAccessClassID("Administrator"))
            return true;

        if (dstcom.Tables["checkaccess"] != null)
            dstcom.Tables["checkaccess"].Clear();
        //DEBUG("class=", class_id);
        string sc = "SELECT id ";
        sc += " FROM menu_admin_id ";
        sc += " WHERE uri LIKE '" + uri + "%' OR sisters LIKE '%" + uri + "%' ";
        //DEBUG("cs =", sc);
        try
        {
            SqlDataAdapter myCommand = new SqlDataAdapter(sc, myConnection);
            if (myCommand.Fill(dstcom, "checkaccess") <= 0) //if there's no record on available menus, then allow it
            {
                if (Session[m_sCompanyName + "AccessLevel"].ToString() != GetAccessClassID("no access"))
                    return true;
            }
        }
        catch (Exception e)
        {
            ShowExp(sc, e);
        }

        if (dstcom.Tables["checkaccess"] != null)
            dstcom.Tables["checkaccess"].Clear();

        sc = "SELECT a.id ";
        sc += " FROM menu_admin_access a JOIN menu_admin_id i ON i.id=a.menu ";
        //sc += " WHERE (i.uri LIKE '" + uri + "%' OR sisters LIKE '%" + uri + "%') "; 
        sc += " WHERE 1=1 ";
        sc += " AND a.class=" + class_id;
        //DEBUG("sc=", sc);
        try
        {
            SqlDataAdapter myCommand = new SqlDataAdapter(sc, myConnection);
            if (myCommand.Fill(dstcom, "checkaccess") >= 1)
                return true;
        }
        catch (Exception e)
        {
            ShowExp(sc, e);
        }
        return false;
    }
    public static string PrintBranchOptions(string current_id)
    {
        return PrintBranchOptions(current_id, false);
    }
   public static string PrintBranchOptions(string current_id, bool bWithAll)
    {
        HttpRequest Request = HttpContext.Current.Request;
        HttpResponse Response = HttpContext.Current.Response;
        HttpSessionState Session = HttpContext.Current.Session;
        if (dstcom.Tables["branch"] != null)
            dstcom.Tables["branch"].Clear();

        if (Session["branch_support"] != null)
        {
            if (current_id == null || current_id == "")
                current_id = Session["branch_id"].ToString();
        }
        else
            current_id = "1";
        int rows = 0;
        string s = "";
        string sc = " SELECT id, name FROM branch ";
        sc += " WHERE activated = 1 ";
        //if(!bGetAllowAccessID(Session[m_sCompanyName + "AccessLevel"].ToString()))
        if (int.Parse(Session["employee_access_level"].ToString()) < 10)
        {
            sc += " AND id =" + current_id + " ";
        }

        sc += " ORDER BY id ";
        //DEBUG("sc=",sc);
        try
        {
            myAdapter = new SqlDataAdapter(sc, myConnection);
            rows = myAdapter.Fill(dstcom, "branch");
        }
        catch (Exception e1)
        {
            ShowExp(sc, e1);
            return "";
        }

        s += "<select name=branch>";
        if (bWithAll)
            s += "<option value=''>All</option>";
        for (int i = 0; i < rows; i++)
        {
            DataRow dr = dstcom.Tables["branch"].Rows[i];
            string id = dr["id"].ToString();
            string name = dr["name"].ToString();
            s += "<option value=" + id;
            if (id == current_id)
                s += " selected";
            s += ">" + name + "</option>";
        }
        s += "</select>";
        return s;
    }
    public static bool PrintBranchNameOptions()
    {
        HttpRequest Request = HttpContext.Current.Request;
        HttpResponse Response = HttpContext.Current.Response;
        HttpSessionState Session = HttpContext.Current.Session;
        int nBranchID = 1;
        if (Request.Form["branch"] != null && Request.Form["branch"] != "")
        {
            nBranchID = int.Parse(Request.Form["branch"]);
            Session["branch_id"] = nBranchID;
        }
        else if (Session["branch_id"] != null && Session["branch_id"].ToString() != "")
        {
            nBranchID = MyIntParse(Session["branch_id"].ToString());
        }
        return PrintBranchNameOptions(nBranchID.ToString());
    }
    public static bool PrintBranchNameOptions(string current_branch)
    {
        return PrintBranchNameOptions(current_branch, "", false);
    }
    public static bool PrintBranchNameOptions(string current_branch, string onchange_url)
    {
        return PrintBranchNameOptions(current_branch, onchange_url, false);
    }

    public static bool PrintBranchNameOptions(string current_branch, string onchange_url, bool bWithAll)
    {
        HttpRequest Request = HttpContext.Current.Request;
        HttpResponse Response = HttpContext.Current.Response;
        HttpSessionState Session = HttpContext.Current.Session;
        DataSet dsBranch = new DataSet();
        int rows = 0;
        string sc = " SELECT b.id, b.name FROM branch b JOIN branch_cat bc ON bc.id = b.cat_id WHERE 1 = 1 ";
        if (!bGetAllowAccessID(Session[m_sCompanyName + "AccessLevel"].ToString()))
        {
            sc += " AND b.id = " + current_branch + " ";
        }
        if (int.Parse(Session["employee_access_level"].ToString()) < 10)
            sc += " AND b.id = " + Session["branch_id"] + " ";
        sc += " AND b.activated = 1 ";
        if (g_cat != "")
            sc += " AND bc.cat = N'" + EncodeQuote(g_cat) + "' ";
        if (g_scat != "")
            sc += " AND bc.s_cat = N'" + EncodeQuote(g_scat) + "' ";
        if (g_sscat != "")
            sc += " AND bc.ss_cat = N'" + EncodeQuote(g_sscat) + "' ";
        sc += " ORDER BY bc.cat, bc.s_cat, bc.ss_cat, b.name ";
        //DEBUG("sc",sc);	
        try
        {
            SqlDataAdapter myCommand = new SqlDataAdapter(sc, myConnection);
            rows = myCommand.Fill(dsBranch, "branch");
        }
        catch (Exception e)
        {
            ShowExp(sc, e);
            return false;
        }

        Response.Write("<select name=branch");
        if (onchange_url != "")
        {
            Response.Write(" onchange=\"window.location=('");
            Response.Write(onchange_url + "'+ this.options[this.selectedIndex].value ) \" ");
        }
        Response.Write(">");
        if (bWithAll)
        {
            //if(Session[m_sCompanyName + "AccessLevel"].ToString() == "10")
            //if(bGetAllowAccessID(Session[m_sCompanyName + "AccessLevel"].ToString()))
            if (int.Parse(Session["employee_access_level"].ToString()) == 10)
                Response.Write("<option value=0>All Branches</option>");
        }
        for (int i = 0; i < rows; i++)
        {
            string bname = dsBranch.Tables["branch"].Rows[i]["name"].ToString();
            string bid = dsBranch.Tables["branch"].Rows[i]["id"].ToString();
            Response.Write("<option value='" + bid + "' ");
            if (bid == current_branch)
                Response.Write("selected");
            Response.Write(">" + bname + "</option>");
        }
        if (rows == 0)
            Response.Write("<option value=1>Branch 1</option>");
        Response.Write("</select>");
        return true;
    }
    public static string GetEnumValue(string sClass, string id)
    {
    
        HttpResponse Response = HttpContext.Current.Response;
     
        if (id == "")
        {
            Response.Write("Empty ID, class=" + sClass);
            return "";
        }

        DataSet dsEnum = new DataSet();
        string sValue = "";
        string sc = "SELECT name FROM enum WHERE class='" + sClass + "' AND id=" + id;
        try
        {
            myAdapter = new SqlDataAdapter(sc, myConnection);
            if (myAdapter.Fill(dsEnum, "enum") == 1)
                sValue = dsEnum.Tables["enum"].Rows[0]["name"].ToString();
        }
        catch (Exception e)
        {
            ShowExp(sc, e);
        }
        return sValue;
    }
    public static bool bGetAllowAccessID(string sUserAccessID)
    {
        return bGetAllowAccessID(sUserAccessID, "");
    }
    public static bool bGetAllowAccessID(string sUserAccessID, string sAllowID)
    {
        if (sUserAccessID == "10")
            return true;
        string AllowAccessID = "";
        if (sAllowID != "")
            AllowAccessID = sAllowID;
        else
            AllowAccessID = GetSiteSettings("SET_ALLOW_ACCESS_ID_FOR_CARD_AND_OTHER_SECURITIES", "10,", true);
        //DEBUG("allowe =", AllowAccessID);	
        string sTemp = "";

        for (int i = 0; i < AllowAccessID.Length; i++)
        {
            if (AllowAccessID[i].ToString() == "," || AllowAccessID[i].ToString() == "|" || AllowAccessID[i].ToString() == ";")
            {
                if (sUserAccessID == sTemp)
                    return true;
                else
                    sTemp = ""; //clean up last id
            }
            if (AllowAccessID[i].ToString() != "," || AllowAccessID[i].ToString() != "|" || AllowAccessID[i].ToString() != ";")
            {
                try
                {
                    sTemp += (int.Parse(AllowAccessID[i].ToString())).ToString();
                }
                catch (Exception e) { }
            }
            if (i == AllowAccessID.Length - 1)
            {
                if (sUserAccessID == sTemp)
                    return true;
            }

        }
        return false;
    }

    public static string GetAccessClassID(string name)
    {
        if (dstcom.Tables["getclassid"] != null)
            dstcom.Tables["getclassid"].Clear();

        string sc = " SELECT id FROM menu_access_class WHERE name='" + name + "'";
        try
        {
            SqlDataAdapter myCommand = new SqlDataAdapter(sc, myConnection);
            if (myCommand.Fill(dstcom, "getclassid") == 1)
                return dstcom.Tables["getclassid"].Rows[0]["id"].ToString();
        }
        catch (Exception e)
        {
            ShowExp(sc, e);
        }
        return name;
    }
    public static void BackToLastPage()
    {
        string url;
        HttpRequest Request = HttpContext.Current.Request;
        HttpResponse Response = HttpContext.Current.Response;
        HttpSessionState Session = HttpContext.Current.Session;
        string currentURL = Request.ServerVariables["URL"];
        if (Session["LastPage"] != null)
        {
            url = Session["LastPage"].ToString();
            int p = 0;
            if (url.IndexOf("checkout.aspx") >= 0)
            {
                if (Session[Company.m_sCompanyName + "sales"] != null && (bool)Session[Company.m_sCompanyName + "sales"])
                {
                    url = "/sales/pos.aspx";
                }
            }

            if (Session["card_type"] != null && Session["card_type"] != "")
            {
                if (Session["card_type"].ToString() == "2")
                {
                    if (url.IndexOf("/dealer") < 0 && Request.ServerVariables["URL"].ToString().IndexOf("/dealer/login.aspx") >= 0)
                    {
                        string tmpURL = Request.ServerVariables["URL"].ToString();
                        tmpURL = tmpURL.Replace("login.aspx", "");
                        url = tmpURL;
                    }
                    else if (url.IndexOf("/dealer") < 0 && Request.ServerVariables["URL"].ToString().IndexOf("/dealer") < 0)
                        url = "dealer/";
                }
            }

        }
        else
        {
            url = "";

            if (Session["card_type"] != null && Session["card_type"] != "")
            {
                if (Session["card_type"].ToString() == "2")
                {
                    if ((Request.ServerVariables["URL"].ToString()).IndexOf("/dealer") < 0)
                        url = "dealer/";
                }
            }

            url += "index.aspx";
        }
        //	Response.Redirect(url);
        Response.Write("<meta http-equiv=\"refresh\" content=\"0; URL=" + url + "\">");
        //Response.Write("<meta http-equiv=\"refresh\" content=\"0; URL=" + url + "\">");
        //return;
    }
   public static void TSRemoveCache(string cn) //remove(refresh) cache 
    {

        //DEBUG("removing cache, m_sCompanyName=" + m_sCompanyName + " cn=", cn);
        HttpRuntime.Cache.Remove(cn); //remove catalog cache

        //remove all catalog contents cache
        IDictionaryEnumerator ide = HttpRuntime.Cache.GetEnumerator();
        if (ide == null)
            return;
        for (int i = HttpRuntime.Cache.Count - 1; i >= 0; i--)
        {
            ide.MoveNext();
            string s = ide.Key.ToString();
            if (s.Length > 7)
            {
                if (String.Compare(s.Substring(0, 7), "System.", true) != 0 && String.Compare(s.Substring(0, 5), "ISAPI", true) != 0)
                {
                    if (s.Length > Company.m_sCompanyName.Length)
                    {
                        if (String.Compare(s.Substring(0, Company.m_sCompanyName.Length), Company.m_sCompanyName, true) == 0)
                        {
                            //						if(Cache[s] != null)
                            //						{
                            HttpRuntime.Cache.Remove(s);
                            //							DEBUG(s, " removed");
                            //						}
                        }
                        //					else
                        //					{
                        //					DEBUG("sub=", s.Substring(0, 4));
                        //					}
                    }
                }
            }
        }
    }

    public static void TSRemoveCache() //remove(refresh) cache 
    {
    
        
        IDictionaryEnumerator ide = HttpRuntime.Cache.GetEnumerator();
        for (int i = HttpRuntime.Cache.Count - 1; i >= 0; i--)
        {
            ide.MoveNext();
            string s = ide.Key.ToString();
            if (s.Length > 7)
            {
                if (String.Compare(s.Substring(0, 7), "System.", true) != 0 && String.Compare(s.Substring(0, 5), "ISAPI", true) != 0)
                {
                    if (s.Length > Company.m_sCompanyName.Length)
                    {
                        if (String.Compare(s.Substring(0, Company.m_sCompanyName.Length), Company.m_sCompanyName, true) == 0)
                        {
                            HttpRuntime.Cache.Remove(s);
                        }
                    }
                }
            }
        }
    }
  public static  bool SecurityCheck(string sLevel)
    {
        HttpRequest Request = HttpContext.Current.Request;
        HttpResponse Response = HttpContext.Current.Response;
        HttpSessionState Session = HttpContext.Current.Session;
        if (sLevel == "normal")
        {
            if (!TS_UserLoggedIn())
            {
                RememberLastPage();
                Response.Redirect("login.aspx");
                return false;
            }
            else
            {
                return true;
            }
        }
        return Common.SecurityCheck(sLevel, true);
    }
   public static bool SecurityCheck(string sLevel, bool bSayNo)
    {
        HttpRequest Request = HttpContext.Current.Request;
        HttpResponse Response = HttpContext.Current.Response;
        HttpSessionState Session = HttpContext.Current.Session;
        if (!TS_UserLoggedIn())
        {
            if (!bSayNo)
                return false;
            RememberLastPage();
            Response.Redirect("login.aspx");
            return false;
        }

     

        if (Common.CheckAccess(Session[m_sCompanyName + "AccessLevel"].ToString()))
            return true;
        else if (bSayNo)
        {
            Response.Write("<h3>ACCESS DENIED1</h3>");
            Response.End();
        }
        return false;
    }
}
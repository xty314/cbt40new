using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;

/// <summary>
/// Common 的摘要说明
/// 公用的static function ，内容整理取自原common.cs 
/// </summary>
public static class Common
{
  private static  System.Web.HttpRequest Request = HttpContext.Current.Request;
    private static System.Web.HttpResponse Response  = HttpContext.Current.Response;
    private static System.Web.SessionState.HttpSessionState Session = HttpContext.Current.Session;
    private static DBhelper dbhelper = new DBhelper();
    /// <summary>
    /// 获得EnumId
    /// </summary>
    /// <param name="sClass"> enum表class字段 例如：card_type</param>
    /// <param name="sValue"> enum表name字段 例如：dealer</param>
    /// <returns>返回enum id</returns>
    public static string GetEnumID(string sClass, string sValue)
    {
        DataSet dsEnum = new DataSet();
      
        string sc = "SELECT id FROM enum WHERE class=@class AND name=@name";
        SqlCommand cmd = dbhelper.GetSqlStringCommond(sc);
        dbhelper.AddInParameter(cmd, "@class", sClass);
        dbhelper.AddInParameter(cmd, "@name", sValue);
        DataTable enumTable = dbhelper.ExecuteDataTable(cmd);
        string sID = enumTable.Rows[0]["id"].ToString();
        return sID;
    }
    public static string p(string key)
    {
       //System.Web.HttpResponse Response;
       System.Web.HttpRequest Request=HttpContext.Current.Request;
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
        string s1 = "<br><br><center><h3>Error, input string \"<font color=red>" + s + "</font>\" was not in a correct format</h3></center>";
        s1 += Environment.StackTrace;
        Response.Write(s1);
        s1 += Environment.StackTrace;
        //	AlertAdmin(s1);
        Response.End();
    }
    //public static double MyMoneyParse(string s)
    //{
    //    Trim(ref s);
    //    if (s == null || s == "")
    //        return 0;

    //    double d = 0;
    //    try
    //    {
    //        d = double.Parse(s, NumberStyles.Currency, null);
    //    }
    //    catch (Exception e)
    //    {
    //        ShowParseException(s);
    //    }
    //    return d;
    //}

}
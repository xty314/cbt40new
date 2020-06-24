<%@ WebHandler  Language="C#" Class="eprice" %>

using System;
using System.Web;
using System.Web.SessionState;
using System.Data;
using System.Collections;
using System.IO;

public class eprice : IHttpHandler{
    string cat;
    string s_cat;
    string ss_cat;

    string kw = "";

    int m_iTotalItems = 0;

    string m_editPriceBox = ""; //for edit retail price

    string m_type = null;
    bool m_bPhasedOut = false;
    string m_sFilter = "0";
    bool m_bService = false;
    bool m_bIDCheck = false;
    bool m_bBarcodePriceEach = false;
    bool m_bBarcodePriceKG  = false;
    bool m_bAutoWeigh = false;
    bool m_bWebsite =false;
    bool m_bAccessUpdate = false;
    bool m_bCoreRange = false;
    string m_ob = "";
    string m_desc = "";
    string m_ph = "";
    string m_cmd = "";
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        int page,rows;
        if (context.Request.QueryString["page"] == null)
        {
            page = 1;
        }
        else
        {
            page =int.Parse(context.Request.QueryString["page"]);
        }
        if (context.Request.QueryString["rows"] == null)
        {
            rows = 20;
        }
        else
        {
            rows =int.Parse(context.Request.QueryString["rows"]);
        }
        string sidx = context.Request.QueryString["sidx"];
        string sord = context.Request.QueryString["sord"];
        string sc = " SELECT  * FROM (";
        sc += " SELECT COUNT(1) OVER() AS count, p.eta, b.barcode, c.id, c.name_cn, c.price1, c.code, c.brand, c.name, c.cat, c.s_cat, c.ss_cat, c.average_cost, c.supplier_code ";
        sc += ", ISNULL((SELECT sum(qty) FROM stock_qty WHERE c.code = code ";
        sc += " ), 0) AS stock ";
        sc += ", c.price1 as price ";
        sc += ", c.supplier_price, c.rate ";
        sc += ", (SELECT TOP 1 id FROM bom WHERE code = c.code) AS bom_id ";
        sc += " FROM code_relations c ";
        sc += " LEFT OUTER JOIN barcode b ON c.code = b.item_code ";
        sc += " LEFT OUTER JOIN product p ON p.code = b.item_code  ";
        sc += " WHERE 1 = 1 ";
        sc += ") AS dTable ";
        if (sidx!=null&&sidx!="")
        {
            sc += " ORDER BY " + sidx + " " + sord;
        }
        DBhelper dbhelper = new DBhelper();
        DataTable dt = dbhelper.ExecutePageTable(sc, rows, page);
      
        int records = (int)dt.Rows[0]["count"];
        double total =(double)records/rows;
        string tables = dt.ToJson();
        Hashtable ht = new Hashtable();
        ht.Add("records", records);//总数据数
        ht.Add("page", page);
        ht.Add("total", Math.Ceiling(total));//总页数
        ht.Add("rows", tables);
        context.Response.Write(ht.ToJson());

    }




    public bool IsReusable {
        get {
            return false;
        }
    }

}
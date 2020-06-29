<%@ WebHandler  Language="C#" Class="eprice" %>

using System;
using System.Web;
using System.Web.SessionState;
using System.Data;
using System.Collections;
using System.IO;

public class eprice : IHttpHandler{
 
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        int page,rows;
        if (context.Request.Form["page"] == null)
        {
            page = 1;
        }
        else
        {
            page =int.Parse(context.Request.Form["page"]);
        }
        if (context.Request.Form["rows"] == null)
        {
            rows = 20;
        }
        else
        {
            rows =int.Parse(context.Request.Form["rows"]);
        }
        string sidx = context.Request.Form["sidx"];
        string sord = context.Request.Form["sord"];
        string supplier = context.Request.Form["supplier"];
        string cat=context.Request.Form["cat"];
        //string scat=context.Request.Form["scat"];
        string scat=context.Request.Form["scat"].ToString();
        string sscat=context.Request.Form["sscat"].ToString();
        string sc = " SELECT  * FROM (";
        sc += " SELECT COUNT(1) OVER() AS count, b.barcode, c.id, c.name_cn, c.price1, c.code, c.brand, c.name, c.cat, c.s_cat, c.ss_cat, c.average_cost, c.supplier_code ";
        sc += ", ISNULL((SELECT sum(qty) FROM stock_qty WHERE c.code = code ";
        sc += " ), 0) AS stock ";
        sc += ", c.price1 as price ";
        sc += ", c.supplier_price, c.rate ";
        sc += ", (SELECT TOP 1 id FROM bom WHERE code = c.code) AS bom_id ";
        sc += " FROM code_relations c ";
        sc += " LEFT OUTER JOIN barcode b ON c.code = b.item_code ";
        //sc += " LEFT OUTER JOIN product p ON p.code = b.item_code  ";
        sc += " WHERE 1 = 1  ";
        if (supplier!="-1")
        {
            sc += "AND c.supplier=" + supplier;
        }
        if (cat != "-1")
        {
            sc += " AND c.cat=N'" + cat + "'";
        }
        if (scat != "-1")
        {
            sc += " AND c.s_cat=N'" + scat + "'";
        }
        if (sscat != "-1")
        {
            sc += " AND c.ss_cat=N'" + sscat + "'";
        }
        sc += " ) AS dTable ";
        if (sidx!=null&&sidx!="")
        {
            sc += " ORDER BY " + sidx + " " + sord;
        }

        DBhelper dbhelper = new DBhelper();
        DataTable dt = dbhelper.ExecutePageTable(sc, rows, page);

        int records;
        double total;
        string tables;
        if (dt.Rows.Count > 1)
        {
            records= (int)dt.Rows[0]["count"];
            total = (double)records / rows;
            tables = dt.ToJson();
        }
        else
        {
            records = 0;
            total = 0;
            tables =  dt.ToJson();
        }

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
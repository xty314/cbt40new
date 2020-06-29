using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class v2_especial : System.Web.UI.Page
{

    string ebrand;
    string ecat;
    string es_cat;
    string ess_cat;
    DBhelper dbhelper = new DBhelper();
    string brand;
    string cat;
    string s_cat;
    string ss_cat;

    string m_type = null;
    int page = 1;
    const int m_nPageSize = 15; //how many rows in oen page
    DataSet dst = new DataSet();    //for creating Temp talbes templated on an existing sql table
    DataTable productDT = new DataTable();
    const string cols = "8";    //how many columns main table has, used to write colspan=
    const string tableTitle = "Edit All Specials";
    const string thisurl = "especial.aspx";
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    public void PrintPage()
    {
        GetQueryStrings();
        WriteHeaders();
        if (m_type == "update")
        {
            //DEBUG("update", page);
            string update = Request.Form["update"];
            if (UpdateAllRows())
            {
                Common.TSRemoveCache(Company.m_sCompanyName + "_" + "header");
                string s = "<br><br>done! wait a moment......... <br>\r\n";
                s += "<meta http-equiv=\"refresh\" content=\"1; URL=";
                s += WriteURLWithoutPageNumber();
                s += "&p=";
                s += page;
                s += "\"></body></html>";
                Response.Write(s);
            }
        }
        else
        {
            if (!DoSearch())
                return;

            MyDrawTable();
        }

    }
    void DrawTableHeader()
    {
        StringBuilder sb = new StringBuilder(); ;
        sb.Append("<table width=100%  align=center valign=center cellspacing=0 cellpadding=0 border=1 bordercolor=#EEEEEE bgcolor=white");
        sb.Append(" style=\"font-family:Verdana;font-size:8pt;border-width:1px;border-style:Solid;border-collapse:collapse;fixed\">");
        sb.Append("<tr style=\"color:white;background-color:#666696;font-weight:bold;\">\r\n");
        sb.Append("<td width=50>code</td>");
        sb.Append("<td>brand</td>");
        sb.Append("<td>name (description)</td>");
        //	sb.Append("<td width=50>cost</td>");
        //	sb.Append("<td width=50>rate</td>");
        //	sb.Append("<td>price</td>");
        //	sb.Append("<td>stock</td>");
        sb.Append("<td>special</td>");
        sb.Append("</tr>\r\n");

        Response.Write(sb.ToString());
        Response.Flush();
    }
    Boolean MyDrawTable()
    {
        Boolean bRet = true;
        DrawTableHeader();
        string s = "";
        DataRow dr;
        Boolean alterColor = true;
        int startPage = (page - 1) * m_nPageSize;
        for (int i = startPage; i < productDT.Rows.Count; i++)
        {
            if (i - startPage >= m_nPageSize)
                break;
            dr = productDT.Rows[i];
            alterColor = !alterColor;
            if (!DrawRow(dr, i, alterColor))
            {
                bRet = false;
                break;
            }
        }

        StringBuilder sb = new StringBuilder();
        sb.Append("<tr><td colspan=" + cols + " align=right><input type=submit name=update value='Update'></td></tr>");
        sb.Append("<tr><td colspan=" + cols + " align=right>Page: ");
        int pages = productDT.Rows.Count / m_nPageSize + 1;
        for (int i = 1; i <= pages; i++)
        {
            if (i != page)
            {
                sb.Append("<a href=");
                sb.Append(WriteURLWithoutPageNumber());
                sb.Append("&p=");
                sb.Append(i.ToString());
                sb.Append(">");
                sb.Append(i.ToString());
                sb.Append("</a> ");
            }
            else
            {
                sb.Append(i.ToString());
                sb.Append(" ");
            }
        }
        sb.Append("</td></tr>");
        sb.Append("</table>\r\n");
        Response.Write(sb.ToString());

        return bRet;
    }
    Boolean DrawRow(DataRow dr, int i, Boolean alterColor)
    {
        string id = dr["id"].ToString();
        string code = dr["code"].ToString();
        string name = dr["name"].ToString();
        string brand = dr["brand"].ToString();
        //	string stock = dr["stock"].ToString();
        //	string supplier_price = dr["supplier_price"].ToString();
        //	string rate = dr["rate"].ToString();
        //	string price = dr["price"].ToString();
        string special = dr["special"].ToString();
        //DEBUG("specialPrice=", specialPrice);
        string index = i.ToString();

        StringBuilder sb = new StringBuilder(); ;

        sb.Append("<tr");
        if (alterColor)
            sb.Append(" bgcolor=#EEEEEE");
        sb.Append(">");

        sb.Append("<input type=hidden name=id");
        sb.Append(index);
        sb.Append(" value='");
        sb.Append(id);
        sb.Append("'>");

        sb.Append("<input type=hidden name=code");
        sb.Append(index);
        sb.Append(" value='");
        sb.Append(code);
        sb.Append("'>");

        //	sb.Append("<input type=hidden name=supplier_price");
        //	sb.Append(index);
        //	sb.Append(" value='");
        //	sb.Append(supplier_price);
        //	sb.Append("'>");

        sb.Append("<input type=hidden name=special_old");
        sb.Append(index);
        sb.Append(" value=");
        if (special != "0")
            sb.Append("on");
        sb.Append(">");

        sb.Append("<td>" + code + "</td>");
        sb.Append("<td>" + brand + "</td>");
        sb.Append("<td>" + name + "</td>");
        //	sb.Append("<td>" + supplier_price + "</td>");
        //	sb.Append("<td>" + rate + "</td>");
        //	sb.Append("<td><input type=text size=7 name=price");
        //	sb.Append(index);
        //	sb.Append(" value='");
        //	if(specialPrice != "0")
        //		sb.Append(specialPrice);
        //	else
        //		sb.Append(price);
        //	sb.Append("'></td>");

        /*	sb.Append("<td><input type=text size=7 name=stock");
            sb.Append(index);
            sb.Append(" value='");
            sb.Append(stock);
            sb.Append("'></td>");
        */
        sb.Append("<td><input type=checkbox name=special");
        sb.Append(index);
        if (special != "0")
            sb.Append(" checked");
        sb.Append("></td>");

        sb.Append("</tr>");

        Response.Write(sb.ToString());
        Response.Flush();
        return true;
    }
    private void GetQueryStrings()
    {
        brand = Request.QueryString["b"];
        cat = Request.QueryString["c"];
        s_cat = Request.QueryString["s"];
        ss_cat = Request.QueryString["ss"];

        ebrand = HttpUtility.UrlEncode(brand);
        ecat = HttpUtility.UrlEncode(cat);
        es_cat = HttpUtility.UrlEncode(s_cat);
        ess_cat = HttpUtility.UrlEncode(ss_cat);
        m_type = Request.QueryString["t"];
        string spage = Request.QueryString["p"];
        if (spage != null)
            page = int.Parse(spage);
        //DEBUG("page=", page);
    }
   private void WriteHeaders()
    {
        StringBuilder sb = new StringBuilder();
        //	sb.Append("<html><style type=\"text/css\">td{FONT-WEIGHT:300;FONT-SIZE:8PT;FONT-FAMILY:verdana;}");
        //	sb.Append("body{FONT-WEIGHT:300;FONT-SIZE:8PT;FONT-FAMILY:verdana;}</style>");
        //	sb.Append("<body bgcolor=#666696>\r\n");
        //	sb.Append("<table width=100% height=100% bgcolor=white align=center valign=center><tr><td valign=top>");
        sb.Append("<br><center><h3>" + tableTitle + "</h3></center>");
        sb.Append("<form action=");
        sb.Append(WriteURLWithoutPageNumber());
        sb.Append("&t=update&p=");
        sb.Append(page);
        sb.Append(" method=post>\r\n");
        Response.Write(sb.ToString());
        Response.Flush();
    }
    private Boolean DoSearch()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("SELECT c.id, p.code, p.brand, p.name, p.cat, p.s_cat, p.ss_cat, ISNULL(s.code, 0) AS special ");
        //	sb.Append(", s.price AS specialPrice ");
        sb.Append(" FROM product p JOIN code_relations c ON p.code=c.code JOIN code_branch s on p.code=s.code");
        sb.Append(" WHERE s.special= 1");
        sb.Append(" ORDER BY p.brand, p.s_cat, p.ss_cat, p.name, p.code");
        //DEBUG("query=", sb.ToString());	
        productDT = dbhelper.ExecuteDataTable(sb.ToString());
  
        return true;
    }

    private string WriteURLWithoutPageNumber()
    {
        StringBuilder sb = new StringBuilder();

        sb.Append(thisurl + "?");
        if (brand != null)
        {
            sb.Append("b=");
            sb.Append(ebrand);
        }
        else
        {
            sb.Append("c=");
            sb.Append(ecat);
        }
        if (s_cat != null)
        {
            sb.Append("&s=");
            sb.Append(es_cat);
        }
        if (ss_cat != null)
        {
            sb.Append("&ss=");
            sb.Append(ess_cat);
        }
        sb.Append("&r=" + DateTime.Now.ToOADate());
        return sb.ToString();
    }
    private Boolean UpdateAllRows()
    {
        int i = (page - 1) * m_nPageSize;
        string id = Request.Form["id" + i.ToString()];
        while (id != null)
        {
            if (!UpdateOneRow(i.ToString()))
                return false; ;
            i++;
            id = Request.Form["id" + i.ToString()];
        }
        return true;
    }
    private Boolean UpdateOneRow(string sRow)
    {
        Boolean bRet = true;

        string id = Request.Form["id" + sRow];
        string code = Request.Form["code" + sRow];
        string name = Request.Form["name" + sRow];
        string brand = Request.Form["brand" + sRow];
        //	string supplier_price = Request.Form["supplier_price"+sRow];
        //	string price = Request.Form["price"+sRow];
        //	string stock = Request.Form["stock"+sRow];


        //	if(stock == "")
        //		stock = "null";
        bool bSpecial = (Request.Form["special" + sRow] == "on");
        bool bSpecial_old = (Request.Form["special_old" + sRow] == "on");

        if (bSpecial != bSpecial_old)
        {
            string sc = "";
            if (bSpecial) //do insert
                sc = "INSERT INTO specials (code) VALUES(" + code + ")";
            else //do delete
                sc = "UPDATE code_branch SET special='0'  WHERE code=" + code;

            dbhelper.ExecuteNonQuery(sc);
        }
        return bRet;
    }
}
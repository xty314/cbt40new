using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class v2_stock : System.Web.UI.Page
{
    string m_id = "";
    string m_name = "";
    string m_sBranch = "";
    string m_last_search = "";
    string cat = "";
    string s_cat = "";
    string ss_cat = "";
    string sSystem = "";
    string sOption = "";
    string ra_id = "";
    string ra_code = "";

    int m_nPageSize = 20;
    int m_page = 1;
    int m_nPageSize1 = 15;
    int m_page1 = 1;
    int m_pageQty = 1;

    int m_RowsReturn = 0;
    int m_SerialReturn = 0;

    //current edit products
    string m_sn = "";
    string m_product_code = "";
    string m_cost = "";
    string m_purchase_date = "";
    string m_branch_id = "";
    string m_po_number = "";
    string m_supplier = "";
    string m_supplier_code = "";
    string m_status = "";

    string m_snQuery = "";
    string m_prodQuery = "";

    string m_branchid = "1";
    string m_sort = "";
    bool m_bDesc = false;
    bool m_ballbranchs = false;
    string tableWidth = "97%";
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void PrintPage()
    {

        if (Session["branch_support"] != null)
        {
            if (Request.QueryString["b"] != null && Request.QueryString["b"] != "")
            {
                m_branchid = Request.QueryString["b"];
                if (m_branchid == "all")
                    m_ballbranchs = true;
            }
            else if (Session["branch_id"] != null)
            {
                m_branchid = Session["branch_id"].ToString();
            }
            if (Request.Form["branch"] != null && Request.Form["branch"] != "")
                m_branchid = Request.Form["branch"];
        }

        if (Request.QueryString["sort"] != null)
            m_sort = Request.QueryString["sort"];
        if (Request.QueryString["desc"] == "1")
            m_bDesc = true;

        //getting catalog and sub catalog 
        if (Request.QueryString["cat"] != null && Request.QueryString["cat"] != "")
            cat = Request.QueryString["cat"].ToString();
        if (Request.QueryString["s_cat"] != null && Request.QueryString["s_cat"] != "")
            s_cat = Request.QueryString["s_cat"].ToString();
        if (Request.QueryString["ss_cat"] != null && Request.QueryString["ss_cat"] != "")
            ss_cat = Request.QueryString["ss_cat"].ToString();

        if (Request.QueryString["ra"] != null && Request.QueryString["ra"] != "")
            ra_code = Request.QueryString["ra"].ToString();
        if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
            ra_id = Request.QueryString["id"].ToString();
        if (Request.QueryString["op"] != null && Request.QueryString["op"] != "")
            sOption = Request.QueryString["op"].ToString();
        if (Request.QueryString["s"] != null && Request.QueryString["s"] != "")
            sSystem = Request.QueryString["s"].ToString();

        if (Request.QueryString["p"] != null)
        {
            if (Common.IsInteger(Request.QueryString["p"]))
                m_page = int.Parse(Request.QueryString["p"]);
        }
        if (Request.QueryString["sp"] != null)
        {
            if (Common.IsInteger(Request.QueryString["sp"]))
                m_page1 = int.Parse(Request.QueryString["sp"]);
        }
        if (Request.QueryString["qtyp"] != null)
        {
            if (Common.IsInteger(Request.QueryString["qtyp"]))
                m_pageQty = int.Parse(Request.QueryString["qtyp"]);
        }
     

        //	Response.Write("<h3><center>STOCK LIST</h3></center>");
        Response.Write("<form name=frmSearchProduct method=post action=stock.aspx?search=" + m_last_search + ">");
        Response.Write("<br><table align=center cellspacing=0 cellpadding=0 width=" + tableWidth + " valign=center bgcolor=white border=0><tr>");
        Response.Write("<td width='17' height='30' id='top-header1'>&nbsp;</td>");
        Response.Write("<td height='30' class='pageName' id='top-header2' ><font size=+1>Stock List</font></font>");

        Response.Write("<td width='76' id='top-header3'>&nbsp;</td>");
        Response.Write("  <td  height='30' id='top-header4'>&nbsp;</td>");
        Response.Write("<td width='40' id='top-header5'>&nbsp;</td>");
        Response.Write("</tr></table>");

        GetSearch();

        if (Request.Form["cmdUpdate"] == "Search Product" || Request.QueryString["search"] != null)
        {
            string sSearch = "", sSearchSN = "";
            if (Request.Form["txtSearch"] != null)
                sSearch = Request.Form["txtSearch"].ToString();
            if (Request.Form["txtSearchSN"] != null)
                sSearchSN = Request.Form["txtSearchSN"].ToString();

            if (Request.QueryString["sp"] != null && Request.QueryString["search"] != null)
            {
                m_last_search = Request.QueryString["search"].ToString();
                if (!SearchProduct(m_last_search))
                    return;
            }
            else
            {
                if (sSearch != "")
                {
                    if (!SearchProduct(sSearch))
                        return;
                    m_last_search = sSearch;
                }
                else if (sSearchSN != "")
                {
                    if (!SearchSNQty(sSearchSN))
                        return;
                }
            }

            if (sSearch != "")
            {
                if (m_RowsReturn <= 0 && dst.Tables["searchQty"].Rows.Count <= 0)
                {
                    Response.Write("<script language=javascript>");
                    Response.Write("window.alert('No Item Found')\r\n");
                    Response.Write("</script");
                    Response.Write(">");
                }
                else
                    BindSearchProduct();
            }
            else if (sSearchSN != "")
            {
                if (dst.Tables["snQty"].Rows.Count <= 0)
                {
                    Response.Write("<script language=javascript>");
                    Response.Write("window.alert('No Item Found')\r\n");
                    Response.Write("</script");
                    Response.Write(">");
                }
                else
                    BindSearchSNProduct();
            }
        }

        if (Request.Form["cmd"] == "Update Product")
        {
            if (Request.Form["txtSN"] != null)
                m_sn = Request.Form["txtSN"].ToString();
            if (Request.Form["txtProd"] != null)
                m_product_code = Request.Form["txtProd"].ToString();
            if (Request.Form["txtCost"] != null)
                m_cost = Request.Form["txtCost"].ToString();
            if (Request.Form["txtProd"] != null)
                m_purchase_date = Request.Form["txtPurDate"].ToString();
            if (Request.Form["txtProd"] != null)
                m_branch_id = Request.Form["txtBranchID"].ToString();
            if (Request.Form["txtProd"] != null)
                m_po_number = Request.Form["txtPOnum"].ToString();
            if (Request.Form["txtProd"] != null)
                m_supplier = Request.Form["txtSupp"].ToString();
            if (Request.Form["txtProd"] != null)
                m_supplier_code = Request.Form["txtSuppCode"].ToString();
            if (Request.Form["txtProd"] != null)
                m_status = Request.Form["txtStatus"].ToString();

            if (!UpdateProduct())
                return;
        }
        if (Request.QueryString["del"] != null)
        {
            string s_id = Request.QueryString["del"].ToString();
            if (!DoDeleteProduct(s_id))
                return;
        }

        /*if(Request.QueryString["edit"] != null || Request.QueryString["id"] != null)
        {
            if(Request.QueryString["i"] != null)
                m_snQuery = Request.QueryString["i"].ToString();

            if(!GetEditProduct(m_snQuery))
                return;
            DisplayEditProduct();
        }
        */
        if (!GetStockQty())
            return;
        BindStockQty();
   
    }
    void BindStockQty()
    {

        Response.Write("<table width='" + tableWidth + "' align=center cellspacing=0 cellpadding=2 border=1 bordercolor=#EEEEEE bgcolor=white");
        Response.Write(" style=\"font-family:Verdana;font-size:8pt;border-width:1px;border-style:Solid;border-collapse:collapse;fixed\">");
        //Response.Write("<tr style=\"color:white;background-color:#666696;font-weight:bold;\">");
        //Response.Write("<tr bgcolor=#E3E3E3><td align=center colspan=2>Stock Quantity Table</td></tr>");
        //Response.Write("<br><hr size=1 color=black>");
        //Response.Write("<tr><td>Stock Quantity</td></tr>");
        //paging class
        PageIndex m_cPI = new PageIndex(); //page index class
        if (Request.QueryString["p"] != null)
            m_cPI.CurrentPage = int.Parse(Request.QueryString["p"]);
        if (Request.QueryString["spb"] != null)
            m_cPI.StartPageButton = int.Parse(Request.QueryString["spb"]);

        string uri = "?cat=" + HttpUtility.UrlEncode(cat) + "&s_cat=" + HttpUtility.UrlEncode(s_cat) + "&ss_cat=" + HttpUtility.UrlEncode(ss_cat);
        if (Request.QueryString["p"] != null && Request.QueryString["spb"] != null)
            uri += "&p=" + Request.QueryString["p"].ToString() + "&spb=" + Request.QueryString["spb"].ToString();


        if (ra_code != "")
        {
            uri += "&ra=" + ra_code;
            uri += "&id=" + ra_id;
        }

        int rows = dst.Tables["stock_qty"].Rows.Count;
        m_cPI.TotalRows = rows;
        m_cPI.PageSize = 30;
        m_cPI.URI = "?ra=" + ra_code + "";
        if (m_branchid != "")
            m_cPI.URI += "&b=" + m_branchid;
        m_cPI.URI += "&s=" + sSystem + "&op=" + sOption + "&id=" + ra_id + "&cat=" + HttpUtility.UrlEncode(cat) + "&s_cat=" + HttpUtility.UrlEncode(s_cat) + "&ss_cat=" + HttpUtility.UrlEncode(ss_cat);
        if (m_sort != null && m_sort != "")
            m_cPI.URI += "&sort=" + HttpUtility.UrlEncode(m_sort) + "";
        if (m_bDesc)
            m_cPI.URI += "&desc=1";
        if (Request.QueryString["pr"] != null)
            m_cPI.URI += "&pr=" + Request.QueryString["pr"];

        //m_cPI.URI = "?stock.aspx?b="+ m_branch_id +"&cat="+ HttpUtility.UrlEncode(cat) +"&s_cat="+ HttpUtility.UrlEncode(s_cat) +"&ss_cat="+ HttpUtility.UrlEncode(ss_cat);
        int i = m_cPI.GetStartRow();
        int end = i + m_cPI.PageSize;
        string sPageIndex = m_cPI.Print();

        Response.Write("<tr><td colspan=3>");
        Response.Write(sPageIndex);
        //************************************* Sort By Qty CH 12.06.08 ****************************//
        Response.Write("</td>");
        Response.Write("<td colspan=1  align=right >");
        Response.Write("Sort By Qty : ");
        Response.Write("<input type=button onclick=\"window.location=('" + Request.ServerVariables["URL"] + "?pr=1&r=" + DateTime.Now.ToOADate() + " ')\" value= ' < '>");
        Response.Write("<input type=button onclick=\"window.location=('" + Request.ServerVariables["URL"] + "?pr=2&r=" + DateTime.Now.ToOADate() + " ')\" value= ' 0 '>");
        Response.Write("<input type=button onclick=\"window.location=('" + Request.ServerVariables["URL"] + "?pr=3&r=" + DateTime.Now.ToOADate() + " ')\" value= ' > '>");
        Response.Write("<input type=button onclick=\"window.location=('" + Request.ServerVariables["URL"] + "?pr=All&r=" + DateTime.Now.ToOADate() + " ')\" value= ' All '>");
        Response.Write("</td>");
        //************************************** END ***********************************************//
        Response.Write("<td align=right colspan=3>");
        if (!DoItemOption())
            return;
        Response.Write("</td></tr>");
        Response.Write("<tr bgcolor=#E3E3E3>");
        /*Response.Write("<th><a href=" + uri + "&sort=p.code");
        if(!m_bDesc)
            Response.Write("&desc=1");
        if(m_branchid != "")
            Response.Write("&b="+ m_branchid +"");
        Response.Write(" title='Click to sort by code' class=o>CODE</a></th>");
        */
        //**new for baisheng for displaying Barcode NO
        Response.Write("<th align=left><a href=" + uri + "&sort=barcode");
        if (!m_bDesc)
            Response.Write("&desc=1");
        if (m_branchid != "")
            Response.Write("&b=" + m_branchid + "");
        Response.Write(" title='Click to sort by Barcode' class=o>BARCODE</a></th>");
        Response.Write("<th align=left>");
        Response.Write("PRODUCT CODE</th>");
        Response.Write("<th align=left>");
        Response.Write("SUPPLIER CODE</th>");

        //************//

        Response.Write("<th align=left><a href=" + uri + "&sort=name");
        if (!m_bDesc)
            Response.Write("&desc=1");
        if (m_branchid != "")
            Response.Write("&b=" + m_branchid + "");
        Response.Write(" title='Click to sort by Description' class=o>DESCRIPTION</a></th>");
        if (Session["branch_support"] != null)
        {
            if (g_bRetailVersion)
            {
                Response.Write("<th align=left>BRANCH</th>");
            }
        }
        Response.Write("<th><a href=" + uri + "&sort=stock");
        if (!m_bDesc)
            Response.Write("&desc=1");
        if (m_branchid != "")
            Response.Write("&b=" + m_branchid + "");
        Response.Write(" title='Click to sort by stock' class=o>STOCK</a></th>");
        Response.Write("<th align=right><a href=" + uri + "&sort=sales");
        if (!m_bDesc)
            Response.Write("&desc=1");
        if (m_branchid != "")
            Response.Write("&b=" + m_branchid + "");
        Response.Write(" title='Click to sort by sales' class=o>SALES</a></th>");
        if (Request.QueryString["ra"] == "code")
            Response.Write("<th>&nbsp;</th>");
        Response.Write("</tr>");

        DataRow dr;
        Boolean alterColor = true;

        for (; i < rows && i < end; i++)
        {
            dr = dst.Tables["stock_qty"].Rows[i];

            string s_product_code = dr["code"].ToString();
            string s_supplier_code = dr["supplier_code"].ToString();
            string s_qty = dr["stock"].ToString();
            string s_description = dr["name"].ToString();
            string barcode = dr["barcode"].ToString();
            string sales = dr["sales"].ToString();
            string branch_name = "";
            if (g_bRetailVersion)
                branch_name = dr["branch_name"].ToString();
            Response.Write("<tr");
            if (alterColor)
                Response.Write(" bgcolor=#EEEEEE");
            Response.Write(">");
            alterColor = !alterColor;
            string code = s_product_code;
            /*Response.Write("<td align=center nowrap>");

            Response.Write("<table border=0><tr>");
            Response.Write("<td><a title='click here to view Sales Ref:' href='salesref.aspx?code=" + code +"' class=o target=_new>");
            Response.Write(code);
            Response.Write("</a> ");
            Response.Write("</td>");

            Response.Write("<td width=50%>");
        //	if(CheckPatent("viewsales"))
            {
                Response.Write("<input type=button title='View Sales History' onclick=\"javascript:viewsales_window=window.open('viewsales.aspx?");
                Response.Write("code=" + code + "','','width=500,height=500');\" value='S' " + Session["button_style"] + ">");
            }
            Response.Write("</td></tr></table>");
            Response.Write("</td>\r\n");
            */
            Response.Write("<td>");
            Response.Write("<table border=0><tr>");
            Response.Write("<td><a title='click here to view Sales Ref:' href='salesref.aspx?code=" + code + "' class=o target=_new>");
            Response.Write("" + barcode + "");
            Response.Write("</a> ");
            Response.Write("</td>");
            Response.Write("<td width=11%>");
            //	Response.Write("<input type=button title='View Sales History' onclick=\"javascript:viewsales_window=window.open('viewsales.aspx?");
            //	Response.Write("code=" + code + "','','width=500,height=500');\" value='S' " + Session["button_style"] + ">");
            Response.Write("</td></tr></table>");
            Response.Write("</td>");
            Response.Write("<td>");
            Response.Write("" + s_product_code + "");
            Response.Write("");
            Response.Write("</td>");
            Response.Write("<td>");
            Response.Write("" + s_supplier_code + "");
            Response.Write("");
            Response.Write("</td>");
            Response.Write("<td width=40%>" + s_description + "</td>");
            if (Session["branch_support"] != null)
            {
                if (g_bRetailVersion)
                    Response.Write("<td>" + branch_name + "</td>");
            }
            Response.Write("<td align=center><font color=");
            if (s_qty == "")
                s_qty = "999999";
            double q = MyDoubleParse(s_qty);
            if (q == 0)
                Response.Write("black");
            else if (q < 0)
                Response.Write("red");
            else if (q > 0)
                Response.Write("green");
            Response.Write(">" + s_qty + "</font></td>");

            Response.Write("<td align=right>" + sales + "</td>");
            if (Request.QueryString["ra"] == "code")
            {
                Response.Write("<td align=right><input type=button name=button value='Add To RA' " + Session["button_style"] + "");
                Response.Write(" onclick=window.location=('techr.aspx?op=" + sOption + "&s=" + sSystem + "&id=" + ra_id + "&sltcode=" + s_product_code + "') ");
                Response.Write(" ></td>");
            }
            Response.Write("</tr>");
        }

        //PrintQtyPageIndex();

        //Response.Write("</tr> "); //<tr><td>Page: <a href=stock.aspx?link='"+i+"'>"+(i+1)+"</a></td></tr>");
        Response.Write("</table>");

    }
    bool GetStockQty()
    {
        string sc = " SELECT p.supplier_code, p.code, CONVERT(varchar(60), p.name) AS name, p.barcode ";
        sc += ", ISNULL((SELECT SUM(s.quantity) FROM sales s JOIN invoice i ON i.invoice_number = s.invoice_number ";
        sc += " WHERE s.code = p.code AND sq.branch_id = i.branch ";
        sc += "), 0) AS sales ";

        //	if(g_bRetailVersion)
        sc += ", ISNULL(sq.qty,0) AS stock , br.name AS branch_name ";
        //	else
        //		sc += ", p.stock ";
        //	sc += " FROM product p ";
        sc += " FROM code_relations p ";
        //	if(g_bRetailVersion)
        {
            sc += " JOIN stock_qty sq ON sq.code = p.code ";
            sc += " JOIN branch br ON br.id = sq.branch_id AND br.activated = 1 ";
        }

        if (ra_code != "" && ra_code != null)
        {
            sc += " JOIN stock_borrow b ON b.code = p.code ";
        }
        sc += " WHERE 1=1 ";
        if (Request.QueryString["cat"] != null && Request.QueryString["cat"] != "all" && Request.QueryString["cat"] != "")
        {
            cat = Request.QueryString["cat"].ToString();
            sc += " AND p.cat = N'" + cat + "' ";
        }
        if (Request.QueryString["s_cat"] != null && Request.QueryString["s_cat"] != "all" && Request.QueryString["s_cat"] != "")
        {
            s_cat = Request.QueryString["s_cat"].ToString();
            sc += " AND p.s_cat = N'" + s_cat + "' ";
        }

        if (Request.QueryString["ss_cat"] != null && Request.QueryString["ss_cat"] != "all" && Request.QueryString["ss_cat"] != "")
        {
            ss_cat = Request.QueryString["ss_cat"].ToString();
            sc += " AND p.ss_cat = N'" + ss_cat + "' ";
        }
        //	if(Request.QueryString["ra"] == "code")
        //	{
        //		sc += " AND p.stock > 0 ";
        //	}
        if (m_branchid != "" && m_branchid != "all")
        {
            //		if(g_bRetailVersion)
            sc += " AND sq.branch_id = " + m_branchid + " ";
        }

        string sortQty = Request.QueryString["pr"]; // Sort By Qty CH 12.06.08
        if (sortQty != null && sortQty != "")
        {
            if (sortQty == "1")
                sc += " AND sq.qty < 0";
            if (sortQty == "2")
                sc += " AND sq.qty = 0";
            if (sortQty == "3")
                sc += " AND sq.qty > 0";
        }

        sc += " ORDER BY ";

        if (m_sort != "")
        {
            //sc += " ORDER BY " + Request.QueryString["sort"];
            //		if(Request.QueryString["sort"] == "barcode" )
            //			sc += " CAST( ";
            sc += Request.QueryString["sort"];
            //		if(Request.QueryString["sort"] == "barcode" )
            //			sc += " AS float(16)) ";
            if (m_bDesc)
                sc += " DESC ";
        }
        else if (Request.QueryString["cat"] == "all" || (Request.QueryString["cat"] != null && Request.QueryString["s_cat"] == "all")
            || (Request.QueryString["cat"] != null && Request.QueryString["cat"] != null && Request.QueryString["ss_cat"] == "all"))
        {
            sc += " p.cat, p.s_cat, p.ss_cat, p.brand, p.name, p.code ";
        }
        else
        {
            //		sc += " CAST(p.barcode AS float(16)) ";
            sc += " p.barcode ";
        }
        //DEBUG("sc=", sc);
        try
        {
            myAdapter = new SqlDataAdapter(sc, myConnection);
            myAdapter.Fill(dst, "stock_qty");

        }
        catch (Exception e)
        {
            ShowExp(sc, e);
            return false;
        }
        if (ra_code != "" && ra_code != null)
        {
            if (dst.Tables["stock_qty"].Rows.Count < 0)
            {
                Response.Write("<script Language=javascript");
                Response.Write(">\r\n");
                Response.Write("window.alert('Sorry NO Item to Replace for RMA, Please Go to Borrow Item from Stock!!')\r\n");
                Response.Write("</script");
                Response.Write(">\r\n ");
                Response.Write("<meta http-equiv=\"refresh\" content=\"0; URL=stk_borrow.aspx\">");
                return false;
            }
        }
        return true;
    }
    bool DoDeleteProduct(string s_id)
    {
        return false; //no delete here DW.

        string sc = " DELETE FROM stock ";
        sc += " WHERE id = " + s_id + "";

        try
        {
            myCommand = new SqlCommand(sc);
            myCommand.Connection = myConnection;
            myConnection.Open();
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
    bool UpdateProduct()
    {
        m_snQuery = Request.QueryString["i"].ToString();

        string sc = "set DATEFORMAT dmy ";
        sc += " UPDATE stock  ";
        sc += " SET sn = '" + m_sn + "', product_code = '" + m_product_code + "', branch_id = " + m_branch_id + ", ";
        sc += " cost = " + m_cost + ", purchase_date = '" + m_purchase_date + "', po_number = '" + m_po_number + "', ";
        sc += " supplier = '" + m_supplier + "', supplier_code = '" + m_supplier_code + "', status = " + m_status + "";
        sc += " WHERE id = " + m_snQuery + "";
        try
        {
            myCommand = new SqlCommand(sc);
            myCommand.Connection = myConnection;
            myConnection.Open();
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
    void BindSearchSNProduct()
    {
        Response.Write("<table width=100% cellspacing=2 cellpadding=3 border=0 bordercolor=#EEEEEE bgcolor=white");
        Response.Write(" style=\"font-family:Verdana;font-size:8pt;border-width:0px;border-style:Solid;border-collapse:collapse;fixed\">");
        Response.Write("<tr style=\"color:white;background-color:#666696;font-weight:bold;\">");
        Response.Write("<tr><td colspan=4><b>Search Found:</b></td></tr>");
        Response.Write("<tr bgcolor=#E3E3E3>");
        Response.Write("<th>Product Code</th> ");
        Response.Write("<th>Supplier Code</th> ");
        Response.Write("<th align=left>Description</th> ");
        Response.Write("<th align=left>Branch</th> ");
        Response.Write("<th align=left>Quantity</th> ");
        Response.Write("<th align=left>Price</th> ");
        Response.Write("</tr>");
        for (int i = 0; i < dst.Tables["snQty"].Rows.Count; i++)
        {
            DataRow dr = dst.Tables["snQty"].Rows[i];
            string s_product_code = dr["code"].ToString();
            string s_supplier_code = dr["supplier_code"].ToString();
            string s_barcode = dr["barcode"].ToString();
            string s_quantity = dr["qty"].ToString();
            string s_prod_desc = dr["name"].ToString();
            string s_branch_name = "";
            if (g_bRetailVersion)
                s_branch_name = dr["branch_name"].ToString();
            string price = dr["price"].ToString();
            double dPrice = 0;
            if (price != null && price != "")
                dPrice = double.Parse(price);
            else
                dPrice = 0;
            Response.Write("<tr>");
            //Response.Write("<td align=center>"+s_product_code+"</td>");
            Response.Write("<th><a title='click here to view the sales reference' href='salesref.aspx?code=" + s_product_code + "' target=_blank class=o>" + s_barcode + "</a>");
            Response.Write("<input type=button title='View Sales History' onclick=\"javascript:viewsales_window=window.open('viewsales.aspx?");
            Response.Write("code=" + s_product_code + "','','width=500,height=500');\" value='S' " + Session["button_style"] + ">");
            Response.Write("<th>" + s_supplier_code + "</th>");
            Response.Write("<td>" + s_prod_desc + "</td>");
            if (g_bRetailVersion)
                Response.Write("<td>" + s_branch_name + "</td>");
            if (int.Parse(s_quantity) == 0)
                Response.Write("<td><b>" + s_quantity + "</b></td>");
            else if (int.Parse(s_quantity) > 0)
                Response.Write("<td><b><font color=Green>" + s_quantity + "</font></b></td>");
            else if (int.Parse(s_quantity) < 0)
                Response.Write("<td><b><font color=Red>" + s_quantity + "</font></b></td>");
            Response.Write("<td>" + dPrice.ToString("c") + "</td>");
            Response.Write("</tr>");
        }
        Response.Write("</table>");
    }

    bool SearchSNQty(string sSearchSN)
    {
        string sc = "";
        sSearchSN = msgEncode(sSearchSN);
        //DEBUG("g_bRetailVersion =", g_bRetailVersion.ToString());
        //DEBUG(" sSEarch = ", sSearchSN);
        /*if(g_bRetailVersion)
        {
            sc = " SELECT sq.id, sq.code, sq.qty, CONVERT(varchar(50),pd.name) AS name, pd.price ";
            sc += " FROM stock_qty sq INNER JOIN product pd ON sq.code = pd.code";
            sc += " WHERE (sq.code = ";
            sc += " (SELECT product_code ";
            sc += " FROM stock ";
            sc += " WHERE (sn = '"+sSearchSN+"'))) "; //OR sn like '%"+sSearchSN+"%')) ";
        }
        else
        */
        //{
        sc = " SELECT DISTINCT st.sn, pi.code, c.supplier_code,  CONVERT(varchar(60), pi.name) AS name ";
        if (g_bRetailVersion)
            sc += ", ISNULL(sq.qty,0) AS qty, br.name AS branch_name ";
        else
            sc += ", pd.stock AS qty ";
        sc += ", c.price1 AS price ";
        //sc += " FROM serial_trace st LEFT OUTER JOIN ";
        sc += " FROM stock st ";
        sc += " JOIN code_relations c ON c.code = st.product_code ";
        sc += " JOIN purchase p ON p.id = st.purchase_order_id ";
        sc += " JOIN purchase_item pi ON st.purchase_order_id = pi.id AND pi.id = p.id ";
        sc += " JOIN product pd ON pd.code = pi.code AND st.product_code = pi.code AND st.product_code = pd.code ";
        //		sc += " JOIN code_relations c ON c.code = pd.code AND c.code = pi.code AND c.code = st.product_code ";
        //		if(g_bRetailVersion)
        {
            sc += " JOIN stock_qty sq ON sq.code = pd.code AND pi.code = sq.code ";
            sc += " JOIN branch br ON br.id = sq.branch_id AND br.activated = 1 ";
        }
        sc += " WHERE (st.sn = '" + sSearchSN + "' ) ";

        if (m_branchid != "" && m_branchid != "all")
        {
            if (g_bRetailVersion)
                sc += " AND sq.branch_id = " + m_branchid + " ";
        }

        sc += " ORDER BY st.sn DESC ";
        //}
        //DEBUG("sc = ", sc);	
        try
        {
            myAdapter = new SqlDataAdapter(sc, myConnection);
            int i = myAdapter.Fill(dst, "snQty");
            //DEBUG( "i = ", i );

        }
        catch (Exception e)
        {
            if (e.ToString().IndexOf("Invalid column name 'activated'") >= 0)
            {
                sc = @"
				alter table branch ADD activated [bit] not null default(1) 
				";
                try
                {
                    myCommand = new SqlCommand(sc);
                    myCommand.Connection = myConnection;
                    myCommand.Connection.Open();
                    myCommand.ExecuteNonQuery();
                    myCommand.Connection.Close();
                }
                catch (Exception e2)
                {
                    ShowExp(sc, e2);
                }
            }
            ShowExp(sc, e);
            return false;
        }


        return true;
    }

    void GetSearch()
    {
        Response.Write("<script language=javascript>");
        Response.Write("<!-- hide from old browser");
        string s = @"
		function checkform()
		{
			
			if(document.frmSearchProduct.txtSearch.value == '' && document.frmSearchProduct.txtSearchSN.value == ''){

				window.alert('Please Input Product Code or Serial number for search!! ');
				document.frmSearchProduct.txtSearch.focus();
				//document.frmSearchProduct.cmdUpdate.disabled=false;
				return false;
			}

			return true;			
		}
		function queryitem()
		{
				if(document.frmSearchProduct.txtQuery.value !='')
					document.frmSearchProduct.cmdQuery.disabled=false;
				else
					document.frmSearchProduct.cmdQuery.disabled=true;
		}
		
		
		function IsNumberic(sText)
		{
		   var ValidChars = '0123456789';
		   var IsNumber=true;
		   var Char;
		   for (i = 0; i < sText.length && IsNumber == true; i++) 
		   { 
			  Char = sText.charAt(i); 
			  if (ValidChars.indexOf(Char) == -1) 
						 IsNumber = false;
		   }
		   return IsNumber;
   		 }
	";
        Response.Write("//-->");
        Response.Write(s);
        Response.Write("</script");
        Response.Write(">");


        Response.Write("<table border=0 align=center width='" + tableWidth + "'");
        Response.Write(" style=\"font-family:Verdana;font-size:8pt;border-width:1px;border-style:Solid;border-collapse:collapse;fixed\">");
        Response.Write("<tr><td colspan=2><br></td></tr>");
        Response.Write("<tr style=\"color:white;background-color:#666696;font-weight:bold;\">");
        Response.Write("<tr><td width=15%>Search by BarCode/Code/MPN:</td><td><input type=text name=txtSearch value='" + Request.Form["txtSearch"] + "'></td>");

        Response.Write("<tr><td>Search by Serial Number:</td>");
        Response.Write("<td><input type=text name=txtSearchSN value=" + Request.Form["txtSearchSN"] + ">");


        Response.Write("<input type=submit name=cmdUpdate value='Search Product' " + Session["button_style"] + " ");
        Response.Write(" onclick='return checkform();'></td></tr>");

        //branch option
        Response.Write("<tr>");

        if (Session["branch_support"] != null)
        {
            Response.Write("<td><b>Branch :</b></td><td align=left>");
            PrintBranchNameOptionsWithOnChange();
            Response.Write(" <input type=button  value='Print Product List' " + Session["button_style"] + " onclick=\"window.location=('stocktake.aspx?cat=" + HttpUtility.UrlEncode(cat) + "&s_cat=" + HttpUtility.UrlEncode(s_cat) + "&ss_cat=" + HttpUtility.UrlEncode(ss_cat) + "&pr=3&br='+document.frmSearchProduct.branch.selectedIndex)\"> ");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        Response.Write("\r\n<script");
        Response.Write(">\r\n document.frmSearchProduct.txtSearch.focus()\r\n</script");
        Response.Write(">\r\n ");

        //Response.Write("<tr><td>&nbsp;</td></tr>");
        Response.Write("</table>");
        Response.Write("</form>");
    }
    void BindSearchProduct()
    {

        Response.Write("<table width=100% cellspacing=2 cellpadding=3 border=0 bordercolor=#EEEEEE bgcolor=white");
        Response.Write(" style=\"font-family:Verdana;font-size:8pt;border-width:0px;border-style:Solid;border-collapse:collapse;fixed\">");
        Response.Write("<tr style=\"color:white;background-color:#666696;font-weight:bold;\">");
        Response.Write("<tr><td colspan=7><b>Search Found:</b></td></tr>");
        Response.Write("<tr bgcolor=#E3E3E3>");
        Response.Write("<th>Product Barcode</th> ");
        Response.Write("<th>Supplier Code</th> ");
        Response.Write("<th align=left>Description</th> ");
        Response.Write("<th align=left>Branch</th> ");
        Response.Write("<th align=left>Quantity</th> ");
        Response.Write("<th align=left>Price</th> ");

        string s = "";
        DataRow dr;
        Boolean alterColor = true;
        int startPage = (m_page1 - 1) * m_nPageSize1;
        for (int i = startPage; i < dst.Tables["searchQty"].Rows.Count; i++)
        {
            if (i - startPage >= m_nPageSize1)
                break;
            dr = dst.Tables["searchQty"].Rows[i];
            alterColor = !alterColor;
            if (!DrawRowSearch(dr, i, alterColor))
                break;
        }

        //PrintPageIndexSearch();

        Response.Write("</table>");
        Response.Write("<hr size=1 color=black>");

    }
    private bool PrintBranchNameOptionsWithOnChange()   //Herman: 21/03/03
    {
        DataSet dsBranch = new DataSet();
        string sBranchID = "1";
        int rows = 0;
        /*	if(Request.QueryString["b"] != null && Request.QueryString["b"] != "")
            {
                sBranchID = Request.QueryString["b"];
                if(sBranchID != "all")
                    Session["branch_id"] = MyIntParse(sBranchID); //Session["branch_id"] is integer
            }
            else if(Session["branch_id"] != null)
            {
                sBranchID = Session["branch_id"].ToString();
            }
        */
        //do search
        string sc = "SELECT id, name FROM branch WHERE activated = 1 ";
        if (int.Parse(Session["employee_access_level"].ToString()) < 10)
        {
            sc += " And id = " + Session["branch_id"] + " ";
            //sb.Append(" order by id ";
        }
        sc += " ORDER BY id";
        try
        {
            SqlDataAdapter myCommand = new SqlDataAdapter(sc, myConnection);
            rows = myCommand.Fill(dsBranch, "branch");
        }
        catch (Exception e)
        {
            if (e.ToString().IndexOf("Invalid column name 'activated'") >= 0)
            {
                sc = @"
				alter table branch ADD activated [bit] not null default(1) 
				";
                try
                {
                    myCommand = new SqlCommand(sc);
                    myCommand.Connection = myConnection;
                    myCommand.Connection.Open();
                    myCommand.ExecuteNonQuery();
                    myCommand.Connection.Close();
                }
                catch (Exception e2)
                {
                    ShowExp(sc, e2);
                }
            }
            ShowExp(sc, e);
            return false;
        }

        Response.Write("<select name=branch");
        Response.Write(" onchange=\"window.location=('" + Request.ServerVariables["URL"]);
        Response.Write("?b=' + this.options[this.selectedIndex].value)\"");
        Response.Write(">");

        if (int.Parse(Session["employee_access_level"].ToString()) >= 10)
        {

            Response.Write("<option value='all'");
            if (m_ballbranchs)
                Response.Write("selected");

            Response.Write("> All Branches</option>");
        }
        else
        {

        }
        for (int i = 0; i < rows; i++)
        {
            string bname = dsBranch.Tables["branch"].Rows[i]["name"].ToString();
            int bid = int.Parse(dsBranch.Tables["branch"].Rows[i]["id"].ToString());
            Response.Write("<option value='" + bid + "' ");
            if (IsInteger(m_branchid))
            {
                if (bid == int.Parse(m_branchid))
                    Response.Write("selected");
            }
            Response.Write(">" + bname + "</option>");
        }

        if (rows == 0)
            Response.Write("<option value=1>Branch 1</option>");
        Response.Write("</select>");
        return true;
    }
    bool SearchProduct(string sSearch)
    {

        /*	string sc = "SELECT * FROM stock s INNER JOIN enum e";
            sc += " ON e.id = s.status ";
            sc += " WHERE e.class = 'stock_status' ";
            sc += " AND e.name = 'In Stock' ";
            if(sSearch != "" || sSearch != null)
                sc += " AND product_code = '"+sSearch+"'";

        */
        bool bIsInt = false;
        try
        {
            string stemp = int.Parse(sSearch).ToString();
            bIsInt = true;
        }
        catch (Exception e)
        {

        }

        string sc = " SELECT p.supplier + p.supplier_code AS ID, p.supplier_code, p.code, c.barcode ";
        if (g_bRetailVersion)
            sc += ", ISNULL(sq.qty,0) AS qty , br.name AS branch_name ";
        else
            sc += " , p.stock AS qty ";
        sc += " , CONVERT(varchar(50),p.name) AS name, c.price1 AS price ";
        sc += " FROM product p JOIN code_relations c ON c.code = p.code ";
        //	if(g_bRetailVersion)
        {
            sc += " JOIN stock_qty sq ON sq.code = p.code ";
            sc += " JOIN branch br ON br.id = sq.branch_id AND br.activated = 1 ";
        }

        if (sSearch != "")
        {
      
            if (bIsInt)
            {
                sc += " WHERE (c.barcode = '" + sSearch + "' OR c.supplier_code = '" + sSearch + "' ";
                if (sSearch.Length < 9)
                    sc += " OR c.code = '" + sSearch + "' ";
                sc += " ) ";
            }
            else
                sc += " WHERE (c.supplier_code = '" + sSearch + "' OR c.barcode = '" + sSearch + "') ";
        }

        if (m_branchid != "" && m_branchid != "all")
        {
            if (g_bRetailVersion)
                sc += " AND sq.branch_id = " + m_branchid + " ";
        }
  
        try
        {
            myAdapter = new SqlDataAdapter(sc, myConnection);
        
            m_RowsReturn = myAdapter.Fill(dst, "searchQty");

 
        }
        catch (Exception e)
        {
            if (e.ToString().IndexOf("Invalid column name 'activated'") >= 0)
            {
                sc = @"
				alter table branch ADD activated [bit] not null default(1) 
				";
                try
                {
                    myCommand = new SqlCommand(sc);
                    myCommand.Connection = myConnection;
                    myCommand.Connection.Open();
                    myCommand.ExecuteNonQuery();
                    myCommand.Connection.Close();
                }
                catch (Exception e2)
                {
                    ShowExp(sc, e2);
                }
            }
            ShowExp(sc, e);
            return false;
        }


        return true;

    }

}
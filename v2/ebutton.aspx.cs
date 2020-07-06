using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class v2_ebutton : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    string m_type = "";     //query type &t=
    string m_action = "";   //query action &a=
    string m_cmd = "";      //post button value, name=cmd
    string m_id = "";
    string m_sid = "";
    string m_kw = "";
    bool m_bEn = false;
    bool m_indi = false;
    string s_indi = "";
    SqlCommand myCommand1 = new SqlCommand();
    SqlConnection myConnection = Common.myConnection;
   
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    public void PrintPage()
    {
        if (Session["card_id"] != null && Session["card_id"] != "")
            m_bEn = GetLanguage(Session["card_id"].ToString());
        m_type = Common.g("t");
        m_action = Common.g("a");
        m_cmd = Common.p("cmd");
        m_indi = Common.MyBooleanParse(Common.p("is_indivisual"));
        s_indi = Common.g("indi");
        //DEBUG("cmd ", m_cmd);
        m_id = Common.MyIntParse(Common.g("id")).ToString();
        m_sid = Common.MyIntParse(Common.g("sid")).ToString();
        if (Request.Form["kw"] != null)
            m_kw =Common.p("kw");
        else
            m_kw = Common.g("kw");
        //DEBUG("11","22");
        switch (m_cmd)
        {
            case "Save":
                if (m_type == "es")
                {
                    if (m_kw == "")
                    {
                        if (DoSaveButtonName())
                        {
                            Response.Write("<meta http-equiv=\"refresh\" content=\"0; URL=?t=" + m_type + "&id=" + m_id + "&sid=" + m_sid + "\">");
                        }
                        return;
                    }
                }
                /*else
                {
                    if(DoSaveCategory())
                    {
                        Response.Write("<meta http-equiv=\"refresh\" content=\"0; URL=?t=" + m_type + "&indi="+m_indi+"&id=" + m_id + "\">");
                        return;
                    }
                }*/
                break;
            case "Add Checked":
                if (DoBatchAdd())
                {
                    Response.Write("<meta http-equiv=\"refresh\" content=\"0; URL=?t=e&id=" + m_id + "\">");
                    return;
                }
                break;
            case "refresh":
                Response.Write("<meta http-equiv=\"refresh\" content=\"0; URL=?t=" + m_type + "&a=" + m_action + "\">");
                break;
            case "Update":
                //DEBUG("22","22");
                DoUpdateCatAll();
                break;
        }


        Docat();
        if (m_type == "e")
        {
            PrintEditForm();
        }
        else if (m_type == "es")
        {
            if (m_action == "assign")
            {
                if (DoAssignItem())
                {
                    PrintEditItemForm();
                    Response.Write("<meta http-equiv=\"refresh\" content=\"0; URL=?t=e&id=" + m_id + "\">");
                }
                return;
            }
            else if (m_action == "del")
            {
                DoDeleteItem();
                //PrintEditItemForm();
                //Response.Write("<meta http-equiv=\"refresh\" content=\"0; URL=?t=e&id=" + m_id + "\">");
            }
            PrintEditItemForm();
            return;
        }
        else if (m_type == "ba")
        {
            PrintBatchAddForm();
        }
        else if (m_type == "u")
        {
            DoUpdateCatAll();
            PrintMainForm();
        }
        else
        {
            PrintMainForm();
        }
    }
    string getCat(string id)
    {
        string sc = "";
        int rows = 0;
        if (ds.Tables["getCat"] != null)
            ds.Tables["getCat"].Clear();
        sc = " SELECT top 1 cat FROM cat where id = '" + id + "'";
        try
        {
            SqlDataAdapter myCommand = new SqlDataAdapter(sc, myConnection);
            if (myCommand.Fill(ds, "getCat") <= 0)
            {
                return "";
            }
        }
        catch (Exception e)
        {
            Common.ShowExp(sc, e);
            return "";
        }
        string cat = ds.Tables["getCat"].Rows[0]["cat"].ToString();
        return cat;
    }
    bool DoUpdateCatAll()
    {
        if (ds.Tables["categorylist"] != null)
            ds.Tables["categorylist"].Clear();
        //string sc = " SELECT id, name, name_en, is_indivisual FROM button ORDER BY id ";
        string sc = " UPDATE cat set cat= '' WHERE cat collate SQL_Latin1_General_CP1_CI_AS NOT IN (SELECT cat FROM catalog) ";
        sc += " SELECT DISTINCT  ca.seq, RTRIM(LTRIM(ca.cat)) AS cat,isnull(c.id,0) as id FROM catalog ca";
        sc += " left outer join cat c on ca.cat = c.cat collate SQL_Latin1_General_CP1_CI_AS ";
        sc += " WHERE ca.cat <> 'Brands' AND ca.cat <> 'ServiceItem' ";
        sc += " ORDER by cat, seq, id ";
        //DEBUG("sc",sc);
        try
        {
            SqlDataAdapter myCommand = new SqlDataAdapter(sc, myConnection);
            if (myCommand.Fill(ds, "categorylist") <= 0)
            {
                return false;
            }
        }
        catch (Exception e)
        {
            Common.ShowExp(sc, e);
            return false;
        }

        int j = 0;
        for (; j < ds.Tables["categorylist"].Rows.Count; j++)
        {
            DataRow dr = ds.Tables["categorylist"].Rows[j];
            string id = Request.Form["id" + j.ToString()];
            string name = (Request.Form["name" + j.ToString()]);
            string old_id = dr["id"].ToString();
            //DEBUG("name",name);
            //DEBUG("old_id",old_id);
            //return false;
            DoUpdateOneCat(id, name, old_id);

        }
        Common.SetSiteSettings("sync_menubutton", "1");
        return true;
    }
    bool DoUpdateOneCat(string id, string cat, string old_id)
    {
        if (Common.MyIntParse(id) > 18)
            return false;

        string sc1 = "";
        if (id == "" || cat == "" || id == "0")
            sc1 = " UPDATE cat SET cat = N'' WHERE cat =  N'" + cat + "'";
        else
            sc1 = " UPDATE cat SET cat = N'" + cat + "' WHERE id = '" + id + "'";
        if (cat != "" && id != "" && id != "0" && old_id != "" && id != old_id)
            //if(old_id != "" && old_id != "0" && id != "" && id != "0" && cat != "")
            sc1 += " UPDATE cat SET cat = N'' WHERE id = '" + old_id + "'";

        //DEBUG("sc1",sc1);
        //return false;
        try
        {
            myCommand1 = new SqlCommand(sc1);
            myCommand1.Connection = myConnection;
            myCommand1.Connection.Open();
            myCommand1.ExecuteNonQuery();
            myCommand1.Connection.Close();
        }
        catch (Exception e)
        {
            Common.ShowExp(sc1, e);
            return false;
        }
        return true;
    }


    bool Docat()
    {
        string sc1 = " If not exists (select id from cat)";
        sc1 += "insert into cat(cat)select ";
        sc1 += " top 18 ''  from catalog where 1=1 ";
        try
        {
            myCommand1 = new SqlCommand(sc1);
            myCommand1.Connection = myConnection;
            myCommand1.Connection.Open();
            myCommand1.ExecuteNonQuery();
            myCommand1.Connection.Close();
        }
        catch (Exception e)
        {
            Common.ShowExp(sc1, e);
            return false;
        }
        return true;
    }

    bool PrintMainForm()
    {
        Response.Write("<br><center><h4>SETUP TOUCH SCREEN</h4>");
        Response.Write("<form name=f action=?t=u method=post>");
        Response.Write("<table>");
        if (ds.Tables["categorylist"] != null)
            ds.Tables["categorylist"].Clear();
        //string sc = " SELECT id, name, name_en, is_indivisual FROM button ORDER BY id ";
        string sc = "  SELECT DISTINCT  ca.seq, RTRIM(LTRIM(ca.cat)) AS cat,isnull(c.id,0) as id FROM catalog ca";
        sc += " left outer join cat c on ca.cat = c.cat collate SQL_Latin1_General_CP1_CI_AS ";
        sc += " WHERE ca.cat <> 'Brands' AND ca.cat <> 'ServiceItem' ";
        sc += " ORDER by cat, seq, id ";
        //DEBUG("sc",sc);
        try
        {
            SqlDataAdapter myCommand = new SqlDataAdapter(sc, myConnection);
            if (myCommand.Fill(ds, "categorylist") <= 0)
            {
                return false;
            }
        }
        catch (Exception e)
        {
            Common.ShowExp(sc, e);
            return false;
        }
        string id = "";
        string id_old = "";
        string name = "";
        string name_en = "";
        bool is_indivisual = false;
        int i = 0;
        int j = 0;
        int n = 6;
        int m = 6;
        string style = "style='width:100px;height:55px;'";
        string style1 = "style='width:50px;height:24px;'";
        string onclick = "onclick=\"window.location='?t=e&id=@@id';\"";
        /*******/
        Response.Write("<tr><td><table><tr>");
        for (; j < ds.Tables["categorylist"].Rows.Count; j++)
        {
            id = ds.Tables["categorylist"].Rows[j]["id"].ToString();
            id_old = ds.Tables["categorylist"].Rows[j]["id"].ToString();
            name = ds.Tables["categorylist"].Rows[j]["cat"].ToString();
            if (id == "0")
                id = "";
            if (id_old == "0")
                id_old = "";
            Response.Write("<td>");
            Response.Write("<table><tr>");
            //Response.Write("<input type=hidden name=name"+j+" value=\""+name+"\">");
            //Response.Write(name+"&nbsp;<input type=text name=id"+j+" width=5 value="+id+"> ");
            Response.Write("<td><input type=hidden name=name" + j + " value=\"" + name + "\">" + name + "</td>");
            Response.Write("</tr><tr>");
            Response.Write("<td><input type=hidden name=old_id" + j + " value=\"" + id_old + "\"><input type=text name=id" + j + " value=" + id + "></td>");
            Response.Write("</tr></table>");
            Response.Write("</td>");
            m--;
            //DEBUG("old_idd",Request.Form["old_id"+j]);
            //DEBUG("name",name);
            if (m <= 0)
            {
                Response.Write("</tr><tr>");
                m = 6;
            }
        }
        Response.Write("</table></td></tr>");
        /********/
        Response.Write("<tr><td><table><tr>");

        if (ds.Tables["category"] != null)
            ds.Tables["category"].Clear();
        //string sc = " SELECT id, name, name_en, is_indivisual FROM button ORDER BY id ";
        sc = "  select * from cat ";
        //DEBUG("sc",sc);
        try
        {
            SqlDataAdapter myCommand = new SqlDataAdapter(sc, myConnection);
            if (myCommand.Fill(ds, "category") <= 0)
            {
                return false;
            }
        }
        catch (Exception e)
        {
            Common.ShowExp(sc, e);
            return false;
        }

        for (; i < 18; i++)
        {
            id = ds.Tables["category"].Rows[i]["id"].ToString();
            name = ds.Tables["category"].Rows[i]["cat"].ToString();
            if (name == "" || name == null)
                name = id;
            name_en = ""; //ds.Tables["category"].Rows[i]["name_en"].ToString();
            is_indivisual = false; //MyBooleanParse(ds.Tables["category"].Rows[i]["is_indivisual"].ToString());
            m_indi = is_indivisual;
            name = name.Replace(" ", "&#x00A;");
            Response.Write("<td><input type=button " + style + " name=" + id + " ");
            onclick = onclick.Replace("@@indi", is_indivisual.ToString());
            Response.Write(" " + onclick.Replace("@@id", id) + " value='" + name + "'>&nbsp;&nbsp;</td>");
            n--;
            if (n <= 0)
            {
                Response.Write("</tr><tr>");
                n = 6;
            }
        }
        Response.Write("</table></td></tr>");
        //Response.Write("<tr><td align=right colspan=6><input type=button value='Save'>&nbsp;&nbsp;&nbsp;</td></tr>");
        Response.Write("<tr><td align=right colspan=6>");
        //Response.Write("<input type=submit name=update title= 'Update All' value='"+Lang("Update")+"' " + Session["button_style"]);
        Response.Write("<input type=submit name=cmd value='Update' class=b>");
        //Response.Write("<input type=submit name=cmd value='Save' class=b>");
        Response.Write("&nbsp;&nbsp;&nbsp;&nbsp;</td></tr>");
        Response.Write("</table>");


        //    Response.Write("<tr><td><table><tr>");
        //    n = 8;
        //    i= 17;	
        //    i++;
        //    for(; i<ds.Tables["category"].Rows.Count; i++)
        //    {
        //        id = ds.Tables["category"].Rows[i]["id"].ToString();
        //        name = ds.Tables["category"].Rows[i]["name"].ToString();
        //        name_en = ds.Tables["category"].Rows[i]["name_en"].ToString();
        ////      is_indivisual = bool.(ds.Tables["buttons"].Rows[i]["is_indivisual"].ToString());
        //        name = name.Replace(" ", "&#x00A;");
        //        if(m_bEn)
        //            name = name_en;
        //        if(name == "")
        //            name = id;
        //        Response.Write("<td><input type=button " + style + " name=" + id + " ");
        //        Response.Write(" " + onclick.Replace("@@id", id) + " value='" + name + "'></td>");
        //        n--;
        //        if(n <= 0)
        //        {
        //            Response.Write("</tr><tr>");
        //            n = 8;
        //        }
        //        if(i > 50)
        //            break;
        //    }

        //    Response.Write("</tr></table></td><td><table>");
        //Response.Write("</table></td></tr>");
        Response.Write("</form>");
        return true;
    }
    bool PrintEditForm()
    {
        int nRows = 0;
        //string sc = " SELECT b.name, b.name_en, b.is_indivisual, i.id AS s_id, i.code, i.name AS s_name, i.name_en AS s_name_en, i.location ";
        //sc += " FROM button b ";
        //sc += " LEFT OUTER JOIN button_item i ON i.button_id = b.id ";
        //sc += " WHERE b.id = " + m_id;

        //string sc = "  SELECT DISTINCT ROW_NUMBER() OVER(ORDER BY c.name_cn) AS Row#, c.code, c.name, c.name_cn,c.barcode, c.price1 as price, c.cat, c.s_cat, c.ss_cat , cb.price1 as cbPrice, ISNULL(cb.special, '0') as is_special, cb.special_price  FROM code_relations c ";
        //sc += " LEFT OUTER JOIN code_branch cb ON cb.code=c.code  ";
        //sc += " WHERE c.skip = 0";
        ////sc += " and cat = N'" + category + "'";
        //sc += " and cat = N'"+getCat(m_id)+"'";
        //sc += " and cb.branch_id = '1'";
        //sc += " AND c.code not in (select code from item where 1=1 and cat_id = " + m_id + " and code is not null and code <> '')";
        //sc += " GROUP BY c.s_cat, c.cat, c.code, c.name, c.name_cn, c.barcode, c.price1, c.ss_cat, cb.price1, cb.special, cb.special_price ";
        //sc += " ORDER by c.name_cn ";

        string sc = "";
        sc = " SELECT i.*, ca.cat, c.name, c.name_cn,c.barcode from item i ";
        sc += " join cat ca on i.cat_id = ca.id ";
        sc += " left outer join code_relations c ON i.code = c.code ";
        //sc += " left outer join code_branch cb on cb.code = c.code ";
        sc += " WHERE 1=1 ";
        sc += " AND ca.cat = N'" + getCat(m_id) + "'";
        sc += " AND i.cat_id = '" + m_id + "'";
        sc += " ORDER by i.seq ";
        //DEBUG("sc",sc);
        try
        {
            SqlDataAdapter myCommand = new SqlDataAdapter(sc, myConnection);
            nRows = myCommand.Fill(ds, "buttons");
        }
        catch (Exception e)
        {
            Common.ShowExp(sc, e);
            return false;
        }

        string name = "";
        string name_en = "";
        string singlebarcode = "";
        string is_indivisual = "";

        string style = "style='width:80px;height:40px;WORD-BREAK:BREAK-ALL;'";
        string onclick = "onclick=\"window.open('ebutton.aspx?t=es&id=" + m_id;
        onclick += "&sid=@@sid');\"";

        Response.Write("<form name=f action=?t=e&indi=" + is_indivisual + "&id=" + m_id + " method=post>");
        Response.Write("<center><font size=+1><b>Edit Category # &nbsp;&nbsp;<font color=red>" + getCat(m_id) + "</font></b></font><br>");

        //Response.Write("<b>Button Name  : </b><input type=text name=name_en value='" + name_en + "' onclick='select()'>");
        //Response.Write("&nbsp;&nbsp;<b>Button Name (Other) : </b><input type=text name=name value='" + name + "' onclick='select()'>");
        //Response.Write("&nbsp;&nbsp;<b>Single Button :</b><input type=checkbox name=is_indivisual ");
        //if(is_indivisual == "1" || is_indivisual == "True")
        //    Response.Write(" checked");
        //else
        //    Response.Write(" unchecked ");
        //Response.Write(">");

        //Response.Write("<input type=submit name=cmd value='Save' class=b><br>");
        //if(is_indivisual == "1" || is_indivisual == "True")
        //{
        //    PrintEditItemFormSingleButton();
        //}
        //else
        {
            /*******button Edit start********/
            Response.Write("<table cellspacing=1 cellpadding=2 border=0 class=t>");
            Response.Write("<tr>");
            Response.Write("<td colspan=8><b>Button List</b> &nbsp; ");
            Response.Write("</td></tr>");
            Response.Write("<tr valign=top>");
            int n = 6;
            int i = 1;
            for (; i < 43; i++)
            {
                string s_id = "";
                string s_name = "";
                string s_name_en = "";
                string cat_id = "";
                string seq = "";
                string code = "";
                string location = "";
                for (int j = 0; j < nRows; j++)
                {
                    DataRow dr = ds.Tables["buttons"].Rows[j];
                    location = ""; //dr["location"].ToString();
                    cat_id = dr["cat_id"].ToString();
                    seq = dr["seq"].ToString();
                    if (i == Common.MyIntParse(seq))
                    {
                        //s_id = dr["s_id"].ToString();
                        s_name = dr["name"].ToString();
                        s_name_en = dr["name_cn"].ToString();
                        if (s_name.Length > 5)
                            s_name = s_name.Substring(0, 5) + "&#x00A;" + s_name.Substring(5, s_name.Length - 5);
                    }
                    else
                    {

                    }
                }
                if (m_bEn)
                    s_name = s_name_en;
                if (s_name == "")
                {
                    s_name = s_id;
                }
                s_id = i.ToString();
                Response.Write("<td><input type=button " + style + " " + onclick.Replace("@@sid", s_id));
                Response.Write(" name='" + s_id + "' value='" + s_name_en + "'></td>");
                n--;
                if (n <= 0)
                {
                    Response.Write("</tr><tr>");
                    n = 6;
                }
            }
            Response.Write("</tr></table>");
        }
        /*******button edit end******************/
        Response.Write("</form>");
        Response.Write("<a href=? class=o>Back to Top Layout</a>");
        return true;
    }
    bool PrintBatchAddForm()
    {
        int nRows = 0;
        string sc = " SELECT TOP 300 c.cat, c.s_cat, c.code, c.supplier_code ";
        sc += ", c.name, c.name_cn ";
        sc += " FROM code_relations c ";
        //	sc += " LEFT OUTER JOIN button_item i ON i.code = c.code ";
        sc += " WHERE 1 = 1 ";
        if (m_kw != "")
        {
            sc += " AND c.name LIKE N'%" + Common.EncodeQuote(m_kw) + "%' COLLATE SQL_Latin1_General_CP1_CI_AS ";
            sc += " OR c.name_cn LIKE N'%" + Common.EncodeQuote(m_kw) + "%' COLLATE SQL_Latin1_General_CP1_CI_AS ";
        }
        sc += " ORDER BY c.name ";
        //DEBUG("sc",sc);
        try
        {
            SqlDataAdapter myCommand = new SqlDataAdapter(sc, myConnection);
            nRows = myCommand.Fill(ds, "items");
        }
        catch (Exception e)
        {
            Common.ShowExp(sc, e);
            return false;
        }

        Response.Write("<form name=f action=?t=ba&id=" + m_id + " method=post>");
        Response.Write("<input type=hidden name=rows value=" + nRows.ToString() + ">");
        Response.Write("<center><font size=+1><b>Batch Add for Category #" + m_id + " " + m_kw + "</b></font><br>");
        Response.Write("Keyword : <input type=text name=kw value='" + m_kw + "'>");
        Response.Write("<input type=submit name=cmd value='Search' class=b><br>");
        Response.Write("<table cellspacing=1 cellpadding=2 border=0 class=t>");
        Response.Write("<tr bgcolor=#EEEEEE><th>Barcode</th><th>Description</th><th>Other Desc</th><th>Select</th></tr>");

        for (int i = 0; i < nRows; i++)
        {
            DataRow dr = ds.Tables["items"].Rows[i];
            string code = dr["code"].ToString();
            string barcode = dr["supplier_code"].ToString();
            string name = dr["name"].ToString();
            string name_cn = dr["name_cn"].ToString();
            string biid = dr["biid"].ToString();

            name = name.Replace("'", "");
            name_cn = name_cn.Replace("'", "");

            Response.Write("<input type=hidden name=code" + i + " value='" + code + "'>");
            Response.Write("<input type=hidden name=name" + i + " value='" + name + "'>");
            Response.Write("<input type=hidden name=name_cn" + i + " value='" + name_cn + "'>");
            Response.Write("<tr id=row" + i.ToString());
            if (i % 2 != 0)
                Response.Write(" bgcolor=#EEEEEE");
            Response.Write(">");
            Response.Write("<td>" + barcode + "</td><td>" + name + "</td><td>" + name_cn + "</td>");
            Response.Write("<td><input type=checkbox value=1 name=c" + i + " ");
            if (biid != "")
                Response.Write(" checked readonly=true");
            Response.Write(" onclick=\"if(this.checked){document.getElementById('row" + i.ToString() + "').style.backgroundColor='pink';}else{document.getElementById('row" + i.ToString() + "').style.backgroundColor='white';}\"></td></tr>");
        }
        Response.Write("<tr><td colspan=4 align=right>");
        Response.Write("<input type=submit name=cmd value='Add Checked' class=b></td></tr>");
        Response.Write("</table></form>");
        return true;
    }

    bool PrintEditItemFormSingleButton()
    {
        int nRows = 0;
        string sc = " SELECT TOP 100 c.cat, c.s_cat, c.code, c.supplier_code ";
        sc += ", b.barcode ";
        sc += ", c.name, c.name_cn, i.id AS biid ";
        sc += " FROM code_relations c ";
        sc += " LEFT OUTER JOIN barcode b  ON b.item_code = c.code";
        sc += " LEFT OUTER JOIN button_item i ON i.code = c.code ";
        sc += " WHERE 1 = 1 ";
        if (m_kw != "")
            sc += " AND c.name LIKE N'%" + Common.EncodeQuote(m_kw) + "%' OR c.name_cn LIKE N'%" + Common.EncodeQuote(m_kw) + "%' ";
        else if (Request.Form["barcode"] != null)
            sc += " AND (c.supplier_code LIKE '%" + Common.EncodeQuote(Common.p("barcode")) + "%'  OR b.barcode  LIKE '%" + Common.EncodeQuote(Common.p("barcode")) + "%') ";
        if (Common.IsInteger(m_kw))
            sc += " OR c.code = '" + m_kw + "'";
        sc += " GROUP BY c.code, c.supplier_code, b.barcode, c.cat, c.s_cat, c.name, c.name_cn, i.id ";
        sc += " ORDER BY c.name ";
        //DEBUG("sc=", sc);	
        try
        {
            SqlDataAdapter myCommand = new SqlDataAdapter(sc, myConnection);
            nRows = myCommand.Fill(ds, "items");
        }
        catch (Exception e)
        {
            Common.ShowExp(sc, e);
            return false;
        }
        string ibarcode = "";
        string icode = "";
        string iname = "";
        string iname_cn = "";
        string btn_name = "";
        string btn_name_en = "";
        if (m_sid != "")
        {
            sc = " SELECT i.code, i.name, i.name_en,  c.name AS item_name, c.name_cn AS item_name_cn, c.supplier_code, c.barcode ";
            sc += " FROM button_item i ";
            sc += " LEFT OUTER JOIN code_relations c ON c.code = i.code ";
            //		sc += " WHERE i.id = " + m_sid;
            sc += " WHERE i.button_id = " + m_id;
            sc += " AND i.location = " + m_sid;
            //DEBUG("sid, sc=", sc);		
            try
            {
                SqlDataAdapter myCommand = new SqlDataAdapter(sc, myConnection);
                if (myCommand.Fill(ds, "button") > 0)
                {
                    DataRow dr = ds.Tables["button"].Rows[0];
                    icode = dr["code"].ToString();
                    ibarcode = dr["barcode"].ToString();
                    iname = dr["item_name"].ToString();
                    iname_cn = dr["item_name_cn"].ToString();
                    btn_name = dr["name"].ToString();
                    btn_name_en = dr["name_en"].ToString();
                    Common.Trim(ref iname);
                    Common.Trim(ref iname_cn);
                    Common.Trim(ref ibarcode);
                }
            }
            catch (Exception e)
            {
                Common.ShowExp(sc, e);
                return false;
            }
        }

        Response.Write("<form name=f action=?t=es&id=" + m_id + "&indi=" + s_indi + "&sid=" + m_sid + " method=post>");
        Response.Write("<input type=hidden name=rows value=" + nRows.ToString() + ">");
        //DEBUG("s",s_indi);

        //	    Response.Write("<center><font size=+1><b>Edit Button #" + m_id + " - " + m_sid + "</b></font><br>");
        Response.Write("<table>");
        if (s_indi != "True")
        {
            Response.Write("<tr><td>Item Name : </td><td>" + iname + "</td></tr>");
            Response.Write("<tr><td>Item Name_CN : </td><td>" + iname_cn + "</td></tr>");
            Response.Write("<tr><td>Button Name CN : </td><td><input type=text name=btn_name value='" + btn_name + "' onclick='select()'>");
            Response.Write("<input type=submit name=cmd value=Save class=b></td></tr>");
            Response.Write("<tr><td>Button Name EN : </td><td><input type=text name=btn_name_en value='" + btn_name_en + "' onclick='select()'>");
        }
        //	Response.Write("<input type=submit name=cmd value=Save class=b></td></tr>");
        Response.Write("<tr><td>Barcode: </td><td><input type=text name=barcode value='" + ibarcode + "' onclick=\"if(event.keyCode == 13){document.getElementById('scan').value=Scan;}\">");
        Response.Write("<input type=submit name=cmd value=Scan class=b id=scan >");

        Response.Write("</td></tr>");
        Response.Write("<tr><td>Search Item by Name : </td><td><input type=text name=kw value='" + m_kw + "' onclick='select()'>");
        Response.Write("<input type=submit name=cmd value='Search' class=b>");

        Response.Write("</td></tr>");
        Response.Write("<tr><td align=center colspan=2 ><input type=submit name=cmd value=Save class=b>&nbsp;");
        if (m_sid != "0")
            Response.Write("&nbsp;<input type=button class=b value=Delete onclick=\"window.location='?t=es&a=del&id=" + m_id + "&sid=" + m_sid + "';\">");
        //	Response.Write("&nbsp;<input type=button class=b value=Close onclick=\"window.opener.document.location.reload();window.close();\">");
        Response.Write("&nbsp;<input type=button class=b value=Close onclick=\"window.location='?';\">");

        Response.Write("</td></tr>");
        Response.Write("</table>");

        Response.Write("<table cellspacing=1 cellpadding=2 border=0 class=t>");
        Response.Write("<tr bgcolor=#EEEEEE>");

        Response.Write("<th>Code </th>");
        Response.Write("<th>Supplier Code </th>");
        Response.Write("<th>Barcode </th>");
        Response.Write("<th>Description</th><th>Other Desc</th><th>Select</th></tr>");

        for (int i = 0; i < nRows; i++)
        {
            DataRow dr = ds.Tables["items"].Rows[i];
            string code = dr["code"].ToString();
            string supplier_code = dr["supplier_code"].ToString();
            string barcode = dr["barcode"].ToString();
            string name = dr["name"].ToString();
            string name_cn = dr["name_cn"].ToString();
            //string biid = dr["biid"].ToString();
            name = name.Replace("'", "");
            name_cn = name_cn.Replace("'", "");
            Common.Trim(ref name);
            Common.Trim(ref name_cn);
            Common.Trim(ref barcode);
            if (barcode == "" || (name == "" && name_cn == ""))
                continue;

            Response.Write("<input type=hidden name=code" + i + " value='" + code + "'>");
            Response.Write("<input type=hidden name=name" + i + " value='" + name + "'>");
            Response.Write("<input type=hidden name=name_cn" + i + " value='" + name_cn + "'>");
            Response.Write("<tr");
            if (i % 2 == 0)
                Response.Write(" bgcolor=#EEEEEE");
            Response.Write(">");
            Response.Write("<td>" + code + "</td>");
            Response.Write("<td>" + supplier_code + "</td>");

            Response.Write("<td align=right>" + barcode + "</td><td>" + name + "</td><td>" + name_cn + "</td>");
            Response.Write("<td><a href=?t=es&id=" + m_id + "&sid=" + m_sid + "&a=assign&indi=" + s_indi + "&code=" + code);
            //Response.Write("<td><a href=?t=es&id=" + m_id + "&sid=" + m_sid + "&indi="+s_indi+"&code=" + code);
            Response.Write("&name=" + HttpUtility.UrlEncode(name_cn) + " class=o>Assign</a></td></tr>");
        }
        Response.Write("</table></form>");
        return true;
    }

    bool PrintEditItemForm()
    {
        int nRows = 0;
        //string sc = " SELECT TOP 1000 c.cat, c.s_cat, c.code, c.supplier_code ";
        //sc += ", b.barcode ";
        //sc += ", c.name, c.name_cn, i.id AS biid ";
        //sc += " FROM code_relations c ";
        //sc += " LEFT OUTER JOIN barcode b  ON b.item_code = c.code";
        //sc += " LEFT OUTER JOIN button_item i ON i.code = c.code ";
        //sc += " WHERE 1 = 1 ";
        //if(m_kw != "")
        //    sc += " AND c.name LIKE N'%" + Common.EncodeQuote(m_kw) + "%' OR c.name_cn LIKE N'%" + Common.EncodeQuote(m_kw) + "%' ";
        //else if(Request.Form["barcode"] != null)
        //{
        //    sc += " AND (b.barcode  LIKE '%" + Common.EncodeQuote(p("barcode")) + "%') ";
        //    sc += " OR (c.supplier_code  LIKE '%" + Common.EncodeQuote(p("barcode")) + "%') ";
        //}
        //if(IsInteger(m_kw))
        //    sc += " OR c.code = '"+m_kw+"'";
        //sc += " GROUP BY c.code, c.supplier_code, b.barcode, c.cat, c.s_cat, c.name, c.name_cn, i.id ";
        //sc += " ORDER BY c.name ";
        string sc = "  SELECT DISTINCT ROW_NUMBER() OVER(ORDER BY c.name_cn) AS Row#, c.code, c.name, c.name_cn, c.name_des,c.barcode, c.price1 as price, c.cat, c.s_cat, c.ss_cat , cb.price1 as cbPrice, ISNULL(cb.special, '0') as is_special, cb.special_price  FROM code_relations c ";
        sc += " LEFT OUTER JOIN code_branch cb ON cb.code=c.code  ";
        sc += " WHERE c.skip = 0";
        //sc += " and cat = N'" + category + "'";
        sc += " AND cat = N'" + getCat(m_id) + "' ";
        sc += " AND cb.branch_id  IN ('1','6','10') ";
        sc += " AND c.code NOT IN (select code from item where 1=1 and cat_id = " + m_id + " and code is not null and code <> '')";

        if (m_kw != "")
            sc += " AND c.name LIKE N'%" + Common.EncodeQuote(m_kw) + "%' OR c.name_cn LIKE N'%" + Common.EncodeQuote(m_kw) + "%' ";
        else if (Request.Form["barcode"] != null && Request.Form["barcode"].ToString() != "")
        {
            sc += " AND (c.barcode= '" + Common.EncodeQuote(Common.p("barcode")) + "'";
            if (Common.IsInteger(Common.p("barcode")))
                sc += " OR c.code= '" + Common.EncodeQuote(Common.p("barcode")) + "'";
            sc += ")";
        }
        sc += " GROUP BY c.s_cat, c.cat, c.code, c.name, c.name_cn, c.name_des, c.barcode, c.price1, c.ss_cat, cb.price1, cb.special, cb.special_price ";
        sc += " ORDER by c.name_cn ";
        //DEBUG("sc=", sc);	
        try
        {
            SqlDataAdapter myCommand = new SqlDataAdapter(sc, myConnection);
            nRows = myCommand.Fill(ds, "items");
        }
        catch (Exception e)
        {
            Common.ShowExp(sc, e);
            return false;
        }
        string icode = "";
        string btn_name = "";
        string btn_des = "";
        string btn_kitchen_des = "";
        if (m_sid != "")
        {
            //sc = " SELECT i.code, i.name, i.name_en,  c.name AS item_name, c.name_cn AS item_name_cn, c.supplier_code ";
            //sc += " FROM button_item i ";
            //sc += " LEFT OUTER JOIN code_relations c ON c.code = i.code ";
            //sc += " WHERE i.button_id = "+m_id;
            //sc += " AND i.location = " + m_sid;
            sc = " SELECT i.*, c.name, c.name_cn, c.name_des from item i ";
            sc += " JOIN code_relations c on c.code = i.code ";
            sc += " where 1=1 and i.cat_id = '" + m_id + "' AND i.seq = '" + m_sid + "'";
            //DEBUG("sid, sc=", sc);		
            try
            {
                SqlDataAdapter myCommand = new SqlDataAdapter(sc, myConnection);
                if (myCommand.Fill(ds, "button") > 0)
                {
                    DataRow dr = ds.Tables["button"].Rows[0];
                    icode = dr["code"].ToString();
                    btn_name = dr["name_cn"].ToString();
                    btn_des = dr["name"].ToString();
                    btn_kitchen_des = dr["name_des"].ToString();
                    Common.Trim(ref btn_name);
                    Common.Trim(ref btn_des);
                    Common.Trim(ref btn_kitchen_des);
                }
            }
            catch (Exception e)
            {
                Common.ShowExp(sc, e);
                return false;
            }
        }

        Response.Write("<form name=f action=?t=es&id=" + m_id + "&sid=" + m_sid + " method=post>");
        Response.Write("<input type=hidden name=rows value=" + nRows.ToString() + ">");
        //DEBUG("s_indi",s_indi);

        Response.Write("<center><br><br><font size=+1><b>Edit Button #" + m_id + " - " + m_sid + "</b></font><br>");
        Response.Write("<table>");
        Response.Write("<tr><td> Code: </td><td><input type=text name=barcode value='" + Common.p("barcode") + "' onclick=\"if(event.keyCode == 13){document.getElementById('scan').value=Scan;}\">");
        Response.Write("<input style=\"width:56px;\" type=submit name=cmd value=Scan class=b id=scan ></td></tr>");
        Response.Write("<tr><td>Search Item by Name : </td><td><input type=text name=kw value='" + m_kw + "' onclick='select()'>");
        Response.Write("<input type=submit name=cmd value='Search' class=b></td></tr>");
        Response.Write("<input type=hidden name=item_code value='" + icode + "'>");
        Response.Write("<tr><td>Item code : </td><td> " + icode + "</td></tr>");
        Response.Write("<tr><td>Button Name : </td><td><input type=text name=btn_name_database_name_cn value='" + btn_name + "'  onclick='select()'></td></tr>");
        Response.Write("<tr><td>Invoice Description : </td><td><input type=text name=btn_des_database_name value='" + btn_des + "'  onclick='select()'></td></tr>");
        Response.Write("<tr><td>Kitchen Description : </td><td><input type=text name=btn_kitchen_des_database_name_des value='" + btn_kitchen_des + "'  onclick='select()'><input type=submit name=cmd value=Save class=b></td></tr>");
        Response.Write("<tr><td align=center colspan=2 >");
        if (m_sid != "0")
            Response.Write("&nbsp;<input style=\"width:70px;\" type=button class=b value=Delete onclick=\"window.location='?t=es&a=del&id=" + m_id + "&sid=" + m_sid + "';\">");
        Response.Write("&nbsp;<input style=\"width:70px;\" type=button class=b value=Close onclick=\"window.opener.document.location.reload();window.close();\">");

        /*//if(s_indi!= "True")
        {
            Response.Write("<tr><td>Item Name : </td><td>" + iname + "</td></tr>");
            Response.Write("<tr><td>Item Name_Other : </td><td>" + iname_cn + "</td></tr>");

            Response.Write("<tr><td>Button Name : </td><td><input type=text name=btn_name_en value='" + btn_name_en + "' onclick='select()'>");

            Response.Write("<input type=submit name=cmd value=Save class=b></td></tr>");
            Response.Write("<tr><td>Button Name Other : </td><td><input type=text name=btn_name value='" + btn_name + "' onclick='select()'>");
        }
    //	Response.Write("<input type=submit name=cmd value=Save class=b></td></tr>");
        Response.Write("<tr><td> Code: </td><td><input type=text name=barcode value='" + icode + "' onclick=\"if(event.keyCode == 13){document.getElementById('scan').value=Scan;}\">");
        Response.Write("<input type=submit name=cmd value=Scan class=b id=scan >");

        Response.Write("</td></tr>");
        Response.Write("<tr><td>Search Item by Name : </td><td><input type=text name=kw value='" + m_kw + "' onclick='select()'>");
        Response.Write("<input type=submit name=cmd value='Search' class=b>");

        Response.Write("</td></tr>");
        Response.Write("<tr><td align=center colspan=2 ><input type=submit name=cmd value=Save class=b>&nbsp;");
        if(m_sid != "0")
            Response.Write("&nbsp;<input type=button class=b value=Delete onclick=\"window.location='?t=es&a=del&id="+m_id+"&sid=" + m_sid + "';\">");
        Response.Write("&nbsp;<input type=button class=b value=Close onclick=\"window.opener.document.location.reload();window.close();\">");*/

        Response.Write("</td></tr>");
        Response.Write("</table>");

        Response.Write("<table cellspacing=1 cellpadding=2 border=0 class=t>");
        Response.Write("<tr bgcolor=#EEEEEE>");

        Response.Write("<th>Code </th>");
        Response.Write("<th>S_Code </th>");
        Response.Write("<th>Barcode </th>");
        Response.Write("<th>Button Name</th>");
        Response.Write("<th>Invoice Des</th>");
        Response.Write("<th>Kitchen Des</th>");
        Response.Write("<th>Select</th></tr>");

        //DEBUG("123",nRows.ToString());
        for (int i = 0; i < nRows; i++)
        {
            DataRow dr = ds.Tables["items"].Rows[i];
            string code = dr["code"].ToString();
            string supplier_code = "";// dr["supplier_code"].ToString();
            string barcode = dr["barcode"].ToString();
            string name = dr["name"].ToString();
            string name_cn = dr["name_cn"].ToString();
            string name_des = dr["name_des"].ToString();
            name = name.Replace("'", "");
            name_cn = name_cn.Replace("'", "");
            name_des = name_des.Replace("'", "");
            Common.Trim(ref name);
            Common.Trim(ref name_cn);
            Common.Trim(ref name_des);
            Common.Trim(ref barcode);
            if (barcode == "" || (name == "" && name_cn == ""))
                continue;

            Response.Write("<input type=hidden name=code" + i + " value='" + code + "'>");
            Response.Write("<input type=hidden name=name" + i + " value='" + name + "'>");
            Response.Write("<input type=hidden name=name_cn" + i + " value='" + name_cn + "'>");
            Response.Write("<input type=hidden name=name_des" + i + " value='" + name_des + "'>");
            Response.Write("<tr");
            if (i % 2 == 0)
                Response.Write(" bgcolor=#EEEEEE");
            Response.Write(">");

            Response.Write("<td>" + code + "</td>");
            Response.Write("<td>" + supplier_code + "</td>");
            Response.Write("<td align=left>" + barcode + "</td><td>" + name_cn + "</td><td>" + name + "</td><td>" + name_des + "</td>");
            Response.Write("<td><a href=?t=es&id=" + m_id + "&sid=" + m_sid + "&a=assign&indi=" + s_indi + "&code=" + code);
            //Response.Write("<td><a href=?t=es&id=" + m_id + "&sid=" + m_sid + "&code=" + code);
            Response.Write("&name=" + HttpUtility.UrlEncode(name_cn) + " class=o>Assign</a></td></tr>");
        }
        Response.Write("</table></form>");
        return true;
    }
    bool DoSaveCategory()
    {
        bool bsingle = false;
        string name = Common.p("name");
        string name_en = Common.p("name_en");
        string single = Common.p("is_indivisual");
        string barcode = Common.p("barcode");
        if (single.ToLower() == "on")
            single = "1";
        else
            single = "0";
        string sc = " UPDATE button SET name = N'" + Common.EncodeQuote(name) + "', name_en = N'" + Common.EncodeQuote(name_en) + "' ";
        sc += " ,is_indivisual = N'" + single + "'";
        sc += " WHERE id = " + m_id;
        if (single == "1")
        {
            sc += " if not exists(select id from settings where 1=1 and name='button_id_" + m_id + "')";
            sc += " insert into settings(name, value)VALUES('button_id_" + m_id + "', '" + barcode + "')";
            sc += " ELSE ";
            sc += " update settings SET value = '" + barcode + "' WHERE 1=1 ";
            sc += " AND name = 'button_id_" + m_id + "'";
        }
        else if (single == "0")
        {
            sc += " DELETE FROM settings WHERE 1=1 ";
            sc += " AND name = 'button_id_" + m_id + "'";
        }

        try
        {
            Common.myCommand = new SqlCommand(sc);
            Common.myCommand.Connection = myConnection;
           Common.myCommand.Connection.Open();
            Common.myCommand.ExecuteNonQuery();
            Common.myCommand.Connection.Close();
        }
        catch (Exception e)
        {
            Common.ShowExp(sc, e);
            return false;
        }
        return true;
    }
    bool DoBatchAdd()
    {
        string sc = "";
        int nRows = Common.MyIntParse(Common.p("rows"));
        for (int i = 0; i < nRows; i++)
        {
            string ccheck = Common.p("c" + i);
            if (ccheck != "1")
                continue;
            string code = Common.p("code" + i);
            string name = Common.p("name" + i);
            string name_cn = Common.p("name_cn" + i);
            sc += " IF NOT EXISTS(SELECT id FROM button_item WHERE button_id = " + m_id + " AND code = " + code + ") ";
            sc += " INSERT INTO button_item (button_id, code, name) VALUES(" + m_id + ", " + code + ", N'" + Common.EncodeQuote(name_cn) + "'); ";
        }
        if (sc == "")
            return true;
        try
        {
           Common.myCommand = new SqlCommand(sc);
            Common.myCommand.Connection = myConnection;
            Common.myCommand.Connection.Open();
            Common.myCommand.ExecuteNonQuery();
            Common.myCommand.Connection.Close();
        }
        catch (Exception e)
        {
            Common.ShowExp(sc, e);
            return false;
        }
        return true;
    }
    bool DoSaveButtonName()
    {
        string item_code = Common.p("item_code");
        string btn_name = Common.p("btn_name_database_name_cn");
        string invoice_name = Common.p("btn_des_database_name");
        string Kitchen_name = Common.p("btn_kitchen_des_database_name_des");
        string sc = "";
        sc += " UPDATE code_relations SET name_cn = N'" + Common.EncodeQuote(btn_name) + "', name =N'" + Common.EncodeQuote(invoice_name) + "', name_des =N'" + Kitchen_name + "' ";
        sc += " WHERE code = " + item_code;
        //DEBUG("sc=", sc);
        //return false;	
        try
        {
            Common.myCommand = new SqlCommand(sc);
            Common.myCommand.Connection = myConnection;
            Common.myCommand.Connection.Open();
            Common.myCommand.ExecuteNonQuery();
           Common.myCommand.Connection.Close();
        }
        catch (Exception e)
        {
            Common.ShowExp(sc, e);
            return false;
        }
        return true;
    }
    bool DoAssignItem()
    {
        string code = Common.g("code");
        string name = Common.g("name");
        string sc = "";
        sc = " BEGIN TRANSACTION ";
        //sc += " IF NOT EXISTS(SELECT id FROM button_item WHERE location = " + m_sid + " AND button_id = "+m_id+") ";
        //sc += " INSERT INTO button_item (button_id, code, name, location) VALUES(" + m_id + ", " + code + ", N'" + Common.EncodeQuote(name) + "','"+Common.EncodeQuote(m_sid)+"') ";
        //sc += " ELSE ";
        //sc += " UPDATE button_item SET code = " + code + ", name = N'" + Common.EncodeQuote(name) + "', location = "+m_sid+" WHERE button_id = " + m_id;
        //sc += " AND location = "+m_sid;
        sc += " UPDATE item SET code = '" + code + "' WHERE 1=1 ";
        sc += " AND cat_id = '" + m_id + "'";
        sc += " AND seq = '" + m_sid + "'";
        sc += " COMMIT ";

        try
        {
            Common.myCommand = new SqlCommand(sc);
            Common.myCommand.Connection = myConnection;
            Common.myCommand.Connection.Open();
            Common.myCommand.ExecuteNonQuery();
            Common.myCommand.Connection.Close();
        }
        catch (Exception e)
        {
            Common.ShowExp(sc, e);
            return false;
        }
        return true;
    }
    bool DoDeleteItem()
    {
        if (m_sid == "0")
            return true;
        //string sc = " DELETE FROM button_item WHERE id = " + m_sid;
        string sc = " UPDATE item SET code = '' WHERE cat_id = '" + m_id + "' AND seq = '" + m_sid + "'";
        //DEBUG("sc",sc);
        //return false;
        try
        {
            Common.myCommand = new SqlCommand(sc);
           Common.myCommand.Connection = myConnection;
           Common. myCommand.Connection.Open();
            Common.myCommand.ExecuteNonQuery();
            Common.myCommand.Connection.Close();
        }
        catch (Exception e)
        {
            Common.ShowExp(sc, e);
            return false;
        }
        Response.Write("<script language=javascript>window.close();window.opener.document.location.reload();</script");
        //Response.Write("<meta http-equiv=\"refresh\" content=\"0; URL=?t=" + m_type + "&id=" + m_id + "&sid=" + m_sid + "\">");
        Response.Write(">");
        return true;
    }
    bool GetLanguage(string id)
    {
        if (ds.Tables["language"] != null)
            ds.Tables["language"].Clear();
        string sc = " SELECT language FROM card WHERE id = " + id;
        try
        {
            SqlDataAdapter myCommand = new SqlDataAdapter(sc, myConnection);
            if (myCommand.Fill(ds, "language") > 0)
            {
                if (ds.Tables["language"].Rows[0]["language"].ToString() == "2")
                    return true;
            }
        }
        catch (Exception e)
        {
            Common.ShowExp(sc, e);
            return false;
        }
        return false;
    }
}
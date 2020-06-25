using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;






public partial class mobile_layout_LeftSidebar : System.Web.UI.UserControl
{
    protected DataTable menuTable;
   protected string sc;
    public DBhelper dbhelper = new DBhelper();
 
    protected void Page_Load(object sender, EventArgs e)
    {


        //string accessLevel = Session[Company.m_sCompanyName + "AccessLevel"].ToString();
        sc = "select * from menu_admin_catalog order by seq";
        //if (accessLevel == "10")
        //{
        //    sc = "select * from menu_admin_catalog order by seq";
        //}
        //else
        //{
        //    sc = "select * from menu_admin_catalog order by seq";
        //}

        menuTable = dbhelper.ExecuteDataTable(sc);
    }
    protected DataTable getSubTable(string id)
    {
        //string accessLevel = Session[Company.m_sCompanyName + "AccessLevel"].ToString();
        string accessLevel = "10";
        if (accessLevel == "10")
        {
            sc = @"SELECT i.name,i.uri FROM menu_admin_sub s 
              JOIN  menu_admin_id i ON s.menu=i.id
           
                WHERE s.cat=" + id ;
        }
        else
        {
            sc = @"SELECT i.name,i.uri FROM menu_admin_sub s 
              JOIN  menu_admin_id i ON s.menu=i.id
              JOIN menu_admin_access a on a.menu=i.id
                WHERE s.cat=" + id + " AND a.class="+accessLevel;
        }
       
        //Response.Write(sc);
        DataTable menuSubTable = dbhelper.ExecuteDataTable(sc);
        return menuSubTable;
    }
    protected string GetFontawesome(int i)
    {
        string[] fontName = { "fa-tachometer-alt","fa-th","fa-copy" ,"fa-chart-pie","fa-tree","fa-edit","fa-table",
        "fa-calendar-alt","fa-image","fa-envelope","fa-book","fa-plus-square","fa-file"};

        int t = i % fontName.Length;
        return fontName[t];
    }
    protected string UseNewVersionMenu(string submenu)
    {
        string[] newMenu = { "eprice.aspx?", "ec.aspx" };
        if (Array.IndexOf(newMenu, submenu) != -1)
        {
            return "/v2/" + submenu;
        }
        else
        {
            return "/admin/" + submenu;
        }

    }
}


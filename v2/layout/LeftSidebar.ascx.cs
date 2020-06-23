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
       
     
  
        sc = "select * from menu_admin_catalog order by seq";
      
       menuTable = dbhelper.ExecuteDataTable(sc);
    }
    protected DataTable getSubTable(string id)
    {
        sc = @"SELECT i.name,i.uri FROM menu_admin_sub s 
              JOIN  menu_admin_id i ON s.menu=i.id
              JOIN menu_admin_access a on a.menu=i.id
                WHERE s.cat=" +id+" AND a.class=10";
        //Response.Write(sc);
        DataTable menuSubTable = dbhelper.ExecuteDataTable(sc);
        return menuSubTable;
    }

}


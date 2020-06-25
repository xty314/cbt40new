using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class v2_eprice : System.Web.UI.Page
//public partial class v2_eprice : AdminBasePage

{
    public DBhelper dbhelper = new DBhelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        ContentHeader.GTitle = "Item List(dd)";
        string sc = "SELECT company,id FROM card where type=3 ";
        DataTable dt = dbhelper.ExecuteDataTable(sc);
        SupplierDropDownList.DataSource = dt;
        SupplierDropDownList.DataTextField = "company";
        SupplierDropDownList.DataValueField = "id";
        SupplierDropDownList.DataBind();
    }


}
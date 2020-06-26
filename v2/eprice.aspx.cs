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
        ContentHeader.GTitle = "Item List";
        if (!IsPostBack)
        {
            init_SupplierDropDownList();
            init_CatDropDownList();

            sCatDropDownList.Visible = false;
            ssCatDropDownList.Visible = false;

        }

       


    }
    private void init_SupplierDropDownList()
    {

        string sc = "SELECT trading_name,id FROM card where type=3  ORDER BY trading_name";
        DataTable dt = dbhelper.ExecuteDataTable(sc);
        SupplierDropDownList.DataSource = dt;
        SupplierDropDownList.DataTextField = "trading_name";
        SupplierDropDownList.DataValueField = "id";
        SupplierDropDownList.DataBind();
        SupplierDropDownList.Items.Insert(0, new ListItem("All Supplier", "-1"));
        init_CatDropDownList();
    }
    private void init_CatDropDownList()
    {
        string sc = "SELECT distinct cat FROM code_relations";
        //bind the cat with the supplier
        if (SupplierDropDownList.SelectedValue != "-1")
        {
            sc += " WHERE supplier=" + SupplierDropDownList.SelectedValue;
        }
       
        DataTable dt = dbhelper.ExecuteDataTable(sc);
        CatDropDownList.DataSource = dt;
        CatDropDownList.DataTextField = "cat";
        CatDropDownList.DataValueField = "cat";
        CatDropDownList.DataBind();
        CatDropDownList.Items.Insert(0, new ListItem("All", "-1"));
    }
    private void init_sCatDropDownList()
    {
        string cat = CatDropDownList.Text;
      
        if (!String.IsNullOrEmpty(cat))
        {
       
            string sc = "SELECT distinct s_cat FROM code_relations where cat=N'" + cat+"'";
            if (SupplierDropDownList.SelectedValue != "-1")
            {
                sc += " AND supplier=" + SupplierDropDownList.SelectedValue;
            }
            DataTable dt = dbhelper.ExecuteDataTable(sc);
            if (dt.Rows.Count > 0)
            {
                sCatDropDownList.Visible = true;
            }
            sCatDropDownList.DataSource = dt;
            sCatDropDownList.DataTextField = "s_cat";
            sCatDropDownList.DataValueField = "s_cat";
            sCatDropDownList.DataBind();
            sCatDropDownList.Items.Insert(0, new ListItem("All", "-1"));

        }

    }
    private void init_ssCatDropDownList()
    {
        string cat = CatDropDownList.Text;
        string scat = sCatDropDownList.Text;
       
            string sc = "SELECT distinct s_cat FROM code_relations where cat=N'" + cat + "' AND s_cat=N'"+scat+"'";
            if (SupplierDropDownList.SelectedValue != "-1")
            {
                sc += " AND supplier=" + SupplierDropDownList.SelectedValue;
            }
            DataTable dt = dbhelper.ExecuteDataTable(sc);
        if (dt.Rows.Count > 0)
        {
            ssCatDropDownList.Visible = true;
        }
        ssCatDropDownList.DataSource = dt;
            ssCatDropDownList.DataTextField = "s_cat";
            ssCatDropDownList.DataValueField = "s_cat";
            ssCatDropDownList.DataBind();
            ssCatDropDownList.Items.Insert(0, new ListItem("All", "-1"));

     

    }



    protected void SupplierDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        sCatDropDownList.Visible = false;
        ssCatDropDownList.Visible = false;
        init_CatDropDownList();
        init_sCatDropDownList();
        init_ssCatDropDownList();
    }

    protected void CatDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        sCatDropDownList.Visible = false;
        ssCatDropDownList.Visible = false;
        init_sCatDropDownList();
        init_ssCatDropDownList();
    }

    protected void sCatDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {

        ssCatDropDownList.Visible = false;
        init_ssCatDropDownList();

    }
}
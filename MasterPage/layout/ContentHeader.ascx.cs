using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class mobile_layout_ContentHeader : System.Web.UI.UserControl
{
    public string GTitle
    {
        get { return this.Title1.InnerText; }
        set {
            this.Title1.InnerText = value;
            this.Title2.InnerText = value;
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {

    }
}
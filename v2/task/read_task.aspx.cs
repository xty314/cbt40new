using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class v2_read_task : System.Web.UI.Page
{
    DBhelper dbhelper = new DBhelper();

    protected void Page_Load(object sender, EventArgs e)
    {
        //string taskid =Request.QueryString["taskid"];
        //string sc = "SELECT note FROM dev_task_note where id="+taskid;
        //string note = (string)dbhelper.ExecuteScalar(sc);
        //MessageBoard.InnerHtml = note;
    }
    //[WebMethod]
    //public static string test(string id)
    //{
    //    return id;
    //}
}
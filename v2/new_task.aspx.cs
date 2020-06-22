using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class v2_newtask : System.Web.UI.Page
{
    DBhelper dbhelper = new DBhelper();
  //public  JSONhelper jsonhelper = new JSONhelper();
    protected string m_id="";
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["card_id"] = "6";

        //Response.Write(TaskNote.InnerText);
    }

    [WebMethod]
    public static string test(string id)
    {
        return id;
    }
    [WebMethod]
    public static string ajax_test(int? id, Boolean is_test, string cmd = "")
    {
        var result = new { id = id.Value, cmd = "sb", is_test = is_test };
        DBhelper dbhelper = new DBhelper();
        JSONhelper jsonhelper = new JSONhelper();
        string sc = "select * from code_relations ";
        DataTable dt = dbhelper.ExecuteDataTable(sc);
       
        return dt.Dtb2Json();
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        bool bUnAssigned = false;
 

        string founder = Session["card_id"].ToString();
       
        string owner = "0";
        if (owner == "0")
        {
            owner = founder;
            bUnAssigned = true;
        }

        string url = TextBoxURL.Text;
        string subject = TextBoxSubject.Text;
        string priority = TextBoxPriority.Text;
     
        string note = TaskNote.InnerText;
        string status="" ;
        if (status == "")
            status = "pending";
        string customer_id = "3";//p("customer_id"];
        string TVID = TextBoxTVID.Text;
        string finish_date_required = TextBoxFDR.Text;
        string task_type = TextBoxTaskType.Text;
        string task_level =TextBoxTaskLevel.Text;
        int nPoint = 1;
     

       


        string sc = "";
        if (m_id == "") //new task
        {
           
            sc += " INSERT INTO dev_task (founder, owner, assign_to, subject, task_type, url, priority, task_level, point) VALUES('";
            sc += founder + "', " + owner + ", " + owner + ", N'" + subject + "' ";
            sc += ", " + task_type;
            sc += ", '" + url + "', '" + priority + "', " + task_level + ", " + nPoint + ")";
          
            m_id =dbhelper.InsertAndGetId(sc).ToString();
        }
        if (note != "")
        {
           
            sc = " INSERT INTO dev_task_note (task_id, card_id, note) VALUES(" + m_id + ", ";
            sc += Session["card_id"].ToString() + ", N'" + Common.EncodeQuote(note) + "') ";
         string  m_noteid= dbhelper.InsertAndGetId(sc).ToString();

        }





    }
}
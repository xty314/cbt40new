using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class v2_dev_task : System.Web.UI.Page
{
    public int userId=3;
    public DBhelper dbhelper = new DBhelper();

    protected DataTable task_note_table;
    protected DataTable taskTable;
    protected string m_type = "";
    protected string m_action = "";
    protected string m_cmd = "";
    protected string m_owner = "";
    protected string m_founder = "";
    protected string m_status = "";
    protected string m_id = "";
    protected string m_noteid = "";
    protected string m_customerID = "";
    protected string m_customerName = "";
    protected string m_sTaskTypeId = "0";
    protected string m_sTaskLevelId = "1";
    protected string m_sFinishDateRequired = "";
    protected int m_nPoint = 1;

    protected void Page_Load(object sender, EventArgs e)
    {
        GetQuery();
        GetTask();
        //Response.Write(GetTask());
    }
    private void GetQuery()
    {
        
            m_type = Common.g("t");
            m_cmd = Common.p("cmd");
 
        if (Request.QueryString["owner"] != null)
        {
            if (Request.QueryString["owner"].ToString() == "")
            {
                m_owner = "";
            }
            else if (Request.QueryString["owner"] == "me")
            {
                m_owner = Session["card_id"].ToString();
                Session["dev_task_card_id"] = m_owner;
            }
            else if (Common.IsInteger(Request.QueryString["owner"]))
            {
                m_owner = Request.QueryString["owner"];
                Session["dev_task_card_id"] = m_owner;
            }
        }
        else if (Session["dev_task_card_id"] != null)
            m_owner = Session["dev_task_card_id"].ToString();
        //	if(m_owner == "")
        //		m_owner = Session["card_id"].ToString();

        if (Request.QueryString["founder"] != null)
            m_founder = Request.QueryString["founder"];
        if (Request.QueryString["status"] != null)
            m_status = Request.QueryString["status"];
        if (Request.QueryString["id"] != null)
            m_id = Request.QueryString["id"];
        if (Request.QueryString["nid"] != null)
            m_noteid = Request.QueryString["nid"];

        m_sTaskTypeId = Common.p("task_type");
        m_sFinishDateRequired = Common.p("finish_date_required");

        if (Request.Form["customer"] != null)
            m_customerID = Common.p("customer");
        else if (Request.QueryString["ci"] != null)
            m_customerID = Request.QueryString["ci"];
        if (Common.MyIntParse(m_customerID) != 0)
            m_customerName = Common.TSGetUserCompanyByID(m_customerID);
     
       // r = DateTime.Now.ToOADate().ToString();
       
     
      

    
    
   
    }
    private bool GetTask()
    {
        string sMonth = Common.g("month");
        int nMonth = Common.MyIntParse(sMonth);
        string sRepeat =Common.g("repeat");

        string sc = " SET DATEFORMAT dmy ";
        sc += " SELECT t.* ";
        sc += ", CONVERT(varchar(100), t.finish_date_required, 103) AS date_required ";
        sc += ", CONVERT(varchar(100), t.repeat_next_date, 103) AS srepeat_next_date ";
        sc += ", (SELECT trading_name FROM card WHERE id = t.customer_id) AS customer ";
        sc += ", (SELECT name FROM card WHERE id = t.assign_to) AS assign_to_name ";
        sc += ", (SELECT name FROM enum WHERE class = 'task_type' AND id = t.task_type) AS s_task_type ";
        sc += ", (SELECT name FROM enum WHERE class = 'task_level' AND id = t.task_type) AS stask_level ";
        sc += " FROM dev_task t ";
        sc += " WHERE 1 = 1 ";
        if (sRepeat != "")
            sc += " AND t.repeat = 1 ";
        else
            sc += " AND t.status <> 'closed' ";
        if (m_sFinishDateRequired != "")
            sc += " AND t.finish_date_required <= '" + m_sFinishDateRequired + "' ";
        if (m_id != "")
        {
            sc += " AND t.id = " + m_id;
        }
        else
        {
            if (m_founder != "")
                sc += " AND t.founder = " + m_founder;
            if (m_customerID != "")
                sc += " AND t.customer_id = " + m_customerID;
            if (sMonth != "")
            {
                if (m_owner != "")
                    sc += " AND t.assign_to = " + m_owner;
                sc += " AND DATEDIFF(month, t.finish_time, GETDATE()) = " + nMonth;
                if (Common.g("finished") == "1")
                    sc += " AND (t.status = 'finished' OR t.status = 'completed') ";
            }

   
            if (m_owner != "")
                sc += " AND (t.owner = " + m_owner + " OR t.founder = " + m_owner + ") ";
            if (m_status != "" && m_status != "all")
                sc += " AND t.status = '" + m_status + "' ";
            else if (m_id == "" && Request.QueryString["finished"] != "1" && m_status != "all")
                sc += " AND t.status <> 'finished' AND t.status <> 'deffered' AND t.status <> 'completed' ";
         
        }
        sc += " ORDER BY t.id DESC";
        taskTable = dbhelper.ExecuteDataTable(sc);
        //DEBUG("sc=", sc);	


        if (m_id != "")
        {
            m_customerID = taskTable.Rows[0]["customer_id"].ToString();
            m_customerName = taskTable.Rows[0]["customer"].ToString();
            sc = " SELECT * FROM dev_task_note WHERE task_id = " + m_id + " ORDER BY note_time ";
            task_note_table = dbhelper.ExecuteDataTable(sc);
   
        }
        return true;
    }
}
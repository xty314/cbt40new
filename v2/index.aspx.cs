﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class v2_index : System.Web.UI.Page
{
   public string kkk = "ddd";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack)
        {
            Response.Write("kkk");
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        Label1.Text = "shishi"; TextBox1.Text = "ddd";
    }

    protected void TextBox1_TextChanged(object sender, EventArgs e)
    {
        Label1.Text = TextBox1.Text;
    }

    protected void Button2_Click(object sender, EventArgs e)
    {

    }
}
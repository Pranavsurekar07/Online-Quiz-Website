﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TestDemo
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            sqlConnection.Open();
            SqlCommand cmd = new SqlCommand("usp_CheckEmailExists", sqlConnection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@email", txtEmail.Text);
            int result = (Int32)cmd.ExecuteScalar();
            if (result == 1)
            {
                Session["Email"] = txtEmail.Text;
                Response.Redirect("Exam.aspx");
                ScriptManager.RegisterStartupScript(this, GetType(), "success", "alert('Logged in successfully');", true);
            }
            else
            {
                List<Tuple<string, string>> parametersList = new List<Tuple<string, string>>();
                parametersList.Add(new Tuple<string, string>("@email", txtEmail.Text));
                var data = SqlDataBind.SaveDataByStoredProcedure("usp_InsertUserData", "ConnectionString", parametersList);
                if (data > 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "success", "alert('Logged in successfully');", true);

                }
                Session["Email"] = txtEmail.Text;
                Response.Redirect("Exam.aspx");
            }

        }
    }
}
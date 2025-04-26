using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace newHR.Controllers
{
    public static class static_class
    {
        static public DataSet getbysql(string sql)
        {
            var ds = new DataSet();
            var dt_data = new DataTable("data");
            var dt_status = new DataTable("status");
            dt_data.Columns.Add(new DataColumn("data"));
            dt_status.Columns.Add(new DataColumn("status"));
            dt_status.Columns.Add(new DataColumn("message"));

            try
            {
                dt_status.Rows.Add("success","success!");
                var c = new SqlConnection(ConfigurationManager.ConnectionStrings["CS"].ConnectionString); // Your Connection String here
                var dataAdapter1 = new SqlDataAdapter(sql, c);
                dataAdapter1.Fill(dt_data);
                //return ds;
            }
            catch (Exception ex)
            {
                dt_status.Rows.Add("error",ex.Message);
                dt_data.Rows.Add("");
            }
            ds.Tables.Add(dt_status);
            ds.Tables.Add(dt_data);
            return ds;
        }
        static public DataSet insertbysql(string sql)
        {
            var ds = new DataSet();
            var dt_data = new DataTable("data");
            var dt_status = new DataTable("status");
            dt_data.Columns.Add(new DataColumn("data"));
            dt_status.Columns.Add(new DataColumn("status"));
            dt_status.Columns.Add(new DataColumn("message"));
            
            try
            {
                SqlCommand com = new SqlCommand(sql);
                com.CommandType = CommandType.Text;
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CS"].ConnectionString))
                {
                    con.Open();
                    com.Connection = con;
                    // return com.ExecuteNonQuery().ToString();//for update and delete
                    dt_status.Rows.Add("success", "success!");
                    dt_data.Rows.Add(com.ExecuteScalar().ToString());

                }
            }
            catch (Exception ex)
            {
                dt_status.Rows.Add("error", ex.Message);
                dt_data.Rows.Add("");
            }
            ds.Tables.Add(dt_status);
            ds.Tables.Add(dt_data);
            return ds;
        }
        static public DataSet updatebysql(string sql)
        {
            var ds = new DataSet();
            var dt_data = new DataTable("data");
            var dt_status = new DataTable("status");
            dt_data.Columns.Add(new DataColumn("data"));
            dt_status.Columns.Add(new DataColumn("status"));
            dt_status.Columns.Add(new DataColumn("message"));

            try
            {
                SqlCommand com = new SqlCommand(sql);
                com.CommandType = CommandType.Text;
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CS"].ConnectionString))
                {
                    con.Open();
                    com.Connection = con;
                    //com.ExecuteScalar().ToString();//for insert
                    dt_status.Rows.Add("success", "success!");
                    dt_data.Rows.Add(com.ExecuteNonQuery().ToString());

                }
            }
            catch (Exception ex)
            {
                dt_status.Rows.Add("error", ex.Message);
                dt_data.Rows.Add("");
            }
            ds.Tables.Add(dt_status);
            ds.Tables.Add(dt_data);
            return ds;
        }
        static public DataSet Authrize(string username)
        {
          
            string sql = @"SELECT u.Id userId,*
                           from Users2 u 
                           where u.UserName='" + username + "' and u.password='" + "'";
            
           return static_class.getbysql(sql);
            
        }
    }
}
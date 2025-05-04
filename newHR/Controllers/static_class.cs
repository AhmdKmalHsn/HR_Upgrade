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

            dt_status.Columns.Add(new DataColumn("status"));
            dt_status.Columns.Add(new DataColumn("message"));

            try
            {

                var c = new SqlConnection(ConfigurationManager.ConnectionStrings["CS"].ConnectionString); // Your Connection String here
                var dataAdapter1 = new SqlDataAdapter(sql, c);
                dataAdapter1.Fill(dt_data);
                dt_status.Rows.Add("success", "success!");
                //return ds;
            }
            catch (Exception ex)
            {
                dt_status.Rows.Add("error", ex.Message);

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
        static public DataSet Authrizes(string token)
        {

            string sql =
                $@"SELECT  m.ModuleName 'module_name',
                          ISNULL(m.url,'')'module_url',
                          [acesss]  'p_access',	
                          [read]	'p_read',	
                          [create]	'p_create',	
                          [update]	'p_update',	
                          [delete]	'p_delete',
                          u.login_to,
                          u.login_permit
                    FROM AK_Users u JOIN 
                         AK_Roles r ON u.RoleId = r.Id JOIN
                         AK_Roles_lines rl ON r.Id = rl.role_id JOIN
                         AK_Modules m ON rl.module_id = m.Id
                    WHERE u.token='{token}'";

            return static_class.getbysql(sql);

        }
        static public DataTable o_Authrizes(string token)
        {

            string sql =
                $@"SELECT  m.ModuleName 'module_name',
                          ISNULL(m.url,'')'module_url',
                          [acesss]  'p_access',	
                          [read]	'p_read',	
                          [create]	'p_create',	
                          [update]	'p_update',	
                          [delete]	'p_delete',
                          u.login_to,
                          u.login_permit
                    FROM AK_Users u JOIN 
                         AK_Roles r ON u.RoleId = r.Id JOIN
                         AK_Roles_lines rl ON r.Id = rl.role_id JOIN
                         AK_Modules m ON rl.module_id = m.Id
                    WHERE u.token='{token}'";

            DataTable dt=new DataTable();
            try
            {
                DataTable pure = static_class.getbysql(sql).Tables["data"];
                if (pure.Rows.Count > 0)
                {
                    //for header => left hand object.
                    for (int i = 0; i < pure.Rows.Count; i++)
                    {
                        dt.Columns.Add(new DataColumn(pure.Rows[i]["module_name"].ToString()));
                    }
                    //add single row that is an object. 
                    dt.Rows.Add();
                    // data of object .
                    for (int i = 0; i < pure.Rows.Count; i++)
                    {
                        dt.Rows[0][pure.Rows[i]["module_name"].ToString()]= pure.Rows[i]["p_access"].ToString();
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return dt;

        }
        static public DataSet Authrize(string module, string token, bool view = false)
        {

            string sql =
                $@"SELECT  m.ModuleName 'module_name',
                          ISNULL(m.url,'')'module_url',
                          [acesss]  'p_access',	
                          [read]	'p_read',	
                          [create]	'p_create',	
                          [update]	'p_update',	
                          [delete]	'p_delete',
                          u.login_to,
                          u.login_permit
                    FROM AK_Users u JOIN 
                         AK_Roles r ON u.RoleId = r.Id JOIN
                         AK_Roles_lines rl ON r.Id = rl.role_id JOIN
                         AK_Modules m ON rl.module_id = m.Id
                    WHERE u.token='{token}' and
                          m.ModuleName='{module}'";

            return static_class.getbysql(sql);

        }
        static public DataSet is_Authrize(string module, string token, string acrud, bool view = false)
        {
            DataSet ds = Authrize(module, token, view);


            if (ds.Tables["data"].Rows.Count == 0)
            {
                ds.Tables["status"].Rows[0]["status"] = "error";
                ds.Tables["status"].Rows[0]["message"] = "Not Authorized";
            }
            else if (
                ds.Tables["data"].Rows[0]["login_permit"].ToString() != "-1" &&
                DateTime.Parse(ds.Tables["data"].Rows[0]["login_to"].ToString()) <= DateTime.Now
                )
            {
                ds.Tables["status"].Rows[0]["status"] = "error";
                ds.Tables["status"].Rows[0]["message"] = "Session Timeout";
            }
            else
            {
                try
                {
                    if ((bool)ds.Tables["data"].Rows[0][acrud] == false)
                    {
                        ds.Tables["status"].Rows[0]["status"] = "error";
                        ds.Tables["status"].Rows[0]["message"] = "Not Authorized";
                    }
                }
                catch (Exception)
                {
                    ds.Tables["status"].Rows[0]["status"] = "error";
                    ds.Tables["status"].Rows[0]["message"] = "Exception Not Authorized";
                }

            }
            return ds;
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using System.Web.Routing;

namespace newHR.Controllers
{
    public static class static_class
    {
        public static ActionResult GetView(
                                            this Controller controller,
                                            string module = "test",
                                            HttpRequestBase httpRequest = null,
                                            RouteData routeData = null
                                            )
        {
            // Use provided parameters or fall back to controller's context
            httpRequest = httpRequest ?? controller.Request;
            routeData = routeData ?? controller.RouteData;

            // Get token from cookies
            string token = httpRequest.Cookies.Get("token")?.Value ?? string.Empty;

            // Get controller and action names from route data
            string controllerName = routeData.Values["controller"]?.ToString();
            string actionName = routeData.Values["action"]?.ToString();

            // Get view information
            string[] viewInfo = GetStatusView(module, token, controllerName, actionName);

            // Set permissions if not login view
            if (viewInfo[0] != "Log")
            {
                controller.ViewBag.perms = static_class.Authrizes(token); /**///ConvertToJsonWithNamedRows(static_class.Authrizes(token), "module_name");
            }

            // Return the appropriate view
            return new ViewResult
            {
                ViewName = $"~/views/{viewInfo[1]}/{viewInfo[0]}.cshtml",
                ViewData = controller.ViewData,
                TempData = controller.TempData
            };
        }

        /************************** crud ************************/
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
        static public DataSet updatebysql(string sql)//for update and delete 
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
        /************************* auth *************************/
        static public DataTable Authrizes(string token)
        {

            string sql =
                $@"SELECT ml.name 'module_name',
                          ISNULL(m.url,'')'module_url',
                          ISNULL([access],'false')  'p_access',	
                          ISNULL([read],'false')	'p_read',	
                          ISNULL([create],'false')	'p_create',	
                          ISNULL([update],'false')	'p_update',	
                          ISNULL([delete],'false')	'p_delete',
                          u.login_to,
                          u.login_permit
                    FROM AK_Users u JOIN 
                         AK_Roles r ON u.RoleId = r.Id JOIN
                         AK_Roles_lines rl ON r.Id = rl.role_id JOIN
                         AK_Modules_lines ml ON rl.module_line_id = ml.id JOIN
                         AK_modules m ON m.id=ml.module_id
                    WHERE u.token='{token}'";

            return static_class.getbysql(sql).Tables["data"];

        }
        static public DataTable o_Authrizes(string token)
        {

            string sql =
                $@"SELECT ml.name 'module_name',
                          ISNULL(m.url,'')'module_url',
                          [access]  'p_access',	
                          [read]	'p_read',	
                          [create]	'p_create',	
                          [update]	'p_update',	
                          [delete]	'p_delete',
                          u.login_to,
                          u.login_permit
                    FROM AK_Users u JOIN 
                         AK_Roles r ON u.RoleId = r.Id JOIN
                         AK_Roles_lines rl ON r.Id = rl.role_id JOIN
                         AK_Modules_lines ml ON rl.module_line_id = ml.id JOIN
                         AK_modules m ON m.id=ml.module_id
                    WHERE u.token='{token}'";

            DataTable dt = new DataTable();
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
                        dt.Rows[0][pure.Rows[i]["module_name"].ToString()] = pure.Rows[i]["p_access"].ToString();
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
                $@"SELECT ml.name 'module_name',
                          ISNULL(m.url,'')'module_url',
                          [access]  'p_access',	
                          [read]	'p_read',	
                          [create]	'p_create',	
                          [update]	'p_update',	
                          [delete]	'p_delete',
                          u.login_to,
                          u.login_permit
                    FROM AK_Users u JOIN 
                         AK_Roles r ON u.RoleId = r.Id JOIN
                         AK_Roles_lines rl ON r.Id = rl.role_id JOIN
                         AK_Modules_lines ml ON rl.module_line_id = ml.id JOIN
                         AK_modules m ON m.id=ml.module_id
                    WHERE u.token='{token}' and
                          ml.name='{module}'";

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
        //check  if page can access or not
        static public bool is_Authenticated(string token)
        {
            string sql =
                $@"SELECT 
                          datediff(MINUTE,GETDATE(),u.login_to)est,
                          u.login_to,
                          u.login_permit
                FROM AK_Users u 
                WHERE u.token='{token}'";

            DataSet ds= static_class.getbysql(sql);
            if (ds.Tables["data"].Rows.Count == 0)
            {
                ds.Tables["status"].Rows[0]["status"] = "error";
                ds.Tables["status"].Rows[0]["message"] = "Not Authorized";
                return false;
            }
            else if (
                ds.Tables["data"].Rows[0]["login_permit"].ToString() != "-1" &&
                DateTime.Parse(ds.Tables["data"].Rows[0]["login_to"].ToString()) <= DateTime.Now
                )
            {
                ds.Tables["status"].Rows[0]["status"] = "error";
                ds.Tables["status"].Rows[0]["message"] = "Session Timeout";
                return false;
            }
            else
            {
                try
                {
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }

            }
            
        }
        static public string[] GetStatusView(string moduleName, string token, string c, string a)
        {

            DataSet ds;
            try
            {
                if (is_Authenticated(token))
                {
                    ds = static_class.is_Authrize(moduleName, token, "p_access");
                    if (ds.Tables[0].Rows[0]["status"].ToString() == "error")
                    {
                        if (ds.Tables[0].Rows[0]["message"].ToString() == "Not Authorized")
                            return new string[] { "_NotAuthorized", "Home" };
                        else
                            return new string[] { "Log", "Home" };
                    }
                    else
                    {
                        return new string[] { a, c };
                    }
                }
                else
                {
                    return new string[] { "Log", "Home" };
                }
            }
            catch (Exception)
            {
                return new string[] { "Log", "Home" };
            }

        }


        /****************************** Queries **********************************/
        static string InsertFromObject(string table, JObject obj)
        {

            string sql1 = "";
            string sql2 = "";
            foreach (JProperty property in obj.Properties())
            {
                string key = property.Name;
                JToken value = property.Value;
                if (property.Value.Type != JTokenType.Array)
                {
                    sql1 += $"{key},";
                    sql2 += $"'{value}',";
                }
            }
            sql1 = sql1.Length > 0 ? sql1.Substring(0, sql1.Length - 1) : sql1;
            sql2 = sql2.Length > 0 ? sql2.Substring(0, sql2.Length - 1) : sql2;

            string sql = $"insert into [{table}]({sql1})values({sql2});";
            return sql;
        }
        static string InsertFromObject(string table, string fkKey, object fkValue, JArray arr)
        {
            string sql1 = $"{fkKey},";
            string sql2 = "";
            //   all keys
            foreach (JProperty property in ((JObject)arr[0]).Properties())
            {
                string key = property.Name;
                if (property.Value.Type != JTokenType.Array)
                {
                    sql1 += $"{key},";
                }
            }
            sql1 = sql1.Length > 0 ? sql1.Substring(0, sql1.Length - 1) : sql1;
            //     all values
            for (int i = 0; i < arr.Count; i++)
            {
                sql2 += $"('{fkValue}',";
                foreach (JProperty property in ((JObject)arr[i]).Properties())
                {
                    JToken value = property.Value;
                    sql2 += $"'{value}',";
                }
                sql2 = sql2.Length > 0 ? sql2.Substring(0, sql2.Length - 1) : sql2;
                sql2 += "),";
            }
            sql2 = sql2.Length > 0 ? sql2.Substring(0, sql2.Length - 1) : sql2;

            string sql = $"insert into [{table}]({sql1})values{sql2};";
            return sql;
        }

        static string UpdateFromObject(string table, string idKey, object idValue, JObject obj)
        {

            string sql1 = "";
            foreach (JProperty property in obj.Properties())
            {
                string key = property.Name;
                JToken value = property.Value;
                if (property.Value.Type != JTokenType.Array)
                {
                    sql1 += $"{key}='{value}',";
                }
            }
            sql1 = sql1.Length > 0 ? sql1.Substring(0, sql1.Length - 1) : sql1;
            //sql2 = sql2.Length > 0 ? sql2.Substring(0, sql2.Length - 1) : sql2;

            string sql = $"update  [{table}] set {sql1} where {idKey}={idValue};";
            return sql;
        }
        static string UpdateFromObject(string table, string fkKey, object fkValue, JArray arr)
        {
            string sql1 = "";
            string sql2 = "";
            string sql = "";
            sql1 = sql1.Length > 0 ? sql1.Substring(0, sql1.Length - 1) : sql1;
            //     all values
            for (int i = 0; i < arr.Count; i++)
            {
                sql2 = "";
                sql1 = $"update [{table}] set ";
                foreach (JProperty property in ((JObject)arr[i]).Properties())
                {
                    string key = property.Name;
                    JToken value = property.Value;
                    if (key.ToLower() != "id") sql1 += $"{key}= '{value}',";
                    else sql2 += $" where {fkKey}={fkValue} and id= {value};";
                }
                sql1 = sql1.Length > 0 ? sql1.Substring(0, sql1.Length - 1) : sql1;
                if (sql2.Contains("id")) sql += sql1 + sql2;
            }
            return sql;
        }

        static string InsertEmptyLine(string table, string fkKey, object fkValue)
        {
            string sql = $"insert into [{table}]({fkKey})values{fkValue};";
            return sql;
        }
        static string DeleteLine(string table, string fkKey, object fkValue)
        {
            string sql = $"delete from [{table}] where {fkKey} = {fkValue};";
            return sql;
        }
    }

}
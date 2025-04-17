using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace newHR.Controllers
{
    public class DBContext
    {

        public DataTable getData(string cmdText)
        {

            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CS"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(cmdText, con))
                    {
                        con.Open();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ahmed kamal=" + ex.Message);
                return null;
            }
            return dt;
        }
        public DataTable getData(SqlCommand cmd)
        {

            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CS"].ConnectionString))
                {

                    using (cmd)
                    {
                        cmd.Connection = con;
                        con.Open();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ahmed kamal=" + ex.Message);
                return null;
            }
            return dt;
        }
        public string toJSON(DataTable table)
        {
            return JsonConvert.SerializeObject(table);
        }
        
        public int exec(SqlCommand com)
        {
            string cs = ConfigurationManager.ConnectionStrings["CS"].ConnectionString;
            try
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();
                    com.Connection = con;
                    com.ExecuteNonQuery();
                    return 1;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ahmed kamal=" + ex.Message);
                return 0;
            }
        }
    }
}
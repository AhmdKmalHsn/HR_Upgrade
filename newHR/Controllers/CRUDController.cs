using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace newHR.Controllers
{
    public class CRUDController : Controller
    {
        // GET: CRUD
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult simplest()
        {
            return View();
        }
        /********************** helpers ****************************/
        
        public DataSet ReadSql(string sql = "")
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["CS"].ConnectionString);
            DataSet dataSet = new DataSet();
            try
            {
                SqlDataAdapter data = new SqlDataAdapter(sql, connection);
                data.Fill(dataSet, "data");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dataSet;
        }
        public string ReadSql(int pageNo = 0, int pageSize = 0, string sql = "")
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["CS"].ConnectionString);
            string json = "{}";
            try
            {
                SqlDataAdapter data = new SqlDataAdapter(sql, connection);
                DataSet dataSet = new DataSet();
                data.Fill(dataSet, (pageNo - 1) < 0 ? 0 : (pageNo - 1) * pageSize, pageSize, "data");
                json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);
            }
            catch (Exception ex)
            {
                json = "{error :\"" + ex.Message + "\",data:[]}";
            }
            return json;
        }
        public string GetTable(int pageNo = 0, int pageSize = 0, string table = "")
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["CS"].ConnectionString);

            string dataSQL = "SELECT * FROM " + table;
            string metaSQL = "SELECT count(*)count FROM " + table;

            // Assumes that connection is a valid SqlConnection object.  
            SqlDataAdapter meta = new SqlDataAdapter(metaSQL, connection);
            SqlDataAdapter data = new SqlDataAdapter(dataSQL, connection);

            DataSet dataSet = new DataSet();
            meta.Fill(dataSet, "meta");
            data.Fill(dataSet, (pageNo - 1) < 0 ? 0 : (pageNo - 1) * pageSize, pageSize, "data");

            //serialize the data 
            string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);
            return json;
        }
        public ActionResult GetByTable(int pageNo = 0, int pageSize = 0, string table = "")
        {
            return Content(GetTable(pageNo, pageSize, table), "application/json; charset=utf-8");
        }
        public ActionResult GetBySql(int pageNo = 0, int pageSize = 0, string sql = "")
        {
            return Content(ReadSql(pageNo, pageSize, sql), "application/json; charset=utf-8");
        }
        /********************* api json **************************/
        public ActionResult GetUsers(int pageNo = 0, int pageSize = 0)
        {
            string sql = "select * from users2";
            return Content(ReadSql(pageNo, pageSize, sql), "application/json; charset=utf-8");
        }
        public ActionResult GetSQL(string t="",string json="")
        {
             json = "{'a':'aaa','b':'bbb','c':'ccc'}";
            
            JObject resJson = JObject.Parse(json);
            resJson["name"] = "ahmed kamal";
            var v = resJson.CreateReader();
            
            return Content(resJson["name"].ToString(), "application/json; charset=utf-8");
        }
        /************************ test area ************************/
        public string ConvertDataTabletoString(DataTable dt)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            foreach (DataRow dr in dt.Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    row.Add(col.ColumnName, dr[col]);
                }
                rows.Add(row);
            }
            return serializer.Serialize(rows);
        }
    }
}
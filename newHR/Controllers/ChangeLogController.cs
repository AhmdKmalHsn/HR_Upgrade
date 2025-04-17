using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace newHR.Controllers
{
    public class ChangeLogController : Controller
    {
        // GET: ChangeLog
        public ActionResult Index()
        {
            return View();
        }
        DBContext db = new DBContext();
        public JsonResult getAll() {
            SqlCommand cmd = new SqlCommand("select * from changeLog");
            return Json(db.toJSON(db.getData(cmd)), JsonRequestBehavior.AllowGet);
        }
        public JsonResult getAllByDate(DateTime f,DateTime t)
        {
            SqlCommand cmd = new SqlCommand("select * from changeLog where cast(timedone as date)>=@f and cast(timedone as date)<=@t");
            cmd.Parameters.Add("@f", SqlDbType.Date).Value = f.Date;
            cmd.Parameters.Add("@t", SqlDbType.Date).Value = t.Date;
            return Json(db.toJSON(db.getData(cmd)), JsonRequestBehavior.AllowGet);
        }
        public JsonResult getDetails(int fno)
        {
            SqlCommand cmd = new SqlCommand("select * from ChangelogDetails where changeLogId=@fno");
            cmd.Parameters.Add("@fno", SqlDbType.Int).Value = fno;
            return Json(db.toJSON(db.getData(cmd)), JsonRequestBehavior.AllowGet);
        }
        public JsonResult createLog(string user, string ip,string crud,string table,string value1, string value2,bool isUpdate)
        {
            string[] val1 = Newtonsoft.Json.JsonConvert.DeserializeObject<string[]>(value1);
            string[] val2 = Newtonsoft.Json.JsonConvert.DeserializeObject<string[]>(value2);
            string sql = @"
insert into changeLog([user],ip,crud,tableName)
values(@user,@ip,@crud,@table)
insert into ChangelogDetails(columnName,Value1,Value2,changeLogId)
values ";
            if (isUpdate==false)
            {
                for (int i = 0; i < val1.Length - 1; i++)
                {
                    if (i % 2 == 0)
                    {
                        if (i != val1.Length - 2)
                            sql += "('" + val1[i] + "','" + val1[i + 1] + "',''" + ",SCOPE_IDENTITY())" + ",";
                        else
                            sql += "('" + val1[i] + "','" + val1[i + 1] + "',''" + ",SCOPE_IDENTITY())";
                    }
                }
            }
            else
            {
                int j = 0;
                for (int i = 1; i < val1.Length; i+=2)
                {
                    if (val1[i] != val2[i])
                    {
                        j++;
                    if (j==1)
                    {
                        sql += "('" + val1[i-1] + "','" + val1[i] + "','" + val2[i] + "',SCOPE_IDENTITY())";
                    }                           
                   else
                    {
                        sql += ",('" + val1[i-1] + "','" + val1[i] + "','" + val2[i] + "',SCOPE_IDENTITY())";
                    }
                    }      
                    
                }
            }//+"('"++"','"++"','"++"','" + ",SCOPE_IDENTITY())" + ",";
      
            SqlCommand cmd = new SqlCommand(sql);
            cmd.Parameters.Add("@user", SqlDbType.NVarChar).Value = user;
            cmd.Parameters.Add("@ip", SqlDbType.NVarChar).Value = ip;
            cmd.Parameters.Add("@crud", SqlDbType.NVarChar).Value = crud;
            cmd.Parameters.Add("@table", SqlDbType.NVarChar).Value = table;
            return Json(db.exec(cmd), JsonRequestBehavior.AllowGet);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace newHR.Controllers
{
    public class SQLController : Controller
    {
        // GET: SQL
        DBContext db = new DBContext();
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult Excute(string sql)
        {
            SqlCommand com = new SqlCommand(sql);
            com.CommandType = CommandType.Text;
            return Json(db.exec(com), JsonRequestBehavior.AllowGet);
        }
        public JsonResult Read(string sql)
        {
            SqlCommand com = new SqlCommand(sql);
            return Json(db.toJSON(db.getData(com)), JsonRequestBehavior.AllowGet);
        }
        public JsonResult ReadSQL(string sql)
        {
            SqlCommand com = new SqlCommand(sql);
            return new JsonResult() { Data = db.toJSON(db.getData(com)), JsonRequestBehavior = JsonRequestBehavior.AllowGet,MaxJsonLength=Int32.MaxValue };
        }
    }
}
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
            if (static_class.is_Authenticated(Request.Cookies.Get("token").Value))
            {
                SqlCommand com = new SqlCommand(sql);
                com.CommandTimeout = 60;
                return Json(db.toJSON(db.getData(com)), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new {error="Not Authenticated !?" }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult ReadSQL(string sql)
        {
            SqlCommand com = new SqlCommand(sql);
            return new JsonResult() { Data = db.toJSON(db.getData(com)), JsonRequestBehavior = JsonRequestBehavior.AllowGet,MaxJsonLength=Int32.MaxValue };
        }
    }
}
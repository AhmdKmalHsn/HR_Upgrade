using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace newHR.Controllers
{
    public class PostingPeriodController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        /**************************db Period**********************/
        DBContext db = new DBContext();
        public JsonResult getPeriods(int id = 0)
        {
            if (id == 0)
            {
                string sql = @"select Id,DateFrom,DateTo,status from PostingPeriods";
                return Json(db.toJSON(db.getData(sql)), JsonRequestBehavior.AllowGet);
            }
            else
            {
                string sql = @"select Id,DateFrom,DateTo,status from PostingPeriods where Id=@id";
                SqlCommand cmd = new SqlCommand(sql);
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                return Json(db.toJSON(db.getData(cmd)), JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult checkPeriod(DateTime date)
        {     
                string sql = @"select count(status)c from PostingPeriods where @f>=DateFrom and @f<=DateTo and status='c'";
                SqlCommand cmd = new SqlCommand(sql);
                cmd.Parameters.Add("@f", SqlDbType.Date).Value = date;
                return Json(db.toJSON(db.getData(cmd)), JsonRequestBehavior.AllowGet);
           
        }
        public JsonResult createPeriod(DateTime DateFrom, DateTime DateTo, string status)
        {
            string sql = @"
	            insert into PostingPeriods(DateFrom,DateTo,status)
                values(@DateFrom,@DateTo,@status)";
            SqlCommand cmd = new SqlCommand(sql);
            cmd.Parameters.Add("@DateFrom", SqlDbType.Date).Value = DateFrom;
            cmd.Parameters.Add("@DateTo", SqlDbType.Date).Value = DateTo;
            cmd.Parameters.Add("@status", SqlDbType.NVarChar).Value = status;
            
            return Json(db.exec(cmd), JsonRequestBehavior.AllowGet);
        }
        public JsonResult updatePeriod(int Id, DateTime DateFrom, DateTime DateTo, string status)
        {
            string sql = @"
	            update PostingPeriods set
                DateFrom=@DateFrom,DateTo=@DateTo,status=@status
                where id=@id";
            SqlCommand cmd = new SqlCommand(sql);
            cmd.Parameters.Add("@DateFrom", SqlDbType.Date).Value = DateFrom;
            cmd.Parameters.Add("@DateTo", SqlDbType.Date).Value = DateTo;
            cmd.Parameters.Add("@status", SqlDbType.NVarChar).Value = status;
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = Id;
            return Json(db.exec(cmd), JsonRequestBehavior.AllowGet);
        }
    }
}

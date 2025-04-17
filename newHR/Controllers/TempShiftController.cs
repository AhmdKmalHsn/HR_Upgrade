using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace newHR.Controllers
{
    
    public class TempShiftController : Controller
    {
        db.TempShiftDB t = new db.TempShiftDB();
        DBContext db = new DBContext();
        // GET: TempShift
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult All()
        {
            return View();
        }
        public ActionResult PrivateShift()
        {
            return View();
        }
        public ActionResult Entry() {
            return View();
        }
        public JsonResult getdata( int Id)
        {
            return Json(t.getTempShift(Id), JsonRequestBehavior.AllowGet);
        }
        public JsonResult getAlldata(int DeptId)
        {
           
            return Json(t.getAllTempShift(DeptId), JsonRequestBehavior.AllowGet);
        }
        public JsonResult getEntry()
        {
            string sql = @"select e.KnownAs'name',e.FileNumber,ts.Id,DateFrom,DateTo,ShiftFrom,ShiftTo,ShiftHours 
from tempShiftEntry te,Employees e,TempShifts ts 
where te.fileNumber=e.FileNumber and
      te.tempShiftId=ts.Id";
            return Json(db.toJSON(db.getData(sql)), JsonRequestBehavior.AllowGet);
        }

        public JsonResult delete(int Id)
        {
            return Json(t.delete(Id), JsonRequestBehavior.AllowGet);
        }
        public JsonResult create(int ShiftId,DateTime  DateFrom,DateTime DateTo,string ShiftFrom,string ShiftTo,float ShiftHours,int DepartmentId,bool IsPrivate)
        {
            return Json(t.create(ShiftId,DateFrom,DateTo,ShiftFrom,ShiftTo,ShiftHours,DepartmentId, IsPrivate), JsonRequestBehavior.AllowGet);
        }
        public JsonResult update(int Id,int ShiftId, DateTime DateFrom, DateTime DateTo, string ShiftFrom, string ShiftTo, float ShiftHours, int DepartmentId, bool IsPrivate)
        {
            return Json(t.update(Id, ShiftId, DateFrom, DateTo, ShiftFrom, ShiftTo, ShiftHours, DepartmentId, IsPrivate), JsonRequestBehavior.AllowGet);
        }

        public JsonResult updatePrivcy(int fn,int p) {
           
            string sql = @"update BasicBayWorks set IsPrivate=@p 
            where EmployeeId=(select id from Employees where FileNumber=@fn)";
            SqlCommand com = new SqlCommand(sql);
            com.Parameters.Add("@p", SqlDbType.Int).Value = p;
            com.Parameters.Add("@fn", SqlDbType.Int).Value = fn;
            return Json(db.exec(com), JsonRequestBehavior.AllowGet);
        }
    }
}
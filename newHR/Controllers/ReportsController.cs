using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace newHR.Controllers
{
    public class ReportsController : Controller
    {
        // GET: reports
        db.Report r = new db.Report();
        db.additionDB adb = new db.additionDB();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Attendance()
        {
            return View();
        }
        public ActionResult All()
        {
            return View();
        }
        public ActionResult EditPrivate()
        {
            return View();
        }
        public ActionResult report()//route 
        {
            return View();
        }
        public ActionResult payments()//خصومات
        {
            return View();
        }
        public ActionResult paymentsDept()//خصومات
        {
            return View();
        }
        public ActionResult Loans()//سلف
        {
            return View();
        }
        public ActionResult Employees()//بيانات العمال
        {
            return View();
        }
        public ActionResult Sedulity()
        {
            return View();
        }
        public ActionResult AttendanceTotal()//الحضور والانصراف لفترة معينة
        {
            return View();
        }
        public ActionResult CutsReport()//الحضور والانصراف لفترة معينة
        {
            return View();
        }

        /**************************************************/
        public JsonResult getAttendBydepartment(DateTime date,int dept) {
            return Json(r.getAttendByDepartment(date,dept), JsonRequestBehavior.AllowGet);
        }
        public JsonResult getAllBydepartment(DateTime date, int dept)//تقرير مجمع
        {
            return Json(r.getAllByDepartment(date, dept), JsonRequestBehavior.AllowGet);
        }
        public JsonResult createAddition(DateTime from, int fileNo, float hoursNo, int offset)
        {
            return Json(adb.create(from, fileNo,hoursNo,offset), JsonRequestBehavior.AllowGet);
        }
        public JsonResult createVacation(DateTime from, int fileNo, float dayPart,int type)
        {
            return Json(r.createVacation(from, fileNo, dayPart,type), JsonRequestBehavior.AllowGet);
        }
        public JsonResult deleteVacation(DateTime from, int fileNo)
        {
            return Json(r.deleteVacation(from, fileNo), JsonRequestBehavior.AllowGet);
        }
        public JsonResult deleteAddition(DateTime from, int fileNo)
        {
            return Json(adb.deleteAddition(from, fileNo), JsonRequestBehavior.AllowGet);
        }
        /****************** json new *******************/
        DBContext database = new DBContext();      
        public JsonResult minus()//مفردات المرتب
        {
            string sql = @"
	select distinct
		  cast(e.FileNumber as int)'رقم الملف',
		  e.KnownAs'الأسم',
		  s.Name'الحالة',
		  dept.Name 'القسم',
		  isNull(b.TotalSalary,0)'الاساسي',
		  isNull(b.RegularityIncentive,0)'الانتظام',
		  b.ConstValue'قيمة ثابتة',
		  b.NumberOfDays'عدد الأيام',
		  isNull(b.ExpensiveLivingConditons,0)'غلاء المعيشة',
		  isNull(b.SkillIncentive,0)'المهارة',
		  isNull(b.IncentiveIncentiveForAbsence,0)'الادارة',

		  isNull(b.TotalSalary,0)+isNull(b.RegularityIncentive,0)+
		  isNull(b.ExpensiveLivingConditons,0)+isNull(b.SkillIncentive,0)+
		  isNull(b.IncentiveIncentiveForAbsence,0)'اجمالي المستحق',

		  isNull(i.EmployeeFixedSalary*.11 ,0)'خصم التأمين',
		  
		  isNull(b.TotalSalary,0)+isNull(b.RegularityIncentive,0)+
		  isNull(b.ExpensiveLivingConditons,0)+isNull(b.SkillIncentive,0)+
		  isNull(b.IncentiveIncentiveForAbsence,0)-isNull(i.EmployeeFixedSalary*.11 ,0)
		  'صافي المرتب'
		  
	from Employees E 
	left join Departements dept on dept.Id=e.DepartementId 
	left join Personals p on e.PersonalId=p.Id 
	left join Status s on s.Id =p.StatusId
	left join BasicBayWorks b on E.Id=b.EmployeeId
	 left join(select max(id)id,EmployeeId from InsuranceDetails group by EmployeeId)q on q.EmployeeId=e.id 
          join InsuranceDetails i on i.id=q.id
	";
            return Json(database.toJSON(database.getData(sql)), JsonRequestBehavior.AllowGet);
        }
        public JsonResult sanctions_total(DateTime f,DateTime t)//خصومات
        {
            SqlCommand cmd = new SqlCommand(@"
                      select --e.FileNumber,
       --d.Id,
	   d.Name,
	   --e.KnownAs,
	   --cast(p.Date as date)date,
	   --b.TotalSalary,
	   sum(p.DaysNumber)days,
	   sum(p.Amount)amount 
from deductions p,
     Employees e,
	 Departements d,
	 BasicBayWorks b
where  (p.PaymentDeductionId=4 or p.PaymentDeductionId=3)and 
       p.EmployeeId=e.id and 
	   e.id=b.EmployeeId and 
	   cast(p.Date as date)>=@f and cast(p.Date as date)<=@t and 
	   d.Id=e.DepartementId
group by(d.Name)  
                
                ");
            cmd.Parameters.Add("@f", SqlDbType.Date).Value = f.Date;
            cmd.Parameters.Add("@t", SqlDbType.Date).Value = t.Date;
            return Json(database.toJSON(database.getData(cmd)), JsonRequestBehavior.AllowGet);
        }
        public JsonResult sanctions_details(DateTime f, DateTime t)//خصومات
        {
            SqlCommand cmd = new SqlCommand(@"
                    select e.FileNumber,
       --d.Id,
	   d.Name,
	   e.KnownAs,
	   cast(p.Date as date)date,
	   b.TotalSalary,
	   p.DaysNumber,
	   p.Amount 
from deductions p,
     Employees e,
	 Departements d,
	 BasicBayWorks b
where  (p.PaymentDeductionId=4  or p.PaymentDeductionId=3) and 
       p.EmployeeId=e.id and 
	   e.id=b.EmployeeId and 
	   cast(p.Date as date)>=@f and cast(p.Date as date)<=@t and 
	   d.Id=e.DepartementId
              ");
            cmd.Parameters.Add("@f", SqlDbType.Date).Value = f.Date;
            cmd.Parameters.Add("@t", SqlDbType.Date).Value = t.Date;
            return Json(database.toJSON(database.getData(cmd)), JsonRequestBehavior.AllowGet);
        }

    }
}
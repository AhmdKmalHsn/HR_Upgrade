using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace newHR.Controllers
{
    public class NotiController : Controller
    {
        // GET: Noti
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult InOutNoti()
        {
            DBContext db = new DBContext();
            SqlCommand com = new SqlCommand();
            string sql = @"select q.*,e.FileNumber,e.KnownAs'name',(select name from Departements where id=DepartementId)'dept' from
(
select a.*,abc.DayPart 
from Mix_Att_Mission a  full join 
	 Absences abc on a.dateFrom=abc.DateFrom and a.EmployeeId=abc.EmployeeId
 where a.DateFrom < dateadd(day,-1,GETDATE()) and DATENAME(DW,a.dateFrom)<>'friday'
)q join Employees e on q.EmployeeId=e.id
where (TimeFrom is null or TimeTo is null) and (DayPart<>1 or DayPart is null) and e.DepartementId in (select Id from Departements where [group]=0)
and
(
    ( DATEPART(day,GETDATE())<26 and  
	 datefrom>=cast( cast( 
	(case when DATEPART(MONTH,GETDATE())=1 
	     then DATEPART(YEAR,GETDATE())-1 
	     else DATEPART(YEAR,GETDATE()) 
	end) 
		 as varchar(4))+
		 '-'+cast(DATEPART(MONTH,dateadd(month,-1,GETDATE())) as varchar(2))+'-'+'26' as datetime))
    or
    ( DATEPART(day,GETDATE())>=26 and  datefrom>=cast(cast(DATEPART(YEAR,GETDATE())as varchar(4))+'-'+cast(DATEPART(MONTH,GETDATE()) as varchar(2))+'-'+'26' as datetime))
)";
            com.CommandText = sql;
            return Json(db.toJSON(db.getData(com)), JsonRequestBehavior.AllowGet);
        }
    }
}
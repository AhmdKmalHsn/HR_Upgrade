using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using newHR.Models;

namespace newHR.Controllers
{
    public class DeferredVacationsDB
    {
        string cs = ConfigurationManager.ConnectionStrings["CS"].ConnectionString;
        public List<VacationsModel> ListAll()
        {
            List<VacationsModel> lst = new List<VacationsModel>();
            string sql = @"
SELECT     
		   ISNULL(E.FileNumber,0)FileNumber,
		   ISNULL(E.KnownAs,0) 'name',
		   ISNULL(bb.EmployeeId,0)EmployeeId,
		   d.Name'dept',
		   ISNULL(AB.vacation,0)vacation,
		   ISNULL(bb.NumberOfStageVacations,0) 'deferredVacation',
		   bb.VacationDeferredDate   'deferredDate',
		   ISNULL(dbo.Ak_Monthly_Balance(VacationDeferredDate),0)
		   +ISNULL(bb.NumberOfStageVacations,0)'deferred',
		   bb.deferred'olddeferred',
		   ISNULL(dbo.Ak_Monthly_Balance(VacationDeferredDate),0)
		   +ISNULL(bb.NumberOfStageVacations,0)
		   -isnull(ab.vacation,0) 'availableVacation',
		   bb.deferred-isnull(ab.vacation,0) 'oldavailableVacation'
	FROM
	(
	select EmployeeId,
		   VacationDeferredDate,
		   NumberOfStageVacations,
	ISNULL(NumberOfStageVacations,0) deferred
	from BasicBayWorks 
	)BB left join
   (
	select sum(
	       case when a.DayPart>0 then a.DayPart else 
	       cast(
		   case when (timeFrom=null and timeto=null )or HoursNo=0 then 1
		        when s.DailyHours<>0 then HoursNo/s.DailyHours
			    else 0
		   end as decimal(10,2))
		   end
		   )  vacation,
		   FileNumber,
		   a.EmployeeId
	from Absences a join 
	     BasicBayWorks b  on a.EmployeeId=b.EmployeeId join
		 Shifts s on b.ShiftId=s.ShiftId join 
		 Employees e on e.Id=a.EmployeeId
	where (a.AbsenceTypeId=3 or a.AbsenceTypeId=7) --and Payment=1
		  and a.DateFrom>=VacationDeferredDate and a.DateFrom<=getdate()
	group by FileNumber,A.EmployeeId

	)AB on ab.EmployeeId=bb.EmployeeId
	join Employees E on bb.EmployeeId=E.id join Personals P on p.Id=e.PersonalId join Departements d on e.DepartementId=d.Id
	where p.StatusId<>3 
	order by cast(e.filenumber as int)
	";
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand(sql, con);
                com.CommandType = CommandType.Text;
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    lst.Add(new VacationsModel
                    {
                        fileNumber =  rdr["FileNumber"] != "" ? Convert.ToInt32(rdr["FileNumber"]) : 0,
                        name = rdr["name"].ToString(),
                        abcenceDays = rdr["vacation"] != "" ? Convert.ToDecimal(rdr["vacation"]) : 0,
                        departmentId = rdr["Dept"].ToString(),
                        availableDays = rdr["availableVacation"] != "" ? Convert.ToDecimal(rdr["availableVacation"]) : 0,
                        deferredDate = rdr["deferredDate"].ToString(),
                        deferredDays = rdr["deferredVacation"] != "" ? Convert.ToDecimal(rdr["deferredVacation"]) : 0
                    });
                }
                return lst;
            }
        }
        public List<VacationsModel> GetById(int Id)
        {
            List<VacationsModel> lst = new List<VacationsModel>();
            string sql = @"
SELECT ISNULL(AB.vacation,0)vacation,
		   ISNULL(E.FileNumber,0)FileNumber,
		   ISNULL(E.KnownAs,0) 'name',
		   ISNULL(bb.EmployeeId,0)EmployeeId,
		   d.Name'dept',
		   bb.NumberOfStageVacations 'deferredVacation',
		   bb.VacationDeferredDate   'deferredDate',
		   bb.deferred-isnull(ab.vacation,0) 'availableVacation'
	FROM
	(
	select EmployeeId,
		   VacationDeferredDate,
		   NumberOfStageVacations,
	ISNULL(
	case 
		 when DATEPART(DAY,VacationDeferredDate)>=26 and DATEPART(DAY,GETDATE())>=26 then DATEDIFF(MONTH,VacationDeferredDate,getdate())-1+1
		 when DATEPART(DAY,VacationDeferredDate)< 26 and DATEPART(DAY,GETDATE())>=26 then DATEDIFF(MONTH,VacationDeferredDate,getdate())+1
		 when DATEPART(DAY,VacationDeferredDate)>=26 and DATEPART(DAY,GETDATE())< 26 then DATEDIFF(MONTH,VacationDeferredDate,getdate())-1
		 when DATEPART(DAY,VacationDeferredDate)< 26 and DATEPART(DAY,GETDATE())< 26 then DATEDIFF(MONTH,VacationDeferredDate,getdate())
	end*1.75,1.75)+ISNULL(NumberOfStageVacations,0) deferred
	from BasicBayWorks 
	)BB left join
   (
	select sum(
	       case when a.DayPart>0 then a.DayPart else 
	       cast(
		   case when (timeFrom=null and timeto=null )or HoursNo=0 then 1
		        when s.DailyHours<>0 then HoursNo/s.DailyHours
			    else 0
		   end as decimal(10,2))
		   end
		   )  vacation,
		   FileNumber,
		   a.EmployeeId
	from Absences a join 
	     BasicBayWorks b  on a.EmployeeId=b.EmployeeId join
		 Shifts s on b.ShiftId=s.ShiftId join 
		 Employees e on e.Id=a.EmployeeId
	where (a.AbsenceTypeId=3 or a.AbsenceTypeId=7) and Payment=1
		  and a.DateFrom>=VacationDeferredDate and a.DateFrom<=getdate()
	group by FileNumber,A.EmployeeId

	)AB on ab.EmployeeId=bb.EmployeeId
	join Employees E on bb.EmployeeId=E.id join Personals P on p.Id=e.PersonalId join Departements d on e.DepartementId=d.Id
	where p.StatusId<>3 and e.filenumber=@FN 
	";
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand(sql, con);
                com.Parameters.Add("@FN", SqlDbType.Int).Value = Id;
                com.CommandType = CommandType.Text;
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    lst.Add(new VacationsModel
                    {
                        fileNumber = rdr["FileNumber"] != "" ? Convert.ToInt32(rdr["FileNumber"]) : 0,
                        name = rdr["name"].ToString(),
                        abcenceDays = rdr["vacation"] != "" ? Convert.ToDecimal(rdr["vacation"]) : 0,
                        departmentId = rdr["Dept"].ToString(),
                        availableDays = rdr["availableVacation"] != "" ? Convert.ToDecimal(rdr["availableVacation"]) : 0,
                        deferredDate = rdr["deferredDate"].ToString(),
                        deferredDays = rdr["deferredVacation"] != "" ? Convert.ToDecimal(rdr["deferredVacation"]) : 0
                    });
                }
                return lst;
            }
        }

        public List<truant> VactionInMonth(int M, int Y)
        {
            DateTime d1 = new DateTime(M > 1 ? Y : Y - 1, M > 1 ? M - 1 : 12, 26);
            DateTime d2 = new DateTime(Y, M, 25);
            List<truant> lst = new List<truant>();
            string sql = @"select sum(
	       case when a.DayPart>0 then a.DayPart else 
	       cast(
		   case when (timeFrom=null and timeto=null )or HoursNo=0 then 1
		        when s.DailyHours<>0 then HoursNo/s.DailyHours
			    else 0
		   end as decimal(10,2))
		   end
		   )  vacation,
		   FileNumber,
		   a.EmployeeId,
		   e.KnownAs'name'
	from Absences a join 
	     BasicBayWorks b  on a.EmployeeId=b.EmployeeId join
		 Shifts s on b.ShiftId=s.ShiftId join 
		 Employees e on e.Id=a.EmployeeId
	where (a.AbsenceTypeId=3 or a.AbsenceTypeId=7) and Payment=1
		  and a.DateFrom>=@f and a.DateFrom<=@t
	group by FileNumber,A.EmployeeId, e.KnownAs
 ";
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand(sql, con);
                com.CommandType = CommandType.Text;
                com.Parameters.Add("@F", SqlDbType.Date).Value = d1.Date;
                com.Parameters.Add("@T", SqlDbType.Date).Value = d2.Date;

                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    lst.Add(new truant
                    {
                        fileNumber = Convert.ToInt32(rdr["FN"].ToString()),
                        name = rdr["name"].ToString(),
                        vacation = Convert.ToDecimal(rdr["vacation"].ToString())
                    });
                }
                return lst;
            }
        }

        public int updateDate(int FN, DateTime d)
        {
            string sql = @"update BasicBayWorks set VacationDeferredDate=@DT
                         where id=@Id
                          ";
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand(sql, con);
                com.CommandType = CommandType.Text;
                com.Parameters.Add("@Id", SqlDbType.Int).Value = FN;
                com.Parameters.Add("@DT", SqlDbType.Date).Value = d.Date;
                com.ExecuteNonQuery();
            }
            return 1;
        }
        public string selectDate(int FN)
        {
            string sql = @"select convert(varchar,VacationDeferredDate,23)d from BasicBayWorks where id=@Id";
            string d;
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand(sql, con);
                com.CommandType = CommandType.Text;
                com.Parameters.Add("@Id", SqlDbType.Int).Value = FN;
                d = com.ExecuteScalar().ToString();
            }
            return d;
        }
    }
    public class DeferredVacationsController : Controller
    {
        // GET: DeferredVacations
        DeferredVacationsDB dv = new DeferredVacationsDB();
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult ListAll()
        {
            return Json(dv.ListAll(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult VacationInMonth(int M, int Y)
        {
            return Json(dv.VactionInMonth(M, Y), JsonRequestBehavior.AllowGet);

        }
        public JsonResult updateDate(int Id, DateTime d)
        {
            return Json(dv.updateDate(Id, d), JsonRequestBehavior.AllowGet);
        }
        public JsonResult selectDate(int Id)
        {
            return Json(dv.selectDate(Id), JsonRequestBehavior.AllowGet);
        }
    }
}
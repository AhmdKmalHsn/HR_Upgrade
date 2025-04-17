using newHR.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace newHR.Controllers
{
  
    public class TruantController : Controller
    {
        
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AttendanceAll()
        {
            return View();
        }
        public ActionResult Details()
        {
            return View();
        }
        public ActionResult AttendanceNew()
        { 
            return View();
        }
        public ActionResult AttendanceTest()
        {
            return View();
        }
        public ActionResult SalaryIndex()
        {
            return View();
        }
        public ActionResult SalarySheet()
        {
            return View();
        }
        public ActionResult Abscence()
        {
            return View();
        }
        public ActionResult Attendance()
        {
            return View();
        }
        public JsonResult LateDetails(string fileNo ,int M, int Y)
        {
            return Json(ListLateByFileNo(fileNo,M,Y), JsonRequestBehavior.AllowGet);
        }
        DBContext database = new DBContext();
        public JsonResult SalaryFN2(string fileNo, int M, int Y)
        {
            DateTime d1 = new DateTime(M > 1 ? Y : Y - 1, M > 1 ? M - 1 : 12, 26);
            DateTime d2 = new DateTime(Y, M, 25);
            string sql = @"WITH CTE_Months
AS
(
    SELECT @f dates
    UNION ALL
    SELECT DATEADD(DAY,1,dates)
    FROM CTE_Months
    WHERE DATEADD(DAY,1,dates) <= @t
)
SELECT c.day,
       c.dates,
       c.FNO,
       emp.KnownAs'name',
	   cast(bb.TotalSalary/30 as decimal(10,2))'dailySalary',
	   cast(bb.TotalSalary/30/shft.DailyHrs as decimal(10,2))'hourSalary',
	   cast(bb.TotalSalary/30/t.DailyHours as decimal(10,2))'hourSalary1',
       t.TIMEFROM,
       t.TIMETO,
       t.ShiftId,
       case when t.DailyHours is null then shft.DailyHrs else t.DailyHours end 'DailyHours',
	   Late,
	   case 
	   when t.ShiftId =17 then
	   case 
	       when (late<=3 and late>=0 )or (late-[.25]<=7 and late-[.25]>=0 )or(late-[.5]<=9 and late-[.5]>=0 ) then 0
	       when (late<=22 and late>=4)or (late-[.25]<=24 and late-[.25]>=8)or(late-[.5]<=22 and late-[.5]>=10)then .25 
	       when (late<=39 and late>=23)or (late-[.25]<=37 and late-[.25]>=25)or(late-[.5]<=33 and late-[.5]>=23)then .50
	       when (late<=52 and late>=40)or (late-[.25]<=48 and late-[.25]>=38)or(late-[.5]<=52 and late-[.5]>=34)then .75
	       when (late<=63 and late>=53)or (late-[.25]<=67 and late-[.25]>=49)or(late-[.5]<=69 and late-[.5]>=53)then 1

	    end
	 else
		case 
	       when (late<=15 and late>=0 )or (late-[.25]<=9 and late-[.25]>=0 )or(late-[.5]<=15 and late-[.5]>=0 ) then 0
	       when (late<=22 and late>=16)or (late-[.25]<=22 and late-[.25]>=10)or(late-[.5]<=22 and late-[.5]>=10)then .25 
	       when (late<=39 and late>=23)or (late-[.25]<=45 and late-[.25]>=23)or(late-[.5]<=45 and late-[.5]>=23)then .50
	       when (late<=52 and late>=40)or (late-[.25]<=52 and late-[.25]>=46)or(late-[.5]<=52 and late-[.5]>=46)then .75
	       when (late<=75 and late>=53)or (late-[.25]<=75 and late-[.25]>=53)or(late-[.5]<=75 and late-[.5]>=53)then 1
	    end 
	   end lateValue,
	   ISNULL(attend,0)'attend',
	  (t.DailyHours*60)-(attend+Late)'Leave',
	  case 
	       when ((t.DailyHours*60)-(attend+Late)<=7 and (t.DailyHours*60)-(attend+Late)>=0 )or ((t.DailyHours*60)-(attend+Late)-[.25]<=7 and (t.DailyHours*60)-(attend+Late)-[.25]>=0 )or((t.DailyHours*60)-(attend+Late)-[.5]<=7 and (t.DailyHours*60)-(attend+Late)-[.5]>=0 ) then 0
	       when ((t.DailyHours*60)-(attend+Late)<=19 and (t.DailyHours*60)-(attend+Late)>=8)or ((t.DailyHours*60)-(attend+Late)-[.25]<=19 and (t.DailyHours*60)-(attend+Late)-[.25]>=8)or((t.DailyHours*60)-(attend+Late)-[.5]<=19 and (t.DailyHours*60)-(attend+Late)-[.5]>=8)then .25 
	       when ((t.DailyHours*60)-(attend+Late)<=37 and (t.DailyHours*60)-(attend+Late)>=20)or ((t.DailyHours*60)-(attend+Late)-[.25]<=37 and (t.DailyHours*60)-(attend+Late)-[.25]>=20)or((t.DailyHours*60)-(attend+Late)-[.5]<=37 and (t.DailyHours*60)-(attend+Late)-[.5]>=20)then .50
	       when ((t.DailyHours*60)-(attend+Late)<=45 and (t.DailyHours*60)-(attend+Late)>=38)or ((t.DailyHours*60)-(attend+Late)-[.25]<=45 and (t.DailyHours*60)-(attend+Late)-[.25]>=38)or((t.DailyHours*60)-(attend+Late)-[.5]<=45 and (t.DailyHours*60)-(attend+Late)-[.5]>=38)then .75
	       when ((t.DailyHours*60)-(attend+Late)<=67 and (t.DailyHours*60)-(attend+Late)>=46)or ((t.DailyHours*60)-(attend+Late)-[.25]<=67 and (t.DailyHours*60)-(attend+Late)-[.25]>=46)or((t.DailyHours*60)-(attend+Late)-[.5]<=67 and (t.DailyHours*60)-(attend+Late)-[.5]>=46)then 1
		   when(salary.DepartementId=324 and ((t.DailyHours*60)-(attend+Late)<=80 and (t.DailyHours*60)-(attend+Late)>=68)or ((t.DailyHours*60)-(attend+Late)-[.25]<=80 and (t.DailyHours*60)-(attend+Late)-[.25]>=68)or((t.DailyHours*60)-(attend+Late)-[.5]<=80 and (t.DailyHours*60)-(attend+Late)-[.5]>=68))then 1.25
	    end leaveValue,
		NoOfHour addition,
	  	bb.Overtime,
        ABM.cnt mission,
	   
	   
	   cast((case when (ab.TimeFrom ='' or ab.TimeTo='')  then 60*case when t.DailyHours is null then shft.DailyHrs else t.DailyHours end
	        when (ab.TimeFrom < ab.TimeTo) then DATEDIFF(MINUTE,ab.timefrom,ab.TimeTo)
	        when (ab.TimeFrom > ab.TimeTo) then DATEDIFF(MINUTE,ab.TimeTo,AB.TimeFrom)
	   end/cast(60 as float))/case when t.DailyHours is null then shft.DailyHrs else t.DailyHours end as decimal(10,2)) absenceValue,

	   ab1.DayPart,
	   ab1.AbsenceTypeId,
	   
case  when (cast(ab.TimeFrom as time)<cast(ab.TimeTo as time)) and (cast(t.TIMEFROM as time)>cast(ab.TimeFrom as time) and cast(t.TIMEFROM as time)<cast(ab.TimeTo as time)) then '1'				     
	        when (cast(ab.TimeFrom as time)>cast(ab.TimeTo as time)) and (cast(t.TIMEFROM as time)<cast(ab.TimeFrom as time) and cast(t.TIMEFROM as time)>cast(ab.TimeTo as time)) then '1'
			else '0'
	   end 'inbetween',
	   v.AttendanceValue,
	   salary.*,
	   vacDef.availableVacation
FROM 
(
select DATENAME(WEEKDAY, dates) 'day',cast (dates as date)'dates',
       @FNO 'FNO' 
from CTE_Months
)C left join 
            (
			  select * from Ak_3STAGES_FUNC_V2(@f, @t,@fno)where(select top(1) IsPrivate from BasicBayWorks where EmployeeId=(select id from Employees where FileNumber=@fno))=1
			   union All
			  select * from Ak_3STAGES_FUNC_ALL_V2(@f, @t,@fno) where (select top(1) IsPrivate from BasicBayWorks where EmployeeId=(select id from Employees where FileNumber=@fno))=0
			)T 
			on(c.dates=t.[date] and c.FNO=t.FNO)
   left join (select sum(NoOfHour)'NoOfHour',EmployeeId,[Date] from AdditionApprovals group by EmployeeId,[Date]) aa on( aa.EmployeeId=t.ID and aa.[Date]=T.[date])
   left join Employees emp on (c.FNO=emp.FileNumber) 
   left join BasicBayWorks bb on bb.EmployeeId = emp.Id
   left join Shifts shft on shft.ShiftId=bb.ShiftId 
   left join Absences AB on (AB.DateFrom=c.dates and AB.EmployeeId=emp.ID and ab.AbsenceTypeId<>4)--أجازات 
   left join 
            (
			  select DateFrom,EmployeeId,AbsenceTypeId,sum(DayPart)DayPart  
			  from Absences 
			  group by DateFrom,EmployeeId,AbsenceTypeId
			)AB1 on (AB1.DateFrom=c.dates and AB1.EmployeeId=emp.ID and ab1.AbsenceTypeId<>4)--أجازات 
   left join 
            (
			 select DateFrom,EmployeeId,count(*) cnt from Absences where AbsenceTypeId=4 group by DateFrom,EmployeeId 
			)ABM on (ABM.DateFrom=c.dates and ABM.EmployeeId=emp.Id )--مأمورية
   left join Vacations v on v.Date=c.dates
   left join(
   select s.Id,
		   s.fileNo,
		   s.DepartementId,
		   d.Name'Dept',
		   monthlySalary,
		   regular,
		   expensive,
		   skill,
		   management,
		   insurance,
		    max(case when clothesDate=@f then clothes else 0 end)'clothes',
		    max(case when SanctionsDate=@f then sanctions else 0 end)'sanctions',
		    max(case when deductionsDate=@f then deductions else 0 end)'deductions',
		    max(case when loansDate=@f  then loans else 0 end)'loans'
	from AK_SALARY_DETAILS_V1  s left join   Departements d on d.Id=s.DepartementId 
	group by s.Id,
	       s.name,
		   s.fileNo,
		   s.DepartementId,
		   d.Name,
		   monthlySalary,
		   regular,
		   expensive,
		   skill,
		   management,
		   insurance
   )salary on salary.fileNo=c.FNO
  left join (
SELECT ISNULL(AB.vacation,0)vacation,
		   ISNULL(E.FileNumber,0)FileNumber,
		   ISNULL(E.KnownAs,0) 'name',
		   ISNULL(bb.EmployeeId,0)EmployeeId,
		   ISNULL(E.DepartementId,0)DepartementId,
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
		 when DATEPART(DAY,VacationDeferredDate)>=26 and DATEPART(DAY,@f)>=26 then DATEDIFF(MONTH,VacationDeferredDate,@f)-1+1
		 when DATEPART(DAY,VacationDeferredDate)< 26 and DATEPART(DAY,@f)>=26 then DATEDIFF(MONTH,VacationDeferredDate,@f)+1
		 when DATEPART(DAY,VacationDeferredDate)>=26 and DATEPART(DAY,@f)< 26 then DATEDIFF(MONTH,VacationDeferredDate,@f)-1
		 when DATEPART(DAY,VacationDeferredDate)< 26 and DATEPART(DAY,@f)< 26 then DATEDIFF(MONTH,VacationDeferredDate,@f)
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
		  and a.DateFrom>=VacationDeferredDate and a.DateFrom<=@T
	group by FileNumber,A.EmployeeId

	)AB on ab.EmployeeId=bb.EmployeeId
	join Employees E on bb.EmployeeId=E.id join Personals P on p.Id=e.PersonalId
	where p.StatusId<>3 
)vacDef on vacDef.FileNumber=c.FNO 
order by c.dates asc";
            SqlCommand com = new SqlCommand(sql);
            com.CommandType = CommandType.Text;
            com.Parameters.Add("@F", SqlDbType.Date).Value = d1.Date;
            com.Parameters.Add("@T", SqlDbType.Date).Value = d2.Date;
            com.Parameters.Add("@FNO", SqlDbType.Int).Value = fileNo;
            return Json(database.toJSON(database.getData(com)), JsonRequestBehavior.AllowGet);

        }
        public JsonResult SalaryFN3(string fileNo, int M, int Y)
        {
            DateTime d1 = new DateTime(M > 1 ? Y : Y - 1, M > 1 ? M - 1 : 12, 26);
            DateTime d2 = new DateTime(Y, M, 25);
            string sql = @"WITH CTE_Months
AS
(
    SELECT @f dates
    UNION ALL
    SELECT DATEADD(DAY,1,dates)
    FROM CTE_Months
    WHERE DATEADD(DAY,1,dates) <= @t
)

select 
       c.day,
       c.dates,
 a.ID,
 a.FNO,
 a.name,
 a.dept,
 a.date,
 a.TIMEFROM,
 a.TIMETO,
 a.ShiftId,
 a.Shift2,
 a.Shift3,
 a.StartTime,
 a.EndTime,
 a.DailyHours,
 a.Late,
 case 
	   when a.ShiftId =17 then
	   case 
	       when (late<=3 and late>=0 )or (late-[.25]<=7 and late-[.25]>=0 )or(late-[.5]<=9 and late-[.5]>=0 ) then 0
	       when (late<=22 and late>=4)or (late-[.25]<=24 and late-[.25]>=8)or(late-[.5]<=22 and late-[.5]>=10)then .25 
	       when (late<=39 and late>=23)or (late-[.25]<=37 and late-[.25]>=25)or(late-[.5]<=33 and late-[.5]>=23)then .50
	       when (late<=52 and late>=40)or (late-[.25]<=48 and late-[.25]>=38)or(late-[.5]<=52 and late-[.5]>=34)then .75
	       when (late<=63 and late>=53)or (late-[.25]<=67 and late-[.25]>=49)or(late-[.5]<=69 and late-[.5]>=53)then 1

	    end
	 else
		case 
	       when (late<=15 and late>=0 )or (late-[.25]<=9 and late-[.25]>=0 )or(late-[.5]<=15 and late-[.5]>=0 ) then 0
	       when (late<=22 and late>=16)or (late-[.25]<=22 and late-[.25]>=10)or(late-[.5]<=22 and late-[.5]>=10)then .25 
	       when (late<=39 and late>=23)or (late-[.25]<=45 and late-[.25]>=23)or(late-[.5]<=45 and late-[.5]>=23)then .50
	       when (late<=52 and late>=40)or (late-[.25]<=52 and late-[.25]>=46)or(late-[.5]<=52 and late-[.5]>=46)then .75
	       when (late<=75 and late>=53)or (late-[.25]<=75 and late-[.25]>=53)or(late-[.5]<=75 and late-[.5]>=53)then 1
	    end 
	   end lateValue,
	   ISNULL(attend,0)'attend',
	  (a.DailyHours*60)-(attend+Late)'Leave',
	  case 
	       when ((a.DailyHours*60)-(attend+Late)<=7 and (a.DailyHours*60)-(attend+Late)>=0 )or ((a.DailyHours*60)-(attend+Late)-[.25]<=7 and (a.DailyHours*60)-(attend+Late)-[.25]>=0 )or((a.DailyHours*60)-(attend+Late)-[.5]<=7 and (a.DailyHours*60)-(attend+Late)-[.5]>=0 ) then 0
	       when ((a.DailyHours*60)-(attend+Late)<=19 and (a.DailyHours*60)-(attend+Late)>=8)or ((a.DailyHours*60)-(attend+Late)-[.25]<=19 and (a.DailyHours*60)-(attend+Late)-[.25]>=8)or((a.DailyHours*60)-(attend+Late)-[.5]<=19 and (a.DailyHours*60)-(attend+Late)-[.5]>=8)then .25 
	       when ((a.DailyHours*60)-(attend+Late)<=37 and (a.DailyHours*60)-(attend+Late)>=20)or ((a.DailyHours*60)-(attend+Late)-[.25]<=37 and (a.DailyHours*60)-(attend+Late)-[.25]>=20)or((a.DailyHours*60)-(attend+Late)-[.5]<=37 and (a.DailyHours*60)-(attend+Late)-[.5]>=20)then .50
	       when ((a.DailyHours*60)-(attend+Late)<=45 and (a.DailyHours*60)-(attend+Late)>=38)or ((a.DailyHours*60)-(attend+Late)-[.25]<=45 and (a.DailyHours*60)-(attend+Late)-[.25]>=38)or((a.DailyHours*60)-(attend+Late)-[.5]<=45 and (a.DailyHours*60)-(attend+Late)-[.5]>=38)then .75
	       when ((a.DailyHours*60)-(attend+Late)<=67 and (a.DailyHours*60)-(attend+Late)>=46)or ((a.DailyHours*60)-(attend+Late)-[.25]<=67 and (a.DailyHours*60)-(attend+Late)-[.25]>=46)or((a.DailyHours*60)-(attend+Late)-[.5]<=67 and (a.DailyHours*60)-(attend+Late)-[.5]>=46)then 1
		   when(dept in(324,1009,1010,1011,1012,1013,1014,1015,1016,1017,1018,1019,1020,1021) and ((a.DailyHours*60)-(attend+Late)<=80 and (a.DailyHours*60)-(attend+Late)>=68)or ((a.DailyHours*60)-(attend+Late)-[.25]<=80 and (a.DailyHours*60)-(attend+Late)-[.25]>=68)or((a.DailyHours*60)-(attend+Late)-[.5]<=80 and (a.DailyHours*60)-(attend+Late)-[.5]>=68))then 1.25
	    end leaveValue,
 a.[.25],
 a.[.5],
 --a.attend,
 a.TotalSalary'monthlySalary',
 a.Overtime'overtime',
 a.SkillIncentive'skill',
 a.ExpensiveLivingConditons'expensive',
 a.IncentiveIncentiveForAbsence'management',
 a.RegularityIncentive'regular',
Vacation.daypart,
vacation.AbsenceTypeId,
(select Name from AbsenceTypes where ID= Vacation.absenceTypeid)'vacationType',
addition.noofhour,cast(addition.noofhour*a.overtime*a.totalsalary/dailyhours/30 as decimal(10,2))'overtimeValue',
clothes.amount 'clothes',
loans.amount 'loans',
deductions.amount 'deductions',
mission.mission,
balance.availableVacation'balance',
insurance.EmployeeFixedSalary*.11'insurance'
 /*****tables*****/
 from 
 (
    select DATENAME(WEEKDAY, dates) 'day',cast (dates as date)'dates',
       @FNO 'FNO' 
   from CTE_Months
 )C 
 left join --حضور 
 Ak_3STAGES_FUNC_ALL_V3(@f,@t,@fno) a on c.dates=a.date
 left join --اجازات 
 (
 select sum(daypart)daypart,DateFrom,AbsenceTypeId from Absences 
 where EmployeeId=(select Id from Employees where FileNumber=@fno) and AbsenceTypeId<>4
 group by DateFrom,AbsenceTypeId
 ) vacation on c.dates=vacation.DateFrom
 left join --مأمورية  
 (select count(*)mission,DateFrom from Absences 
 where EmployeeId=(select Id from Employees where FileNumber=30) and AbsenceTypeId=4
 group by DateFrom
 ) mission on c.dates=mission.DateFrom
 left join --أضافي 
 (select sum(noofhour)noofhour,date from AdditionApprovals  where EmployeeId=(select Id from Employees where FileNumber=@fno)group by DATE)
 addition   on c.dates=addition.Date
 left join --سلف
 (select  DATE,sum(amount)amount  from Payments p where PaymentDeductionId in (1)      and EmployeeId=(select Id from Employees where FileNumber=@fno) group by date)
 loans      on c.dates=loans.date
 left join --ملبس
 (select  DATE,sum(amount)amount  from Payments p where PaymentDeductionId in (2,9)    and EmployeeId=(select Id from Employees where FileNumber=@fno) group by date)
 clothes    on c.dates=clothes.date
 left join --جزءات وتأخيرات
 (select  DATE,sum(amount)amount  from Payments p where PaymentDeductionId in (3,4,7,8)and EmployeeId=(select Id from Employees where FileNumber=@fno) group by date)
 deductions on c.dates=deductions.date
 left join --رصيد الاجازات
 (SELECT 
		   ISNULL(bb.EmployeeId,0)EmployeeId,
		   ISNULL(bb.deferred,0)-isnull(ab.vacation,0) 'availableVacation'
	FROM
	(
	select EmployeeId,
		   VacationDeferredDate,
		   NumberOfStageVacations,
	ISNULL(
	case 
		 when DATEPART(DAY,VacationDeferredDate)>=26 and DATEPART(DAY,@f)>=26 then DATEDIFF(MONTH,VacationDeferredDate,@f)-1+1
		 when DATEPART(DAY,VacationDeferredDate)< 26 and DATEPART(DAY,@f)>=26 then DATEDIFF(MONTH,VacationDeferredDate,@f)+1
		 when DATEPART(DAY,VacationDeferredDate)>=26 and DATEPART(DAY,@f)< 26 then DATEDIFF(MONTH,VacationDeferredDate,@f)-1
		 when DATEPART(DAY,VacationDeferredDate)< 26 and DATEPART(DAY,@f)< 26 then DATEDIFF(MONTH,VacationDeferredDate,@f)
	end*1.75,1.75)+ISNULL(NumberOfStageVacations,0) deferred
	from BasicBayWorks 
	)BB 
	left join
   (
           select sum(a.DayPart)vacation, 
		   a.EmployeeId
	from Absences a left join BasicBayWorks b on b.EmployeeId=a.EmployeeId  
	where a.AbsenceTypeId in(3,5,7)
		  and a.DateFrom>=VacationDeferredDate and a.DateFrom<=@T		 
	group by A.EmployeeId
	)AB on ab.EmployeeId=bb.EmployeeId
	)balance
 on balance.EmployeeId=a.id
 left join  --التامينات
 (
	select * from
		(
			select ROW_NUMBER()over(partition by employeeid order by id desc)n,
		           EmployeeId,
				   EmployeeFixedSalary 
				   from InsuranceDetails
		)q where n=1
 )
insurance on a.id=insurance.EmployeeId
";
            SqlCommand com = new SqlCommand(sql);
            com.CommandType = CommandType.Text;
            com.Parameters.Add("@F", SqlDbType.Date).Value = d1.Date;
            com.Parameters.Add("@T", SqlDbType.Date).Value = d2.Date;
            com.Parameters.Add("@FNO", SqlDbType.Int).Value = fileNo;
            return Json(database.toJSON(database.getData(com)), JsonRequestBehavior.AllowGet);
        }
        public JsonResult List(DateTime from,DateTime to,int dept)//تقرير الغياب بالقسم
        {
            return Json(ListAllBetween(from,to,dept), JsonRequestBehavior.AllowGet);
        }
        public JsonResult ListDetails(string ID,DateTime from, DateTime to)//الغياب بالفرد
        {
            return Json(ListDetailsBetween(ID,from, to), JsonRequestBehavior.AllowGet);
        }
       
    
        //attendance manual crud
        /***************************************************************************************/

        DBContext db = new DBContext();
        public JsonResult AttendIn(string fileNo, DateTime d,string t)
        {
            string sql = 
 @"if exists(select * from Attendances where EmployeeId = (select id from Employees where FileNumber=@fn)and DateFrom = @d)
begin
  --print('update');
            update Attendances set
            TimeFrom = @t
            --,
  --TimeTo
  where EmployeeId = (select id from Employees where FileNumber = @fn) 
    and DateFrom = @d
end

else
begin
  --print('insert');
            insert into Attendances(DateFrom, TimeFrom, AttendanceTypeId, EmployeeId)
         values(@d, @t, 2, (select id from Employees where FileNumber = @fn))
end";
            SqlCommand com = new SqlCommand(sql);
            com.CommandType = CommandType.Text;
            com.Parameters.Add("@d", SqlDbType.Date).Value = d.Date;
            com.Parameters.Add("@FN", SqlDbType.Int).Value = fileNo;
            com.Parameters.Add("@t", SqlDbType.NVarChar).Value = t;
            return Json(db.exec(com), JsonRequestBehavior.AllowGet);
        }
        public JsonResult AttendOut(string fileNo, DateTime d, string t)
        {
            string sql =
 @"if exists(select *from Attendances where EmployeeId = (select id from Employees where FileNumber=@fn)and DateFrom = @d)
begin
  --print('update');
  update Attendances set 
  --TimeFrom=@t
  --,
  TimeTo =@t
  where EmployeeId = (select id from Employees where FileNumber=@fn) 
    and DateFrom = @d 
end

else
begin 
  --print('insert');
  insert into Attendances(DateFrom,TimeTo,AttendanceTypeId,EmployeeId) 
         values(@d,@t,2,(select id from Employees where FileNumber=@fn))
end
";
            SqlCommand com = new SqlCommand(sql);
            com.CommandType = CommandType.Text;
            com.Parameters.Add("@d", SqlDbType.Date).Value = d.Date;
            com.Parameters.Add("@FN", SqlDbType.Int).Value = fileNo;
            com.Parameters.Add("@t", SqlDbType.NVarChar).Value = t;
            return Json(db.exec(com), JsonRequestBehavior.AllowGet);
        }
        //salary sheet entries
        /**************************************************************************************/
        public JsonResult SalarySheetAdd(string sql)
        {
            SqlCommand com = new SqlCommand(sql);
            com.CommandType = CommandType.Text;
            return Json(db.exec(com), JsonRequestBehavior.AllowGet);
        }
        
        /**************************************************************************************/
       // DBContext db = new DBContext();
        //Return list of all Employees  
                                       #region غياب
        public JsonResult ListAllBetween(DateTime from, DateTime to, int dept)
        {
            string sql = @"with cte as (
select @f date ,DateName(WEEKDAY,@f)dateName
union All
select DATEADD(day,1,date),DATENAME(WEEKDAY,DATEADD(day,1,date))dateName
from cte
where DATEADD(day,1,date)<=@t
)
select q.FileNumber'fileNo',q.KnownAs'name',count(*)absence
from
(
select q1.*,v.AttendanceValue
from 
(
select cte.*,e.FileNumber,e.KnownAs ,e.id,cast(p.JoiningDate as date)JoiningDate
from cte,Employees e left join Personals p on e.PersonalId=p.id
where DepartementId=@dept and p.StatusId=1
)q1
left join Attendances  att on(att.DateFrom=q1.date and att.EmployeeId=q1.id)
left join Absences ab on (ab.DateFrom=q1.date and ab.EmployeeId=q1.Id)
left join (select * from Vacations where AttendanceValue>0 ) v on (q1.date=v.Date)
where att.datefrom is null 
  and ab.DateFrom is null 
  and q1.dateName<>'friday'
  and v.AttendanceValue is null
  and q1.date>=q1.JoiningDate
)Q
group by Q.FileNumber,q.KnownAs
";
            SqlCommand com = new SqlCommand(sql);
            com.CommandType = CommandType.Text;
            com.Parameters.Add("@f", SqlDbType.Date).Value = from.Date;
            com.Parameters.Add("@t", SqlDbType.Date).Value = to.Date;
            com.Parameters.Add("@dept", SqlDbType.Int).Value = dept;
            return Json(db.toJSON(db.getData(com)), JsonRequestBehavior.AllowGet);

        }
        public JsonResult ListDetailsBetween(object ID, DateTime from, DateTime to)
        {
            string sql = @"
with cte as (
select @f date ,DateName(WEEKDAY,@f)dateName
union All
select DATEADD(day,1,date),DATENAME(WEEKDAY,DATEADD(day,1,date))dateName
from cte
where DATEADD(day,1,date)<=@t
)

select q1.*,v.AttendanceValue
from 
(
select cte.*,e.FileNumber,e.KnownAs ,e.id,cast(p.JoiningDate as date)JoiningDate
from cte,Employees e left join Personals p on e.PersonalId=p.id
where FileNumber=@Fno and p.StatusId=1
)q1
left join Attendances  att on(att.DateFrom=q1.date and att.EmployeeId=q1.id)
left join Absences ab on (ab.DateFrom=q1.date and ab.EmployeeId=q1.Id)
left join (select * from Vacations where AttendanceValue>0 ) v on (q1.date=v.Date)
where att.datefrom is null 
  and ab.DateFrom is null 
  and q1.dateName<>'friday'
  and v.AttendanceValue is null
  and q1.date>=q1.JoiningDate
";


            SqlCommand com = new SqlCommand(sql);
            com.CommandType = CommandType.Text;
            com.Parameters.Add("@FNO", SqlDbType.VarChar).Value = ID;
            com.Parameters.Add("@f", SqlDbType.Date).Value = from.Date;
            com.Parameters.Add("@t", SqlDbType.Date).Value = to.Date;

            return Json(db.toJSON(db.getData(com)), JsonRequestBehavior.AllowGet);
        }
        #endregion
                                     #region جدول الحضور
        public JsonResult ListLateByFileNo(object fn, int M, int Y)
        {
            DateTime d1 = new DateTime(M > 1 ? Y : Y - 1, M > 1 ? M - 1 : 12, 26);
            DateTime d2 = new DateTime(Y, M, 25);
            string sql = @"
WITH CTE_Months
AS
(
    SELECT @f dates
    UNION ALL
    SELECT DATEADD(DAY,1,dates)
    FROM CTE_Months
    WHERE DATEADD(DAY,1,dates) <= @t
)

SELECT c.day,
       c.dates,
       c.FNO,
       emp.KnownAs'name',
	   bb.TotalSalary'monthlysalary',
	   cast(bb.TotalSalary/30 as decimal(10,2))'dailySalary',
	   cast(bb.TotalSalary/30/t.DailyHours as decimal(10,2))'hourSalary',
	   bb.ManagementIncentive'Management',
	   bb.ExpensiveLivingConditons'Expensive',
	   bb.RegularityIncentive 'Regular',
	   bb.SkillIncentive 'skill',
       t.TIMEFROM,
       t.TIMETO,
       t.ShiftId,
       case when t.DailyHours is null then shft.DailyHrs else t.DailyHours end 'DailyHours',
	   Late,
	   case 
	   when t.ShiftId =17 then
	   case 
	       when (late<=3 and late>=0 )or (late-[.25]<=7 and late-[.25]>=0 )or(late-[.5]<=9 and late-[.5]>=0 ) then 0
	       when (late<=22 and late>=4)or (late-[.25]<=24 and late-[.25]>=8)or(late-[.5]<=22 and late-[.5]>=10)then .25 
	       when (late<=39 and late>=23)or (late-[.25]<=37 and late-[.25]>=25)or(late-[.5]<=33 and late-[.5]>=23)then .50
	       when (late<=52 and late>=40)or (late-[.25]<=48 and late-[.25]>=38)or(late-[.5]<=52 and late-[.5]>=34)then .75
	       when (late<=63 and late>=53)or (late-[.25]<=67 and late-[.25]>=49)or(late-[.5]<=69 and late-[.5]>=53)then 1
	    end
	 else
		case 
	       when (late<=15 and late>=0 )or (late-[.25]<=9 and late-[.25]>=0 )or(late-[.5]<=15 and late-[.5]>=0 ) then 0
	       when (late<=22 and late>=16)or (late-[.25]<=22 and late-[.25]>=10)or(late-[.5]<=22 and late-[.5]>=10)then .25 
	       when (late<=39 and late>=23)or (late-[.25]<=45 and late-[.25]>=23)or(late-[.5]<=45 and late-[.5]>=23)then .50
	       when (late<=52 and late>=40)or (late-[.25]<=52 and late-[.25]>=46)or(late-[.5]<=52 and late-[.5]>=46)then .75
	       when (late<=75 and late>=53)or (late-[.25]<=75 and late-[.25]>=53)or(late-[.5]<=75 and late-[.5]>=53)then 1
	    end 
	   end lateValue,
	   ISNULL(attend,0)'attend',
	  (t.DailyHours*60)-(attend+Late)'Leave',
	  case 
	       when ((t.DailyHours*60)-(attend+Late)<=7 and (t.DailyHours*60)-(attend+Late)>=0 )or ((t.DailyHours*60)-(attend+Late)-[.25]<=7 and (t.DailyHours*60)-(attend+Late)-[.25]>=0 )or((t.DailyHours*60)-(attend+Late)-[.5]<=7 and (t.DailyHours*60)-(attend+Late)-[.5]>=0 ) then 0
	       when ((t.DailyHours*60)-(attend+Late)<=19 and (t.DailyHours*60)-(attend+Late)>=8)or ((t.DailyHours*60)-(attend+Late)-[.25]<=19 and (t.DailyHours*60)-(attend+Late)-[.25]>=8)or((t.DailyHours*60)-(attend+Late)-[.5]<=19 and (t.DailyHours*60)-(attend+Late)-[.5]>=8)then .25 
	       when ((t.DailyHours*60)-(attend+Late)<=37 and (t.DailyHours*60)-(attend+Late)>=20)or ((t.DailyHours*60)-(attend+Late)-[.25]<=37 and (t.DailyHours*60)-(attend+Late)-[.25]>=20)or((t.DailyHours*60)-(attend+Late)-[.5]<=37 and (t.DailyHours*60)-(attend+Late)-[.5]>=20)then .50
	       when ((t.DailyHours*60)-(attend+Late)<=45 and (t.DailyHours*60)-(attend+Late)>=38)or ((t.DailyHours*60)-(attend+Late)-[.25]<=45 and (t.DailyHours*60)-(attend+Late)-[.25]>=38)or((t.DailyHours*60)-(attend+Late)-[.5]<=45 and (t.DailyHours*60)-(attend+Late)-[.5]>=38)then .75
	       when ((t.DailyHours*60)-(attend+Late)<=67 and (t.DailyHours*60)-(attend+Late)>=46)or ((t.DailyHours*60)-(attend+Late)-[.25]<=68 and (t.DailyHours*60)-(attend+Late)-[.25]>=46)or((t.DailyHours*60)-(attend+Late)-[.5]<=68 and (t.DailyHours*60)-(attend+Late)-[.5]>=46)then 1
	    end leaveValue,
		NoOfHour addition,
	  
         ABM.cnt mission,
	   
	   cast((case when (ab.TimeFrom ='' or ab.TimeTo='')  then 60*case when t.DailyHours is null then shft.DailyHrs else t.DailyHours end
	        when (ab.TimeFrom < ab.TimeTo) then DATEDIFF(MINUTE,ab.timefrom,ab.TimeTo)
	        when (ab.TimeFrom > ab.TimeTo) then DATEDIFF(MINUTE,ab.TimeTo,AB.TimeFrom)
	   end/cast(60 as float))/case when t.DailyHours is null then shft.DailyHrs else t.DailyHours end as decimal(10,2)) absenceValue,
	   
case  when (cast(ab.TimeFrom as time)<cast(ab.TimeTo as time)) and (cast(t.TIMEFROM as time)>cast(ab.TimeFrom as time) and cast(t.TIMEFROM as time)<cast(ab.TimeTo as time)) then '1'				     
	        when (cast(ab.TimeFrom as time)>cast(ab.TimeTo as time)) and (cast(t.TIMEFROM as time)<cast(ab.TimeFrom as time) and cast(t.TIMEFROM as time)>cast(ab.TimeTo as time)) then '1'
			else '0'
	   end 'inbetween'
FROM 
(
select DATENAME(WEEKDAY, dates) 'day',cast (dates as date)'dates',
       @FNO 'FNO' 
from CTE_Months
)C left join (select * from AK_3STAGES_V4 where date between @f and @t)T on(c.dates=t.[date] and c.FNO=t.FNO)
   left join (select sum(NoOfHour)'NoOfHour',EmployeeId,[Date] from AdditionApprovals group by EmployeeId,[Date]) aa on( aa.EmployeeId=t.ID and aa.[Date]=T.[date])
   left join Employees emp on (c.FNO=emp.FileNumber) 
   left join BasicBayWorks bb on bb.EmployeeId = emp.Id
   left join Shifts shft on shft.ShiftId=bb.ShiftId 
   left join Absences AB on (AB.DateFrom=c.dates and AB.EmployeeId=emp.ID and ab.AbsenceTypeId<>4)--أجازات 
   left join (select datefrom,EmployeeId,count(*) cnt from Absences where AbsenceTypeId=4 group by DateFrom,EmployeeId )ABM on (ABM.DateFrom=c.dates and ABM.EmployeeId=emp.Id )--مأمورية
order by c.dates asc";
            
               
                SqlCommand com = new SqlCommand(sql);
                com.CommandType = CommandType.Text;
                com.Parameters.Add("@FNO", SqlDbType.VarChar).Value = fn;
                com.Parameters.Add("@f", SqlDbType.Date).Value = d1.Date;
                com.Parameters.Add("@t", SqlDbType.Date).Value = d2.Date;


            return Json(db.toJSON(db.getData(com)), JsonRequestBehavior.AllowGet);
        
        }
        public JsonResult AttendanceBetween(int fn, DateTime f, DateTime t)
        {
            #region query
            string sql = @"
WITH CTE_Months
AS
(
    SELECT @f dates
    UNION ALL
    SELECT DATEADD(DAY,1,dates)
    FROM CTE_Months
    WHERE DATEADD(DAY,1,dates) <= @t
)
SELECT c.day,c.daynum%7'daynum',
       case when shft.WeekendFromDay<=shft.WeekendToDay then 
	       case when c.daynum%7 >=shft.WeekendFromDay and c.daynum%7 <=shft.WeekendToDay then 1
				else 0
		   end
		   else case when not(c.daynum%7 <shft.WeekendFromDay and c.daynum%7 >shft.WeekendToDay) then 1
				else 0
		   end
	   end 'weekend',
       c.dates,
       c.FNO,
       emp.KnownAs'name',
	   cast(bb.TotalSalary/30 as decimal(10,2))'dailySalary',
	   cast(bb.TotalSalary/30/shft.DailyHrs as decimal(10,4))'hourSalary',
	   cast(bb.TotalSalary/30/t.DailyHours as decimal(10,2))'hourSalary1',
	   
       t.TIMEFROM,
       t.TIMETO,
       t.ShiftId,
       case when t.DailyHours is null then shft.DailyHrs else t.DailyHours end 'DailyHours',
	   Late,
	   case 
	   when t.ShiftId =17 then
	   case 
	       when (late<=3 and late>=0 )or (late-[.25]<=7 and late-[.25]>=0 )or(late-[.5]<=9 and late-[.5]>=0 ) then 0
	       when (late<=22 and late>=4)or (late-[.25]<=24 and late-[.25]>=8)or(late-[.5]<=22 and late-[.5]>=10)then .25 
	       when (late<=39 and late>=23)or (late-[.25]<=37 and late-[.25]>=25)or(late-[.5]<=33 and late-[.5]>=23)then .50
	       when (late<=52 and late>=40)or (late-[.25]<=48 and late-[.25]>=38)or(late-[.5]<=52 and late-[.5]>=34)then .75
	       when (late<=75 and late>=53)or (late-[.25]<=67 and late-[.25]>=49)or(late-[.5]<=69 and late-[.5]>=53)then 1

	    end
	 else
		case 
	       when (late<=15 and late>=0 )or (late-[.25]<=9 and late-[.25]>=0 )or(late-[.5]<=15 and late-[.5]>=0 ) then 0
	       when (late<=22 and late>=16)or (late-[.25]<=22 and late-[.25]>=10)or(late-[.5]<=22 and late-[.5]>=10)then .25 
	       when (late<=39 and late>=23)or (late-[.25]<=45 and late-[.25]>=23)or(late-[.5]<=45 and late-[.5]>=23)then .50
	       when (late<=52 and late>=40)or (late-[.25]<=52 and late-[.25]>=46)or(late-[.5]<=52 and late-[.5]>=46)then .75
	       when (late<=75 and late>=53)or (late-[.25]<=75 and late-[.25]>=53)or(late-[.5]<=75 and late-[.5]>=53)then 1
	    end 
	   end lateValue,
	   ISNULL(attend,0)'attend',
	  (t.DailyHours*60)-(attend+Late)'Leave',
	  case 
	   when t.ShiftId =17 then 
        case 
	       when ((t.DailyHours*60)-(attend+Late)<=7 and (t.DailyHours*60)-(attend+Late)>=0 )or ((t.DailyHours*60)-(attend+Late)-[.25]<=5 and (t.DailyHours*60)-(attend+Late)-[.25]>=0 )or((t.DailyHours*60)-(attend+Late)-[.5]<=7 and (t.DailyHours*60)-(attend+Late)-[.5]>=0 ) then 0
	       when ((t.DailyHours*60)-(attend+Late)<=20 and (t.DailyHours*60)-(attend+Late)>=8)or ((t.DailyHours*60)-(attend+Late)-[.25]<=22 and (t.DailyHours*60)-(attend+Late)-[.25]>=6)or((t.DailyHours*60)-(attend+Late)-[.5]<=15 and (t.DailyHours*60)-(attend+Late)-[.5]>=8)then .25 
	       when ((t.DailyHours*60)-(attend+Late)<=37 and (t.DailyHours*60)-(attend+Late)>=21)or ((t.DailyHours*60)-(attend+Late)-[.25]<=30 and (t.DailyHours*60)-(attend+Late)-[.25]>=23)or((t.DailyHours*60)-(attend+Late)-[.5]<=37 and (t.DailyHours*60)-(attend+Late)-[.5]>=16)then .50
	       when ((t.DailyHours*60)-(attend+Late)<=45 and (t.DailyHours*60)-(attend+Late)>=38)or ((t.DailyHours*60)-(attend+Late)-[.25]<=52 and (t.DailyHours*60)-(attend+Late)-[.25]>=31)or((t.DailyHours*60)-(attend+Late)-[.5]<=50 and (t.DailyHours*60)-(attend+Late)-[.5]>=38)then .75
	       when ((t.DailyHours*60)-(attend+Late)<=67 and (t.DailyHours*60)-(attend+Late)>=46)or ((t.DailyHours*60)-(attend+Late)-[.25]<=67 and (t.DailyHours*60)-(attend+Late)-[.25]>=53)or((t.DailyHours*60)-(attend+Late)-[.5]<=67 and (t.DailyHours*60)-(attend+Late)-[.5]>=51)then 1
		   when(((t.DailyHours*60)-(attend+Late)<=80 and (t.DailyHours*60)-(attend+Late)>=68))then 1.25
	    end
     else 
        case 
	       when ((t.DailyHours*60)-(attend+Late)<=7 and (t.DailyHours*60)-(attend+Late)>=0 )or ((t.DailyHours*60)-(attend+Late)-[.25]<=7 and (t.DailyHours*60)-(attend+Late)-[.25]>=0 )or((t.DailyHours*60)-(attend+Late)-[.5]<=7 and (t.DailyHours*60)-(attend+Late)-[.5]>=0 ) then 0
	       when ((t.DailyHours*60)-(attend+Late)<=20 and (t.DailyHours*60)-(attend+Late)>=8)or ((t.DailyHours*60)-(attend+Late)-[.25]<=20 and (t.DailyHours*60)-(attend+Late)-[.25]>=8)or((t.DailyHours*60)-(attend+Late)-[.5]<=20 and (t.DailyHours*60)-(attend+Late)-[.5]>=8)then .25 
	       when ((t.DailyHours*60)-(attend+Late)<=37 and (t.DailyHours*60)-(attend+Late)>=21)or ((t.DailyHours*60)-(attend+Late)-[.25]<=37 and (t.DailyHours*60)-(attend+Late)-[.25]>=21)or((t.DailyHours*60)-(attend+Late)-[.5]<=37 and (t.DailyHours*60)-(attend+Late)-[.5]>=21)then .50
	       when ((t.DailyHours*60)-(attend+Late)<=45 and (t.DailyHours*60)-(attend+Late)>=38)or ((t.DailyHours*60)-(attend+Late)-[.25]<=52 and (t.DailyHours*60)-(attend+Late)-[.25]>=38)or((t.DailyHours*60)-(attend+Late)-[.5]<=45 and (t.DailyHours*60)-(attend+Late)-[.5]>=38)then .75
	       when ((t.DailyHours*60)-(attend+Late)<=67 and (t.DailyHours*60)-(attend+Late)>=46)or ((t.DailyHours*60)-(attend+Late)-[.25]<=67 and (t.DailyHours*60)-(attend+Late)-[.25]>=53)or((t.DailyHours*60)-(attend+Late)-[.5]<=67 and (t.DailyHours*60)-(attend+Late)-[.5]>=46)then 1
		   when(t.DailyHours>8 and ((t.DailyHours*60)-(attend+Late)<=80 and (t.DailyHours*60)-(attend+Late)>=68))then 1.25
	    end 
       end  leaveValue,
		NoOfHour addition,
	  	bb.Overtime,
        ABM.cnt mission,
	   
	   
	   cast((case when (ab.TimeFrom ='' or ab.TimeTo='')  then 60*case when t.DailyHours is null then shft.DailyHrs else t.DailyHours end
	        when (ab.TimeFrom < ab.TimeTo) then DATEDIFF(MINUTE,ab.timefrom,ab.TimeTo)
	        when (ab.TimeFrom > ab.TimeTo) then DATEDIFF(MINUTE,ab.TimeTo,AB.TimeFrom)
	   end/cast(60 as float))/case when t.DailyHours is null then shft.DailyHrs else t.DailyHours end as decimal(10,2)) absenceValue,
	   ab1.DayPart,
	   ab1.AbsenceTypeId,
	   
case  when (cast(ab.TimeFrom as time)<cast(ab.TimeTo as time)) and (cast(t.TIMEFROM as time)>cast(ab.TimeFrom as time) and cast(t.TIMEFROM as time)<cast(ab.TimeTo as time)) then '1'				     
	        when (cast(ab.TimeFrom as time)>cast(ab.TimeTo as time)) and (cast(t.TIMEFROM as time)<cast(ab.TimeFrom as time) and cast(t.TIMEFROM as time)>cast(ab.TimeTo as time)) then '1'
			else '0'
	   end 'inbetween',
	   v.AttendanceValue,
	   salary.*,
	   vacDef.availableVacation,
	   cast(p.JoiningDate as date)joiningDate,
	   StatusId,
	   special special,
	    case
	       when (special<=22 and special>=0)  then .25 
	       when (special<=39 and special>=23) then .50
	       when (special<=52 and special>=40) then .75
	       when (special<=75 and special>=53)  then 1
		   when (special>75 )  then -1
		   else 0
		end specialValue
	   /************************************************/
FROM 
(
select DATENAME(WEEKDAY, dates) 'day',datepart(DW, dates) 'daynum',cast (dates as date)'dates',
       @FNO 'FNO' 
from CTE_Months
)C left join 
            (
			  select * from Ak_3STAGES_FUNC_V2(@f, @t,@fno)where(select top(1) IsPrivate from BasicBayWorks where EmployeeId=(select id from Employees where FileNumber=@fno))=1
			   union All
			  select * from Ak_3STAGES_FUNC_ALL_V2(@f, @t,@fno) where (select top(1) IsPrivate from BasicBayWorks where EmployeeId=(select id from Employees where FileNumber=@fno))=0
			)T 
			on(c.dates=t.[date] and c.FNO=t.FNO)
   left join (select sum(NoOfHour)'NoOfHour',EmployeeId,[Date] from AdditionApprovals group by EmployeeId,[Date]) aa on( aa.EmployeeId=t.ID and aa.[Date]=T.[date])
   left join Employees emp on (c.FNO=emp.FileNumber) 
   left join BasicBayWorks bb on bb.EmployeeId = emp.Id
   left join Personals p on emp.PersonalId = p.Id
   left join Shifts shft on shft.ShiftId=bb.ShiftId 
   left join Absences AB on (AB.DateFrom=c.dates and AB.EmployeeId=emp.ID and ab.AbsenceTypeId not in (4,11))--أجازات 
   left join 
            (
			  select DateFrom,EmployeeId,AbsenceTypeId,sum(DayPart)DayPart  
			  from Absences 
			  group by DateFrom,EmployeeId,AbsenceTypeId
			)AB1 on (AB1.DateFrom=c.dates and AB1.EmployeeId=emp.ID and ab1.AbsenceTypeId not in (4,11))--أجازات 
   left join 
            (
			 select DateFrom,EmployeeId,count(*) cnt from Absences where AbsenceTypeId=4  group by DateFrom,EmployeeId 
			)ABM on (ABM.DateFrom=c.dates and ABM.EmployeeId=emp.Id )--مأمورية
left join 
            (
			 select DateFrom,EmployeeId,datediff(MINUTE,TimeFrom,TimeTo) 'special' from Absences where AbsenceTypeId=11 
			)ABM0 on (ABM0.DateFrom=c.dates and ABM0.EmployeeId=emp.Id )--مأموريةخاصة
   left join Vacations v on v.Date=c.dates
   left join(
   select s.Id,
		   s.fileNo,
		   s.DepartementId,
		   d.Name'Dept',
		   monthlySalary,
		   regular,
		   expensive,
		   skill,
		   management,
		   insurance,
		   clothes,
		   sanctions ,
		   deductions ,
		   loans 
	from AK_SALARY_DETAILS_FUNC_V2(@f,@t,@fno) s left join   Departements d on d.Id=s.DepartementId 
   )salary on salary.fileNo=c.FNO
  left join (
SELECT ISNULL(AB.vacation,0)vacation,
		   ISNULL(E.FileNumber,0)FileNumber,
		   ISNULL(E.KnownAs,0) 'name',
		   ISNULL(bb.EmployeeId,0)EmployeeId,
		   ISNULL(E.DepartementId,0)DepartementId,
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
		 when DATEPART(DAY,VacationDeferredDate)>=26 and DATEPART(DAY,@f)>=26 then DATEDIFF(MONTH,VacationDeferredDate,@f)-1+1
		 when DATEPART(DAY,VacationDeferredDate)< 26 and DATEPART(DAY,@f)>=26 then DATEDIFF(MONTH,VacationDeferredDate,@f)+1
		 when DATEPART(DAY,VacationDeferredDate)>=26 and DATEPART(DAY,@f)< 26 then DATEDIFF(MONTH,VacationDeferredDate,@f)-1
		 when DATEPART(DAY,VacationDeferredDate)< 26 and DATEPART(DAY,@f)< 26 then DATEDIFF(MONTH,VacationDeferredDate,@f)
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
		  and a.DateFrom>=VacationDeferredDate and a.DateFrom<=@T
	group by FileNumber,A.EmployeeId

	)AB on ab.EmployeeId=bb.EmployeeId
	join Employees E on bb.EmployeeId=E.id join Personals P on p.Id=e.PersonalId
	where p.StatusId<>3 
)vacDef on vacDef.FileNumber=c.FNO 
order by c.dates asc";
            #endregion

            SqlCommand com = new SqlCommand(sql);
            com.CommandType = CommandType.Text;
            com.Parameters.Add("@FNO", SqlDbType.Int).Value = fn;
            com.Parameters.Add("@f", SqlDbType.Date).Value = f.Date;
            com.Parameters.Add("@t", SqlDbType.Date).Value = t.Date;

            return Json(db.toJSON(db.getData(com)), JsonRequestBehavior.AllowGet);
        }
        public JsonResult SalaryFN(object fn, int M, int Y)
        {
            DateTime d1 = new DateTime(M > 1 ? Y : Y - 1, M > 1 ? M - 1 : 12, 26);
            DateTime d2 = new DateTime(Y, M, 25);
            List<salaryAttModel> lst = new List<salaryAttModel>();
            #region query
            string sql = @"WITH CTE_Months
AS
(
    SELECT @f dates
    UNION ALL
    SELECT DATEADD(DAY,1,dates)
    FROM CTE_Months
    WHERE DATEADD(DAY,1,dates) <= @t
)
SELECT c.day,
       c.dates,
       c.FNO,
       emp.KnownAs'name',
	   cast(bb.TotalSalary/30 as decimal(10,2))'dailySalary',
	   cast(bb.TotalSalary/30/shft.DailyHrs as decimal(10,4))'hourSalary',
	   cast(bb.TotalSalary/30/t.DailyHours as decimal(10,2))'hourSalary1',
	   
       t.TIMEFROM,
       t.TIMETO,
       t.ShiftId,
       case when t.DailyHours is null then shft.DailyHrs else t.DailyHours end 'DailyHours',
	   Late,
	   case 
	   when t.ShiftId =17 then
	   case 
	       when (late<=3 and late>=0 )or (late-[.25]<=7 and late-[.25]>=0 )or(late-[.5]<=9 and late-[.5]>=0 ) then 0
	       when (late<=22 and late>=4)or (late-[.25]<=24 and late-[.25]>=8)or(late-[.5]<=22 and late-[.5]>=10)then .25 
	       when (late<=39 and late>=23)or (late-[.25]<=37 and late-[.25]>=25)or(late-[.5]<=33 and late-[.5]>=23)then .50
	       when (late<=52 and late>=40)or (late-[.25]<=48 and late-[.25]>=38)or(late-[.5]<=52 and late-[.5]>=34)then .75
	       when (late<=63 and late>=53)or (late-[.25]<=67 and late-[.25]>=49)or(late-[.5]<=69 and late-[.5]>=53)then 1

	    end
	 else
		case 
	       when (late<=15 and late>=0 )or (late-[.25]<=9 and late-[.25]>=0 )or(late-[.5]<=15 and late-[.5]>=0 ) then 0
	       when (late<=22 and late>=16)or (late-[.25]<=22 and late-[.25]>=10)or(late-[.5]<=22 and late-[.5]>=10)then .25 
	       when (late<=39 and late>=23)or (late-[.25]<=45 and late-[.25]>=23)or(late-[.5]<=45 and late-[.5]>=23)then .50
	       when (late<=52 and late>=40)or (late-[.25]<=52 and late-[.25]>=46)or(late-[.5]<=52 and late-[.5]>=46)then .75
	       when (late<=75 and late>=53)or (late-[.25]<=75 and late-[.25]>=53)or(late-[.5]<=75 and late-[.5]>=53)then 1
	    end 
	   end lateValue,
	   ISNULL(attend,0)'attend',
	  (t.DailyHours*60)-(attend+Late)'Leave',
	  case 
	       when ((t.DailyHours*60)-(attend+Late)<=7 and (t.DailyHours*60)-(attend+Late)>=0 )or ((t.DailyHours*60)-(attend+Late)-[.25]<=7 and (t.DailyHours*60)-(attend+Late)-[.25]>=0 )or((t.DailyHours*60)-(attend+Late)-[.5]<=7 and (t.DailyHours*60)-(attend+Late)-[.5]>=0 ) then 0
	       when ((t.DailyHours*60)-(attend+Late)<=19 and (t.DailyHours*60)-(attend+Late)>=8)or ((t.DailyHours*60)-(attend+Late)-[.25]<=19 and (t.DailyHours*60)-(attend+Late)-[.25]>=8)or((t.DailyHours*60)-(attend+Late)-[.5]<=19 and (t.DailyHours*60)-(attend+Late)-[.5]>=8)then .25 
	       when ((t.DailyHours*60)-(attend+Late)<=37 and (t.DailyHours*60)-(attend+Late)>=20)or ((t.DailyHours*60)-(attend+Late)-[.25]<=37 and (t.DailyHours*60)-(attend+Late)-[.25]>=20)or((t.DailyHours*60)-(attend+Late)-[.5]<=37 and (t.DailyHours*60)-(attend+Late)-[.5]>=20)then .50
	       when ((t.DailyHours*60)-(attend+Late)<=45 and (t.DailyHours*60)-(attend+Late)>=38)or ((t.DailyHours*60)-(attend+Late)-[.25]<=45 and (t.DailyHours*60)-(attend+Late)-[.25]>=38)or((t.DailyHours*60)-(attend+Late)-[.5]<=45 and (t.DailyHours*60)-(attend+Late)-[.5]>=38)then .75
	       when ((t.DailyHours*60)-(attend+Late)<=67 and (t.DailyHours*60)-(attend+Late)>=46)or ((t.DailyHours*60)-(attend+Late)-[.25]<=67 and (t.DailyHours*60)-(attend+Late)-[.25]>=46)or((t.DailyHours*60)-(attend+Late)-[.5]<=67 and (t.DailyHours*60)-(attend+Late)-[.5]>=46)then 1
		   when(salary.DepartementId in(324,1009,1010,1011,1012,1013,1014,1015,1016,1017,1018,1019,1020,1021) and ((t.DailyHours*60)-(attend+Late)<=80 and (t.DailyHours*60)-(attend+Late)>=68)or ((t.DailyHours*60)-(attend+Late)-[.25]<=80 and (t.DailyHours*60)-(attend+Late)-[.25]>=68)or((t.DailyHours*60)-(attend+Late)-[.5]<=80 and (t.DailyHours*60)-(attend+Late)-[.5]>=68))then 1.25
	    end leaveValue,
		NoOfHour addition,
	  	bb.Overtime,
        ABM.cnt mission,
	   
	   
	   cast((case when (ab.TimeFrom ='' or ab.TimeTo='')  then 60*case when t.DailyHours is null then shft.DailyHrs else t.DailyHours end
	        when (ab.TimeFrom < ab.TimeTo) then DATEDIFF(MINUTE,ab.timefrom,ab.TimeTo)
	        when (ab.TimeFrom > ab.TimeTo) then DATEDIFF(MINUTE,ab.TimeTo,AB.TimeFrom)
	   end/cast(60 as float))/case when t.DailyHours is null then shft.DailyHrs else t.DailyHours end as decimal(10,2)) absenceValue,
	   ab1.DayPart,
	   ab1.AbsenceTypeId,
	   
case  when (cast(ab.TimeFrom as time)<cast(ab.TimeTo as time)) and (cast(t.TIMEFROM as time)>cast(ab.TimeFrom as time) and cast(t.TIMEFROM as time)<cast(ab.TimeTo as time)) then '1'				     
	        when (cast(ab.TimeFrom as time)>cast(ab.TimeTo as time)) and (cast(t.TIMEFROM as time)<cast(ab.TimeFrom as time) and cast(t.TIMEFROM as time)>cast(ab.TimeTo as time)) then '1'
			else '0'
	   end 'inbetween',
	   v.AttendanceValue,
	   salary.*,
	   vacDef.availableVacation,
	   cast(p.JoiningDate as date)joiningDate,
	   StatusId
FROM 
(
select DATENAME(WEEKDAY, dates) 'day',cast (dates as date)'dates',
       @FNO 'FNO' 
from CTE_Months
)C left join 
            (
			  select * from Ak_3STAGES_FUNC_V2(@f, @t,@fno)where(select top(1) IsPrivate from BasicBayWorks where EmployeeId=(select id from Employees where FileNumber=@fno))=1
			   union All
			  select * from Ak_3STAGES_FUNC_ALL_V2(@f, @t,@fno) where (select top(1) IsPrivate from BasicBayWorks where EmployeeId=(select id from Employees where FileNumber=@fno))=0
			)T 
			on(c.dates=t.[date] and c.FNO=t.FNO)
   left join (select sum(NoOfHour)'NoOfHour',EmployeeId,[Date] from AdditionApprovals group by EmployeeId,[Date]) aa on( aa.EmployeeId=t.ID and aa.[Date]=T.[date])
   left join Employees emp on (c.FNO=emp.FileNumber) 
   left join BasicBayWorks bb on bb.EmployeeId = emp.Id
   left join Personals p on emp.PersonalId = p.Id
   left join Shifts shft on shft.ShiftId=bb.ShiftId 
   left join Absences AB on (AB.DateFrom=c.dates and AB.EmployeeId=emp.ID and ab.AbsenceTypeId<>4)--أجازات 
   left join 
            (
			  select DateFrom,EmployeeId,AbsenceTypeId,sum(DayPart)DayPart  
			  from Absences 
			  group by DateFrom,EmployeeId,AbsenceTypeId
			)AB1 on (AB1.DateFrom=c.dates and AB1.EmployeeId=emp.ID and ab1.AbsenceTypeId<>4)--أجازات 
   left join 
            (
			 select DateFrom,EmployeeId,count(*) cnt from Absences where AbsenceTypeId=4 group by DateFrom,EmployeeId 
			)ABM on (ABM.DateFrom=c.dates and ABM.EmployeeId=emp.Id )--مأمورية
   left join Vacations v on v.Date=c.dates
   left join(
   select s.Id,
		   s.fileNo,
		   s.DepartementId,
		   d.Name'Dept',
		   monthlySalary,
		   regular,
		   expensive,
		   skill,
		   management,
		   insurance,
		   clothes,
		   sanctions ,
		   deductions ,
		   loans 
	from AK_SALARY_DETAILS_FUNC_V2(@f,@t,@fno) s left join   Departements d on d.Id=s.DepartementId 
   )salary on salary.fileNo=c.FNO
  left join (
SELECT ISNULL(AB.vacation,0)vacation,
		   ISNULL(E.FileNumber,0)FileNumber,
		   ISNULL(E.KnownAs,0) 'name',
		   ISNULL(bb.EmployeeId,0)EmployeeId,
		   ISNULL(E.DepartementId,0)DepartementId,
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
		 when DATEPART(DAY,VacationDeferredDate)>=26 and DATEPART(DAY,@f)>=26 then DATEDIFF(MONTH,VacationDeferredDate,@f)-1+1
		 when DATEPART(DAY,VacationDeferredDate)< 26 and DATEPART(DAY,@f)>=26 then DATEDIFF(MONTH,VacationDeferredDate,@f)+1
		 when DATEPART(DAY,VacationDeferredDate)>=26 and DATEPART(DAY,@f)< 26 then DATEDIFF(MONTH,VacationDeferredDate,@f)-1
		 when DATEPART(DAY,VacationDeferredDate)< 26 and DATEPART(DAY,@f)< 26 then DATEDIFF(MONTH,VacationDeferredDate,@f)
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
		  and a.DateFrom>=VacationDeferredDate and a.DateFrom<=@T
	group by FileNumber,A.EmployeeId

	)AB on ab.EmployeeId=bb.EmployeeId
	join Employees E on bb.EmployeeId=E.id join Personals P on p.Id=e.PersonalId
	where p.StatusId<>3 
)vacDef on vacDef.FileNumber=c.FNO 
order by c.dates asc";
            #endregion
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CS"].ConnectionString))
            {
                con.Open();
                SqlCommand com = new SqlCommand(sql, con);
                com.CommandType = CommandType.Text;
                com.Parameters.Add("@FNO", SqlDbType.VarChar).Value = fn;
                com.Parameters.Add("@f", SqlDbType.Date).Value = d1.Date;
                com.Parameters.Add("@t", SqlDbType.Date).Value = d2.Date;

                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    //Console.WriteLine("count = "+rdr.FieldCount);
                    //Console.WriteLine("line"+ rdr["day"].ToString()+","+rdr["dates"].ToString());
                    lst.Add(new salaryAttModel
                    {
                        day = rdr["day"].ToString(),
                        date = rdr.GetDateTime(1).Date.ToShortDateString(),
                        fileNo = Convert.ToInt32(rdr["FNO"].ToString()),
                        dailyHour = rdr["dailyHours"].ToString() == "" ? 0 : Convert.ToDecimal(rdr["dailyHours"].ToString()),
                        late = rdr["late"].ToString() == "" ? 0 : Convert.ToInt32(rdr["late"].ToString()),
                        lateValue = rdr["lateValue"].ToString() == "" ? -1 : Convert.ToDecimal(rdr["lateValue"].ToString()),
                        shiftId = rdr["shiftId"].ToString() == "" ? 0 : Convert.ToInt32(rdr["shiftId"].ToString()),
                        leave = rdr["leave"].ToString() == "" ? 0 : Convert.ToInt32(rdr["leave"].ToString()),
                        leaveValue = rdr["leaveValue"].ToString() == "" ? -1 : Convert.ToDecimal(rdr["leaveValue"].ToString()),
                        addition = rdr["addition"].ToString() == "" ? 0 : Convert.ToDecimal(rdr["addition"].ToString()),
                        timeFrom = rdr["timeFrom"].ToString(),
                        timeTo = rdr["timeTo"].ToString(),
                        mission = rdr["mission"].ToString() == "" ? 0 : Convert.ToInt32(rdr["mission"].ToString()),
                        absenceValue = rdr["absenceValue"].ToString() == "" ? 0 : Convert.ToDecimal(rdr["absenceValue"].ToString()),
                        attend = rdr["attend"].ToString() == "" ? 0 : Convert.ToDecimal(rdr["attend"].ToString()),
                        //duration = rdr["duration"].ToString() == "" ? 0 : Convert.ToDecimal(rdr["duration"].ToString()),
                        name = rdr["name"].ToString(),
                        dailySalary = rdr["dailySalary"].ToString() == "" ? 0 : Convert.ToDecimal(rdr["dailySalary"].ToString()),
                        hourSalary = rdr["hourSalary"].ToString() == "" ? 0 : Convert.ToDecimal(rdr["hourSalary"].ToString()),
                        monthlySalary = rdr["monthlySalary"].ToString() == "" ? 0 : Convert.ToDecimal(rdr["monthlySalary"].ToString()),
                        regular = rdr["Regular"].ToString() == "" ? 0 : Convert.ToDecimal(rdr["Regular"].ToString()),
                        expensive = rdr["Expensive"].ToString() == "" ? 0 : Convert.ToDecimal(rdr["Expensive"].ToString()),
                        skill = rdr["skill"].ToString() == "" ? 0 : Convert.ToDecimal(rdr["skill"].ToString()),
                        management = rdr["Management"].ToString() == "" ? 0 : Convert.ToDecimal(rdr["Management"].ToString()),

                        inbetween = Convert.ToInt32(rdr["inbetween"].ToString()),
                        dayPart = rdr["dayPart"].ToString() == "" ? 0 : Convert.ToDecimal(rdr["dayPart"].ToString()),
                        absenceType = rdr["AbsenceTypeId"].ToString() == "" ? 0 : Convert.ToInt32(rdr["AbsenceTypeId"].ToString()),
                        officialAtt = rdr["AttendanceValue"].ToString() == "" ? -1 : Convert.ToDecimal(rdr["AttendanceValue"].ToString()),
                        statusId = rdr["statusId"].ToString() == "" ? 0 : Convert.ToInt32(rdr["statusId"].ToString()),
                        joiningDate = rdr["joiningDate"].ToString(),
                        overTime = rdr["Overtime"].ToString() == "" ? 0 : Convert.ToDecimal(rdr["Overtime"].ToString()),

                        balance = rdr["availableVacation"].ToString() == "" ? 0 : Convert.ToDecimal(rdr["availableVacation"].ToString()),
                        department = rdr["Dept"].ToString(),
                        insurance = rdr["insurance"].ToString() == "" ? 0 : Convert.ToDecimal(rdr["insurance"].ToString()),
                        loans = rdr["loans"].ToString() == "" ? 0 : Convert.ToDecimal(rdr["loans"].ToString()),
                        clothes = rdr["clothes"].ToString() == "" ? 0 : Convert.ToDecimal(rdr["clothes"].ToString()),
                        deductions = rdr["deductions"].ToString() == "" ? 0 : Convert.ToDecimal(rdr["deductions"].ToString()),
                        sanctions = rdr["sanctions"].ToString() == "" ? 0 : Convert.ToDecimal(rdr["sanctions"].ToString())
                    });
                }
                
                return Json(lst, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult SalaryFNO(string fileNo, int M, int Y)
        {
            DateTime d1 = new DateTime(M > 1 ? Y : Y - 1, M > 1 ? M - 1 : 12, 26);
            DateTime d2 = new DateTime(Y, M, 25);
            //List<salaryAttModel> lst = new List<salaryAttModel>();
            #region query
            string sql = @"
WITH CTE_Months
AS
(
    SELECT @f dates
    UNION ALL
    SELECT DATEADD(DAY,1,dates)
    FROM CTE_Months
    WHERE DATEADD(DAY,1,dates) <= @t
)
SELECT c.day,c.daynum%7'daynum',
       case when shft.WeekendFromDay<=shft.WeekendToDay then 
	       case when c.daynum%7 >=shft.WeekendFromDay and c.daynum%7 <=shft.WeekendToDay then 1
				else 0
		   end
		   else case when not(c.daynum%7 <shft.WeekendFromDay and c.daynum%7 >shft.WeekendToDay) then 1
				else 0
		   end
	   end 'weekend',
       c.dates,
       c.FNO,
       emp.KnownAs'name',
	   cast(bb.TotalSalary/30 as decimal(10,2))'dailySalary',
	   cast(bb.TotalSalary/30/shft.DailyHrs as decimal(10,4))'hourSalary',
	   cast(bb.TotalSalary/30/t.DailyHours as decimal(10,2))'hourSalary1',
       t.TIMEFROM,
       t.TIMETO,
       t.ShiftId,
       case when t.DailyHours is null then shft.DailyHrs else t.DailyHours end 'DailyHours',
	   t.ShiftDef,
	   t.LateDef,
	   t.LeaveDef,
	   t.AttValue,
	   Late,
	   case 
		when t.LateDef is null then 
		   case
			 when t.ShiftId =17 then
				case 
					when (late<=3 and late>=0 )or (late-[.25]<=7 and late-[.25]>=0 )or(late-[.5]<=9 and late-[.5]>=0 ) then 0
					when (late<=22 and late>=4)or (late-[.25]<=24 and late-[.25]>=8)or(late-[.5]<=22 and late-[.5]>=10)then .25 
					when (late<=39 and late>=23)or (late-[.25]<=37 and late-[.25]>=25)or(late-[.5]<=33 and late-[.5]>=23)then .50
					when (late<=52 and late>=40)or (late-[.25]<=48 and late-[.25]>=38)or(late-[.5]<=52 and late-[.5]>=34)then .75
					when (late<=75 and late>=53)or (late-[.25]<=67 and late-[.25]>=49)or(late-[.5]<=69 and late-[.5]>=53)then 1

			    end
			 else
				case 
					when (late<=15 and late>=0 )or (late-[.25]<=9 and late-[.25]>=0 )or(late-[.5]<=15 and late-[.5]>=0 ) then 0
					when (late<=22 and late>=16)or (late-[.25]<=22 and late-[.25]>=10)or(late-[.5]<=22 and late-[.5]>=10)then .25 
					when (late<=39 and late>=23)or (late-[.25]<=45 and late-[.25]>=23)or(late-[.5]<=45 and late-[.5]>=23)then .50
					when (late<=52 and late>=40)or (late-[.25]<=52 and late-[.25]>=46)or(late-[.5]<=52 and late-[.5]>=46)then .75
					when (late<=75 and late>=53)or (late-[.25]<=75 and late-[.25]>=53)or(late-[.5]<=75 and late-[.5]>=53)then 1
			 end
		   end 
		  else t.LateDef
	   end lateValue,
	   ISNULL(attend,0)'attend',
	  (t.DailyHours*60)-(attend+Late)'Leave',
	  case 
	   when t.ShiftId =17 then 
        case 
	       when ((t.DailyHours*60)-(attend+Late)<=7 and (t.DailyHours*60)-(attend+Late)>=0 )or ((t.DailyHours*60)-(attend+Late)-[.25]<=5 and (t.DailyHours*60)-(attend+Late)-[.25]>=0 )or((t.DailyHours*60)-(attend+Late)-[.5]<=7 and (t.DailyHours*60)-(attend+Late)-[.5]>=0 ) then 0
	       when ((t.DailyHours*60)-(attend+Late)<=20 and (t.DailyHours*60)-(attend+Late)>=8)or ((t.DailyHours*60)-(attend+Late)-[.25]<=22 and (t.DailyHours*60)-(attend+Late)-[.25]>=6)or((t.DailyHours*60)-(attend+Late)-[.5]<=15 and (t.DailyHours*60)-(attend+Late)-[.5]>=8)then .25 
	       when ((t.DailyHours*60)-(attend+Late)<=37 and (t.DailyHours*60)-(attend+Late)>=21)or ((t.DailyHours*60)-(attend+Late)-[.25]<=30 and (t.DailyHours*60)-(attend+Late)-[.25]>=23)or((t.DailyHours*60)-(attend+Late)-[.5]<=37 and (t.DailyHours*60)-(attend+Late)-[.5]>=16)then .50
	       when ((t.DailyHours*60)-(attend+Late)<=45 and (t.DailyHours*60)-(attend+Late)>=38)or ((t.DailyHours*60)-(attend+Late)-[.25]<=52 and (t.DailyHours*60)-(attend+Late)-[.25]>=31)or((t.DailyHours*60)-(attend+Late)-[.5]<=50 and (t.DailyHours*60)-(attend+Late)-[.5]>=38)then .75
	       when ((t.DailyHours*60)-(attend+Late)<=67 and (t.DailyHours*60)-(attend+Late)>=46)or ((t.DailyHours*60)-(attend+Late)-[.25]<=67 and (t.DailyHours*60)-(attend+Late)-[.25]>=53)or((t.DailyHours*60)-(attend+Late)-[.5]<=67 and (t.DailyHours*60)-(attend+Late)-[.5]>=51)then 1
		   when(((t.DailyHours*60)-(attend+Late)<=80 and (t.DailyHours*60)-(attend+Late)>=68))then 1.25
	    end
     else 
        case 
	       when ((t.DailyHours*60)-(attend+Late)<=7 and (t.DailyHours*60)-(attend+Late)>=0 )or ((t.DailyHours*60)-(attend+Late)-[.25]<=7 and (t.DailyHours*60)-(attend+Late)-[.25]>=0 )or((t.DailyHours*60)-(attend+Late)-[.5]<=7 and (t.DailyHours*60)-(attend+Late)-[.5]>=0 ) then 0
	       when ((t.DailyHours*60)-(attend+Late)<=20 and (t.DailyHours*60)-(attend+Late)>=8)or ((t.DailyHours*60)-(attend+Late)-[.25]<=20 and (t.DailyHours*60)-(attend+Late)-[.25]>=8)or((t.DailyHours*60)-(attend+Late)-[.5]<=20 and (t.DailyHours*60)-(attend+Late)-[.5]>=8)then .25 
	       when ((t.DailyHours*60)-(attend+Late)<=37 and (t.DailyHours*60)-(attend+Late)>=21)or ((t.DailyHours*60)-(attend+Late)-[.25]<=37 and (t.DailyHours*60)-(attend+Late)-[.25]>=21)or((t.DailyHours*60)-(attend+Late)-[.5]<=37 and (t.DailyHours*60)-(attend+Late)-[.5]>=21)then .50
	       when ((t.DailyHours*60)-(attend+Late)<=45 and (t.DailyHours*60)-(attend+Late)>=38)or ((t.DailyHours*60)-(attend+Late)-[.25]<=52 and (t.DailyHours*60)-(attend+Late)-[.25]>=38)or((t.DailyHours*60)-(attend+Late)-[.5]<=45 and (t.DailyHours*60)-(attend+Late)-[.5]>=38)then .75
	       when ((t.DailyHours*60)-(attend+Late)<=67 and (t.DailyHours*60)-(attend+Late)>=46)or ((t.DailyHours*60)-(attend+Late)-[.25]<=67 and (t.DailyHours*60)-(attend+Late)-[.25]>=53)or((t.DailyHours*60)-(attend+Late)-[.5]<=67 and (t.DailyHours*60)-(attend+Late)-[.5]>=46)then 1
		   when(t.DailyHours>8 and ((t.DailyHours*60)-(attend+Late)<=80 and (t.DailyHours*60)-(attend+Late)>=68))then 1.25
	    end 
       end  leaveValue,
		NoOfHour addition,
	  	bb.Overtime,
        ABM.cnt mission,
	   
	   
	   cast((case when (ab.TimeFrom ='' or ab.TimeTo='')  then 60*case when t.DailyHours is null then shft.DailyHrs else t.DailyHours end
	        when (ab.TimeFrom < ab.TimeTo) then DATEDIFF(MINUTE,ab.timefrom,ab.TimeTo)
	        when (ab.TimeFrom > ab.TimeTo) then DATEDIFF(MINUTE,ab.TimeTo,AB.TimeFrom)
	   end/cast(60 as float))/case when t.DailyHours is null then shft.DailyHrs else t.DailyHours end as decimal(10,2)) absenceValue,
	   ab1.DayPart,
	   ab1.AbsenceTypeId,
	   
case  when (cast(ab.TimeFrom as time)<cast(ab.TimeTo as time)) and (cast(t.TIMEFROM as time)>cast(ab.TimeFrom as time) and cast(t.TIMEFROM as time)<cast(ab.TimeTo as time)) then '1'				     
	        when (cast(ab.TimeFrom as time)>cast(ab.TimeTo as time)) and (cast(t.TIMEFROM as time)<cast(ab.TimeFrom as time) and cast(t.TIMEFROM as time)>cast(ab.TimeTo as time)) then '1'
			else '0'
	   end 'inbetween',
	   v.AttendanceValue,
	   salary.*,
	   vacDef.availableVacation,
	   cast(p.JoiningDate as date)joiningDate,
	   StatusId,
	   special special,
	    case
	       when (special<=22 and special>=0)  then .25 
	       when (special<=39 and special>=23) then .50
	       when (special<=52 and special>=40) then .75
	       when (special<=75 and special>=53)  then 1
		   when (special>75 )  then -1
		   else 0
		end specialValue
	   /************************************************/
FROM 
(
select DATENAME(WEEKDAY, dates) 'day',datepart(DW, dates) 'daynum',cast (dates as date)'dates',
       @FNO 'FNO' 
from CTE_Months
)C left join 
            (
			 select att.*,at.AttValue,at.Late 'LateDef',at.Leave 'LeaveDef',at.ShiftId'ShiftDef' from Ak_3STAGES_FUNC_V2(@f, @t,@fno)att left join Attendances at on (att.date=at.DateFrom and att.ID=at.EmployeeId)  where(select top(1) IsPrivate from BasicBayWorks where EmployeeId=(select id from Employees where FileNumber=@fno))=1 
			   union All
			  select att.*,at.AttValue,at.Late 'LateDef',at.Leave 'LeaveDef',at.ShiftId'ShiftDef' from Ak_3STAGES_FUNC_ALL_V2(@f, @t,@fno)att left join Attendances at on (att.date=at.DateFrom and att.ID=at.EmployeeId)  where (select top(1) IsPrivate from BasicBayWorks where EmployeeId=(select id from Employees where FileNumber=@fno))=0
			)T 
			on(c.dates=t.[date] and c.FNO=t.FNO)
   left join (select sum(NoOfHour)'NoOfHour',EmployeeId,[Date] from AdditionApprovals group by EmployeeId,[Date]) aa on( aa.EmployeeId=t.ID and aa.[Date]=T.[date])
   left join Employees emp on (c.FNO=emp.FileNumber) 
   left join BasicBayWorks bb on bb.EmployeeId = emp.Id
   left join Personals p on emp.PersonalId = p.Id
   left join Shifts shft on shft.ShiftId=bb.ShiftId 
   left join Absences AB on (AB.DateFrom=c.dates and AB.EmployeeId=emp.ID and ab.AbsenceTypeId not in (4,11))--أجازات 
   left join 
            (
			  select DateFrom,EmployeeId,AbsenceTypeId,sum(DayPart)DayPart  
			  from Absences 
			  group by DateFrom,EmployeeId,AbsenceTypeId
			)AB1 on (AB1.DateFrom=c.dates and AB1.EmployeeId=emp.ID and ab1.AbsenceTypeId not in (4,11))--أجازات 
   left join 
            (
			 select DateFrom,EmployeeId,count(*) cnt from Absences where AbsenceTypeId=4  group by DateFrom,EmployeeId 
			)ABM on (ABM.DateFrom=c.dates and ABM.EmployeeId=emp.Id )--مأمورية
left join 
            (
			 select DateFrom,EmployeeId,datediff(MINUTE,TimeFrom,TimeTo) 'special' from Absences where AbsenceTypeId=11 
			)ABM0 on (ABM0.DateFrom=c.dates and ABM0.EmployeeId=emp.Id )--مأموريةخاصة
   left join Vacations v on v.Date=c.dates
   left join(
   select s.Id,
		   s.fileNo,
		   s.DepartementId,
		   d.Name'Dept',
		   monthlySalary,
		   regular,
		   expensive,
		   skill,
		   management,
		   insurance,
		   clothes,
		   sanctions ,
		   deductions ,
		   loans 
	from AK_SALARY_DETAILS_FUNC_V2(@f,@t,@fno) s left join   Departements d on d.Id=s.DepartementId 
   )salary on salary.fileNo=c.FNO
  left join (
SELECT ISNULL(AB.vacation,0)vacation,
		   ISNULL(E.FileNumber,0)FileNumber,
		   ISNULL(E.KnownAs,0) 'name',
		   ISNULL(bb.EmployeeId,0)EmployeeId,
		   ISNULL(E.DepartementId,0)DepartementId,
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
		 when DATEPART(DAY,VacationDeferredDate)>=26 and DATEPART(DAY,@f)>=26 then DATEDIFF(MONTH,VacationDeferredDate,@f)-1+1
		 when DATEPART(DAY,VacationDeferredDate)< 26 and DATEPART(DAY,@f)>=26 then DATEDIFF(MONTH,VacationDeferredDate,@f)+1
		 when DATEPART(DAY,VacationDeferredDate)>=26 and DATEPART(DAY,@f)< 26 then DATEDIFF(MONTH,VacationDeferredDate,@f)-1
		 when DATEPART(DAY,VacationDeferredDate)< 26 and DATEPART(DAY,@f)< 26 then DATEDIFF(MONTH,VacationDeferredDate,@f)
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
		  and a.DateFrom>=VacationDeferredDate and a.DateFrom<=@T
	group by FileNumber,A.EmployeeId

	)AB on ab.EmployeeId=bb.EmployeeId
	join Employees E on bb.EmployeeId=E.id join Personals P on p.Id=e.PersonalId
	where p.StatusId<>3 
)vacDef on vacDef.FileNumber=c.FNO 
order by c.dates asc";
            #endregion
                SqlCommand com = new SqlCommand(sql);
                com.CommandType = CommandType.Text;
                com.Parameters.Add("@FNO", SqlDbType.Int).Value = fileNo;
                com.Parameters.Add("@f", SqlDbType.Date).Value = d1.Date;
                com.Parameters.Add("@t", SqlDbType.Date).Value = d2.Date;
                return Json(db.toJSON(db.getData(com)), JsonRequestBehavior.AllowGet);

        }
        public JsonResult SalaryFileNo(string fileNo, int M, int Y)
        {
            #region query
            string sql = @"
declare @f date ,@t date;
set @f=
(
select 
case when start <15 then 
cast((@y+'-'+@m+'-'+cast(Start as varchar)) as date) 
else
dateadd(month,-1,cast((@y+'-'+@m+'-'+cast(Start as varchar)) as date))
end
from Months
)
set @t=(select dateadd(day,-1,dateadd(month,1,@f)));
with month_cte as
( 
select @f dateDay
union All
select dateadd(day,1,dateDay)
from month_cte
where dateDay<@t
),
/***************************/
data as(
select 
      ROW_NUMBER()over(partition by dateday order by abs(late))n,--*
      DayName,
	  dateDay,
	  fno,weekend,name,
      ShiftId,StartTime,EndTime, 
	  TimeFrom,TimeTo,
	  lateHours,leaveHours,
	  attendbefore,
	  case when worktime-attend>worktime/2 then 0 
	       when worktime-attend>0 and worktime-attend<=worktime/2 then .5
		   when worktime-attend=0 then 1 
	  end 'att',
	  worktime,
	  cast(overtime as float(2)) /60'overTimeC',
	  overTimeHour,
	  vacDay,vacMinute,AbsenceType,AbsenceTypeId,
	  Amount,PaymentDeduction,PaymentDeductionId
 from
(
SELECT cte.* ,
	 case when DATEPART(DW, cte.dateDay)%7 between 
	 sh.WeekendFromDay and sh.WeekendToDay
	 then 1 else 0
	 end 'weekend',
     FirstName+' '+MidelName+' '+LastName 'name',
	 sh.ShiftId,sh.StartTime,sh.EndTime,
	 sh.EarlyArrive,
	 a.TimeFrom,a.TimeTo,
	 DATEDIFF(minute,StartTime,TimeFrom)late,
	 DATEDIFF(minute,TimeTo,EndTime)overtime--
	 ,cast(DATEPART(MINUTE,dly.value)+DATEPART(HOUR,dly.value)*60 as float)/60 'lateHours',
	 cast(DATEPART(MINUTE,lev.value)+DATEPART(HOUR,lev.value)*60 as float)/60 'leaveHours',
	 DATEDIFF(minute,TimeFrom,TimeTo)'attendbefore',
	 case when DATEDIFF(minute,TimeFrom,TimeTo)<0 then  24*60+DATEDIFF(minute,TimeFrom,TimeTo)
	 else DATEDIFF(minute,TimeFrom,TimeTo)
	 end 
	 +/*التاخير*/
	 case when DATEDIFF(minute,StartTime,TimeFrom)<0 then DATEDIFF(minute,StartTime,TimeFrom)
	      when (DATEDIFF(minute,StartTime,TimeFrom)>0 and dly.value is not null ) then DATEDIFF(minute,StartTime,TimeFrom)
		  else 0
	  end
	   +/*الانصراف المبكر لم يتم*/
	 case when DATEDIFF(minute,TimeTo,EndTime)<0 then DATEDIFF(minute,TimeTo,EndTime)
	      when (DATEDIFF(minute,TimeTo,EndTime)>0 and lev.value is not null) then DATEDIFF(minute,TimeTo,EndTime)-- يلزم جدول للقراءة
		  else 0
	  end'attend',
	  case 
	      when StartTime<EndTime /*DATEDIFF(minute,StartTime,EndTime)<0*/ then DATEDIFF(minute,StartTime,EndTime)
	      when StartTime>EndTime/*DATEDIFF(minute,StartTime,EndTime)>0*/  then 60*24+DATEDIFF(minute,StartTime,EndTime)
	  end'worktime'
	 ,ad.NoOfHour'overTimeHour'
	 ,cast(case when abc.HoursNo is null or abc.HoursNo='' then 1 else abc.HoursNo/sh.DailyHours end as float(2))'vacDay'
	 ,abc.HoursNo*60 'vacMinute'
	 ,abc.AbsenceType
	 ,abc.AbsenceTypeId
	 ,py.Amount
	 ,py.PaymentDeduction
	 ,py.PaymentDeductionId
FROM
(select DATENAME(DW,dateDay)DayName, dateDay,@fno fno from month_cte)cte left join 
 Employees E on e.FileNumber=cte.fno left join
 BasicBayWorks b on b.EmployeeId=e.id left join 
 Shifts sh on (
               (
			     (sh.ShiftId = b.ShiftId )and
				 (select IsOneShift from Shifts s where s.ShiftId= b.ShiftId) =1
			   ) or 
			   (
			     (sh.ShiftId = b.ShiftId or sh.Shift2 = b.ShiftId) and
				 (select IsTwoShift from Shifts s where s.ShiftId= b.ShiftId) =1
			   )or 
			   (
			     (sh.ShiftId = b.ShiftId or sh.Shift2 = b.ShiftId or sh.Shift3 = b.ShiftId)  and
				 (select IsThreeShift from Shifts s where s.ShiftId= b.ShiftId) =1
			   )
			   )left join
 (select DateFrom,case when TimeFrom ='' then null else TimeFrom end TimeFrom,
	        case when TimeTo  ='' then null else TimeTo end TimeTo,EmployeeId from  Attendances) a on( a.EmployeeId=e.id and cte.dateDay=a.DateFrom )
left join 
(
select ShiftId,DelayFromToes.* from Delays  
left join DelayFromToes  on Delays.Id=DelayId

)dly on (
           dly.ShiftId = sh.ShiftId and
		   a.TimeFrom >= dly.[From] and
		   a.TimeFrom <= dly.[To]
		)
left join 
(
select ShiftId,DelayFromToes.* from Delays  
left join DelayFromToes  on Delays.Id=DelayId

)lev on (
           lev.ShiftId = sh.ShiftId and
		   a.Timeto >= lev.[From] and
		   a.Timeto <= lev.[To]
		)
left join(select SUM(NoOfHour)NoOfHour,EmployeeId,Date from AdditionApprovals group by EmployeeId,Date )ad on (ad.Date=cte.dateDay and e.Id=ad.EmployeeId)
left join(
select EmployeeId, HoursNo,DateFrom, AbsenceTypeId,(select name from AbsenceTypes where id=AbsenceTypeId) AbsenceType
 from Absences ) abc on (abc.EmployeeId=e.Id and abc.datefrom=cte.dateDay)
 left join ( 
 select Date
,Amount
,PaymentDeductionId,(select name from PaymentDeductions where id= PaymentDeductionId) PaymentDeduction
,EmployeeId  from Payments
 )py on py.Date=cte.dateDay and py.EmployeeId=e.id
)Final
)
select * from data  where n=1
--order by dateDay";
            #endregion
            SqlCommand com = new SqlCommand(sql);
            com.CommandType = CommandType.Text;
            com.Parameters.Add("@FNO", SqlDbType.Int).Value = fileNo;
            com.Parameters.Add("@m", SqlDbType.VarChar).Value = M;
            com.Parameters.Add("@y", SqlDbType.VarChar).Value = Y;
            return Json(db.toJSON(db.getData(com)), JsonRequestBehavior.AllowGet);

        }

        public JsonResult SalaryFileNo2(string fileNo, int M, int Y)//AMMAR
        {
            #region query
            string sql = @"
declare @f date ,@t date;
set @f=
(
select 
case when start <15 then 
cast((@y+'-'+@m+'-'+cast(Start as varchar)) as date) 
else
dateadd(month,-1,cast((@y+'-'+@m+'-'+cast(Start as varchar)) as date))
end
from Months
)
set @t=(select dateadd(day,-1,dateadd(month,1,@f)));
with month_cte as
( 
select @f dateDay
union All
select dateadd(day,1,dateDay)
from month_cte
where dateDay<@t
),
/***************************/
data as(
select 
      ROW_NUMBER()over(partition by dateday order by abs(late))n,
      DayName,
	  dateDay,
	  fno,weekend,name,
     ShiftId,StartTime,EndTime, 
	  TimeFrom,TimeTo,
	  lateHours,
	  leaveHours,
	  attendbefore,
	   case when weekend=1 then 1
	       else  case when worktime-attend>worktime/2 then 0 
					  when worktime-attend>0 and worktime-attend<=worktime/2 then .5
					  when worktime-attend=0 then 1 
		   end 
	  end 'att',
	  vacDay,
	  worktime,
	  cast(overtime as float(2))/60 'overTimeC',
	  overTimeHour,
	  vacMinute,
	  AbsenceType,
	  AbsenceTypeId--,
	  --Amount,
	  --PaymentDeduction,
	  --PaymentDeductionId
 from
(
SELECT cte.* ,
	  case when sh.WeekendFromDay <= sh.WeekendToDay and DATEPART(DW, cte.dateDay)%7 between sh.WeekendFromDay and sh.WeekendToDay then 1
	      when sh.WeekendFromDay > sh.WeekendToDay and  not (DATEPART(DW, cte.dateDay)%7 >  sh.WeekendToDay and DATEPART(DW, cte.dateDay)%7 < sh.WeekendFromDay) then 1  
	      else 0
	 end 'weekend',
     FirstName+' '+MidelName+' '+LastName 'name',
	 sh.ShiftId,sh.StartTime,sh.EndTime,
	 sh.EarlyArrive,
	 a.TimeFrom,a.TimeTo,
	 DATEDIFF(minute,StartTime,TimeFrom)late,
	 DATEDIFF(minute,TimeTo,EndTime)overtime--
	 ,cast(DATEPART(MINUTE,dly.value)+DATEPART(HOUR,dly.value)*60 as float)/60 'lateHours',
	 cast(DATEPART(MINUTE,lev.value)+DATEPART(HOUR,lev.value)*60 as float)/60 'leaveHours',
	 DATEDIFF(minute,TimeFrom,TimeTo)'attendbefore',
	 case when DATEDIFF(minute,TimeFrom,TimeTo)<0 then  24*60+DATEDIFF(minute,TimeFrom,TimeTo)
	 else DATEDIFF(minute,TimeFrom,TimeTo)
	 end 
	 +/*التاخير*/
	 case when DATEDIFF(minute,StartTime,TimeFrom)<0 then DATEDIFF(minute,StartTime,TimeFrom)
	      when (DATEDIFF(minute,StartTime,TimeFrom)>0 and dly.value is not null ) then DATEDIFF(minute,StartTime,TimeFrom)
		  else 0
	  end
	   +/*الانصراف المبكر لم يتم*/
	 case when DATEDIFF(minute,TimeTo,EndTime)<0 then DATEDIFF(minute,TimeTo,EndTime)
	      when (DATEDIFF(minute,TimeTo,EndTime)>0 and lev.value is not null) then DATEDIFF(minute,TimeTo,EndTime)-- يلزم جدول للقراءة
		  else 0
	  end'attend',
	  case 
	      when StartTime<EndTime /*DATEDIFF(minute,StartTime,EndTime)<0*/ then DATEDIFF(minute,StartTime,EndTime)
	      when StartTime>EndTime/*DATEDIFF(minute,StartTime,EndTime)>0*/  then 60*24+DATEDIFF(minute,StartTime,EndTime)
	  end'worktime'
	 ,ad.NoOfHour'overTimeHour'
	 ,
	     case 
   			    when abc.AbsenceTypeId is null then '0'
				when abc.AbsenceTypeId is not null and abc.HoursNo is null then '1' 
				else cast(isnull(abc.HoursNo,0)/sh.DailyHours as decimal(10,2)) 
			end
	   'vacDay' --abc.HoursNo,sh.DailyHours
	 ,abc.HoursNo*60 'vacMinute'
	 ,abc.AbsenceType
	 ,abc.AbsenceTypeId
	 --,py.Amount
	 --,py.PaymentDeduction
	 --,py.PaymentDeductionId
FROM
(select DATENAME(DW,dateDay)DayName, dateDay,@fno fno from month_cte)cte left join 
 Employees E on e.FileNumber=cte.fno left join
 BasicBayWorks b on b.EmployeeId=e.id left join 
 Shifts sh on (
               (
			     (sh.ShiftId = b.ShiftId )and
				 (select IsOneShift from Shifts s where s.ShiftId= b.ShiftId) =1
			   ) or 
			   (
			     (sh.ShiftId = b.ShiftId or sh.Shift2 = b.ShiftId) and
				 (select IsTwoShift from Shifts s where s.ShiftId= b.ShiftId) =1
			   )or 
			   (
			     (sh.ShiftId = b.ShiftId or sh.Shift2 = b.ShiftId or sh.Shift3 = b.ShiftId)  and
				 (select IsThreeShift from Shifts s where s.ShiftId= b.ShiftId) =1
			   )
			   )left join
 (select DateFrom,case when TimeFrom ='' then null else TimeFrom end TimeFrom,
	        case when TimeTo  ='' then null else TimeTo end TimeTo,EmployeeId from  Attendances) a on( a.EmployeeId=e.id and cte.dateDay=a.DateFrom )
left join 
(
select ShiftId,DelayFromToes.* from Delays  
left join DelayFromToes  on Delays.Id=DelayId

)dly on (
           dly.ShiftId = sh.ShiftId and
		   a.TimeFrom >= dly.[From] and
		   a.TimeFrom <  dateadd(MINUTE,1,dly.[To])
		)
left join 
(
select ShiftId,DelayFromToes.* from Delays  
left join DelayFromToes  on Delays.Id=DelayId

)lev on (
           lev.ShiftId = sh.ShiftId and
		   a.Timeto >= lev.[From] and
		   a.Timeto <  dateadd(MINUTE,1,lev.[To])
		)
left join(select SUM(NoOfHour)NoOfHour,EmployeeId,Date from AdditionApprovals group by EmployeeId,Date )ad on (ad.Date=cte.dateDay and e.Id=ad.EmployeeId)
left join(
select EmployeeId,HoursNo,DateFrom, AbsenceTypeId,(select name from AbsenceTypes where id=AbsenceTypeId) AbsenceType
 from Absences ) abc on (abc.EmployeeId=e.Id and abc.datefrom=cte.dateDay)
 /*left join ( 
 select Date
,Amount
,PaymentDeductionId,(select name from PaymentDeductions where id= PaymentDeductionId) PaymentDeduction
,EmployeeId  from Payments
 )py on py.Date=cte.dateDay and py.EmployeeId=e.id*/
)Final
)
select * from data  where n=1 order by dateDay";
            #endregion
            SqlCommand com = new SqlCommand(sql);
            com.CommandType = CommandType.Text;
            com.Parameters.Add("@FNO", SqlDbType.Int).Value = fileNo;
            com.Parameters.Add("@m", SqlDbType.VarChar).Value = M;
            com.Parameters.Add("@y", SqlDbType.VarChar).Value = Y;
            return Json(db.toJSON(db.getData(com)), JsonRequestBehavior.AllowGet);

        }


    }

    #endregion
}
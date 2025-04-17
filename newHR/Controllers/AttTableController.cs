using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace newHR.Controllers
{
    public class AttTableController : Controller
    {
        DBContext db = new DBContext();
        // GET: AttTable
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Index2()
        {
            return View();
        }
        public ActionResult AttHours()
        {
            return View();
        }
        public ActionResult Production()
        {
            return View();
        }
        public ActionResult AttPeriod()
        {
            return View();
        }
        public ActionResult Weekly()
        {
            return View();
        }
        public ActionResult ByProduction()
        {
            return View();
        }
        public JsonResult AttTable(string fn)
        {
            DBContext db = new DBContext();
            SqlCommand com = new SqlCommand();
            string sql = @"select E.KnownAs'الاسم',
	   A.FileNumber,
	   cast(DateTime as date)date,
	   cast(DateTime as time(0))time,
	   case when Type='Out' then 'خروج' 
	        else 'دخول'
			end'type'
        from AClogs A join Employees E on A.FileNumber=E.FileNumber  
        where cast(Datetime as date) >= @f and cast(Datetime as date) <= @t ";
            com.CommandText = sql;
            com.Parameters.Add("@fno", SqlDbType.Date).Value = fn;

            return Json(db.toJSON(db.getData(com)), JsonRequestBehavior.AllowGet);
        }
        public JsonResult SalaryFileNo(string fileNo, int M, int Y)//AMMAR+Ahmed Eid
        {
            #region query
            string sql = @" 
--declare @Y varchar(4)='2023',@M varchar(2)='05',@fno int ='10002'
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
      --ROW_NUMBER()over(partition by dateday order by attendbefore desc,abs(late))n,
      DayName,
	  dateDay,
	  fno,weekend,name,dept,
      ShiftId,StartTime,EndTime, 
	  TimeFrom,
	  TimeTo,
	  case when lateValue is null then lateHours else lateValue end lateHours,
	  LateChecker,
	  LateChecker2,
	  case when leaveHours is null then leaveHours else leaveHours end leaveHours ,
	  leaveChecker,
	  leaveChecker2,
	  attendbefore,
	  late,
	  worktime-attend'0',
	  case when AttValue is null then isnull
	  (case when weekend=1 then 1
	       else  case when worktime-attend>worktime/2 then 0 
					  when worktime-attend>worktime/4 and worktime-attend<=worktime/2 then .5
					  when worktime-attend>0 and worktime-attend<=worktime/4 then .75 
					  when worktime-attend<=0 then 1 
		   end 
	  end,0)-isnull(levpart,0)-isnull(dlypart,0)
	   else AttValue end 'att',
	  vacDay,
	  worktime,
	  cast(overtime as float(2))/60 'overTimeC',
	  overTimeHour,
	  --vacMinute,
	  AbsenceType,
	  AbsenceTypeId,
	  errand,
	  special,
	  official,
	  TotalSalary, 
	  OvertimeRate, 
		/*SkillIncentive, 
		ExpensiveLivingConditons, 
		RegularityIncentive,  
		ManagementIncentive,*/
		   	  
	(
	select  
	ISNULL(
	case 
		 when DATEPART(DAY,VacationDeferredDate)>=26 and DATEPART(DAY,@t)>=26 then DATEDIFF(MONTH,VacationDeferredDate,@t)-1+1
		 when DATEPART(DAY,VacationDeferredDate)< 26 and DATEPART(DAY,@t)>=26 then DATEDIFF(MONTH,VacationDeferredDate,@t)+1
		 when DATEPART(DAY,VacationDeferredDate)>=26 and DATEPART(DAY,@t)< 26 then DATEDIFF(MONTH,VacationDeferredDate,@t)-1
		 when DATEPART(DAY,VacationDeferredDate)< 26 and DATEPART(DAY,@t)< 26 then DATEDIFF(MONTH,VacationDeferredDate,@t)
	end*1.75,1.75)+ISNULL(NumberOfStageVacations,0) deferred
	from BasicBayWorks 
	where EmployeeId=(select id from Employees where FileNumber=@fno)
	)
   -isnull((
	select sum(a.DayPart)  vacation
	from Absences a join 
	     BasicBayWorks b  on a.EmployeeId=b.EmployeeId 
	where (a.AbsenceTypeId=3 or a.AbsenceTypeId=7) 
		  and a.DateFrom>=VacationDeferredDate and a.DateFrom<=@t --@t
	      and a.EmployeeId=(select id from employees where FileNumber=@fno)
	),0)
	'balance'
		/*,
		   monthlySalary,
		   regular,
		   expensive,
		   skill,
		   management,
		   insurance,
		   clothes,
		   sanctions ,
		   deductions ,
		   loans */

 from
(
SELECT cte.* ,
	  case when sh.WeekendFromDay <= sh.WeekendToDay and DATEPART(DW, cte.dateDay)%7 between sh.WeekendFromDay and sh.WeekendToDay then 1
	      when sh.WeekendFromDay > sh.WeekendToDay and  not (DATEPART(DW, cte.dateDay)%7 >  sh.WeekendToDay and DATEPART(DW, cte.dateDay)%7 < sh.WeekendFromDay) then 1  
	      else 0
	 end 'weekend',
     FirstName+' '+MidelName+' '+LastName 'name',(select name from Departements where id=e.DepartementId)dept,
	 sh.ShiftId,
	 sh.StartTime,
	 sh.EndTime,
	 sh.EarlyArrive,
	 a.TimeFrom,
	 a.TimeTo,
	 a.AttValue,
	 a.Late'lateValue',
	 a.Leave'leaveValue',
	 lev.DayPart'levpart',
	 dly.DayPart'dlypart',
	 case --when DATEDIFF(minute,StartTime,a.TimeFrom)>sh.DailyHours*0.75*60 then DATEDIFF(minute,StartTime,a.TimeFrom)-24*60 
	      when DATEDIFF(minute,StartTime,a.TimeFrom)>0 and abs(DATEDIFF(minute,StartTime,a.TimeFrom))>sh.DailyHours*60 then DATEDIFF(minute,StartTime,a.TimeFrom)-24*60 
	      when DATEDIFF(minute,StartTime,a.TimeFrom)<0 and abs(DATEDIFF(minute,StartTime,a.TimeFrom))>sh.DailyHours*60 then DATEDIFF(minute,StartTime,a.TimeFrom)+24*60 
		  else DATEDIFF(minute,StartTime,a.TimeFrom) 
	  end --DATEDIFF(minute,StartTime,a.TimeFrom) 
	  late,
	  DATEDIFF(minute,StartTime,a.TimeFrom)latetest,
	 DATEDIFF(minute,a.TimeTo,EndTime)overtime--
	 ,cast(DATEPART(MINUTE,dly.value)+DATEPART(HOUR,dly.value)*60 as float)/60 'lateHours',
	 cast(DATEPART(MINUTE,lev.value)+DATEPART(HOUR,lev.value)*60 as float)/60 'leaveHours',
	 case when abc.TimeFrom<=abc.TimeTo then 
                case when a.TimeFrom >= abc.TimeFrom and a.TimeFrom <=abc.TimeTo then 'true' else 'false' end 
			else 
			    case when not(a.TimeFrom >= abc.TimeTo and a.TimeFrom <=abc.TimeFrom) then 'true' else 'false' end 
     end 'LateChecker',
	 case when abc.TimeFrom2<=abc.TimeTo2 then 
                case when a.TimeFrom >= abc.TimeFrom2 and a.TimeFrom <=abc.TimeTo2 then 'true' else 'false' end 
			else 
			    case when not(a.TimeFrom >= abc.TimeTo2 and a.TimeFrom <=abc.TimeFrom2) then 'true' else 'false' end 
     end 'LateChecker2',
     case when abc.TimeFrom<=abc.TimeTo then 
                case when a.TimeTo >= abc.TimeFrom and a.TimeTo <=abc.TimeTo then 'true' else 'false' end 
			else 
			    case when not(a.TimeTo >= abc.TimeTo and a.TimeTo <=abc.TimeFrom) then 'true' else 'false' end 
	 end 'leaveChecker',
	 case when abc.TimeFrom<=abc.TimeTo2 then 
                case when a.TimeTo >= abc.TimeFrom2 and a.TimeTo <=abc.TimeTo2 then 'true' else 'false' end 
			else 
			    case when not(a.TimeTo >= abc.TimeTo2 and a.TimeTo <=abc.TimeFrom2) then 'true' else 'false' end 
	 end 'leaveChecker2',
	 case when DATEDIFF(minute,a.TimeFrom,a.TimeTo)<0 then  24*60+DATEDIFF(minute,a.TimeFrom,a.TimeTo)
	 else DATEDIFF(minute,a.TimeFrom,a.TimeTo)end 'attendbefore',
	 case when DATEDIFF(minute,a.TimeFrom,a.TimeTo)<0 then  24*60+DATEDIFF(minute,a.TimeFrom,a.TimeTo)
	 else DATEDIFF(minute,a.TimeFrom,a.TimeTo)
	 end 
	 +/*التاخير*/
	 case when abs(case when DATEDIFF(minute,StartTime,a.TimeFrom)<0 then DATEDIFF(minute,StartTime,a.TimeFrom)
	      when (DATEDIFF(minute,StartTime,a.TimeFrom)>0 and dly.value is not null ) then DATEDIFF(minute,StartTime,a.TimeFrom)
		  else 0
	      end)>sh.DailyHours*60 then 
          (case when DATEDIFF(minute,StartTime,a.TimeFrom)<0 then DATEDIFF(minute,StartTime,a.TimeFrom)
	      when (DATEDIFF(minute,StartTime,a.TimeFrom)>0 and dly.value is not null ) then DATEDIFF(minute,StartTime,a.TimeFrom)
		  else 0
	      end)+24*60
     else
          (case when DATEDIFF(minute,StartTime,a.TimeFrom)<0 then DATEDIFF(minute,StartTime,a.TimeFrom)
	      when (DATEDIFF(minute,StartTime,a.TimeFrom)>0 and dly.value is not null ) then DATEDIFF(minute,StartTime,a.TimeFrom)
		  else 0
	      end)
	  end
	   +/*الانصراف المبكر لم يتم*/
	 case when abs(case when DATEDIFF(minute,a.TimeTo,EndTime)<0 then DATEDIFF(minute,a.TimeTo,EndTime)
	      when (DATEDIFF(minute,a.TimeTo,EndTime)>0 and lev.value is not null) then DATEDIFF(minute,a.TimeTo,EndTime)-- يلزم جدول للقراءة
		  else 0
	      end)>sh.DailyHours*60 then
      (case when DATEDIFF(minute,a.TimeTo,EndTime)<0 then DATEDIFF(minute,a.TimeTo,EndTime)
	      when (DATEDIFF(minute,a.TimeTo,EndTime)>0 and lev.value is not null) then DATEDIFF(minute,a.TimeTo,EndTime)-- يلزم جدول للقراءة
		  else 0
	      end) +24*60
     else
      (case when DATEDIFF(minute,a.TimeTo,EndTime)<0 then DATEDIFF(minute,a.TimeTo,EndTime)
	      when (DATEDIFF(minute,a.TimeTo,EndTime)>0 and lev.value is not null) then DATEDIFF(minute,a.TimeTo,EndTime)-- يلزم جدول للقراءة
		  else 0
	      end)
     end  
     'attend',
	  case 
	      when StartTime<EndTime  then DATEDIFF(minute,StartTime,EndTime)
	      when StartTime>EndTime  then 60*24+DATEDIFF(minute,StartTime,EndTime)
	  end'worktime'
	 ,ad.NoOfHour'overTimeHour'
	 ,
	      isnull(abc.DayPart,0)+isnull(abc.daypart2,0)
		
	   'vacDay' --abc.HoursNo,sh.DailyHours
	 --,abc.HoursNo*60 'vacMinute'
	 ,abc.AbsenceType
	 ,abc.AbsenceTypeId
	 ,abc1.cnt 'errand'
	 ,abc2.cnt 'special'
	 ,vac.AttendanceValue'official',
	    TotalSalary, 
		Overtime'OvertimeRate', 
		SkillIncentive, 
		ExpensiveLivingConditons, 
		RegularityIncentive,  
		ManagementIncentive		   
FROM
(select DATENAME(DW,dateDay)DayName, dateDay,@fno fno from month_cte)cte left join 
 Employees E on e.FileNumber=cte.fno left join
 (select * from BasicBayWorks where EmployeeId=(select id from Employees where FileNumber=@fno)) b on b.EmployeeId=e.id left join
 (select QT.DateFrom,
         att.ShiftId,
	     case when timefrom ='' then null else timefrom end TimeFrom,
	     case when timeto  ='' then null else timeto end TimeTo,
		 att.Late,
		 att.Leave,
		 att.ShiftTempId,
		 att.AttValue,
		 att.isStrict
from 
(
select Q.DateFrom,
       min(Q.TimeFrom)timeTotalfrom,
	   max(Q.TimeTo)timeTotalto
 from 
(
select DateFrom,TimeFrom,TimeTo,HoursNo,Remarks,AttendanceTypeId,EmployeeId from Attendances 
union
select DateFrom,TimeFrom,TimeTo,HoursNo,Remarks,AbsenceTypeId,EmployeeId from Absences
)Q
where Q.EmployeeId=(select id from Employees where FileNumber=@fno)
group by Q.DateFrom
)QT 
join
(
select Q.DateFrom,
       min(Q.TimeFrom)timefrom,
	   max(Q.TimeTo)timeto
from 
	(
		select DateFrom,
		       case when TimeFrom='' then NULL else TimeFrom end TimeFrom ,
		       case when TimeTo='' then NULL else TimeTo end TimeTo,
			   HoursNo,Remarks,AttendanceTypeId,EmployeeId from Attendances 
			union
		select DateFrom,
		       case when TimeFrom='' then NULL else TimeFrom end TimeFrom ,
			   case when TimeTo='' then NULL else TimeTo end TimeTo,
		       HoursNo,Remarks,AbsenceTypeId,EmployeeId from Absences where AbsenceTypeId=4
	)Q
where Q.EmployeeId=(select id from Employees where FileNumber=@fno)
group by Q.DateFrom
)QN on qt.DateFrom=qn.DateFrom left join
(
select datefrom,EmployeeId,ShiftId,ShiftTempId,Late,AttValue,isStrict,Leave from Attendances
) att on att.DateFrom=qt.DateFrom and att.EmployeeId=(select id from Employees where FileNumber=@fno)
) a on( cte.dateDay=a.DateFrom ) left join
Shifts sh on (
               a.ShiftId is null and
               (
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
			  )
			  ) or
			  (
			    a.ShiftId is not null and a.ShiftId=sh.ShiftId and a.isStrict=1
			  )
			  or
			  (
			    a.ShiftId is not null and a.ShiftId=sh.ShiftId and (a.isStrict is null or a.isStrict=0)or
				a.ShiftId is not null and a.ShiftId=sh.shift2 and (a.isStrict is null or a.isStrict=0)or
				a.ShiftId is not null and a.ShiftId=sh.shift3 and (a.isStrict is null or a.isStrict=0)
			  )

left join 
(
select ShiftId,DelayFromToes.* from Delays  
left join DelayFromToes  on Delays.Id=DelayId

)dly on (
           dly.DelayId = sh.XDelayId and
		   a.TimeFrom >= dly.[From] and
		   a.TimeFrom <  dateadd(MINUTE,1,dly.[To])
		)
left join 
(
select ShiftId,LeaveFromToes.* from Delays  
left join LeaveFromToes  on Delays.Id=DelayId
)lev on (
           lev.DelayId = sh.XLeaveId and
		   a.TimeTo >= lev.[From] and
		   a.TimeTo <  dateadd(MINUTE,1,lev.[To])
		)
left join(select SUM(NoOfHour)NoOfHour,EmployeeId,Date from AdditionApprovals group by EmployeeId,Date )ad on (ad.Date=cte.dateDay and e.Id=ad.EmployeeId)
left join(
 select koko.*, koko2.DayPart'daypart2',koko2.TimeFrom'timefrom2',koko2.TimeTo'timeto2',koko2.AbsenceTypeId'AbsenceTypeId2',koko2.AbsenceType'AbsenceType2' from 
 (
 select ROW_NUMBER( )over (partition by datefrom order by datefrom ) n,
        EmployeeId,
		DayPart,
		DateFrom,
		AbsenceTypeId,
		(select name from AbsenceTypes where id=AbsenceTypeId) AbsenceType,
		TimeFrom,
		TimeTo
 from Absences 
 where EmployeeId=(select id from employees where FileNumber=@fno) and AbsenceTypeId not in (4,11)
 )koko left join
 (
 select ROW_NUMBER( )over (partition by datefrom order by datefrom ) n,
        EmployeeId,
		DayPart,
		DateFrom,
		AbsenceTypeId,
		(select name from AbsenceTypes where id=AbsenceTypeId) AbsenceType,
		TimeFrom,
		TimeTo
 from Absences 
 where EmployeeId=(select id from employees where FileNumber=@fno) and AbsenceTypeId not in (4,11)
 )koko2 on (koko.DateFrom=koko2.DateFrom and koko.n=1 and koko2.n=2)
 where  koko.n=1
 ) abc on (e.id=abc.EmployeeId and abc.datefrom=cte.dateDay)

 left join(
select EmployeeId,count(*)cnt,DateFrom
 from Absences where AbsenceTypeId  in(4)group by  EmployeeId,DateFrom) abc1 on (abc1.EmployeeId=e.id and abc1.datefrom=cte.dateDay)
 
 left join(
select EmployeeId,sum(DayPart)cnt,DateFrom
 from Absences where AbsenceTypeId  in(11)group by  EmployeeId,DateFrom) abc2 on (abc2.EmployeeId=e.id and abc2.datefrom=cte.dateDay)
 left join(
 select * from vacations where AttendanceValue=1 and Active =1
 )vac on vac.Date=cte.dateDay
)Final
)
select d.* ,
		case when DATEDIFF(HOUR,StartTime,d.timefrom)+delo.value<0 then 0 else DATEDIFF(HOUR,StartTime,d.timefrom)+delo.value end  'delo',
		case when DATEDIFF(HOUR,d.TimeTo,d.EndTime)-levo.value<0 then 0 else DATEDIFF(HOUR,d.TimeTo,d.EndTime)-levo.value end 'levo'
from(select ROW_NUMBER()over(partition by dateday order by attendbefore desc,abs(late))n,* from data)d   
        left join XLeave delo on DATEPART(minute,case when TimeFrom ='' then null else TimeFrom end) between delo.[from] and delo.[to]  
        left join XLeave levo on DATEPART(minute,case when TimeTo='' then null else TimeTo end) between levo.[from] and levo.[to]
where n=1 order by dateDay 

";
            #endregion
            SqlCommand com = new SqlCommand(sql);
            com.CommandType = CommandType.Text;
            com.Parameters.Add("@FNO", SqlDbType.Int).Value = fileNo;
            com.Parameters.Add("@m", SqlDbType.VarChar).Value = M;
            com.Parameters.Add("@y", SqlDbType.VarChar).Value = Y;
            //if ((new DateTime(2024, 4, 4) - DateTime.Now).Minutes > 0)
                return Json(db.toJSON(db.getData(com)), JsonRequestBehavior.AllowGet);
            //else return Json(new { Result = false }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SalaryFileNoV3(string fileNo, int M, int Y)//AMMAR+Ahmed Eid
        {
            #region query
            string sql = @" 
--declare @Y varchar(4)='2025',@M varchar(2)='04',@fno int ='2234'
declare @f date ,@t date,@empId int
,@jn date,@tr date --edit 
set @f=
(
select 
case when start <15 then 
cast(cast( @y AS varchar(4))+'-'+cast( @m AS varchar(2))+'-'+cast(Start as varchar(2)) as date) 
else
dateadd(month,-1,cast((cast( @y AS varchar(4))+'-'+cast( @m AS varchar(2))+'-'+cast(Start as varchar)) as date))
end
from Months
)
set @t=(select dateadd(day,-1,dateadd(month,1,@f)));
set @empId=(select id from Employees where FileNumber=@fno);
set @jn =(select datefrom from 
(select ROW_NUMBER()over (partition by FileNumber order by a.datefrom ) n,e.FileNumber,TerminationDate,a.datefrom
from  (select * from Employees where FileNumber=@fno) e join 
	  Generals g on e.GeneralId=g.id join 
	  Attendances a on a.EmployeeId=e.id 
where (a.AttValue > 0 OR a.AttValue IS NULL) AND g.TerminationDate is not null and a.DateFrom>g.TerminationDate
)q
where n=1
);--edit
set @jn=(select case when @jn is null then (select JoiningDate from personals where id = (select PersonalId from Employees where FileNumber=@fno ))else @jn end);
set @tr =(select TerminationDate from Generals where id=(select GeneralId from Employees where FileNumber=@fno));--edit

with month_cte as
( 
select @f dateDay
union All
select dateadd(day,1,dateDay)
from month_cte
where dateDay<@t
)
/************real code*************/
SELECT  DayName,
		dateDay,
		@jn joining,@tr terminate, 
		case when @tr is null then
									case when dateday>=@jn then 'active'
									     else 'inactive'
									end
			 when @tr is not null and @jn<@tr then 
									case when dateday>=@jn  and dateDay<@tr then 'active'
									     else 'inactive'
									end
			 when @tr is not null and @jn>@tr then
									case when dateday>=@tr  and dateDay<@jn then 'inactive'
									     else 'active'
									end
												
	    end 'indcator'/*edit*/,
		fno,
		weekend,
		name,
		dept,
		shift_id'ShiftId',
        shift_id_flag,
		worktime,
		StartTime,
		EndTime,
		TimeFrom,
		TimeTo,
		late lateHours,
		LateChecker,
		LateChecker2,
		Leave  leaveHours,
		leaveChecker,
		leaveChecker2,
		sumAtt,		
		EmployeeId,
		case when AttValue is null then isnull
	    (case when weekend=1 then 1
	       else  case
                      when TimeFrom is not null and TimeFrom <> '' and TimeTo is not Null and TimeTo <>''
	                    then 1  
		              else 0  
		   end 
	     end,0
        )-isnull(levpart,0)-isnull(dlypart,0)
	    else AttValue end 'att',
        att 'att2',
	    isnull(abc1_Part,0)+isnull(abc2_part,0) 'vacDay',
        CASE WHEN abc1_type = 3 THEN isnull(abc1_Part,0) ELSE 0 END
        +CASE WHEN abc2_type = 3 THEN isnull(abc2_part,0) ELSE 0 END 'vacDayBal',
		(select name from AbsenceTypes where id=abc1_type)AbsenceType,
		abc1_type AbsenceTypeId,
        (select name from AbsenceTypes where id=abc2_type)AbsenceType2,
		abc2_type AbsenceTypeId2,
		(select name from AbsenceTypes where id=abc1_type)AbsenceType,
		errand,
		special,
		official,
		overTimeC,
		overTimeHour,
		OvertimeRate,
		balance, 
		attValue_flag,
		late_flag,
		leave_flag 	,
        final.weekend_cancel  
/************ end real code *************/ 
FROM   (
		SELECT month_cte.dateDay,
			   DATENAME(DW,dateDay)'DayName',
			   case when sh.weekendFromDay <= sh.weekendToDay and DATEPART(DW, month_cte.dateDay)%7 between sh.weekendFromDay and sh.weekendToDay then 1
					when sh.weekendFromDay > sh.weekendToDay and  not (DATEPART(DW, month_cte.dateDay)%7 >  sh.weekendToDay and DATEPART(DW, month_cte.dateDay)%7 < sh.weekendFromDay) then 1  
					else 0
    			end 'weekend',
			   @fno 'fno',
			   sh.DailyHours*60 'worktime',
			   e.KnownAs 'name',
			   (select name from Departements where id= e.DepartementId )'dept' ,
			   ak.*,
			   case when abc1.TimeFrom<=abc1.TimeTo then 
                case when ak.TimeFrom >= abc1.TimeFrom and ak.TimeFrom <=abc1.TimeTo then 'true' else 'false' end 
					else 
			    case when not(ak.TimeFrom >= abc1.TimeTo and ak.TimeFrom <=abc1.TimeFrom) then 'true' else 'false' end 
			   end 'LateChecker',
			    case when abc2.TimeFrom<=abc2.TimeTo then 
                case when ak.TimeFrom >= abc2.TimeFrom and ak.TimeFrom <=abc2.TimeTo then 'true' else 'false' end 
					else 
			    case when not(ak.TimeFrom >= abc2.TimeTo and ak.TimeFrom <=abc2.TimeFrom) then 'true' else 'false' end 
			   end 'LateChecker2',
			   case when abc1.TimeFrom<=abc1.TimeTo then 
                case when ak.TimeTo >= abc1.TimeFrom and ak.TimeTo <=abc1.TimeTo then 'true' else 'false' end 
					else 
			    case when not(ak.TimeTo >= abc1.TimeTo and ak.TimeTo <=abc1.TimeFrom) then 'true' else 'false' end 
				end 'leaveChecker',
				 case when abc2.TimeFrom<=abc2.TimeTo then 
                case when ak.TimeTo >= abc2.TimeFrom and ak.TimeTo <=abc2.TimeTo then 'true' else 'false' end 
					else 
			    case when not(ak.TimeTo >= abc2.TimeTo and ak.TimeTo <=abc2.TimeFrom) then 'true' else 'false' end 
				end 'leaveChecker2',
			   --attend,
			   abc1.DayPart'abc1_Part',
			   abc1.AbsenceTypeId'abc1_type',
			   abc2.DayPart'abc2_part',
			   abc2.AbsenceTypeId'abc2_type',
			   errand.cnt'errand',
			   special.cnt'special',
			   vac.AttendanceValue'official',
			   cast(DATEDIFF(minute,ak.TimeTo,ak.EndTime) as float(2))/60'overtimeC',
			   NoOfHour'overTimeHour',
			   (select Overtime from BasicBayWorks where EmployeeId=@empId)'OvertimeRate',
			   (
				select  
				ISNULL(
				case 
					 when DATEPART(DAY,VacationDeferredDate)>=26 and DATEPART(DAY,@t)>=26 then DATEDIFF(MONTH,VacationDeferredDate,@t)-1+1
					 when DATEPART(DAY,VacationDeferredDate)< 26 and DATEPART(DAY,@t)>=26 then DATEDIFF(MONTH,VacationDeferredDate,@t)+1
					 when DATEPART(DAY,VacationDeferredDate)>=26 and DATEPART(DAY,@t)< 26 then DATEDIFF(MONTH,VacationDeferredDate,@t)-1
					 when DATEPART(DAY,VacationDeferredDate)< 26 and DATEPART(DAY,@t)< 26 then DATEDIFF(MONTH,VacationDeferredDate,@t)
				end*1.75,1.75)+ISNULL(NumberOfStageVacations,0) deferred
				from BasicBayWorks 
				where EmployeeId=(select id from Employees where FileNumber=@fno)
				)
			   -isnull((
				select sum(a.DayPart)  vacation
				from Absences a join 
					 BasicBayWorks b  on a.EmployeeId=b.EmployeeId 
				where (a.AbsenceTypeId=3 or a.AbsenceTypeId=7) 
					  and a.DateFrom>=VacationDeferredDate and a.DateFrom<=@t --@t
					  and a.EmployeeId=(select id from employees where FileNumber=@fno)
				),0)'balance'
      ,
                (	select isnull(weekend_cancel,0) 
	                from Attendances att_k 
	                where  att_k.EmployeeId=ak.employeeid and att_k.DateFrom=ak.date
                ) 'weekend_cancel'

		FROM month_cte left join  
			  
       
			 Employees e on e.id=@empId left join  
			 [dbo].[Ak_ATT_SH](@f,@t,@fno)ak on month_cte.dateDay=ak.date left join
	     Shifts sh on 
        ( 
          (sh.id=ak.shift_id AND ak.shift_id IS NOT NULL)
          OR 
          (sh.id=(select ShiftId from BasicBayWorks where EmployeeId=@empId) AND ak.shift_id IS NULL)
        ) 
			 left join 
				(
					select * 
					from
					(
					select row_number() over(partition by datefrom order by datefrom ) n, * from Absences 
					where DateFrom between @f and @T and AbsenceTypeId not in (4,11,16) and EmployeeId=@empId
					)q  
					where q.n=1
				)abc1 on  abc1.datefrom =month_cte.dateDay 
				/*1الاجازات*/
				left join 
				(
				select row_number() over(partition by datefrom order by datefrom ) n, * from Absences 
				where DateFrom between @f and @T and AbsenceTypeId not in (4,11,16) and EmployeeId=@empId
				)abc2 on  abc2.datefrom =month_cte.dateDay  and abc2.n=2
				/*2الاجازات*/
				left join 
				(
				select count(*) cnt,datefrom,EmployeeId from Absences where DateFrom between @f and @T and AbsenceTypeId  in (4) 
				group by datefrom,EmployeeId
				)errand on errand.datefrom =month_cte.dateDay and errand.employeeId=@empId
				/*المأموريات*/
				left join 
				(
				select sum(HoursNo) cnt,datefrom,EmployeeId  from Absences where DateFrom between @f and @T and AbsenceTypeId in (11) 
				group by datefrom,EmployeeId
				)special on special.datefrom =month_cte.dateDay and special.employeeId=@empId
				/*المأموريات الخاصة*/
				left join
				(
				 select * from vacations where AttendanceValue=1 and Active =1
				 )vac on vac.Date=month_cte.dateDay
				 /* اجازة سنوية*/
				 left join
				 (
				 select SUM(NoOfHour)NoOfHour,Date from AdditionApprovals where EmployeeId=@empId  group by Date 
				 )ad on ad.Date=month_cte.dateDay
			)final
				 /*الاضافي*/
order by final.dateDay
";
            #endregion
            SqlCommand com = new SqlCommand(sql);
            com.CommandType = CommandType.Text;
            com.Parameters.Add("@FNO", SqlDbType.Int).Value = fileNo;
            com.Parameters.Add("@m", SqlDbType.VarChar).Value = M;
            com.Parameters.Add("@y", SqlDbType.VarChar).Value = Y;
            //if ((new DateTime(2024,4, 4) - DateTime.Now).Minutes > 0) 
            return Json(db.toJSON(db.getData(com)), JsonRequestBehavior.AllowGet);
            //else return Json(new {Result = false }, JsonRequestBehavior.AllowGet); 
        }
        public JsonResult AttPeriod_api(string date1, string date2)//AMMAR+Ahmed Eid+AK
        {
            #region query
            string sql = @" declare @f date = '"+date1+"',@t date = '"+date2+@"'
select KnownAs,
        FileNumber,
        color,
        TimeFrom,
        TimeTo,
        sumAtt,
        shift_id,
        att,
        attend,
        Name
from
employees e left join
(select attsh.date,
        attsh.StartTime,
        attsh.EndTime,
        attsh.TimeFrom,
        attsh.TimeTo,
        attsh.sumAtt,
        attsh.shift_id,
        attsh.shift_id_flag,
        EmployeeId,
		case when Late is null then dls.value else late end 'late',
		case when Late is null then 0 else 1 end 'late_flag',
		case when Leave is null then lvs.value else Leave end 'Leave',
		case when Leave is null then 0 else 1 end 'leave_flag',
        attsh.AttValue,
		case when attsh.AttValue is null then 0 else 1 end 'AttValue_flag',
        dls.DayPart dlypart,
        lvs.DayPart levpart,
        isnull(
		case when DATEDIFF(minute, TimeFrom, TimeTo)<0 then  24*60+DATEDIFF(minute, TimeFrom, TimeTo)
			 else DATEDIFF(minute, TimeFrom, TimeTo)

        end 
			 +/*التاخير*/
		case when abs(case when DATEDIFF(minute, StartTime, TimeFrom)<0 then DATEDIFF(minute, StartTime, TimeFrom)

            when(DATEDIFF(minute, StartTime, TimeFrom)>0 and dls.val is not null ) then DATEDIFF(minute, StartTime, TimeFrom)
			else 0
			end)>DailyHours*60 then
            (case when DATEDIFF(minute, StartTime, TimeFrom)<0 then DATEDIFF(minute, StartTime, TimeFrom)

            when(DATEDIFF(minute, StartTime, TimeFrom)>0 and dls.val is not null ) then DATEDIFF(minute, StartTime, TimeFrom)
			else 0
			end)+24*60
		else
			(case when DATEDIFF(minute, StartTime, TimeFrom)<0 then DATEDIFF(minute, StartTime, TimeFrom)

            when(DATEDIFF(minute, StartTime, TimeFrom)>0 and dls.val is not null ) then DATEDIFF(minute, StartTime, TimeFrom)
			else 0
			end)
		end
			   +/*الانصراف المبكر لم يتم*/
		case when abs(case when DATEDIFF(minute, TimeTo, EndTime)<0 then DATEDIFF(minute, TimeTo, EndTime)

            when(DATEDIFF(minute, TimeTo, EndTime)>0 and lvs.val is not null) then DATEDIFF(minute, TimeTo, EndTime)-- يلزم جدول للقراءة
			else 0
			end)>DailyHours*60 then
        (case when DATEDIFF(minute, TimeTo, EndTime)<0 then DATEDIFF(minute, TimeTo, EndTime)

            when(DATEDIFF(minute, TimeTo, EndTime)>0 and lvs.val is not null) then DATEDIFF(minute, TimeTo, EndTime)-- يلزم جدول للقراءة
			else 0
			end) +24*60
		else
		(case when DATEDIFF(minute, TimeTo, EndTime)<0 then DATEDIFF(minute, TimeTo, EndTime)

            when(DATEDIFF(minute, TimeTo, EndTime)>0 and lvs.val is not null) then DATEDIFF(minute, TimeTo, EndTime)-- يلزم جدول للقراءة
			else 0
			end)
		end,0)  
     'attend',
	  case when TimeFrom is not null and TimeFrom<> '' and TimeTo is not Null and TimeTo <>''

               then 1-isnull(dls.DayPart,0)-isnull(lvs.DayPart,0)
		  else 0
	 End 'att'
from
    (
        select date,
                StartTime,
                EndTime,
				case when TimeFrom = '' then null else TimeFrom end 'TimeFrom',
				case when TimeTo = '' then null else TimeTo end 'TimeTo',
                sumAtt,
				case when att.ShiftId is null then ak.shift_id else att.ShiftId end 'shift_id',--shift_id,
				case when att.ShiftId is null then 0 else 1 end 'shift_id_flag',
                ak.EmployeeId,
                Late,
                Leave,
				--ShiftId,
				(select DailyHours from Shifts where id = ak.shift_id)'DailyHours',
				ak.AttValue
        from Ak_Algo_all_V3(@f,@t/*,@fno*/) ak left join
            (
                select EmployeeId,
                        Late,
                        Leave,
                        NULL ShiftId,
                        AttValue,
                        DateFrom
                from Attendances
                where DateFrom >= @f and DateFrom <= @T /*and EmployeeId=(select id from Employees where FileNumber=@fno)*/

            ) att on ak.date=att.DateFrom and ak.EmployeeId=att.EmployeeId 
		)attsh left join
        /* الحضور واختيار الوردية */
        (
            select sh.Id,
            [From],
            [To],
            isnull(datepart(HOUR, Value)+cast(datepart(MINUTE, Value) as float)/60,0) 'value',
			Value 'val',		
			DayPart
            from Shifts sh left join delays d on sh.XDelayId=d.Id left join DelayFromToes dft on d.Id= dft.DelayId
        )dls on attsh.shift_id=dls.Id and attsh.TimeFrom >= dls.[From]
        and attsh.TimeFrom<dateadd(minute,1,dls.[To]) left join
       /*التاخيرات*/
       (
           select sh.Id,
           [From],
           [To],
           isnull(datepart(HOUR, Value)+cast(datepart(MINUTE, Value) as float)/60,0) 'value',		
			value 'val',
			DayPart
            from Shifts sh left join delays d on sh.XLeaveId=d.Id left join LeaveFromToes dft on d.Id= dft.DelayId
        )lvs on attsh.shift_id=lvs.Id and attsh.TimeTo >= lvs.[From]
       and attsh.TimeTo<dateadd(minute,1,lvs.[To])
)que on e.Id=que.EmployeeId
join Personals p on p.id=e.PersonalId
join Departements d on d.groups= 0 and d.Id= e.DepartementId
where p.StatusId= 1 and e.DepartementId<>351";
            #endregion
            SqlCommand com = new SqlCommand(sql);
            com.CommandType = CommandType.Text;        
            return Json(db.toJSON(db.getData(com)), JsonRequestBehavior.AllowGet);
        }
        public JsonResult product(string dept, DateTime dateFrom, DateTime dateTo)//AMMAR+Ahmed Eid
        {
            #region query
            string sql = @" 
--declare @Y varchar(4)='2023',@M varchar(2)='10',@fno int ='299'
declare @f date ,@t date,@empId int
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
set @empId=(select id from Employees where FileNumber=@fno);
set @f=dateadd(day,1-(datepart(DW,@F)+1)%7,@f);
set @t=dateadd(day,-1*(datepart(DW,@t)+1)%7,@t);
with month_cte as
( 
select @f dateDay
union All
select dateadd(day,1,dateDay)
from month_cte
where dateDay<@t
)
select case when datepart(DW,date)=7 then DATEPART(WEEK,dateDay)+1 else DATEPART(WEEK,dateDay) end wk,
	   datename(DW,dateDay)name,
	   (datepart(DW,date)+1)%7 i,
	   dateDay,
	   isnull(abc,0)abc,
	   isnull(att,0)att,
	   1-isnull(att,0)-isnull(abc,0) no 
from month_cte left join 
 Ak_ATT_SH(@f,@t,@fno)ak on month_cte.dateDay=ak.date left join 
     (
	 select  Employeeid ,sum(daypart) abc,DateFrom from Absences where AbsenceTypeId not in(4,11)group by EmployeeId,DateFrom
	 ) ab on ab.EmployeeId=ak.EmployeeId and DateFrom=date 
";
            #endregion
            SqlCommand com = new SqlCommand(sql);
            com.CommandType = CommandType.Text;
            com.Parameters.Add("@dept", SqlDbType.Int).Value = dept;
            com.Parameters.Add("@dateFrom", SqlDbType.Date).Value = dateFrom;
            com.Parameters.Add("@dateTo", SqlDbType.Date).Value = dateTo;
            //if ((new DateTime(2024, 4, 4) - DateTime.Now).Minutes > 0) 
            return Json(db.toJSON(db.getData(com)), JsonRequestBehavior.AllowGet);
            //else return Json(new { Result = false }, JsonRequestBehavior.AllowGet); 
        }  
    }
}
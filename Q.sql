declare @Y varchar(4)='2023',@M varchar(2)='09',@fno int ='49'
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
		fno,
		weekend,
		name,
		dept,
		shift_id'ShiftId',
		worktime,
		StartTime,
		EndTime,
		TimeFrom,
		TimeTo,
		late lateHours,
		LateChecker,
		LateChecker2,
		Leave LeaveHours,
		leaveChecker,
		leaveChecker2,
		sumAtt,		
		EmployeeId,
		levpart,
		dlypart,
		attend,
		worktime-attend,
		case 
		when AttValue is null then isnull(
			case when weekend=1 then 1
			   else  
					case when attend=0 then 0
					else case 
					        when worktime-attend>worktime/2 then 0 
							when worktime-attend>worktime/4 and worktime-attend<=worktime/2 then .5
							when worktime-attend>0 and worktime-attend<=worktime/4 then .75 
							when worktime-attend<=0 then 1 
			     end 
			   end
		  end
		  ,0)-isnull(levpart,0)-isnull(dlypart,0)
	   else AttValue 
	   end 'att',
	   isnull(abc1_Part,0)+isnull(abc2_part,0) 'vacDay',
		worktime,
	
		
		(select name from AbsenceTypes where id=abc1_type)abc1_type_name,
		abc1_type,
		
		abc2_type,
		errand,
		special,
		official,
		overtimeC,
		overTimeHour,
		OvertimeRate,
		balance 	   
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
			   cast(DATEDIFF(minute,ak.EndTime,ak.TimeTo) as float(2))/60'overtimeC',
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

		FROM month_cte left join  
			 Shifts sh on sh.id=(select ShiftId from BasicBayWorks where EmployeeId=@empId)left join
			 Employees e on e.id=@empId left join  
			 [dbo].[Ak_ATT_SH](@f,@t,@fno)ak on month_cte.dateDay=ak.date 
	 
			 left join 
				(
					select * 
					from
					(
					select row_number() over(partition by datefrom order by datefrom ) n, * from Absences 
					where DateFrom between @f and @T and AbsenceTypeId not in (4,11) and EmployeeId=@empId
					)q  
					where q.n=1
				)abc1 on  abc1.datefrom =month_cte.dateDay 
				/*1الاجازات*/
				left join 
				(
				select row_number() over(partition by datefrom order by datefrom ) n, * from Absences 
				where DateFrom between @f and @T and AbsenceTypeId not in (4,11) and EmployeeId=@empId
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
				select count(*) cnt,datefrom,EmployeeId  from Absences where DateFrom between @f and @T and AbsenceTypeId in (11) 
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

declare @fno int=2234,@m varchar(2)='03',@y varchar(4)='2023';
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
	  TimeFrom,
	  TimeTo,
	  lateHours,
	  LateChecker,
	  LateChecker2,
	  leaveHours,
	  leaveChecker,
	  leaveChecker2,
	  attendbefore,
	  isnull
	  (case when weekend=1 then 1
	       else  case when worktime-attend>worktime/2 then 0 
					  when worktime-attend>worktime/4 and worktime-attend<=worktime/2 then .5
					  when worktime-attend>0 and worktime-attend<=worktime/4 then .75 
					  when worktime-attend=0 then 1 
		   end 
	  end,0) 'att',
	  vacDay,
	  worktime,
	  cast(overtime as float(2))/60 'overTimeC',
	  overTimeHour,
	  --vacMinute,
	  AbsenceType,
	  AbsenceTypeId,
	  errand,
	  special,
	  official
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
	 sh.ShiftId,
	 sh.StartTime,
	 sh.EndTime,
	 sh.EarlyArrive,
	 a.TimeFrom,
	 a.TimeTo,
	 
	 DATEDIFF(minute,StartTime,a.TimeFrom)late,
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
	 DATEDIFF(minute,a.TimeFrom,a.TimeTo)'attendbefore',
	 case when DATEDIFF(minute,a.TimeFrom,a.TimeTo)<0 then  24*60+DATEDIFF(minute,a.TimeFrom,a.TimeTo)
	 else DATEDIFF(minute,a.TimeFrom,a.TimeTo)
	 end 
	 +/*التاخير*/
	 case when DATEDIFF(minute,StartTime,a.TimeFrom)<0 then DATEDIFF(minute,StartTime,a.TimeFrom)
	      when (DATEDIFF(minute,StartTime,a.TimeFrom)>0 and dly.value is not null ) then DATEDIFF(minute,StartTime,a.TimeFrom)
		  else 0
	  end
	   +/*الانصراف المبكر لم يتم*/
	 case when DATEDIFF(minute,a.TimeTo,EndTime)<0 then DATEDIFF(minute,a.TimeTo,EndTime)
	      when (DATEDIFF(minute,a.TimeTo,EndTime)>0 and lev.value is not null) then DATEDIFF(minute,a.TimeTo,EndTime)-- يلزم جدول للقراءة
		  else 0
	  end'attend',
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
	 ,vac.AttendanceValue'official'
	 --,py.Amount
	 --,py.PaymentDeduction
	 --,py.PaymentDeductionId
FROM
(select DATENAME(DW,dateDay)DayName, dateDay,@fno fno from month_cte)cte left join 
 Employees E on e.FileNumber=cte.fno left join
 BasicBayWorks b on b.EmployeeId=e.id left join
 
 (select QT.DateFrom,
         att.ShiftId,
	     case when TimeFrom ='' then null else TimeFrom end TimeFrom,
	     case when TimeTo  ='' then null else TimeTo end TimeTo,
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
		select DateFrom,TimeFrom,TimeTo,HoursNo,Remarks,AttendanceTypeId,EmployeeId from Attendances 
			union
		select DateFrom,TimeFrom,TimeTo,HoursNo,Remarks,AbsenceTypeId,EmployeeId from Absences where AbsenceTypeId=4
	)Q
where Q.EmployeeId=(select id from Employees where FileNumber=@fno)
group by Q.DateFrom
)QN on qt.DateFrom=qn.DateFrom left join
(select datefrom,EmployeeId,ShiftId,ShiftTempId,Late,AttValue,isStrict,Leave from Attendances) att on att.DateFrom=qt.DateFrom and att.EmployeeId=(select id from Employees where FileNumber=@fno)
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
           dly.ShiftId = sh.ShiftId and
		   a.TimeFrom >= dly.[From] and
		   a.TimeFrom <  dateadd(MINUTE,1,dly.[To])
		)
left join 
(
select ShiftId,LeaveFromToes.* from Delays  
left join LeaveFromToes  on Delays.Id=DelayId
)lev on (
           lev.ShiftId = sh.ShiftId and
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
select EmployeeId,sum(DayPart)cnt,DateFrom
 from Absences where AbsenceTypeId  in(11)group by  EmployeeId,DateFrom) abc1 on (abc1.EmployeeId=e.id and abc1.datefrom=cte.dateDay)
 
 left join(
select EmployeeId,sum(DayPart)cnt,DateFrom
 from Absences where AbsenceTypeId  in(4)group by  EmployeeId,DateFrom) abc2 on (abc2.EmployeeId=e.id and abc2.datefrom=cte.dateDay)
 left join(
 select * from vacations where AttendanceValue=1 and Active =1
 )vac on vac.Date=cte.dateDay
)Final
)
select d.* from data d where n=1 order by dateDay 
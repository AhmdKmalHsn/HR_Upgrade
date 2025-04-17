select * from
(select ROW_NUMBER()over (partition by date order by sumAtt desc )n,* from (
select date,StartTime,EndTime,TimeFrom,TimeTo,
	   dbo.calc(q1.in1,q1.out1,q1.timeF1,timeT1) +dbo.calc(q1.in2,q1.out2,q1.timeF2,timeT2)+dbo.calc(q1.in1,q1.out1,q1.timeF2,timeT2)+dbo.calc(q1.in2,q1.out2,q1.timeF1,timeT1)sumAtt

from (
select cast(DateFrom as date) date,	   
	   case 
			when StartTime<EndTime then StartTime 
			when StartTime>EndTime then StartTime 
	   end in1,
	   case 
			when StartTime<EndTime then EndTime
			when StartTime>EndTime then '23:59:59'
	   end out1,
	   case 
			when StartTime<EndTime then null 
			when StartTime>EndTime then '00:00:00' 
	   end in2,
	   case 
			when StartTime<EndTime then null
			when StartTime>EndTime then EndTime
	   end out2,
	   /*********************************************/
	   case 
			when timeFrom<timeTo then timeFrom 
			when timeFrom>timeTo then timeFrom 
	   end timeF1,
	   case 
			when timeFrom<timeTo then timeTo
			when timeFrom>timeTo then '23:59:59'
	   end timeT1,
	   case 
			when timeFrom<timeTo then null 
			when timeFrom>timeTo then '00:00:00' 
	   end timeF2,
	   case 
			when timeFrom<timeTo then null
			when timeFrom>timeTo then timeTo
	   end timeT2,
       TimeFrom,
	   TimeTo,
	   StartTime,
	   EndTime	
from Attendances a,
    (select StartTime,EndTime,DailyHours,0 offset1, case when StartTime<EndTime then 0 else 1 end offset2 from Shifts where id=22 or Shift2=22 or Shift3=22)sh 
where EmployeeId=(select id from Employees where FileNumber=367) and DateFrom>'2023-08-25' 
)q1
)q2
)t0 where n=1
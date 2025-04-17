select DateFrom,timefrom,case when delo<0 or delo is null then 0 else delo end delo
               ,TimeTo,case when levo<0 or levo is null then 0 else levo end levo
 from (
select a.DateFrom,a.TimeFrom,DATEDIFF(HOUR,'10:00',a.timefrom)+delo.value'delo',
                  a.TimeTo,DATEDIFF(HOUR,a.TimeTo,'19:00')-levo.value'levo'
from(select case when TimeFrom ='' then null else TimeFrom end TimeFrom,case when TimeTo='' then null else TimeTo end TimeTo,EmployeeId,DateFrom from Attendances )a 
					join XLeave delo on DATEPART(minute,a.timefrom) between delo.[from] and delo.[to]  
                    join XLeave levo on DATEPART(minute,a.TimeTo) between levo.[from] and levo.[to]
where a.EmployeeId=(select id from Employees where FileNumber=10002) and a.DateFrom>'2023-04-25'
)said 


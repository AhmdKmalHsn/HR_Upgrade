using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BioMetrixCore;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace newHR.Controllers
{
    public class MacController : Controller
    {
        // GET: Mac

        AhmdKmal ak = new AhmdKmal();
        MacDB macDB = new MacDB();
        public ActionResult MacUp()
        {
            return View();
        }
        public ActionResult MacAtt()
        {
            return View();
        }
        public ActionResult MacReport()
        {
            return View();
        }
        public JsonResult MoveReport(DateTime f,DateTime t) {
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
            com.Parameters.Add("@f", SqlDbType.Date).Value = f.Date;
            com.Parameters.Add("@t", SqlDbType.Date).Value = t.Date;
            return Json(db.toJSON(db.getData(com)), JsonRequestBehavior.AllowGet);
        }
        public JsonResult MoveReportFN(int FN,DateTime f, DateTime t)
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
where cast(Datetime as date) >= @f and cast(Datetime as date) <= @t and A.FileNumber=@fn ";
            com.CommandText = sql;
            com.Parameters.Add("@fn", SqlDbType.Int).Value = FN;
            com.Parameters.Add("@f", SqlDbType.Date).Value = f.Date;
            com.Parameters.Add("@t", SqlDbType.Date).Value = t.Date;
            return Json(db.toJSON(db.getData(com)), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetMacAtt( DateTime F, DateTime T)
        {
            return Json(macDB.getMacAtt(F, T), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetMacAttNew(DateTime F, DateTime T)
        {
            return Json(macDB.getMacAttNew(F, T), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetMacAttNewFN(DateTime F, DateTime T,int FN)
        {
            return Json(macDB.getMacAttNewFN(F, T,FN), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetMacAttNewDept(DateTime F, DateTime T,int dept)
        {
            return Json(macDB.getMacAttNewDept(F, T,dept), JsonRequestBehavior.AllowGet);
        }
        public JsonResult SetMacAtt(int FN,string DT,string F, string T)
        {
            return Json(macDB.ATT_IT(FN,DT,F, T), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAllLogs(string ip)
        {
            return Json(ak.data(ip), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAllLogsBetween(string ip,DateTime F,DateTime T)
        {
            return Json(ak.dataBetween(ip,F,T), JsonRequestBehavior.AllowGet);
        }
        public JsonResult getdata(int Id)
        {
            return Json(macDB.getMac(Id), JsonRequestBehavior.AllowGet);
        }
        public JsonResult delete(int Id)
        {
            return Json(macDB.delete(Id), JsonRequestBehavior.AllowGet);
        }
        public JsonResult create(string MacIP, string MacType, string MacName)
        {
            return Json(macDB.create( MacIP,  MacType,  MacName), JsonRequestBehavior.AllowGet);
        }
        public JsonResult update(int id, string MacIP, string MacType, string MacName)
        {
            return Json(macDB.update(id,  MacIP,  MacType,  MacName), JsonRequestBehavior.AllowGet);
        }
        public JsonResult upload(int FN, string DT, string TP, string IP)
        {
            return Json(macDB.upload(FN, DT, TP,IP), JsonRequestBehavior.AllowGet);
        }

        #region mac crud
        public class Mac
        {
            public int Id { set; get; }
            public string MacIP { set; get; }
            public string MacType { set; get; }
            public string MacName { set; get; }
            public int IsFixed { set; get; }
        }
        public class Att {
            public int fileNumber { set; get; }
            public string name { set; get; }
            public string dept { set; get; }
            public string date { set; get; }
            public string timeFrom { set; get; }
            public string timeTo { set; get; }

        }
        public class AttDetails
        {
            public int fileNumber { set; get; }
            public string datetime { set; get; }
            public string type { set; get; }
        }
        public class MacDB
        {
            string cs = ConfigurationManager.ConnectionStrings["CS"].ConnectionString;
            #region select
            public List<Mac> getMac(int id)
            {
                List<Mac> lst = new List<Mac>();
                string sql = @"
select Id,MacIP,MacType,MacName from Mac
where Id=@id
       ";
                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();
                    SqlCommand com = new SqlCommand(sql, con);
                    com.CommandType = CommandType.Text;
                    com.Parameters.Add("@id", SqlDbType.Int).Value = id;

                    SqlDataReader rdr = com.ExecuteReader();
                    while (rdr.Read())
                    {
                        lst.Add(new Mac
                        {
                            Id = rdr["Id"].ToString() == "" ? 0 : int.Parse(rdr["Id"].ToString()),
                            MacIP = rdr["MacIP"].ToString(),
                            MacType = rdr["MacType"].ToString(),
                            MacName = rdr["MacName"].ToString()
                        });
                    }
                    return lst;
                }
            }
            #endregion
            #region macAtt
            public List<Att> getMacAtt(DateTime F,DateTime T)//very very  important رفع الحضور والانصراف من البصمة
            {
                List<Att> lst = new List<Att>();
                string sql = @"WITH CTE_Months
AS
(
	SELECT @f dates
	UNION ALL
	SELECT DATEADD(DAY,1,dates)
	FROM CTE_Months
	WHERE DATEADD(DAY,1,dates) <= @t
),
Q AS( 
select coll2.dates,
       coll2.FileNumber,
       logsIn.DateTime 'intime',
	   logsOut.DateTime 'outtime',
	   DATEDIFF(minute, logsIn.DateTime,logsOut.DateTime) diff
from
(select 
  dates,
  dates1,
  dates2,
  coll1.FileNumber,
  StartTime,
  EndTime,
  dateadd(minute,on1,CONVERT(DATETIME, CONVERT(CHAR(8), dates1, 112) + ' ' + CONVERT(CHAR(8), StartTime, 108)))in1,
  dateadd(minute,on2,CONVERT(DATETIME, CONVERT(CHAR(8), dates2, 112) + ' ' + CONVERT(CHAR(8), EndTime, 108)))in2,
  --logsIn.DateTime 'LogIn',
  dateadd(minute,off1,CONVERT(DATETIME, CONVERT(CHAR(8), dates1, 112) + ' ' + CONVERT(CHAR(8), StartTime, 108)))out1,
  dateadd(minute,off2,CONVERT(DATETIME, CONVERT(CHAR(8), dates2, 112) + ' ' + CONVERT(CHAR(8), EndTime, 108)))out2
from 
(
select CTE_Months.dates,
case when cast(StartTime as time(0))<'04:00' then DATEADD(day,1,dates) else dates 
     end dates1,
case when cast(EndTime as time(0))<cast(StartTime as time(0)) then DATEADD(day,1,dates)
     when cast(StartTime as time(0))<'04:00' then DATEADD(day,1,dates) else dates 
     end  dates2, 
e.id,
e.FileNumber,
s.ShiftId ,
case when s.DailyHrs is null then s.DailyHours else s.DailyHrs end dailyhours
,s.StartTime,
case when s.ShiftId=28 then -5*60 else -2*60 end 'on1',
-2*60 'on2',
s.EndTime,
case when s.NoOfShifts=0 then DailyHours/2*60-67
     when s.NoOfShifts=1 then DailyHours/2*60-67
	 when s.NoOfShifts=2 then DailyHours/2*60-67
end 'off1',
case when s.NoOfShifts=0 then 7*60
     when s.NoOfShifts=1 and (s.ShiftId=18 or s.ShiftId=19) then 5*60
	 when s.NoOfShifts=1 and not (s.ShiftId=18 or s.ShiftId=19) then 3*60
	 when s.NoOfShifts=2 then 2*60
end 'off2'

from CTE_Months,Employees e,BasicBayWorks b, Shifts s
where e.id=b.EmployeeId and 
(b.ShiftId=s.ShiftId or b.ShiftId=s.Shift2 or b.ShiftId=s.Shift3 )
)coll1
)coll2
left join 
( 
select  FileNumber,DateTime from AClogs where Type='In'
)logsIn on( logsIn.FileNumber=coll2.FileNumber and logsIn.DateTime >=coll2.in1 and logsIn.DateTime<=coll2.in2)
left join 
( 
select  FileNumber,DateTime from AClogs where Type='Out'
)logsOut on( logsOut.FileNumber=coll2.FileNumber and logsOut.DateTime >=coll2.out1 and logsOut.DateTime<=coll2.out2) 
where not (logsIn.DateTime is null and logsout.DateTime is  null) 
) 
select dates'date',fin.FileNumber,cast(intime as time(0))'timeIN',cast(outtime as time(0))'timeOUT',e.name,e.dept from (
select row_number() over(partition by dates,filenumber order by diff desc)n,
     dates,	FileNumber,	intime,outtime,diff
from  
(
select dates,	FileNumber,	min(intime)intime,max(outtime)outtime,max(diff)diff
from q 
where intime is not null and outtime is not null
group by dates,FileNumber
union all 
select dates,	FileNumber,min(intime),null outtime	,0
from q 
where outtime is null 
group by dates,FileNumber
union all 
select dates,	FileNumber,null	intime ,max (outtime),0	
from q 
where intime is null 
group by dates,FileNumber
)fine
)fin join (select FileNumber,KnownAs'name',Departements.Name'dept' from Employees,Departements where Employees.DepartementId=Departements.Id )e on e.FileNumber=fin.FileNumber
where fin.n=1 --and fin.FileNumber=@fno
order by cast(fin.FileNumber as int),date
";
                    /*@"WITH CTE_Months
AS
(
	SELECT @f dates
	UNION ALL
	SELECT DATEADD(DAY,1,dates)
	FROM CTE_Months
	WHERE DATEADD(DAY,1,dates) <= @t
) 
, tbl as (
select q.* 
from
(
select final.dates,
	   ac.FileNumber,
	   case when ac.Type='In' then min(ac.DateTime)
			when ac.type='Out'then max(ac.DateTime)
	   end 'DateTime',
	   ac.Type
	   From(
select dates
	  ,f.FileNumber
	  ,f.ShiftId
	  ,f.NoOfShifts
	  ,case when inTime>=cast('08:00:00'as time) then [In]
			else dateadd(day,1,[In])
			end'DateTimeIn'
	  ,case when inTime<cast('08:00:00'as time) then dateadd(day,1,[Out])
			when inTime<outTime then [Out]	        
			else dateadd(day,1,[Out])
			end'DateTimeOut'
	  
From
(
select * ,
CONVERT(DATETIME, CONVERT(CHAR(8), dates, 112) 
  + ' ' + CONVERT(CHAR(8), inTime, 108))'In',
  CONVERT(DATETIME, CONVERT(CHAR(8), dates, 112) 
  + ' ' + CONVERT(CHAR(8), outTime, 108))'Out'
from CTE_Months ct, 
(select e.FileNumber
	  ,sh.ShiftId
	  ,sh.DailyHrs
	  ,NoOfShifts
	  ,cast(sh.StartTime as time(0))inTime
	  ,cast(sh.EndTime as time(0))outTime
from 
Employees E left join
Personals p on p.Id=e.PersonalId left join 
BasicBayWorks b on e.Id=b.EmployeeId left join
Shifts sh on (b.ShiftId=sh.ShiftId or b.ShiftId=sh.Shift2 or b.ShiftId=sh.Shift3)
where p.StatusId<>3
  --and e.FileNumber=@FN
)det --on det.FileNumber=AClogs.FileNumber
)f
)Final join AClogs ac on(
ac.FileNumber=Final.FileNumber and
(
(ac.DateTime >= dateadd(hour,-2,Final.DateTimeIn) and ac.DateTime<=dateadd(HOUR,-2,Final.DateTimeOut)and Type='In')or
(ac.DateTime >= dateadd(hour,-7,Final.DateTimeIn) and ac.DateTime<=dateadd(HOUR,-2,Final.DateTimeOut)and Type='In' and Final.NoOfShifts=0 and Final.DateTimeIn<Final.DateTimeOut) or
(Final.NoOfShifts=0 and ac.DateTime >= dateadd(hour,2,Final.DateTimeIn) and ac.DateTime<=dateadd(HOUR,7,Final.DateTimeOut)and  type='Out' )or
(Final.NoOfShifts=1 and ac.DateTime >= dateadd(hour,2,Final.DateTimeIn) and ac.DateTime<=dateadd(HOUR,4,Final.DateTimeOut)and  type='Out' and Final.ShiftId<>18 and final.ShiftId<>19)or
(Final.NoOfShifts=1 and ac.DateTime >= dateadd(hour,2,Final.DateTimeIn) and ac.DateTime<=dateadd(HOUR,6,Final.DateTimeOut)and  type='Out' and (Final.ShiftId=18 or final.ShiftId=19))or
(Final.NoOfShifts=2 and ac.DateTime >= dateadd(hour,2,Final.DateTimeIn) and ac.DateTime<=dateadd(HOUR,2,Final.DateTimeOut)and  type='Out' ) 
)
)
group by final.dates,ac.FileNumber,ac.Type
)q
)
select query.*,e.knownas'name' from (
select case when q1.dates is null then q2.dates else q1.dates end'date' ,
       case when q1.FileNumber is null then q2.FileNumber else q1.FileNumber end'fileNumber',
	   cast( q1.DateTime as time(0))'timeIN',
	   cast( q2.DateTime as time(0))'timeOUT'
from  (select dates,FileNumber,DateTime from tbl a where a.Type='in')q1
full join (select dates,FileNumber,DateTime from tbl a where a.Type='out')q2 on (q1.dates=q2.dates and q1.FileNumber=q2.FileNumber )
)query join employees e on e.fileNumber=query.filenumber
order by date
       ";*/
                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();
                    SqlCommand com = new SqlCommand(sql, con);
                    com.CommandType = CommandType.Text;
                    //com.Parameters.Add("@FN", SqlDbType.Int).Value = FN;
                    com.Parameters.Add("@F", SqlDbType.Date).Value = F.Date;
                    com.Parameters.Add("@T", SqlDbType.Date).Value = T.Date;
                    SqlDataReader rdr = com.ExecuteReader();
                    while (rdr.Read())
                    {
                        lst.Add(new Att
                        {
                            date = rdr["date"].ToString(),
                            name = rdr["name"].ToString(),
                            dept = rdr["dept"].ToString(),
                            fileNumber = rdr["fileNumber"].ToString() == "" ? 0 : int.Parse(rdr["fileNumber"].ToString()),
                            timeFrom = rdr["timeIN"].ToString(),
                            timeTo = rdr["timeOUT"].ToString()
                        });
                    }
                    return lst;
                }
            }
            public List<Att> getMacAttNew(DateTime F, DateTime T)//very very  important رفع الحضور والانصراف من البصمة
            {
                List<Att> lst = new List<Att>();
                string sql = @"with acIn as (select * from AClogs where Type='In' and  cast(datetime as date) >=@f and cast(datetime as date) <=dateadd(day,1,@t)),
	 acOut as (select * from AClogs where Type='Out' and  cast(datetime as date) >=@f and cast(datetime as date) <=dateadd(day,1,@t))
	,month_cte as
( 
select @f dateDay
union All
select dateadd(day,1,dateDay)
from month_cte
where dateDay<@t
)
/******************************real_code********************************/
select 	final.FileNumber 'fileNumber',
		dateDay 'date',
		cast(inf as time(0)) 'timeIN',
		cast(outf as time(0)) 'timeOUT',	
		--worktime '',
		e.dept,
		e.name
from
(
	select ROW_NUMBER() over(partition by filenumber,dateday order by worktime desc ) n,num.* 
	from
	(
		select q.FileNumber,q.dateDay,q.Id,acIn.DateTime inf,acOut.DateTime outf,datediff(minute,acIn.DateTime,acOut.DateTime)worktime
		from 
			(select dateDay,
					FileNumber,
					Id,ShiftName,
					inbound,
					outbound,
					DailyHrs,
					dateadd(MINUTE,-1*60,inbound)inMin,
					dateadd(MINUTE,3*dailyhrs/4*60,inbound)inMax,
					dateadd(MINUTE,dailyhrs/4*60,inbound)outMin,
					case 
							when NoOfShifts=2  then dateadd(MINUTE,2*60,outbound) 
							when NoOfShifts=1  then dateadd(MINUTE,3.5*60,outbound) 
							else dateadd(MINUTE,5*60,outbound) 
					end outMax
			from
			(
				select  mon.dateDay,
						FileNumber,
						sh.Id,ShiftName,
						dateadd(day,inoffset,cast(dateDay as  varchar(10))+' '+sh.StartTime)inbound,
						dateadd(day,OutOffset,cast(dateDay as  varchar(10))+' '+sh.EndTime)outbound,
						sh.DailyHrs,
						sh.NoOfShifts
				from month_cte mon,
					 Employees e join 
					 BasicBayWorks b on e.id=b.EmployeeId join 
					 Shifts sh on b.ShiftId=sh.Id or sh.id=(select Shift2 from Shifts where id=b.ShiftId ) or sh.id=(select Shift3 from Shifts where id=b.ShiftId )
				)bound
			--where FileNumber=16107 --out
		)q left join acIn on q.FileNumber=acIn.FileNumber and acin.DateTime between q.inMin and q.inMax 
		   left join acOut on q.FileNumber=acOut.FileNumber and acOut.DateTime between q.outMin and q.outMax 
	)num
	
)final join (select FileNumber,KnownAs'name',Departements.Name'dept' from Employees,Departements where Employees.DepartementId=Departements.Id )e on e.FileNumber=final.FileNumber
where n=1 and not (inf is null and outf is null)
";

                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();
                    SqlCommand com = new SqlCommand(sql, con);
                    com.CommandType = CommandType.Text;
                    //com.Parameters.Add("@FN", SqlDbType.Int).Value = FN;
                    com.Parameters.Add("@F", SqlDbType.Date).Value = F.Date;
                    com.Parameters.Add("@T", SqlDbType.Date).Value = T.Date;
                    SqlDataReader rdr = com.ExecuteReader();
                    while (rdr.Read())
                    {
                        lst.Add(new Att
                        {
                            date = rdr["date"].ToString(),
                            name = rdr["name"].ToString(),
                            dept = rdr["dept"].ToString(),
                            fileNumber = rdr["fileNumber"].ToString() == "" ? 0 : int.Parse(rdr["fileNumber"].ToString()),
                            timeFrom = rdr["timeIN"].ToString(),
                            timeTo = rdr["timeOUT"].ToString()
                        });
                    }
                    return lst;
                }
            }
            public List<Att> getMacAttNewFN(DateTime F, DateTime T,int FN)//very very  important رفع الحضور والانصراف من البصمة
            {
                List<Att> lst = new List<Att>();
                string sql = @"with acIn as (select * from AClogs where Type='In' and  cast(datetime as date) >=@f and cast(datetime as date) <=dateadd(day,1,@t)),
	 acOut as (select * from AClogs where Type='Out' and  cast(datetime as date) >=@f and cast(datetime as date) <=dateadd(day,1,@t))
	,month_cte as
( 
select @f dateDay
union All
select dateadd(day,1,dateDay)
from month_cte
where dateDay<@t
)
/******************************real_code********************************/
select 	final.FileNumber 'fileNumber',
		dateDay 'date',
		cast(inf as time(0))'timeIN',
		cast(outf as time(0))'timeOUT',	
		--worktime '',
		e.dept,
		e.name
from
(
	select ROW_NUMBER() over(partition by filenumber,dateday order by worktime desc ) n,num.* 
	from
	(
		select q.FileNumber,q.dateDay,q.Id,acIn.DateTime inf,acOut.DateTime outf,datediff(minute,acIn.DateTime,acOut.DateTime)worktime
		from 
			(select dateDay,
					FileNumber,
					Id,ShiftName,
					inbound,
					outbound,
					DailyHrs,
					dateadd(MINUTE,-1*60,inbound)inMin,
					dateadd(MINUTE,3*dailyhrs/4*60,inbound)inMax,
					dateadd(MINUTE,dailyhrs/4*60,inbound)outMin,
					case 
							when NoOfShifts=2  then dateadd(MINUTE,2*60,outbound) 
							when NoOfShifts=1  then dateadd(MINUTE,3*60,outbound) 
							else  dateadd(MINUTE,5*60,outbound) 
					end outMax
			from
			(
				select  mon.dateDay,
						FileNumber,
						sh.Id,ShiftName,
						dateadd(day,inoffset,cast(dateDay as  varchar(10))+' '+sh.StartTime)inbound,
						dateadd(day,OutOffset,cast(dateDay as  varchar(10))+' '+sh.EndTime)outbound,
						sh.DailyHrs,
						sh.NoOfShifts
				from month_cte mon,
					 Employees e join 
					 BasicBayWorks b on e.id=b.EmployeeId join 
					 Shifts sh on b.ShiftId=sh.Id or sh.id=(select Shift2 from Shifts where id=b.ShiftId ) or sh.id=(select Shift3 from Shifts where id=b.ShiftId )
				)bound
			where FileNumber=@FN --out
		)q left join acIn on q.FileNumber=acIn.FileNumber and acin.DateTime between q.inMin and q.inMax 
		   left join acOut on q.FileNumber=acOut.FileNumber and acOut.DateTime between q.outMin and q.outMax 
	)num
	
)final join (select FileNumber,KnownAs'name',Departements.Name'dept' from Employees,Departements where Employees.DepartementId=Departements.Id )e on e.FileNumber=final.FileNumber
where n=1 and not (inf is null and outf is null)
";

                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();
                    SqlCommand com = new SqlCommand(sql, con);
                    com.CommandType = CommandType.Text;
                    com.Parameters.Add("@FN", SqlDbType.Int).Value = FN;
                    com.Parameters.Add("@F", SqlDbType.Date).Value = F.Date;
                    com.Parameters.Add("@T", SqlDbType.Date).Value = T.Date;
                    SqlDataReader rdr = com.ExecuteReader();
                    while (rdr.Read())
                    {
                        lst.Add(new Att
                        {
                            date = rdr["date"].ToString(),
                            name = rdr["name"].ToString(),
                            dept = rdr["dept"].ToString(),
                            fileNumber = rdr["fileNumber"].ToString() == "" ? 0 : int.Parse(rdr["fileNumber"].ToString()),
                            timeFrom = rdr["timeIN"].ToString(),
                            timeTo = rdr["timeOUT"].ToString()
                        });
                    }
                    return lst;
                }
            }
            public List<Att> getMacAttNewDept(DateTime F, DateTime T, int dept)//very very  important رفع الحضور والانصراف من البصمة
            {
                List<Att> lst = new List<Att>();
                string sql = @"with acIn as (select * from AClogs where Type='In' and  cast(datetime as date) >=@f and cast(datetime as date) <=dateadd(day,1,@t)),
	 acOut as (select * from AClogs where Type='Out' and  cast(datetime as date) >=@f and cast(datetime as date) <=dateadd(day,1,@t))
	,month_cte as
( 
select @f dateDay
union All
select dateadd(day,1,dateDay)
from month_cte
where dateDay<@t
)
/******************************real_code********************************/
select 	final.FileNumber 'fileNumber',
		dateDay 'date',
		cast(inf as time(0))'timeIN',
		cast(outf as time(0))'timeOUT',	
		--worktime '',
		e.dept,
		e.name
from
(
	select ROW_NUMBER() over(partition by filenumber,dateday order by worktime desc ) n,num.* 
	from
	(
		select q.FileNumber,q.dateDay,q.Id,acIn.DateTime inf,acOut.DateTime outf,datediff(minute,acIn.DateTime,acOut.DateTime)worktime
		from 
			(select dateDay,
					FileNumber,
					Id,ShiftName,
					inbound,
					outbound,
					DailyHrs,
					dateadd(MINUTE,-1*60,inbound)inMin,
					dateadd(MINUTE,3*dailyhrs/4*60,inbound)inMax,
					dateadd(MINUTE,dailyhrs/4*60,inbound)outMin,
					case 
							when NoOfShifts=2  then dateadd(MINUTE,2*60,outbound) 
							when NoOfShifts=1  then dateadd(MINUTE,3*60,outbound) 
							else  dateadd(MINUTE,5*60,outbound) 
					end outMax
			from
			(
				select  mon.dateDay,
						FileNumber,
						sh.Id,ShiftName,
						dateadd(day,inoffset,cast(dateDay as  varchar(10))+' '+sh.StartTime)inbound,
						dateadd(day,OutOffset,cast(dateDay as  varchar(10))+' '+sh.EndTime)outbound,
						sh.DailyHrs,
						sh.NoOfShifts
				from month_cte mon,
					 Employees e join 
					 BasicBayWorks b on e.id=b.EmployeeId join 
					 Shifts sh on b.ShiftId=sh.Id or sh.id=(select Shift2 from Shifts where id=b.ShiftId ) or sh.id=(select Shift3 from Shifts where id=b.ShiftId )
				)bound
			where FileNumber  in (select FileNumber from Employees where DepartementId=@dept) 
		)q left join acIn on q.FileNumber=acIn.FileNumber and acin.DateTime between q.inMin and q.inMax 
		   left join acOut on q.FileNumber=acOut.FileNumber and acOut.DateTime between q.outMin and q.outMax 
	)num
	
)final join (select FileNumber,KnownAs'name',Departements.Name'dept' from Employees,Departements where Employees.DepartementId=Departements.Id )e on e.FileNumber=final.FileNumber
where n=1 and not (inf is null and outf is null)
";

                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();
                    SqlCommand com = new SqlCommand(sql, con);
                    com.CommandType = CommandType.Text;
                    com.Parameters.Add("@dept", SqlDbType.Int).Value = dept;
                    com.Parameters.Add("@F", SqlDbType.Date).Value = F.Date;
                    com.Parameters.Add("@T", SqlDbType.Date).Value = T.Date;
                    SqlDataReader rdr = com.ExecuteReader();
                    while (rdr.Read())
                    {
                        lst.Add(new Att
                        {
                            date = rdr["date"].ToString(),
                            name = rdr["name"].ToString(),
                            dept = rdr["dept"].ToString(),
                            fileNumber = rdr["fileNumber"].ToString() == "" ? 0 : int.Parse(rdr["fileNumber"].ToString()),
                            timeFrom = rdr["timeIN"].ToString(),
                            timeTo = rdr["timeOUT"].ToString()
                        });
                    }
                    return lst;
                }
            }
            #endregion
            #region delete
            public int delete(int id)
            {
                string sql =
               @"DELETE FROM Mac WHERE Id=@id";
                try
                {
                    using (SqlConnection con = new SqlConnection(cs))
                    {
                        con.Open();
                        SqlCommand com = new SqlCommand(sql, con);
                        com.CommandType = CommandType.Text;
                        com.Parameters.Add("@id", SqlDbType.Int).Value = id;

                        com.ExecuteNonQuery();
                        return 1;
                    }
                }
                catch (Exception)
                {
                    return 0;
                }
            }
            #endregion
            #region update
            public int update(int id, string MacIP, string MacType, string MacName)
            {
                string sql =
               @"update Mac set 
MacIP=@MacIP,
MacType=@MacType,
MacName=@MacName
where Id=@Id";
                try
                {
                    using (SqlConnection con = new SqlConnection(cs))
                    {
                        con.Open();
                        SqlCommand com = new SqlCommand(sql, con);
                        com.CommandType = CommandType.Text;
                        com.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                        com.Parameters.Add("@MacIP", SqlDbType.VarChar).Value = MacIP;
                        com.Parameters.Add("@MacType", SqlDbType.VarChar).Value = MacType;
                        com.Parameters.Add("@MacName", SqlDbType.VarChar).Value = MacName;
                        com.ExecuteNonQuery();
                        return 1;
                    }
                }
                catch (Exception)
                {

                    return 0;
                }
            }
            #endregion
            #region create
            public int create(string MacIP, string MacType, string MacName)
            {
                string sql =
               @"insert into Mac( 
MacIP,
MacType,
MacName
)
Values(
@MacIP,
@MacType,
@MacName
)
";
                try
                {
                    using (SqlConnection con = new SqlConnection(cs))
                    {
                        con.Open();
                        SqlCommand com = new SqlCommand(sql, con);
                        com.CommandType = CommandType.Text;
                       
                        com.Parameters.Add("@MacIP", SqlDbType.VarChar).Value = MacIP;
                        com.Parameters.Add("@MacType", SqlDbType.VarChar).Value = MacType;
                        com.Parameters.Add("@MacName", SqlDbType.VarChar).Value = MacName;
                        //com.Parameters.Add("@IsFixed", SqlDbType.Int).Value = IsFixed;

                        com.ExecuteNonQuery();
                        return 1;
                    }
                }
                catch (Exception)
                {

                    return 0;
                }
            }
            #endregion
            #region upload
            public int upload(int fn, string dt, string tp, string ip)
            {
                string sql =
               @"
if not exists
(
select *  from AClogs ac 
where ac.FileNumber=@FN and
      ac.DateTime=@dt  and
	  ac.Type=@TP
)
begin
   insert into AClogs(FileNumber,DateTime,Type,IP)values(@FN,@DT,@TP,@IP)
end
";
                try
                {
                    using (SqlConnection con = new SqlConnection(cs))
                    {
                        con.Open();
                        SqlCommand com = new SqlCommand(sql, con);
                        com.CommandType = CommandType.Text;

                        com.Parameters.Add("@FN", SqlDbType.Int).Value = fn;
                        com.Parameters.Add("@DT", SqlDbType.DateTime).Value =dt ;
                        com.Parameters.Add("@TP", SqlDbType.VarChar).Value = tp;
                        com.Parameters.Add("@IP", SqlDbType.VarChar).Value = ip;
                        //com.Parameters.Add("@IsFixed", SqlDbType.Int).Value = IsFixed;

                        com.ExecuteNonQuery();
                        return 1;
                    }
                }
                catch (Exception)
                {

                    return 0;
                }
            }
            #endregion
            #region Attendence ahmed it
            public int ATT_IT(int FN, string DT, string F, string T)
            {
                string sql =
               @"
if not exists(
select * from Attendances A join Employees E on A.EmployeeId=E.Id where A.DateFrom=@dt and  e.FileNumber=@FN
)
begin
insert into  Attendances(
        DateFrom,
	    EmployeeId,
	    TimeFrom,
	    TimeTo,
		AttendanceTypeId
	   )
select
@dt,id,@from,@to,2 from Employees where FileNumber=@FN
end
else
begin
update  A set 
	    A.TimeFrom=@from,
	    A.TimeTo=@to,
		A.AttendanceTypeId=2
from Attendances A join Employees E on E.id=A.EmployeeId  
where FileNumber=@FN and A.DateFrom=@dt
end
";
                try
                {
                    using (SqlConnection con = new SqlConnection(cs))
                    {
                        con.Open();
                        SqlCommand com = new SqlCommand(sql, con);
                        com.CommandType = CommandType.Text;

                        com.Parameters.Add("@FN", SqlDbType.Int).Value = FN;
                        com.Parameters.Add("@dt", SqlDbType.DateTime).Value = DT;
                        com.Parameters.Add("@from", SqlDbType.VarChar).Value = F;
                        com.Parameters.Add("@to", SqlDbType.VarChar).Value = T;
                       
                        com.ExecuteNonQuery();
                        return 1;
                    }
                }
                catch (Exception)
                {

                    return 0;
                }
            }
            #endregion
        }
        #endregion
    }
}
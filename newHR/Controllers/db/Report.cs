using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using newHR.Models;
namespace newHR.Controllers.db
{
    public class Report
    {
        //declare connection string  
        string cs = ConfigurationManager.ConnectionStrings["CS"].ConnectionString;

        #region حضور بالقسم يوم واحد
        public List<Attend> getAttendByDepartment(DateTime from, int dept)
        {
            List<Attend> lst = new List<Attend>();
            string sql = @"
       select ak.date,
       ak.name,
	   ak.FNO'fileNo',
	   TIMEFROM,
	   TIMETO,
	   ad.hrs
       from AK_3STAGES_V3 ak left join(select employeeId,sum(noofhour)hrs,Date from AdditionApprovals aa group by date,EmployeeId) ad on (ad.EmployeeId=ak.ID and ad.Date=AK.date)
       where ak.date=@f and ak.dept=@dept
       ";
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand(sql, con);
                com.CommandType = CommandType.Text;
                com.Parameters.Add("@f", SqlDbType.Date).Value = from.Date;
                com.Parameters.Add("@dept", SqlDbType.Int).Value = dept;
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    lst.Add(new Attend
                    {
                        fileNo = Convert.ToInt32(rdr["fileNo"]),
                        name = rdr["name"].ToString(),
                        Date = rdr["date"].ToString(),
                        timeFrom = rdr["TIMEFROM"].ToString(),
                        timeTo = rdr["TIMETO"].ToString(),
                        addition = rdr["hrs"].ToString() == "" ? 0 : float.Parse(rdr["hrs"].ToString())
                    });
                }
                return lst;
            }
        }
        #endregion
        #region كل القسم يوم واحد
        public List<Attend> getAllByDepartment(DateTime from, int dept)//قاعدة بيانات التقرير المجمع
        {
            List<Attend> lst = new List<Attend>();
            string sql = @"
-- declare @f date='2023-10-26',@dept int=0
	 if(@dept>0)
 begin
	 select e.date,
			e.FileNumber'fileNo',
			e.KnownAs 'name',
			e.deptName,
			e.DepartementId,
            e.color,
			--q.ShiftId,
			q.TIMEFROM,
			q.TIMETO,
			v.DayPart,
			ad.hrs
	 from
	  (
	 select @f date,
			Employees.* ,
			d.Name'deptName'
			from employees left join Personals on employees.PersonalId = Personals.Id  
			left join Departements d on Employees.DepartementId=d.Id
			where StatusId=1 and DepartementId=@dept
	 )e left join
	 (	 
			select EmployeeId,
				    Q.DateFrom date,
					min(Q.TimeFrom)timefrom,
					max(Q.TimeTo)timeto
			from 
				(
					select DateFrom,
							case when TimeFrom='' then NULL else TimeFrom end TimeFrom ,
							case when TimeTo='' then NULL else TimeTo end TimeTo,
							HoursNo,Remarks,AttendanceTypeId,EmployeeId 
					from Attendances 
					where DateFrom=@f
						union
					select DateFrom,
							case when TimeFrom='' then NULL else TimeFrom end TimeFrom ,
							case when TimeTo='' then NULL else TimeTo end TimeTo,
							HoursNo,Remarks,AbsenceTypeId,EmployeeId 
					from Absences 
					where datefrom=@f and  AbsenceTypeId=4
				)Q
				group by EmployeeId,DateFrom
		
	  )q on e.id=q.EmployeeId left join 
	 (
	  select employeeId,sum(noofhour)hrs,Date from AdditionApprovals aa group by date,EmployeeId
	  ) ad on (ad.EmployeeId=e.ID and ad.Date=q.date)
	  left join 
	  (
	   select sum(DayPart) DayPart,EmployeeId,DateFrom from Absences where AbsenceTypeId not in (4,11)
	  group by EmployeeId,DateFrom
	  )v on (v.EmployeeId=e.ID and v.DateFrom=q.date)
 end
  else
  begin
  	 select e.date,
			e.FileNumber'fileNo',
			e.KnownAs 'name',
			e.deptName,
			e.DepartementId,
            e.color,
			--q.ShiftId,
			q.TIMEFROM,
			q.TIMETO,
			v.DayPart,
			ad.hrs
	 from
	 (select @f date, 
			Employees.* ,
			d.Name'deptName'
			from employees left join Personals on employees.PersonalId = Personals.Id  
			left join Departements d on Employees.DepartementId=d.Id
			where StatusId=1 --and DepartementId=@dept
	 )e left join
	 (	 
			select EmployeeId,
				    Q.DateFrom date,
					min(Q.TimeFrom)timefrom,
					max(Q.TimeTo)timeto
			from 
				(
					select DateFrom,
							case when TimeFrom='' then NULL else TimeFrom end TimeFrom ,
							case when TimeTo='' then NULL else TimeTo end TimeTo,
							HoursNo,Remarks,AttendanceTypeId,EmployeeId 
					from Attendances 
					where DateFrom=@f
						union
					select DateFrom,
							case when TimeFrom='' then NULL else TimeFrom end TimeFrom ,
							case when TimeTo='' then NULL else TimeTo end TimeTo,
							HoursNo,Remarks,AbsenceTypeId,EmployeeId 
					from Absences 
					where datefrom=@f and  AbsenceTypeId=4
				)Q
				group by EmployeeId,DateFrom
		
	  )q on e.id=q.EmployeeId left join 
	 (
	  select employeeId,sum(noofhour)hrs,Date from AdditionApprovals aa group by date,EmployeeId
	  ) ad on (ad.EmployeeId=e.ID and ad.Date=q.date)
	  left join 
	  (
	   select sum(DayPart) DayPart,EmployeeId,DateFrom from Absences where AbsenceTypeId not in (4,11)
	  group by EmployeeId,DateFrom
	  )v on (v.EmployeeId=e.ID and v.DateFrom=q.date)
  end
       ";
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand(sql, con);
                com.CommandType = CommandType.Text;
                com.Parameters.Add("@f", SqlDbType.Date).Value = from.Date;
                com.Parameters.Add("@dept", SqlDbType.Int).Value = dept;
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    lst.Add(new Attend
                    {
                        fileNo = Convert.ToInt32(rdr["fileNo"]),
                        name = rdr["name"].ToString(),
                        Date = rdr["date"].ToString().Substring(0,10),
                        color=rdr["color"].ToString(),
                        //shiftId = rdr["shiftId"].ToString(),
                        deptName = rdr["deptName"].ToString(),
                        timeFrom = rdr["TIMEFROM"].ToString(),
                        timeTo = rdr["TIMETO"].ToString(),
                        addition = rdr["hrs"].ToString() == "" ? 0 : float.Parse(rdr["hrs"].ToString()),
                        vaction= rdr["DayPart"].ToString() == "" ? 0 : float.Parse(rdr["DayPart"].ToString())
                    });
                }
                return lst;
            }
        }
        public int createVacation(DateTime from, int fileNo, float dayPart,int type)
        {
            string sql =
           @" INSERT INTO Absences(DateFrom,EmployeeId ,DayPart,AbsenceTypeId,Payment)
             SELECT @d,Id,@DayPart,@ABT,'1' FROM Employees e WHERE e.FileNumber=@FN";
            try
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();
                    SqlCommand com = new SqlCommand(sql, con);
                    com.CommandType = CommandType.Text;
                    com.Parameters.Add("@d", SqlDbType.Date).Value = from.Date;
                    com.Parameters.Add("@DayPart", SqlDbType.Float).Value = dayPart;
                    com.Parameters.Add("@FN", SqlDbType.Int).Value = fileNo;
                    com.Parameters.Add("@ABT", SqlDbType.Int).Value = type;

                    com.ExecuteNonQuery();
                    return 1;
                }
            }
            catch (Exception)
            {

                return 0;
            }
        }
        //)
        public int deleteVacation(DateTime from, int fileNo)
        {
            string sql =
           @"delete  from Absences where DateFrom=@d and EmployeeId=(select id from Employees where FileNumber=@fn)";
            try
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();
                    SqlCommand com = new SqlCommand(sql, con);
                    com.CommandType = CommandType.Text;
                    com.Parameters.Add("@d", SqlDbType.Date).Value = from.Date;
                    com.Parameters.Add("@FN", SqlDbType.Int).Value = fileNo;                   

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
}
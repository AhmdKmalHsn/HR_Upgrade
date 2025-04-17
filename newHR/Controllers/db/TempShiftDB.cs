using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace newHR.Controllers.db
{
    public class tempShift {
        public int Id { set; get; }
        public int ShiftId { set; get; }
        public string DateFrom { set; get; }
        public string DateTo { set; get; }
        public string ShiftFrom { set; get; }
        public string ShiftTo { set; get; }
        public float ShiftHours { set; get; }
        public int DepartmentId { set; get; }
        public bool IsPrivate { set; get; }
    }
    public class EmployeeTempShift
    {
        public int FileNumber { set; get; }
        public int TempShiftId { set; get; }
        public string Department { set; get; }
        public bool IsPrivate  { set; get; }
        public string Name { set; get; }
    }
    public class TempShiftDB
    {

        string cs = ConfigurationManager.ConnectionStrings["CS"].ConnectionString;
        //crud
        #region read
        public List<tempShift> getTempShift(int id)
        {
            List<tempShift> lst = new List<tempShift>();
            string sql = @"
       select 
Id,
ShiftId,
DateFrom,
DateTo,
ShiftFrom,
ShiftTo,
ShiftHours,
DepartmentId,
IsPrivate
from 
TempShifts
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
                    lst.Add(new tempShift
                    {
                        Id= rdr["Id"].ToString() == "" ? 0 : int.Parse(rdr["Id"].ToString()),
                        ShiftId = rdr["ShiftId"].ToString() == "" ? 0 : int.Parse(rdr["ShiftId"].ToString()),
                        DateFrom = rdr["DateFrom"].ToString(),
                        DateTo = rdr["DateTo"].ToString(),
                        ShiftFrom = rdr["ShiftFrom"].ToString(),
                        ShiftTo = rdr["ShiftTo"].ToString(),
                        ShiftHours = rdr["ShiftHours"].ToString() == "" ? 0 : float.Parse(rdr["ShiftHours"].ToString()),
                        DepartmentId = rdr["DepartmentId"].ToString() == "" ? 0 : int.Parse(rdr["DepartmentId"].ToString()),
                        IsPrivate= rdr["IsPrivate"].ToString() == "0" ? false : true
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
           @"DELETE FROM TempShifts WHERE Id=@id";
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
        public int update( int id, int ShiftId, DateTime DateFrom , DateTime DateTo , string ShiftFrom, string ShiftTo, float ShiftHours , int DepartmentId,bool IsPrivate)
        {
            string sql =
           @"update TempShifts set 
ShiftId=@ShiftId,
DateFrom=@DateFrom,
DateTo=@DateTo,
ShiftFrom=@ShiftFrom,
ShiftTo=@ShiftTo,
ShiftHours=@ShiftHours,
DepartmentId=@DepartmentId,
IsPrivate=@IsPrivate
where Id=@Id";
            try
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();
                    SqlCommand com = new SqlCommand(sql, con);
                    com.CommandType = CommandType.Text;
                    com.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                    com.Parameters.Add("@shiftId", SqlDbType.Int).Value = ShiftId;
                    com.Parameters.Add("@dateFrom", SqlDbType.DateTime).Value = DateFrom.Date;
                    com.Parameters.Add("@dateTo", SqlDbType.DateTime).Value = DateTo.Date;
                    com.Parameters.Add("@shiftFrom", SqlDbType.VarChar).Value = ShiftFrom;
                    com.Parameters.Add("@shiftTo", SqlDbType.VarChar).Value = ShiftTo;
                    com.Parameters.Add("@shiftHours", SqlDbType.Float).Value = ShiftHours;
                    com.Parameters.Add("@departmentId", SqlDbType.Int).Value = DepartmentId;
                    com.Parameters.Add("@isPrivate", SqlDbType.Bit).Value = IsPrivate;

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
        public int create(int ShiftId, DateTime DateFrom, DateTime DateTo, string ShiftFrom, string ShiftTo, float ShiftHours, int DepartmentId, bool IsPrivate)
        {
            string sql =
           @"insert into TempShifts( 
ShiftId ,
DateFrom ,
DateTo ,
ShiftFrom ,
ShiftTo ,
ShiftHours ,
DepartmentId ,
IsPrivate
)
Values(
 @ShiftId,
 @DateFrom,
 @DateTo,
 @ShiftFrom,
 @ShiftTo,
 @ShiftHours,
 @DepartmentId,
 @IsPrivate
)
";
            try
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();
                    SqlCommand com = new SqlCommand(sql, con);
                    com.CommandType = CommandType.Text;
                    com.Parameters.Add("@shiftId", SqlDbType.Int).Value = ShiftId;
                    com.Parameters.Add("@dateFrom", SqlDbType.DateTime).Value = DateFrom.Date;
                    com.Parameters.Add("@dateTo", SqlDbType.DateTime).Value = DateTo.Date;
                    com.Parameters.Add("@shiftFrom", SqlDbType.VarChar).Value = ShiftFrom;
                    com.Parameters.Add("@shiftTo", SqlDbType.VarChar).Value = ShiftTo;
                    com.Parameters.Add("@shiftHours", SqlDbType.Float).Value = ShiftHours;
                    com.Parameters.Add("@departmentId", SqlDbType.Int).Value = DepartmentId;
                    com.Parameters.Add("@isPrivate", SqlDbType.Bit).Value = IsPrivate;

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
        
        #region getAllEmployee
        public List<EmployeeTempShift> getAllTempShift(int DeptId)
        {
            List<EmployeeTempShift> lst = new List<EmployeeTempShift>();
            string sql =
@"if(@DeptId=0)
begin
select
       e.FileNumber
      ,e.KnownAs'name'
	  ,d.Name'Department'
	  ,b.IsPrivate
	  ,b.TempShiftId
from Employees e left join BasicBayWorks b on e.Id = b.EmployeeId
                 left join Departements d on d.Id = e.DepartementId
                 left join Personals p on e.PersonalId = p.Id
                 left join Status on Status.Id = p.StatusId
where p.StatusId <> 3 
end
else 
begin
select
       e.FileNumber
      ,e.KnownAs'name'
	  ,d.Name'Department'
	  ,b.IsPrivate
	  ,b.TempShiftId
from Employees e left join BasicBayWorks b on e.Id = b.EmployeeId
                 left join Departements d on d.Id = e.DepartementId
                 left join Personals p on e.PersonalId = p.Id
                 left join Status on Status.Id = p.StatusId
where p.StatusId <> 3 and e.DepartementId=@DeptId
end";

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand(sql, con);
                com.CommandType = CommandType.Text;
                com.Parameters.Add("@DeptId", SqlDbType.Int).Value = DeptId;

                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    lst.Add(new EmployeeTempShift
                    {
                        FileNumber = rdr["FileNumber"].ToString() == "" ? 0 : int.Parse(rdr["FileNumber"].ToString()),
                        TempShiftId = rdr["TempShiftId"].ToString() == "" ? 0 : int.Parse(rdr["TempShiftId"].ToString()),
                        Name = rdr["Name"].ToString(),
                        Department = rdr["Department"].ToString(),
                        IsPrivate = rdr["IsPrivate"].ToString()==""?false:bool.Parse(rdr["IsPrivate"].ToString())
                    });
                }
                return lst;
            }
        }
        #endregion
    }
}
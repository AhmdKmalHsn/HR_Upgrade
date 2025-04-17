using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace newHR.Controllers.db
{
    public class additionDB
    {
        //declare connection string  
        string cs = ConfigurationManager.ConnectionStrings["CS"].ConnectionString;

        #region حضور بالقسم يوم واحد
        public int create(DateTime from, int fileNo,float hoursNo,int offset)
        {
            string sql = 
           @"INSERT INTO AdditionApprovals(NoOfHour,Date,EmployeeId ,TimeOf)
             SELECT @HoursNo,@d,Id,@offset FROM Employees e WHERE e.FileNumber=@FN";
            try
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();
                    SqlCommand com = new SqlCommand(sql, con);
                    com.CommandType = CommandType.Text;
                    com.Parameters.Add("@d", SqlDbType.Date).Value = from.Date;
                    com.Parameters.Add("@HoursNo", SqlDbType.Float).Value = hoursNo;
                    com.Parameters.Add("@offset", SqlDbType.Int).Value = offset;
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
        public int deleteAddition(DateTime from, int fileNo)
        {
            string sql =
           @"delete  from AdditionApprovals where Date=@d and EmployeeId=(select id from Employees where FileNumber=@fn)";
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
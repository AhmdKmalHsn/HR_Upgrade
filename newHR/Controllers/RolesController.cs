using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace newHR.Controllers
{
    public class RolesController : Controller
    {
        // GET: Roles
        public ActionResult Index()
        {
            return View();
        }       
        public ActionResult UsersIndex()
        {
            return View();
        }
       
        /**************************db user**********************/
        DBContext db = new DBContext();
        public JsonResult getUsers(int id=0)
        {
            if (id == 0) { 
            string sql = @"
	        select u.Id,	u.UserName,	'****'password,	u.Name,	r.name 'role'	
            from Users2 u left join Roles2 r on r.Id=u.RoleId";
            return Json(db.toJSON(db.getData(sql)), JsonRequestBehavior.AllowGet);
            }else { 
            string sql = @"
	        select u.Id,	u.UserName,	''password,	u.Name,u.RoleId
            from Users2 u where u.Id=@id";
            SqlCommand cmd = new SqlCommand(sql);
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
            return Json(db.toJSON(db.getData(cmd)), JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult createUser(string UserName, string password, string Name, int RoleId)
        {
            string sql = @"
	            insert into Users2(UserName,password,Name,RoleId)
                values(@UserName,@password,@Name,@RoleId)";
            SqlCommand cmd = new SqlCommand(sql);
            cmd.Parameters.Add("@UserName", SqlDbType.NVarChar).Value = UserName;
            cmd.Parameters.Add("@password", SqlDbType.NVarChar).Value = password;
            cmd.Parameters.Add("@Name", SqlDbType.NVarChar).Value = Name;
            cmd.Parameters.Add("@RoleId", SqlDbType.Int).Value = RoleId;
            return Json(db.exec(cmd), JsonRequestBehavior.AllowGet);
        }
        public JsonResult updateUser(int Id,string UserName, string password, string Name, int RoleId)
        {
            string sql = @"
	            update Users2 set
                UserName=@UserName,password=@password,Name=@Name,RoleId=@RoleId
                where id=@id";
            SqlCommand cmd = new SqlCommand(sql);
            cmd.Parameters.Add("@UserName", SqlDbType.NVarChar).Value = UserName;
            cmd.Parameters.Add("@password", SqlDbType.NVarChar).Value = password;
            cmd.Parameters.Add("@Name", SqlDbType.NVarChar).Value = Name;
            cmd.Parameters.Add("@RoleId", SqlDbType.Int).Value = RoleId;
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = Id;
            return Json(db.exec(cmd), JsonRequestBehavior.AllowGet);
        }
        /**************************db roles*************************/
        public JsonResult getRoles(int id = 0)
        {
            if (id == 0)
            {
                string sql = @"
	        select 
r.Id
,r.name,
(select name from Accessibility where id=Vacation)Vacation,
(select name from Accessibility where id=Mission) Mission,
(select name from Accessibility where id=Attendance) Attendance,
(select name from Accessibility where id=OverTime) OverTime,
(select name from Accessibility where id=Salary) Salary,
(select name from Accessibility where id=ClosingPeriod) ClosingPeriod,
(select name from Accessibility where id=FingerMachine) FingerMachine,
(select name from Accessibility where id=Reports_Abs) Reports_Abs,
(select name from Accessibility where id=Reports_Salary_Details) Reports_Salary_Details,
(select name from Accessibility where id=Logs) Logs,
(select name from Accessibility where id=TempShifts) TempShifts
from  Roles2 r";
                return Json(db.toJSON(db.getData(sql)), JsonRequestBehavior.AllowGet);
            }
            else
            {
                string sql = @"
	        select Id,
name,
Vacation,
Mission,
Attendance,
OverTime,
Salary,
ClosingPeriod,
FingerMachine,
Reports_Abs,
Reports_Salary_Details,
Logs,
TempShifts
from  Roles2  where Id=@id";
                SqlCommand cmd = new SqlCommand(sql);
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                return Json(db.toJSON(db.getData(cmd)), JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult createRole(string  name, int Vacation, int Mission, int Attendance, int OverTime, int Salary, int ClosingPeriod, int FingerMachine, int Reports_Abs, int Reports_Salary_Details, int Logs, int TempShifts)
        {
            string sql = @"
	            insert into Roles2(name,Vacation,Mission,Attendance,OverTime,Salary,ClosingPeriod,FingerMachine,Reports_Abs,Reports_Salary_Details,Logs,TempShifts)
                values(@Id,@name,@Vacation,@Mission,@Attendance,@OverTime,@Salary,@ClosingPeriod,@FingerMachine,@Reports_Abs,@Reports_Salary_Details,@Logs,@TempShifts)";
            SqlCommand cmd = new SqlCommand(sql);
            //cmd.Parameters.Add("@Id ", SqlDbType.Int).Value =Id;
            cmd.Parameters.Add("@name", SqlDbType.NVarChar).Value = name;
            cmd.Parameters.Add("@Vacation", SqlDbType.Int).Value = Vacation;
            cmd.Parameters.Add("@Mission", SqlDbType.Int).Value = Mission;
            cmd.Parameters.Add("@Attendance", SqlDbType.Int).Value = Attendance;
            cmd.Parameters.Add("@OverTime", SqlDbType.Int).Value = OverTime;
            cmd.Parameters.Add("@Salary", SqlDbType.Int).Value = Salary;
            cmd.Parameters.Add("@ClosingPeriod", SqlDbType.Int).Value = ClosingPeriod;
            cmd.Parameters.Add("@FingerMachine", SqlDbType.Int).Value = FingerMachine;
            cmd.Parameters.Add("@Reports_Abs", SqlDbType.Int).Value = Reports_Abs;
            cmd.Parameters.Add("@Reports_Salary_Details", SqlDbType.Int).Value = Reports_Salary_Details;
            cmd.Parameters.Add("@Logs", SqlDbType.Int).Value = Logs;
            cmd.Parameters.Add("@TempShifts", SqlDbType.Int).Value = TempShifts;
            

            return Json(db.exec(cmd), JsonRequestBehavior.AllowGet);
        }
        public JsonResult updateRole(int Id,string name, int Vacation, int Mission, int Attendance, int OverTime, int Salary, int ClosingPeriod, int FingerMachine, int Reports_Abs, int Reports_Salary_Details, int Logs, int TempShifts)
        {
            
            string sql = @"
	            update Roles2 set 
name=@name,
Vacation=@Vacation,
Mission=@Mission,
Attendance=@Attendance,
OverTime= @OverTime,
Salary=@Salary,
ClosingPeriod=@ClosingPeriod,
FingerMachine=@FingerMachine,
Reports_Abs=@Reports_Abs,
Reports_Salary_Details=@Reports_Salary_Details,
Logs=@Logs,
TempShifts=@TempShifts
where id=@Id        ";
            SqlCommand cmd = new SqlCommand(sql);
            cmd.Parameters.Add("@Id ", SqlDbType.Int).Value =Id;
            cmd.Parameters.Add("@name", SqlDbType.NVarChar).Value = name;
            cmd.Parameters.Add("@Vacation", SqlDbType.Int).Value = Vacation;
            cmd.Parameters.Add("@Mission", SqlDbType.Int).Value = Mission;
            cmd.Parameters.Add("@Attendance", SqlDbType.Int).Value = Attendance;
            cmd.Parameters.Add("@OverTime", SqlDbType.Int).Value = OverTime;
            cmd.Parameters.Add("@Salary", SqlDbType.Int).Value = Salary;
            cmd.Parameters.Add("@ClosingPeriod", SqlDbType.Int).Value = ClosingPeriod;
            cmd.Parameters.Add("@FingerMachine", SqlDbType.Int).Value = FingerMachine;
            cmd.Parameters.Add("@Reports_Abs", SqlDbType.Int).Value = Reports_Abs;
            cmd.Parameters.Add("@Reports_Salary_Details", SqlDbType.Int).Value = Reports_Salary_Details;
            cmd.Parameters.Add("@Logs", SqlDbType.Int).Value = Logs;
            cmd.Parameters.Add("@TempShifts", SqlDbType.Int).Value = TempShifts;


            return Json(db.exec(cmd), JsonRequestBehavior.AllowGet);
        }


    }
}